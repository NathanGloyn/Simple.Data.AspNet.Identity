using System;

namespace Simple.Data.AspNet.Identity 
{

    class RoleTable
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        public RoleTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }

        public int Delete(string roleId)
        {
            return _db[_tables.Roles].DeleteById(roleId);
        }

        public void Insert(IdentityRole role) 
        {
            _db[_tables.Roles].Insert(role);
        }

        public string GetRoleName(string roleId)
        {
            var result = _db[_tables.Roles]
                     .FindAllById(roleId)
                     .Select(_db[_tables.Roles].Name)
                     .FirstOrDefault();

            if (result != null) 
            {
                return Convert.ToString(result.Name);
            }

            return null;
        }


        public string GetRoleId(string roleName)
        {
            var result = _db[_tables.Roles]
                           .FindAllByName(roleName)
                           .Select(_db[_tables.Roles].Id)
                           .FirstOrDefault();

            if (result != null)
            {
                return Convert.ToString(result.Id);
            }

            return null;
        }

        public IdentityRole GetRoleById(string roleId) 
        {

            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if (roleName != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;

        }

        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;
        }

        public int Update(IdentityRole role) 
        {
            return _db[_tables.Roles].UpdateById(role);
        }
    }
}