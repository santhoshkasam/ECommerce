using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
  public class SearchService : ISearchService
  {
    private readonly IOrderService orderService;
    private readonly IProductService productService;
    private readonly ICustomerService customerService;

    public SearchService(IOrderService orderService, IProductService productService, ICustomerService customerService)
    {
      this.orderService = orderService;
      this.productService = productService;
      this.customerService = customerService;
    }
    public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
    {
      var customerResult = await customerService.GetCustomerAsync(customerId);
      var orderResult = await orderService.GetOrdersAsync(customerId);
      var productResult = await productService.GetProductAsync();
      if (orderResult.IsSuccess)
      {
        foreach (var order in orderResult.Orders)
        {
          foreach (var item in order.Items)
          {
            item.ProductName = productResult.IsSuccess ?
            productResult.Products.FirstOrDefault(p => p.Id == item.Id)?.Name : "Product information is not available";
          }
        }
        var result = new
        {
          Customer = customerResult.IsSucess ? customerResult.Customers : new { Name = "Customer information is not available" },
          Orders = orderResult.Orders
        };
        return (true, result);
      }
      return (false, null);
    }
  }
}
