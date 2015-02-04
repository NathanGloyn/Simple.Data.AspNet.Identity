using System;
using Microsoft.AspNet.Identity;

namespace Simple.Data.AspNet.Identity 
{

    /// <summary>
    /// Class that implements the ASP.NET Identity
    /// IRole interface 
    /// </summary>
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Default constructor for Roles 
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name): this()
        {
            Name = name;
        }

        /// <summary>
        /// Constructor that takes name and id as arguments
        /// </summary>
        /// <param name="name">Role name</param>
        /// <param name="id">Role id</param>
        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Roles ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Roles name
        /// </summary>
        public string Name { get; set; }
    }
}