<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserControl.aspx.vb" Inherits="UserControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/html5shiv.min.js"></script>
    <script src="Scripts/respond.js"></script>
    <style type="text/css">
        .body {
            width:1280px;
            height:1024px;
            vertical-align:bottom
            
        }

    </style>
</head>
    
<body>
    <form id="form1" runat="server">
    <div class="body">
        <div style="width:1280px;height:224px;">
            <div class="btn-group margin:0px auto;">
                <button type="button" class="btn btn-default dropdown-toggle btn-primary" data-toggle="dropdown">
                      外白板<span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:HyperLink ID="HyperLink1" runat="server" >值班醫師</asp:HyperLink></li>
                   <li><asp:HyperLink ID="HyperLink2" runat="server">連絡人</asp:HyperLink></li>
                </ul>
            </div>
            <div class="btn-group margin:0px auto;">
                <button type="button" class="btn btn-default dropdown-toggle btn-success" data-toggle="dropdown">
                      醫生班表<span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:HyperLink ID="HyperLink3" runat="server" >班表</asp:HyperLink></li>
                </ul>
            </div>
            <div class="btn-group margin:0px auto;">
                <button type="button" class="btn btn-default dropdown-toggle btn-primary" data-toggle="dropdown">
                      佈告欄<span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:HyperLink ID="HyperLink4" runat="server" >內容編輯</asp:HyperLink></li>
                </ul>
            </div>
        </div>
        <div style="width:1280px;height:800px;"></div>  
        
    </div>
    </form>
</body>
</html>
