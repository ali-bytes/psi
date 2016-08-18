using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Domain.SearchService{
    public class DemandSearch{

        public List<Demand> SearchDemandsByPaid(BasicSearchModel model,ISPDataContext context){
            
                var demands = context.Demands.Where(x => x.Paid == model.Paid).ToList();
                if(model.BranchId != null)
                    demands = demands.Where(x => x.WorkOrder.BranchID == model.BranchId.Value).ToList();

                if(model.GovernorateId != null)
                    demands = demands.Where(x => x.WorkOrder.CustomerGovernorateID == model.GovernorateId.Value).ToList();

                if(model.ResellerId != null)
                    demands = demands.Where(x => x.WorkOrder.ResellerID == model.ResellerId.Value).ToList();

                if(model.GovernorateId != null)
                    demands = demands.Where(x => x.WorkOrder.CustomerGovernorateID == model.GovernorateId.Value).ToList();

                if(model.CentralId != null)
                    demands = demands.Where(x => x.WorkOrder.CentralId == model.CentralId.Value).ToList();

                if(model.From != null)
                    demands = demands.Where(x => x.StartAt.Date >= model.From.Value.Date).ToList();

                if(model.To != null)
                    demands = demands.Where(x => x.StartAt.Date <= model.To.Value.Date).ToList();

                if(model.PaymentTypeId != null)
                    demands = demands.Where(x => x.WorkOrder.PaymentTypeID == model.PaymentTypeId).ToList();
                return demands;
            
        }


        public List<Demand> SearchDemandsNonModeled(BasicSearchModel model, ISPDataContext context1)
        {
            //var context1 = new ISPDataContext();
                var demands = context1.Demands.Where(d => d.Paid).ToList();
                if(model.BranchId != null)
                    demands = demands.Where(x => x.WorkOrder.BranchID == model.BranchId.Value).ToList();

                if(model.GovernorateId != null)
                    demands = demands.Where(x => x.WorkOrder.CustomerGovernorateID == model.GovernorateId.Value).ToList();

                if(model.ResellerId != null)
                    demands = model.ResellerId != 0 ? demands.Where(x => x.WorkOrder.ResellerID == model.ResellerId.Value).ToList() : demands.Where(x => x.WorkOrder.ResellerID == null).ToList();

            if (model.PaymentTypeId != null)
                demands = demands.Where(x => x.WorkOrder.PaymentTypeID == model.PaymentTypeId.Value).ToList();

            if(model.GovernorateId != null)
                    demands = demands.Where(x => x.WorkOrder.CustomerGovernorateID == model.GovernorateId.Value).ToList();

            if (model.ProviderId != null)
                demands = demands.Where(x => x.WorkOrder.ServiceProviderID == model.ProviderId.Value).ToList();

                if(model.CentralId != null)
                    demands = demands.Where(x => x.WorkOrder.CentralId == model.CentralId.Value).ToList();

                if(model.From != null)
                    if (model.DateSearchType ==1)
                    {
                        demands = demands.Where(x => x.StartAt != null && x.StartAt.Date >= model.From.Value.Date).ToList();
                    }
                    else
                    {
                        demands = demands.Where(x => x.PaymentDate != null && x.PaymentDate.Value.Date >= model.From.Value.Date).ToList();
                    }

                if(model.To != null)
                    if (model.DateSearchType == 1)
                    {
                        demands =
                            demands.Where(x => x.StartAt != null && x.StartAt.Date <= model.To.Value.Date).ToList();
                    }
                    else
                    {
                        demands =
                            demands.Where(x => x.PaymentDate != null && x.PaymentDate.Value.Date <= model.To.Value.Date)
                                .ToList();
                    }
            if(model.PaymentTypeId != null)
                    demands = demands.Where(x => x.WorkOrder.PaymentTypeID == model.PaymentTypeId).ToList();
                if(model.Isrequested == true)
                    demands = demands.Where(x => x.IsRequested == model.Isrequested).ToList();
                return demands;
            
        }


        public List<DemandResultModel> SearchDemands(BasicSearchModel model,ISPDataContext context){

          return  SearchDemandsNonModeled(model,context).Select(DemandResultModel.To).ToList();
             
        }


        public List<DemandResultModel> SearchDemandsByUser(int userId, DateTime start, DateTime end, bool ? paid){
            using(var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                var demands = context2.Demands.Where(d => d.UserId == userId && d.PaymentDate!=null).ToList().Where(x => x.PaymentDate != null && x.PaymentDate.Value.Date >= start.Date && x.PaymentDate.Value.Date <= end.Date).ToList();
                if(paid != null && paid.Value){
                    demands = demands.Where(d => d.Paid).ToList();
                } else{
                    demands = demands.Where(d => !d.Paid).ToList();
                }
                return demands.Select(DemandResultModel.To).ToList();
            }
        }


        public List<DemandResultModel> ResellerDemands(int resellerId, bool ? paid){
            using(var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                var demands = context3.Demands.Where(x => x.WorkOrder.ResellerID == resellerId).ToList();
                if(paid == null){
                    return demands.Select(DemandResultModel.To).ToList();
                }
                if(paid.Value){
                    return demands.Where(d => d.Paid).ToList().Select(DemandResultModel.To).ToList();
                }

                return demands.Where(d => !d.Paid).ToList().Select(DemandResultModel.To).ToList();
            }
        }
       

        public List<DemandResultModel2> ResellerDemandsAdo(int resellerId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            cmd.CommandType = CommandType.Text;


             cmd.CommandText =  "select * from Demands INNER JOIN WorkOrders ON WorkOrders.ID = Demands.WorkOrderId INNER JOIN Governorates ON WorkOrders.CustomerGovernorateID = Governorates.ID INNER JOIN Centrals ON WorkOrders.CentralId = Centrals.Id INNER JOIN ServiceProviders ON WorkOrders.ServiceProviderID = ServiceProviders.ID INNER JOIN ServicePackages ON WorkOrders.ServicePackageID = ServicePackages.ID   INNER JOIN Status ON WorkOrders.WorkOrderStatusID = Status.ID where WorkOrders.ResellerID ="+resellerId+ " and WorkOrders.WorkOrderStatusID <> 9 and WorkOrders.WorkOrderStatusID <> 8";
                      cmd.Connection.Open();

             SqlDataReader dr = cmd.ExecuteReader();
             DataTable dt = new DataTable();
             dt.Load(dr);
             cmd.Connection.Close();

            
             var myEnumerable = dt.AsEnumerable();
            using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                List<DemandResultModel2> myClassList =
                    (from item in myEnumerable
                        select new DemandResultModel2
                        {
                            Name = item.Field<string>("CustomerName"),
                            Phone = item.Field<string>("CustomerPhone"),
                            Governorate = item.Field<string>("GovernorateName"),
                            Central = item.Field<string>("Name"),
                            Provider = item.Field<string>("SPName"),
                            Package = item.Field<string>("ServicePackageName"),
                            Offer =  item.Field<int?>("OfferId") == null ? "" : context3.Offers.Where(x => x.Id == item.Field<int>("OfferId")).Select(x => x.Title).FirstOrDefault(),
                              WorkorderId = item.Field<int>("WorkOrderId"),
                           
                                Id = item.Field<int>("id"),
                            Paid = item.Field<bool>("Paid"),
                            Status = item.Field<string>("StatusName"),
                            TStart = item.Field<DateTime>("StartAt"),
                            TEnd = item.Field<DateTime>("EndAt"),
                            Amount = item.Field<decimal>("Amount"),
                            Isrequested =
                                (item.Field<bool?>("IsRequested") == null) ? false : item.Field<bool>("IsRequested"),
                            DAmount = item.Field<decimal>("Amount"),
                          

                        }).ToList();

           


            return myClassList;
 }


        }

        public List<DemandResultModel2> BranchDemandsAdo(int branid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            cmd.CommandType = CommandType.Text;


            cmd.CommandText = "select * from Demands INNER JOIN WorkOrders ON WorkOrders.ID = Demands.WorkOrderId INNER JOIN Governorates ON WorkOrders.CustomerGovernorateID = Governorates.ID INNER JOIN Centrals ON WorkOrders.CentralId = Centrals.Id INNER JOIN ServiceProviders ON WorkOrders.ServiceProviderID = ServiceProviders.ID INNER JOIN ServicePackages ON WorkOrders.ServicePackageID = ServicePackages.ID   INNER JOIN Status ON WorkOrders.WorkOrderStatusID = Status.ID where WorkOrders.BranchID =" + branid + " and WorkOrders.ResellerID is null  and WorkOrders.WorkOrderStatusID <> 9 and WorkOrders.WorkOrderStatusID <> 8";
            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            cmd.Connection.Close();


            var myEnumerable = dt.AsEnumerable();
            using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                List<DemandResultModel2> myClassList =
                    (from item in myEnumerable
                     select new DemandResultModel2
                     {
                         Name = item.Field<string>("CustomerName"),
                         Phone = item.Field<string>("CustomerPhone"),
                         Governorate = item.Field<string>("GovernorateName"),
                         Central = item.Field<string>("Name"),
                         Provider = item.Field<string>("SPName"),
                         Package = item.Field<string>("ServicePackageName"),
                         Offer = item.Field<int?>("OfferId") == null ? "" : context3.Offers.Where(x => x.Id == item.Field<int>("OfferId")).Select(x => x.Title).FirstOrDefault(),
                         WorkorderId = item.Field<int>("WorkOrderId"),

                         Id = item.Field<int>("id"),
                         Paid = item.Field<bool>("Paid"),
                         Status = item.Field<string>("StatusName"),
                         TStart = item.Field<DateTime>("StartAt"),
                         TEnd = item.Field<DateTime>("EndAt"),
                         Amount = item.Field<decimal>("Amount"),
                         Isrequested =
                             (item.Field<bool?>("IsRequested") == null) ? false : item.Field<bool>("IsRequested"),
                         DAmount = item.Field<decimal>("Amount"),


                     }).ToList();




                return myClassList;
            }


        }


        public List<DemandResultModel> NonResellerDemands(int branchId, bool ? paid){
            using(var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                var demands = context4.Demands.Where(x => x.WorkOrder.BranchID == branchId && (x.WorkOrder.ResellerID == -1 || x.WorkOrder.ResellerID == null)).ToList();
                if(paid == null){
                    return demands.Select(DemandResultModel.To).ToList();
                }
                if(paid.Value){
                    return demands.Where(d => d.Paid).ToList().Select(DemandResultModel.To).ToList();
                }

                return demands.Where(d => !d.Paid).ToList().Select(DemandResultModel.To).ToList();
            }
        }

    }
}
