using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using ShopsRUs.Core;
using ShopsRUs.Data.Domain;
using ShopsRUs.Services;
using ShopsRUs.Services.Interfaces;
using ShopsRUs.Services.Models;
using Xunit;

namespace ShopsRUs.Test;

public class CustomerServiceTest : IDisposable
{
    private readonly Mock<IRepository<Customers>> _customerRepository;

    private readonly CustomerService _customerService;

    public CustomerServiceTest()
    {
        _customerRepository = new Mock<IRepository<Customers>>(MockBehavior.Strict);
        _customerService = new CustomerService(_customerRepository.Object);
    }

    public void Dispose()
    {
        _customerRepository.VerifyAll();
    }

    [Fact]
    public void GetCustomerByIdTest()
    {
        _customerRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Customers, bool>>>()))
            .Returns((Expression<Func<Customers, bool>> filter) => new List<Customers>()
            {
                new Customers()
                {
                    Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    CreatedDateTime = DateTime.Now,
                    CustomerType = "Employee",
                    IsActive = true,
                    IsDeleted = false,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        var result = _customerService.GetCustomerById(new GetCustomerRequestModel { CustomerId = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80") });
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"), result.Id);
        Assert.Equal("Employee", result.CustomerType);
    }
    
    [Fact]
    public void GetCustomerByIdNullTest()
    {
        _customerRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Customers, bool>>>()))
            .Returns((Expression<Func<Customers, bool>> filter) => new List<Customers>()
            {
                new Customers()
                {
                    Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    CreatedDateTime = DateTime.Now,
                    CustomerType = "Employee",
                    IsActive = true,
                    IsDeleted = false,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        var result = _customerService.GetCustomerById(new GetCustomerRequestModel { CustomerId = Guid.NewGuid() });
        Assert.Null(result);
    }
}