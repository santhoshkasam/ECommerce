using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext customersDbContext;
        private readonly ILogger<ICustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext customersDbContext, ILogger<ICustomersProvider> logger, IMapper mapper)
        {
            this.customersDbContext = customersDbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedCustomerData();
        }

        private void SeedCustomerData()
        {
            if (!customersDbContext.Customers.Any())
            {
                customersDbContext.Add(new Db.Customer() { Id = 1, Name = "San", Address = "Lingampally" });
                customersDbContext.Add(new Db.Customer() { Id = 2, Name = "Guru", Address = "Balangar" });
                customersDbContext.Add(new Db.Customer() { Id = 3, Name = "Sita", Address = "Mallampet" });
                customersDbContext.Add(new Db.Customer() { Id = 4, Name = "Shekar", Address = "Ammenpur" });
                customersDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await customersDbContext.Customers.ToListAsync();
                if (customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await customersDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
