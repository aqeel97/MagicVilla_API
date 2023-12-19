using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace MagicVilla_VillaAPI.Data
{
    //in Consol write this commands to migration the Database
    //add-migration AddVillaTable
    //to apply this migration add write this command
    //update-database
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
    }
}
