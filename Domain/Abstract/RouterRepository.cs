using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Helpers;
using System.Transactions;

namespace NewIspNL.Domain.Abstract{
    public class RouterRepository : IRouterRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IRouterRepository Members


        public int Quantity(){
            return _context.Routers.Any() ? _context.Routers.Sum(r => r.Quantity) : 0;
        }


        public RouterSaveState Save(int quantity){
            const int leastQauntity = 0;
            var currentQuantity = Quantity();
            var resultQuantity = currentQuantity + quantity;
            if(resultQuantity < leastQauntity){
                return RouterSaveState.Problem;
            }
            var router = new Router{
                Quantity = quantity,
                Time = DateTime.Now.AddHours(),
            };
            SaveRouter(router, 1);
            return RouterSaveState.Saved;
        }


        public void SaveRouter(Router router, int operationId){
            router.RouterOperationsId = operationId;
            _context.Routers.InsertOnSubmit(router);
            _context.SubmitChanges();
        }


        public RouterSaveState TransferToReseller(int resellerId, int quantity){
            const int leastQauntity = 0;

            var available = Quantity();
            if(available < leastQauntity){
                return RouterSaveState.Problem;
            }
            var result = available - quantity;
            if(result < leastQauntity){
                return RouterSaveState.Problem;
            }
            var time = DateTime.Now.AddHours();
            var router = new Router{
                Quantity = -quantity,
                Time = time,
                ResellerId = resellerId
            };
            try{
                using(var save = new TransactionScope()){
                    SaveRouter(router, 2);
                    SaveResellerRouter(resellerId, quantity, time);
                    save.Complete();
                    return RouterSaveState.Saved;
                }
            }
            catch(Exception){
                return RouterSaveState.Problem;
            }
        }


       /* public int ResellerQuantity(int resellerId){
            return _context.ResellerRouters.Sum(r => r.Quantity);
        }*/


        public void SaveResellerRouter(int resellerId, int quantity, DateTime time){
            var resellerRouter = new ResellerRouter{
                Quantity = quantity,
                Time = time,
                ResellerId = resellerId,
                RouterOperationsId = 1
            };
            _context.ResellerRouters.InsertOnSubmit(resellerRouter);
            _context.SubmitChanges();
        }


        public void SaveResellerRouter(int resellerId, int quantity, DateTime time, int workOrderId){
            var resellerRouter = new ResellerRouter{
                Quantity = quantity,
                Time = time,
                RouterOperationsId = 3,
                WorkOrderId = workOrderId,
                ResellerId = resellerId
            };
            
            _context.ResellerRouters.InsertOnSubmit(resellerRouter);
            _context.SubmitChanges();
        }


        public List<ResellerRouterStock> CalculateResellersStocks(List<User> resellers){
            var stocks = new List<ResellerRouterStock>();

            foreach(var reseller in resellers){
                var reseller1 = reseller;
                var resellerItems = _context.ResellerRouters.Where(r => r.ResellerId == reseller1.ID).ToList();
                var quantity = resellerItems.Sum(s => s.Quantity);
                var item = new ResellerRouterStock{
                    ResellerId = reseller.ID,
                    Name = reseller.UserName,
                    Stock = quantity
                };
                stocks.Add(item);
            }
            return stocks.OrderByDescending(s => s.Stock).ToList();
        }


        public List<RouterHistory> CalculateMainHistory(DateTime start, DateTime end){
            var items = new List<RouterHistory>();
            var all = _context.Routers.Where(s => s.Time.Date >= start.Date && s.Time.Date <= end.Date);
            foreach(var router in all){
                var newItem = new RouterHistory{
                    Operation = router.RouterOperation.Name,
                    Quantity = router.Quantity,
                    Time = router.Time,
                };

                switch(router.RouterOperationsId){
                    case 1 :
                        newItem.Consumer = router.RouterOperation.Name;
                        break;
                    case 2 :
                        newItem.Consumer = router.User.UserName;
                        break;
                    case 3 :
                        newItem.Consumer = router.WorkOrder.CustomerName;
                        break;
                    default :
                        newItem.Consumer = "-";
                        break;
                }
                items.Add(newItem);
            }
            return items.OrderByDescending(x => x.Time).ToList();
        }


        public List<ResellerRouterHistory> CalculateResellerHistory(DateTime start, DateTime end, int resellerId, string reseller, int operationId){
            var items = new List<ResellerRouterHistory>();
            var all = _context.ResellerRouters.Where(s => s.RouterOperationsId == operationId && s.ResellerId == resellerId && s.Time.Date >= start.Date && s.Time.Date <= end.Date);
            foreach(var router in all){
                var newItem = new ResellerRouterHistory{
                    Operation = router.RouterOperation.Name,
                    Quantity = router.Quantity,
                    Time = router.Time,
                    Reseller = reseller
                };

                switch(router.RouterOperationsId){
                    case 3 :
                        newItem.Consumer = router.WorkOrder.CustomerName;
                        newItem.Branch = router.WorkOrder.Branch.BranchName;
                        newItem.Governate = router.WorkOrder.Governorate.GovernorateName;
                        newItem.IpPackage = router.WorkOrder.IpPackage.IpPackageName;
                        newItem.Package = router.WorkOrder.ServicePackage.ServicePackageName;
                        newItem.Phone = router.WorkOrder.CustomerPhone;
                        newItem.Provider = router.WorkOrder.ServiceProvider.SPName;
                        newItem.Status = router.WorkOrder.Status.StatusName;
                        newItem.Branch = router.WorkOrder.Branch.BranchName;
                        break;
                }
                items.Add(newItem);
            }
            return items.OrderByDescending(x => x.Time).ToList();
        }


        public void SaveWorkOrderRouter(WorkOrderRouter workOrderRouter){
            if(workOrderRouter.Id == 0){
                _context.WorkOrderRouters.InsertOnSubmit(workOrderRouter);
            }
            _context.SubmitChanges();
        }


        #endregion
    }
}
