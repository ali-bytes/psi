using System;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.BL.Abstract;


namespace BL.Concrete{
    public class IspClientService : IIspClientService{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IIspClientService Members


        /// <summary>
        ///     Checks for last activation date for certain work order
        /// </summary>
        /// <param name="id">work order id</param>
        /// <returns>last activation date for a work order</returns>
        public DateTime ? GetLastActivationDate(int id){
            var activationDates =
                _context.WorkOrderStatus.Where(w => w.WorkOrderID == id && w.StatusID == 6).OrderByDescending(
                                                                                                              w => w.UpdateDate).ToList();

            if(activationDates.Count > 0){
                // fetch last activation date
                var lastState = activationDates.FirstOrDefault();
                if(lastState != null){
                    var activationDate = lastState.UpdateDate;
                    if(activationDate != null){
                        return activationDate.Value;
                    }
                }
            }
            return null;
        }


        #endregion
    }
}
