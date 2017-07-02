using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppUserMap
    {
        public AppUserMap(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder.ToTable("AspNetUsers");
            entityBuilder.HasKey(e=>e.Id);
            entityBuilder.Property(e=>e.Id).ValueGeneratedOnAdd();
            entityBuilder.Property(e=>e.FirstName).HasMaxLength(256);
            entityBuilder.Property(e=>e.LastName).HasMaxLength(256);
            entityBuilder.Property(e=>e.CompanyId);
        }
    }
}