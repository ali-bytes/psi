using System;
using System.Collections.Generic;
using System.Linq;
using BL.Concrete;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.OfferServices;
using Resources;

namespace NewIspNL.Services.DemandServices{
    public class CancelProcessDemandService{
        readonly DemandFactory _demandFactory;

        

        readonly PriceServices _priceServices;


        public CancelProcessDemandService( DemandFactory demandFactory){
            _demandFactory = demandFactory;
            _priceServices = new PriceServices();
        }


        public DemandsReportContainer HandleCancelRequest(int orderId, DateTime date, int userId, int workOrderStatusId, bool previewOnly = false, int cancelOption = -1){
           var ispEntries = new IspEntries();
            var container = new DemandsReportContainer{
                After = new List<Demand>()
            };
            var order = ispEntries.GetWorkOrder(orderId);
         //   var demand = ispEntries.OrderDemand(orderId).Where(a=>a.Amount!=30).OrderByDescending(x => x.Id).FirstOrDefault();
            List<Demand> dem;
            Demand demand2 = null;

            if (workOrderStatusId == 6)
            {
                dem = ispEntries.OrderDemand2Withoutdate(orderId).ToList();
            
            demand2 = dem.LastOrDefault(x=>x.Paid != true);
            }
            if (workOrderStatusId == 11)
            {
                 dem = ispEntries.OrderDemand2Withoutdate(orderId).ToList();

                var demand4 = dem.LastOrDefault();
       


                if (dem.Count == 0)
                { return container; }
                if (demand4 != null && demand4.Notes == "غرامة 30 جنية لان عدد ايام السسبند اصبح 90 يوم")
                {
                    var lenght = Convert.ToInt32(dem.Count());
                    if (lenght <= 1)
                    {

                        return container;
                    }
                    demand2 = dem[lenght - 2];

                }
                else
                {
                    demand2 = dem.LastOrDefault();
                }

            }
            if (demand2 == null || order == null) return container;
           

            var startAt = demand2.StartAt;
            var billDefault = _priceServices.CustomerInvoiceDetailsDefault(order, startAt.Month, startAt.Year);//BillDefault(order, startAt.Month, startAt.Year, null);
            if (billDefault == null) return container;
            var basicBill = billDefault.Net;
            var offer = order.Offer;
            var spentDays = (date.Date - demand2.StartAt.Date).Days + 1;
           if(offer!=null) basicBill -= OfferPricingServices.GetOfferPrice(offer, basicBill,spentDays);
            var orginialDemandEndDate = new DateTime(startAt.Year, startAt.Month, startAt.Day);
            const int daysInMonth = 30; //DateTime.DaysInMonth(startAt.Year, startAt.Month);
            var oldPaid = demand2.Paid;
          
            
            if(workOrderStatusId == 6 ){
                
                var netRequired = basicBill * Convert.ToDecimal(spentDays) / Convert.ToDecimal(daysInMonth);
                demand2.EndAt = date;
                
                order.RequestDate = date;
               
                container.Title = Tokens.FilterCustomerInvoice;

                var oldAmount = demand2.Amount;
                netRequired += Convert.ToDecimal(order.PaymentType.Amount);
                if (cancelOption == 0) {
                    if (!oldPaid) demand2.Amount = netRequired;
                }
                if (cancelOption == 1 ){
                    container.Title = Tokens.CancelDept;
                    if(!oldPaid)demand2.Amount = 0;
                }
                if(cancelOption==2){
                    container.Title = Tokens.PayDept;
                    if (!oldPaid) demand2.Amount = netRequired;
                    demand2.Paid = true;
                }
                if (cancelOption == 3) {
                    container.Title = Tokens.CompleteInvoice;
                }
                if(!previewOnly)ispEntries.Commit();
                if(previewOnly)container.After.Add(_demandFactory.ToFakeDemand(demand2, order));
                if (!oldPaid) return container;
                var amo = (netRequired - oldAmount);
                var customerRemainingAmountDemand = _demandFactory.CreateDemand(order, date.AddDays(1), orginialDemandEndDate,amo ,userId);
                if(!previewOnly){
                    order.RequestDate = date;
                    ispEntries.AddDemands(customerRemainingAmountDemand);
                    ispEntries.Commit();
                }
                if(previewOnly){
                    container.After.Add(_demandFactory.ToFakeDemand(customerRemainingAmountDemand, demand2.WorkOrder));
                }
                return container;
            }

            if(workOrderStatusId == 11)
            {


              
                //var lastSuspendRequest = ispEntries.GetLastOrderRequest(6, order.ID);
                var lastSuspendStatus = ispEntries.LastWorkOrderStatus(order.ID, 11, null, null);
                if(lastSuspendStatus == null || lastSuspendStatus.UpdateDate == null) return container;
               // var activationStatusB4Suspend = ispEntries.LastWorkOrderStatus(order.ID, 6, null, null);
                //if(activationStatusB4Suspend == null || activationStatusB4Suspend.UpdateDate == null) return container;
                var suspendDate = lastSuspendStatus.UpdateDate.Value.Date;
                var period = (suspendDate - demand2.StartAt).Days+1;
                if (period < 0) period = 0;
                //if(lastSuspendRequest == null || lastSuspendRequest.ProcessDate == null) return container;
                var spent = basicBill * Convert.ToDecimal(period) / Convert.ToDecimal(daysInMonth);
                spent += Convert.ToDecimal(order.PaymentType.Amount);
                demand2.EndAt = suspendDate;
                /*if(!demand.Paid){
                    demand.Amount = spent;
                }*/
                var oldAmount = demand2.Amount;
                container.Title = Tokens.FilterCustomerInvoice;
                if (cancelOption == 0){
                    if (!oldPaid) demand2.Amount = spent;
                }
                if (cancelOption == 1){
                    container.Title = Tokens.CancelDept;
                    if (!demand2.Paid)demand2.Amount = 0;
                }
                if (cancelOption == 2){
                    container.Title = Tokens.PayDept;
                    if (!oldPaid) demand2.Amount = spent;
                    demand2.Paid = true;
                }
                if (cancelOption == 3) {
                    container.Title = Tokens.CompleteInvoice;
                }

                if(!previewOnly){
                    //demand.WorkOrder.RequestDate = suspendDate;
                    ispEntries.Commit();
                }
                if(previewOnly)container.After.Add(_demandFactory.ToFakeDemand(demand2, order));
                if (!demand2.Paid) return container;
                var netRequired = (spent - oldAmount);// basicBill;
                var customerRemainingAmountDemand = _demandFactory.CreateDemand(order, suspendDate, DateTime.Now.AddHours(), netRequired,userId);
                if(previewOnly) container.After.Add(_demandFactory.ToFakeDemand(customerRemainingAmountDemand, order));
                else{
                    ispEntries.AddDemands(customerRemainingAmountDemand);
                    //demand.WorkOrder.RequestDate = customerRemainingAmountDemand.EndAt;
                    ispEntries.Commit();
                }
            }
            return container;
        }
    }
}
