<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Patient.aspx.vb" Inherits="Patient" %>

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
.Patient_sub {
      width:196px; 
      height:135px; 
      float:left; 
      background-color:#FFF8DC; 
      border-style:solid;
      border-radius:16px;
      border-width:1px;
      margin-left:1px;
      margin-bottom:2px;
      
      
      

}
.Text_align_left {
    text-align:left;
}
.Text_align_right {
    text-align:right;
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
             <div id="Main" >
                 <div id="Main_content" style="overflow-y:scroll;" >
                     <asp:Label ID="LabelWard" runat="server" Text=""></asp:Label><!--病床資料-->
                     <!--Sample
                     <div class="Patient_sub">
                         <table style="width: 100%;">
                             <tr style="width:100%; height:26px; background-color:#20B2AA;"  >
                                 <td style="width:56px; height:26px; border-top-left-radius:15px;">圖</td>
                                 <td style="width:120px; height:26px;">病床號</td>
                                 <td style="width:20px; height:26px;text-align:left; border-top-right-radius:15px;"><span style="color:#ffd800; font-weight:bold;">交</span></td>
                             </tr>
                             <tr style="width:100%; height:26px;"  >
                                 <td style="width:56px; height:26px">姓名：</td>
                                 <td style="width:120px; height:26px;">病床號</td>
                                 <td style="width:20px; height:26px;"><span style="color:#f00;font-weight:bold;">敏</span></td>
                             </tr>
                             <tr style="width:100%; height:52px;">
                                 <td style="width:56px; height:52px">主治：</td>
                                 <td style="width:120px; height:52px;">
                                     <div style="width:120px;height:52px;">
                                            <div style="width:60px; height:26px; float:left">豬八戒</div>
                                            <div style="width:60px; height:26px; float:left">豬八戒</div>
                                            <div style="width:60px; height:26px; float:left">豬八戒</div>
                                     </div>
                                 </td>
                             </tr>
                             <tr style="width:100%; height:26px;"  >
                                 <td style="width:56px; height:26px;border-bottom-left-radius:15px;">護理師</td>
                                 <td style="width:120px; height:26px;">黃罔市</td>
                                 <td style="width:20px; height:26px;"></td>
                             </tr>
                         </table>
                     </div>
                     -->
                 </div>
             </div>
         </div>
         <div id="Footer">
             <div id="Sub">
                <div class="btn-group btn-group-justified"> 
                        <div class="btn-group">
                              <asp:LinkButton ID="Button1" runat="server" Text="病房資訊"    PostBackUrl="~/Ward.aspx"  class="btn btn-default myButton "/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button2" runat="server" Text="病人資訊"    PostBackUrl="~/Patient.aspx" class="btn btn-default myButton myButton_focus"/>
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
        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    </form>
</body>

</html>
