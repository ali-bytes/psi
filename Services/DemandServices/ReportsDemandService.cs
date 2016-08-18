using System;
using System.Collections.Generic;
using System.Linq;
using BL.Concrete;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Models;
using NewIspNL.Services.OfferServices;
using Resources;

namespace NewIspNL.Services.DemandServices{
    public class ReportsDemandService{
        readonly IspEntries _ispEntries;


        public ReportsDemandService(IspEntries ispEntries){
            _priceServices = new PriceServices();
            _ispEntries = ispEntries;
        }


        public virtual DemandsReportContainer UpdateRequestDate(int orderId,DateTime time, int fromStatusId, int daysCount=0, bool previewOnly = false,int postponed=0){
            var demandsReportContainer = new DemandsReportContainer{
                After = new List<Demand>()
            };
            var request = _ispEntries.GetLastRequest(fromStatusId, orderId);
            if(request == null) return demandsReportContainer;
            if(request.UpdateDate == null) return demandsReportContainer;
           
            var order = _ispEntries.GetWorkOrder(orderId);
            if(order == null) return demandsReportContainer;
            var days = /*(time.Date - request.UpdateDate.Value.Date).Days*/postponed;
           // var demand = _ispEntries.DemandsbyOrderId(orderId).OrderByDescending(x => x.Id).FirstOrDefault();
            var dem = _ispEntries.OrderDemand2(orderId, time).ToList();
            var demand2 = dem.LastOrDefault();
            if(demand2 != null){
                var endAt = demand2.EndAt.AddDays(days);
                demand2.EndAt = endAt;
                demand2.Notes = string.Empty;
                order.RequestDate = endAt;
                demandsReportContainer.Title = Tokens.PostponeSuspendDays;
                if(daysCount>0){
                    demandsReportContainer.Title = Tokens.DeductionWithFixedRequestDate +" - "+ Tokens.DaysCount+" ("+daysCount+")";
                    
                    const int monthDays = 30; //DateTime.DaysInMonth(demand.StartAt.Year, demand.StartAt.Month);
                    var billDefault = _priceServices.BillDefault(order, demand2.StartAt.Month, demand2.StartAt.Year, null);
                    if (billDefault != null)
                    {
                        decimal deductionAmount;
                        var billAmount = billDefault.Net;
                        if (order.OfferId != null)
                        {
                            var offerPrice = OfferPricingServices.GetOfferPrice(order.Offer, billAmount, billAmount);
                            var netRequired = billAmount - offerPrice;
                            deductionAmount = netRequired*daysCount/monthDays;
                        }
                        else
                        {
                            deductionAmount = billAmount*daysCount/monthDays;
                        }
                        demand2.Amount -= deductionAmount;
                    }
                }
            }
               
            if(!previewOnly)_ispEntries.Commit();
            //if(demand == null) return demandsReportContainer;
            //demand.WorkOrder = order;
           // demandsReportContainer.After.Add(DemandFactory.CreateNewDemandObject(demand));

            return demandsReportContainer;
        }


        readonly PriceServices _priceServices;
    }

    
}
