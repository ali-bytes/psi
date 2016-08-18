using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AddOffer : CustomPage
    {
        
            readonly OffersEntryRepository _entryRepository;


            public AddOffer()
            {
                _entryRepository = new OffersEntryRepository();
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


            void PopulateDetails()
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var list = _entryRepository.Get(true, db);
                    GvOffers.DataSource = list;
                    GvOffers.DataBind();
                }
            }


            void AddItem()
            {
                Clear();
                flag.Value = "2";
                ImgOffer.ImageUrl = TbName.Text = Editor2.Content = string.Empty;
            }


            void Clear()
            {
                MsgSuccess.Visible = MsgError.Visible = false;
                MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
            }


            protected void DeleteEvent(object sender, EventArgs e)
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var btn = sender as LinkButton;//HtmlButton;
                    if (btn == null) return;
                    var id = Convert.ToInt32(btn.ValidationGroup);
                    var detail = _entryRepository.Get(id, db);
                    if (detail != null &&
                        File.Exists(Server.MapPath(string.Format("~/_offerDetailsImages/{0}", detail.ImageUrl))))
                        File.Delete(Server.MapPath(string.Format("~/_offerDetailsImages/{0}", detail.ImageUrl)));

                    var deleted = _entryRepository.Delete(detail, db);

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
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    flag.Value = "1";
                    MsgSuccess.Visible = MsgError.Visible = false;
                    MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
                    var btn = sender as HtmlButton;
                    if (btn == null) return;
                    var id = Convert.ToInt32(btn.ValidationGroup);
                    selected.Value = btn.ValidationGroup;
                    var detail = _entryRepository.Get(true, db).FirstOrDefault(x => x.Id == id);
                    if (detail == null) return;
                    TbName.Text = detail.Name;
                    Editor2.Content = detail.Data;
                    ImgOffer.ImageUrl = detail.ImageUrl;
                }
            }



        private void Save()
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Editor2.Content == "")
                {
                    Label4.Visible = true;


                }
                else
                {
                    //if (string.IsNullOrWhiteSpace(FuImag.FileName))
                    //{
                    //    var x = Path.GetExtension(FuImag.FileName);
                    //    if (x != null)
                    //    {
                    //        var extension = x.ToLower();
                    //        if (!extension.Equals(".jpg") || !extension.Equals(".bmp") || !extension.Equals(".jpeg"))
                    //        {
                    //            MsgError.Visible = true;
                    //            MsgError.InnerHtml = string.Format("{0},{1}", Tokens.BadFileExtension, Tokens.OnlyJpgBmp);
                    //            return;
                    //        }
                    //    }
                    //}

                    var details = Editor2.Content;
                    var name = TbName.Text;
                   if(flag.Value=="2")

                   {
                    if (string.IsNullOrWhiteSpace(selected.Value))
                    {
                        if (string.IsNullOrWhiteSpace(FuImag.FileName))
                        {
                            MsgError.Visible = true;
                            MsgError.InnerHtml = Tokens.PleaseUploadImage;
                            return;
                        }

                         var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                         string ex = Path.GetExtension(FuImag.FileName);

                        if (!string.IsNullOrEmpty(ex) &&
                            extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                        {

                            string virtualName =
                                DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                            var fileName = virtualName + ".jpeg";

                            var item = new OffersDetail
                            {
                                Data = details,
                                Name = name,
                                ImageUrl = fileName
                            };
                            FuImag.SaveAs(Server.MapPath(string.Format("~/_offerDetailsImages/{0}", fileName)));
                            _entryRepository.Save(item, db);
                            MsgSuccess.Visible = true;
                            MsgSuccess.InnerHtml = Tokens.Saved;
                            MsgError.Visible = false;
                            MsgError.InnerHtml = string.Empty;
                            flag.Value = "0";
                            PopulateDetails();
                            return;
                        }
                        else
                        {
                            return;
                        }

                    }
                   }
                   else if (flag.Value == "1") { 
                    // edit
                    var id = Convert.ToInt32(selected.Value);
                    var detail = _entryRepository.Get(id, db);
                    detail.Name = name;
                    detail.Data = details;
                    if (!string.IsNullOrWhiteSpace(FuImag.FileName))
                    {
                        if (File.Exists(Server.MapPath(string.Format("~/_offerDetailsImages/{0}", detail.ImageUrl))))
                        {
                            File.Delete(Server.MapPath(string.Format("~/_offerDetailsImages/{0}", detail.ImageUrl)));
                        }
                          var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                         string ex = Path.GetExtension(FuImag.FileName);

                        if (!string.IsNullOrEmpty(ex) &&
                            extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                        {
                            string virtualName =
                                DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                            var fileName = virtualName + ".jpeg";
                            detail.ImageUrl = fileName;
                            FuImag.SaveAs(Server.MapPath(string.Format("~/_offerDetailsImages/{0}", fileName)));
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
                    _entryRepository.Save(detail, db);
                    flag.Value = "0";
                    PopulateDetails();
                   }
                }
            }
        }


        void Activate()
            {
                GvOffers.DataBound += (o, e) => Helper.GridViewNumbering(GvOffers, "LNo");
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
 