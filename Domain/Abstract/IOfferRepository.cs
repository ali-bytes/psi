using System.Collections.Generic;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IOfferRepository{
        IQueryable<Offer> Offers { get; }


        void Save(Offer offer);


        void Delete(Offer offer);


        List<Offer> GetOffersByUser(int userId);


        decimal GetServicePackageDiscount(int offerId, int packageId);
    }
}
