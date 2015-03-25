using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    
    public class RoleStore<TRole>: IRoleStore<TRole> where TRole : IdentityRole, new()
    {
        private bool _disposed;

        private readonly Storage<IdentityUser,TRole> _storage;

        /// <summary>
        /// <see cref="Tables"/> object with names of tables used by identity
        /// </summary>
        public Tables Tables { get; set; }

        /// <summary>
        /// Default constructor that initializes a new
        /// instance using the default Simple.Data connection string
        /// from properties.
        /// 
        /// Uses the default AspNet Identity table names
        /// </summary>
        public RoleStore():this(null,null) { }

        /// <summary>
        /// Constructor that initializes a new
        /// instance using the named connection string
        /// 
        /// Uses the default AspNet Identity table names
        /// </summary>
        public RoleStore(string connectionName) :this(connectionName,null) { }

        /// <summary>
        /// Constructor that initializes a new
        /// instance using the default Simple.Data connection string
        /// from properties.
        /// 
        /// Uses the table names provided
        /// </summary>
        public RoleStore(Tables tables) :this(null,tables) { }

        /// <summary>
        /// Constructor that initializes a new
        /// instance using the named connection string
        /// 
        /// Uses the table names provided
        /// </summary>
        public RoleStore(string connectionName, Tables tables)
        {
            Tables = tables ?? new Tables();
            _storage = connectionName == null ? new Storage<IdentityUser, TRole>(Tables) : new Storage<IdentityUser, TRole>(connectionName, Tables);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="RoleStore"/>
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }

        /// <summary>
        /// Inserts a role.
        /// </summary>
        /// <param name="role">The Role to be inserted</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="role"/> is null</exception>
        /// <exception cref="ArgumentException">If Role.Id is null, empty or whitespace</exception>
        /// <exception cref="ArgumentException">If Role.Name is null, empty or whitespace</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="RoleStore"/> has been disposed</exception>
        public async Task CreateAsync(TRole role) {

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

            await _storage.RolesTable.Insert(role);

        }

        /// <summary>
        /// Updates a role.
        /// </summary>
        /// <param name="role">The Role to update</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="role"/> is null</exception>
        /// <exception cref="ArgumentException">If Role.Id is null, empty or whitespace</exception>
        /// <exception cref="ArgumentException">If Role.Name is null, empty or whitespace</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="RoleStore"/> has been disposed</exception>
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

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">The role to delete</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="role"/> is null</exception>
        /// <exception cref="ArgumentException">If Role.Id is null, empty or whitespace</exception>
        /// <exception cref="ObjectDisposedException">If <see cref="RoleStore"/> has been disposed</exception>
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

        /// <summary>
        /// Finds a role by using the specified identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The task representing the asynchronous operation</returns>
        /// <exception cref="ObjectDisposedException">If <see cref="RoleStore"/> has been disposed</exception>
        public async Task<TRole> FindByIdAsync(string roleId) {

            ThrowIfDisposed();

            var result = await _storage.RolesTable.GetRoleById(roleId) as TRole;

            return result;
        }

        /// <summary>
        /// Finds a role by its name
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>The task representing the asynchronous operation</returns>
        /// <exception cref="ObjectDisposedException">If <see cref="RoleStore"/> has been disposed</exception>
        public async Task<TRole> FindByNameAsync(string roleName) {

            ThrowIfDisposed();

            var result = await _storage.RolesTable.GetRoleByName(roleName) as TRole;

            return result;
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