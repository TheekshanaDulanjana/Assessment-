using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Infrastructure.Data
{
    /// <summary>
    /// Applies pending EF Core migrations and seeds baseline reference data
    /// (Clients, Items) so the app is immediately usable after first run.
    /// </summary>
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!context.Clients.Any())
            {
                context.Clients.AddRange(
                    new Client { CustomerName = "Acme Trading Pvt Ltd", Address1 = "12 Galle Road", Address2 = "Colombo 03", Suburb = "Colombo", State = "Western", PostCode = "00300" },
                    new Client { CustomerName = "Blue Ocean Exports", Address1 = "45 Marine Drive", Suburb = "Negombo", State = "Western", PostCode = "11500" },
                    new Client { CustomerName = "Ceylon Hardware Supplies", Address1 = "7 Kandy Road", Suburb = "Kurunegala", State = "North Western", PostCode = "60000" }
                );
            }

            if (!context.Items.Any())
            {
                context.Items.AddRange(
                    new Item { ItemCode = "ITM-001", Description = "A4 Copy Paper (Ream)", Price = 850m },
                    new Item { ItemCode = "ITM-002", Description = "Ballpoint Pen (Box of 50)", Price = 1200m },
                    new Item { ItemCode = "ITM-003", Description = "Office Chair - Standard", Price = 15500m },
                    new Item { ItemCode = "ITM-004", Description = "USB Flash Drive 32GB", Price = 1800m },
                    new Item { ItemCode = "ITM-005", Description = "Laser Printer Toner", Price = 6200m }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
