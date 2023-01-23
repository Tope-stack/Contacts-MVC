using ContactsMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContactsMVC.Data
{
    public class ContactsDemoDbContext : DbContext
    {
        public ContactsDemoDbContext(DbContextOptions options) : base(options)
        { 
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
