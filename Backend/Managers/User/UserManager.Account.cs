using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;

using SocialNet.Backend.Helpers;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;
using SocialNet.Backend.Exceptions;
using SocialNet.Backend.Security;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.Configuration;

namespace SocialNet.Backend.Managers
{
    public partial class UserManager
    {
        private static readonly TimeSpan TokenExpirationPeriod = TimeSpan.FromDays(365);

        private static readonly object _initLock = new object();
        private static List<Application> Applications;

		private UserCredentialCatalog _userCredentialCatalog;
		private UserCredentialCatalog UserCredentialCatalog
		{
			get
			{
				if (_userCredentialCatalog == null)
				{
					_userCredentialCatalog = new UserCredentialCatalog();
				}

				return _userCredentialCatalog;
			}
		}

		private UserRolesCatalog _userRolesCatalog;
		private UserRolesCatalog UserRolesCatalog
		{
			get
			{
				if (_userRolesCatalog == null)
				{
					_userRolesCatalog = new UserRolesCatalog();
				}

				return _userRolesCatalog;
			}
		}

		public  async Task<string> LogInAsync(
			string email,
			string password,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");

			var filter = new UserItemFilter { Email = email };
			var user = await UserCatalog.GetAsync(filter, cancellationToken);
			if (user == null) throw new UnauthorizedAccessException();

			var credentialsFilter = new UserCredentialItemFilter() { UserId = user.Id };

			var credentials = await UserCredentialCatalog.GetAsync(credentialsFilter, cancellationToken);
			if (credentials == null) throw new UnauthorizedAccessException();

			var passwordProcessor = await PasswordProcessorFactory.GetInstance();

			var result = passwordProcessor.Validate(password, credentials.Value);
			if (!result) throw new UnauthorizedAccessException();

			var userRoles = await UserRolesCatalog.GetAsync(new UserRolesItemFilter() { UserId = user.Id});

			var userName = string.Concat(user.FirstName, " ", user.LastName);

            var claims = GetClaims(user.Id, userName, userRoles != null ? userRoles.Roles : null);

			var tokenIssuer = await TokenIssuerFactory.GetInstance();

			return tokenIssuer.GetToken(claims);
		}

		public  async Task<string> SignUpAsync(
			User user,
			string password,
			string [] roles = null,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (user == null) throw new ArgumentException("user");

			if (roles == null || roles.Length == 0)
			{
				roles = new string[] { Roles.EndUser };
            }

			string contextAppId = ""; //get from security context

			user.AppId = contextAppId;
			if (user.Email != null)
			{
				user.Email = user.Email.ToLower();

				var existing = await UserCatalog.GetAsync(new UserItemFilter(){Email = user.Email }, cancellationToken);

				if (existing != null) throw new IdentifierTakenException("The email is taken");
			}

			var hashPassword = "";
			if (!string.IsNullOrWhiteSpace(password))
			{
				var passwordProcessor = await PasswordProcessorFactory.GetInstance();
				hashPassword = passwordProcessor.Process(password);
			}

			user = await UserCatalog.StoreAsync(user, cancellationToken);

			if (!string.IsNullOrWhiteSpace(hashPassword))
			{
				var credentials = new UserCredential
				{
					AppId = contextAppId,
					Type = UserCredentialType.Password,
					UserId = user.Id,
					Status = UserCredentialStatus.Current,
					Value = hashPassword
				};

				await UserCredentialCatalog.StoreAsync(credentials, cancellationToken);
			}

			var userRoles = new UserRoles()
			{
				UserId = user.Id,
				AppId = contextAppId,
				Roles = roles
			};

			await UserRolesCatalog.StoreAsync(userRoles);

			var userName = string.Concat(user.FirstName, " ", user.LastName);

			var claims = GetClaims(user.Id, userName, roles);

			var tokenIssuer = await TokenIssuerFactory.GetInstance();

			return tokenIssuer.GetToken(claims);
		}

		public  async Task SendConfirmEmailAsync(
			string email,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

			var user = await UserCatalog.GetAsync(new UserItemFilter() { Email = email}, cancellationToken);
			if (user == null) throw new UnauthorizedAccessException("User does not exist");

			var builder = new StringBuilder(SystemSettings.ConfirmEmailFeature.MessageBody);

			if (user.FirstName != null || user.LastName != null)
			{
				var name = string.Format("{0} {1}", user.FirstName ?? "", user.LastName ?? "");
				name = name.Trim();
				builder.Replace("[NAME]", name);
			}

			var claims = GetClaims(user.Id).ToList();
			claims.Add(new Claim(Claims.CustomData, CustomData.EmailConfirmation));
			var tokenIssuer = await TokenIssuerFactory.GetInstance(TimeSpan.FromSeconds(SystemSettings.ConfirmEmailFeature.ConfirmationLinkLifeTime));
			var token = tokenIssuer.GetToken(claims);
			builder.Replace("[TOKEN]", token);

			var mailClient = MailClientFactory.GetConfrimEmailInstance();

			await mailClient.SendAsync(
				SystemSettings.ConfirmEmailFeature.MessageFrom,
				user.Email,
				SystemSettings.ConfirmEmailFeature.MessageSubject,
				builder.ToString(),
				SystemSettings.ConfirmEmailFeature.IsMessageBodyHtml,
				Encoding.UTF8);
		}

