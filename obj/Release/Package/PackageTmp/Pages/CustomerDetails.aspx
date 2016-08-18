<%@ Page Title="<%$Resources:Tokens,CustomerDetails %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs" Inherits="NewIspNL.Pages.CustomerDetails" %>
<%@ Import Namespace="Resources" %>

<%@ Register TagPrefix="uc1" TagName="UserFile" Src="~/WebUserControls/UserFile.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <script src="../Scripts/jquery-1.7.1.min.js"></script>
    <script src="../Scripts/tooltip.min.js"></script>
    <link href="../Content/airview.min.css" rel="stylesheet" />
    <script src="../Scripts/airview.min.js"></script>
    <style type="text/css">
        #HlEdit{ text-decoration: none;}
    </style>
    <div class="page-header"><h1>
                <asp:Label ID="Label40" runat="server" Text="<%$Resources:Tokens,CustomerDetails %>"></asp:Label>
            </h1></div>
    <div>
        <fieldset>

            <p><asp:HyperLink runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="<%$Resources:Tokens,Edit %>" ID="HlEdit"></asp:HyperLink>
            </p>
            <table class="table table-bordered table-condensed"><!--table table-hover table-condensed table-striped white-->
            <tr><td colspan="5"><%=Tokens.CustomerDetails %></td></tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="<%$Resources:Tokens,Customer %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CustomerName" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label14" runat="server" Text="<%$Resources:Tokens,Governorate %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_GovernorateName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label33" runat="server" Text="<%$Resources:Tokens,Address %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CustomerAddress" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                                        <td>
                        <asp:Label ID="Label46" runat="server" Text="<%$Resources:Tokens,NationalId %>"></asp:Label>
                    </td>
                    <td style="width: 162px">
                        <asp:Label ID="lbl_NationaId" runat="server"></asp:Label>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label15" runat="server" Text="<%$Resources:Tokens, Phone%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CustomerPhone" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="<%$Resources:Tokens,Ip.Package %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_IpPackageName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" Text="<%$Resources:Tokens,Mobile %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CustomerMobile" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label21" runat="server" Text="<%$Resources:Tokens, Email%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CustomerEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Tokens,Mobile2 %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Mobile2" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="<%$Resources:Tokens, LineOwner%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_lineowner" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="<%$Resources:Tokens,Prepaid %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblprepaid" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="<%$Resources:Tokens, InstallationCost%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblinstalationcost" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                     <td>
                        <asp:Label ID="Label18" runat="server" Text="<%$Resources:Tokens, ContractingCost%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblconstractingsoct" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="<%$Resources:Tokens,CreationDate %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCreationDate" runat="server"></asp:Label>
                    </td>
                   
                </tr>
                <tr>
                    <td colspan="5"><%=Tokens.ServiceDetails %></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label22" runat="server" Text="<%$Resources:Tokens,Provider %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_SPName" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label23" runat="server" Text="<%$Resources:Tokens,Package %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ServicePackageName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label24" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ResellerName" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label25" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_BranchName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label26" runat="server" Text="<%$Resources:Tokens,VPI %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_VPI" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label27" runat="server" Text="<%$Resources:Tokens,VCI %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_VCI" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label28" runat="server" Text="<%$Resources:Tokens,UserName %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Client_UserName" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label29" runat="server" Text="<%$Resources:Tokens,Status %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_StatusName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label30" runat="server" Text="<%$Resources:Tokens,Password %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Password" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label32" runat="server" Text="<%$Resources:Tokens,Extra.Gigas %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ExtraGigas" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label35" runat="server" Text="<%$Resources:Tokens, PaymentType%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_PaymentType" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label47" runat="server" Text="<%$Resources:Tokens, Offer%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Offer" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label48" runat="server" Text="<%$Resources:Tokens,Activation.Date %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="l_activationdate" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text="<%$Resources:Tokens,RequestNumber %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblRequestnumber" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,WorkorderNumber %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblWorkorderNumber" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,WorkorderDate %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblWorkorderDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Tokens,RouterSerial %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblrouterserial" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                       
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                        <td>
                            <asp:Label runat="server" Text="<%$Resources:Tokens,FirstDateForOffer %>"></asp:Label>
                        </td>
                        <td>
                            <label runat="server" id="lblOfferStart">
                            </label>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="Label34" runat="server" Text="<%$Resources:Tokens,LastDateForOffer %>"></asp:Label>
                        </td>
                        <td>
                            <label runat="server" id="lblOfferEnd">
                            </label>
                        </td>
                    </tr>
                <tr>
                    <td colspan="5"><%=Tokens.CentralDetails %></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,PortNumber %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPortNumber" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,Block %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBlock" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,Dslam %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDslam" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Tokens,Central %>" ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblcentral" runat="server"></asp:Label>
                    </td>
                </tr>
                                <tr>
                    <td>
                        <asp:Label ID="lbl_Notes_Head" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="lbl_Notes" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="tr_Files" runat="server" >
        <div><fieldset>
            <uc1:UserFile ID="UserFile1" runat="server" CanEdit="False" /></fieldset>
        </div>
    </div>
    <div id="tr_Status">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Label ID="Label41" runat="server" Text="<%$Resources:Tokens,CustomerStatuseHistory %>"></asp:Label>
            </h3>
            <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="table table-bordered table-condensed text-center"
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="StatusName" HeaderText="<%$Resources:Tokens,Status %>"/>
                    <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens, Admin_Employee%>"/>
                    <asp:TemplateField HeaderText="Date-Time">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </div>
    <div id="tr_Requests">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Label ID="Label42" runat="server" Text="<%$Resources:Tokens,CustomerRequestsHistory %>"></asp:Label>
            </h3>
            <asp:GridView Width="100%" ID="grd_Requests" runat="server" AutoGenerateColumns="False"
                CellPadding="4" CssClass="table table-bordered table-condensed text-center" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="RequestName" HeaderText="<%$Resources:Tokens,Request.Type %>"/>
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$Resources:Tokens, Request.Date%>"/>
                    <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,WhoOrder %>"/>
                    <asp:BoundField DataField="UserName2" HeaderText="<%$Resources:Tokens,WhoManage %>"/>
                    <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,OldSpeed %>"/>
                    <asp:BoundField DataField="ServicePackageName2" HeaderText="<%$Resources:Tokens,New.Service.Package %>"/>
                    <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Amount %>"/>
                    <asp:BoundField DataField="RSName" HeaderText="<%$Resources:Tokens,RequestStatus %>"/>
                    <asp:BoundField DataField="RejectReason" HeaderText="<%$Resources:Tokens,RejectReason %>"/>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="Label35" runat="server" Text="No Requests"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
    </div>
    <div id="tr_Tickets">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Label ID="Label43" runat="server" Text="<%$Resources:Tokens, CustomerTicketHistory%>"></asp:Label>
            </h3>
            <%--<asp:GridView ID="grd_Tickets" runat="server" Width="100%" AutoGenerateColumns="False"
                CellPadding="4" ForeColor="#333333" GridLines="None">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governorate %>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens, Phone%>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens, Provider%>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="<%$Resources:Tokens,Ticket.Reason %>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Details" HeaderText="<%$Resources:Tokens, Details%>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Comment" DataField="<%$Resources:Tokens, Comment%>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="StatusName" HeaderText="<%$Resources:Tokens,Status %>">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TicketDate" HeaderText="<%$Resources:Tokens, OpenDate%>" />
                    <asp:BoundField DataField="CommentDate" HeaderText="<%$Resources:Tokens,SolvedDate %>" />
                </Columns>
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:Label ID="Label35" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$Resources:Tokens,NoTickects %>"></asp:Label>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>--%>
            <asp:GridView ID="grd_Tickets" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>"/>
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>"/>
                        <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>"/>
                        <asp:BoundField DataField="Title" HeaderText="<%$ Resources:Tokens,Ticket.Reason %>"/>
                        <asp:BoundField DataField="Details" HeaderText="<%$ Resources:Tokens,Details %>"/>
                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>"/>
                        <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:Tokens,Comment %>"/>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>"/>
                        <asp:BoundField DataField="TicketDate" HeaderText="<%$ Resources:Tokens,OpenDate %>" />
                        <asp:BoundField DataField="CommentDate" HeaderText="<%$ Resources:Tokens,SolvedDate %>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,DaysCount %>">
                            <ItemTemplate>
                                <asp:Literal ID="DaysCount" runat="server" Text='<%#Eval("ID") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,NoTickects %>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
        </fieldset>
    </div>
    <div id="tr_woInfo">
        <fieldset>
           <h3 class="header smaller lighter blue">
                <asp:Label ID="Label44" runat="server" Text="<%$Resources:Tokens,CustomerInfoHistory %>"></asp:Label>
            </h3>
            <asp:GridView  ID="grd_Info" runat="server" AutoGenerateColumns="False"
                CellPadding="4" CssClass="table table-bordered table-condensed text-center" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governorate %>"/>
                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>"/>
                    <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens, Provider%>"/>
                    <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens, Branch%>"/>
                    <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,WhoManage %>"/>
                    <asp:BoundField DataField="UpdateDate" HeaderText="<%$Resources:Tokens,UpdateDate %>"/>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="Label36" runat="server" Text="<%$Resources:Tokens,NoUpdates %>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
    </div>
    <div id="RequestDateHistory">
        <fieldset>
            <h3 class="header smaller lighter blue"><%=Tokens.ChangeRequestDate %></h3>
            <div>
                                    <asp:GridView runat="server" ID="GVRequestDateHistory" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>"/>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,User %>"/>
                            <asp:BoundField DataField="newRequestDate" HeaderText="<%$ Resources:Tokens,newRequestDate %>"/>
                            <asp:BoundField DataField="oldRequestDate" HeaderText="<%$ Resources:Tokens,oldRequestDate %>"/>
                            <asp:BoundField DataField="ChangeDate" HeaderText="<%$ Resources:Tokens,ProcessDate %>"/>
                            </Columns>
                                                    <EmptyDataTemplate>
                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                        </EmptyDataTemplate>
                            </asp:GridView>
            </div>
        </fieldset>
    </div>
    
     <div id="customerRouter" runat="server" >
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <%= Tokens.Routers %></h3>
                    <div>
                        <asp:GridView runat="server" ID="GVRouter" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed"
                            DataKeyNames="Id" OnDataBound="GVRouter_DataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RouterSerial" HeaderText="<%$Resources:Tokens,RouterSerial %>" />
                                <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Tokens,AddStore %>" />
                                <asp:BoundField DataField="CompanyUserName" HeaderText="<%$Resources:Tokens,RecieveFromCompany %>" />
                                <asp:BoundField DataField="CustomerUserName" HeaderText="<%$Resources:Tokens,CustomerConfirmerUser %>" />
                                <asp:BoundField DataField="CompanyDate" HeaderText="<%$Resources:Tokens,CompanyProcessDate %>" />
                                <asp:BoundField DataField="CustomerDate" HeaderText="<%$Resources:Tokens,CustomerProcessDate %>" />
                                <asp:BoundField DataField="IsRecieved" HeaderText="<%$Resources:Tokens,Done %>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a href='<%#Eval("Attach") %>' runat="server" class="btn btn-success btn-xs" target="_blank"
                                            id="link" visible="False"><i class="icon-only icon-download"></i></a>&nbsp;
                                        <a href='<%#Eval("Attach2") %>' runat="server" class="btn btn-primary btn-xs" target="_balnk"
                                            id="link2" visible="False"><i class="icon-only icon-download"></i></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
     <script type="text/javascript">
         $(document).ready(function() {
             $('img').each(function() {
                 $(this).airview({
                     width: 500,
                     container: 'body',
                     trigger: 'hover',
                     error: 'Sorry! No Image found',
                     //template: '<div class="airview" role="tooltip"><div class="airview-arrow"></div><div class="airview-inner"><div class="airview-loader"></div><img /></div></div>',
                     placement: 'right',
                     //html: true,
                     //animation: true,
                     url: false
                 });
             });
         });
              </script>
</asp:Content>
