using System;
using System.Collections.Generic;

namespace Simple.Data.AspNet.Identity {

    /// <summary>
    /// Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleTable
    {
        private readonly dynamic db;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="db"></param>
        public RoleTable(dynamic db) 
        {
            this.db = db;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            return db.AspNetRoles.DeleteById(roleId);
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public void Insert(IdentityRole role) 
        {

            if (role == null) {
                throw new ArgumentNullException("role");
            }

            db.AspNetRoles.Insert(role);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(string roleId)
        {
            var result = db.AspNetRoles
                     .FindAllById(roleId)
                     .Select(db.AspNetRoles.Name)
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
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            var result = db.AspNetRoles
                           .FindAllByName(roleName)
                           .Select(db.AspNetRoles.Id)
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
            return db.AspNetRoles.UpdateById(role);
        }

        public IEnumerable<TRole> AllRoles<TRole>() {
            return db.AspNetRoles.All().ToList<TRole>();
        }  
    }
}