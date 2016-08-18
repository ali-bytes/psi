<%@ Page Title="<%$Resources:Tokens,Groups %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="NewIspNL.Pages.Groups" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
    <table style="width: 100%">
        <tr>
            <td>
                <fieldset>
                    <div class="page-header"><h1>
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,EditGroup  %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                        <div>
                            <label>
                                <asp:Literal ID="Label1" runat="server" Text="<%$ Resources:Tokens,SelectGroup %>"></asp:Literal></label>
                            <div>
                                <asp:DropDownList ID="ddl_Groups" runat="server" Width="205px" DataTextField="GroupName"
                                                  DataValueField="ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="ddl_Groups"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <p>
                            <br/>
                            <button  runat="server" class="btn btn-primary" onserverclick="btn_Edit_Click">
                                <i class="icon-edit"></i>&nbsp;<%=Tokens.EditGroup %></button>
                        </p>
                    </div>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,GroupDetails %>"></asp:Label>
                    </h3>
                    <div class="well">
                        <table width="100%">
                            <tr>
                                <td style="width: 110px">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,GroupName %>"></asp:Label>
                                </td>
                                <td style="width: 185px">
                                    <asp:TextBox ID="txt_GroupName" runat="server" Width="200px"></asp:TextBox>
                                </td>
                                <td style="width: 76px">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                                ControlToValidate="txt_GroupName" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 80px">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,DataLevel %>"></asp:Label>
                                </td>
                                <td style="width: 203px">
                                    <asp:DropDownList ID="ddl_DataLevel" Width="200px" runat="server" DataTextField="Name"
                                                      DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                                ControlToValidate="ddl_DataLevel"  ValidationGroup="Process"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 110px" valign="top">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,GroupPrivileges %>"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TreeView ID="treePRIVILAGES2" ClientIDMode="Static" runat="server" ShowCheckBoxes="All"
                                                  onclick="client_OnTreeNodeChecked();">
                                    </asp:TreeView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center">
                                    <br/>
                                    &nbsp;
                                    <asp:LinkButton ID="btn_Insert" runat="server"  Width="30%"
                                                ValidationGroup="Process" CssClass="btn btn-success" OnClick="btn_Insert_Click"><i class="icon-ok bigger-110"></i>&nbsp;<asp:Literal runat="server" Text="<%$ Resources:Tokens,Add%>"></asp:Literal></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btn_Update" runat="server" CssClass="btn btn-primary" Width="30%"
                                                ValidationGroup="Process" OnClick="btn_Update_Click" Visible="False"><i class="icon-edit bigger-110"></i>&nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,Update%>"></asp:Literal></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btn_Delete" runat="server" CssClass="btn btn-danger"  Width="30%"
                                                ValidationGroup="Process" OnClientClick="return confirm ('تاكيد الحذف ؟');" Visible="False" OnClick="btn_Delete_Click"><i class="icon-trash bigger-110"></i>&nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Tokens,Delete%>"></asp:Literal></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btn_Cancel" runat="server" CssClass="btn btn-warning" 
                                                Width="30%" OnClick="btn_Cancel_Click" Visible="False"><i class="icon-arrow-right bigger-110"></i>&nbsp;<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Tokens,Cancel%>"></asp:Literal></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center">
                                    <asp:Label ID="lbl_ProcessResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                
            </td>
        </tr>
    </table>
    </div>
   <%--     <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        //هذه الدالة بعمل اخيار الازرار فى الـ Tree View 
        function client_OnTreeNodeChecked() {
            var obj = window.event.srcElement;
            var treeNodeFound = false;
            var checkedState;
            //checking whether obj consists of checkbox or not 
            if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                //easier to read
                var treeNode = obj;
                //record the checked state of the TreeNode 
                checkedState = treeNode.checked;
                //work our way back to the parent <table> element 
                do {
                    obj = obj.parentElement;
                } while (obj.tagName != "TABLE")
                //keep track of the padding level for comparison with any children 
                var parentTreeLevel = obj.rows[0].cells.length;
                var parentTreeNode = obj.rows[0].cells[0];
                //get all the TreeNodes inside the TreeView (the parent <div>) 
                var tables = obj.parentElement.getElementsByTagName("TABLE");
                //checking for any node is checked or unchecked during operation 
                if (obj.tagName == "TABLE") {
                    // if any node is unchecked then their parent node are unchecked 
                    if (!treeNode.checked) {
                        //head1 gets the parent node of the unchecked node 
                        var head1 = obj.parentElement.previousSibling;
                        if (head1.tagName == "TABLE") {
                            //checks for the input tag which consists of checkbox 
                            var matchElement1 = head1.getElementsByTagName("INPUT");
                            //matchElement1[0] gives us the checkbox and it is unchecked 
                            matchElement1[0].checked = false;
                        }
                        else {
                            head1 = obj.parentElement.previousSibling;
                        }
                        if (head1.tagName == "TABLE") {
                            //head2 gets the parent node of the unchecked node 
                            var head2 = obj.parentElement.parentElement.previousSibling;
                            if (head2.tagName == "TABLE") {
                                //checks for the input tag which consists of checkbox 
                                var matchElement2 = head2.getElementsByTagName("INPUT");
                                matchElement2[0].checked = false;
                            }
                        }
                        else {
                            head2 = obj.parentElement.previousSibling;
                        }
                        if (head2.tagName == "TABLE") {
                            //head3 gets the parent node of the unchecked node 
                            var head3 = obj.parentElement.parentElement.parentElement.previousSibling;
                            if (head3.tagName == "TABLE") {
                                //checks for the input tag which consists of checkbox 
                                var matchElement3 = head3.getElementsByTagName("INPUT");
                                matchElement3[0].checked = false;
                            }
                        }
                        else {
                            head3 = obj.parentElement.previousSibling;
                        }
                        if (head3.tagName == "TABLE") {
                            //head4 gets the parent node of the unchecked node 
                            var head4 = obj.parentElement.parentElement.parentElement.parentElement.previousSibling;
                            if (head4 != null) {
                                if (head4.tagName == "TABLE") {
                                    //checks for the input tag which consists of checkbox 
                                    var matchElement4 = head4.getElementsByTagName("INPUT");
                                    matchElement4[0].checked = false;
                                }
                            }
                        }
                    } //end if - unchecked
                    //total number of TreeNodes 
                    var numTables = tables.length
                    if (numTables >= 1) {
                        //cycle through all the TreeNodes 
                        //until we find the TreeNode we checked 
                        for (i = 0; i < numTables; i++) {
                            if (tables[i] == obj) {
                                treeNodeFound = true;
                                i++;
                                if (i == numTables) {
                                    //if we're on the last 
                                    //TreeNode then stop 
                                    //return;
                                    break;
                                }
                            }
                            if (treeNodeFound == true) {
                                var childTreeLevel = tables[i].rows[0].cells.length;
                                //if the current node is under the parent 
                                //the level will be deeper (greater) 
                                if (childTreeLevel > parentTreeLevel) {
                                    //jump to the last cell... it contains the checkbox 
                                    var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                    //set the checkbox to match the checkedState 
                                    //of the TreeNode that was clicked 
                                    var inputs = cell.getElementsByTagName("INPUT");
                                    inputs[0].checked = checkedState;
                                }
                                else {
                                    //if any of the preceding TreeNodes are not deeper stop 
                                    //return; 
                                    break;
                                }
                            } //end if 
                        } //end for 
                    } //end if - numTables >= 1
                    // if all child nodes are checked then their parent node is checked
                    if (treeNode.checked) {
                        var chk1 = true;
                        var head1 = obj.parentElement.previousSibling;
                        var pTreeLevel1 = obj.rows[0].cells.length;
                        if (head1.tagName == "TABLE") {
                            var tbls = obj.parentElement.getElementsByTagName("TABLE");
                            var tblsCount = tbls.length;
                            for (i = 0; i < tblsCount; i++) {
                                var childTreeLevel = tbls[i].rows[0].cells.length;
                                if (childTreeLevel = pTreeLevel1) {
                                    var chld = tbls[i].getElementsByTagName("INPUT");
                                    if (chld[0].checked == false) {
                                        chk1 = false;
                                        break;
                                    }
                                }
                            }
                            var nd = head1.getElementsByTagName("INPUT");
                            nd[0].checked = chk1;
                        }
                        else {
                            head1 = obj.parentElement.previousSibling;
                        }
                        var chk2 = true;
                        if (head1.tagName == "TABLE") {
                            var head2 = obj.parentElement.parentElement.previousSibling;
                            if (head2.tagName == "TABLE") {
                                var tbls = head1.parentElement.getElementsByTagName("TABLE");
                                var pTreeLevel2 = head1.rows[0].cells.length;
                                var tblsCount = tbls.length;
                                for (i = 0; i < tblsCount; i++) {
                                    var childTreeLevel = tbls[i].rows[0].cells.length;
                                    if (childTreeLevel = pTreeLevel2) {
                                        var chld = tbls[i].getElementsByTagName("INPUT");
                                        if (chld[0].checked == false) {
                                            chk2 = false;
                                            break;
                                        }
                                    }
                                }
                                var nd = head2.getElementsByTagName("INPUT");
                                nd[0].checked = (chk2 && chk1);
                            }
                        }
                        else {
                            head2 = obj.parentElement.previousSibling;
                        }
                        var chk3 = true;
                        if (head2.tagName == "TABLE") {
                            var head3 = obj.parentElement.parentElement.parentElement.previousSibling;
                            if (head3.tagName == "TABLE") {
                                var tbls = head2.parentElement.getElementsByTagName("TABLE");
                                var pTreeLevel3 = head2.rows[0].cells.length;
                                var tblsCount = tbls.length;
                                for (i = 0; i < tblsCount; i++) {
                                    var childTreeLevel = tbls[i].rows[0].cells.length;
                                    if (childTreeLevel = pTreeLevel3) {
                                        var chld = tbls[i].getElementsByTagName("INPUT");
                                        if (chld[0].checked == false) {
                                            chk3 = false;
                                            break;
                                        }
                                    }
                                }
                                var nd = head3.getElementsByTagName("INPUT");
                                nd[0].checked = (chk3 && chk2 && chk1);
                            }
                        }
                        else {
                            head3 = obj.parentElement.previousSibling;
                        }
                        var chk4 = true;
                        if (head3.tagName == "TABLE") {
                            var head4 = obj.parentElement.parentElement.parentElement.parentElement.previousSibling;
                            if (head4.tagName == "TABLE") {
                                var tbls = head3.parentElement.getElementsByTagName("TABLE");
                                var pTreeLevel4 = head3.rows[0].cells.length;
                                var tblsCount = tbls.length;
                                for (i = 0; i < tblsCount; i++) {
                                    var childTreeLevel = tbls[i].rows[0].cells.length;
                                    if (childTreeLevel = pTreeLevel4) {
                                        var chld = tbls[i].getElementsByTagName("INPUT");
                                        if (chld[0].checked == false) {
                                            chk4 = false;
                                            break;
                                        }
                                    }
                                }
                                var nd = head4.getElementsByTagName("INPUT");
                                nd[0].checked = (chk4 && chk3 && chk2 && chk1);
                            }
                        }
                    } //end if - checked
                } //end if - tagName = TABLE
            } //end if
        }

    </script>

    <script>
        $(document).ready(function () {
            $('#treePRIVILAGES2').find('a').each(function () {
                $(this).removeAttr("onclick").attr("href", "#").click(false);
            });
        });
    </script>
    <style type="text/css">
        #treePRIVILAGES2 a {
            color: black;
        }
    </style>
</asp:Content>
