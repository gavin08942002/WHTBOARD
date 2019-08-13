<%@ Page Language="VB" AutoEventWireup="false" CodeFile="_GET.aspx.vb" Inherits="AJAX_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../Scripts/jquery-1.7.1.min.js"></script>
    <script language="JavaScript">
        $(document).ready(function () {
            $("button").click(function () {
            //    alert("111111");
                $.ajax(
            {
                type: "GET",
                url: "_GET.aspx",
                success: function (data) {
                    alert(data);
                },
                 Error: function (){
                     alert("error!");
                    },
 
            });
            });
        });
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="Button1" type="button" value="button" />

        <button>向页面发送 HTTP POST 请求，并获得返回的结果</button>
    </div>
    </form>
</body>
</html>
