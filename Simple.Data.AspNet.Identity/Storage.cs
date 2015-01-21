namespace Simple.Data.AspNet.Identity
{
    public class Storage {
        private readonly Tables _tables;
        private UserTable _userTable;
        private RoleTable _roleTable;
        private UserRoleTable _userRoleTable;
        private UserClaimsTable _userClaimsTable;
        private UserLoginsTable _userLoginsTable;

        private readonly dynamic _database;

        public Storage(Tables tables):this(null,tables) { }

        public Storage(string connectionName, Tables tables)
        {
            _tables = tables;
            _database = connectionName == null ? Database.Open() : Database.OpenNamedConnection(connectionName);
        }

        public UserTable UsersTable
        {
            get { return _userTable ?? (_userTable = new UserTable(_database, _tables)); }
        }

        public RoleTable RolesTable
        {
            get { return _roleTable ?? (_roleTable = new RoleTable(_database, _tables)); }
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