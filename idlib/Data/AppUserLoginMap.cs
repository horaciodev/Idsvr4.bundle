using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppUserLoginMap{

        public AppUserLoginMap(EntityTypeBuilder<IdentityUserLogin<long>> entityBuilder)
        {
             entityBuilder.ToTable("AspNetUserLogins");
             entityBuilder.HasKey(e=> new {e.ProviderKey, e.LoginProvider});
             entityBuilder.Property(e=>e.UserId).IsRequired();
        }
    }
}