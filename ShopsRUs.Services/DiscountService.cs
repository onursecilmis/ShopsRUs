using ShopsRUs.Core;
using ShopsRUs.Data.Domain;
using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Interfaces;
using ShopsRUs.Services.Models;

namespace ShopsRUs.Services;

public class DiscountService : IDiscountService
{
    private readonly IRepository<Discounts> _discountRepository;

    public DiscountService(IRepository<Discounts> discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public DiscountDtoModel? GetDiscountByType(GetDiscountRequestModel request)
    {
        var discount = _discountRepository.Filter(x => x.IsActive && x.Type.ToLower() == request.TypeName.ToLower()).Select(x => new DiscountDtoModel
        {
            Id = x.Id,
            Type = x.Type,
            DiscountRate = x.DiscountRate
        }).FirstOrDefault();

        return discount;
    }
}