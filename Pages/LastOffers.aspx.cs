using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Pages
{
    public partial class LastOffers : CustomPage
    {
       
            readonly OffersEntryRepository _entryRepository;

          
            public  LastOffers()
            {
                _entryRepository = new OffersEntryRepository();
            }

            public List<OffersDetail> Offer2;

            protected void Page_Load(object sender, EventArgs e)
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    if (string.IsNullOrWhiteSpace(Request.QueryString["WOID"]))
                    {

                        //Offers = _entryRepository.Get(true,db);
                        Offer2 = db.OffersDetails.Select(z => z).ToList();
                    }
                    else
                    {
                        var que = Request.QueryString["WOID"];
                        var id = QueryStringSecurity.Decrypt(que);
                        var orderId = Convert.ToInt32(id);
                        Offer2 = db.OffersDetails.Where(z => z.Id == orderId).Select(z => z).ToList();
                    }



                }
            }
        }
    }
 