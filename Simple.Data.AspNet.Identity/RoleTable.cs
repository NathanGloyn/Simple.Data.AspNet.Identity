using System;
using System.Threading.Tasks;

namespace Simple.Data.AspNet.Identity 
{

    class RoleTable<TRole> where TRole : IdentityRole, new()
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public RoleTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }

        public async Task<int> Delete(string roleId)
        {
            return await _db[_tables.Roles].DeleteById(roleId);
        }

        public async Task Insert(TRole role) 
        {
            await _db[_tables.Roles].Insert(role);
        }

        public async Task<string> GetRoleName(string roleId)
        {
            var result = await _db[_tables.Roles]
                     .FindAllById(roleId)
                     .Select(_db[_tables.Roles].Name)
                     .FirstOrDefault();

            if (result != null) 
            {
                return Convert.ToString(result.Name);
            }

            return null;
        }


        public async Task<string> GetRoleId(string roleName)
        {
            var result = await _db[_tables.Roles]
                           .FindAllByName(roleName)
                           .Select(_db[_tables.Roles].Id)
                           .FirstOrDefault();

            if (result != null)
            {
                return Convert.ToString(result.Id);
            }

            return null;
        }

        public async Task<TRole> GetRoleById(string roleId) 
        {

            var roleName = await GetRoleName(roleId);
            TRole role = null;

            if (roleName != null)
            {
                role = new TRole();
                role.Id = roleId;
                role.Name = roleName;
            }

            return role;

        }

        public async Task<TRole> GetRoleByName(string roleName)
        {
            var roleId = await GetRoleId(roleName);
            TRole role = null;

            if (roleId != null)
            {
                role = new TRole();
                role.Id = roleId;
                role.Name = role.Name;
            }

            return role;
        }

        public async Task<int> Update(TRole role) 
        {
            return await _db[_tables.Roles].UpdateById(role);
        }
    }
}