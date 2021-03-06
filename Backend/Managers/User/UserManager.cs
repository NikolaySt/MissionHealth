using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;
using SocialNet.Backend.Exceptions;
using SocialNet.Backend.Helpers;
using SocialNet.Backend.Security;
using SocialNet.Backend.Database;

namespace SocialNet.Backend.Managers
{
    public partial class UserManager
    {

        private UserCatalog _userCatalog;
        private UserCatalog UserCatalog
        {
            get
            {
                if (_userCatalog == null)
                {
                    _userCatalog = new UserCatalog();
                }

                return _userCatalog;
            }
        }
        
        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) throw new ArgumentException("user");

            if (user.IsNew) throw new InvalidOperationException();

            user = await UserCatalog.StoreAsync(user, cancellationToken);

            return user;
        }
        
        public async Task<User> GetAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("userId");

            var user = await UserCatalog.GetAsync(id, cancellationToken);

            if (user == null) return null;

            return user;
        }

		public async Task<User> GetAsync(UserItemFilter filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (filter == null) throw new ArgumentNullException("filter");

            var user = await UserCatalog.GetAsync(filter, cancellationToken);
            if (user == null) return null;

            return user;
        }

        public async Task<PagedResult<User>> GetAsync(
            UserListFilter filter,
            int pageNumber,
            int pageSize,
            string sortOrder = "Created DESC",
            List<string> include = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (filter == null) throw new ArgumentException("filter");

            if (pageNumber < 0) throw new ArgumentException("pageNumber");

            if (pageSize < 0) throw new ArgumentException("pageSize");

            var result = await UserCatalog.GetPageAsync(filter, pageNumber, pageSize, sortOrder, cancellationToken);

            return result;
        }
        
    }
}
