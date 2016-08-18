using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Services.Discounts{
    public class BranchDiscountCalculator{
        readonly ISPDataContext _context;


        public BranchDiscountCalculator(ISPDataContext context)
        {
            _context = context;
        }


        public decimal CalculateDiscount(int branchId, int providerId, int packageId){
            var branchPackagesDiscount = _context.BranchPackagesDiscounts
                .FirstOrDefault(x => x.PackageId == packageId && x.BranchId == branchId && x.ProviderId == providerId
                );
            if(branchPackagesDiscount == null){
                return 0;
            }
            return branchPackagesDiscount.Discount;
        }


        public DiscountData CalculateDiscount(int branchId, int providerId, int packageId, decimal amount){
            var calculateDiscount = CalculateDiscount(branchId, providerId, packageId);
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