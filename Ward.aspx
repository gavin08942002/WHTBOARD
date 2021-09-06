<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Ward.aspx.vb" Inherits="label1" %>

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
#map-top {
    width:1790px;
    height:300px;
}
#map-middle {
    width:1790px;
    height:200px;
}
#map-bottom {
    width:1790px;
    height:300px;
}
.div-table-style {/*病房外框格式*/
    width:100px;
    height:300px;
    background-color:#FFEFD5;
    float:left;
    border-width:1px;
    border-style:groove;
    border-radius:15px;
}
.table-cell1 {/*表格列1欄位1格式--病床標籤*/
    width:50px;
    
}
.table-cell2 {/*表格列1欄位2格式--病床號*/
    width:50px;
    font-size:16px;
    font-weight:bold;
    
}
.table-cell3 {/*表格列2欄位1格式--病患姓名-抬頭*/
    font-size:16px;
    font-weight:bold;
    color:#4eacac;
   
}
.table-cell4 {/*表格列1欄位2格式--病患姓名*/
    font-size:16px;
    font-weight:bold;
   
}
.table-cell5 {/*表格列3欄位1格式--謢理師姓名抬頭*/
    font-size:16px;
    font-weight:bold;
    color:#4eacac;
}
.table-cell6 {/*表格列1欄位2格式--護理師姓名*/
    font-size:16px;
    font-weight:bold;
}

