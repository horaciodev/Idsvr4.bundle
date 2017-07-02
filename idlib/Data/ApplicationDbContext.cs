using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            new AppUserMap(builder.Entity<ApplicationUser>());
            new AppRoleMap(builder.Entity<ApplicationRole>());
            new AppUserRoleMap(builder.Entity<IdentityUserRole<long>>());
            new AppUserLoginMap(builder.Entity<IdentityUserLogin<long>>());
            new AppRoleClaimMap(builder.Entity<IdentityRoleClaim<long>>());
            new AppUserClaimMap(builder.Entity<IdentityUserClaim<long>>());
            new AppUserTokenMap(builder.Entity<IdentityUserToken<long>>());

        }

    }
}