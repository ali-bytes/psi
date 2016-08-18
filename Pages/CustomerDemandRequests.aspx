<%@ Page Title="<%$Resources:Tokens,CustomerDemandRequests %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerDemandRequests.aspx.cs" Inherits="NewIspNL.Pages.CustomerDemandRequests" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .table {
            display: block;
    overflow-x: scroll;

}


    </style>

        <div class="page-header">
							<h1><asp:Label ID="lblTitle" Text="<%$Resources:Tokens,CustomerDemandRequests %>" runat="server"></asp:Label>
							</h1>
						</div>
                        <div class="col-md-12">
                            <div class="alert alert-success" style="display: none" id="divSuccess"><asp:Label runat="server" ID="lblSusc" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Label></div>
                            <div class="alert alert-danger" style="display: none" id="divError"><asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,ErrorMsg %>"></asp:Label></div>
                            <asp:HiddenField runat="server" ID="hdfMsg" ClientIDMode="Static"/>
                            <div>
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,ProcessType %>"></asp:Literal>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlProccessType" OnSelectedIndexChanged="ddlProccess_SelectedIndexChanged" AutoPostBack="True"/>
                                </div>
                            </div>
                            <br/>
                            <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-condensed text-center" style="margin-right: -18px;" DataKeyNames="RequestNotifiId"
                     OnRowDataBound="grd_Requests_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="lbl_No" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>"/>
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <asp:BoundField DataField="Central" HeaderText="<%$ Resources:Tokens,Central%>" />
                        <asp:BoundField DataField="CurrentServicePackageName" HeaderText="<%$ Resources:Tokens,CurrentSpeed %>"/>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Customer.Status %>"/>
                        <asp:BoundField DataField="SuspenDaysCount" HeaderText="<%$ Resources:Tokens,SuspendDaysCount %>"/>
                        <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>"/>
                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>"/>
                        <asp:BoundField DataField="SenderName" HeaderText="<%$Resources:Tokens,SenderName %>"/>
                        <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="ActivationDate" HeaderText="<%$ Resources:Tokens,Activation.Date %>" />
                        <asp:BoundField HeaderText="<%$ Resources:Tokens,Offer%>" DataField="Title" />
                        <asp:BoundField DataField="IpPackageName" HeaderText="<%$ Resources:Tokens,New.IP.Package %>"/>
                        <asp:BoundField DataField="Notes" HeaderText="<%$ Resources:Tokens,Extra.Gigas%>"/>
                        <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:Tokens,PaymentType %>"/>
                        <asp:BoundField DataField="ProccessName" HeaderText="<%$Resources:Tokens,Process %>"/>
                        <asp:BoundField DataField="ProcessDate" HeaderText="<%$ Resources:Tokens,ProcessDate %>"/>
                        <%--<asp:BoundField DataField="Status" HeaderText="<%$ Resources:Tokens,Status %>"/>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton CssClass="btn btn-success btn-xs" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Confirm %>" OnClick="ApproveRequest"
                                 runat="server" ID="lnkApprove" CommandArgument='<%#Eval("RequestNotifiId") %>'><i class="icon-white icon-ok"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Select %>">
                            <HeaderTemplate>
                                <input type="checkbox" id="selecctall"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="SelectItem" runat="server" data-check="select" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoRequests%>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                        </div>
                        <div class="align-center">
                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-lg" ID="btnSelectAll" OnClick="BtnApproveSelectAll">
                               <i class="icon-ok icon-only"></i>
                               <%=Tokens.ApproveSelected %>
                         
                        </asp:LinkButton></div>
                        
                        <script type="text/javascript">
                            $(function () {
                                if ($('#grd_Requests').width() > 1058) {
                                    $('#grd_Requests').css({
                                        //"font-family": "serif",
                                        "font-size": "11px"
                                    });
                                }
                                var div = $('#hdfMsg').val();
                                if (div === "1") {
                                    $('#divSuccess').show();
                                    $('#divError').hide();
                                } else if (div === "0") {
                                    $('#divError').show();
                                    $('#divSuccess').hide();
                                }
                                $('#selecctall').click(function (event) {  //on click 
                                    if (this.checked) { // check select status
                                        $('input[type="checkbox"]').each(function () { //loop through each checkbox
                                            this.checked = true;  //select all checkboxes with class "checkbox1"               
                                        });
                                    } else {
                                        $('input[type="checkbox"]').each(function () { //loop through each checkbox
                                            this.checked = false; //deselect all checkboxes with class "checkbox1"                       
                                        });
                                    }
                                });
                            });
                        </script>
</asp:Content>