.table-tr1 {/*表格列1格式*/
    height:30px;
}
.table-tr2 {/*表格列2格式*/
    height:30px;
}
.table-tr3 {/*表格列3格式*/
    height:30px;
}
.ward-number {
    width:100%;
    height:30px;
    line-height:30px;
    font-size:30px;
    font-weight:bold;
}
.badge-adj {
    display: inline-block;
    min-width: 10px;
    padding: 3px 7px;
    font-size: 14px;
    font-weight: 700;
    line-height: 1;
    color: #fff;
    text-align: center;
    white-space: nowrap;
    vertical-align: baseline;
    background-color: #7744FF;
    border-radius: 10px;
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
             <div id="Main">
                 <div id="Main_content" >
                     <div id="content_top" style="width:1790px;height:120px"><!--統計資訊框-->
                         
                                 <div style="width:600px;height:120px;float:left;"><!--輕中重統計顯示-->
                                     <table style="height:120px;  width: 100%;font-size:28px;font-weight:900;">
                                         <tr style="height:40px;text-align:center;"><!--輕中重字樣-->
                                             <td>輕</td>
                                             <td>中</td>
                                             <td>重</td>
                                         </tr>
                                         <tr style="height:80px;"><!--輕中重統計數據-->
                                             <td style="text-align:center;width:200px;">
                                                 <div style="width:120px;height:80px;margin: 0px auto;">
                                                     <h1 style="margin-top: 0px;margin-bottom: 0px;">
                                                         <span class="label label-success" style="font-size:90%;">1</span><!--輕度病患統計-->
                                                     </h1>
                                                 </div>
                                             </td>
                                             <td style="text-align:center;width:200px;">
                                                 <div style="width:120px;height:80px;margin: 0px auto;">
                                                     <h1 style="margin-top: 0px;margin-bottom: 0px;">
                                                         <span class="label label-warning" style="font-size:90%;">20</span><!--中度病患統計-->
                                                     </h1>
                                                 </div>
                                             </td>
                                             <td style="text-align:center;width:200px;">
                                                 <div style="width:120px;height:80px;margin: 0px auto;">
                                                     <h1 style="margin-top: 0px;margin-bottom: 0px;">
                                                         <span class="label label-danger" style="font-size:90%;">10</span><!--重度病患統計-->
                                                     </h1>
                                                 </div>
                                             </td>
                                             
                                         </tr>
                                     </table>
                                 </div>
                                 <div style="width:590px;height:120px; float:left">&nbsp;</div><!--空白-->
                                 <div style="width:600px;height:120px; float:left"><!--佔床率,空床,全部統計資料顯示-->
                                     <table style="height:120px;  width: 100%;font-size:28px;font-weight:900;">
                                         <tr style="height:40px;text-align:center;"><!--佔床率、空床、全部字樣-->
                                             <td>佔床率</td>
                                             <td>空床</td>
                                             <td>全部</td>
                                         </tr>
                                         <tr style="height:80px;"><!--佔床率、空床、全部統計數據-->
                                             <td style="text-align:center;width:200px;">
                                                 <div style="width:120px;height:80px;margin: 0px auto;">
                                                     <h1 style="margin-top: 0px;margin-bottom: 0px; ">
                                                         <span class="label label-default" style="font-size:90%;"><!--佔床率-->
                                                             <asp:Label ID="LabelWardRate" runat="server" Text="Label"></asp:Label>
                                                         </span>
                                                     </h1>
                                                 </div>
                                             </td>
                                             <td style="text-align:center;width:200px;">
                                                 <div style="width:120px;height:80px;margin: 0px auto;">
                                                     <h1 style="margin-top: 0px;margin-bottom: 0px;">
                                                         <span class="label label-primary" style="font-size:90%;"><!--空床數-->
                                                             <asp:Label ID="LabelEmptyBed" runat="server" Text="Label"></asp:Label>
                                                         </span>
                                                     </h1>
                                                 </div>
                                             </td>
                                             <td style="text-align:center;width:200px;">
                                                 <div style="width:120px;height:80px;margin: 0px auto;">
                                                     <h1 style="margin-top: 0px;margin-bottom: 0px;">
                                                         <span class="label label-default" style="font-size:90%;"><!--病床總數-->
                                                             <asp:Label ID="LabelWardAll" runat="server" Text="Label"></asp:Label>
                                                         </span>
                                                     </h1>
                                                 </div>
                                             </td>
                                             
                                         </tr>
                                     </table>
                                 </div>
                         
                     </div>
                     <div id="content_middle" style ="width:1790px;height:600px; font-weight:bold;"><!--病床位置圖-->
                         <div style="width:1790px;height:250px; background-color:#EEFFBB"><!--上層病床位置-->
                             <div style="width:700px;height:250px;float:left;"><!--左上病房區-->
                                 <div id="Div21" style="width:100px; height:250px;float:left;"><!--病房資料-1311-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1311B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1311A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1311</span></div>
                                 </div>
                                 <div id="Div1" style="width:100px; height:250px;float:left;"><!--病房資料-1315-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1315B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1315A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1315</span></div>
                                 </div>
                                 <div id="Div2" style="width:100px; height:250px;float:left;"><!--病房資料-1317-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1317B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1317A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1317</span></div>
                                 </div>
                                 <div id="Div3" style="width:100px; height:250px;float:left;"><!--病房資料-1319-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1319B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1319A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1319</span></div>
                                 </div>
                                 <div id="Div4" style="width:100px; height:250px;float:left;"><!--病房資料-1321-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1321B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1321A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1321</span></div>
                                 </div>
                                 <div id="Div5" style="width:100px; height:250px;float:left;"><!--病房資料-1323-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1323B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1323A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1323</span></div>
                                 </div>
                                 <div id="Div6" style="width:100px; height:250px;float:left;"><!--病房資料-1325-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1325B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1325A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1325</span></div>
                                 </div>


                                 

                             </div>
                             <div style="width:590px;height:250px;float:left; background-color:#DDDDDD"></div><!--中上護理站-->
                                 <div style="width:500px;height:250px;float:left;"><!--右上護理區-->
                                 <div id="Div7" style="width:100px; height:250px;float:left;"><!--病房資料-1327-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1327B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1327A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1327</span></div>
                                 </div>
                                 <div id="Div8" style="width:100px; height:250px;float:left;"><!--病房資料-1329-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1329B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1329A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1329</span></div>
                                 </div>
                                 <div id="Div9" style="width:100px; height:250px;float:left;"><!--病房資料-1331-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1331B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1331A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1331</span></div>
                                 </div>
                                 <div id="Div10" style="width:100px; height:250px;float:left;"><!--病房資料-1333-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr>
                                             <asp:Label ID="label1333A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1333</span></div>
                                 </div>
                                 <div id="Div11" style="width:100px; height:250px;float:left;"><!--病房資料-1335-->
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr>
                                             <asp:Label ID="label1335A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1335</span></div>
                                 </div>
                             </div>
                         </div>
                         <div style="width:1790px;height:100px"></div><!--走道-->
                         <div style="width:1790px;height:250px; background-color:#CCEEFF;"><!--下層病床位置-->
                             <div style="width:500px;height:250px;float:left"><!--左下病房區-->
                                 <div id="Div12" style="width:100px; height:250px;float:left;"><!--病房資料-1312-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1312</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="labe11312A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1312B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                                 <div id="Div13" style="width:100px; height:250px;float:left;"><!--病房資料-1316-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1316</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="Labe11316A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1316B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                                 <div id="Div14" style="width:100px; height:250px;float:left;"><!--病房資料-1318-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1318</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1318A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1318B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                                 <div id="Div15" style="width:100px; height:250px;float:left;"><!--病房資料-1320-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1320</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1320A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1320B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                                 <div id="Div16" style="width:100px; height:250px;float:left;"><!--病房資料-1322-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1322</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1322A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1322B" runat="server" Text=""></asp:Label><!--B-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                             </div>
                             <div style="width:890px;height:250px;float:left;background-color:#DDDDDD"></div><!--中下護理站-->
                             <div style="width:400px;height:250px;float:left"><!--右下病房區-->
                                 <div id="Div18" style="width:100px; height:250px;float:left;"><!--病房資料-1326-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1326</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1326A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1326B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                                 <div id="Div19" style="width:100px; height:250px;float:left;"><!--病房資料-1328-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1328</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1328A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1328B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                                 <div id="Div20" style="width:100px; height:250px;float:left;"><!--病房資料-1330-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1330</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1330A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1330B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>

                                 <div id="Div17" style="width:100px; height:250px;float:left;"><!--病房資料-1332-->
                                     <div style="width:100px;height:34px;text-align:center;"><span style="font-size:30px;font-weight:bold;">1332</span></div>
                                     <div style="width:100px; height:216px;border-style:groove;border-width:1px;">
                                     <table style="width: 100%; height:216px">
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1332A" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                         <tr style="height:72px;">
                                             <asp:Label ID="label1332B" runat="server" Text=""></asp:Label><!--A-->
                                         </tr>
                                     </table>
                                     </div>
                                 </div>
                             </div>
                         </div>
                     </div>
                     <div id="content_bottom" style=" width:1790px;height:80px;text-align:center;font-size:24px; font-weight:bold; line-height:80px"><!--時間顯示-->
                         更新時間:<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                     </div>
                     
                 </div>
                    
             </div>
         </div>
         <div id="Footer">
             <div id="Sub">
                   <div class="btn-group btn-group-justified"> 
                        <div class="btn-group">
                              <asp:LinkButton ID="Button1" runat="server" Text="病房資訊"    PostBackUrl="~/Ward.aspx"  class="btn btn-default myButton myButton_focus" CommandArgument="string.format({0},{1},(2),0,1,2)" />
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

                    
                        <asp:GridView ID="GridView1" runat="server">
                        </asp:GridView>

                    
                  </div>

             </div>
         </div>
         <div id="Space">Space 欄位</div>
    </form>
</body>
</html>
