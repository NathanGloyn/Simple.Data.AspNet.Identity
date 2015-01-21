using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    
    public class RoleStore<TRole>:IQueryableRoleStore<TRole> where TRole : IdentityRole 
    {
        private readonly Storage _storage;

        public Tables Tables { get; set; }

        public RoleStore():this(null,null) { }

        public RoleStore(string connectionName) :this(connectionName,null) { }

        public RoleStore(Tables tables) :this(null,tables) { }

        public RoleStore(string connectionName, Tables tables)
        {
            Tables = tables ?? new Tables();
            _storage = connectionName == null ? new Storage(Tables) : new Storage(connectionName, Tables);
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

            _storage.RolesTable.Insert(role);

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

            _storage.RolesTable.Update(role);

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

            _storage.RolesTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId) {
            var result = _storage.RolesTable.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName) {
            var result = _storage.RolesTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult(result);
        }

        public IQueryable<TRole> Roles {
            get { return _storage.RolesTable.AllRoles<TRole>().AsQueryable(); }
        }
    }
}