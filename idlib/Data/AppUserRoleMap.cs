using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppUserRoleMap
    {
        public AppUserRoleMap(EntityTypeBuilder<IdentityUserRole<long>> entityBuilder)
        {
             entityBuilder.ToTable("AspNetUserRoles");
             entityBuilder.HasKey(e=> new {e.RoleId, e.UserId});
        }
    }
}