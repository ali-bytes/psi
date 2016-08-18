<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="NewIspNL.Pages.ErrorPage" %>
<%@ Import Namespace="Resources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Error</title>
     <meta charset="utf-8" />
         <meta name="description" content="smart isp" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="keywords" content="isp, pioneers, newisp,smart isp"/>
    
    <link href="../Content/ace-assest/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../Content/ace-assest/css/font-awesome.min.css" />
    <link href="../Content/ace-assest/css/jquery-ui-1.10.3.full.min.css" rel="stylesheet"type="text/css" />
    <link rel="stylesheet" href="../Content/ace-assest/css/ace-fonts.css" />
    <link rel="stylesheet" href="../Content/ace-assest/css/ace.min.css" />
    <link rel="stylesheet" href="../Content/ace-assest/css/ace-rtl.min.css" />
    <link rel="stylesheet" href="../Content/ace-assest/css/ace-skins.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        					<div class="page-content">
						<div class="row">
							<div class="col-xs-12">
        <div class="error-container" style="margin: 10%;">
									<div class="well">
										<h1 class="grey lighter smaller">
											<!--<span class="blue bigger-125">
												<i class="icon-random"></i>
												500
											</span>-->
											<%=Tokens.SomethingWentWrong %>
										</h1>

										<hr />
 

										<div class="space"></div>

										<div>
											<h4 class="lighter smaller"><%=Tokens.MeanwhileError %></h4>

											<ul class="list-unstyled spaced inline bigger-110 margin-15">
												<li>
													<i class="icon-hand-right blue"></i>
													<%=Tokens.Contacttechnicalsupport %>
												</li>

												<li>
													<i class="icon-hand-right blue"></i>
													<%=Tokens.exactInformation %>
												</li>
											</ul>
										</div>

										<hr />
										<div class="space"></div>

										<div class="center">
											<a href="#" class="btn btn-grey" onClick="history.go(-1); return false;">
												<i class="icon-arrow-left"></i>
												<%=Tokens.Back %>
											</a>

											<a href="home.aspx" class="btn btn-primary">
												<i class="icon-home"></i>
												<%=Tokens.Home %>
											</a>
										</div>
									</div>
								</div>
        
        </div></div></div>
   
    </form>
       
</body>
</html>