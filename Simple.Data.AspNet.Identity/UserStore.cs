using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    public class UserStore<TUser>:IQueryableUserStore<TUser>, IUserStore<TUser>, IUserRoleStore<TUser>, IUserPasswordStore<TUser> where TUser: IdentityUser {
        
        private UserTable _userTable;
        private UserTable UsersTable
        {
            get { return _userTable ?? (_userTable = new UserTable(_database, Tables)); }
        }

        private RoleTable _roleTable;
        public RoleTable RolesTable
        {
            get { return _roleTable ?? (_roleTable = new RoleTable(_database, Tables)); }
        }

        private UserRoleTable _userRoleTable;
        public UserRoleTable UserRolesTable
        {
            get { return _userRoleTable ?? (_userRoleTable = new UserRoleTable(_database, Tables)); }
        }

        private readonly dynamic _database ;
        


        public Tables Tables { get; set; }


        public UserStore()
        {
            _database = Database.Open();
            Tables = new Tables();
        }

        public UserStore(Tables tables)
        {
            Tables = tables;
            _database = Database.Open();       
        }

        public UserStore(string connectionName)
        {
           _database = Database.OpenNamedConnection(connectionName);
           Tables = new Tables();
        }

        public UserStore(string connectionName, Tables tables)
        {
            _database = Database.OpenNamedConnection(connectionName);
            Tables = tables;
        }

        public void Dispose() {
            // we can let normal GC behaviour to clean up
        }

        public Task CreateAsync(TUser user) {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UsersTable.Insert(user);

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

            UsersTable.Update(user);

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

            UsersTable.Delete(user.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TUser> FindByIdAsync(string userId) {
            var result = UsersTable.GetUserById(userId) as TUser;

            return Task.FromResult(result);            
        }

        public Task<TUser> FindByNameAsync(string userName) {
            var result = UsersTable.GetUserByName(userName) as TUser;

            return Task.FromResult(result);
        }

        public IQueryable<TUser> Users {
            get { return UsersTable.AllUsers<TUser>().AsQueryable(); }
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            string roleId = RolesTable.GetRoleId(roleName);

            if (string.IsNullOrEmpty(roleId)) 
            {
                throw new ArgumentException("Unknown role: " + roleName);    
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                UserRolesTable.Insert(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName) 
        {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            string roleId = RolesTable.GetRoleId(roleName);

            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentException("Unknown role: " + roleName);
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                UserRolesTable.Delete(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(TUser user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            IList<string> roleNames = UserRolesTable.FindByUserId(user.Id)
                                                    .Select(role => role.Name)
                                                    .ToList();

            return Task.FromResult(roleNames);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName)) {
                throw new ArgumentNullException("roleName");
            }

            var userRoles = UserRolesTable.FindByUserId(user.Id);

            return Task.FromResult(userRoles.Any(x => x.Name == roleName));

        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash) 
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Invalid password hash value","passwordHash");
            }

            user.PasswordHash = passwordHash;

            UsersTable.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            string passwordHash = UsersTable.GetPasswordHash(user);

            return Task.FromResult(passwordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user) {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = !string.IsNullOrEmpty(UsersTable.GetPasswordHash(user));

            return Task.FromResult(result);
        }
    }
}