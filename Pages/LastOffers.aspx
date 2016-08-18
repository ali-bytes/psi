<%@ Page Title="<%$Resources:Tokens,CurrentOffers%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="LastOffers.aspx.cs" Inherits="NewIspNL.Pages.LastOffers" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.CurrentOffers %></h1></div>
            <div>
                <% if (Offer2 != null && Offer2.Any())
                   { %>
                <% foreach (var offer in Offer2)
                   {%>
                <div>
                    <div>
                        <div class="offer-data">
                            <h3>
                                <%= offer.Name %></h3>
                              
                                <br/>
                            <div style="text-align: center">
                                <img class="img-thumbnail" width="30%" src="../_offerDetailsImages/<%= offer.ImageUrl %>" alt=""/>
                            </div>
                              <br/>
                            <p class="offer-p">
                                <%= offer.Data %></p>
                               <div style="margin: 0px 0px 12px;
border-bottom: 3px solid #e2e2e2;
padding-bottom: 16px;
padding-top: 7px;"></div>
            <div>
                        </div>
                    </div>
                </div>
               
                <% } %>
                <%} %>
            </div>      </div>
        </fieldset>
    </div>
</asp:Content>
