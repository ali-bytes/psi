using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Db;
using System.Web.UI.HtmlControls;
using Resources;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class AddCompany : CustomPage
    {
        
            readonly CompanyEntryRepository _entryRepository;


            public AddCompany()
            {
                _entryRepository = new CompanyEntryRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                Activate();
                if (IsPostBack) return;

                flag.Value = "0";
                PopulateDetails();
                MsgError.Visible = false;
                MsgSuccess.Visible = false;
            }


            private void PopulateDetails()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var list = _entryRepository.Get(true, context);
                    GvCompany.DataSource = list;
                    GvCompany.DataBind();
                }
            }


            void AddItem()
            {
                Clear();
                flag.Value = "1";
                ImgOffer.ImageUrl = txtCompanyName.Text = txtCommission.Text = txtServiceFees.Text = string.Empty;
            }


            void Clear()
            {
                MsgSuccess.Visible = MsgError.Visible = false;
                MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
            }


            protected void DeleteEvent(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var btn = sender as HtmlButton;
                    if (btn == null) return;
                    var id = Convert.ToInt32(btn.ValidationGroup);
                    var detail = _entryRepository.Get(id, context);
                    if (detail != null &&
                        File.Exists(Server.MapPath(string.Format("~/SiteLogo/{0}", detail.CompanyImage))))
                        File.Delete(Server.MapPath(string.Format("~/SiteLogo/{0}", detail.CompanyImage)));

                    bool deleted = _entryRepository.Delete(detail, context);

                    if (deleted)
                    {
                        MsgSuccess.Visible = true;
                        MsgSuccess.InnerHtml = Tokens.Deleted;
                    }
                    flag.Value = "0";
                    PopulateDetails();
                }
            }


            protected void EditEvent(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    flag.Value = "1";
                    MsgSuccess.Visible = MsgError.Visible = false;
                    MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
                    var btn = sender as HtmlButton;
                    if (btn == null) return;
                    var id = Convert.ToInt32(btn.ValidationGroup);
                    selected.Value = btn.ValidationGroup;
                    var detail = _entryRepository.Get(true, context).FirstOrDefault(x => x.Id == id);
                    if (detail == null) return;
                    txtCompanyName.Text = detail.CompanyName;
                    txtCommission.Text = detail.CommissionResellerOrBranch.ToString(CultureInfo.InvariantCulture);
                    txtServiceFees.Text = detail.ServiceFees.ToString(CultureInfo.InvariantCulture);

                    ImgOffer.ImageUrl = detail.CompanyImageUrl;
                }
            }



            void Save()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var chk = Convert.ToInt32(selected.Value);
                    var companyName = txtCompanyName.Text;
                    var comisson =txtCommission.Text == null || txtCommission.Text == "" ? 0 : Convert.ToDecimal(txtCommission.Text);
                    var fees = txtServiceFees.Text == null || txtServiceFees.Text == "" ? 0 : Convert.ToDecimal(txtServiceFees.Text);

                    if (chk == 0)
                    {

                        if (string.IsNullOrWhiteSpace(FuImag.FileName))
                        {
                            var x = Path.GetExtension(FuImag.FileName);
                            if (x != null)
                            {
                                var extension = x.ToLower();
                                if (!extension.Equals(".jpg") || !extension.Equals(".bmp") || !extension.Equals(".jpeg"))
                                {
                                    MsgError.Visible = true;
                                    MsgError.InnerHtml = string.Format("{0},{1}", Tokens.BadFileExtension, Tokens.OnlyJpgBmp);
                                    return;
                                }
                            }
                        }
                        //new
                        if (string.IsNullOrWhiteSpace(selected.Value) || chk == 0)
                        {
                            if (string.IsNullOrWhiteSpace(FuImag.FileName))
                            {
                                MsgError.Visible = true;
                                MsgError.InnerHtml = Tokens.PleaseUploadImage;
                                return;
                            }

                            string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                            string filename = virtualName + ".jpeg";
                            var item = new VoiceCompany
                            {
                                CompanyName = companyName,
                                CommissionResellerOrBranch = comisson,
                                ServiceFees = fees,
                                CompanyImage = filename
                            };

                            var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                            string ex = Path.GetExtension(FuImag.FileName);

                            if (!string.IsNullOrEmpty(ex) &&
                                extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                            {
                                FuImag.SaveAs(Server.MapPath(string.Format("~/SiteLogo/{0}", filename)));
                                _entryRepository.Save(item, IspDataContext);
                                MsgSuccess.Visible = true;
                                MsgSuccess.InnerHtml = Tokens.Saved;
                                MsgError.Visible = false;
                                MsgError.InnerHtml = string.Empty;
                                flag.Value = "0";
                                PopulateDetails();
                            }
                        }
                    }
                    else
                    {
                        // edit
                        var id = chk;
                        var detail = _entryRepository.Get(id, context);
                        detail.CompanyName = companyName;
                        detail.CommissionResellerOrBranch = comisson;
                        detail.ServiceFees = fees;

                        if (!string.IsNullOrWhiteSpace(FuImag.FileName))
                        {
                            if (File.Exists(Server.MapPath(string.Format("~/SiteLogo/{0}", detail.CompanyImage))))
                            {
                                File.Delete(Server.MapPath(string.Format("~/SiteLogo/{0}", detail.CompanyImage)));
                            }

                            var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                            string ex = Path.GetExtension(FuImag.FileName);

                            if (!string.IsNullOrEmpty(ex) &&
                                extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                            {
                                string virtualName =
                                    DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                                string filename = virtualName + ".jpeg";
                                detail.CompanyImage = filename;
                                FuImag.SaveAs(Server.MapPath(string.Format("~/SiteLogo/{0}", filename)));
                            }
                            else
                            {
                                return;
                            }
                        }
                        MsgSuccess.Visible = true;
                        MsgSuccess.InnerHtml = Tokens.Saved;
                        MsgError.Visible = false;
                        MsgError.InnerHtml = string.Empty;
                        _entryRepository.Save(detail, context);
                        flag.Value = "0";
                        PopulateDetails();
                    }
                }
            }

            protected void Newdata(object sender, EventArgs e)
            {
                selected.Value = "0";
            }

            void Activate()
            {
                GvCompany.DataBound += (o, e) => Helper.GridViewNumbering(GvCompany, "LNo");
                BAdd.ServerClick += (o, e) => AddItem();
                BSave.ServerClick += (o, e) => Save();
                bCancel.ServerClick += (o, e) =>
                {
                    flag.Value = "0";
                    Clear();
                };

            }
        }
  
}