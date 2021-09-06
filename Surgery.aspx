<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Surgery.aspx.vb" Inherits="_Default" %>

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

<style type="text/css">
.table_title1 {
      background-color:#ff6a00;   
      font-size:16px; 
      font-weight:bold;
      color:#fff;

}
.table_title2 {
      background-color:#5599FF;   
      font-size:16px; 
      font-weight:bold;
      color:#fff;

}
.table-data-cell {
    
    
}
.table-title-cell {
    height:40px;
    
}
</style>
</head>
<body>

    <form id="form1" runat="server">
         <div id="Header">

             <div id="Head_inside">
                 <div >
                     <div class="Header_in">
                         按此更新=><button type="button" class="btn btn-danger btn-lg" id="PageRefresh">13F</button>
                                   <script type="text/javascript">
                                       $('#PageRefresh').click(function () {
                                           location.reload();
                                       });
                                   </script>
                       <!--  <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label> -->
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
             <div id="Main">
                              
                 <div id="Main_content">
                     <div style="width:1790px;height:380px;"><!--手術排程-->
                         <div style="width:40px;height:380px;float:left;border:1px; ">
                             <table style="width: 100%;font-size:32px; text-align:center;">
                                 <tr>
                                     <td style="font-weight:bold;color:#ff6a00;height:120px;">手<br>術<br>排<br>程</td>
                                 </tr>
                             </table>
                         </div>
                         <div style="width:1750px;height:380px;float:left;outline:double; ">
                             <div style="width:1750px;height:40px"><!--手術排程抬頭內容-->
                                 <table style="width: 100%;margin-bottom:0px" class="table table-bordered " ><!--手術排程抬頭-->
                                     <tr class="table_title1" >
                                         <td style="width:150px;height:40px">日期</td>
                                         <td style="width:150px;height:40px">床號</td>
                                         <td style="width:100px;height:40px">病歷號碼</td>
                                         <td style="width:200px;height:40px">姓名</td>
                                         <td style="width:200px;height:40px">地點</td>
                                         <td style="width:450px;height:40px">術式</td>
                                         <td style="width:150px;height:40px">醫師</td>
                                         <td style="width:350px;height:40px">狀態</td>
                                     </tr>
                                     </table>
                             </div>
                             <div style="width:1750px; height:340px; overflow-y:scroll;"><!--手術排程資料-->
                                 <table style="font-size:18px; font-weight:bold; margin-bottom:0px;word-break:break-all;" class="table table-bordered table-striped table-hover">
                                     <asp:Label ID="Label2" runat="server" Text=""></asp:Label><!--手術排程資料列-->
                                                                                                              
                                </table>
                             </div>
                         </div>
                     </div>
                     
                     <div style="width:1790px;height:40px;text-align:center;line-height:40px; font-size:24px; font-weight:bold;"><!--入出院時間更新資料-->
                         更新時間:<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                     </div>

                     <div style="width:1790px;height:380px;"><!--檢查排程-->
                         <div style="width:40px;height:380px;float:left;border:1px; ">
                             <table style="width: 100%;font-size:32px; text-align:center;">
                                 <tr>
                                         <td style="font-weight:bold;color:#5599FF;height:120px;">檢<br>查<br>排<br>程</td>
                                 </tr>
                             </table>
                         </div>
                         <div style="width:1750px;height:380px;float:left;outline:double; ">
                             <div style="width:1750px;height:40px"><!--檢查排程抬頭內容-->
                                 <table style="width: 100%;margin-bottom:0px" class="table table-bordered " ><!--檢查排程抬頭-->
                                     
                                     <tr class="table_title2" >
                                         <td style="width:150px;height:40px">日期</td>
                                         <td style="width:150px;height:40px">床號</td>
                                         <td style="width:200px;height:40px">姓名</td>
                                         <td style="width:200px;height:40px">地點</td>
                                         <td style="width:600px;height:40px">檢查項目</td>
                                         <td style="width:450px;height:40px">行動能力</td>
                                         
                                     </tr>
                                     
                                 </table>
                             </div>
                             <div style="width:1750px; height:340px; overflow-y:scroll;"><!--檢查排程資料-->
                                 <table style="font-size:18px; font-weight:bold; margin-bottom:0px;word-break:break-all;" class="table table-bordered table-striped table-hover">
                                     <asp:Label ID="Label3" runat="server" Text=""></asp:Label><!--檢查排程資料列-->
                                     <tr>
                                        <td style="width:150px;height:50px;vertical-align:middle;">日期</td>
                                         <td style="width:150px;height:50px;vertical-align:middle;">床號</td>
                                         <td style="width:200px;height:50px;vertical-align:middle;">姓名</td>
                                         <td style="width:200px;height:50px;vertical-align:middle;">地點</td>
                                         <td style="width:600px;height:50px;vertical-align:middle;">檢查項目</td>
                                         <td style="width:432px;height:50px;vertical-align:middle;">行動能力</td>
                                    </tr>
 
                                </table>

                                </div>

                         </div>

                     </div>
                     

                 </div>
                 

             

             </div>
         </div>
         <div id="Footer">
             <div id="Sub">
                <div class="btn-group btn-group-justified"> 
                        <div class="btn-group">
                              <asp:LinkButton ID="Button1" runat="server" Text="病房資訊"    PostBackUrl="~/Ward.aspx"  class="btn btn-default myButton"/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button2" runat="server" Text="病人資訊"    PostBackUrl="~/Patient.aspx" class="btn btn-default myButton "/>
                        </div>
                        <div class="btn-group"> 
                              <asp:LinkButton ID="Button3" runat="server" Text="手術/檢查"  PostBackUrl ="~/Surgery.aspx" class="btn btn-default myButton myButton_focus"/>
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
