using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    public class RoleStore<TRole>:IQueryableRoleStore<TRole> where TRole : IdentityRole {

        private RoleTable _roleTable;
        public RoleTable RoleTable
        {
            get { return _roleTable ?? (_roleTable = new RoleTable(_database, Tables)); }
        }

        private readonly dynamic _database;

        public Tables Tables { get; set; }

        public RoleStore() {
            _database = Database.Open();
            Tables = new Tables();
        }

        public RoleStore(string connectionName)
        {
            _database = Database.OpenNamedConnection(connectionName);
            Tables = new Tables();
        }

        public RoleStore(Tables tables)
        {
            _database = Database.Open();
            Tables = tables;
        }

        public RoleStore(string connectionName, Tables tables)
        {
            _database = Database.OpenNamedConnection(connectionName);
            Tables = tables;
        }

        public void Dispose() {
            // We let Simple.Data handle its own disposal
        }

        public Task CreateAsync(TRole role) {
            if (role == null) {
                throw new ArgumentNullException("role");
            }

            if (string.IsNullOrWhiteSpace(role.Id)) {
                throw new ArgumentException("Missing role Id");
            }

            if (string.IsNullOrWhiteSpace(role.Name)) 
            {
                throw new ArgumentException("Missing role Name");    
            }

            RoleTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(TRole role) {
            if (role == null) 
            {
                throw new ArgumentNullException("role");
            }

            if (string.IsNullOrWhiteSpace(role.Id))
            {
                throw new ArgumentException("Missing role Id");
            }

            if (string.IsNullOrWhiteSpace(role.Name))
            {
                throw new ArgumentException("Missing role Name");
            }

            RoleTable.Update(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role) {
            if (role == null) 
            {
                throw new ArgumentNullException("role");
            }

            if (string.IsNullOrWhiteSpace(role.Id))
            {
                throw new ArgumentException("Missing role Id");
            }

            RoleTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId) {
            var result = RoleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName) {
            var result = RoleTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult(result);
        }

        public IQueryable<TRole> Roles {
            get { return RoleTable.AllRoles<TRole>().AsQueryable(); }
        }
    }
}