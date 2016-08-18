<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatisticsChart.ascx.cs"
    Inherits="NewIspNL.WebUserControls.WebUserControls_StatisticsChart" %>
<script src="../Content/lib/Chart.min.js" type="text/javascript"> </script>
<script src="../Scripts/underscore.min.js" type="text/javascript"></script>
<% //if (Proceed)
   { %>
<div class="col-sm-7" id="chart">
    <div class="widget-box transparent">
        <div class="widget-header widget-header-flat">
            <h4 class="lighter">
                <i class="icon-signal"></i>
                <asp:Literal runat="server" ID="lblcus" Text="<%$Resources:Tokens,CustomersStatusInPreviousSixMonths%>"></asp:Literal>
                <%--<%= Tokens.CustomersStatusInPreviousSixMonths %>--%>
            </h4>
            <div class="widget-toolbar">
                <a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>
            </div>
        </div>
        <div class="widget-body">
            <div class="widget-main padding-4">
                <div class="chart-item">
                    <% if (ContanierModels != null && ContanierModels.Any())
                       { %>
                    <%--                <div class="text-center">
                    <h4>
                        <%= Tokens.CustomersStatusInPreviousSixMonths %>
                    </h4>
                </div>--%>
                    <div style="text-align: center;">
                        <% foreach (var containerModel in ContanierModels)
                           { %>
                        <% if (containerModel != null && containerModel.Values.Count == containerModel.Names.Count)
                           { %>
                        <button type="button" class="btn btn-xs chart-btn" id="btn-<%= containerModel.Id %>"
                            data-val="<%=containerModel.Id %>">
                            <%= containerModel.Name %>
                        </button>
                        &nbsp;
                        <% } %>
                        <% } %>
                    </div>
                    <% foreach (var containerModel in ContanierModels)
                       { %>
                    <% if (containerModel != null && containerModel.Values.Count == containerModel.Names.Count)
                       { %>
                    <div id="model-<%= containerModel.Id %>" style="display: none; text-align: center">
                        <div class="text-center">
                            <h4>
                                <%= containerModel.Name %></h4>
                        </div>
                        <canvas id="c-<%= containerModel.Id %>" width="800px" height="400px">
                    </canvas>
                        <% for (int i = 0; i < containerModel.Values.Count; i++)
                           { %>
                        <input type="hidden" data-id="<%= containerModel.Id %>" data-name="<%= containerModel.Id %>"
                            data-vals="<%= containerModel.Values[i] %>" data-names="<%= containerModel.Names[i] %>" />
                        <% } %>
                    </div>
                    <% } %>
                    <% } %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="col-sm-5">
    <div class="widget-box transparent">
        <div class="widget-header widget-header-flat">
            <h4 class="lighter">
                <i class="icon-star orange"></i>
                <asp:Literal runat="server" ID="Literal1" Text="<%$Resources:Tokens,CustomersStatusInPreviousSixMonths%>"></asp:Literal>
            </h4>
            <div class="widget-toolbar">
                <a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>
            </div>
        </div>
        <div class="widget-body">
            <div class="widget-main padding-4">
                <div>
                    <% if (ContanierModels != null && ContanierModels.Any())
                       { %>
                    <div>
                        <table id="by-number">
                            <% foreach (var containerModel in ContanierModels)
                               { %>
                            <% if (containerModel != null && containerModel.Values.Count == containerModel.Names.Count)
                               { %>
                            <tr>
                                <td>
                                    <div>
                                        <table class="cm-item alert" style="margin: 4px;" data-val="<%=containerModel.Id %>">
                                            <tr>
                                                <td class="cm-title text-center" colspan="<%= containerModel.Names.Count %>">
                                                    <h4>
                                                        <%= containerModel.Name %></h4>
                                                </td>
                                            </tr>
                                            <tr>
                                                <% foreach (var name in containerModel.Names)
                                                   { %>
                                                <td class="cm-name">
                                                    <div class="alert" data-val="<%=containerModel.Id %>">
                                                        <span class="badge cm-val" id="<%=containerModel.Id %>" style="background-color: transparent!important;
                                                            color: inherit">
                                                            <%= containerModel.Values[containerModel.Names.IndexOf(name)] %></span>&nbsp;<%= name %>
                                                    </div>
                                                </td>
                                                <% } %>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <% } %>
                            <% } %>
                        </table>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
</div>
<!--chart updated -->
<script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script>
<script src="../Content/app/home.js" type="text/javascript"></script>
<% } %>
<script type="text/javascript">
    $('canvas').css({ 'width': '100%' }); //.width('100%');
    $('[data-val="1"]').addClass('alert-info');
    $('button[data-val="1"]').addClass('btn-primary');
    //$('span[id="1"]').addClass('badge-info');
    $('[data-val="2"]').addClass('alert-success');
    $('button[data-val="2"]').addClass('btn-success');
    //$('span[id="2"]').addClass('badge-success');
    $('[data-val="3"]').addClass('alert-warning');
    $('button[data-val="3"]').addClass('btn-warning');
    //$('span[id="3"]').addClass('badge-warning');
    $('[data-val="4"]').addClass('alert-danger');
    $('button[data-val="4"]').addClass('btn-danger');
    //$('span[id="4"]').addClass('badge-danger');
    $('[data-val="7"]').addClass('alert-info');
    $('button[data-val="7"]').addClass('btn-info');
    //$('span[id="7"]').addClass('badge-primary');
    $('.chart-btn').css({ 'width': '85px' });
    /*$('span.badge').css({
    'background-color': 'transparent!important',
    'color': 'inherit'
    });*/
</script>
<%--<div class="col-sm-12">
										<div class="widget-box transparent">
											<div class="widget-header widget-header-flat">
												<h4 class="lighter">
													<i class="icon-star orange"></i>
													<%=Tokens.CustomersStatusInPreviousSixMonths%>
												</h4>

												<div class="widget-toolbar">
													<a href="#" data-action="collapse">
														<i class="icon-chevron-up"></i>
													</a>
												</div>
											</div>

											<div class="widget-body">
												<div class="widget-main no-padding">
													<table class="table table-bordered table-striped">
														<thead class="thin-border-bottom">
															<tr>
																<th>
																	<i class="icon-caret-right blue"></i>
																	name
																</th>

																<th>
																	<i class="icon-caret-right blue"></i>
																	price
																</th>

																<th class="hidden-480">
																	<i class="icon-caret-right blue"></i>
																	status
																</th>
															</tr>
														</thead>
														<tbody>
															<tr>
																<td>internet.com</td>

																<td>
																	<small>
																		<s class="red">$29.99</s>
																	</small>
																	<b class="green">$19.99</b>
																</td>

																<td class="hidden-480">
																	<span class="label label-info arrowed-right arrowed-in">on sale</span>
																</td>
															</tr>

															<tr>
																<td>online.com</td>

																<td>
																	<small>
																		<s class="red"></s>
																	</small>
																	<b class="green">$16.45</b>
																</td>

																<td class="hidden-480">
																	<span class="label label-success arrowed-in arrowed-in-right">approved</span>
																</td>
															</tr>

															<tr>
																<td>newnet.com</td>

																<td>
																	<small>
																		<s class="red"></s>
																	</small>
																	<b class="green">$15.00</b>
																</td>

																<td class="hidden-480">
																	<span class="label label-danger arrowed">pending</span>
																</td>
															</tr>

															<tr>
																<td>web.com</td>

																<td>
																	<small>
																		<s class="red">$24.99</s>
																	</small>
																	<b class="green">$19.95</b>
																</td>

																<td class="hidden-480">
																	<span class="label arrowed">
																		<s>out of stock</s>
																	</span>
																</td>
															</tr>

															<tr>
																<td>domain.com</td>

																<td>
																	<small>
																		<s class="red"></s>
																	</small>
																	<b class="green">$12.00</b>
																</td>

																<td class="hidden-480">
																	<span class="label label-warning arrowed arrowed-right">SOLD</span>
																</td>
															</tr>
														</tbody>
													</table>
												</div><!-- /widget-main -->
											</div><!-- /widget-body -->
										</div><!-- /widget-box -->
									</div>--%>