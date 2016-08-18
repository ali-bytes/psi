using Db;

namespace NewIspNL.Helpers{
    public static class Transformer{
        public static object DemandToGridPreview(Demand x){
            return new{
                x.Id,
                Amount = Helper.FixNumberFormat(x.Amount),
                From = x.StartAt.ToShortDateString(),
                To = x.EndAt.ToShortDateString(),
                Customer = x.WorkOrder.CustomerName,
                Phone = x.WorkOrder.CustomerPhone,
                Offer = x.OfferId == null ? string.Empty : x.Offer.Title,
                x.Notes,
                DetailsUrl = string.Format("~/Pages/CustomerDetails.aspx?c={0}", QueryStringSecurity.Encrypt(x.WorkOrderId.ToString())),
                User = x.User.UserName,
                x.PaymentComment,
                PaymentDate = x.PaymentDate == null ? "-" : x.PaymentDate.Value.ToString(),
                ForMonth = x.StartAt.Month,
                ForYear = x.StartAt.Year,
                Commisstion =x.IsResellerCommisstions
            };
        }


        /*public static object DemandToGridPreview(DemandModel x){
            return new{
                x.Id,
                OrgininalAmount = Helper.FixNumberFormat(x.Demand.Amount),
                Amount = Helper.FixNumberFormat(x.Amount),
                From = x.StartAt.ToShortDateString(),
                To = x.EndAt.ToShortDateString(),
                Customer = x.Demand.WorkOrder.CustomerName,
                Phone = x.Demand.WorkOrder.CustomerPhone,
                Offer = x.Demand.Offer == null ? string.Empty : x.Demand.Offer.Title,
                ResellerDiscount = Helper.FixNumberFormat(x.Demand.Amount - x.Amount),
                DiscountPercent = Helper.FixNumberFormat((1 - (x.Amount / x.Demand.Amount)) * 100),
                x.Demand.Notes,
                DetailsUrl = string.Format("~/Pages/CustomerDetails.aspx?c={0}", x.Demand.WorkOrderId),
                User = x.Demand.User.UserName,
                x.Demand.PaymentComment,
                PaymentDate = x.Demand.PaymentDate == null ? "-" : x.Demand.PaymentDate.Value.ToString(CultureInfo.InvariantCulture),
                ForMonth = x.StartAt.Month,
                ForYear = x.StartAt.Year
            };
        }*/
    }
}
