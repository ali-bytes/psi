using System.Collections.Generic;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IOffersResellerRepository{
        void Save(OfferReseller providersOffer);


        void SaveMany(List<OfferReseller> providersOffers);


        void Delete(OfferReseller providersOffer);


        void DeleteMany(List<OfferReseller> providersOffers);


        IQueryable<OfferReseller> GetProviderOffersByOfferId(int offerId);


        IQueryable<OfferReseller> GetProviderOffersByProviderId(int providerId);


        void DeleteAllAndAddNew(List<OfferReseller> old, List<OfferReseller> add);
    }
    public interface IOffersProviderRepository{
        void Save(OfferProvider providersOffer);


        void SaveMany(List<OfferProvider> providersOffers);


        void Delete(OfferProvider providersOffer);


        void DeleteMany(List<OfferProvider> providersOffers);


        IQueryable<OfferProvider> GetProviderOffersByOfferId(int offerId);


        //IQueryable<OfferProvider> GetProviderOffersByProviderId(int providerId);

        //void DeleteAllAndAddNew(List<OfferProvider> old, List<OfferProvider> add);
    }
}
