using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Models;

namespace ShopsRUs.Services.Interfaces;

public interface ICustomerService
{
    CustomerDtoModel? GetCustomerById(GetCustomerRequestModel request);
}