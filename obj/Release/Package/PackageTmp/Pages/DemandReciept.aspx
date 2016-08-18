<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="DemandReciept.aspx.cs" Inherits="NewIspNL.Pages.DemandReciept" %>

<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Content/ace-assest/ReceiptFormate/style.css" rel="stylesheet" type="text/css" />
    <link href="../Content/ace-assest/ReceiptFormate/responsive.css" rel="stylesheet" type="text/css" />
   
            <style type="text/css">
        .smallcell
        {
            width: 92px;
        }
        .cell span
        {
            font-size: 74%;
            width: 72px;
            text-align: left;
        }
               
                 #companycorn {
                     -webkit-print-color-adjust:exact;
                      border: 2px solid #888;
                            /*border: 2px solid #c8cee5;*/
                        /*-ms-border-radius: 40px 10px;
                        border-radius: 40px 10px;*/
                     -ms-border-radius: 150px;
                     border-radius: 150px;
                        background-color: #EEE;
                         color: #666;
                        /*padding: 20px;*/
                        /*width: 150px;*/
                        width: auto;
                        height: 40px;
                        direction: rtl;
                        position: fixed;
                        /*margin-top: 10px;*/
                            /*margin-right: 305px;*/
                              margin-right: auto;
                        margin-bottom: 10px;
                            margin-top: 37px;
                             /*margin-left: 30px;*/
                        /*float: ;*/
                        clear: both;                       
                 }

        #logo {
                        position:relative;
                        -ms-border-radius:20px;
                        border-radius:20px;
                        overflow:hidden;

                        margin-bottom: 10px;
                    }

                   table.ftb td {
                         -webkit-print-color-adjust:exact;
                           border-color: #BBB;
                        -ms-border-radius: 0.30em;
                        border-radius: 0.30em;
                           border-style: solid;
                           border-width: 1px;
                       padding: 0.5em;
                       position: relative;
                       text-align: right;

                    }


                    td.clr {
                         -webkit-print-color-adjust:exact;
                        background: #EEE;
                        
                    }
                    table.ftb {
                         -webkit-print-color-adjust:exact;
                         margin-top: -8px;
                             border-collapse: separate;
                             border-spacing: 2px;
                             border-color: grey;
                             font-size: 75%;
                             table-layout: fixed;

                             float: right;

                               
                         }

                    table.tb {
                         width: 16cm;
                          margin-right: -20px;
                        /*max-width: 21cm;
                        font-size: 12pt;
                        line-height: inherit;
                       text-align: right;*/

                      
                        /*border-collapse:collapse;
                        margin-right: -20px;*/
                      

                        
                    }
               
                 #title {
                         margin-top: 20px;
                         position: absolute;
                       margin-right: 34px;
                     }
                  #HCompany {
                         font-size: large;
                         float: left;
                         margin-top: 0px;
                         margin-right: 3px;
                         margin-left: 3px;
                         /*width: 130px;*/
                         width: auto;
                      text-align: center;
                     }
                #box-inner {
                    background: none;
                    margin-top: 13px;
                }
                .signfooter {
                    float: left;
                    margin-left: 33px;


                     border-collapse: separate;
                             border-spacing: 2px;
                            
                             font-size: 75%;
                             table-layout: fixed;
                            

                               border-color: #BBB;
                        -ms-border-radius: 01.30em;
                        border-radius: 01.30em;
                           border-style: solid;
                           border-width: 1px;
                       /*padding: 0.5em;*/
                       padding: 1px;
                       position: relative;
                       text-align: right;
                      
                }
                .signfooter td {
                    -ms-border-radius: 30em;
                    border-radius: 30em;
                }

        #tabelfooter {
                    overflow: hidden;

                }
                #sign {
                   height: 60px;
                   width: 160px;

                }
                #Caution {
                   /*margin-left: -140px;*/
                }
                #compInformation {
                     /*height: 60px;*/
                   width: 160px;
                    margin-bottom: 0px;
                }
                .compInf {
                     float: right;
                     width: 330px;
                    margin-right: 33px;

                     border-collapse: separate;
                             border-spacing: 2px;
                             font-size: 75%;
                             table-layout: fixed;
                            
                               border-color: #BBB;
                        -ms-border-radius: 01.30em;
                        border-radius: 01.30em;
                           border-style: solid;
                           border-width: 1px;
                       /*padding: 0.5em;*/
                       padding: 1px;
                       position: relative;
                       text-align: right;
                       margin-bottom: 0px;
                }
                .compInf td {
                    -ms-border-radius: 30em;
                    border-radius: 30em;
                }

        #box-inner {
                    -ms-border-radius: 0.40em;
                    border-radius: 0.40em;
                }
                #compInformation p {
                    font-size: 8px !important;
                }
                .signimg {
                    float: left;
    margin-left: 35px;
    height: 86px;
    border-left: 1px solid #BBB;
    border-top: 1px solid #BBB;
    border-bottom: 1px solid #BBB;

                }


                @media print {
                    #btnPrint{
                        display: none
                    }
                    #companycorn {
                     -webkit-print-color-adjust:exact;
                      border: 2px solid #888;
                            /*border: 2px solid #c8cee5;*/
                        /*-ms-border-radius: 40px 10px;
                        border-radius: 40px 10px;*/
                     -ms-border-radius: 150px;
                     border-radius: 150px;
                        background-color: #EEE;
                         color: #666;
                        /*padding: 20px;*/
                        /*width: 150px;*/
                        width: auto;
                        height: 40px;
                        direction: rtl;
                        position: fixed;
                        /*margin-top: 10px;*/
                            /*margin-right: 305px;*/
                              margin-right: auto;
                        margin-bottom: 10px;
                            margin-top: 37px;
                             /*margin-left: 30px;*/
                        /*float: ;*/
                        clear: both;                       
                 }

        #logo {
                        position:relative;
                        -ms-border-radius:20px;
                        border-radius:20px;
                        overflow:hidden;

                        margin-bottom: 10px;
                    }

                   table.ftb td {
                         -webkit-print-color-adjust:exact;
                           border-color: #BBB;
                        -ms-border-radius: 0.30em;
                        border-radius: 0.30em;
                           border-style: solid;
                           border-width: 1px;
                       padding: 0.5em;
                       position: relative;
                       text-align: right;

                    }


                    td.clr {
                         -webkit-print-color-adjust:exact;
                        background: #EEE;
                        
                    }
                    table.ftb {
                         -webkit-print-color-adjust:exact;
                         margin-top: -8px;
                             border-collapse: separate;
                             border-spacing: 2px;
                             border-color: grey;
                             font-size: 75%;
                             table-layout: fixed;

                             float: right;

                               
                         }

                    table.tb {
                         width: 16cm;
                          margin-right: -20px;
                        /*max-width: 21cm;
                        font-size: 12pt;
                        line-height: inherit;
                       text-align: right;*/

                      
                        /*border-collapse:collapse;
                        margin-right: -20px;*/
                      

                        
                    }
               
                 #title {
                         margin-top: 20px;
                         position: absolute;
                       margin-right: 34px;
                     }
                  #HCompany {
                         font-size: large;
                         float: left;
                         margin-top: 0px;
                         margin-right: 4px;
                         margin-left: 4px;
                         /*width: 130px;*/
                         width: auto;
                      text-align: center;
                     }
                #box-inner {
                    background: none;
                    margin-top: 13px;
                }
                .signfooter {
                    float: left;
                    margin-left: 33px;


                     border-collapse: separate;
                             border-spacing: 2px;
                            
                             font-size: 75%;
                             table-layout: fixed;
                            

                               border-color: #BBB;
                        -ms-border-radius: 01.30em;
                        border-radius: 01.30em;
                           border-style: solid;
                           border-width: 1px;
                       /*padding: 0.5em;*/
                       padding: 1px;
                       position: relative;
                       text-align: right;
                      
                }
                .signfooter td {
                    -ms-border-radius: 30em;
                    border-radius: 30em;
                }

        #tabelfooter {
                    overflow: hidden;

                }
                #sign {
                   height: 60px;
                   width: 160px;

                }
                #Caution {
                   /*margin-left: -140px;*/
                }
                #compInformation {
                     /*height: 60px;*/
                   width: 160px;
                    margin-bottom: 0px;
                }
                .compInf {
                     float: right;
                     width: 330px;
                    margin-right: 33px;

                     border-collapse: separate;
                             border-spacing: 2px;
                             font-size: 75%;
                             table-layout: fixed;
                            
                               border-color: #BBB;
                        -ms-border-radius: 01.30em;
                        border-radius: 01.30em;
                           border-style: solid;
                           border-width: 1px;
                       /*padding: 0.5em;*/
                       padding: 1px;
                       position: relative;
                       text-align: right;
                       margin-bottom: 0px;
                }
                .compInf td {
                    -ms-border-radius: 30em;
                    border-radius: 30em;
                }

        #box-inner {
                    -ms-border-radius: 0.40em;
                    border-radius: 0.40em;
                }
                #compInformation p {
                    font-size: 8px !important;
                }
                .signimg {
                    float: left;
    margin-left: 35px;
    height: 86px;
    border-left: 1px solid #BBB;
    border-top: 1px solid #BBB;
    border-bottom: 1px solid #BBB;

                }
                }

            </style>
    

