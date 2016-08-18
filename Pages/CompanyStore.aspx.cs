using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CompanyStore : CustomPage
    {
        
            readonly IRouterRepository _routerRepository = new RouterRepository();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                UpdateQuantity();
            }


            void UpdateQuantity()
            {
                L_Quantity.Text = _routerRepository.Quantity().ToString(CultureInfo.InvariantCulture);
            }


            protected void BAddSubClick(object sender, EventArgs e)
            {
                var quantity = Convert.ToInt32(TB_Quantity.Text);
                var saveAttemp = _routerRepository.Save(quantity);
                switch (saveAttemp)
                {
                    case RouterSaveState.Saved:
                        LI_Message.Text = Tokens.Saved;
                        ClearInputs();
                        UpdateQuantity();
                        break;
                    case RouterSaveState.Problem:
                        LI_Message.Text = Tokens.NegativeQuantity;
                        break;
                    default:
                        LI_Message.Text = Tokens.SaveErrorCompanyStore;
                        break;
                }
            }


            void ClearInputs()
            {
                TB_Quantity.Text = string.Empty;
            }
        }
    }
 