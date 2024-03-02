using Mango.Web.Models;

namespace Mango.Web.Service;

public interface ICouponService
{
    Task<ResponseDto?> GetCouponAsync(string couponId);
    Task<ResponseDto?> GetAllCouponsAsync();
    Task<ResponseDto?> GetCouponByIdAsync(int id);
    Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);
    Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
    Task<ResponseDto?> DeleteCouponAsync(int id);
}
