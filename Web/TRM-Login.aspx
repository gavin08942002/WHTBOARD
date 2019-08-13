<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TRM-Login.aspx.vb" Inherits="Web_Default" %>

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
         //    $(function () {
          //       $("input").checkboxradio();
         //    });
  </script>

    <title>馬偕紀念醫院術前高風險評估</title>
</head>
<body style ="color: rgb(102,102,102); background-color: rgb(242,242,242)" >


    <form id="form1" runat="server"  >
    <div id ="Body">
        <div id ="head">
            <!--馬偕LOGO及系統名稱--><div style ="width:1024px; height:60px;">
                <div style ="width:512px; height:50px; float:left; text-align:left;">
                    <img style="width:240px; height:50px; padding-left:20px;" alt src="image/mmhlogo.jpg" />
                </div>
                <div style ="width:512px; height:50px; float:left; text-align:right;">
                    <span class ="h3">麻醉高風險及交班系統登入</span>
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

            </div>
            <!--留空-->
            <div>
                        <div style="width:300px;height:30px">
                                <div style="width:150px;height:30px;float:left">
                                            <label for="PSN">員工代碼：</label>
                                </div>
                                <div style="width:150px;height:30px;float:left">
                                            <input id="PSN" type="text" /><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </div>
                         </div>
                         <div style="width:300px;height:30px">
                                <div style="width:150px;height:30px;float:left">
                                            <label for="PPASSWORD">密碼：      </label>
                                </div>
                                <div style="width:150px;height:30px;float:left">
                                           <input id="PASSWORD" type="password" />
                                </div>
                             </div>
            </div>
        </div>
        
        <div id ="main">
            <!--網頁標題-->


            <!--資料內容-->
            <div id ="content">
                                <!--時間資料和按鈕-->
                <div class ="info" imageurl="image/time_day_b.png">
                    <div style="width: 33%; height: 100%;float:left;" ></div>
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

                <hr />

                <input id="Submit1" type="submit" value="送出" class="Button1" />
            </div>
        </div>
        
    </div>

    </form>
</body>
</html>
