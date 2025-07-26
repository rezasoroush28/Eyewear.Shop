using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
               .ValueGeneratedOnAdd();
        builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(15);
    }
}