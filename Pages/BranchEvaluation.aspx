<%@ Page Title="<%$Resources:Tokens,BranchEvaluation%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchEvaluation.aspx.cs" Inherits="NewIspNL.Pages.BranchEvaluation" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row" style="height: 360px;">
    <div class="view" >
        <fieldset style="float: right ;">
            <div class="page-header">
               <h1> <%= Tokens.BranchEvaluation%></h1></div>
            <div class="well" >
                <div>
                   
                    <label for="DdlBranch">
                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                    </label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlBranch" ClientIDMode="Static" Width="177px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RTbReseller" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="DdlBranch" runat="server" />
                    </div>
                </div>
                <div>
                    <label for="TbStartAt">
                        <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,From %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbStartAt" data-select="dp"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbFrom" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="TbStartAt" runat="server" />
                    </div>
                </div>
                <div>
                    <label for="TbTo">
                        <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,To %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbTo" data-select="dp"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbTo" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="TbTo" runat="server" />
                        <asp:CompareValidator runat="server" ID="CVFromTo" ErrorMessage="<%$Resources:Tokens,DatePeriodMsg %>"
                                              ControlToValidate="TbTo" ControlToCompare="TbStartAt" Operator="GreaterThanEqual"
                                              Type="Date">
                            
                        </asp:CompareValidator>
                    </div>
                </div>
                <p>
                    <br/>
                    <button runat="server" id="BSearch" class="btn btn-success">
                        <i class="icon-search icon-only"></i>&nbsp;<%= Tokens.Search %></button>
                </p>
                
                
            </div>
           
    
        </fieldset>
   
       <fieldset style="  " >
                    <h3 class="blue" style="text-align: center">  <%= Tokens.StatisticsInHome %></h3>
            <div class="well" style=" margin-right: 5px; margin-top: 18px; height: 281px" >
        
            <div style="height: 60px ; margin-top: 18px;">
           <div style="float: right ;margin-left: 30px; margin-top: 10px;" >
            <h4 class="blue" >
                <%= Tokens.ToNewCustomer %></h4></div>
               <div style="margin-right: 30px;"> <asp:DataList  ID="DataList1" runat="server" RepeatColumns="4" CellPadding="4" 
             RepeatDirection="Horizontal" ForeColor="#333333"  >
                <AlternatingItemStyle BackColor="White" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#E3EAEB" />
                <ItemTemplate>
                    <table class="nav-justified">
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label2"  runat="server" Text='<%# Bind("pro_name") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                    
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("coun") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
              
                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
              
            </asp:DataList></div>
                </div>
             
             
           <div style="height: 60px">
                <div  style="float: right;margin-left: 84px; margin-top: 10px;">
            <h4 class="blue">
                <%= Tokens.Suspend %></h4></div>
             <div>   <asp:DataList ID="DataList2" runat="server" RepeatColumns="4" CellPadding="4" 
              RepeatDirection="Horizontal" ForeColor="#333333"  >
                <AlternatingItemStyle BackColor="White" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#E3EAEB" />
                <ItemTemplate>
                    <table class="nav-justified">
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("pro_name") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                      
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("coun") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
              
                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
              
            </asp:DataList></div></div>

            <div style="height: 60px">
             <div   style="float: right ;margin-left: 125px; margin-top: 10px;">
            <h4 class="blue">
                <%= Tokens.MenuCancelled %></h4></div>
               <div>  <asp:DataList ID="DataList3" runat="server" RepeatColumns="4" CellPadding="4" 
              RepeatDirection="Horizontal" ForeColor="#333333"  >
                <AlternatingItemStyle BackColor="White" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#E3EAEB" />
                <ItemTemplate>
                    <table class="nav-justified">
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("pro_name") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                     
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("coun") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
              
                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
              
            </asp:DataList></div></div>

           <div style="height: 60px">
            <div style="float: right ;margin-left: 125px; margin-top: 10px;"  >
            <h4 class="blue">
                 <%= Tokens.Active%></h4></div>
               <div>  <asp:DataList ID="DataList4" runat="server" RepeatColumns="4" CellPadding="4" 
              RepeatDirection="Horizontal" ForeColor="#333333"  >
                <AlternatingItemStyle BackColor="White" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#E3EAEB" />
                <ItemTemplate>
                    <table class="nav-justified">
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("pro_name") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                      
                        <tr>
                            <td class="text-center">
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("coun") %>' style="font-weight: 700"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
              
                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
              
            </asp:DataList></div></div>
           </div>
           </fieldset> 
    </div>
    </div>
    
    <div class="well" >
     <div class="text-center"  style="float: right">
         <h3 class="header smaller lighter blue"><%= Tokens.grafnew %></h3>
         <div style="margin-right: 0px">
   <asp:Chart ID="Chart1" runat="server" Width="480px" Height="350px">
    <Series>
        <asp:Series Name="Series1" ChartType="Pie" CustomProperties="PieLabelStyle=Disabled">
        </asp:Series>
       
    </Series>
       
 
    <ChartAreas>
       
        
        <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="true">
