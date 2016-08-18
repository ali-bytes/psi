using Db;


namespace NewIspNL.Services.OfferServices{
    public static class OfferPricingServices{
        /*public static decimal GetOfferPrice(Offer offer, decimal amount){
            if(offer.ByPercent){
                return amount * offer.Discount / 100;
            }
            return offer.Discount;
        }*/
        public static decimal GetOfferPrice(Offer offer, decimal amount,decimal basicBill)
        {
            if (offer.ByPercent)
            {
                return amount * offer.Discount / 100;
            }
            if (basicBill == 0) return 0;
            var dis = offer.Discount/basicBill*100;
            return amount*dis/100;
        }
    }
}