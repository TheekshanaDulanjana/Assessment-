using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CustomerName).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Address1).HasMaxLength(200);
            builder.Property(c => c.Address2).HasMaxLength(200);
            builder.Property(c => c.Address3).HasMaxLength(200);
            builder.Property(c => c.Suburb).HasMaxLength(100);
            builder.Property(c => c.State).HasMaxLength(100);
            builder.Property(c => c.PostCode).HasMaxLength(20);
            builder.HasIndex(c => c.CustomerName);
        }
    }
}
