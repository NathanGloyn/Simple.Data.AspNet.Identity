using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    public class UserStore<TUser>:IQueryableUserStore<TUser>, IUserRoleStore<TUser> where TUser: IdentityUser {
        
        private readonly UserTable _userTable;

        public UserStore() {
            dynamic database = Database.Open();
            _userTable = new UserTable(database);
        }        
        
        public void Dispose() {
            throw new System.NotImplementedException();
        }

        public Task CreateAsync(TUser user) {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _userTable.Insert(user);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(TUser user) {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(user.Id)) 
            {
                throw new ArgumentException("Missing Id","user");    
            }

            _userTable.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TUser user) {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                throw new ArgumentException("Missing user Id");
            }

            _userTable.Delete(user.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TUser> FindByIdAsync(string userId) {
            var result = _userTable.GetUserById(userId) as TUser;

            return Task.FromResult(result);            
        }

        public Task<TUser> FindByNameAsync(string userName) {
            var result = _userTable.GetUserByName(userName) as TUser;

            return Task.FromResult(result);
        }

        public IQueryable<TUser> Users {
            get { return _userTable.AllUsers<TUser>().AsQueryable(); }
        }

        public Task AddToRoleAsync(TUser user, string roleName) {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName) {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TUser user) {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName) {
            throw new NotImplementedException();
        }
    }
}