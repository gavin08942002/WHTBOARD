<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TRM-DR.aspx.vb" Inherits="Web_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <script src="JavaScript/HeadScript.js"></script>
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <link href="Jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet" />
    <script src="Jquery-ui-1.12.1.custom/jquery-ui.js"></script>
        <link href="CSS/TRM.css" rel="stylesheet" />

   
         <script>
             $(function () {
                 $("#dialog").dialog({
                     buttons: {
                         '關閉': function() {
                             $( this ).dialog( "close" );
                         }
                     },
               //     modal: true,
                     width:800,
                    autoOpen: false,
                     show: {
                         effect: "blind",
                         duration: 500
                     },
                     hide: {
                         effect: "blind",
                         duration: 500
                     }
                 });
                 $("#opener").on("click",function () {
                     $("#dialog").dialog("open");
                 });

             });
  </script>

    <title>馬偕紀念醫院術前高風險評估</title>
</head>
<body style ="color: rgb(102,102,102); background-color: rgb(242,242,242)" >


    <form id="form1" runat="server" >
    <div id ="Body">

<div id="dialog" title="美國麻醉醫學會(ASA)生理狀態分類系統說明" style="width:500px;font-size:24px;">

    <table style="width: 700px" class="Table_style">
        <tr class="Table_tr_header_style">
            <td class="Table_td_style">ASA分類</td>
            <td class="Table_td_style">ASA分級描述</td>
        </tr>
        <tr class="Table_tr_style1">
            <td class="Table_td_style">P1<img alt="" src="image/Light.png" style="width:30px;height:30px;"/></td>
            <td class="Table_td_style">正常健康的人</td>
        </tr>
        <tr class="Table_tr_style1">
            <td class="Table_td_style">P2<img alt="" src="image/Light.png" style="width:30px;height:30px;"/></td>
            <td class="Table_td_style">具有輕度系統性疾病的人，不影響身體功能</td>
        </tr>
       <tr class="Table_tr_style1">
            <td class="Table_td_style">P3<img alt="" src="image/Heavy.png" style="width:30px;height:30px;"/></td>
            <td class="Table_td_style">具有重度系統性疾病的人，影響身體功能</td>
        </tr>
         <tr class="Table_tr_style1">
            <td class="Table_td_style">P4<img alt="" src="image/Heavy.png" style="width:30px;height:30px;"/></td>
            <td class="Table_td_style">具有重度系統性病病，其嚴重度足以威脅生命的人</td>
        </tr>
         <tr class="Table_tr_style1">
            <td class="Table_td_style">P5<img alt="" src="image/Heavy.png" style="width:30px;height:30px;"/></td>
            <td class="Table_td_style">有無接受手術都可能無法存活超過24小時的垂危病人</td>
        </tr>
         <tr class="Table_tr_style1">
            <td class="Table_td_style">P6<img alt="" src="image/Heavy.png" style="width:30px;height:30px;"/></td>
            <td class="Table_td_style">接受器官摘除以供移植的腦死病人</td>
        </tr>
    </table>
