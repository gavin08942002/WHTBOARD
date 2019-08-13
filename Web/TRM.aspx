<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TRM.aspx.vb" Inherits="Web_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="JavaScript/HeadScript.js"></script>


    <link href="CSS/TRM.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="Jquery-ui-1.12.1.custom/jquery-ui.js"></script>
    <link href="Jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet" />
         <script>

  </script>

    <title>馬偕紀念醫院術前高風險評估</title>
</head>
<body style ="color: rgb(102,102,102); background-color: rgb(242,242,242)" >


    <form id="form1" runat="server" enableviewstate="False"  >
    <div id ="Body">
        <div id ="head">
            <!--馬偕LOGO及系統名稱--><div style ="width:1024px; height:60px;">
                <div style ="width:512px; height:50px; float:left; text-align:left;">
                    <img style="width:240px; height:50px; padding-left:20px;" alt src="image/mmhlogo.jpg" />
                </div>
                <div style ="width:512px; height:50px; float:left; text-align:right;">
                    <span class ="h3">麻醉高風險病人評估表</span>
                </div>
            </div>
            <!--操作時間,   操作者-->
            <div class ="info">
                <div style ="width:25%; height:50px; float:left; line-height:50px;">
                </div>
                <div style ="width:41%; height:50px; float:left; text-align:center; line-height:50px">
                                 <div id="Time">
                              <script>/*時間顯示*/
                                  document.getElementById('Time').innerHTML = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                                  setInterval("document.getElementById('Time').innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());var time = new Date();  ", 1000);
                             </script>
                         </div>
                </div>
                <div style ="width:33%; height:50px; float:left;text-align:right; line-height:50px;">
                    <asp:Label ID="Label_Empno" runat="server" Text="Label" Visible="False"></asp:Label>
                    <span class ="h1">
                        操作者：<asp:Label ID="Label_User" runat="server" Text="Label"></asp:Label>
                    </span>  
                </div>
            </div>
            <!--留空-->
            <!--病人資料-->
            <div style ="width:1024px; height:100px;border-radius:10px;border-width:2px;border-color:green;border-style:solid;">
                           <div  style="width:512px; height:100px;float:left;">
                                    <div style="width:256px; height:50px;float:left;line-height:50px;"> <span class ="h2">病歷號：<asp:Label ID="Label_pno" runat="server" Text="Label"></asp:Label></span> 
                                    </div>
                                    <div style="width:256px; height:50px;float:left;line-height:50px;">   <span class ="h2">姓名：<asp:Label ID="Label_name" runat="server" Text="Label"></asp:Label></span>          
                                    </div>
                                    <div style="width:256px; height:50px;float:left;line-height:50px;"> <span class ="h2">醫師：<asp:Label ID="Label_Drcode" runat="server" Text="Label"></asp:Label>
                                        </span>            </div>
                                    <div style="width:256px; height:50px;float:left;line-height:50px;">   <span class ="h2">姓名：<asp:Label ID="Label_Drname" runat="server" Text="Label"></asp:Label>
                                        </span>          </div>
                               
                                 </div>
                           <div  style="width:512px; height:100px;float:left;">
                                     <div style="width:256px; height:50px;float:left;line-height:50px;"> <span class ="h2">排程日期：<asp:Label ID="Label_dt_opdt" runat="server" Text=""></asp:Label>
                                        </span>            </div>
                           </div>

            </div>
        </div>
        
        <div id ="main">
            <!--網頁標題-->


            <!--資料內容-->
            <div id ="content">
                                <!--時間資料和按鈕-->
                <div class ="info" imageurl="image/time_day_b.png">
                    <div style="width: 33%; height: 100%;float:left;" >
                        <asp:Label ID="Label_Bedno" runat="server" Text="Label" Visible="False"></asp:Label>
                        <asp:Label ID="Label_opdt" runat="server" Text="Label" Visible="False"></asp:Label>
                        <asp:Label ID="Label_caseno" runat="server" Text="Label" Visible="False"></asp:Label>
                        <asp:Label ID="Label_hospcode" runat="server" Text="Label" Visible="False"></asp:Label>
                    </div>
                    <div style="width: 33%; height: 100%;float:left;" >
                        <div style =" width:20%; height:100%; float:left; line-height:50px; text-align:right;" >
                        </div>
                        <div style =" width:60%; height:100%; float:left; line-height:50px; text-align:center;" class ="big-red">
                        </div>
                        
                    </div>
                    <div style="width: 33%; height: 100%;float:left;" >
                                <div style =" width:33%; height:100%; float:left; line-height:50px; text-align:center;">
                                </div>
                                <div style =" width:33%; height:100%; float:left; line-height:50px; text-align:center;">
                                </div>
                                <div style =" width:33%; height:100%; float:left; line-height:50px; text-align:center;">
                                </div>
                    </div>
                </div>

                
<div class="widget" style="width:1024px; line-height:30px;">
                <fieldset class="Fieldset-green">
                      <legend class=" legend-green" ><span class =>病史</span></legend>
                                <div style=" width:1024px;height:30px;"></div>
                                <asp:CheckBox ID="A1" runat="server" Enabled="True" Checked="False"  />
                                <label for="A1">年齡超過70歲</label><br>
                                <asp:CheckBox ID="A2" runat="server" />
                                <label for="A2">罹患糖尿病或高血壓超過10年以上</label><br>
                                <asp:CheckBox ID="A3" runat="server" />
                                <label for="A3">心臟疾病(心股梗塞、心律不整、心臟瓣膜、先天性心臟病)</label><br>
                                <asp:CheckBox ID="A4" runat="server" />
                                <label for="A4">洗腎</label><br>
                                <asp:CheckBox ID="A5" runat="server" />
                                <label for="A5">肝硬化</label><br>
                                <asp:CheckBox ID="A6" runat="server" />
                                <label for="A6">近六個月內中風</label><br>
                                <asp:CheckBox ID="A7" runat="server" />  
                                <label for="A7">生命徵象不穩或目前住加護病房</label><br>
                                <asp:CheckBox ID="A8" runat="server" />      
                                <label for="A8">慢性阻塞性肺病或氣喘發作</label>
                                
              </fieldset>
    </div>
                <hr />
                <div class="widget" style="width:1024px; line-height:30px;">
                 <fieldset class="Fieldset-green">
                      <legend class=" legend-green" >術式</legend>
                                <div style=" width:1024px;height:30px;"></div>
                                <asp:CheckBox ID="B1" runat="server" />
                                <label for="B1">心臟手術</label><br>
                               <asp:CheckBox ID="B2" runat="server" />
                                <label for="B2">脊椎手術</label><br>
                                <asp:CheckBox ID="B3" runat="server" />
                                <label for="B3">腦部手術</label><br>
                                <asp:CheckBox ID="B4" runat="server" />
                                <label for="B4">胸腔手術</label><br>
                                <asp:CheckBox ID="B5" runat="server" />
                                <label for="B5">肝或胃切除手術</label><br>
                                <asp:CheckBox ID="B6" runat="server" />
                                <label for="B6">其它</label>
                     <asp:TextBox ID="B6C" runat="server" CssClass="TextBox10"></asp:TextBox>
                                <br>
              </fieldset>
                </div>
                

                <hr />
                <asp:Button ID="Button1" runat="server" Text="送出"  CssClass ="Button1"/>

            </div>
        </div>
        
    </div>

        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>
        <asp:GridView ID="GridView3" runat="server">
        </asp:GridView>
    </form>
</body>
</html>
