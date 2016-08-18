using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Domain
{
    /// <summary>
    /// Summary description for CompanyEntryRepository
    /// </summary>
    public class CompanyEntryRepository
    {
        /*readonly ISPDataContext _context;

    public CompanyEntryRepository()
    {
       _context = context;
    }*/

        public List<VoiceCompany> Get(ISPDataContext context){

            return context.VoiceCompanies.OrderByDescending(x => x.Id).ToList();

        }


        public List<CompanyDetails> Get(bool reshaped,ISPDataContext context){
            return Get(context).Select(To).ToList();
        }


        public static CompanyDetails To(VoiceCompany x){
            return new CompanyDetails{
                Id = x.Id,
                CompanyName = x.CompanyName,
                ServiceFees = Convert.ToDecimal(x.ServiceFees),//string.Format("{0} ..", x.Data.Length > 500 ? x.Data.Substring(0, 400) : x.Data),
                CommissionResellerOrBranch = Convert.ToDecimal(x.CommissionResellerOrBranch),
                CompanyImageUrl = string.Format("~/SiteLogo/{0}", x.CompanyImage),
                HtmlImageUrl = string.Format("../SiteLogo/{0}", x.CompanyImage),
            };
        }


        public VoiceCompany Get(int id,ISPDataContext context){

            return context.VoiceCompanies.FirstOrDefault(x => x.Id == id);
            
        }


        /*public CompanyDetails GetModel(int id,ISPDataContext context){
            return To(Get(id, context));
        }*/


        public bool Save(VoiceCompany companyDetail,ISPDataContext context){

            try{
                if(companyDetail.Id == 0){
                    context.VoiceCompanies.InsertOnSubmit(companyDetail);
                }
                context.SubmitChanges();
                return true;
            }
            catch(Exception){
                return false;
            }
            
        }


        public bool Delete(VoiceCompany companyDetail,ISPDataContext context){

            try{
                if(companyDetail != null){
                    context.VoiceCompanies.DeleteOnSubmit(companyDetail);
                }
                context.SubmitChanges();
                return true;
            }
            catch(Exception){
                return false;
            }
            
        }


        /*public bool Delete(int id,ISPDataContext db){
            
            try{
                var companyDetail = Get(id,db);
                if(companyDetail != null){
                    db.VoiceCompanies.DeleteOnSubmit(companyDetail);
                }
                db.SubmitChanges();
                return true;
            }
            catch(Exception){
                return false;
            }
            
        }*/
    }
}