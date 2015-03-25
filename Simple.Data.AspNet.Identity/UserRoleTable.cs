using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Data.AspNet.Identity 
{
    class UserRoleTable 
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public UserRoleTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }


        public async Task Insert(IdentityUser user, string roleId) {
            await _db[_tables.UsersRoles].Insert(UserId: user.Id, roleId: roleId);
        }

        public async void Delete(IdentityUser user, string roleId) {
            await _db[_tables.UsersRoles].Delete(UserId: user.Id, RoleId: roleId);
        }

        public async Task<IEnumerable<IdentityRole>> FindByUserId(string userId) {
            return await _db[_tables.UsersRoles].FindAllByUserId(userId)
                 .Select(_db[_tables.UsersRoles][_tables.Roles].Name, _db[_tables.UsersRoles].RoleId);
        }
    }
}