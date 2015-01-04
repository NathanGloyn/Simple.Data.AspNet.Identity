﻿using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity {
    public class UserTable {

        private readonly dynamic db;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="db"></param>
        public UserTable(dynamic db) 
        {
            this.db = db;
        }

        public void Insert(IdentityUser user)
        {
            db[DefaultTables.Users].Insert(user);
        }

        public int Delete(string id) {
            return db[DefaultTables.Users].DeleteById(id);
        }

        public IdentityUser GetUserById(string userId) 
        {
            return db[DefaultTables.Users]
                     .FindAllById(userId)
                     .FirstOrDefault();
        }

        public IdentityUser GetUserByName(string userName) {
            return db[DefaultTables.Users]
                     .FindAllByUserName(userName)
                     .FirstOrDefault();
        }

        public int Update(IdentityUser user) {
            return db[DefaultTables.Users].UpdateById(user);
        }

        public IEnumerable<TUser> AllUsers<TUser>() {
            return db[DefaultTables.Users].All();
        }

        public string GetPasswordHash(IdentityUser user) {
            var userDetails =  GetUserById(user.Id);

            if (userDetails != null)
            {
                return userDetails.PasswordHash;
            }
            
            return string.Empty;
        }
    }
}