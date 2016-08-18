using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

//using ScipBe.Common.Office;


namespace NewIspNL.Helpers{
    public static class Helper{
        public static void GridViewNumbering(GridView gridViewId, string numberPlaceHolderLabelId){
            foreach(GridViewRow row in gridViewId.Rows){
               var text = (row.RowIndex + 1).ToString(CultureInfo.InvariantCulture);
                var labelPlaceHolder = row.FindControl(numberPlaceHolderLabelId) as Label;
                if(labelPlaceHolder == null){
                    continue;
                }
                labelPlaceHolder.Text = text;

                var literal=row.FindControl(numberPlaceHolderLabelId) as Literal;
                if(literal == null){
                    continue;
                }
                literal.Text = text;
            }
        }


        public static void BindDrop(DropDownList ddl, object dataSource, string text, string val, bool addDefault = true, string defaultText = ""){
            ddl.DataSource = dataSource;
            ddl.DataTextField = text;
            ddl.DataValueField = val;
            ddl.DataBind();
            if(addDefault){
                if(!string.IsNullOrEmpty(defaultText)){
                    AddDefaultItem(ddl, defaultText);
                } else{
                    AddDefaultItem(ddl);
                }
            }
        }


        public static void HideShowControl(GridView gridViewId, string controlId, bool hide){
            foreach(GridViewRow row in gridViewId.Rows){
                var control = row.FindControl(controlId);
                if(control != null) control.Visible = hide;
            }
        }


        public static void AddDefaultItem(DropDownList list, string text = "--Chose--", int index = 0){
            var loc = new Loc();
            text = loc.IterateResource(text == "--Chose--" ? "Chose" : text);
            var item = new ListItem(text, ""){
                Value = string.Empty
            };
            list.Items.Insert(index, item);
            list.SelectedIndex = list.Items.IndexOf(item);
        }


        public static void AddDropDownItem(DropDownList list, int itemIndex, string text, string value = "0"){
            var item = new ListItem(text, value);
            list.Items.Insert(itemIndex, item);
            list.SelectedIndex = itemIndex;
        }


        public static void Populate1To12(DropDownList drop){
            var count = new List<int>();
            for(var i = 1;i < 12;i++) count.Add(i);
            drop.DataSource = count;
            drop.DataBind();
            AddDefaultItem(drop);
        }


        public static void PopulateMonths(DropDownList drop){
            PopulateDrop(FillMonthsByTokens(), drop, "Id", "Name");
        }


        public static Dictionary<int, string> FillMonths(){
            var names = new Dictionary<int, string>{
                {
                    1, Tokens.Jan
                },{
                    2, Tokens.Feb
                },{
                    3, Tokens.Mar
                },{
                    4, Tokens.Apr
                },{
                    5, Tokens.May
                },{
                    6, Tokens.Jun
                },{
                    7, Tokens.Jul
                },{
                    8, Tokens.Aug
                },{
                    9, Tokens.Sep
                },{
                    10, Tokens.Oct
                },{
                    11, Tokens.Nov
                },{
                    12, Tokens.Dec
                }
            };

            return names;
        }


        public static List<IdName> FillMonthsByTokens(){
            var names = new List<IdName>{
                new IdName{
                    Id = 1,
                    Name = Tokens.Jan
                },
                new IdName{
                    Id = 2,
                    Name = Tokens.Feb
                },
                new IdName{
                    Id = 3,
                    Name = Tokens.Mar
                },
                new IdName{
                    Id = 4,
                    Name = Tokens.Apr
                },
                new IdName{
                    Id = 5,
                    Name = Tokens.May
                },
                new IdName{
                    Id = 6,
                    Name = Tokens.Jun
                },
                new IdName{
                    Id = 7,
                    Name = Tokens.Jul
                },
                new IdName{
                    Id = 8,
                    Name = Tokens.Aug
                },
                new IdName{
                    Id = 9,
                    Name = Tokens.Sep
                },
                new IdName{
                    Id = 10,
                    Name = Tokens.Oct
                },
                new IdName{
                    Id = 11,
                    Name = Tokens.Nov
                },
                new IdName{
                    Id = 12,
                    Name = Tokens.Dec
                }
            };

            return names;
        }


        public static string FixNumberFormat(decimal amount){
            
            return string.Format("{0:####.##}", amount) == string.Empty ? "0.0" : string.Format("{0:####.##}", amount);
            
        }


