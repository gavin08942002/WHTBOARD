<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sample.aspx.vb" Inherits="Web_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <link href="CSS/HeadStyle.css" rel="stylesheet" />
    <script src="JavaScript/HeadScript.js"></script>

    <title>馬偕紀念醫院住院病房護理查詢系統</title>
</head>
<body style ="color: rgb(102,102,102); background-color: rgb(242,242,242)" >
    <form id="form1" runat="server">
    <div id ="Body">
        <div id ="head">
            <!--馬偕LOGO及系統名稱--><div style ="width:1024px; height:60px;">
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
                                 <div id="Time">
                              <script>/*時間顯示*/
                                  document.getElementById('Time').innerHTML = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                                  setInterval("document.getElementById('Time').innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());var time = new Date();   if ( time.getMinutes()%10 == 0 && time.getSeconds() == 5) myrefresh();", 1000);
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
            <div style ="width:1024px; height:60px;">
                <div style="width:880px; height:60px; margin:0px auto;">
                    <div class ="Topic_Image" >
                 
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink3" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b3.png" onmouseover="over(this, 'image/menu_r3.png')" onmouseout="out(this, 'image/menu_b3.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >
                        <asp:HyperLink ID="HyperLink5" runat="server"> 
                            <img class ="Topic_image_file" alt src="image/menu_b5.png" onmouseover="over(this, 'image/menu_r5.png')" onmouseout="out(this, 'image/menu_b5.png')"  />
                        </asp:HyperLink>
                    </div>
                    <div class ="Topic_Image" >

                    </div>
                    <div class ="Topic_Image" >

                    </div>
                    <div class ="Topic_Image" >

                    </div>
                    <div class ="Topic_Image" >

                    </div>
                    <div class ="Topic_Image" >

                    </div>
                </div>
            </div>
        </div>
        
        <div id ="main">
            <!--網頁標題-->
            <div id ="topic"><span class ="big-red">護理排班</span></div>
            <hr />
            <!--資料內容-->
            <div id ="content">
                                <!--時間資料和按鈕-->
                <div class ="info" imageurl="image/time_day_b.png">
                    <div style="width: 33%; height: 100%;float:left;" ></div>
                    <div style="width: 33%; height: 100%;float:left;" >
                        <div style =" width:20%; height:100%; float:left; line-height:50px; text-align:right;" >
                            <asp:ImageButton ID="Backward_ImageButton" runat="server" ImageUrl="~/Web/image/date_backward.png" Width="30px" Height="30px" ImageAlign="AbsMiddle" />
                        </div>
                        <div style =" width:60%; height:100%; float:left; line-height:50px; text-align:center;" class ="big-red">
                            <asp:Label ID="Date_Label" runat="server" Text="2015/04/02"></asp:Label>
                        </div>
                        <div style =" width:20%; height:100%; float:left; line-height:50px" >
                            <asp:ImageButton ID="Forward_ImageButton" runat="server" ImageUrl="~/Web/image/date_foward.png" Width="30px" Height="30px" ImageAlign="AbsMiddle" />
                        </div>
                        
                    </div>
                    <div style="width: 33%; height: 100%;float:left;" >
                                <div style =" width:33%; height:100%; float:left; line-height:50px; text-align:center;">
                                        <asp:HyperLink ID="Day_HyperLink" runat="server" >
                                                     <img style ="vertical-align:middle;  width:100px; height:32px" src="image/time_day_b.png" onmouseover="over(this,'image/time_day_r.png')"  onmouseout ="out(this, 'image/time_day_b.png')"/>
                                        </asp:HyperLink>
                                </div>
                                <div style =" width:33%; height:100%; float:left; line-height:50px; text-align:center;">
                                        <asp:HyperLink ID="Everning_HyperLink" runat="server">
                                                     <img style ="vertical-align:middle;  width:100px; height:32px" src="image/time_evering_b.png" onmouseover="over(this,'image/time_evering_r.png')"  onmouseout ="out(this, 'image/time_evering_b.png')"/>
                                        </asp:HyperLink>
                                </div>
                                <div style =" width:33%; height:100%; float:left; line-height:50px; text-align:center;">
                                        <asp:HyperLink ID="Night_HyperLink" runat="server">
                                                    <img style ="vertical-align:middle;  width:100px; height:32px" src="image/time_night_b.png" onmouseover="over(this,'image/time_night_r.png')"  onmouseout ="out(this, 'image/time_night_b.png')"/>
                                        </asp:HyperLink>
                                </div>
                    </div>
                </div>
                 <!--依護理師-->
                <div class ="Table_Title_big_blue">白班-依護理師</div>
                <div>
                    <table  class ="Table_style">
                          <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:7%;">班別</td>
                                <td class="Table_td_style" style="width:10%;">護理師</td>
                                <td class="Table_td_style" style="width:10%;">聯絡電話</td>
                                <td class="Table_td_style" style="width:53%;">專責床位</td>
                                <td class="Table_td_style" style="width:5%;">床數</td>
                                <td class="Table_td_style" style="width:10%;">消防</td>
                                <td class="Table_td_style" style="width:5%;">代理</td>
                        </tr>
                        <tr class ="Table_tr_style1">
                                 <td class="Table_td_style" >A</td>
                                <td class="Table_td_style" >王品淳</td>
                                <td class="Table_td_style" >7210</td>
                                <td class="Table_td_style" >11B/22A/22C</td>
                                <td class="Table_td_style" >10</td>
                                <td class="Table_td_style" >通報</td>
                                <td class="Table_td_style" >C</td>
                        </tr>
                        <tr class ="Table_tr_style2">
                                 <td class="Table_td_style" ">B</td>
                                <td class="Table_td_style" >江芳晰</td>
                                <td class="Table_td_style" >7220</td>
                                <td class="Table_td_style" >12A/12C/13B</td>
                                <td class="Table_td_style" >6</td>
                                <td class="Table_td_style" >引導</td>
                                <td class="Table_td_style" >H</td>
                        </tr>
                    </table>
                </div>
                <hr />
                <!--依病床號-->
                <div class ="Table_Title_big_blue">白班-依病床</div>
                <div>
                    <table  class ="Table_style">
                          <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:25%;">床號/護理師/連絡電話</td>
                                <td class="Table_td_style" style="width:25%;">床號/護理師/連絡電話</td>
                                <td class="Table_td_style" style="width:25%;">床號/護理師/連絡電話</td>
                                <td class="Table_td_style" style="width:25%;">床號/護理師/連絡電話</td>
                        </tr>
                        <tr class ="Table_tr_style1">
                                 <td class="Table_td_style" >11A 王品淳 / 7210</td>
                                <td class="Table_td_style" >11B 王品淳 / 7210</td>
                                <td class="Table_td_style" >11B 王品淳 / 7210</td>
                                <td class="Table_td_style" ></td>
                        </tr>
                        <tr class ="Table_tr_style2">
                                 <td class="Table_td_style" ">12A 江芳晰 / 7211</td>
                                <td class="Table_td_style" >12B 江芳晰 / 7211</td>
                                <td class="Table_td_style" >12B 江芳晰 / 7211</td>
                                <td class="Table_td_style" ></td>
                        </tr>
                    </table>
                </div>
                <hr />
            </div>
        </div>
        
    </div>

    </form>
</body>
</html>
