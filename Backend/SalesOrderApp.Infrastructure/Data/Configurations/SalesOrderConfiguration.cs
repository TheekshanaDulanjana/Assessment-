using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Infrastructure.Data.Configurations
{
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.ToTable("SalesOrders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.InvoiceNo).IsRequired().HasMaxLength(50);
            builder.HasIndex(o => o.InvoiceNo).IsUnique();
            builder.Property(o => o.ReferenceNo).HasMaxLength(100);
            builder.Property(o => o.Note).HasMaxLength(1000);
            builder.Property(o => o.TotalExcl).HasColumnType("decimal(18,2)");
            builder.Property(o => o.TotalTax).HasColumnType("decimal(18,2)");
            builder.Property(o => o.TotalIncl).HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Client)
                   .WithMany(c => c.SalesOrders)
                   .HasForeignKey(o => o.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Items)
                   .WithOne(i => i.SalesOrder!)
                   .HasForeignKey(i => i.SalesOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
