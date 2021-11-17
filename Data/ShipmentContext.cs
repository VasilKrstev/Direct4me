using Direct4me.Models;
using Microsoft.EntityFrameworkCore;

namespace Direct4me.Data
{
    public class ShipmentContext : DbContext
    {
        public ShipmentContext(DbContextOptions<ShipmentContext> options)
            : base(options)
        {
        }

        public DbSet<Shipment> Shipment { get; set; }
    }
}
