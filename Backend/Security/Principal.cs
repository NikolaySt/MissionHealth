using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SocialNet.Backend.Security
{
	public class Principal : IPrincipal
	{
		private readonly GenericIdentity _identity;
		private readonly string[] _roles;
		private readonly Dictionary<string, object> _context;

		public Principal(GenericIdentity identity, string[] roles)
		{
			if (identity == null) throw new ArgumentException("identity");
			if (roles == null) throw new ArgumentException("roles");

			_identity = identity;
			_roles = roles;
			
			_context = new Dictionary<string, object>();
		}

		public IIdentity Identity
		{
			get { return _identity; }
		}

		public bool IsInRole(string role)
		{
			if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("role");

			return _roles.Any(item => item.ToLower().Equals(role.ToLower()));
		}

		public Dictionary<string, object> Context
		{
			get { return _context; }
		}
	}
}
