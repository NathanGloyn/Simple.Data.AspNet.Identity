using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity {
    public class UserRoleTable 
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public UserRoleTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }


        public void Insert(IdentityUser user, string roleId) {
            _db[_tables.UsersRoles].Insert(UserId: user.Id, roleId: roleId);
        }

        public void Delete(IdentityUser user, string roleId) {
            _db[_tables.UsersRoles].Delete(UserId: user.Id, RoleId: roleId);
        }

        public IEnumerable<IdentityRole> FindByUserId(string userId) {
            return _db[_tables.UsersRoles].FindAllByUserId(userId)
                 .Select(_db[_tables.UsersRoles][_tables.Roles].Name, _db[_tables.UsersRoles].RoleId);
        }
    }
}