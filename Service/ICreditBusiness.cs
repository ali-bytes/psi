using System;
using Db;
using NewIspNL.Service.Enums;

namespace NewIspNL.Service
{
    public interface ICreditBusiness{
        decimal? GetCredit(int bankid);


        Save GetLastRecord(int id);


        CreditOperationResult Pay(decimal amount, int saveid);


        CreditOperationResult Receive(decimal amount, int saveid);


        /// <summary>
        ///   Returns Credit of exact day
        /// </summary>
        /// <param name="time"> Date of the required credit </param>
        /// <returns> DayCreditItemModel </returns>
        DayCreditItemModel CreditOfDay(DateTime time, int saveid);


       //DayCreditItemModel DayOpeningByType(CreditType creditType, DateTime time);



        //BasicCredit CreditByType(CreditType creditType, DateTime day);


        void AddTreasuryMovement(DateTime date, decimal amount, int saveid, int userId, string note, int type, string kind);
    }
}