        public static string FixNumberFormat(double amount){
            return string.Format("{0:####.##}", amount) == string.Empty ? "0.0" : string.Format("{0:####.##}", amount);
        }


        public static string FixNumberFormat(decimal ? amount){
            return string.Format("{0:####.##}", amount) == string.Empty ? "0.0" : string.Format("{0:####.##}", amount);
        }


        public static string FixNumberFormat(double ? amount){
            return string.Format("{0:####.##}", amount) == string.Empty ? "0.0" : string.Format("{0:####.##}", amount);
        }


        public static List<int> FillInts(int start, int final){
            var years = new List<int>();
            for(var i = start;i <= final;i++){
                years.Add(i);
            }
            return years;
        }


        public static List<int> FillYears(int startYear, int endYear){
            var years = new List<int>();
            for(var i = startYear;i <= endYear;i++){
                years.Add(i);
            }
            return years;
        }


        /*public static HttpPostedFile LoadExcelFile(FileUpload fileUpload, List<string> extentions, string path,
            Page page, string fileName){
            var file = fileUpload.PostedFile;
            var extention = Path.GetExtension(file.FileName);
            if(extentions.Any(currentExtention => currentExtention == extention)){
                file.SaveAs(page.Server.MapPath(string.Format("~/{0}/{1}", path, file.FileName)));
                return file;
            }
            return null;
        }*/


        public static void AddAllDefaultItem(Control control){
            var drop = control as DropDownList;
            if(drop != null){
                if(drop.Items.OfType<ListItem>().ToList().All(i => i.Text != Tokens.Chose)){
                    AddDefaultItem(drop);
                }
            }

            if(control.HasControls()){
                foreach(Control current in control.Controls){
                    AddAllDefaultItem(current);
                }
            }
        }


        /*public static ExcelProvider LoadExcelProvider(string path, Page page, string sheetName = "Sheet1"){
            var provider = ExcelProvider.Create(page.Server.MapPath(path), sheetName);
            return provider;
        }*/


        public static int ? GetDropValue(DropDownList drp){
            if(drp.SelectedIndex > 0){
                return Convert.ToInt32(drp.SelectedItem.Value);
            }
            return null;
        }


        public static void Reset(Control container){
            foreach(var control in container.Controls){
                var text = control as TextBox;
                if(text != null){
                    text.Text = string.Empty;
                }
            }
            foreach(var control in container.Controls){
                var text = control as DropDownList;
                if(text != null){
                    text.SelectedIndex = -1;
                }
            }
        }



        /*public static Button GetButton(object sender){
            return sender as Button;
        }*/

        public static LinkButton GetLinkButton(object sender)
        {
            return sender as LinkButton;
        }
        public static void PopulateDrop(object dataSource, 
            DropDownList dropId, string dataValueField, string dataTextField,
            bool addDefaultItem = true,string defaultItemtext=""){
            dropId.DataSource = dataSource;
            dropId.DataValueField = dataValueField;
            dropId.DataTextField = dataTextField;
            dropId.DataBind();
            if(addDefaultItem){
                if (string.IsNullOrWhiteSpace(defaultItemtext))
                {
                    AddAllDefaultItem(dropId);    
                } else{
                    AddDefaultItem(dropId, defaultItemtext);
                }
                
            }
        }


        public static void PopulateDrop(object dataSource, DropDownList dropId, bool addDefaultItem = true){
            dropId.DataSource = dataSource;
            dropId.DataBind();
            if(addDefaultItem){
                AddDefaultItem(dropId);
            }
        }


        public static void EmptyDrop(DropDownList drop){
            drop.DataSource = null;
            drop.DataBind();
        }


        public static void AddTextBoxesText(TextBox[] textBoxs, string txt){
            if(textBoxs.Any()){
                textBoxs.ToList().ForEach(x=>x.Text=txt);
            }
        }


        public static void AssignFirstAndLastDateOfMonth(TextBox txtStart, TextBox txtEndTo,DateTime time){
            var first = new DateTime(time.Year, time.Month, 1);
            var end = first.AddMonths(1).AddDays(-1);
            txtStart.Text = first.ToShortDateString();
            txtEndTo.Text = end.ToShortDateString();
        }
       
    }

    public class IdName{
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
