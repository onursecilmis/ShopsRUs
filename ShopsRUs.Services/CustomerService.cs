using ShopsRUs.Core;
using ShopsRUs.Data.Domain;
using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Interfaces;
using ShopsRUs.Services.Models;

namespace ShopsRUs.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customers> _customerRepository;

    public CustomerService(IRepository<Customers> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public CustomerDtoModel? GetCustomerById(GetCustomerRequestModel request)
    {
        var customer = _customerRepository.Filter(x => x.IsActive && x.Id == request.CustomerId).Select(x=>new CustomerDtoModel
        {
            Id = x.Id,
            CreatedDateTime = x.CreatedDateTime,
            CustomerType = x.CustomerType
        }).FirstOrDefault();

        return customer;
    }
}