</head>
<body style="direction: rtl;">
    <form id="form1" runat="server">
         <div id="box">
    <div id="all"> 

                    <div id="box-header">
                       
                        <div>
                <div id="logo">
                    <asp:Image runat="server" ID="LImg" />
                </div>
                  
                        </div>
                <div id="title">
                   
                 <table class="ftb">
                     <tr>
                         <td class="clr">
                             بواسطة :
                         </td>
                          <td>
                              <asp:Literal ID="Emp" runat="server" />
                         </td>
                     </tr>
                     <tr>
                         <td class="clr">
                             التاريخ : 
                         </td>
                         <td>
                             <%= DateTime.Now.AddHours().ToString("yyyy-M-d dddd", System.Globalization.CultureInfo.InvariantCulture)%> 
                         </td>
                     </tr>
                     <tr>
                         <td class="clr">
                              <asp:Literal runat="server" ID="lblRecieptNum" Text="رقم الايصال :"></asp:Literal>
                         </td>
                         <td>
                             <asp:Literal runat="server" ID="RecieptNum"></asp:Literal>
                         </td>
                     </tr>
                 </table>                                  
                           
                </div>
                          <div id="companycorn">
                    <h3 style="margin: 0px 0 0 0;"><label class="stretch" id="HCompany" runat="server">
                    </label></h3></div>
            </div>
            


        <div id="box-inner"><!--style="font-size: 13pt; width: 100%; line-height: 12pt; font-weight: 700"-->
                
            
                     <div class="inner">
                         
                          <table class="ftb tb" >
                     <tr>
                         <td width="15%" class="clr">
                            العميل
                         </td>
                          <td width="20%">
                              <asp:Literal ID="Customer" runat="server" />
                         </td>
                         <td width="15%" class="clr">
                             تليفون
                         </td>
                         <td width="35%">
                              <asp:Literal ID="Phone" runat="server" />
                        </td>
                          
                     </tr>
                     <tr>
                         <td width="15%" class="clr">
                            من
                         </td>
                          <td width="30%">
                              <asp:Literal ID="StartAt" runat="server" />
                         </td>
                          <td width="15%" class="clr">
                             الى
                         </td>
                         <td>
                             <asp:Literal ID="EndAt" runat="server" />
                        </td>
                     </tr>
                     <tr>
                         <td class="clr">
                              مزود الخدمة
                         </td>
                         <td>
                             <asp:Literal ID="lblServiceProvider" runat="server" />
                         </td>
                          <td class="clr">
                              السرعة
                         </td>
                         <td>
                              <asp:Literal ID="Pack" runat="server" />
                         </td>
                        
                     </tr>
                      <tr>
                         <td class="clr" >
                             ملاحظات
                         </td>
                         <td colspan="3">
                              <asp:Literal ID="Notes" runat="server" />
                         </td>
                     </tr>    
                     <tr>
                         <td class="clr">
                           المبلغ
                         </td>
                         <td>
                             <asp:Literal ID="Amount" runat="server" />
                         </td>
                          <td class="clr">
                              الإمضاء
                         </td>
                         <td>
                              <asp:Literal ID="ss" runat="server" />
                         </td>
                     </tr>
                          
                 </table>        
                         

                            <%-- last comment --%>
                     <%--  <div class="cell">
                        <span>العميل</span>
                        <div class="field">
                            <asp:Literal ID="Customer" runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>تليفون</span>
                        <div class="field">
                            <asp:Literal ID="Phone" runat="server" />
                        </div>
                    </div>--%>
                            <%-- last comment --%>  
                            

                                                           <%-- <div class="cell">
                        <span>الموبيل</span>
                        <div class="field">
                            <asp:Literal ID="lblMobile" runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>المحافظة</span>
                        <div class="field">
                            <asp:Literal ID="Gov" runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>سنترال</span>
                        <div class="field">
                            <asp:Literal ID="Central" runat="server" />
                        </div>
                    </div>--%>
                            
                            
                              <%-- last comment --%>
                                       <%-- <div class="cell">
                        <span>مزود الخدمة</span>
                        <div class="field">
                            <asp:Literal ID="lblServiceProvider" runat="server" />
                        </div>
                    </div>
                
             <div class="cell">
                        <span>السرعة</span>
                        <div class="field">
                            <asp:Literal ID="Pack" runat="server" />
                        </div>
                    </div>
             <div class="cell">
                        <span>المبلغ</span>
                        <div class="field">
                            <asp:Literal ID="Amount" runat="server" />
                        </div>
                    </div>
                            </div>
                                <div class="inner">--%>
                                      <%-- last comment --%>
                                    

                   <%-- <div class="cell">
                        <span>السرعة</span>
                        <div class="field">
                            <asp:Literal ID="Pack" runat="server" />
                        </div>
                    </div>--%>
                                                           <%-- <div class="cell">
                        <span>العنوان</span>
                        <div class="field">
                            <asp:Literal ID="lblAddress" runat="server" />
                        </div>
                    </div>--%>
                                       <%-- <div class="cell">
                        <span>المبلغ</span>
                        <div class="field">
                            <asp:Literal ID="Amount" runat="server" />
                        </div>
                    </div>--%>
                                    

                                      <%-- last comment --%>
                                       <%-- <div class="cell">
                        <span>من</span>
                        <div class="field">
                            <asp:Literal ID="StartAt" runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>الى</span>
                        <div class="field">
                            <asp:Literal ID="EndAt" runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>ملاحظات</span>
                        <div class="field" style="height: auto">
                            <asp:Literal ID="Notes" runat="server" />
                        </div>
                    </div>
                                     <div class="cell">
                        <span>الإمضاء</span>
                        <div class="field">
                            <asp:Literal ID="ss" runat="server" />
                        </div>
                    </div>
                                     <div class="cell">
                        <span>الختم</span>
                        <div style="min-height: 75px; border:1px solid black;" class="field">
                            
                        </div>
                    </div>--%>
             <%-- last comment --%>
                </div>
           

            <%--<table id="rcpt-content" width="100%" style="text-align: right; border-color: rgb(184, 184, 184);
                border-left: 0; border-right: 0;">
                <tr>
                    <td ><!--class="span2"-->
                        العميل
                    </td>
                    <td ><!--class="span1"-->
                        :
                    </td>
                    <td ><!--class="span8"-->
                        <asp:Literal ID="Customer" runat="server" />
                    </td>
                    <td>
                        السرعة
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Pack" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        تليفون
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Phone" runat="server" />
                    </td>
                    <td>
                        المبلغ
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Amount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        المحافظة
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Gov" runat="server" />
                    </td>
                    <td>
                        من
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="StartAt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        السنترال
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Central" runat="server" />
                    </td>
                    <td>
                        الى
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="EndAt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        مزود الخدمة 
                    </td>
                    <td> : </td>
                    <td><asp:Literal runat="server" ID="lblServiceProvider"></asp:Literal></td>
                    <td>
                        ملاحظات
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Notes" runat="server" />
                    </td>
                </tr>
            </table>--%>
        </div>
        

