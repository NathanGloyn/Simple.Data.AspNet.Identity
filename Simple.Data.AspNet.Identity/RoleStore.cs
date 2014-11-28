using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity {
    public class RoleStore<TRole>:IQueryableRoleStore<TRole> where TRole : IdentityRole {

        private readonly RoleTable _roleTable;

        public RoleStore() {
            dynamic database = Database.Open();
            _roleTable = new RoleTable(database);
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

            _roleTable.Insert(role);

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

            _roleTable.Update(role);

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

            _roleTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId) {
            var result = _roleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName) {
            var result = _roleTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult(result);
        }

        public IQueryable<TRole> Roles {
            get { return _roleTable.AllRoles<TRole>().AsQueryable(); }
        }
    }
}