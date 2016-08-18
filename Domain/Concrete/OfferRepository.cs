using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class OfferRepository : IOfferRepository{
        readonly ISPDataContext _context;
         public OfferRepository(ISPDataContext context){
             _context = context;
             _offersResellerRepository = new OffersResellerRepository(_context);
         }


        public OfferRepository(){
            _offersResellerRepository = new OffersResellerRepository();
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        }


        readonly IOffersResellerRepository _offersResellerRepository;

        readonly IUserRepository _userRepository = new UserRepository();


        #region IOfferRepository Members


        public IQueryable<Offer> Offers{
            get { return _context.Offers; }
        }



        public void Save(Offer offer){
            if(offer.Id == 0){
                _context.Offers.InsertOnSubmit(offer);
            }
            _context.SubmitChanges();
        }


        public void Delete(Offer offer){
            if(offer != null){
                _context.Offers.DeleteOnSubmit(offer);
                _context.SubmitChanges();
            }
        }


        public List<Offer> GetOffersByUser(int userId){
            var user = _userRepository.Users.FirstOrDefault(u => u.ID == userId);
            if(user == null){
                return null;
            }

            // todo: confirm reseller!!
            switch(user.GroupID){
                case 1 :
                    return Offers.ToList();
                case 6 :
                    var resellerOffers = _offersResellerRepository.GetProviderOffersByProviderId(user.ID);
                    return resellerOffers.Select(offer => offer.Offer).ToList();
            }
            return null;
        }


        public decimal GetServicePackageDiscount(int offerId, int packageId){
            return 0;

        }


        #endregion
    }
}
