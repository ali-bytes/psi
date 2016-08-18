using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerEvaluation : CustomPage
    {
      
    readonly IspDomian _domian;

    readonly WorkOrderService _orderService;


    public ResellerEvaluation(){
        var context = IspDataContext;
        _orderService = new WorkOrderService(context);
        _domian = new IspDomian(context);
    }


    protected void Page_Load(object sender, EventArgs e)
    { //ToDo: modify  
        DataList1.Visible = false;
          DataList2.Visible = false;
          DataList3.Visible = false;

        Activate();
        if (IsPostBack) return;
        _domian.PopulateResellers(DdlReseller);
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
           var user= UserServices.Get(userId, context);
           if(user.GroupID!=6) Helper.AddDropDownItem(DdlReseller, 1, Tokens.All);
            Helper.AssignFirstAndLastDateOfMonth(TbStartAt, TbTo, DateTime.Now.AddHours());
        }
    }


    void Activate(){
        BSearch.ServerClick += (o, e) => Search();
        GvToNewCustomer.DataBound += (o, e) => Helper.GridViewNumbering(GvToNewCustomer, "LNo");
        GvSuspend.DataBound += (o, e) => Helper.GridViewNumbering(GvSuspend, "LNo");
        GvCancelled.DataBound += (o, e) => Helper.GridViewNumbering(GvCancelled, "LNo");
        GridView4.DataBound += (o, e) => Helper.GridViewNumbering(GridView4, "LNo");
    }


    void Search(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            List<WorkOrder> list;
            if (DdlReseller.SelectedIndex > 0 && DdlReseller.SelectedIndex == 1)
            {
                list = context.WorkOrders.Where(a => a.ResellerID != null).ToList();
            }
            else
            {
                list = context.WorkOrders.Where(x => x.ResellerID == Convert.ToInt32(DdlReseller.SelectedItem.Value)).ToList();
            }
             
            totalCount.Text = string.Format("{0}", list.Count);
            divcustomerCount.Visible = list.Count > 0 ? true : false;
            var start = Convert.ToDateTime(TbStartAt.Text);
            var end = Convert.ToDateTime(TbTo.Text);
            var toNewOrders = _orderService.WorkOrdersInStateCount(start, end, 0, list).Select(x => WorkOrderRepository.GetOrderBasicData(x, context)).ToList();
            GvToNewCustomer.DataSource = toNewOrders;
            GvToNewCustomer.DataBind();

            //ToDo: modify  
            List<info> chart2 = new List<info>();
            List<info> chart = new List<info>();
            var sss = new info();
            var sss2 = new info();
            var sss3 = new info();
            var sss4 = new info();
            List<info> addcount2 = new List<info>();
            List<info> addcount = new List<info>();
            var providers = (from g in db.ServiceProviders select g).ToList();
                foreach (var serviceProvider in providers)
                {
                    var coun = 0;
                    var ssss = new info();
            foreach (var orderBasicData in toNewOrders)
            { 
               
               
                    if(orderBasicData.Provider==serviceProvider.SPName)
                    {
                        
                        var check = addcount.Where(x => x.pro_name == serviceProvider.SPName).Select(x => x).ToList();
                        if(check.Count>0)
                        { }
                      coun = ++coun;
        
             }
                
                   
                
            }
            if (coun!=0)
            {ssss.coun =coun;
                    ssss.pro_name = serviceProvider.SPName;
                    addcount2.Add(ssss);}
                    
                    
                    
          
            }
                DataList1.Visible = true;
                DataList1.DataSource = addcount2;
                DataList1.DataBind();
            var newcus = addcount2.Select(x => x.coun).Sum();
            sss.pro_name = "(" + newcus.ToString() + ")" + " " + "عميل جديد";
            sss.coun = newcus;
            chart.Add(sss);



            var toSuspend = _orderService.WorkOrdersInStateCount(start, end, 11, list).Where(s=>s.WorkOrderStatusID==11).Select(x => WorkOrderRepository.GetOrderBasicData(x, context)).ToList();
            GvSuspend.DataSource = toSuspend;
            GvSuspend.DataBind();

             //ToDo: modify  
            List<info> addcount4 = new List<info>();
            List<info> addcount3 = new List<info>();
           
                foreach (var serviceProvider in providers)
                {
                    var coun = 0;
                    var ssss = new info();
            foreach (var orderBasicData in toSuspend)
            { 
               
               
                    if(orderBasicData.Provider==serviceProvider.SPName)
                    {
                        
                        var check = addcount3.Where(x => x.pro_name == serviceProvider.SPName).Select(x => x).ToList();
                        if(check.Count>0)
                        { }
                      coun = ++coun;
        
             }
                
                   
                
            }
            if (coun!=0)
            {ssss.coun =coun;
                    ssss.pro_name = serviceProvider.SPName;
                    addcount4.Add(ssss);}
                    
                    
                    
          
            }
                DataList2.Visible = true;
                DataList2.DataSource = addcount4;
                DataList2.DataBind();
                var suscus = addcount4.Select(x => x.coun).Sum();
                sss2.pro_name = "(" + suscus.ToString() + ")" + " " + "ايقاف مؤقت";
                sss2.coun = suscus;
                chart.Add(sss2);




            var cancelOreders = _orderService.WorkOrdersInStateCount(start, end, 8, list);
            cancelOreders.AddRange(_orderService.WorkOrdersInStateCount(start, end, 9, list));
            var toCancelled = cancelOreders.Select(x => WorkOrderRepository.GetOrderBasicData(x, context)).ToList();
            GvCancelled.DataSource = toCancelled;
            GvCancelled.DataBind();

            //ToDo: modify  
            List<info> addcount5 = new List<info>();
            List<info> addcount6 = new List<info>();

            foreach (var serviceProvider in providers)
            {
                var coun = 0;
                var ssss = new info();
                foreach (var orderBasicData in toCancelled)
                {


                    if (orderBasicData.Provider == serviceProvider.SPName)
                    {

                        var check = addcount5.Where(x => x.pro_name == serviceProvider.SPName).Select(x => x).ToList();
                        if (check.Count > 0)
                        { }
                        coun = ++coun;

                    }



                }
                if (coun != 0)
                {
                    ssss.coun = coun;
                    ssss.pro_name = serviceProvider.SPName;
                    addcount6.Add(ssss);
                }




            }
            DataList3.Visible = true;
            DataList3.DataSource = addcount6;
            DataList3.DataBind();

            var cancus = addcount6.Select(x => x.coun).Sum();
            sss3.pro_name = "(" + cancus.ToString() + ")" + " " + "الغاء";
            sss3.coun = cancus;
            chart.Add(sss3);




//ToDo: modify 


            List<WorkOrder> list2;
            if (DdlReseller.SelectedIndex > 0 && DdlReseller.SelectedIndex == 1)
            {
                list2 = context.WorkOrderStatus.Where(a => a.WorkOrder.ResellerID != null&&a.WorkOrder.WorkOrderStatusID==6 &&a.IsNew==true ).Select(x=>x.WorkOrder).ToList();
            }
            else
            {
                list2 = context.WorkOrderStatus.Where(x => x.WorkOrder.ResellerID == Convert.ToInt32(DdlReseller.SelectedItem.Value) && x.WorkOrder.WorkOrderStatusID == 6 && x.IsNew == true).Select(x => x.WorkOrder).ToList();
            }
            var activeOreders = _orderService.WorkOrdersInStateCountactive(start, end, 6, list2);
          
            var toactive = activeOreders.Select(x => WorkOrderRepository.GetOrderBasicData(x, context)).ToList();
            GridView4.DataSource = toactive;
            GridView4.DataBind();

             
            List<info> addcount7 = new List<info>();
            List<info> addcount8 = new List<info>();

            foreach (var serviceProvider in providers)
            {
                var coun = 0;
                var ssss = new info();
                foreach (var orderBasicData in toactive)
                {


                    if (orderBasicData.Provider == serviceProvider.SPName)
                    {

                        var check = addcount7.Where(x => x.pro_name == serviceProvider.SPName).Select(x => x).ToList();
                        if (check.Count > 0)
                        { }
                        coun = ++coun;

                    }



                }
                if (coun != 0)
                {
                    ssss.coun = coun;
                    ssss.pro_name = serviceProvider.SPName;
                    addcount8.Add(ssss);
                }




            }
            DataList4.Visible = true;
            DataList4.DataSource = addcount8;
            DataList4.DataBind();
            var actcus = addcount8.Select(x => x.coun).Sum();

            sss4.pro_name ="("+ actcus.ToString()+")"+" " + "مفعل";
            sss4.coun = actcus;
            chart.Add(sss4);

         

            Chart1.DataSource = chart;
            Chart1.Series["Series1"].XValueMember = "pro_name" ;
            Chart1.Series["Series1"].YValueMembers = "coun";
            Chart1.DataBind();
     
        
 
         //graph2 new
            var newcusnow = new List<WorkOrder>();
            if (DdlReseller.SelectedIndex > 0 && DdlReseller.SelectedIndex == 1)
            {
                newcusnow =
                    (from g in db.WorkOrders select g).ToList();
            }
            else
            {
                newcusnow =
                   (from g in db.WorkOrders where g.ResellerID==Convert.ToInt32(DdlReseller.SelectedItem.Value) select g).ToList();
            }
            var status = (from g in db.Status select g).ToList();
            List<info> addcount9 = new List<info>();
            List<info> addcount10 = new List<info>();
           


            foreach (var statuse in status)
            {
                var coun = 0;
               
                foreach (var orderBasicData in newcusnow)
                {


                    if (orderBasicData.Status.StatusName == statuse.StatusName)
                    {

                        var check = addcount9.Where(x => x.pro_name == statuse.StatusName).Select(x => x).ToList();
                        if (check.Count > 0)
                        { }
                        coun = ++coun;

                    }



                }
                if (coun != 0)
                {
                    var sss5 = new info();
                    sss5.coun = coun;
                    sss5.pro_name = statuse.StatusName;
                    addcount10.Add(sss5);
                }

            }
            var li = addcount10;
            foreach (var info2 in li)
            {
                var sss6 = new info();
                sss6.pro_name = "(" + info2.coun.ToString() + ")" + " " + info2.pro_name.ToString();
                sss6.coun = Convert.ToInt32(info2.coun);
                chart2.Add(sss6);




            

                Chart2.DataSource = chart2;
            Chart2.Series["Series1"].XValueMember = "pro_name";
            Chart2.Series["Series1"].YValueMembers = "coun";

            Chart2.DataBind();
            }
        }
    }

  

    //ToDo: modify  
    ISPDataContext db=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
    public class info
    {
        
      public string pro_name { get; set; }
      public string status_name { get; set; }
      public int coun { get; set; }
    }


      

    }
}