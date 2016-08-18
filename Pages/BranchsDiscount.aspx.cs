using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class BranchsDiscount : CustomPage
    {
        
            readonly BranchDiscountsService _branchDiscountsService;

            readonly IspDomian _domian;



            public  BranchsDiscount()
            {
                _domian = new IspDomian(IspDataContext);
                _branchDiscountsService = new BranchDiscountsService(IspDataContext);
            }



            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulatePage();
                MultiView1.SetActiveView(v_index);
                l_message.Text = "";
            }


            void PopulatePage()
            {
                _domian.PopulateProviders(DdlProvider);
                _domian.PopulateBranches(DdlBranch);
            }



            protected void gv_index_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_index, "l_number");
            }



            protected void ResellerPacksDiscounts(object sender, EventArgs e)
            {
                SearchDiscounts();
            }


            void SearchDiscounts()
            {
                if (DdlBranch.SelectedIndex < 1 || DdlProvider.SelectedIndex < 1)
                {
                    gv_index.DataSource = null;
                    gv_index.DataBind();
                    return;
                }
                var discounts = _branchDiscountsService
                    .BranchDiscounts(Convert.ToInt32(DdlBranch.SelectedItem.Value),
                    Convert.ToInt32(DdlProvider.SelectedItem.Value));
                var models = BranchDiscountsService.To(discounts);
                gv_index.DataSource = models;
                gv_index.DataBind();
            }


            protected void SaveDiscount(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(TbDiscount.Text) || string.IsNullOrEmpty(HfBra.Value) || string.IsNullOrEmpty(HfProv.Value)
                   || string.IsNullOrEmpty(HfPack.Value)) return;
                var discount = Convert.ToDecimal(TbDiscount.Text);
                var resellerId = Convert.ToInt32(HfBra.Value);
                var providerId = Convert.ToInt32(HfProv.Value);
                var packageId = Convert.ToInt32(HfPack.Value);
                _branchDiscountsService.SaveDiscount(resellerId, providerId, packageId, discount);
                _branchDiscountsService.Commit();
                SearchDiscounts();
                TbDiscount.Text = HfBra.Value = HfProv.Value = HfPack.Value = string.Empty;
            }
        }
    }
 