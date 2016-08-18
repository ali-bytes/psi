using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Services.Discounts{
    public class ResellerDiscountCalculator{
        readonly ISPDataContext _context;


        public ResellerDiscountCalculator(ISPDataContext context){
            _context = context;
        }


        public decimal CalculateDiscount(int resellerId, int providerId, int packageId){
            var resellerPackagesDiscount = _context.ResellerPackagesDiscounts.FirstOrDefault(x => x.PackageId == packageId &&
                                                                                                  x.ResellerId == resellerId && x.ProviderId == providerId
                );
            if(resellerPackagesDiscount == null){
                return 0;
            }
            return resellerPackagesDiscount.Discount;
        }


        public DiscountData CalculateDiscount(int resellerId, int providerId, int packageId, decimal amount){
            var calculateDiscount = CalculateDiscount(resellerId, providerId, packageId);
            var discountPrice = amount * calculateDiscount / 100;
            return new DiscountData{
                Original = amount,
                DiscountPercent = calculateDiscount,
                Discount = discountPrice,
                Net = amount - discountPrice
            };
        }
    }
}
