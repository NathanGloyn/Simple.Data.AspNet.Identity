namespace Simple.Data.AspNet.Identity
{
    public class Tables
    {
        public Tables()
        {
            Users = DefaultTables.Users;
            Roles = DefaultTables.Roles;
            UsersRoles = DefaultTables.UserRoles;
        }

        public string Users { get; private set; }
        public string Roles { get; private set; }
        public string UsersRoles { get; private set; }

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

        private class DefaultTables
        {
            internal const string Users = "AspNetUsers";
            internal const string UserRoles = "AspNetUserRoles";
            internal const string Roles = "AspNetRoles";
        }
    }
}