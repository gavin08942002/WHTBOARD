<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

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
    <script src="http://www.clocklink.com/embed.js"></script> 
  
    <style type="text/css">
        .auto-style1 {
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 500px;width:400px;">
    
        <div style="height: 100px;width:200px;float:left;border-bottom-width: 30px;">
            <asp:Label ID="Label4" runat="server" Text="Label4"></asp:Label>
        </div>
        <div style="height: 100px;width:200px">
            <asp:Repeater ID="Repeater1" runat="server">
            </asp:Repeater>
        </div>
        <div style="height: 100px;width:200px">
            <asp:Label ID="Label3" runat="server" Text="Label3"></asp:Label>
            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
        </div>
        <div style="height: 100px;width:200px; background-color:#ff6a00; border-radius:50px 0 0 0 ;">
            <asp:Button ID="Button1" runat="server" Text="Button" />
        </div>
        <div style="height: 100px;width:200px">
            <table style="width:100%;">
                <tr>
                    <td class="auto-style1"></td>
                    <td class="auto-style1"></td>
                    <td class="auto-style1"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td rowspan="2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <div style="height: 100px;width:200px">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        </div>
    
    </div>
    </form>
</body>
</html>
