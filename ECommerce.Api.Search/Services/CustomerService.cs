using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using System.Text.Json;

namespace ECommerce.Api.Search.Services
{
  public class CustomerService : ICustomerService
  {
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<CustomerService> logger;

    public CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger)
    {
      this.httpClientFactory = httpClientFactory;
      this.logger = logger;
    }
    public async Task<(bool IsSucess, dynamic Customers, string ErrorMessage)> GetCustomerAsync(int customerId)
    {
      try
      {
        var client = httpClientFactory.CreateClient("CustomerService");
        var response = await client.GetAsync($"api/customers/{customerId}");
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsByteArrayAsync();
          var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
          var result = JsonSerializer.Deserialize<dynamic>(content, options);
          return (true, result, null);
        }
        return (false, null, response.ReasonPhrase);
      }
      catch (Exception ex)
      {
        logger?.LogError(ex.Message);
        return (false, null, "customer not found");
      }
    }
  }
}
