using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Models;

namespace ShopsRUs.Services.Interfaces;

public interface IDiscountService
{
    DiscountDtoModel? GetDiscountByType(GetDiscountRequestModel request);
}