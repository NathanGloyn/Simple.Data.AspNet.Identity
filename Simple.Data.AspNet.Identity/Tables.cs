namespace Simple.Data.AspNet.Identity
{
    public class Tables
    {
        private const string DefaultUsersTable = "AspNetUsers";
        private const string DefaultUserRolesTable = "AspNetUserRoles";
        private const string DefaultRolesTable = "AspNetRoles";
        private const string DefaultUserClaimsTable = "AspNetUserClaims";

        public Tables()
        {
            Users = DefaultUsersTable;
            Roles = DefaultRolesTable;
            UsersRoles = DefaultUserRolesTable;
            UsersClaims = DefaultUserClaimsTable;
        }

        public string Users { get; private set; }
        public string Roles { get; private set; }
        public string UsersRoles { get; private set; }
        public string UsersClaims { get; private set; }

        public Tables SetUsersTable(string tableName)
        {
            Users = tableName;
            return this;
        }

        public Tables SetRolesTable(string tableName)
        {
            Roles = tableName;
            return this;
        }

        public Tables SetUserRolesTable(string tableName)
        {
            UsersRoles = tableName;
            return this;
        }

        public Tables SetUserClaimsTable(string tableName)
        {
            UsersClaims = tableName;
            return this;
        }
    }
}