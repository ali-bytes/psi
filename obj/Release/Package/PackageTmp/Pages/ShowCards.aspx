<%@ Page Title="<%$Resources:Tokens,AvilableCards %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ShowCards.aspx.cs" Inherits="NewIspNL.Pages.ShowCards" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <div class="page-header" >
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,AvilableCards %>"></asp:Literal>
                </h1>
            </div>
      
      <div dir="rtl" style="text-align: center; ">
                      <asp:GridView runat="server" ID="GvitemData" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                              <asp:TemplateField HeaderText="#">
                            <ItemTemplate >
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,Cardnum %>" />
                        <asp:BoundField DataField="TypeName" HeaderText="<%$Resources:Tokens,distypename %>" />
                        <asp:BoundField DataField="Price" HeaderText="<%$Resources:Tokens,Value %>"  />
                       
                             <asp:TemplateField HeaderText="<%$Resources:Tokens,use %>">
                               <ItemTemplate>
                                  
                                  <button  id="ed" type="button" value="button" class="btn btn-success" onclick="show(<%#Eval("ID")%>)" title="<%=Tokens.use %>" > <i class="icon-white icon-plus-sign"> </i></button>
                                  
                                    </ItemTemplate>
                                          </asp:TemplateField>
                                      
                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                    
    <asp:HiddenField ID="cardid" runat="server" ClientIDMode="Static" />
    
                   
                    
                          <div id="multi" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"style="margin-right:200px;" >
   
                            <div class="modal-dialog" style="background: aliceblue ;float: right;width: 450px">
                <%=Tokens.Phone%> 
                                <asp:TextBox ID="txtphone" runat="server"></asp:TextBox>
            
           <input type="button" id="btnSubmit" value=" <%=Tokens.Search%> " onclick="res()"  class="btn btn-grey" title=" "/>  
                        <label id="lblmsg" style="color:green" />              
      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"                                             
       ControlToValidate="txtphone"    ValidationGroup="edit"></asp:RequiredFieldValidator>
    
    </div>      <br/> <br/>
     <div style="margin-left:400px;">
<asp:Button ID="Button1" runat="server"  
       class="btn btn-primary" Text="<%$Resources:Tokens,use %>" 
          ValidationGroup="edit" OnClick="Button1_Click" /> 
       </div>
     </div>  
            <script type="text/javascript">
                document.getElementById("<%= Button1.ClientID %>").style.display = "none";
                function show(id) {
                    document.getElementById("<%= cardid.ClientID %>").value = id;
                    $('#cardid').val(id);
                    $('#multi').modal('show');
                    



                };




                function res() {
                    var ph = document.getElementById('<%=txtphone.ClientID%>');
                   var id= document.getElementById("<%= cardid.ClientID %>").value;
                    //id= $('#cardid').val(id);
                      $.ajax({
                          type: "Post",
                          contentType: "application/json; charset=utf-8",
                          url: "ShowCards.aspx/InsertData",
                          dataType: "json",
                          data: "{'phone':'" + ph.value + "','id':'"+id+"'}",
                          success: function (data) {
                              var obj = data.d;
                              if (obj != null) {

                                  if (obj == "") {

                                      $('#lblmsg').html("هذا العميل حصل علي كارت خصم من قبل");
                                      document.getElementById("<%= Button1.ClientID %>").style.display = "none";
                                  } else if (obj === "1") {
                                      $('#lblmsg').html("هذا العميل ليس لة مطالبات غير مدفوعة");
                                      document.getElementById("<%= Button1.ClientID %>").style.display = "none";
                                  } else if (obj === "2") {

                                      $('#lblmsg').html("مطالبة هذا العميل اقل من قيمة الكارت");
                                      document.getElementById("<%= Button1.ClientID %>").style.display = "none";
                                  } else {
                                      document.getElementById("<%= Button1.ClientID %>").style.display = "";
                                      $('#lblmsg').html(obj);
                                  }
                                }

                                else {
                                    $('#lblmsg').html("العميل غير مسجل");
                                    document.getElementById("<%= Button1.ClientID %>").style.display = "none";
                                }

                            },
                            error: function (result) {

                                alert(result);
                            }
                        });
                    };


            </script>
  
   
 
  
   
</asp:Content>
    
