using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CallCenterEntry : CustomPage
    {
       
            readonly LtsCallRepository _repository = new LtsCallRepository();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    var id = Request.QueryString["id"] != null ? Request.QueryString["id"] : null;
                    if (id == null) return;
                    PopulateCallToEdit(Convert.ToInt32(id));
                }
            }



            protected void b_save_Click(object sender, EventArgs e)
            {
                if (IsValid)
                {
                    SaveCall();
                    ClearInputs(p_editor);
                    l_message.Text = Tokens.Saved;
                }
            }


            void SaveCall()
            {
                var id = Request.QueryString["id"] != null ? Request.QueryString["id"] : null;
                if (id == null)
                {
                    var call = new Call
                    {
                        CustomerName = tb_CustomerName.Text,
                        Responsible = tb_Responsible.Text,
                        EmployeeName = tb_EmployeeName.Text,
                        Topic = tb_Topic.Text,
                        Result = tb_Result.Text,
                        VisitDate = Convert.ToDateTime(tb_VisitDate.Text),
                        Address = tb_Address.Text,
                        CallDate = Convert.ToDateTime(tb_CallDate.Text),
                    };
                    _repository.SaveCall(call);
                }
                else
                {
                    var existingCall = _repository.Calls.FirstOrDefault(c => c.Id == Convert.ToInt32(id));
                    if (existingCall != null)
                    {
                        existingCall.CustomerName = tb_CustomerName.Text;
                        existingCall.Responsible = tb_Responsible.Text;
                        existingCall.EmployeeName = tb_EmployeeName.Text;
                        existingCall.Topic = tb_Topic.Text;
                        existingCall.Result = tb_Result.Text;
                        existingCall.VisitDate = Convert.ToDateTime(tb_VisitDate.Text);
                        existingCall.Address = tb_Address.Text;
                        existingCall.CallDate = Convert.ToDateTime(tb_CallDate.Text);
                        _repository.SaveCall(existingCall);
                    }
                }
            }


            static void ClearInputs(Control container)
            {
                foreach (TextBox textBox in container.Controls.OfType<TextBox>())
                {
                    textBox.Text = string.Empty;
                }
            }


            void PopulateCallToEdit(int id)
            {
                var call = _repository.Calls.FirstOrDefault(c => c.Id == id);
                if (call == null) return;
                tb_CustomerName.Text = call.CustomerName;
                tb_Responsible.Text = call.Responsible;
                tb_Topic.Text = call.Topic;
                tb_Result.Text = call.Result;
                tb_EmployeeName.Text = call.EmployeeName;
                if (call.VisitDate != null) tb_VisitDate.Text = call.VisitDate.Value.ToShortDateString();
                tb_EmployeeName.Text = call.EmployeeName;
                tb_Address.Text = call.Address;
                if (call.CallDate != null) tb_CallDate.Text = call.CallDate.Value.ToShortDateString();
            }


            /*protected void Button1_Click(object sender, EventArgs e){
                ClearInputs(p_editor);
            }*/
        }
    }
 