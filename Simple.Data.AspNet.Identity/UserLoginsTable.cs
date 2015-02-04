using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity
{
    class UserLoginsTable
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public UserLoginsTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }

        public List<UserLoginInfo> GetLogins(IdentityUser user)
        {
            var records = _db[_tables.UsersLogins].FindAllByUserId(user.Id);

            var logins = new List<UserLoginInfo>();
            foreach (dynamic record in records)
            {
                logins.Add(new UserLoginInfo(record.LoginProvider,record.ProviderKey));
            }

            return logins;
        }

        public void AddLogin(IdentityUser user, UserLoginInfo login)
        {
            _db[_tables.UsersLogins].Insert(
                LoginProvider: login.LoginProvider,
                ProviderKey: login.ProviderKey,
                UserId: user.Id);
        }

        public void RemoveLogin(IdentityUser user, UserLoginInfo login)
        {
            _db[_tables.UsersLogins].Delete(
                UserId: user.Id,
                LoginProvider: login.LoginProvider,
                ProviderKey: login.ProviderKey);
        }

        public string FindUserId(UserLoginInfo login)
        {
            var result =
                _db[_tables.UsersLogins].FindAllByLoginProviderAndProviderKey(login.LoginProvider, login.ProviderKey)
                    .Select(_db[_tables.UsersLogins].UserId)
                    .FirstOrDefault();

            if (result != null)
            {
                return result.ToScalar();
            }

            return string.Empty;
        }
    }
}