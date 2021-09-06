<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Ward-Sample.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
<meta http-equiv="refresh" content="600">
    <title></title>
    
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/html5shiv.min.js"></script>
    <script src="Scripts/respond.js"></script>
    <link href="Content/E-board.css" rel="stylesheet" />

</head>
<body>

    <form id="form1" runat="server">
         <div id="Header">

             <div id="Head_inside">
                 <div >
                     <div class="Header_in">
                         按此更新=><button type="button" class="btn btn-danger btn-lg" id="PageRefresh">100F</button>
                                   <script type="text/javascript">
                                       $('#PageRefresh').click(function () {
                                           location.reload();
                                       });
                                   </script>
                     </div>
                     <div class="Header_in" style="text-align:right;">
                         <div id="Time"></div>
                              <script>/*時間顯示*/
                                  document.getElementById('Time').innerHTML = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                                  setInterval("document.getElementById('Time').innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());", 1000);
                             </script>
                     </div>
                 </div>
             </div>
             

         </div>
         <div id="Body">
             <div id="Main">Main</div>
         </div>
         <div id="Footer">
             <div id="Sub">
                <div class="btn-group btn-group-justified"> 
                        <div class="btn-group">
                              <asp:LinkButton ID="Button1" runat="server" Text="病房資訊"    PostBackUrl="~/Ward.aspx"  class="btn btn-default myButton myButton_focus"/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button2" runat="server" Text="病人資訊"    PostBackUrl="~/Patient.aspx" class="btn btn-default myButton "/>
                        </div>
                        <div class="btn-group"> 
                              <asp:LinkButton ID="Button3" runat="server" Text="手術/檢查"  PostBackUrl ="~/Surgery.aspx" class="btn btn-default myButton"/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button4" runat="server" Text="入/出院"     PostBackUrl="~/Hospitalize.aspx" class="btn btn-default myButton"/>  
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button5" runat="server" Text="護理師班表" PostBackUrl="~/Nurse.aspx" class="btn btn-default myButton"/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button6" runat="server" Text="醫師資訊" PostBackUrl="~/Doctor.aspx" class="btn btn-default myButton"/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button7" runat="server" Text="佈告欄" PostBackUrl="Board.aspx" class="btn btn-default myButton"/>
                        </div>

                    
            </div>

             </div>
         </div>
         <div id="Space">Space 欄位</div>
    </form>
</body>
</html>
