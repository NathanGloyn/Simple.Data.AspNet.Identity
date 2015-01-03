using System.Collections.Generic;
using System.IO.Pipes;

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
            db.AspNetUsers.Insert(user);
        }

        public int Delete(string id) {
            return db.AspNetUsers.DeleteById(id);
        }

        public IdentityUser GetUserById(string userId) 
        {
            return db.AspNetUsers
                     .FindAllById(userId)
                     .FirstOrDefault();
        }

        public IdentityUser GetUserByName(string userName) {
            return db.AspNetUsers
                     .FindAllByUserName(userName)
                     .FirstOrDefault();
        }

        public int Update(IdentityUser user) {
            return db.AspNetUsers.UpdateById(user);
        }

        public IEnumerable<TUser> AllUsers<TUser>() {
            return db.AspNetUsers.All();
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