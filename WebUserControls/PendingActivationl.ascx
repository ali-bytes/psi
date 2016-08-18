<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PendingActivationl.ascx.cs"
    Inherits="NewIspNL.WebUserControls.WebUserControls_PendingActivationl" %>
<%@ Import Namespace="Resources" %>
<% if (Proceed)
   {%>
<div class="container-fluid">
    <div class="row-fluid">
        <div class="span4 widgethox">
            <div class="customers-label">
                <div>
                    <%= Tokens.CustomersCount %>
                    <div class="customer-count">
                        <%= CustomersCount %></div>
                </div>
            </div>
        </div>
        <div class="span1">
        </div>
        <div class="span7 widgethox">
            <div style="padding: 5px;">
                <div class="text-center">
                    <h4>
                        <%= Tokens.CustomersStatus %></h4>
                </div>
                <table class="cm-item">
                    <tr>
                        <% if (StatModels != null && StatModels.Any())
                           { %>
                        <% foreach (var model in StatModels.Take(2))
                           { %>
                        <td class="span4">
                            <div class="alert alert-info">
                                <span class="badge badge-info">
                                    <%= model.Count %></span>&nbsp;<%= model.TName %></div>
                        </td>
                        <% } %>
                        <% } %>
                    </tr>
                    <tr>
                        <% if (StatModels != null && StatModels.Any())
                           { %>
                        <% foreach (var model in StatModels.Skip(2).Take(2))
                           { %>
                        <td class="span4">
                            <div class="alert alert-info">
                                <span class="badge badge-info">
                                    <%= model.Count %></span>&nbsp;<%= model.TName %></div>
                        </td>
                        <% } %>
                        <% } %>
                    </tr>
                    <tr>
                        <% if (StatModels != null && StatModels.Any())
                           { %>
                        <% foreach (var model in StatModels.Skip(4).Take(2))
                           { %>
                        <td class="span4">
                            <div class="alert alert-info">
                                <span class="badge badge-info">
                                    <%= model.Count %></span>&nbsp;<%= model.TName %></div>
                        </td>
                        <% } %>
                        <% } %>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<hr />
<%  } %>




