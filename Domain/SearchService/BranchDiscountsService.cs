using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Domain.SearchService{
    public class BranchDiscountsService{
        readonly ISPDataContext _context;


        public BranchDiscountsService(ISPDataContext context){
            _context = context;
        }


        public List<BranchPackagesDiscount> BranchDiscounts(int branchId, int providerId){
            var provider = _context.ServiceProviders.FirstOrDefault(p => p.ID == providerId);
            var branch = _context.Branches.FirstOrDefault(u => u.ID == branchId);
            if(branch == null || provider == null) return null;
            var packagesDiscounts = _context.BranchPackagesDiscounts.Where(x => x.BranchId == branchId).ToList();
            var packages = provider.ServicePackages.ToList();
            var fakes = new List<BranchPackagesDiscount>();
            foreach(var package in packages){
                if(packagesDiscounts.Any(x => x.PackageId == package.ID)){
                    var currentDiscount = packagesDiscounts.FirstOrDefault(x => x.PackageId == package.ID);
                    if(currentDiscount != null) fakes.Add(currentDiscount);
                    continue;
                }
                fakes.Add(new BranchPackagesDiscount{
                    Discount = 0,
                    ServicePackage = package,
                    ServiceProvider = provider,
                    Branch = branch,
                    Id = 0
                });
            }
            return fakes;
        }


        public static BranchPackagesDiscountModel To(BranchPackagesDiscount input){
            return new BranchPackagesDiscountModel{
                Discount = Helper.FixNumberFormat(input.Discount),
                Notes = input.Notes,
                Package = input.ServicePackage.ServicePackageName,
                Provider = input.ServiceProvider.SPName,
                Branch = input.Branch.BranchName,
                ProviderId = input.ServiceProvider.ID,
                PackageId = input.ServicePackage.ID,
                BranchId=input.BranchId
            };
        }


        public static List<BranchPackagesDiscountModel> To(List<BranchPackagesDiscount> inputs){
            return inputs.Select(To).ToList();
        }


        public BranchPackagesDiscount GetBranchPackageDiscount(int branchId, int providerId, int packageId){
            return _context.BranchPackagesDiscounts.FirstOrDefault(x => x.BranchId == branchId && x.ProviderId == providerId && x.PackageId == packageId);
        }


        public decimal GetBranchPackageDiscountPercent(int branchId, int providerId, int packageId, bool divideBy100 = false){
            var discount = _context.BranchPackagesDiscounts
                .FirstOrDefault(x =>
                    x.BranchId == branchId &&
                    x.ProviderId == providerId &&
                    x.PackageId == packageId);
            if(discount == null){
                return 0;
            }
            if(divideBy100){
                return discount.Discount / 100;
            }
            return discount.Discount;
        }



        public void SaveDiscount(int resellerId, int providerId, int packageId, decimal discount){
            BranchPackagesDiscount oldDiscount = GetBranchPackageDiscount(resellerId, providerId, packageId);
            if(oldDiscount != null){
                oldDiscount.Discount = discount;
            } else{
                var disc = CreateBranchPackageDiscount(resellerId, providerId, packageId, discount);
                _context.BranchPackagesDiscounts.InsertOnSubmit(disc);
            }
        }


        static BranchPackagesDiscount CreateBranchPackageDiscount(int branchId, int providerId, int packageId, decimal discount){
            return new BranchPackagesDiscount{
                Discount = discount,
                PackageId = packageId,
                BranchId = branchId,
                ProviderId = providerId,
            };
        }


        public void Commit(){
            _context.SubmitChanges();
        }
    }
}