<%--        <table style="text-align: center">
            <tr>
                <td class="span12" colspan="2">
                    <b runat="server" id="Caution"></b>
                </td>
            </tr>
            <tr style="text-align: right;">
                <td>
                    <asp:Literal ID="Address" runat="server" />
                </td>
                <td>ختم :</td>
            </tr>
        </table>--%>
         <div style="margin: 0px; padding:0px; clear: both; height: 6px">
         &nbsp;
       </div>
                    
        

        <div id="tabelfooter">
          <%--  <img src="../PrintLogos/sign.jpg" alt="sign" class="signimg" />--%>
             <table class="signfooter" >
                 <%--<tr><td class="to-center"><b runat="server" id="Caution"></b></td></tr>--%>
           <tr style="text-align: center;">
               <td class="clr">الختـم</td>
           </tr>
           <tr><td  rowspan="3" id="sign" style="height: 67px">
             
           </td></tr>
       </table>
            
            <table class="compInf">
                <tr>
                    <td class="clr" style="text-align: center;" >Company</td>
                </tr>
                 <tr>
                    <td  rowspan="3" id="compInformation">
                        <%-- <asp:Literal ID="Address" runat="server" />--%>
                        <asp:Label ID="Address" runat="server" Text="" style="margin-bottom: 0px; font-size: 6pt;"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="to-center" id="cautiondiv">
                <b runat="server" id="Caution"></b>
            </div>
        </div>



