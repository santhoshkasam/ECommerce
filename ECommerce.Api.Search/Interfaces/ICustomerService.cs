using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
  public interface ICustomerService
  {
    Task<(bool IsSucess, dynamic Customers, string ErrorMessage)> GetCustomerAsync(int customerId);
  }
}
