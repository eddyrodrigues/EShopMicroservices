using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> logger;
        private readonly DiscountContext dbContext;

        public DiscountService(ILogger<DiscountService> logger, DiscountContext context)
        {
            this.logger = logger;
            this.dbContext = context;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext
                .Coupons
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            //Coupon coupon = null;
            if (coupon is null)
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {

            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalida Request"));
            }

            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();


            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalida Request"));
            }

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var couponName = request.ProductName;

            var coupon = await dbContext
               .Coupons
               .FirstOrDefaultAsync(x => x.ProductName == couponName);
            
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon Not Found / Discount Not Found"));
            }

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse() {
                Success= true
            };
        }
    }
}