		public  async Task ConfirmEmailAsync(
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(SecurityContext.UserId)) throw new UnauthorizedAccessException("Invalid solution");

			if (string.IsNullOrWhiteSpace(SecurityContext.CustomData)) throw new UnauthorizedAccessException("Invalid solution");

			var user = await UserCatalog.GetAsync(SecurityContext.UserId, cancellationToken);

			if (user == null) throw new UnauthorizedAccessException("User does not exist");

			await UserCatalog.UpdateAsync(user.Id, new Dictionary<string, object> { { "EmailStatus", UserEmailStatus.Confirmed } }, cancellationToken);
		}

		public  async Task ResetPasswordAsync(
			string email,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

			var user = await UserCatalog.GetAsync(new UserItemFilter() { Email = email }, cancellationToken);
			if (user == null) throw new UnauthorizedAccessException("User does not exist");
			
			var passwordResetSettings = SystemSettings.ResetPasswordFeature;
			if (passwordResetSettings == null) throw new ApplicationException("Password reset feature not configured");

			var builder = new StringBuilder(passwordResetSettings.MessageBody);

			if (user.FirstName != null || user.LastName != null)
			{
				var name = String.Format("{0} {1}", user.FirstName ?? "", user.LastName ?? "");
				name = name.Trim();
				builder.Replace("[NAME]", name);
			}

			var claims = GetClaims(user.Id).ToList();
			claims.Add(new Claim(Claims.CustomData, CustomData.ResetPassword));
			var tokenIssuer = await TokenIssuerFactory.GetInstance(TimeSpan.FromSeconds(passwordResetSettings.ResetLinkLifeTime));
			var token = tokenIssuer.GetToken(claims);
			builder.Replace("[TOKEN]", token);

			var mailClient = MailClientFactory.GetResetPasswordInstance();

			await mailClient.SendAsync(
				passwordResetSettings.MessageFrom,
				user.Email,
				passwordResetSettings.MessageSubject,
				builder.ToString(),
				passwordResetSettings.IsMessageBodyHtml,
				Encoding.UTF8);
		}

		public  async Task ChangePasswordAsync(
			string @new,
			string current = null,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(@new)) throw new ArgumentException("@new");

			var contextUserId = SecurityContext.UserId;

			var filter = new UserCredentialItemFilter
				{
					UserId = SecurityContext.UserId,
					Type = UserCredentialType.Password,
					Status = UserCredentialStatus.Current
				};

			var credentials = await UserCredentialCatalog.GetAsync(filter, cancellationToken);

			var passwordProcessor = await PasswordProcessorFactory.GetInstance();

			if (!string.IsNullOrWhiteSpace(current) && credentials != null)
			{
				var match = passwordProcessor.Validate(current, credentials.Value);
				if (!match) throw new UnauthorizedAccessException("Invalid password");
			}

			if (credentials != null)
			{
				credentials.Status = UserCredentialStatus.Old;
				await UserCredentialCatalog.StoreAsync(credentials, cancellationToken);
			}

			credentials = new UserCredential
			{
				UserId = contextUserId,
				Type = UserCredentialType.Password,
				Status = UserCredentialStatus.Current,
				Value = passwordProcessor.Process(@new)
			};

			await UserCredentialCatalog.StoreAsync(credentials, cancellationToken);
		}

		protected  IEnumerable<Claim> GetClaims(string userId, string userName = null, string[] roles = null)
		{
			var claims = new List<Claim>();

			var claim = new Claim(Claims.AppRealmId, SecurityContext.AppRealmId);
			claims.Add(claim);

			claim = new Claim(Claims.AppId, SecurityContext.AppId);
			claims.Add(claim);

			claim = new Claim(Claims.UserId, userId);
			claims.Add(claim);

			if (!string.IsNullOrWhiteSpace(userName))
			{
				claim = new Claim(Claims.UserName, userName);
				claims.Add(claim);
			}

			if (roles != null && roles.Any())
			{
				claim = new Claim(Claims.Role, string.Join(",", roles));
				claims.Add(claim);
			}

			return claims;
		}
	}
}
