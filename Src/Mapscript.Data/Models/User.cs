//
// MAPSCRIPT .NET
// Licensed under the MIT license.
//
// (c) Sentrasoft. All rights reserved. 
//
// Author       : Fahmi L. Ramdhani (lrfahmi)
// Creation     : 2017, November (revised)
//
// GITHUB:
// https://github.com/sentrasoft/mapscript-net
//
// MAPSCRIPT: https://mapscript.sentrasoft.com
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapscript.Data.Models
{
    [Table("Permissions")]
    public class Permission
    {
        public Permission()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public bool Active { get; set; } = true;
        public bool Editable { get; set; } = true;
        public bool Deleteable { get; set; } = true;
        public string CreationBy { get; set; } = "";
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        #region Methods
        public bool HasPermission(string requiredPermission)
        {
            bool found = false;
            foreach (Role role in Roles)
            {
                found = (role.Permissions.Where(p => p.Name == requiredPermission).ToList().Count > 0);
                if (found)
                    break;
            }

            return found;
        }

        public bool HasRole(string role)
        {
            return (Roles.Where(p => p.Name == role).ToList().Count > 0);
        }

        public bool HasRoles(string roles)
        {
            bool found = false;
            string[] hasRoles = roles.ToLower().Split(';');

            foreach (Role role in this.Roles)
            {
                try
                {
                    found = hasRoles.Contains(role.Name.ToLower());
                    if (found)
                        return found;
                }
                catch (Exception) { }
            }

            return found;
        }
        #endregion
    }

    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            Permissions = new HashSet<Permission>();
            Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public bool Active { get; set; } = true;
        public bool Editable { get; set; } = true;
        public bool Deleteable { get; set; } = true;
        public string CreationBy { get; set; } = "";
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedDate { get; set; }

        public virtual HashSet<Permission> Permissions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

    [Table("Users")]
    public class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        [MaxLength(32)]
        public string UserName { get; set; }

        public string Password { get; set; }
        public string State { get; set; } = "";
        public DateTime? LastLogin { get; set; }

        public bool Active { get; set; } = true;
        public bool Editable { get; set; } = true;
        public bool Deleteable { get; set; } = true;
        public string CreationBy { get; set; } = "";
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
