using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Domain.SearchService{
    public class ResellerDiscountsService{
        readonly ISPDataContext _context;


        public ResellerDiscountsService(ISPDataContext context){
            _context = context;
        }


        public List<ResellerPackagesDiscount> ResellerDiscounts(int resellerId, int providerId){
            var provider = _context.ServiceProviders.FirstOrDefault(p => p.ID == providerId);
            var reseller = _context.Users.FirstOrDefault(u => u.ID == resellerId);
            if(reseller == null || provider == null) return null;
            var packagesDiscounts = _context.ResellerPackagesDiscounts.Where(x => x.ResellerId == resellerId).ToList();
            var packages = provider.ServicePackages.ToList();
            var fakes = new List<ResellerPackagesDiscount>();
            foreach(var package in packages){
                if(packagesDiscounts.Any(x => x.PackageId == package.ID)){
                    var currentDiscount = packagesDiscounts.FirstOrDefault(x => x.PackageId == package.ID);
                    if(currentDiscount != null) fakes.Add(currentDiscount);
                    continue;
                }
                fakes.Add(new ResellerPackagesDiscount{
                    Discount = 0,
                    ServicePackage = package,
                    ServiceProvider = provider,
                    User = reseller,
                    Id = 0
                });
            }
            return fakes;
        }


        public static ResellerPackagesDiscountModel To(ResellerPackagesDiscount input){
            return new ResellerPackagesDiscountModel{
                Discount = Helper.FixNumberFormat(input.Discount),
                Notes = input.Notes,
                Package = input.ServicePackage.ServicePackageName,
                Provider = input.ServiceProvider.SPName,
                Reseller = input.User.UserName,
                ResellerId = input.User.ID,
                ProviderId = input.ServiceProvider.ID,
                PackageId = input.ServicePackage.ID
            };
        }


        public static List<ResellerPackagesDiscountModel> To(List<ResellerPackagesDiscount> inputs){
            return inputs.Select(To).ToList();
        }


        public ResellerPackagesDiscount GetResellerPackageDiscount(int resellerId, int providerId, int packageId){
            return _context.ResellerPackagesDiscounts.FirstOrDefault(x => x.ResellerId == resellerId && x.ProviderId == providerId && x.PackageId == packageId);
        }


        public decimal GetResellerPackageDiscountPercent(int resellerId, int providerId, int packageId, bool divideBy100 = false){
            var discount = _context.ResellerPackagesDiscounts.FirstOrDefault(x => x.ResellerId == resellerId && x.ProviderId == providerId && x.PackageId == packageId);
            if(discount == null){
                return 0;
            }
            if(divideBy100){
                return discount.Discount / 100;
            }
            return discount.Discount;
        }



        public void SaveDiscount(int resellerId, int providerId, int packageId, decimal discount){
            var oldDiscount = GetResellerPackageDiscount(resellerId, providerId, packageId);
            if(oldDiscount != null){
                oldDiscount.Discount = discount;
            } else{
                var disc = CreateResellerPackageDiscount(resellerId, providerId, packageId, discount);
                _context.ResellerPackagesDiscounts.InsertOnSubmit(disc);
            }
        }


        static ResellerPackagesDiscount CreateResellerPackageDiscount(int resellerId, int providerId, int packageId, decimal discount){
            return new ResellerPackagesDiscount{
                Discount = discount,
                PackageId = packageId,
                ResellerId = resellerId,
                ProviderId = providerId,
            };
        }


        public void Commit(){
            _context.SubmitChanges();
        }
    }
}
