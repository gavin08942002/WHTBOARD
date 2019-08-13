<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Nurses.aspx.vb" Inherits="Web_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="CSS/HeadStyle.css" rel="stylesheet" />
    <script src="JavaScript/HeadScript.js"></script>
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="JavaScript/bootstrap.min.js"></script>
    <script src="JavaScript/jquery-ui-1.12.1.custom/jquery-ui.js"></script>
    <link href="JavaScript/jquery-ui-1.12.1.custom/jquery-ui.theme.css" rel="stylesheet" />
    <link href="JavaScript/jquery-ui-1.12.1.custom/jquery-ui.structure.css" rel="stylesheet" />
    <script src="JavaScript/jquery.url.js"></script>
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="JavaScript/datepicker-zh-TW.js"></script>
    <title>馬偕紀念醫院住院病房護理查詢系統</title>
</head>
<body style ="color: rgb(102,102,102); background-color: rgb(242,242,242)" >
                <script type="text/javascript">
                    //日期選擇器
                    $(function () {
                        $("#datepicker").datepicker();
                    });
                    $(document).ready(function () {
                        //表格雙數行變色
                        $("#NurseDen table.Table_style  tr:odd").css("background-color", "#B2E0FF");
                        //無操作者姓名,"動態護理"反灰
                        var UserName = document.getElementById("User_Label").innerText;
                        if (UserName == "") {
                            $("#Button2 Span").css("color", "#9d9d9d");
                        }
                        //判斷檔案名稱
                        var script_name = document.location.pathname.match(/[^\/]+$/)[0];
                        console.log(script_name);
                        switch (script_name) {
                            case "Operation.aspx":
                                $("#Button3").addClass("active").css("color", "#dff0d8");
                                //  $("#Button3 Span").css("color", "rgb(173,1,1)");
                                break;
                            case "Nurses.aspx":
                                $("#Button5").addClass("active").css("color", "#dff0d8");
                                break;
                            case "Activity.aspx":
                                $("#Button1").addClass("active").css("color", "#dff0d8");
                                break;
                        }
                    });
                    function Link_To(page) {
                        var HospCode = $.url.param("HospCode");
                        var WardCode = $.url.param("WardCode");
                        var to_nurses = page + ".aspx?HospCode=" + HospCode + "&WardCode=" + WardCode
                        self.location.href = to_nurses
                    }
                    function Link_To_k234() {
                        var HospCode = $.url.param("HospCode");
                        var WardCode = $.url.param("WardCode");
                        var to_nurses = "http://websrv2.hc.mmh.org.tw/Nsmove/webform1.aspx?HospCode=" + HospCode + "&WardCode=" + WardCode
                        var UserName = document.getElementById("User_Label").innerText;
                        if (UserName == "")
                        { alert("無操作者資料!!,\n請關閉目前網頁,重新登入急住醫令-護理,\n重新連結護理內白板") }
                        else
                        { window.open(to_nurses) }
                    }
                    //按日期查詢
                    function To_Date() {
                        var HospCode = $.url.param("HospCode");
                        var WardCode = $.url.param("WardCode");
                        var GoalDate = $("#datepicker").val();
                        var to_pages = "Nurses.aspx?HospCode=" + HospCode + "&WardCode=" + WardCode + "&todate=" + GoalDate
                        self.location.href = to_pages
                    }

                    //其它動態連結
                    function Link_To_k234_oth() {
                        var HospCode = $.url.param("HospCode");
                        var WardCode = $.url.param("WardCode");
                        var to_nurses = "http://websrv2.hc.mmh.org.tw/Nsmoveoth/index.aspx "
                        var UserName = document.getElementById("User_Label").innerText;
                        if (UserName == "")
                        { alert("無操作者資料!!,\n請關閉目前網頁,重新登入急住醫令-護理,\n重新連結護理內白板") }
                        else
                        { window.open(to_nurses) }
                    }
   </script>

    <form id="form1" runat="server">
    <div id ="Body">
        <div id ="head">
            <!--馬偕LOGO及系統名稱--><div style ="width:1024px; height:60px;">  
                <div style ="width:512px; height:60px; float:left; text-align:left; line-height:60px;">
                    <div style ="width:20px; height:60px; float:left;"></div>
                    <asp:Image ID="LOGO_Image" runat="server" />
                </div>
                <div style ="width:512px; height:60px; float:left; text-align:right;line-height:60px;">
                    <img style="width:360px; height:48px; padding-right:10px;" src="image/title2.png" />
                </div>
            </div>
            <!--病房名稱,  查詢時間,   操作者-->
            <div class ="info">
                <div style ="width:25%; height:50px; float:left; line-height:50px;">
                    <span class ="big-red">
                        <asp:Label ID="Ward_Name_Label" runat="server" Text="Label"></asp:Label>
                    </span>
                </div>
                <div style ="width:41%; height:50px; float:left; text-align:center; line-height:50px">
                                 <div id="Showclock">
                              <script>/*時間顯示*/
                                  document.getElementById('Showclock').innerHTML = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                             </script>
                         </div>
                </div>
                <div style ="width:33%; height:50px; float:left;text-align:right; line-height:50px;">
                    <span class ="big-blue">
                        操作者：<asp:Label ID="User_Label" runat="server" Text="Label"></asp:Label>
                    </span>  
                </div>
            </div>
            <!--留空-->
            <div style ="width: 100%; height:10px;"></div>
            <!--超連結圖示-->
            <div style ="width:1024px; height:45px;">
                <div style="width:880px; height:45px; margin:0px auto;">
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink3" runat="server"> 
                            <button id="Button3" class ="btn btn-primary btn-lg" type="button" onclick ="Link_To('Operation')" >   
                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true">手術檢查</span>
                            </button>
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                           <asp:HyperLink ID="HyperLink5" runat="server"> 
                               <button id="Button5" class ="btn btn-primary btn-lg" type="button"  onclick ="Link_To('Nurses')">   
                                  <span class="glyphicon glyphicon-tasks" aria-hidden="true">護理排班</span>
                               </button>
                             </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                              <asp:HyperLink ID="HyperLink1" runat="server"> 
                               <button id="Button1" class ="btn btn-primary btn-lg" type="button" onclick ="Link_To('Activity')"  >   
                                  <span class="glyphicon glyphicon-road" aria-hidden="true">行動能力</span>
                               </button>
                             </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                                  <button id="Button2" class ="btn btn-primary btn-lg" type="button" onclick ="Link_To_k234()"  >   
                                  <span class="glyphicon glyphicon-send" aria-hidden="true">動態護理</span>
                               </button>
                    </div>
                    <div class ="Topic_Image" >
                               <button id="Button4" class ="btn btn-primary btn-lg" type="button" onclick ="Link_To_k234_oth()"  >   
                                  <span class="glyphicon glyphicon-send" aria-hidden="true">其它動態</span>
                               </button>
                    </div>
                    <div class ="Topic_Image" >
                    </div>
                </div>
            </div>
        </div>
        
        <div id ="main">
            <!--網頁標題-->
            <div id ="topic"><span class ="big-red">護理排班</span></div>
            <hr  class="hr_margin"/>
            <!--資料內容-->
            <div id ="content">
                                <!--時間資料和按鈕-->
                <div class ="info" imageurl="image/time_day_b.png">
                    <div style="width: 33%; height: 100%;float:left;" >
                        <input id="datepicker" type="text" readonly="readonly" />
                        <button class="ui-button  ui-widget ui-corner-all" type="button"  style =" font-size:0.8em;" onclick ="To_Date()" >查詢</button>
                    </div>
                    <div style="width: 33%; height: 100%;float:left;" class ="Table_Title_big_blue" >
                        排班查詢日期：<asp:Label ID="LabelCheckdate" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div style="width: 33%; height: 100%;float:left;" >

                    </div>
                </div>
                 <!--依護理師-->
                <div  class ="Table_Title_big_blue">護理師班表</div>
                <div id="NurseDen">
                    <table  class ="Table_style">
                          <tr class ="Table_tr_header_style">
                              <td class="Table_td_style" style="width:7%;">班別</td>
                                <td class="Table_td_style" style="width:10%;">護理師</td>
                                <td class="Table_td_style" style="width:54%;">專責床位</td>
                                <td class="Table_td_style" style="width:7%;">床數</td>
                                <td class="Table_td_style" style="width:7%;">任務</td>
                                <td class="Table_td_style" style="width:15%;">消防</td>
                       
                        </tr>
<!--         
                        <tr class ="Table_tr_style1">
                                 <td class="Table_td_style" >D</td>
                            <td class="Table_td_style" >A</td>
                                <td class="Table_td_style" >王品淳</td>
                                <td class="Table_td_style" style="word-wrap:break-word" >11B/22A/22C</td>
                                <td class="Table_td_style" >10</td>
                                <td class="Table_td_style" >通報</td>
                        </tr>
                        -->
                        <asp:Label ID="NurseBedListShow" runat="server" Text="目前無資料!!"></asp:Label>
                    </table>
                </div>
                <hr />
                <!--依病床號-->

            </div>
        </div>
        
    </div>

        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView3" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView4" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView5" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView6" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView7" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView8" runat="server">
        </asp:GridView>

    </form>
</body>
</html>
