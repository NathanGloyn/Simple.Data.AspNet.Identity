using System.Text.RegularExpressions;

namespace Simple.Data.AspNet.Identity
{
    class Storage<TUser, TRole> where TUser : IdentityUser 
                                where TRole : IdentityRole, new()
    {
        private readonly Tables _tables;
        private UserTable<TUser> _userTable;
        private RoleTable<TRole> _roleTable;
        private UserRoleTable _userRoleTable;
        private UserClaimsTable _userClaimsTable;
        private UserLoginsTable _userLoginsTable;
        private readonly Regex _isConnectionString = new Regex("^([^\\\"])([^=;]+=[^=;]+;){1,}(.*[^\\\"]$)", RegexOptions.Compiled);

        private readonly dynamic _database;

        /// <summary>
        /// Constructor that takes table details to use
        /// </summary>
        /// <param name="tables"><see cref="Tables"/> holding table details</param>
        public Storage(Tables tables):this(null,tables) { }

        /// <summary>
        /// Constructor that takes connection name and table details to use
        /// </summary>
        /// <param name="connection">Name of connection string in config or a valid connection string</param>
        /// <param name="tables"><see cref="Tables"/> holding table details</param>
        public Storage(string connection, Tables tables)
        {
            _tables = tables;
            if (string.IsNullOrWhiteSpace(connection))
            {
                _database = Database.Open();
            }
            else
            {
                _database = _isConnectionString.IsMatch(connection) ? Database.OpenConnection(connection) : Database.OpenNamedConnection(connection);
            }
            
        }

        private void OpenConnection(string connection)
        {
            
        }

        public UserTable<TUser> UsersTable
        {
            get { return _userTable ?? (_userTable = new UserTable<TUser>(_database, _tables)); }
        }

        public RoleTable<TRole> RolesTable
        {
            get { return _roleTable ?? (_roleTable = new RoleTable<TRole>(_database, _tables)); }
        }

        public UserRoleTable UserRolesTable
        {
            get { return _userRoleTable ?? (_userRoleTable = new UserRoleTable(_database, _tables)); }
        }

        public UserClaimsTable UserClaimsTable
        {
            get { return _userClaimsTable ?? (_userClaimsTable = new UserClaimsTable(_database, _tables)); }
        }

        public UserLoginsTable UserLoginsTable
        {
            get { return _userLoginsTable ?? (_userLoginsTable = new UserLoginsTable(_database, _tables)); }
        }
    }
}