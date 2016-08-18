using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Domain{
    public class OffersEntryRepository{
        //readonly ISPDataContext _context;
        public List<OffersDetail> Get(ISPDataContext context){
            
                return context.OffersDetails.OrderByDescending(x => x.Id).ToList();
            //}
        }


        public List<OfferDetailsModel> Get(bool reshaped,ISPDataContext context){
            return Get(context).Select(To).ToList();
        }


        public static OfferDetailsModel To(OffersDetail x){
            return new OfferDetailsModel{
                Id = x.Id,
                Name = x.Name,
                Brief = string.Format("{0} ..", x.Data.Length > 500 ? x.Data.Substring(0, 400) : x.Data),
                ImageUrl = string.Format("~/_offerDetailsImages/{0}", x.ImageUrl),
                Data = x.Data,
                htmlImageUrl = string.Format("../_offerDetailsImages/{0}", x.ImageUrl),
            };
        }


        public OffersDetail Get(int id,ISPDataContext context){
                return context.OffersDetails.FirstOrDefault(x => x.Id == id);
        }
      


        /*public OfferDetailsModel GetModel(int id,ISPDataContext context){
            return To(Get(id,context));
        }*/


        public bool Save(OffersDetail offersDetail, ISPDataContext context)
        {
                try{
                    if(offersDetail.Id == 0){
                        context.OffersDetails.InsertOnSubmit(offersDetail);
                    }
                    context.SubmitChanges();
                    return true;
                }
                catch(Exception){
                    return false;
                }
        }


        public bool Delete(OffersDetail offersDetail, ISPDataContext context)
        {
                try{
                    if(offersDetail != null){
                        context.OffersDetails.DeleteOnSubmit(offersDetail);
                    }
                    context.SubmitChanges();
                    return true;
                }
                catch(Exception){
                    return false;
                }
        }


       /* public bool Delete(int id, ISPDataContext context)
        {
                try{
                    var offersDetail = Get(id,context);
                    if(offersDetail != null){
                        context.OffersDetails.DeleteOnSubmit(offersDetail);
                    }
                    context.SubmitChanges();
                    return true;
                }
                catch(Exception){
                    return false;
                }
            
        }*/
    }
}
