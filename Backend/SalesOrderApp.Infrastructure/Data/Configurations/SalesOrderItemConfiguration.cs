using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Infrastructure.Data.Configurations
{
    public class SalesOrderItemConfiguration : IEntityTypeConfiguration<SalesOrderItem>
    {
        public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
        {
            builder.ToTable("SalesOrderItems");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.ItemCode).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Description).IsRequired().HasMaxLength(300);
            builder.Property(i => i.Note).HasMaxLength(500);
            builder.Property(i => i.Quantity).HasColumnType("decimal(18,2)");
            builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
            builder.Property(i => i.TaxRate).HasColumnType("decimal(5,2)");
            builder.Property(i => i.ExclAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.TaxAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.InclAmount).HasColumnType("decimal(18,2)");
        }
    }
}
