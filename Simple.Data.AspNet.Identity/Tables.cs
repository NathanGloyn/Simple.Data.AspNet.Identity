namespace Simple.Data.AspNet.Identity
{
    /// <summary>
    /// Class provides ability to configure the names of the tables
    /// used by the provider
    /// </summary>
    public class Tables
    {
        private const string DefaultUsersTable = "AspNetUsers";
        private const string DefaultUserRolesTable = "AspNetUserRoles";
        private const string DefaultRolesTable = "AspNetRoles";
        private const string DefaultUserClaimsTable = "AspNetUserClaims";
        private const string DefaultUserLoginsTable = "AspNetUserLogins";

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tables()
        {
            Users = DefaultUsersTable;
            Roles = DefaultRolesTable;
            UsersRoles = DefaultUserRolesTable;
            UsersClaims = DefaultUserClaimsTable;
            UsersLogins = DefaultUserLoginsTable;
        }

        /// <summary>
        /// Name of the users table
        /// </summary>
        public string Users { get; private set; }
        
        /// <summary>
        /// Name of the roles table
        /// </summary>
        public string Roles { get; private set; }
        
        /// <summary>
        /// Name of the UsersRoles table
        /// </summary>
        public string UsersRoles { get; private set; }
        
        /// <summary>
        /// Name of the UsersClaims table
        /// </summary>
        public string UsersClaims { get; private set; }
        
        /// <summary>
        /// Name of the UsersLogins table
        /// </summary>
        public string UsersLogins { get; private set; }

        /// <summary>
        /// Sets the name of the Users table
        /// </summary>
        /// <param name="tableName">Name to use for the table</param>
        /// <returns>Tables instance</returns>
        public Tables SetUsersTable(string tableName)
        {
            Users = tableName;
            return this;
        }

        /// <summary>
        /// Sets the name of the Roles table
        /// </summary>
        /// <param name="tableName">Name to use for the table</param>
        /// <returns>Tables instance</returns>
        public Tables SetRolesTable(string tableName)
        {
            Roles = tableName;
            return this;
        }

        /// <summary>
        /// Sets the name of the UserRoles table
        /// </summary>
        /// <param name="tableName">Name to use for the table</param>
        /// <returns>Tables instance</returns>
        public Tables SetUserRolesTable(string tableName)
        {
            UsersRoles = tableName;
            return this;
        }

        /// <summary>
        /// Sets the name of the UserClains table
        /// </summary>
        /// <param name="tableName">Name to use for the table</param>
        /// <returns>Tables instance</returns>
        public Tables SetUserClaimsTable(string tableName)
        {
            UsersClaims = tableName;
            return this;
        }

        /// <summary>
        /// Sets the name of the UserLogins table
        /// </summary>
        /// <param name="tableName">Name to use for the table</param>
        /// <returns>Tables instance</returns>
        public Tables SetUserLoginsTable(string tableName)
        {
            UsersLogins = tableName;
            return this;
        }
    }
}