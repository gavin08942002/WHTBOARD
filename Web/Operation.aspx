<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Operation.aspx.vb" Inherits="Web_Default" %>

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
    <form id="form1" runat="server">
    <div id ="Body">
            <script type="text/javascript">
                $(document).ready(function () {
                    //表格雙數行變色
                    $("#surgery table.Table_style  tr:odd,#examination table.Table_style  tr:odd").css("background-color", "#B2E0FF");
                    //無操作者姓名,"動態護理"反灰
                    //var UserName = document.getElementById("User_Label").innerText;
                    //if (UserName == "")
                    //{
                    //    $("#Button2 Span").css("color", "#9d9d9d");
                    //}


                    //判斷檔案名稱,讓Button變色
                    var script_name = document.location.pathname.match(/[^\/]+$/)[0];
                    console.log(script_name);
                    switch (script_name)
                               {
                        case "Operation.aspx":
                            $("#Button3").addClass("active").css("color", "#dff0d8");

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
                //動態護理連結
                function Link_To_k234() {
                    var HospCode = $.url.param("HospCode");
                    var WardCode = $.url.param("WardCode");
                    var to_nurses = "http://websrv2.hc.mmh.org.tw/Nsmove/webform.aspx?HospCode=" + HospCode + "&WardCode=" + WardCode
                    var UserName = document.getElementById("User_Label").innerText;
                    window.open(to_nurses)
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
            <div id ="topic"><span class ="big-red">手術檢查</span></div>
            <hr class="hr_margin" />
            <!--資料內容-->
            <div id ="content">
                <!--時間資料和按鈕-->
                <div class ="info">
                    <div style="width: 33%; height: 100%;float:left;" ></div>
                    <div style="width: 33%; height: 100%;float:left;" ></div>
                    <div style="width: 33%; height: 100%;float:left;" >
                        <div style =" width:20%; height:100%; float:left; line-height:50px; text-align:right;" >
                        </div>
                        <div style =" width:60%; height:100%; float:left; line-height:50px; text-align:center;" class ="big-red">
                        </div>
                        
                    </div>
                </div>
                <!--手術排程-->
                <div class ="Table_Title_big_blue">手術排程</div>
                <div id ="surgery">
                    <table   class ="Table_style">
                          <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:7%;">日期</td>
                                <td class="Table_td_style" style="width:7%;">報到</td>
                                <td class="Table_td_style" style="width:5%;">床號</td>
                                <td class="Table_td_style" style="width:11%;">病歷號碼</td>
                                <td class="Table_td_style" style="width:10%;">姓名</td>
                                <td class="Table_td_style" style="width:40%;">術式</td>
                                <td class="Table_td_style" style="width:10%;">醫師</td>
                                <td class="Table_td_style" style="width:10%;">狀態</td>
                        </tr>
                        <!--
                        <tr class ="Table_tr_style1">
                                 <td class="Table_td_style">11/22</td>
                                <td class="Table_td_style">32A</td>
                                <td class="Table_td_style">12345678</td>
                                <td class="Table_td_style">馬英九</td>
                               
                                <td class="Table_td_style">on PEGwefqwfwqefqwf wergwferg ergegasvaewveqfbaeb ;</td>
                                <td class="Table_td_style">王小明 4276</td>
                                <td class="Table_td_style">取消報到</td>
                        </tr>
                        -->
                        <asp:Label ID="Operation_List" runat="server" Text="Label"></asp:Label>
                    </table>
                </div>
                <hr />
                <!--檢查排程-->
                <div class ="Table_Title_big_blue">檢查排程</div>
                <div id="examination">
                    <table class ="Table_style">
                        <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:5%;">日期</td>
                                <td class="Table_td_style" style="width:7%;">時間</td>
                                <td class="Table_td_style" style="width:5%;">床號</td>
                                <td class="Table_td_style" style="width:10%;">病歷號碼</td>
                                <td class="Table_td_style" style="width:10%;">姓名</td>
                                <td class="Table_td_style" style="width:13%;">地點</td>
                                <td class="Table_td_style" style="width:50%;">檢查項目</td>
                        </tr>
                        <!--
                        <tr class ="Table_tr_style1">
                                <td class="Table_td_style">11/22</td>
                                <td class="Table_td_style">13:46</td>
                                <td class="Table_td_style">15B</td>
                                <td class="Table_td_style">12345676</td>
                                <td class="Table_td_style">連小文</td>
                                <td class="Table_td_style">(台北)放射科</td>
                                <td class="Table_td_style">C.T. WITHOUT ENHANCEMENT電腦斷層造影無造</td>
                                <td class="Table_td_style"">
                                </td>
                        </tr>
                        -->
                        <asp:Label ID="Phy_LIST" runat="server" Text="Label"></asp:Label>
                    </table>
                </div>
                <hr />


            </div>
        </div>
        
    </div>

        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView3" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView7" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView4" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView5" runat="server">
        </asp:GridView>

        <asp:GridView ID="GridView6" runat="server">
        </asp:GridView>

    </form>
</body>
</html>
