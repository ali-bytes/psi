<%@ Page Title="<%$Resources:Tokens,susandcancoutage %>" EnableEventValidation="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SusAndCancRep.aspx.cs" Inherits="NewIspNL.Pages.SusAndCancRep" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



     <div class="row">
      
            <div class="col-xs-12">
                                <div runat="server" ID="suspendStatus" Visible="False">   
                    <label><%=Tokens.susandcancoutage %></label>
                  
                </div>
                
                <br/>
                
                 <fieldset>
                        <div class="view">
       
            <div class="page-header"><h1><asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,susandcancoutage %>"></asp:Label></h1></div>
            <div class="well col-md-6 col-sm-12">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Type%>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlrepType"  Width="178px"/>
                </div>
                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
                            <div>
                            <asp:TextBox ID="tb_from" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_from"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_from"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                                                  </div>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To %>"></asp:Label>
                            <div>
                            <asp:TextBox ID="tb_to" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_to"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="tb_to"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                            &nbsp;
                             </div>
                                                  <br/>
                <asp:Button runat="server" ID="btnSearch" Text="<%$Resources:Tokens,Search %>" 
                    CssClass="btn btn-success" onclick="btnSearch_Click" ClientIDMode="Static" Width="83px"/>
       
            </div>
     <div class="well col-md-6 col-sm-12">
          <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Reseller%>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlReseller"  Width="178px"/>
                </div>
          <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Branch%>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlBranch"  Width="178px"/>
                </div>
     </div>
    </div>
                
                
                    <div id="dd" runat="server">
                <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                   OnDataBound="grd_Requests_OnDataBound" CssClass="table table-bordered table-condensed text-center" style="margin-right: -18px;"
                  
                   PageSize="50" AllowPaging="False" 
                     OnPageIndexChanging="grdData_PageIndexChanging">
                    <PagerSettings Position="TopAndBottom"></PagerSettings>
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label  ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>
                         <asp:BoundField HeaderText="<%$Resources:Tokens,Customer %>" DataField="Customer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Phone %>" DataField="Phone" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Status %>" DataField="Status" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Branch %>" DataField="Branch"/>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Central %>" DataField="Central" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Governorate %>" DataField="Governorate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Offer %>" DataField="Offer" />
                        
                        <asp:BoundField HeaderText="<%$Resources:Tokens,From %>" DataField="TStartAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,To %>" DataField="TEndAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="TAmount" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,PaymentType %>" DataField="PaymentMethod" />
                         <asp:BoundField HeaderText="<%$Resources:Tokens,PaymentDate %>" DataField="payDate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Address %>" DataField="Address" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Mobile %>" DataField="Mobile" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,PaymentComment %>" DataField="PaymentComment" />
                        
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes %>">
                            <ItemTemplate>
                                
                             <div  class="<%#Eval("style") %>" >   <asp:Label ID="note"   runat="server" ><%#Eval("notes") %></asp:Label>
                           </div> </ItemTemplate>
                                 </asp:TemplateField>
                        
                       
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoRequests%>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                 
                        <asp:Label ID="total" runat="server" Text=""></asp:Label>

                      <div style="text-align: center" runat="server" id="export_div" Visible="False">
                            <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="b_export" OnClick="b_export_Click" ClientIDMode="Static"
                                       Width="100px"><i class="icon-file"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Export %>"></asp:Literal></asp:LinkButton>
                        </div> </div>
                    </fieldset>         
            </div>
          
    </div>












        <script>

            $(document).ready(function () {
                $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
                $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });

            });
    </script>







</asp:Content>