<%--            <table class="to-center" id="tabelfooter">
                <tr style="text-align: right;">
                    <td>--%>

                      <%--  old comment--%>
                      <%--  <div style="margin-left: 80px; margin-top: 5px; float: left">
                        <asp:Label  ID="sig" runat="server" Text="<%$Resources:Tokens,sign %> "></asp:Label>:
                   </div>--%>
                    <%--  old comment--%>

                        <%--<div>
                        <asp:Literal ID="Address" runat="server" />
                        </div>
                         </td>
                </tr>
            </table>--%>
        

    </div>
    </div>
       
        <button class="btn btn-success" id="btnPrint"><%=Tokens.Print %></button> 
    <script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script> 
     <script type="text/javascript">
         $(document).ready(function () {
             if (window.matchMedia("screen").matches) {
                 var width = $("#HCompany").width();
                 if (width >= 190 && width < 280) {
                     $("#companycorn").css("margin-right", "250px");
                 } else if (width > 170 && width <= 190) {
                     $("#companycorn").css("margin-right", "290px");

                 } else if (width < 170 && width >= 100) {
                     $("#companycorn").css("margin-right", "305px");
                     //$("#HCompany").width("220");
                 } else if (width <= 130) {
                     $("#companycorn").css("margin-right", "305px");
                     $("#HCompany").width("130");
                 } else if (width >= 280) {
                     $("#companycorn").css("margin-right", "225px");
                 }
             }
             var width = $("#HCompany").width();
             if (width >= 190 && width < 280) {
                 $("#companycorn").css("margin-right", "250px");
             } else if (width > 170 && width <= 190) {
                 $("#companycorn").css("margin-right", "290px");

             } else if (width < 170 && width >= 100) {
                 $("#companycorn").css("margin-right", "305px");
                 //$("#HCompany").width("220");
             } else if (width <= 130) {
                 $("#companycorn").css("margin-right", "305px");
                 $("#HCompany").width("130");
             } else if (width >= 280) {
                 $("#companycorn").css("margin-right", "225px");
             }


             $('#btnPrint').click(function () {
                 $('#box').replaceWith($("#box").html());
                 window.print();
             });

             var afterPrint = function () {
                 window.close();
             };


             //function PrintWindow() {
             //    window.print();
             //}

             //PrintWindow();

             //--CLOSE After print For firefox only
             window.onafterprint = afterPrint;

             //--CLOSE After print For chrome and other browsers
             if (window.matchMedia) {
                 var mediaQueryList = window.matchMedia('print');
                 mediaQueryList.addListener(function (mql) {
                     if (mql.matches) {
                         //do some code before print
                     } else {
                         $(document).one('mouseover', afterPrint);
                     }
                 });
             }
         });
    </script>
    </form>
</body>
</html>
