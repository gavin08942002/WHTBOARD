<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TEST1.aspx.vb" Inherits="AJAX_TEST1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../Scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="jquery.js"></script>
    <script language="JavaScript">
        $(document).ready(function(){
        $('#user_name').change(function () {
            $.ajax({
                url: 'RECEVER1.aspx',
                type: 'GET',
                data: {
                    user_name: $('#user_name').val()
                },
                error: function (xhr) {
                    alert('Ajax request 發生錯誤1');
                },
                success: function (response) {
                    alert('Ajax request 成功');
                }
            });
        });
        $("button").click(function () {
            $("#msg_user_name").hide();
            $('#button1:visible').hide(); // selector matched, button found
            // now hiding the button

           // $('#button1:hidden').show();
        });


        });

        checkRegAcc = function () {

            $.ajax({
                url: 'RECEVER1.aspx',
                type: 'GET',
                data: {
                    user_name: $('#user_name').val()
                },
                error: function (xhr) {
                    alert('Ajax request 發生錯誤');
                },
                success: function (response) {
                //    $('#msg_user_name').html(response);
                 //   $('#msg_user_name').fadeIn();
                    alert('Ajax request 發生錯誤');
                }
            });
        };
        //獲得外部內容TEST


        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p><input type="text" name="user_name" size="20" id="user_name">
            <span id="msg_user_name">sfgnsfn</span>

        </p>
        <div id="div1"><h2>使用 jQuery AJAX ?改?文本</h2></div>
       <button id="button1">?得外部?容</button>
    </div>

    </form>
</body>
</html>
