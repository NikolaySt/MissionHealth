using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace SocialNet.Backend.Security
{
	public static class SecurityContext
	{
		public static string TokenId
		{
			get
			{
				var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.TokenId);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static string AppRealmId
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.AppRealmId);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static string AppId
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.AppId);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static string UserId
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.UserId);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static string UserName
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.UserName);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static string[] Roles
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.Role);

				if (claim == null) return new string[0];

				return claim.Value.Split(',');
			}
		}

		public static string CustomData
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.CustomData);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static string SessionId
		{
			get
			{
				var principal = GetPrincipal();
				if (principal == null) throw new UnauthorizedAccessException();

				var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.SessionId);
				if (claim == null) return null;

				return claim.Value;
			}
		}

		public static void SetSecurityContext(IEnumerable<Claim> claimsIdentity)
		{
			// TODO: Figure out how to configure the hardcoded "JWT" value

			var identity = new ClaimsIdentity(claimsIdentity, "JWT");

			var principal = new ClaimsPrincipal(identity);

			SetPrincipal(principal);
		}

		static ClaimsPrincipal GetPrincipal()
		{
			if (HttpContext.Current != null) return HttpContext.Current.User as ClaimsPrincipal;

			return Thread.CurrentPrincipal as ClaimsPrincipal;
		}

		static void SetPrincipal(ClaimsPrincipal principal)
		{
			if (HttpContext.Current != null) HttpContext.Current.User = principal;

			Thread.CurrentPrincipal = principal;
		}
	}
}

