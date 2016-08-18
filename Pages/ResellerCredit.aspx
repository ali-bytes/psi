<%@ Page Title="<%$ Resources:Tokens,ResellerCredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerCredit.aspx.cs" Inherits="NewIspNL.Pages.ResellerCredit" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <div class="view">
        <fieldset>
            <div class="page-header"><h1><%=Tokens.ResellerCredit %></h1></div>
            <asp:Panel runat="server" ID="p_resquest">
                <div class="well">
                <div style="padding-bottom: 5px;">
                    <asp:Label runat="server" Text="<%$ Resources:Tokens,Reseller %>" ID="labelReseller"></asp:Label>
                </div>
                    <div>
                        <asp:DropDownList runat="server" CssClass="width-60 chosen-select" ID="ddl_reseller" Width="150px" 
                                          ClientIDMode="Static">
                        </asp:DropDownList>
                        <asp:Literal runat="server" ID="VMessage"></asp:Literal>
                    </div>
                    <label><%=Tokens.CreditFrom %></label><br />
                    <div>
                    <asp:RadioButtonList runat="server" ID="rblFrom" RepeatDirection="Vertical" CssClass="radio">
                        <asp:ListItem Selected="True" Value="<%$Resources:Tokens,ResellerPR %>"></asp:ListItem>
                        <asp:ListItem Value="<%$Resources:Tokens,ResellerBalanceSheet %>"></asp:ListItem>
                        <asp:ListItem Value="<%$Resources:Tokens,ResellerVoiceCredit %>"></asp:ListItem>
                        <asp:ListItem Value="<%$Resources:Tokens,UpdatedResellerBS %>"></asp:ListItem>
                    </asp:RadioButtonList></div>
                <p>
                    <br/>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="b_addRequest" Text="<%$ Resources:Tokens,ShowCredit %>" 
                                OnClick="b_addRequest_Click" Width="210px"/>
                                 <asp:Button runat="server" CssClass="btn btn-default" ID="Button1" Text="<%$ Resources:Tokens,ClearAllResellercredits %>" 
                                OnClick="b_addRequest1_Click" Visible="False" />
                    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
                </p>
                </div>
                <section>
                    <div runat="server" id="one" Visible="False">
                        <asp:GridView runat="server" ID="GCredits"  GridLines="Horizontal" 
                                      AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller%>" />
                                <asp:BoundField DataField="Credit" HeaderText="<%$ Resources:Tokens,Credit %>" />
                            </Columns>
                        </asp:GridView>
                  
                            <p>
                     
                    </p>
                        
                         <div id="sum" runat="server" Visible="False">
                    <table   class="table table-bordered table-condensed center" >
                        <tr>
                            <td style="background-color: cadetblue;font-weight: bold;color: white"><asp:Label ID="lblcurrentdemand" runat="server" Text="" Visible="False"></asp:Label></td>
                           <%-- <td style="background-color: cadetblue;font-weight: bold;color: white"> <asp:Label ID="maden" runat="server" Text="" Visible="False"></asp:Label></td>
                            <td style="background-color: cadetblue;font-weight: bold;color: white"><asp:Label ID="dayen" runat="server" Text="" Visible="False"></asp:Label></td>--%>
                            <td style="background-color: cadetblue;font-weight: bold;color: #FFCC66;font-size: large"> <asp:Label ID="saf" runat="server" Text="" Visible="False"></asp:Label></td>
                        </tr>
                      

                    </table>
                </div>
                          </div>
                      <div runat="server" id="all" Visible="False">
                        <asp:GridView runat="server" ID="Allreseleer"  GridLines="Horizontal" 
                                      AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller%>" />
                                 <asp:BoundField DataField="Branchname" HeaderText="<%$ Resources:Tokens,Branch%>" />
                             
                                 <asp:BoundField DataField="Credit" HeaderText="<%$ Resources:Tokens,Credit %>" />
                            <asp:BoundField DataField="currentpill" HeaderText="<%$ Resources:Tokens,currentpill %>" />
                           
                                 <asp:BoundField DataField="MenuResellerCredit" HeaderText="<%$ Resources:Tokens,MenuResellerCredit %>" />
                           
                                
                                 </Columns>
                        </asp:GridView>
                  
                            <p>
                      
                    </p>
                          </div>
                       <asp:Literal runat="server" ID="LTotal"></asp:Literal>
                </section>
                <br />
               
            </asp:Panel>
        </fieldset>
    </div>
    
      
 
    <script>
        $(document).ready(function () {
            $(".chosen-select").chosen();
            $('#ddl_reseller').change(function () {
                var selection = $('#ddl_reseller').val();
                var txt = $("#ddl_reseller option:selected").text();
                console.log("selection: " + selection + ", txt: " + txt);
                $('#HiddenField1').val(selection);
                $('#HiddenField2').val(txt);
               
            });
            $('tr td input[type=radio]').addClass("ace");
            $('tr td label').addClass("lbl");
        });

    </script>
</asp:Content>


