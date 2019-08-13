<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Patients.aspx.vb" Inherits="Web_Default" %>

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
            <div id ="topic"><span class ="big-red">病人資訊</span></div>
            <hr />
            <!--資料內容-->
            <div id ="content">
                <!--統計資料-->
                <div  id="statistic" >
                    <div >
                        <div class ="class_statistic_1 big" >輕</div>
                        <div class ="class_statistic_2 big " style ="background-color: rgb(155, 187, 89);color:black;" >28</div>
                    </div>

                    <div >
                        <div class ="class_statistic_1 big" >中</div>
                        <div class ="class_statistic_2 big " style ="background-color: yellow;color:black;" >28</div>
                    </div>
                    <div >
                        <div class ="class_statistic_1 big" >重</div>
                        <div class ="class_statistic_2 big "  style ="background-color: rgb(255, 51, 51);color:black;">28</div>
                    </div>
                    <div >
                        <div class ="class_statistic_1 big" >空床</div>
                        <div class ="class_statistic_2 big "  style ="background-color: silver;color:black;">28</div>
                    </div>
                    <div >
                        <div class ="class_statistic_1 big" >全部</div>
                        <div class ="class_statistic_2 big "  style ="background-color: black;color:white">28</div>
                    </div>
                    <div  style ="width:230px;">
                        <div style ="width:130px;height:70%; float:left; line-height:50px; text-align:right;" class="big" >佔床率：</div>
                        <div style="width:100px; height:70%; float:left; line-height:50px" class ="big "  >28</div>
                    </div>
                </div>
               <!--病床資料大外框-->
                 <div id ="Patient_Bed">
                     <div style ="width:100%; height:50px;"></div>


                     <div class ="Bed">
                         <!--病人資訊表頭-->
                         <div class ="Bed_Head">
                             <div style =" width:20%; height:40px; float:left; font-size:28px; color:black;">11A</div>
                             <div style =" width:10%; height:40px; float:left; font-size:28px; color:black;"></div>
                             <div style =" width:70%; height:40px; float:left; font-size:28px; text-align:left; padding-top:5px;">
                                 <!--性別圖示-->
                                 <img src="image/icon_sex_f.png" height="30px" width="30px" />
                             <!--行動能力圖示-->
                                 <img src="image/icon_move_1.png" width="30px" height="30px" />
                             </div>
                         </div>
                         <!--病人資訊內容-->
                          <div class ="Bed_Body">
                              <div style ="width:250px; height:40px; ">
                                  <!--病人姓名-->
                                  <div class ="Bed_Body_Div1">姓名：</div>
                                  <div class ="Bed_Body_Div2">白帶魚</div>
                              </div>
                              <!--主治-->
                              <div style ="width:250px; height:80px;">
                                  <div class ="Bed_Body_Div1">主治：</div>
                                  <div class ="Bed_Body_Div2">尼莫/大法師/德魯伊/孫悟空</div>
                              </div>
                              <!--護理師-->
                              <div style ="width:250px; height:40px;">
                                  <div class ="Bed_Body_Div1">護理師：</div>
                                  <div class ="Bed_Body_Div2">白帶魚</div>
                              </div>
                          </div>
                         <!--病人資訊標尾圖示-->
                         <div class ="Bed_footer">
                             <img src="image/icon_rem_1.png" width="30px" height="30px" />
                             <img src="image/icon_rem_2.png" width="30px" height="30px" />
                             <img src="image/icon_rem_r.png" width="30px" height="30px" />
                             <img src="image/icon_rem_st.png" width="30px" height="30px" />
                         </div>

                     </div>
                     <!--空床位顯示-->
                     <div class ="Bed">
                         <div class ="Bed_Head" style="background-color:silver;">
                             <div style =" width:20%; height:40px; float:left; font-size:28px; color:black;">11A</div>
                        </div>
                         <div class ="Bed_Body" style ="font-size:48px; text-align:center;  line-height:160px;">空床位</div>
                         <div class ="Bed_footer"></div>

                     </div>
                     <asp:Label ID="PATLabel" runat="server" Text="Label"></asp:Label>
                </div>
            </div>
        </div>
    </div>
        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>
    </form>
</body>
</html>