<Area3DStyle Enable3D="True"></Area3DStyle>
<AxisX LineColor="DarkGray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisX>
                    <AxisY LineColor="DarkGray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisY>
                    <Area3DStyle Enable3D="True" WallWidth="5" LightStyle="Realistic"></Area3DStyle>
               
        </asp:ChartArea>
    </ChartAreas>
     <Legends>
                <asp:Legend>
                </asp:Legend>
            </Legends>
</asp:Chart></div></div>

   
   <div class="text-center"   >
         <h3 class="header smaller lighter blue"><%= Tokens.graphnew2 %></h3>
   <asp:Chart ID="Chart2" runat="server" Width="500px" Height="350px">
    <Series>
        <asp:Series Name="Series1" ChartType="Pie" CustomProperties="PieLabelStyle=Disabled">
        </asp:Series>
       
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="true">
<Area3DStyle Enable3D="True"></Area3DStyle>
<AxisX LineColor="DarkGray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisX>
                    <AxisY LineColor="DarkGray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisY>
                    <Area3DStyle Enable3D="True" WallWidth="5" LightStyle="Realistic"></Area3DStyle>
               
        </asp:ChartArea>
    </ChartAreas>
     <Legends>
                <asp:Legend>
                </asp:Legend>
            </Legends>
</asp:Chart>
</div>      

    </div>
    

    
    
    <div class="view">
    	<fieldset>
    	    <h3 class="header smaller lighter blue"><%= Tokens.CustomersCount %></h3>
            <div class="center" runat="server" id="divcustomerCount" Visible="False">
                                            <span class="btn btn-app btn-info no-hover" style="border-radius: 5px;">
													<span class="line-height-1 bigger-170"><asp:Literal runat="server" ID="totalCount"></asp:Literal> </span>

                                                <br/>
													<span class="line-height-1 smaller-90"> <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,CustomersCount %>" ></asp:Literal> </span>
												</span></div>

    	</fieldset>
    </div>
    

    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.ToNewCustomer %></h3>
        </fieldset>
        <div>
            <asp:GridView runat="server" ID="GvToNewCustomer" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="LNo" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="Govornorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                    <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                    <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State %>" />
                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                    <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                    <asp:BoundField DataField="ActivationDate" HeaderText="<%$Resources:Tokens,Activation.Date %>" />
                    <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                    <asp:BoundField DataField="Installer" HeaderText="<%$Resources:Tokens,Installer %>" />
                    <asp:BoundField DataField="InstallationTime" HeaderText="<%$Resources:Tokens,InstallationTime %>" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
     
        
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.SuspendCustomersUntilNow %></h3>
        </fieldset>
        <div>
            <asp:GridView runat="server" ID="GvSuspend" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="LNo" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="Govornorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                    <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                    <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State %>" />
                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                    <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                    <asp:BoundField DataField="ActivationDate" HeaderText="<%$Resources:Tokens,Activation.Date %>" />
                    <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                    <asp:BoundField DataField="Installer" HeaderText="<%$Resources:Tokens,Installer %>" />
                    <asp:BoundField DataField="InstallationTime" HeaderText="<%$Resources:Tokens,InstallationTime %>" />
                </Columns>
                 <EmptyDataTemplate>
                    No Data
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
   
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.MenuCancelled %></h3>
        </fieldset>
        <div>
            <asp:GridView runat="server" ID="GvCancelled" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="LNo" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="Govornorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                    <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                    <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State %>" />
                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                    <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                    <asp:BoundField DataField="ActivationDate" HeaderText="<%$Resources:Tokens,Activation.Date %>" />
                    <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                    <asp:BoundField DataField="Installer" HeaderText="<%$Resources:Tokens,Installer %>" />
                    <asp:BoundField DataField="InstallationTime" HeaderText="<%$Resources:Tokens,InstallationTime %>" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    
              <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                 <%= Tokens.Active%></h3>
        </fieldset>
        <div>
            <asp:GridView runat="server" ID="GridView4" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="LNo" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="Govornorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                    <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                    <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State %>" />
                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                    <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                    <asp:BoundField DataField="ActivationDate" HeaderText="<%$Resources:Tokens,Activation.Date %>" />
                    <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                    <asp:BoundField DataField="Installer" HeaderText="<%$Resources:Tokens,Installer %>" />
                    <asp:BoundField DataField="InstallationTime" HeaderText="<%$Resources:Tokens,InstallationTime %>" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
  
     
  
 
    <script type="text/javascript">
        $('input[data-select="dp"]').datepicker({
            showOtherMonths: true,
            selectOtherMonths: false,
            dateFormat: 'dd/mm/yy'
        });
    </script>
</asp:Content>
