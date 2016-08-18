<%@ Page Title="<%$Resources:Tokens,UserTracking%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UserTrack.aspx.cs" Inherits="NewIspNL.Pages.UserTrack" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
     
    <div class="view">
        <asp:Panel ID="p_search" runat="server">
             <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,Search %>"></asp:Literal></h1></div>
            <div class="span6">
                <div class="well">
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Employee %>"></asp:Label>
                    <div>
                        <div>
                            <asp:DropDownList ID="ddl_employee" runat="server" Width="200px" ClientIDMode="Static" CssClass="chosen">
                            </asp:DropDownList>
                            </div>
                    </div>

                     <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Process %>"></asp:Label>
                    <div>
                        <div>
                            <asp:DropDownList ID="ddlProcess" runat="server" Width="200px" ClientIDMode="Static" CssClass="chosen" >
                            </asp:DropDownList>
                           </div>
                    </div>

                    <div>
                        <asp:Label ID="l_from" runat="server" Text="<%$Resources:Tokens,From %>"></asp:Label>
                        <div>
                            <asp:TextBox ID="tb_from" runat="server" Width="200px" ClientIDMode="Static" data-calender="db"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="l_to" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Label></div>
                    <asp:TextBox ID="tb_to" runat="server" Width="200px" ClientIDMode="Static" data-calender="db" ></asp:TextBox>
                </div>
                <p>
                    <asp:Button ID="b_show" CssClass="btn btn-success" runat="server" Text="<%$Resources:Tokens,Show %>"
                        Width="90px" OnClick="b_show_Click" />
                </p>
            </div>
        </asp:Panel>
    </div>
    
    
      <div class="view">
        <asp:Panel runat="server" ID="p_results">
            <h3 class="header smaller lighter blue"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Tokens,Results %>"></asp:Literal></h3>
            <div runat="server" style="text-align: center" id="Print">
                <asp:GridView ID="gv_results" runat="server" CssClass="table table-bordered table-condensed"
                    OnDataBound="gv_results_DataBound" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="l_no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="User" HeaderText="<%$ Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Process" HeaderText="<%$ Resources:Tokens,Process%>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$ Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Customer" HeaderText="<%$ Resources:Tokens,Customer%>" />
                         <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Customer.Phone%>" />
                        <asp:BoundField DataField="Note" HeaderText="<%$ Resources:Tokens,Notes%>" />
                    </Columns>
                    <EmptyDataTemplate>
                        <p>
                            <asp:Literal runat="server" ID="gv_l_message" Text="<%$Resources:Tokens,NoResults %>"></asp:Literal></p>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            
            <p>
                <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="<%$Resources:Tokens,Export %>"
                    OnClick="Button1_Click" Width="90px"/>
            </p>
        </asp:Panel>
    </div>

    

    
    <script type="text/javascript" src="~/Content/ace-assest/js/chosen.jquery.min.js"></script>
   <script type="text/javascript">
         
         $(document).ready(function () {
             jQuery(".chosen").chosen();
             $('input[data-calender="db"]').datepicker({ dateFormat: 'dd-mm-yy' });

            
         });
     </script>
</asp:Content>
