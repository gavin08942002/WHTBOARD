<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TRM-DL.aspx.vb" Inherits="Web_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="JavaScript/HeadScript.js"></script>


    <link href="CSS/TRM.css" rel="stylesheet" />
  <!--
    
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="JavaScript/bootstrap.min.js"></script>
    <script src="JavaScript/jquery-ui-1.12.1.custom/jquery-ui.js"></script>
    
    


  
<script src="JavaScript/jquery.url.js"></script
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />

    <script src="JavaScript/datepicker-zh-TW.js"></script>
-->
<script src="JavaScript/HeadScript.js"></script>
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="Jquery-ui-1.12.1.custom/jquery-ui.js"></script>
    <link href="JavaScript/jquery-ui-1.12.1.custom/jquery-ui.structure.css" rel="stylesheet" />
    <link href="Jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet" />
   <link href="JavaScript/jquery-ui-1.12.1.custom/jquery-ui.theme.css" rel="stylesheet" />
       <script src="JavaScript/datepicker-zh-TW.js"></script>

         <script>
             $(function () {
                 //$("#datepicker").datepicker({
                 //    minDate: "-0d",
                 //});
                 $("#datepickerB").datepicker({
                 });
                 $("#datepickerE").datepicker({
                 });
         });
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
                    <span class ="h3">術前麻醉高風險病人評估</span>
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
                    <span class ="h1">
                        操作者：<asp:Label ID="User_Label" runat="server" Text="Label"></asp:Label>
                    </span>  
                </div>
            </div>
            <!--留空-->
            <!--病人資料-->
            <div style ="width:1024px; height:100px;border-radius:10px;border-width:2px;border-color:green;border-style:solid;">
                           <div  style="width:300px; height:100px;float:left;">
                                    <div style="width:300px; height:50px;float:left;line-height:50px;"> <span class ="h2">日期：</span>         
                                        
                                        <asp:TextBox ID="datepickerB" runat="server" Width="80px" ></asp:TextBox>
                                    ~        
                                        <asp:TextBox ID="datepickerE" runat="server" Width="80px" ></asp:TextBox>
                                        
                                   </div>
                                    <div style="width:300px; height:50px;float:left;line-height:50px;">   <span class ="h2">科別：</span>
                                        <asp:DropDownList ID="DropDownList_dept" runat="server" Height="36px" Width="150px" AppendDataBoundItems="True" Font-Size="16pt" >
                                        <asp:ListItem Value="0">請選擇</asp:ListItem>
                                        </asp:DropDownList>
                                        </div>
                           </div>
                           <div  style="width:300px; height:100px;float:left;">
                                    <div style="width:300px; height:50px;float:left;line-height:50px;"> <span class ="h2">醫師：</span>
                                        <asp:DropDownList ID="DropDownList_DR" runat="server" Height="36px" Width="150px" AppendDataBoundItems="True" Font-Size="16pt" >                           
                                        <asp:ListItem Value="0">請選擇</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="width:300px; height:50px;float:left;line-height:50px;">   <span class ="h2">回覆狀況：</span>
                                    <asp:DropDownList ID="DropDownList_Re" runat="server" AppendDataBoundItems="True" Font-Size="16pt" Height="36px" Width="150px">
                                        <asp:ListItem Value="0">請選擇</asp:ListItem>
                                        <asp:ListItem Value="Y">已回覆</asp:ListItem>
                                        <asp:ListItem Value="N">未回覆</asp:ListItem>
                                    </asp:DropDownList>
                                    </div>
                           </div>
                           <div  style="width:300px; height:100px;float:left;">
                                    <div style="width:300px; height:50px;float:left;line-height:50px;">
                                        <asp:CheckBox ID="CheckBoxHandOver" runat="server" AutoPostBack="True" />
                                        <label for="CheckBoxHandOver"><span class ="h2" >交班</span></label>

                                        <asp:CheckBox ID="CheckBoxLeave" runat="server" AutoPostBack="True" />
                                        <label for="CheckBoxLeave"><span class ="h2" >顯示已離開</span></label>
                                    </div>
                                    <div style="width:300px; height:50px;float:left;line-height:50px;"></div>
                           </div>

                           <asp:Button CssClass="ButtonSerch" ID="ButtonSerch" runat="server" Text="查詢" type="button" />

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
                                <div class ="Table_Title_big_blue">手術病人列表</div>
                <div id ="surgery" style="width:1024px;overflow-y:auto;">
                    <table   class ="Table_style">
                          <tr class ="Table_tr_header_style">
                                <td class="Table_td_style" style="width:4%;">日期</td>
                               <td class="Table_td_style" style="width:4%;">回覆</td>
                               <td class="Table_td_style" style="width:4%;">高</td>
                              <td class="Table_td_style" style="width:4%;">檢驗</td>
                                <td class="Table_td_style" style="width:8%;">床號</td>
                                <td class="Table_td_style" style="width:8%;">病歷號碼</td>
                                <td class="Table_td_style" style="width:7%;">姓名</td>
                                <td class="Table_td_style" style="width:2%;">年齡</td>
                                <td class="Table_td_style" style="width:45%;">術式</td>
                                <td class="Table_td_style" style="width:7%;">醫師</td>
                                <td class="Table_td_style" style="width:7%;">狀態</td>
                        </tr>
 <!--           
                        <tr class ="Table_tr_style1">
                                 <td class="Table_td_style">11/22</td>
                                <td class="Table_td_style"><button style="width:50px;height:40px; font-size:16px;background-color:#95cfcf;"  type ="button" onclick="javascript:location.href='TRM-DR.aspx'">回覆</button></td>
                                <td class="Table_td_style"><img alt="" src="image/Heavy.png" style="width:30px;height:30px;"/></td>
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
                
                <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                <hr />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
