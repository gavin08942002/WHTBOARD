<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Inout.aspx.vb" Inherits="Web_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <link href="CSS/HeadStyle.css" rel="stylesheet" />
    <link href="CSS/Patients.css" rel="stylesheet" />
    <script src="JavaScript/HeadScript.js"></script>

    <title>馬偕紀念醫院住院病房護理查詢系統</title>
</head>
<body style ="color: rgb(102,102,102); background-color: rgb(242,242,242)" >
    <form id="form1" runat="server">
    <div id ="Body">
        <div id ="head">
            <!--馬偕LOGO及系統名稱-->
            <div style ="width:1024px; height:60px;">
                <div style ="width:512px; height:50px; float:left; text-align:left;">
                    <img style="width:240px; height:50px; padding-left:20px;" alt src="image/mmhlogo.png" />
                </div>
                <div style ="width:512px; height:50px; float:left; text-align:right;">
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
                    <span class ="big">
                        查詢時間：<asp:Label ID="Update_Time_Label" runat="server" Text="Label"></asp:Label>
                    </span>
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
            <div style ="width:1024px; height:60px;">
                <div style="width:880px; height:60px; margin:0px auto;">
                    <div class ="Topic_Image" >
                       <asp:HyperLink ID="HyperLink2" runat="server"> 
                            <img class ="Topic_image_file"  alt src="image/menu_b2.png" onmouseover="over(this, 'image/menu_r2.png')" onmouseout="out(this, 'image/menu_b2.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink3" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b3.png" onmouseover="over(this, 'image/menu_r3.png')" onmouseout="out(this, 'image/menu_b3.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink4" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b4.png" onmouseover="over(this, 'image/menu_r4.png')" onmouseout="out(this, 'image/menu_b4.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink5" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b5.png" onmouseover="over(this, 'image/menu_r5.png')" onmouseout="out(this, 'image/menu_b5.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink6" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b6.png" onmouseover="over(this, 'image/menu_r6.png')" onmouseout="out(this, 'image/menu_b6.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink7" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b7.png" onmouseover="over(this, 'image/menu_r7.png')" onmouseout="out(this, 'image/menu_b7.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink8" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b8.png" onmouseover="over(this, 'image/menu_r8.png')" onmouseout="out(this, 'image/menu_b8.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink1" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b1.png" onmouseover="over(this, 'image/menu_r1.png')" onmouseout="out(this, 'image/menu_b1.png')"  />
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
        
        <div id ="main">
            <!--網頁標題-->
            <div id ="topic"><span class ="big-red">入/出院</span></div>
            <hr />
            <!--資料內容-->
            <div id ="content">
                <!--統計資料-->
                <div  id="statistic" >
                    <div >
                        <div class ="class_statistic_1 big" >入院</div>
                        <div class ="class_statistic_2 big " style ="background-color: rgb(173, 1, 1); color:white;" >28</div>
                    </div>
                    <div >
                        <div class ="class_statistic_1 big" >轉入</div>
                        <div class ="class_statistic_2 big " style ="background-color: rgb(173, 1, 1);color:white;" >28</div>
                    </div>
                    <div >
                        <div class ="class_statistic_1 big" >共計</div>
                        <div class ="class_statistic_2 big "  style ="background-color: rgb(173, 1, 1);color:white;">28</div>
                    </div>
                    <div  style ="width:230px;">
                        <div style ="width:130px;height:70%; float:left; line-height:50px; text-align:right;" class="big" >出院</div>
                        <div style="width:100px; height:70%; float:left; line-height:50px;background-color:rgb(79,129,189);color:white;" class ="big "  >28</div>
                    </div>
                </div>
                <!--入/出院-->
                <div class ="Table_Title_big_blue">人院名單</div>
                <div>
                    <table  class ="Table_style">
                          <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:5%;">床號</td>
                                <td class="Table_td_style" style="width:5%;">註記</td>
                                <td class="Table_td_style" style="width:10%;">病歷號碼</td>
                                <td class="Table_td_style" style="width:10%;">姓名</td>
                                <td class="Table_td_style" style="width:5%;">性別</td>
                                <td class="Table_td_style" style="width:5%;">年齡</td>
                                <td class="Table_td_style" style="width:12%;">主治</td>
                                <td class="Table_td_style" style="width:35%;">主診斷</td>
                              <td class="Table_td_style" style="width:13%;">備註</td>
                        </tr>
            <!--            <tr class ="Table_tr_style1">
                                 <td class="Table_td_style" style="width:5%;">11/22</td>
                                <td class="Table_td_style" style="width:5%;">入院</td>
                                <td class="Table_td_style" style="width:10%;">12345678</td>
                                <td class="Table_td_style" style="width:10%;">馬英九</td>
                                <td class="Table_td_style" style="width:5%;">男</td>
                                <td class="Table_td_style" style="width:5%;">20</td>
                                <td class="Table_td_style" style="width:12%;">王小明/4276</td>
                                <td class="Table_td_style" style="width:35%;">shrhsrsthnsfsrthshbsrth</td>
                            <td class="Table_td_style" style="width:13%;">已離開</td>
-->
                        <asp:Label ID="IN_Label" runat="server" Text="Label"></asp:Label>
                    </table>
                </div>
                <hr />
                                
                <div class ="Table_Title_big_blue">出院名單</div>
                <div>
                    <table  class ="Table_style">
                          <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:5%;">床號</td>
                                <td class="Table_td_style" style="width:5%;">註記</td>
                                <td class="Table_td_style" style="width:10%;">病歷號碼</td>
                                <td class="Table_td_style" style="width:10%;">姓名</td>
                                <td class="Table_td_style" style="width:70%;">備註</td>
                        </tr>
 <!--                       <tr class ="Table_tr_style1">
                                 <td class="Table_td_style" style="width:5%;">11/22</td>
                                <td class="Table_td_style" style="width:5%;">出院</td>
                                <td class="Table_td_style" style="width:10%;">12345678</td>
                                <td class="Table_td_style" style="width:10%;">馬英九</td>
                                <td class="Table_td_style" style="width:70%;">男</td>
                        </tr>
                        -->
                        <asp:Label ID="OUTLabel" runat="server" Text="Label"></asp:Label>
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

    </form>
</body>
</html>
