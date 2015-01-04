using System;
using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity {

    /// <summary>
    /// Class that represents the Roles table in the MySQL Database
    /// </summary>
    public class RoleTable
    {
        private readonly dynamic _db;
        private readonly Tables _tables;

        /// <summary>
        /// Constructor that takes a Simple.Data db instance 
        /// </summary>
        /// <param name="db">Simple.Data database instance</param>
        /// <param name="tables"><see cref="Tables"/> instance with table names</param>
        public RoleTable(dynamic db, Tables tables) 
        {
            _db = db;
            _tables = tables;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            return _db[_tables.Roles].DeleteById(roleId);
        }

        /// <summary>
        /// Inserts a new Roles in the Roles table
        /// </summary>
        /// <param name="role">The role's to insert</param>
        /// <returns></returns>
        public void Insert(IdentityRole role) 
        {
            _db[_tables.Roles].Insert(role);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Roles name</returns>
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

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Roles's name</param>
        /// <returns>Roles's Id</returns>
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

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
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

        public IEnumerable<TRole> AllRoles<TRole>() {
            return _db[_tables.Roles].All().ToList<TRole>();
        }  
    }
}