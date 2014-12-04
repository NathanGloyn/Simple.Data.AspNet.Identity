namespace Simple.Data.AspNet.Identity {
    public class UserRoleTable 
    {
        private readonly dynamic db;

        public UserRoleTable(dynamic db) 
        {
            this.db = db;
        }


        public void Insert(IdentityUser user, string roleId) {
            db.AspNetUserRole.Insert(UserId: user.Id, roleId: roleId);
        }

        public void Delete(IdentityUser user, string roleId) {
            db.AspNetUserRole.Delete(UserId: user.Id, RoleId: roleId);
        }
    }
}