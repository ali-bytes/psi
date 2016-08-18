<%@ Page Title="<%$ Resources:Tokens,AddNewTicket%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddTicket.aspx.cs" Inherits="NewIspNL.Pages.AddTicket" %>
    

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div id="tr_Search" runat="server">
            <div class="view">
                <fieldset>
                    <div class="page-header">
                       <h1> <asp:Label ID="l_Search" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                        <div>
                            <div>
                                <asp:Label ID="Label31" runat="server" Text="<%$Resources:Tokens,Customer.Phone %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_CustomerPhone0" runat="server" Width="150px" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerPhone0" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div runat="server" id="GovBox">
                                <label for="DdlGovernorate">
                                    <%= Tokens.Governorate %></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlGovernorate" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RddlGovernorate" ErrorMessage="*" ControlToValidate="DdlGovernorate"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <div>
                                    <br/>
                                    <asp:Button ClientIDMode="Static" CssClass="btn btn-success" ID="btn_search" runat="server" Text="<%$Resources:Tokens,Search%>"
                                        Width="100px" OnClick="btn_search_Click" ValidationGroup="SearchVG" />
                                </div>
                            </div>
                            <div>
                                <div>
                                    <asp:Label ID="lbl_SearchResult" runat="server" EnableViewState="False" Font-Bold="True"
                                        ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
        <div id="tr_AddTicket" runat="server" visible="false">
            <div class="col-xs-12 col-sm-6">
                <div class="widget-box">
                    <div class="widget-header">
                    <h4> <asp:Label ID="l_t_AddTicket" runat="server" Text="<%$Resources:Tokens,Add.Ticket%>"></asp:Label>
                        </h4>
                    <span class="widget-toolbar" style="padding: 11px"><a href="#" data-action="reload"><i class="icon-refresh">
                    </i></a><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a><a href="#" data-action="close"><i class="icon-remove"></i></a></span>
                </div>
                <div class="widget-body">
                    <div class="well" style="border-bottom: none">
                        <table width="100%">
                            <tr>
                                <td style="width: 111px">
                                    <asp:Label ID="Label35" runat="server" Text="<%$Resources:Tokens,Ticket.Reason%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_TicketReasons" runat="server" Width="555px" DataTextField="Title"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                        ValidationGroup="vg_AddTicket" ControlToValidate="ddl_TicketReasons"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 111px">
                                    <asp:Label ID="Label36" runat="server" Text="<%$Resources:Tokens,Comment%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Details" runat="server" Rows="10" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                        ValidationGroup="vg_AddTicket" ControlToValidate="txt_Details"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btn_AddTicket" runat="server" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Add.Ticket%>"
                                        Width="100px" ValidationGroup="vg_AddTicket" OnClick="btn_AddTicket_Click"  UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                                </td>
                                <td align="center">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lbl_AddResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="center">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div></div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="widget-box">
                    <div class="widget-header">
                    <h4><asp:Literal Text="<%$Resources:Tokens,Customer.Info %>" runat="server" />
                        </h4>
                    <span class="widget-toolbar" style="padding: 11px"><a href="#" data-action="reload"><i class="icon-refresh">
                    </i></a><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a><a href="#" data-action="close"><i class="icon-remove"></i></a></span>
                </div>
                    <div class="widget-body">  
                    <table class="table table-bordered">
                        <tr>
                            <td>
                                <asp:Literal ID="Literal11" Text="<%$Resources:Tokens,Name%>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lName"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal10" Text="<%$Resources:Tokens,Phone %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lPhone"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Package %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lPackage"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Governrate %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lGovernate"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Central %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lCentral"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,State %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lState"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Provider %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lProvider"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal6" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lReseller"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal7" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lBranch"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal8" Text="<%$Resources:Tokens,Activation.Date %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lActivation"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal9" Text="<%$Resources:Tokens,Offer %>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lOffer"></asp:Literal>
                            </td>
                        </tr>
                    </table></div>  
                </div>
            </div>
            
        </div>
    </div>
    
    <script type="text/javascript">
        function plswait(id) {

            var type = document.getElementById('<%=ddl_TicketReasons.ClientID%>').value;
            var com = document.getElementById('<%=txt_Details.ClientID%>').value;
            if (type == "" || com == "") { return; }
                else {
                    var check2 = document.getElementById(id);
                    check2.disabled = 'true'; check2.value = 'Please wait...';
                }

            }

        $(document).ready(function () {
            $('#txt_CustomerPhone0').keypress(function (e) {
                var key = e.which;
                if (key === 13) {
                    $('#btn_search').click();
                    return false;
                } else {
                    return true;
                }
            });
        });
    </script>
</asp:Content>
