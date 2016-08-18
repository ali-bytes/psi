<%@ Page Title="<%$Resources:Tokens,RouterSerailSearch%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RouterSerialsSearch.aspx.cs" Inherits="NewIspNL.Pages.RouterSerialsSearch" %>

<%@ Import Namespace="Resources" %>
<%@ Import Namespace="NewIspNL.Helpers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,RouterSerailSearch %>" runat="server" /></h1></div>
            <div class="well">
                <div>
                    <asp:Label AssociatedControlID="TbName" Text="<%$Resources:Tokens,SerialNumber %>"
                        runat="server" />
                    <div>
                        <asp:TextBox runat="server" ID="TbName" ClientIDMode="Static" EnableViewState="True" />
                    </div>
                </div>
                <p>
                    <br/>
                    <button id="Button1" type="button" runat="server" ClientIDMode="Static" onserverclick="BSearch_OnClick"
                        class="btn btn-success">
                        <i class="icon-white icon-search"></i>&nbsp;<asp:Literal ID="Literal18" Text="<%$Resources:Tokens,Search %>"
                            runat="server" /></button>
                </p>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal19" Text="<%$Resources:Tokens,Results %>" runat="server"></asp:Literal>
                <% if (Results != null && Results.Any())
                   { %>
                &nbsp;-&nbsp;(<%= Results.Count %>)
                <% }%>
            </h3>
            <div>
                <% if (Results != null && Results.Any())
                   {%>
                <table class="table table-bordered table-condensed text-center">
                    <thead>
                        <tr>
                            <th>
                            </th>
                            <th>
                                <asp:Literal ID="Literal20" Text="<%$Resources:Tokens,Customer %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal21" Text="<%$Resources:Tokens,Phone %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal22" Text="<%$Resources:Tokens,Offer %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal23" Text="<%$Resources:Tokens,State %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal24" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal25" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal26" Text="<%$Resources:Tokens,Central %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal27" Text="<%$Resources:Tokens,Package %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal ID="Literal28" Text="<%$Resources:Tokens,Activation.Date %>" runat="server" />
                            </th>
                            <th>
                                <asp:Literal Text="<%$Resources:Tokens,RouterSerial %>" runat="server" />
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                    <% foreach (var result in Results)
                       {%>
                    <tr>
                        <td>
                            <%= Results.IndexOf(result) + 1 %>
                        </td>
                        <td>
                            <%= result.Customer %>
                        </td>
                        <td>
                            <%= result.Phone %>
                        </td>
                        <td>
                            <%= result.Offer %>
                        </td>
                        <td>
                            <%= result.State %>
                        </td>
                        <td>
                            <%= result.Branch %>
                        </td>
                        <td>
                            <%= result.Reseller %>
                        </td>
                        <td>
                            <%= result.Central %>
                        </td>
                        <td>
                            <%= result.Package %>
                        </td>
                        <td>
                            <%= result.ActivationDate %>
                        </td>
                        <td>
                            <span>
                                <%= result.RouterSerial %></span>
                        </td>
                        <td style="width: 150px;">
                            <% if (CanEdit)
                               { %>
                            <a  title="<%=Tokens.Edit %>" data-rel="tooltip" class="btn btn-primary btn-sm" href="EditCustomer.aspx?WOID=<%=QueryStringSecurity.Encrypt(result.Id.ToString()) %>"
                                target="_blank">
                               <i class="icon-edit icon-only"></i>
                            </a>&nbsp;&nbsp;
                            <%  } %>
                            <a title="<%=Tokens.Details %>" data-rel="tooltip" class="btn btn-success btn-sm" href="CustomerDetails.aspx?c=<%=QueryStringSecurity.Encrypt(result.Id.ToString()) %>"
                                target="_blank"><i class="icon-building icon-only"></i>
                               
                            </a>
                        </td>
                    </tr>
                    <% } %>
                </table>
                <% }
                   else
                   {%>
                <label>
                    <asp:Literal ID="Literal31" Text="<%$Resources:Tokens,NoResults %>" runat="server" />
                </label>
                <% } %>
            </div>
        </fieldset>
    </div>
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('#TbName').keypress(function (e) {
                                var key = e.which;
                                if (key == 13) {
                                    $('#Button1').click();
                                    return false;
                                } else {
                                    return true;
                                }
                            });
                        });
    </script>
</asp:Content>

