using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Genesis.idlib.Models;

namespace Genesis.idlib.Data
{
    public class AppRoleMap
    {
        public AppRoleMap(EntityTypeBuilder<ApplicationRole> entityBuilder)
        {
            entityBuilder.ToTable("AspNetRoles");
            entityBuilder.HasKey(e=>e.Id);
            entityBuilder.Property(e=>e.Id).ValueGeneratedOnAdd();
            entityBuilder.Property(e=>e.RoleDescription).HasMaxLength(256);
        }
    }
}