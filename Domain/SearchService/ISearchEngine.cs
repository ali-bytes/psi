using System.Collections.Generic;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Domain.SearchService{
    public interface ISearchEngine{
       // bool UserCanEditCustomer(int userId);


        List<CustomerResult> Search(AdvancedBasicSearchModel model, int CheckPoint, int isMonthChecked, int isPrePaidChecked, int isTextEmpty,int userId);
   
        List<CustomerResult> SearchByRouterSerial(string serial);


        CustomerResult ToCustomerResult(WorkOrder order);


        CanEditModel EditCustomer(int userId);


    }
}
