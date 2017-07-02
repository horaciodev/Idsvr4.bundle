using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppUserTokenMap{

        public AppUserTokenMap(EntityTypeBuilder<IdentityUserToken<long>> entityBuilder)
        {
            entityBuilder.ToTable("AspNetUserTokens");
            entityBuilder.HasKey(e=> new {e.UserId, e.LoginProvider, e.Name});
        }
    }
}