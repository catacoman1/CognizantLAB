using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Interfaces;

public interface IFoodShareDbContext
{
    DbSet<Courier> Couriers { get; set; }
    
    DbSet<Beneficiary> Beneficiaries { get; set; }
    
    DbSet<Donor> Donors { get; set; }
    
    DbSet<Donation> Donations { get; set; }

    DbSet<DonationStatus> DonationStatuses { get; set; }
    
    DbSet<Order> Orders { get; set; }
    
    DbSet<OrderStatus> OrderStatuses { get; set; }
    
    DbSet<City> Cities { get; set; }
    
    DbSet<Product> Products { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
}