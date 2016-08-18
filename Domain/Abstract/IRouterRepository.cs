using System;
using System.Collections.Generic;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IRouterRepository{
        int Quantity();


        RouterSaveState Save(int quantity);


        void SaveRouter(Router router, int operationId);


        RouterSaveState TransferToReseller(int resellerId, int quantity);


        //int ResellerQuantity(int resellerId);


        void SaveResellerRouter(int resellerId, int quantity, DateTime time);


        void SaveResellerRouter(int resellerId, int quantity, DateTime time, int workOrderId);


        List<ResellerRouterStock> CalculateResellersStocks(List<User> resellers);


        List<RouterHistory> CalculateMainHistory(DateTime start, DateTime end);


        List<ResellerRouterHistory> CalculateResellerHistory(DateTime start, DateTime end, int resellerId, string reseller, int operationId);


        void SaveWorkOrderRouter(WorkOrderRouter workOrderRouter);
    }

    public class ResellerRouterHistory : RouterHistory{
        public string Reseller { get; set; }

        public string Provider { get; set; }

        public string Package { get; set; }

        public string Phone { get; set; }

        public string Governate { get; set; }

        public string IpPackage { get; set; }

        public string Branch { get; set; }

        public string Status { get; set; }
    }
}
