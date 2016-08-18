using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.SearchService;
using NewIspNL.Services.OfferServices;

namespace NewIspNL.Domain.Concrete{
    public class PriceServices {
        readonly ISPDataContext _context;

        readonly IOfferRepository _offerRepository;

        readonly IWorkOrderRepository _workOrderRepository;
        readonly ResellerDiscountsService _resellerDiscountsService;

        public PriceServices(){
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _resellerDiscountsService = new ResellerDiscountsService(_context);
                _offerRepository = new OfferRepository();
                _workOrderRepository = new WorkOrderRepository();
            

        }


        #region IPriceServices Members


        public Pricing GetPrice(int servicePackageId, int serviceProviderId,bool redirect=true){
            var price =
                _context.Pricings.FirstOrDefault(
                                                 p => p.ServicePackagesID == servicePackageId && p.ServiceProvidersID == serviceProviderId);
            if(price != null){
                return price;
            }
            if(redirect){
                if(HttpContext.Current != null){
                    var provider = _context.ServiceProviders.FirstOrDefault(p => p.ID == serviceProviderId);
                    var package = _context.ServicePackages.FirstOrDefault(p => p.ID == servicePackageId);


                    if(HttpContext.Current != null){
                        HttpContext.Current.Response.Redirect(
                                                              string.Format("~/Pages/PriceNotFound.aspx?Prov={0}&Pac={1}"
                                                                  , provider == null ? "Unknown Provider" : provider.SPName
                                                                  , package == null ? "Unknow package" : package.ServicePackageName)
                            );
                    }
                }
            }
            return null;
        }


        public decimal CustomerInvoiceDefault(WorkOrder order, int month, int year){
            var net = new decimal();
            if(order.ServicePackageID == null || order.ServiceProviderID == null) return net;
            var packagePrice = GetPrice(order.ServicePackageID.Value, order.ServiceProviderID.Value);
            if(packagePrice == null){
                return net;
            }

            if(packagePrice.Price == null) return net;

            net = Convert.ToDecimal(packagePrice.Price.Value);

            if(order.Offer != null){
                var activation = _workOrderRepository.GetActivationDate(order.ID);
                if(activation != null){
                    var date = new DateTime(year, month, 1);
                    var offerEnd = activation.Value.AddMonths(order.Offer.LifeTime);
                    if(date.Date >= activation.Value.Date && date.Date < offerEnd.Date){
                        if(order.OfferId != null){
                            var discount = _offerRepository.GetServicePackageDiscount(order.OfferId.Value, order.ServicePackageID.Value);
                            net -= discount;
                        }
                    }
                }
            }


           
            return net;
        }

       
        public BillItem BillDefault(WorkOrder order, int month, int year, DateTime ? activationDate,bool redirect=true)
        {
            var activation = _workOrderRepository.GetActivationDate(order.ID);
            if (activation == null) return null;
            var details = CustomerInvoiceDetailsDefault(order, month, year,redirect);
            var isp = new IspDomian(_context);


            var day = Math.Min(DateTime.DaysInMonth(year, month), activation.Value.Day);
            var startDate = new DateTime(year, month, day);
            var endDate = startDate.AddMonths(1);
            var bill = new BillItem{
                Discount = details.Discount,
                Central = order.Central == null ? string.Empty : order.Central.Name,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                EndDate = endDate,
                StartDate = startDate,
                TEndDate = endDate.ToShortDateString(),
                TStartDate = startDate.ToShortDateString(),
                MainPrice = details.MainPrice,
                Net = details.Net,
                Price = details.Price,
                PriceAfterDisc = details.PriceAfterDisc,
                ActivationDate = activationDate == null ? string.Empty : activationDate.Value.ToShortDateString(),
                Branch = order.Branch == null ? string.Empty : order.Branch.BranchName,
                GovernorateName = order.Governorate.GovernorateName,
                Offer = order.Offer == null ? string.Empty : order.Offer.Title,
                Reseller = isp.GetReseller(order.ResellerID),
                ResellerDiscount = details.ResellerDiscount,
                SpName = order.ServiceProvider == null ? string.Empty : order.ServiceProvider.SPName,
                ServicePackageName = order.ServicePackage == null ? string.Empty : order.ServicePackage.ServicePackageName,
                StatusName = order.Status == null ? string.Empty : order.Status.StatusName
            };
            return bill;
        }


        public BillDetails CustomerInvoiceDetailsDefault(WorkOrder order, int month, int year,bool redirect=true){
            var details = new BillDetails{
                Discount = 0,
                MainPrice = 0,
                Net = 0,
                Price = 0,
                PriceAfterDisc = 0,
                ResellerDiscount = 0
            };
            if(order.ServicePackageID == null || order.ServiceProviderID == null) return details;
            var packagePrice = GetPrice(order.ServicePackageID.Value, order.ServiceProviderID.Value,redirect);
            if(packagePrice == null){
                return details;
            }

            if(packagePrice.Price == null) return details;
            details.Price = Convert.ToDouble(packagePrice.Price);
            details.MainPrice = Convert.ToDecimal(packagePrice.Price);
            details.Net = Convert.ToDecimal(packagePrice.Price.Value);

            if(order.Offer != null){
                HandleDiscount(order, month, year, details);
            }

           
            //var resellerDisc = _context.ResellersDiscounts.FirstOrDefault(r => r.ResellerID == order.ResellerID && r.ServiceProviderID == order.ServiceProviderID && r.ServicePackagesTypeID == order.ServicePackage.ServicePackageTypeID);
           /* if (order.ResellerID != null){
                var discount = _resellerDiscountsService.GetResellerPackageDiscountPercent(order.ResellerID.Value, order.ServiceProviderID.Value, order.ServicePackageID.Value);
                var resDiscount = details.Net * discount / 100;
                details.ResellerDiscount = resDiscount;
                details.Net = details.Net - resDiscount;
            }*/
            return details;
        }


