using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using ShopsRUs.Core;
using ShopsRUs.Data.Domain;
using ShopsRUs.Services;
using ShopsRUs.Services.Models;
using Xunit;

namespace ShopsRUs.Test;

public class DiscountServiceTest : IDisposable
{
    private readonly Mock<IRepository<Discounts>> _discountRepository;

    private readonly DiscountService _discountService;
    
    public DiscountServiceTest()
    {
        _discountRepository = new Mock<IRepository<Discounts>>(MockBehavior.Strict);
        _discountService = new DiscountService(_discountRepository.Object);
    }
    
    public void Dispose()
    {
        _discountRepository.VerifyAll();
    }

    [Fact]
    public void GetDiscountByTypeTest()
    {
        _discountRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Discounts, bool>>>()))
            .Returns((Expression<Func<Discounts, bool>> filter) => new List<Discounts>()
            {
                new Discounts()
                {
                    Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    CreatedDateTime = DateTime.Now,
                    Type = "Employee",
                    DiscountRate = 30,
                    IsActive = true,
                    IsDeleted = false,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        var result = _discountService.GetDiscountByType(new GetDiscountRequestModel { TypeName = "Employee" });
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),result.Id);
        Assert.Equal("Employee",result.Type);
        Assert.Equal(30,result.DiscountRate);
    }
    
    [Fact]
    public void GetDiscountByTypeNullTest()
    {
        _discountRepository.Setup(s => s.Filter(It.IsAny<Expression<Func<Discounts, bool>>>()))
            .Returns((Expression<Func<Discounts, bool>> filter) => new List<Discounts>()
            {
                new Discounts()
                {
                    Id = Guid.Parse("9d447e33-dcd7-4c91-a92c-f51608a89e80"),
                    CreatedDateTime = DateTime.Now,
                    Type = "Employee",
                    DiscountRate = 30,
                    IsActive = true,
                    IsDeleted = false,
                    UpdatedDateTime = null
                }
            }.Where(filter.Compile()).AsQueryable());

        var result = _discountService.GetDiscountByType(new GetDiscountRequestModel { TypeName = string.Empty });
        Assert.Null(result);
    }
}