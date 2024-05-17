using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service;

public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;

    public CouponService(IBaseService baseService)
    {
        _baseService = baseService;
    }


    public async Task<ResponseDto?> GetCouponByCodeAsync(string couponCode)
    {
        return await _baseService.SendAsyncFr(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = CouponAPIBase+"/api/coupon/GetByCode/"+couponCode
        });
    }

    public async Task<ResponseDto?> GetAllCouponsAsync()
    {
        return await _baseService.SendAsyncFr(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = CouponAPIBase+"/api/coupon"
        });
    }

    public async Task<ResponseDto?> GetCouponByIdAsync(int id)
    {
        return await _baseService.SendAsyncFr(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = CouponAPIBase+"/api/coupon/"+id
        });
    }

    public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsyncFr(new RequestDto()
        {
            ApiType = ApiType.POST,
            Url = CouponAPIBase+"/api/coupon",
            Data = couponDto
        });
    }

    public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsyncFr(new RequestDto()
        {
            ApiType = ApiType.PUT,
            Url = CouponAPIBase+"/api/coupon",
            Data = couponDto
        });
    }

    public async Task<ResponseDto?> DeleteCouponAsync(int id)
    {
        return await _baseService.SendAsyncFr(new RequestDto()
        {
            ApiType = ApiType.DELETE,
            Url = CouponAPIBase+"/api/coupon/"+id
        });
    }
}