using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FoodShareNet.Repository.Data;


    public class FoodShareNetDbContextFactory : IDesignTimeDbContextFactory<FoodShareNetDbContext>
    {
        public FoodShareNetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FoodShareNetDbContext>();
            
            var connectionString = "Server=localhost,1433;initial catalog=localDB;TrustServerCertificate=True;user id=sa;password=Supremulcata12.";

            optionsBuilder.UseSqlServer(connectionString);

            return new FoodShareNetDbContext(optionsBuilder.Options);
        }
    }
