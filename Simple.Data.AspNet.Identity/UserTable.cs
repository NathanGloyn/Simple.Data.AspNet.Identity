using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task Insert(TUser user)
        {
            await _db[_tables.Users].Insert(user);
        }

        public async Task<int> Delete(string id) {
            return await _db[_tables.Users].DeleteById(id);
        }

        public async Task<TUser> GetUserById(string userId) 
        {
            return await _db[_tables.Users]
                     .FindAllById(userId)
                     .FirstOrDefault();
        }

        public async Task<TUser> GetUserByName(string userName) {
            return await _db[_tables.Users]
                     .FindAllByUserName(userName)
                     .FirstOrDefault();
        }

        public async Task<int> Update(TUser user) {
            return await _db[_tables.Users].UpdateById(user);
        }

        public async Task<IEnumerable<TUser>> AllUsers() {
            return await _db[_tables.Users].All();
        }

        public async Task<string> GetPasswordHash(TUser user) {
            var userDetails =  await GetUserById(user.Id);

            if (userDetails != null)
            {
                return userDetails.PasswordHash;
            }
            
            return string.Empty;
        }

        public async Task<TUser> GetUserByEmail(string email)
        {
            return await _db[_tables.Users].FindAllByEmail(email).FirstOrDefault();
        }
    }
}