using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity {
    public class UserRoleTable 
    {
        private readonly dynamic db;

        public UserRoleTable(dynamic db) 
        {
            this.db = db;
        }


        public void Insert(IdentityUser user, string roleId) {
            db[DefaultTables.UserRoles].Insert(UserId: user.Id, roleId: roleId);
        }

        public void Delete(IdentityUser user, string roleId) {
            db[DefaultTables.UserRoles].Delete(UserId: user.Id, RoleId: roleId);
        }

        public IEnumerable<IdentityRole> FindByUserId(string userId) {
            return db[DefaultTables.UserRoles].FindAllByUserId(userId)
                 .Select(db[DefaultTables.UserRoles][DefaultTables.Roles].Name, db.AspNetUserRole.RoleId);
        }
    }
}