</div>
 



        <div id ="head">
            <!--馬偕LOGO及系統名稱--><div style ="width:1024px; height:60px;">
                <div style ="width:512px; height:50px; float:left; text-align:left;">
                    <img style="width:240px; height:50px; padding-left:20px;" alt src="image/mmhlogo.jpg" />
                </div>
                <div style ="width:512px; height:50px; float:left; text-align:right;">
                    <span class ="h3">麻醉高風險病人評估-麻醫回覆</span>
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
                                    <div style="width:300px; height:50px;float:left;line-height:50px;"> <span class ="h2">病歷號：<asp:Label ID="Label_pno" runat="server" Text="Label"></asp:Label></span>            </div>
                                    <div style="width:300px; height:50px;float:left;line-height:50px;">   <span class ="h2">姓名：<asp:Label ID="Label_name" runat="server" Text="Label"></asp:Label></span>          </div>
                           </div>
                           <div  style="width:512px; height:100px;float:left;">
                                    <div style="width:300px; height:50px;float:left;line-height:50px;"> <span class ="h2">醫師：<asp:Label ID="Label_Drcode" runat="server" Text="Label"></asp:Label>
                                        </span>            </div>
                                    <div style="width:300px; height:50px;float:left;line-height:50px;">   <span class ="h2">姓名：<asp:Label ID="Label_Drname" runat="server" Text="Label"></asp:Label>
                                        </span>          </div>
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

  <!--  開刀房選項    -->   
                
                <div class="widget" style="width:1024px; line-height:30px;">
                <fieldset class="Fieldset-blue">
                      <legend class=" legend-blue" ><span class =>病史</span></legend>
                                <div style=" width:1024px;height:30px;"></div>
                                <asp:CheckBox ID="A1" runat="server" Enabled="false"  />
                                <label for="A1">年齡超過70歲</label><br>
                                <asp:CheckBox ID="A2" runat="server" Enabled="False" />
                                <label for="A2">罹患糖尿病或高血壓超過10年以上</label><br>
                                <asp:CheckBox ID="A3" runat="server" Enabled="False" />
                                <label for="A3">心臟疾病(心股梗塞、心律不整、心臟瓣膜、先天性心臟病)</label><br>
                                <asp:CheckBox ID="A4" runat="server" Enabled="False" />
                                <label for="A4">洗腎</label><br>
                                <asp:CheckBox ID="A5" runat="server" Enabled="False" />
                                <label for="A5">肝硬化</label><br>
                                <asp:CheckBox ID="A6" runat="server" Enabled="False" />
                                <label for="A6">近六個月內中風</label><br>
                                <asp:CheckBox ID="A7" runat="server" Enabled="False" />  
                                <label for="A7">生命徵象不穩或目前住加護病房</label><br>
                                <asp:CheckBox ID="A8" runat="server" Enabled="False" />      
                                <label for="A8">慢性阻塞性肺病或氣喘發作</label>
                                
              </fieldset>
    </div>
                <hr />
                <div class="widget" style="width:1024px; line-height:30px;">
                 <fieldset class="Fieldset-blue">
                      <legend class=" legend-blue" >術式</legend>
                                <div style=" width:1024px;height:30px;"></div>
                                <asp:CheckBox ID="B1" runat="server" Enabled="False" />
                                <label for="B1">心臟手術</label><br>
                               <asp:CheckBox ID="B2" runat="server" Enabled="False" />
                                <label for="B2">脊椎手術</label><br>
                                <asp:CheckBox ID="B3" runat="server" Enabled="False" />
                                <label for="B3">腦部手術</label><br>
                                <asp:CheckBox ID="B4" runat="server" Enabled="False" />
                                <label for="B4">胸腔手術</label><br>
                                <asp:CheckBox ID="B5" runat="server" Enabled="False" />
                                <label for="B5">肝或胃切除手術</label><br>
                                <asp:CheckBox ID="B6" runat="server" Enabled="False" />
                                <label for="B6">其它</label>
                                                     <asp:TextBox ID="B6C" runat="server" CssClass="TextBox10" Enabled="False"></asp:TextBox>
                                
                                <br>
              </fieldset>
                </div>     
                <hr>      
