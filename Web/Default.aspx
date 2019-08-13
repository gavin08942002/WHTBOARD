<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Web_Default" %>

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
            <div id ="topic"><span class ="big-red">病人資訊</span></div>
            <hr />
            <!--資料內容-->
            <div id ="content"></div>
        </div>
        
    </div>

    </form>
</body>
</html>
