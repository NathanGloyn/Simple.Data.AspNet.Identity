using System;

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

        public int Delete(string roleId)
        {
            return _db[_tables.Roles].DeleteById(roleId);
        }

        public void Insert(TRole role) 
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

        public TRole GetRoleById(string roleId) 
        {

            var roleName = GetRoleName(roleId);
            TRole role = null;

            if (roleName != null)
            {
                role = new TRole();
                role.Id = roleId;
                role.Name = roleName;
            }

            return role;

        }

        public TRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            TRole role = null;

            if (roleId != null)
            {
                role = new TRole();
                role.Id = roleId;
                role.Name = role.Name;
            }

            return role;
        }

        public int Update(TRole role) 
        {
            return _db[_tables.Roles].UpdateById(role);
        }
    }
}