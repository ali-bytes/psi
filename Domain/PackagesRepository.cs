using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Domain{
    public class PackagesRepository{
        readonly ISPDataContext _context;

        readonly Loc _loc;


        public PackagesRepository(ISPDataContext context){
            _context = context;
            _loc = new Loc();
        }


       /* public List<ServicePackage> Packages(){
            return _context.ServicePackages.ToList();
        }*/


        public ServicePackage GetPackage(int id){
            return _context.ServicePackages.FirstOrDefault(x => x.ID == id);
        }


        public List<ServicePackage> ProviderPackages(int providerId, bool activeOnly = false){
            if(activeOnly){
                var packages = _context.ServicePackages.Where(x => x.ProviderId == providerId).Where(x => x.Active == null || x.Active.Value).ToList();
                return packages;
            }
            return _context.ServicePackages.Where(x => x.ProviderId == providerId).ToList();
        }


        public void Save(ServicePackage package){
            if(package.ID == 0){
                _context.ServicePackages.InsertOnSubmit(package);
            }
            _context.SubmitChanges();
        }


        public List<PackagePreview> ToPreview(bool activeOnly = false){
            var packages = _context.ServicePackages.OrderBy(x => x.ServiceProvider.SPName).ThenBy(x => x.ServicePackageTypeID).ToList();
            if(activeOnly)
                packages = packages.Where(x => x.Active == null || x.Active.Value).ToList();
            return packages.Select(ToPackagePreview).ToList();
        }


        public PackagePreview ToPackagePreview(ServicePackage x){
            var price = GetPrice(x.ProviderId, x.ID);
            var packagePreview = new PackagePreview{
                ID = x.ID,
                Type = x.ServicePackagesType.SPTName,
                Name = x.ServicePackageName,
                Provider = x.ServiceProvider.SPName,
                CanDelete = CanDelete(x),
                Notes = x.Notes,
                Price = price,
                TPrice = Helper.FixNumberFormat(price),
                Active = x.Active == null || x.Active.Value ? _loc.IterateResource("true") : _loc.IterateResource("false"),
                PurchasePrice=Helper.FixNumberFormat(x.PurchasePrice)
            };

            return packagePreview;
        }


        public double GetPrice(int providerId, int packageId){
            var pricing = GetPricing(providerId, packageId);
            if(pricing == null) return 0;
            return pricing.Price == null ? 0 : pricing.Price.Value;
        }


        public Pricing GetPricing(int providerId, int packageId){
            return _context.Pricings.FirstOrDefault(x => x.ServicePackagesID == packageId && x.ServiceProvidersID == providerId);
        }


        /*public void UpdatePricing(Pricing pricing, double price){
            pricing.Price = price;
            _context.SubmitChanges();
        }*/


        public void SavePricing(Pricing pricing){
            if(pricing.ID == 0){
                _context.Pricings.InsertOnSubmit(pricing);
            }
            _context.SubmitChanges();
        }


        public bool CanDelete(ServicePackage package){
            if(package.WorkOrderRequests.Any()){
                return false;
            }

            if(package.WorkOrderRequests1.Any()){
                return false;
            }

            return !package.WorkOrders.Any();
        }



        public bool Delete(ServicePackage package){
            if(!CanDelete(package)) return false;
            try
            {
                _context.Pricings.DeleteAllOnSubmit(package.Pricings);
                _context.ResellerPackagesDiscounts.DeleteAllOnSubmit(package.ResellerPackagesDiscounts);
                _context.BranchPackagesDiscounts.DeleteAllOnSubmit(package.BranchPackagesDiscounts);
                _context.SubmitChanges();
                _context.ServicePackages.DeleteOnSubmit(package);
                _context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public List<ServiceProvider> Providers(){
            return _context.ServiceProviders.ToList();
        }


        public List<ServicePackagesType> PackageTypes(){
            return _context.ServicePackagesTypes.ToList();
        }
    }
}
