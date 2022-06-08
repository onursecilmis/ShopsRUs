using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using ShopsRUs.Core;
using ShopsRUs.Data.Domain;
using ShopsRUs.Services;
using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Interfaces;
using ShopsRUs.Services.Models;
using Xunit;

namespace ShopsRUs.Test;

public class InvoiceServiceTest : IDisposable
{
    private readonly Mock<IRepository<Invoices>> _invoiceRepository;
    private readonly Mock<ICustomerService> _customerService;
    private readonly Mock<IDiscountService> _discountService;

    private readonly InvoiceService _invoiceService;

    public InvoiceServiceTest()
    {
        _invoiceRepository = new Mock<IRepository<Invoices>>(MockBehavior.Strict);
        _customerService = new Mock<ICustomerService>(MockBehavior.Strict);
        _discountService = new Mock<IDiscountService>(MockBehavior.Strict);

        _invoiceService = new InvoiceService(_invoiceRepository.Object, _customerService.Object, _discountService.Object);
    }

    public void Dispose()
    {
        _invoiceRepository.VerifyAll();
        _customerService.VerifyAll();
        _discountService.VerifyAll();
    }

    [Fact]
    public void Test()
    {
    }

    [Fact]
    public void GetInvoiceNullTest()
    {
        _invoiceRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Invoices, bool>>>()))
            .Returns((Expression<Func<Invoices, bool>> filter) => new List<Invoices>()
            {
                new Invoices()
                {
                    Id = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"),
                    CreatedDateTime = DateTime.Now,
                    CustomerId = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    IsActive = true,
                    IsGroceries = false,
                    IsDeleted = false,
                    TotalAmount = 1000,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        var model = new GetInvoiceRequestModel { InvoiceId = Guid.NewGuid() };
        var result = _invoiceService.GetInvoiceCalculate(model);
        Assert.Null(result);
    }

    [Fact]
    public void GetInvoiceCustomerTest()
    {
        _invoiceRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Invoices, bool>>>()))
            .Returns((Expression<Func<Invoices, bool>> filter) => new List<Invoices>()
            {
                new Invoices()
                {
                    Id = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"),
                    CreatedDateTime = DateTime.Now,
                    CustomerId = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    IsActive = true,
                    IsGroceries = false,
                    IsDeleted = false,
                    TotalAmount = 1000,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        _customerService.Setup(x => x.GetCustomerById(It.IsAny<GetCustomerRequestModel>())).Returns((CustomerDtoModel?)null);

        var model = new GetInvoiceRequestModel { InvoiceId = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a") };
        var result = _invoiceService.GetInvoiceCalculate(model);
        Assert.NotNull(result);
        Assert.Equal(result.Id, Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"));
    }

    [Fact]
    public void GetInvoiceEmployeeCustomer()
    {
        _invoiceRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Invoices, bool>>>()))
            .Returns((Expression<Func<Invoices, bool>> filter) => new List<Invoices>()
            {
                new Invoices()
                {
                    Id = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"),
                    CreatedDateTime = DateTime.Now,
                    CustomerId = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    IsActive = true,
                    IsGroceries = false,
                    IsDeleted = false,
                    TotalAmount = 1000,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        _customerService.Setup(x => x.GetCustomerById(It.IsAny<GetCustomerRequestModel>())).Returns(() => new CustomerDtoModel
        {
            Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
            CustomerType = "Employee",
            CreatedDateTime = DateTime.Now
        });

        _discountService.Setup(x => x.GetDiscountByType(It.IsAny<GetDiscountRequestModel>())).Returns(() => new DiscountDtoModel
        {
            Id = Guid.Parse("0151a1f7-2c1b-4b5b-b6c6-ed40ef023110"),
            Type = "Employee",
            DiscountRate = 30
        });

        var model = new GetInvoiceRequestModel { InvoiceId = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a") };
        var result = _invoiceService.GetInvoiceCalculate(model);
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"), result.Id);
        Assert.Equal(Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"), result.CustomerId);
        Assert.Equal(650, result.NetAmount);
        Assert.Equal(350, result.DiscountAmount);
        Assert.Equal(1000, result.TotalAmount);
        Assert.False(result.IsGroceries);
    }
    
    [Fact]
    public void GetInvoiceAffiliateCustomer()
    {
        _invoiceRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Invoices, bool>>>()))
            .Returns((Expression<Func<Invoices, bool>> filter) => new List<Invoices>()
            {
                new Invoices()
                {
                    Id = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"),
                    CreatedDateTime = DateTime.Now,
                    CustomerId = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    IsActive = true,
                    IsGroceries = false,
                    IsDeleted = false,
                    TotalAmount = 1000,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        _customerService.Setup(x => x.GetCustomerById(It.IsAny<GetCustomerRequestModel>())).Returns(() => new CustomerDtoModel
        {
            Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
            CustomerType = "Affiliate",
            CreatedDateTime = DateTime.Now
        });

        _discountService.Setup(x => x.GetDiscountByType(It.IsAny<GetDiscountRequestModel>())).Returns(() => new DiscountDtoModel
        {
            Id = Guid.Parse("0151a1f7-2c1b-4b5b-b6c6-ed40ef023110"),
            Type = "Affiliate",
            DiscountRate = 10
        });

        var model = new GetInvoiceRequestModel { InvoiceId = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a") };
        var result = _invoiceService.GetInvoiceCalculate(model);
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"), result.Id);
        Assert.Equal(Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"), result.CustomerId);
        Assert.Equal(850, result.NetAmount);
        Assert.Equal(150, result.DiscountAmount);
        Assert.Equal(1000, result.TotalAmount);
        Assert.False(result.IsGroceries);
    }
    
    [Fact]
    public void GetInvoiceOldCustomer()
    {
        _invoiceRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Invoices, bool>>>()))
            .Returns((Expression<Func<Invoices, bool>> filter) => new List<Invoices>()
            {
                new Invoices()
                {
                    Id = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"),
                    CreatedDateTime = DateTime.Now,
                    CustomerId = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    IsActive = true,
                    IsGroceries = false,
                    IsDeleted = false,
                    TotalAmount = 1000,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        _customerService.Setup(x => x.GetCustomerById(It.IsAny<GetCustomerRequestModel>())).Returns(() => new CustomerDtoModel
        {
            Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
            CustomerType = string.Empty,
            CreatedDateTime = DateTime.Now.AddYears(-2)
        });

        _discountService.Setup(x => x.GetDiscountByType(It.IsAny<GetDiscountRequestModel>())).Returns(() => new DiscountDtoModel
        {
            Id = Guid.Parse("0151a1f7-2c1b-4b5b-b6c6-ed40ef023110"),
            Type = "OldCustomer",
            DiscountRate = 5
        });

        var model = new GetInvoiceRequestModel { InvoiceId = Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a") };
        var result = _invoiceService.GetInvoiceCalculate(model);
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("30aefe7a-6363-4be1-87c5-2f3bc632d72a"), result.Id);
        Assert.Equal(Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"), result.CustomerId);
        Assert.Equal(900, result.NetAmount);
        Assert.Equal(100, result.DiscountAmount);
        Assert.Equal(1000, result.TotalAmount);
        Assert.False(result.IsGroceries);
    }
}