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
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapscript.Data
{
    using Models;

    public class DataContext : DbContext
    {
        public DataContext() : base("Default")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
        }

        #region Entities
        public virtual DbSet<User> Users { get; set; }
        #endregion

        #region Overrides
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Permissions)
                .Map(m => m.ToTable("HasRolePermissions")
                .MapLeftKey("PermissionId").MapRightKey("RoleId"));

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("HasUserRoles")
                .MapLeftKey("RoleId").MapRightKey("UserId"));
        }
        #endregion

    }
}
