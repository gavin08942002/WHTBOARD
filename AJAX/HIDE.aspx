<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HIDE.aspx.vb" Inherits="AJAX_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="../Scripts/jquery-1.7.1.min.js"></script>
        <script type="text/javascript" src="jquery.js"></script>
        <script language="JavaScript">
            $(document).ready(function () {
                $("button").click(function () {
                    alert("Value: " + $("#test").val());
                });
            });
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<p>姓名：<input type="text" id="test" value="米老鼠"></p>
<button>顯示值</button>
    </div>
    </form>
</body>
</html>