        #endregion


        void HandleDiscount(WorkOrder order, int month, int year, BillDetails details){
            var activation = _workOrderRepository.GetActivationDate(order.ID);
            if(activation == null) return;
            if(order.Offer == null) return;
            var monthDays = DateTime.DaysInMonth(year, month);
            if (order.OfferStart == null)return;
                var nextMonth = new DateTime(year, month, Math.Min(order.OfferStart.Value.Day, monthDays));
                /*var dt = new DateTime(activation.Value.Year, activation.Value.Month, 1);
            dt = dt.AddMonths(order.Offer.LifeTime);
            dt = dt.AddDays(-1);*/
                var offerEnd = order.OfferStart.Value.AddMonths(order.Offer.LifeTime);
                var dayOfCalculation = new DateTime(year, month, nextMonth.Day).Date;
                if (dayOfCalculation < order.OfferStart.Value.Date || dayOfCalculation.Date >= offerEnd.Date)
                {
                    return;
                }


            if(order.OfferId == null) return;
            if(order.ServicePackageID == null) return;
            var servicePackageDiscount = _offerRepository.GetServicePackageDiscount(order.OfferId.Value, order.ServicePackageID.Value);
            var discount = servicePackageDiscount;
            details.Discount = discount;
            details.Net -= discount;
        }


        static BillDetails CreateBillDetails(){
            return new BillDetails{
                Discount = 0,
                MainPrice = 0,
                Net = 0,
                Price = 0,
                PriceAfterDisc = 0,
                ResellerDiscount = 0
            };
        }


        static void HandlePriceMainPriceNet(Pricing packagePrice, BillDetails details){
            if (packagePrice.Price != null){
                details.Price = Convert.ToDouble(packagePrice.Price);
                details.MainPrice = Convert.ToDecimal(packagePrice.Price);
                details.Net = Convert.ToDecimal(packagePrice.Price.Value);
            }
        }


        void CalculateOfferAndResellerDiscount(WorkOrder order, int month, int year, BillDetails details){
            if(order.Offer != null){
                var activation = _workOrderRepository.GetActivationDate(order.ID);
                if(activation != null){
                    var date = new DateTime(year, month, 1);
                    //if(order.Offer.LifeTime != null){
                        var offerEnd = activation.Value.AddMonths(order.Offer.LifeTime);
                        if(date.Date >= activation.Value.Date && date.Date < offerEnd.Date){
                            if(order.OfferId != null&&order.ServicePackageID!=null)
                            {
                                var discount = OfferPricingServices.GetOfferPrice(order.Offer, details.Net, details.Net);//_offerRepository.GetServicePackageDiscount(order.OfferId.Value, order.ServicePackageID.Value);
                                details.Discount = discount;
                                details.Net -= discount;
                                //net -= discount;
                            }
                        }
                    //}
                }
            }


            if (order.ResellerID != null && order.ServiceProviderID!=null && order.ServicePackageID!=null){
                var discount = _resellerDiscountsService.GetResellerPackageDiscountPercent(order.ResellerID.Value, order.ServiceProviderID.Value, order.ServicePackageID.Value);
                var resDiscount = details.Net * discount / 100;
                details.ResellerDiscount = resDiscount;
                details.Net = details.Net - resDiscount;
            }
        }


        public BillDetails CustomerInvoiceDetailsDefaultbByPackage(WorkOrder order, int month, int year, int packageId){
            var details = CreateBillDetails();
            var package = _context.ServicePackages.FirstOrDefault(p => p.ID == packageId);
            details.PackageName = package == null ? "" : package.ServicePackageName;
            if(order.ServicePackageID == null || order.ServiceProviderID == null) return details;
            var packagePrice = GetPrice(packageId, order.ServiceProviderID.Value);
            if(packagePrice == null) return details;
            if(packagePrice.Price == null) return details;
            HandlePriceMainPriceNet(packagePrice, details);
           CalculateOfferAndResellerDiscount(order, month, year, details);
            return details;
        }
        public BillDetails CustomerInvoiceDetailsByPackageWithoutResellerDiscount(WorkOrder order, int month, int year, int packageId)
        {
            var details = CreateBillDetails();
            var package = _context.ServicePackages.FirstOrDefault(p => p.ID == packageId);
            details.PackageName = package == null ? "" : package.ServicePackageName;
            if (order.ServicePackageID == null || order.ServiceProviderID == null) return details;
            var packagePrice = GetPrice(packageId, order.ServiceProviderID.Value);
            if (packagePrice == null) return details;
            if (packagePrice.Price == null) return details;
            HandlePriceMainPriceNet(packagePrice, details);
            return details;
        }
    }
}
