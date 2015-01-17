using System;
using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity {
    public class UserTable {

        private readonly dynamic _db;
        private readonly Tables _tables;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="db">Simple.Data database object</param>
        /// <param name="userTable">Name of the users table</param>
        public UserTable(dynamic db, Tables userTable) 
        {
            _db = db;
            _tables = userTable;
        }

        public void Insert(IdentityUser user)
        {
            _db[_tables.Users].Insert(user);
        }

        public int Delete(string id) {
            return _db[_tables.Users].DeleteById(id);
        }

        public IdentityUser GetUserById(string userId) 
        {
            return _db[_tables.Users]
                     .FindAllById(userId)
                     .FirstOrDefault();
        }

        public IdentityUser GetUserByName(string userName) {
            return _db[_tables.Users]
                     .FindAllByUserName(userName)
                     .FirstOrDefault();
        }

        public int Update(IdentityUser user) {
            return _db[_tables.Users].UpdateById(user);
        }

        public IEnumerable<TUser> AllUsers<TUser>() {
            return _db[_tables.Users].All();
        }

        public string GetPasswordHash(IdentityUser user) {
            var userDetails =  GetUserById(user.Id);

            if (userDetails != null)
            {
                return userDetails.PasswordHash;
            }
            
            return string.Empty;
        }

        public DateTime? GetLockoutEndDate(IdentityUser user)
        {
            var userDetails = GetUserById(user.Id);

            return userDetails.LockoutEndDateUtc;
        }
    }
}