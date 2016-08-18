using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class OffersBranchRepository{
        readonly ISPDataContext _context;


        public OffersBranchRepository(ISPDataContext context){
            _context = context;
        }
        public OffersBranchRepository(){
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        }


        public void Save(OfferBranch offerBranch){
            _context.OfferBranches.InsertOnSubmit(offerBranch);
            _context.SubmitChanges();
        }


        public void SaveMany(List<OfferBranch> offerBranches){
            foreach(var providersOffer in offerBranches){
                Save(providersOffer);
            }
        }


        public void Delete(OfferBranch offerBranch){
            _context.OfferBranches.DeleteOnSubmit(offerBranch);
            _context.SubmitChanges();
        }


        public void DeleteMany(List<OfferBranch> offerBranches){
            foreach(var providersOffer in offerBranches){
                Delete(providersOffer);
            }
        }


        public IQueryable<OfferBranch> GetProviderOffersByOfferId(int offerId){
            return _context.OfferBranches.Where(o => o.OfferId == offerId);
        }


       /* public IQueryable<OfferBranch> GetProviderOffersByProviderId(int branchId){
            return _context.OfferBranches.Where(o => o.BranchId == branchId);
        }*/


        public void DeleteAllAndAddNew(List<OfferBranch> old, List<OfferBranch> add){
            DeleteMany(old);
            SaveMany(add);
        }
    }

    public class OffersResellerRepository : IOffersResellerRepository{
        readonly ISPDataContext _context;


        public OffersResellerRepository(ISPDataContext context){
            _context = context;
        }


        public OffersResellerRepository(){
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        }


        #region IOffersResellerRepository Members


        public void Save(OfferReseller providersOffer){
            _context.OfferResellers.InsertOnSubmit(providersOffer);
            _context.SubmitChanges();
        }


        public void SaveMany(List<OfferReseller> providersOffers){
            foreach(var providersOffer in providersOffers){
                Save(providersOffer);
            }
        }


        public void Delete(OfferReseller providersOffer){
            _context.OfferResellers.DeleteOnSubmit(providersOffer);
            _context.SubmitChanges();
        }


        public void DeleteMany(List<OfferReseller> providersOffers){
            foreach(var providersOffer in providersOffers){
                Delete(providersOffer);
            }
        }


        public IQueryable<OfferReseller> GetProviderOffersByOfferId(int offerId){
            return _context.OfferResellers.Where(o => o.OfferId == offerId);
        }


        public IQueryable<OfferReseller> GetProviderOffersByProviderId(int providerId){
            return _context.OfferResellers.Where(o => o.UserId == providerId);
        }


        public void DeleteAllAndAddNew(List<OfferReseller> old, List<OfferReseller> add){
            DeleteMany(old);
            SaveMany(add);
        }


        #endregion
    }

    public class OffersProviderRepository : IOffersProviderRepository{
        readonly ISPDataContext _context;


        public OffersProviderRepository(ISPDataContext context){
            _context = context;
        }


        public OffersProviderRepository(){
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        }


        #region IOffersProviderRepository Members


        public void Save(OfferProvider providersOffer){
            _context.OfferProviders.InsertOnSubmit(providersOffer);
            _context.SubmitChanges();
        }


        public void SaveMany(List<OfferProvider> providersOffers){
            foreach(var providersOffer in providersOffers){
                Save(providersOffer);
            }
        }


        public void Delete(OfferProvider providersOffer){
            _context.OfferProviders.DeleteOnSubmit(providersOffer);
            _context.SubmitChanges();
        }


        public void DeleteMany(List<OfferProvider> providersOffers){
            foreach(var providersOffer in providersOffers){
                Delete(providersOffer);
            }
        }


        public IQueryable<OfferProvider> GetProviderOffersByOfferId(int offerId){
            return _context.OfferProviders.Where(o => o.OfferId == offerId);
        }


        /*public IQueryable<OfferProvider> GetProviderOffersByProviderId(int providerId){
            return _context.OfferProviders.Where(o => o.ProviderId == providerId);
        }


        public void DeleteAllAndAddNew(List<OfferProvider> old, List<OfferProvider> add){
            DeleteMany(old);
            SaveMany(add);
        }*/


        #endregion
    }
}
