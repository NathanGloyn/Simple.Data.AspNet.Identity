﻿using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity 
{
    class UserTable<TUser> where TUser : IdentityUser
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public UserTable(dynamic db, Tables userTable) 
        {
            _db = db;
            _tables = userTable;
        }

        public void Insert(TUser user)
        {
            _db[_tables.Users].Insert(user);
        }

        public int Delete(string id) {
            return _db[_tables.Users].DeleteById(id);
        }

        public TUser GetUserById(string userId) 
        {
            return _db[_tables.Users]
                     .FindAllById(userId)
                     .FirstOrDefault();
        }

        public TUser GetUserByName(string userName) {
            return _db[_tables.Users]
                     .FindAllByUserName(userName)
                     .FirstOrDefault();
        }

        public int Update(TUser user) {
            return _db[_tables.Users].UpdateById(user);
        }

        public IEnumerable<TUser> AllUsers() {
            return _db[_tables.Users].All();
        }

        public string GetPasswordHash(TUser user) {
            var userDetails =  GetUserById(user.Id);

            if (userDetails != null)
            {
                return userDetails.PasswordHash;
            }
            
            return string.Empty;
        }

        public TUser GetUserByEmail(string email)
        {
            return _db[_tables.Users].FindAllByEmail(email).FirstOrDefault();
        }
    }
}