<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Nurse-2.aspx.vb" Inherits="_Default" %>

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
.Title_font {
}
.table>tbody>tr>td{/*修正.table>tbody>tr>td*/
    padding:0px;
    vertical-align:middle;
}
.table {/*修正Bootstrap .table*/
    margin-bottom:0px;
}
#Main_button {
    width:1770px; 
    height:80px; 
    margin:0px auto;
    background-color:#EEE8AA;
}
.btn-lg-adj{/*修正Button的大小*/
padding: 15px 24px;
font-size: 36px;
line-height: 1.33;
border-radius: 20px;
}
    .nurse-name {
    font-size:24px;
    font-weight:bolder;
}
    .employees-number {
    font-size:18px;
    color:#f00;
    font-weight:bolder;   
}
    .ward-number-button {
    font-size:24px;
    float:left; 
    width:117px;
    height:60px;
    line-height:60px;
    text-align:center;
    border-radius:20px;
    background-color:#FFD700; 
    border-width:1px;
    border-style:solid;
    
      
}



</style>
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
             <div id="Main">
                 <div id="Main_content">
                     <div style="width:1770px; height:720px; margin:0px auto; overflow:auto">
                         <table class="table" style="width: 100%;">
                             <tr style="height:120px;background-color:#FFF8DC"><!--資料列-->
                                 <td style="width:200px;">
                                     <div style="width:200px;height:120px;border-radius:40px;background-color:#66CDAA">
                                     <p style="font-size:16px; ">(7)</p>
                                     <p class="nurse-name">劉曉穎</p>
                                     <p class="employees-number">7330</p>
                                     </div>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                         <div class="ward-number-button">11b</div>
                                        
                                         
                                     </div>
                                 </td>
                                 <td style="width:200px;height:120px;"> 
                                     <div style="width:200px;height:120px;background-color:wheat;border-radius:40px;vertical-align:middle;padding-top:30px">
                                     <p class="nurse-name">代理人</p>
                                     <p class="employees-number">0894</p>
                                     </div>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;font-size:24px;font-weight:bold;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr style="height:120px"><!--資料列-->
                                 <td style="width:200px;">
                                     <p style="font-size:18px;">(床數)</p>
                                     <p>護理師姓名</p>
                                     <p>2143</p>
                                 </td>
                                 <td style="width:1170px;">
                                     <div style="width:1170px; height:120px;">
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                         <div style="float:left; width:117px;height:60px;">11b</div>
                                     </div>
                                 </td>
                                 <td style="width:200px;"> 
                                     <p>代理人</p>
                                     <p>員工編號</p>
                                 </td>
                                 <td style="width:200px;">
                                     <table style="width: 100%;height:120px;">
                                         <tr>
                                             <td>G</td>
                                         </tr>
                                         <tr>
                                            <td>引導</td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>






                         </table>
                         
                     </div>
                     <div id="Main_button"><!--日晚夜及排序列-->
                         <div style="width:590px; height:80px; float:left;"><!--日中晚班按鈕-->
                             <table style="width: 100%; height:80px;">
                                 <tr>
                                     <td><asp:Button ID="Button8" runat="server" Text="早班" class="btn btn-primary btn-lg btn-lg-adj"/></td>
                                     <td><asp:Button ID="Button9" runat="server" Text="晚班" class="btn btn-info btn-lg btn-lg-adj"/></td>
                                     <td><asp:Button ID="Button10" runat="server" Text="夜班" class="btn btn-success btn-lg btn-lg-adj"/></td>
                                 </tr>
                             </table>
                         </div>
                         <div style="width:590px; height:80px; float:left; text-align:center; line-height:80px;"><!--更新時間-->
                             
                             <h3>更新時間:<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></h3>
                         </div>
                         <div style="width:590px; height:80px; float:left;"><!--依病床、護理師排序按鈕-->
                              <table style="width: 100%; height:80px;">
                                 <tr>
                                     <td><asp:Button ID="Button13" runat="server" Text="月班表" class="btn btn-warning btn-lg btn-lg-adj"/></td>
                                     <td><asp:Button ID="Button11" runat="server" Text="依病床" class="btn btn-warning btn-lg btn-lg-adj"/></td>
                                     <td><asp:Button ID="Button12" runat="server" Text="依護理師" class="btn btn-danger btn-lg btn-lg-adj"/></td>
                                     
                                 </tr>
                             </table>
                         </div>

                     </div>
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
                              <asp:LinkButton ID="Button2" runat="server" Text="病人資訊"    PostBackUrl="~/Patient.aspx" class="btn btn-default myButton "/>
                        </div>
                        <div class="btn-group"> 
                              <asp:LinkButton ID="Button3" runat="server" Text="手術/檢查"  PostBackUrl ="~/Surgery.aspx" class="btn btn-default myButton"/>
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button4" runat="server" Text="入/出院"     PostBackUrl="~/Hospitalize.aspx" class="btn btn-default myButton"/>  
                        </div>
                        <div class="btn-group">
                              <asp:LinkButton ID="Button5" runat="server" Text="護理師班表" PostBackUrl="~/Nurse.aspx" class="btn btn-default myButton myButton_focus"/>
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
