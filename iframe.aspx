<%@ Page Language="VB" AutoEventWireup="false" CodeFile="iframe.aspx.vb" Inherits="iframe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

    <script src="Scripts/respond.min.js"></script>
    <script src="Scripts/html5shiv.min.js"></script>

           <script> $('#myTab a').click(function (e) {
                    e.preventDefault()
                    $(this).tab('show')
                    })
           </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 81px">

        <..! 測試..>


                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist">
                      <li class="active"><a href="#home" role="tab" data-toggle="tab">Home</a></li>
                      <li><a href="#profile" role="tab" data-toggle="tab">Profile</a></li>
                      <li><a href="#messages" role="tab" data-toggle="tab">Messages</a></li>
                      <li><a href="#settings" role="tab" data-toggle="tab">Settings</a></li>
                    </ul>

                    <!-- Tab panes -->
                    <div class="tab-content">
                      <div class="tab-pane fade in active" id="home">It is not a good idea!</div>
                      <div class="tab-pane fade" id="profile">Maybe I should go to sleep</div>
                      <div class="tab-pane fade" id="messages">...</div>
                      <div class="tab-pane fade" id="settings">...</div>
                    </div>








    
    </div>
    </form>
</body>
</html>
