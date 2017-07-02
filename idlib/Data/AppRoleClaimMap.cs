using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppRoleClaimMap
    {
        public AppRoleClaimMap(EntityTypeBuilder<IdentityRoleClaim<long>> entityBuilder)
        {
            entityBuilder.ToTable("AspNetRoleClaims");
            entityBuilder.HasKey(e=>e.Id);
            entityBuilder.Property(e=>e.RoleId).IsRequired();
        }
    }
}