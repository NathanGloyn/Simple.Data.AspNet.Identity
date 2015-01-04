﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    public class UserStore<TUser>:IQueryableUserStore<TUser>, IUserStore<TUser>, IUserRoleStore<TUser>, IUserPasswordStore<TUser> where TUser: IdentityUser {

        private UserTable _userTable;
        private RoleTable _roleTable;
        private UserRoleTable _userRoleTable;

        public UserStore()
        {
            dynamic database = Database.Open();
            Init(database);
        }


        public UserStore(string connectionName)
        {
            dynamic database = Database.OpenNamedConnection(connectionName);
            Init(database);
        }


        private void Init(dynamic database)
        {
            _userTable = new UserTable(database);
            _roleTable = new RoleTable(database);
            _userRoleTable = new UserRoleTable(database);
        }
        
        public void Dispose() {
            // we can let normal GC behaviour to clean up
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

            string roleId = _roleTable.GetRoleId(roleName);

            if (string.IsNullOrEmpty(roleId)) 
            {
                throw new ArgumentException("Unknown role: " + roleName);    
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                _userRoleTable.Insert(user, roleId);
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

            string roleId = _roleTable.GetRoleId(roleName);

            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentException("Unknown role: " + roleName);
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                _userRoleTable.Delete(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(TUser user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            IList<string> roleNames = _userRoleTable.FindByUserId(user.Id)
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

            var userRoles = _userRoleTable.FindByUserId(user.Id);

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

            _userTable.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            string passwordHash = _userTable.GetPasswordHash(user);

            return Task.FromResult(passwordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user) {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = !string.IsNullOrEmpty(_userTable.GetPasswordHash(user));

            return Task.FromResult(result);
        }
    }
}