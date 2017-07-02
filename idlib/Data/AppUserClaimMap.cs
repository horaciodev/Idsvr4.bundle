using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppUserClaimMap{

        public AppUserClaimMap(EntityTypeBuilder<IdentityUserClaim<long>> entityBuilder)
        {
            entityBuilder.ToTable("AspNetUserClaims");
            entityBuilder.HasKey(e=>e.Id);
            entityBuilder.Property(e=>e.UserId).IsRequired();
        }
    }
}