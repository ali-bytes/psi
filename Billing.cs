#region

using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Db;

/*using NewIspNL1.BL.Concrete;
 * using System.Data;
using Domain.Concrete;
using Domain.SearchService;
using Helpers;*/


#endregion


namespace NewIspNL
{
    public class Billing{
        


        #region Other


        

        public static double SubString2Digits(double ammount){
            //var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            string[] allAmount = ammount.ToString().Split('.');
            string AfterDot;
            if(allAmount.Length > 1){
                AfterDot = allAmount[1];
                try{
                    AfterDot = AfterDot.Substring(0, 2);
                }
                catch{}
                return Convert.ToDouble(allAmount[0] + "." + AfterDot);
            } else{
                return Convert.ToDouble(allAmount[0]);
            }
        }


       

        /// <summary>
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="type"> Branch , Reseller or WorkOrder </param>
        /// <returns> </returns>
        public static double GetLastBalance(int id, string type){
            var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
            double total = 0;
            switch (type)
            {
                case "Branch":
                {
                    var usersTransactionList = (from userTransaction in dataContext.UsersTransactions
                        where userTransaction.BranchID == id
                        select userTransaction).ToList();
                    if(usersTransactionList.Count > 0)
                        total = usersTransactionList.Last().Total.Value;
                }
                    break;
                case "Reseller":
                {
                    var usersTransactionList = (from userTransaction in dataContext.UsersTransactions
                        where userTransaction.ResellerID == id
                        select userTransaction).ToList();
                    if(usersTransactionList.Count > 0)
                        total = usersTransactionList.Last().Total.Value;
                }
                    break;
                case "WorkOrder":
                {
                    var usersTransactionList = (from userTransaction in dataContext.UsersTransactions
                        where userTransaction.WorkOrderID == id
                        select userTransaction).ToList();
                    if(usersTransactionList.Count > 0)
                        total = usersTransactionList.Last().Total.Value;
                }
                    break;
            }
            return total;
        }




        #endregion
    }
}