<div style="width:1024px; line-height:30px;">
                <fieldset class="Fieldset-green">
                      <legend class=" legend-green" ><span >一、依美國麻醉醫學會(ASA)生理狀態</span></legend>
                                    <div style=" width:1024px;height:30px;"></div>
                    <asp:DropDownList CssClass="select-1" ID="DropDownList1" runat="server" AppendDataBoundItems="True">
                      
                        <asp:ListItem Value="P1">P1</asp:ListItem>
                        <asp:ListItem Value="P2">P2</asp:ListItem>
                        <asp:ListItem Value="P3">P3</asp:ListItem>
                        <asp:ListItem Value ="P4">P4</asp:ListItem>
                        <asp:ListItem Value =" P5">P5</asp:ListItem>
                        <asp:ListItem Value =" p6">P6</asp:ListItem>
                      </asp:DropDownList>
                    <button id="opener" type="button"><span class="h1">說明</span></button>

              </fieldset>
    </div>

                <hr />
                <!--二、麻醫回覆-->
                <div  style="width:1024px; line-height:30px;">
                          <fieldset class="Fieldset-green">
                                        <legend class=" legend-green" >二、麻醫回覆</legend>
                                <div style=" width:1024px;height:30px;"></div>
                              <asp:TextBox  ID="DR_TextArea" runat="server" Columns="50" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                 
                         </fieldset>
                </div>
                <hr />
                <!--三、麻醉科醫師建議-->
                                <div class="widget"  style="width:1024px; line-height:30px;">
                                    
                          <fieldset class="Fieldset-green">
                                        <legend class=" legend-green" >三、麻醉科醫師建議</legend>
                                <div style=" width:1024px;height:30px;"></div>
                                    1.請會診：
                                <asp:CheckBox ID="C1" runat="server" />
                                <label for="C1">心臟內科</label>
                              <asp:CheckBox ID="C2" runat="server" />
                                <label for="C2">心臟外科</label>
                              <asp:CheckBox ID="C3" runat="server" />
                                <label for="C3">胸腔內科</label>
                              <asp:CheckBox ID="C4" runat="server" />
                                <label for="C4">胸腔外科</label>
                              <asp:CheckBox ID="C5" runat="server" />
                                <label for="C5">其他</label><br>
                                2.檢驗檢查：
                               <asp:CheckBox ID="D1" runat="server" /> 
                                <label for="D1">心臟超音波</label>
                              <asp:CheckBox ID="D2" runat="server" />
                                <label for="D2">PFT</label>
                              <asp:CheckBox ID="D3" runat="server" />
                                <label for="D3">其他</label><br>
                                3.術後轉加護病房：
                              <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="2">
                                  <asp:ListItem Value="Y">是</asp:ListItem>
                                  <asp:ListItem Value="N">否</asp:ListItem>
                                        </asp:RadioButtonList>
                         </fieldset>

                </div>
                <hr />

                                <!--四、備註-->
                <div  style="width:1024px; line-height:30px;">
                    
                          <fieldset class="Fieldset-green">
                                        <legend class=" legend-green" >四、備註</legend>
                               <div style=" width:1024px;height:30px;"></div>
                                    <asp:TextBox  ID="Remark_TextArea" runat="server" Columns="50" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    
                         </fieldset>
                </div>
                <hr />
                        <!--麻醉回覆-->
                        <fieldset class="Fieldset-green">
                                <legend class=" legend-green" >術前備物</legend>
                                <div style=" width:1024px;height:30px;"></div>
                                    術前備物：
                                <asp:CheckBox ID="G1" runat="server" />
                                <label for="G1">A-line</label>
                               <asp:CheckBox ID="G2" runat="server" />
                                <label for="G2">CVP</label>
                               <asp:CheckBox ID="G3" runat="server" />
                                <label for="G3">輸血line</label>
                               <asp:CheckBox ID="G4" runat="server" />
                               <label for="G4">其它</label>
                            <asp:TextBox ID="G4C" CssClass="TextBox10" runat="server"></asp:TextBox>
                         </fieldset>
                </div>
                <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
            <asp:GridView ID="GridView2" runat="server">
            </asp:GridView>
            <asp:GridView ID="GridView3" runat="server">
            </asp:GridView>
            <asp:Button CssClass="Button1"  ID="Button1" runat="server" Text="送出 "  />

            </div>
               
    </div>

    </form>
</body>
</html>
