<%@ Page Title="<%$Resources:Tokens,UpdateDemands%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UpdateDemands.aspx.cs" Inherits="NewIspNL.Pages.UpdateDemands" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    \
    <style type="text/css">
        .btn-custom {
            background-color: #24748f !important;
            background-color: hsl(195, 60%, 35%) !important;
  background-repeat: repeat-x;
            -webkit-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#2d95b7", endColorstr="#23748e");
            -moz-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#2d95b7", endColorstr="#23748e");
            -o-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#2d95b7", endColorstr="#23748e");
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#2d95b7", endColorstr="#23748e");
  background-image: -khtml-gradient(linear, left top, left bottom, from(#2d95b7), to(#23748e));
  background-image: -moz-linear-gradient(top, #2d95b7, #23748e);
  background-image: -ms-linear-gradient(top, #2d95b7, #23748e);
  background-image: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #2d95b7), color-stop(100%, #23748e));
  background-image: -webkit-linear-gradient(top, #2d95b7, #23748e);
  background-image: -o-linear-gradient(top, #2d95b7, #23748e);
  background-image: linear-gradient(#2d95b7, #23748e);
            border-color: #23748e #23748e #216c85;
            border-color: #23748e #23748e hsl(195, 60%, 32.5%);
  color: #fff !important;
            -ms-text-shadow: 0 -1px 0 #000000;
            -ms-text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.16);
            text-shadow: 0 -1px 0 #000000;
            text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.16);
  -webkit-font-smoothing: antialiased;
        }
        .btn-custom2 {
            background-color: #cc7a00 !important;
            background-color: hsl(36, 100%, 40%) !important;
  background-repeat: repeat-x;
            -webkit-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#ffad32", endColorstr="#cc7a00");
            -moz-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#ffad32", endColorstr="#cc7a00");
            -o-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#ffad32", endColorstr="#cc7a00");
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#ffad32", endColorstr="#cc7a00");
  background-image: -khtml-gradient(linear, left top, left bottom, from(#ffad32), to(#cc7a00));
  background-image: -moz-linear-gradient(top, #ffad32, #cc7a00);
  background-image: -ms-linear-gradient(top, #ffad32, #cc7a00);
  background-image: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #ffad32), color-stop(100%, #cc7a00));
  background-image: -webkit-linear-gradient(top, #ffad32, #cc7a00);
  background-image: -o-linear-gradient(top, #ffad32, #cc7a00);
  background-image: linear-gradient(#ffad32, #cc7a00);
  border-color: #cc7a00 #cc7a00 hsl(36, 100%, 35%);
  color: #333 !important;
            -ms-text-shadow: 0 1px 1px #ffffff;
            -ms-text-shadow: 0 1px 1px rgba(255, 255, 255, 0.33);
            text-shadow: 0 1px 1px #ffffff;
            text-shadow: 0 1px 1px rgba(255, 255, 255, 0.33);
  -webkit-font-smoothing: antialiased;
        }
    </style>
        <div class="view">
        <fieldset style="width: 95%">
           
            <button id="Button1" class="btn btn-primary btn-block" runat="server" OnServerClick="Update">
                <i class="icon-refresh bigger-130"></i> </button>
                <br/><br/><br/>
                <button id="btn2" class="btn btn-danger btn-block" runat="server" OnServerClick="UpdateThisMonth">
                    <i class="icon-refresh bigger-130"></i>&nbsp;<%=Tokens.UpdateDemandthisMonth %>
                </button>
             <br/><br/><br/>
                <button id="nextmonth" class="btn btn-info btn-block" runat="server" OnServerClick="UpdatenextMonth">
                    <i class="icon-refresh bigger-130"></i>&nbsp;<%=Tokens.updatedemandnextmonth %>
                </button>
            <br/><br/><br/>
                <button id="btnres" class="btn btn-custom btn-block" runat="server" OnServerClick="UpdateThisMonthForReseller">
                    <i class="icon-refresh bigger-130"></i>&nbsp;<%=Tokens.UpdateResellerDemands %>
                </button>
             <br/><br/><br/>
                <button id="btndir" class="btn btn-custom2 btn-block" runat="server" OnServerClick="UpdateThisMonthForDirectUser">
                    <i class="icon-refresh bigger-130"></i>&nbsp;<%=Tokens.UpdateDirectCustDemands %>
                </button>
             <br/><br/><br/>
             <div>
                            <label>
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Provider %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlProvider" EnableViewState="True"/>
                            </div>
                        </div>
             <br />
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,DaysCount %>"></asp:Literal>
                                :
                                <asp:TextBox runat="server" ID="txtDaysCount"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="txtDaysCount" runat="server" />
                                <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="txtDaysCount"
                                    Type="Integer" Operator="DataTypeCheck" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"></asp:CompareValidator>
                <button id="Button2" class="btn btn-custom2 btn-block" runat="server" OnServerClick="Addrequestsuspend">
                    <i class="icon-refresh bigger-130"></i>&nbsp;<%=Tokens.Add_Request %>&nbsp;<%=Tokens.Suspend %>
                </button>
            <span runat="server" id="msg">
            </span>
        </fieldset>
    </div>
  
    <script type="text/javascript">
        $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
    </script>
</asp:Content>
