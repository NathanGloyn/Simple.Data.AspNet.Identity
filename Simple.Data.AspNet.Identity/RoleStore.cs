using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    
    public class RoleStore<TRole>:IRoleStore<TRole> where TRole : IdentityRole
    {
        private bool _disposed;

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

        public void Dispose()
        {
            _disposed = true;
        }

        public Task CreateAsync(TRole role) {

            ThrowIfDisposed();

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

            ThrowIfDisposed();

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

            ThrowIfDisposed();

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

            ThrowIfDisposed();

            var result = _storage.RolesTable.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName) {

            ThrowIfDisposed();

            var result = _storage.RolesTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult(result);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}