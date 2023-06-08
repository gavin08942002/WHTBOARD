<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Station-PU.aspx.vb" Inherits="Station" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1"/>
<meta http-equiv="refresh" content="600"/>
    <title></title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/html5shiv.min.js"></script>
    <script src="Scripts/respond.js"></script>
    <script language="JavaScript">
        function myrefresh() {
            window.location.reload();
        }
        setTimeout('myrefresh()', 660000); //指定11分鐘刷新一次
    </script>
<style type="text/css">

#Header{
    width:1860px;
    height:60px;
    text-align:center;
    /*line-height:80px; */
    font-size:32px;
    color:#fffaf3;
    font-weight:bold;
    background-color:#000;
    margin:0px auto;
}
#Head_inside {
	width:1860px;
	height:50px;
	background-color:#000;
	margin: 0px auto;
}
.Header_adjustment_right {/*調整header內時間和樓名的對齊方式*/
    text-align:right
}
.Header_adjustment_left {/*調整header內時間和樓名的對齊方式*/
    text-align:left
}
#Body{
    width:1860px;
    height:900px;
    background-color:#000;
    margin:0px auto;
}
#Body1{
    width:600px;
    height:900px;
    background-color:#000;
    vertical-align: top;
	float:left;
    color:#ffffff
}
#Body1-1 {
    width:600px;
    height:500px;
    background-color:#000;
    vertical-align: top;
	color:#ffffff
}
#Body1-2 {
    width:600px;
    height:400px;
    background-color:#000;
    vertical-align: top;
	color:#77db88;
    border-top-style:solid;
    border-top-color:#FAF0E6;
    border-top-width:2px;
    }
#Body2{
    width:1000px;
    height:900px;
    background-color:#fffaf3;
    float:left;
    
        
}
#Body3{
    width:260px;
    height:900px;
    background-color:#000;
	float:left;
    color:#ffffff;
}
#Footer{
    width:1860px;
    height:100px;
    text-align:center;
    color:#000000;
    font-weight:bold;
    background-color:#000;
	margin:0px auto;
    
}
.Nurse {/*護士所用資料大框架的格式*/
	
	background-color: #000;
	height: 100px;
	width: 600px;
    text-align:left;
    border-bottom-style:solid;
    border-bottom-color:#FAF0E6;
    border-bottom-width:2px;

    
}
.row_adjust {/*調整.row裡面的參數*/
    width:360px;
    height:25px;
    text-align:left;
    margin-right:0px;
    margin-left:0px;
}
.Nurse_ward {/*護士所顧病床的表格顯示*/
    left:0px;
    top:0px;
    border-color:#1C42D8;
    color:#333;
    font-size:35px;
    font-weight:bold;
    height:33px;
    padding-top:-3px;
    text-align:center;
    border-style:groove;
    border-width:1px;
    border-radius:10px;
    float:left;
    width:20%;
    line-height:30px;
    background-color:#eece11;
    font-family:Arial;
}
.Nurse_ward_2 {/*護士所顧病床的表格顯示*/
    left:0px;
    top:0px;
    border-color:#D0EC27;
    color:#FAF0E6;
    font-size:25px;
    font-weight:bold;
    height:25px;
    padding-top:-3px;
    text-align:center;
    border-style:groove;
    border-width:1px;
    border-radius:10px;
    float:left;
    width:16%;
    line-height:25px;
}
.New_nurse_ward {/*New Patient 病床格式*/
    left:0px;
    top:0px;
    border-color:#0094ff;
    color:#FAF0E6;
    font-size:35px;
    font-weight:bold;
    height:33px;
    padding-top:-3px;
    text-align:center;
    border-style:groove;
    border-width:1px;
    border-radius:10px;
    background-color:#0094ff;
    float:left;
    width:20%;
    line-height:30px;
    font-family: Arial;
    }
    .test {
    
    left:0px;
    top:0px;
    border-color:#0094ff;
    color:#FAF0E6;
    font-size:25px;
    font-weight:bold;
    height:25px;
    padding-top:-3px;
    text-align:center;
    border-style:groove;
    border-width:1px;
    border-radius:10px;
    background-color:#0094ff;
    float:left;
    width:16%;
    line-height:25px;
    }

.doctor_ward {/*醫生所顧病床button 顯示*/
    width:60px; 
    height:39px; 
    float:left; 
    line-height:35px; 
    text-align:center;
    font-size:30px;
    font-weight:bold;
    border-color:#98890c;
    border-style:solid;
    border-radius:15px;
    background-color:#eece11;
    border-width:1px;
    font-family:Arial;
}
    .new_doctor_ward {
    width:60px; 
    height:39px; 
    float:left; 
    line-height:35px; 
    text-align:center;
    font-size:28px;
    font-weight:bold;
    color:#ffffff;
    border-color:#98890c;
    border-style:solid;
    border-radius:15px;
    background-color:#0094ff;
    border-width:1px;
    font-family:Arial;
    
    }
    .Doctor_title {
    background-color: #AE7C3F;
    float: left;
    font-size: 24px;
    font-weight: bold;
    color: #FFF;
    border-radius: 10px;
    vertical-align: middle;
    line-height: 50px;
}
    .Numeric-font {
    font-family:Arial;
    font-weight:700;
    
    }
</style>
<link href="Content/Patient.css" rel="stylesheet" type="text/css"/></head>
    <script type="text/javascript">var _jf = _jf || []; _jf.push(['p', '32786']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'css', '.sourcehansans-tc-bold']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'alias', 'sourcehansans-tc-bold']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'weight', 700]); (function (f, q, c, h, e, i, r, d) { var k = f._jf; if (k.constructor === Object) { return } var l, t = q.getElementsByTagName("html")[0], a = function (u) { for (var v in k) { if (k[v][0] == u) { if (false === k[v][1].call(k)) { break } } } }, j = /\S+/g, o = /[\t\r\n\f]/g, b = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, g = "".trim, s = g && !g.call("\uFEFF\xA0") ? function (u) { return u == null ? "" : g.call(u) } : function (u) { return u == null ? "" : (u + "").replace(b, "") }, m = function (y) { var w, z, v, u, x = typeof y === "string" && y; if (x) { w = (y || "").match(j) || []; z = t[c] ? (" " + t[c] + " ").replace(o, " ") : " "; if (z) { u = 0; while ((v = w[u++])) { if (z.indexOf(" " + v + " ") < 0) { z += v + " " } } t[c] = s(z) } } }, p = function (y) { var w, z, v, u, x = arguments.length === 0 || typeof y === "string" && y; if (x) { w = (y || "").match(j) || []; z = t[c] ? (" " + t[c] + " ").replace(o, " ") : ""; if (z) { u = 0; while ((v = w[u++])) { while (z.indexOf(" " + v + " ") >= 0) { z = z.replace(" " + v + " ", " ") } } t[c] = y ? s(z) : "" } } }, n; k.push(["_eventActived", function () { p(h); m(e) }]); k.push(["_eventInactived", function () { p(h); m(i) }]); k.addScript = n = function (u, A, w, C, E, B) { E = E || function () { }; B = B || function () { }; var x = q.createElement("script"), z = q.getElementsByTagName("script")[0], v, y = false, D = function () { x.src = ""; x.onerror = x.onload = x.onreadystatechange = null; x.parentNode.removeChild(x); x = null; a("_eventInactived"); B() }; if (C) { v = setTimeout(function () { D() }, C) } x.type = A || "text/javascript"; x.async = w; x.onload = x.onreadystatechange = function (G, F) { if (!y && (!x.readyState || /loaded|complete/.test(x.readyState))) { y = true; if (C) { clearTimeout(v) } x.src = ""; x.onerror = x.onload = x.onreadystatechange = null; x.parentNode.removeChild(x); x = null; if (!F) { setTimeout(function () { E() }, 200) } } }; x.onerror = function (H, G, F) { if (C) { clearTimeout(v) } D(); return true }; x.src = u; z.parentNode.insertBefore(x, z) }; a("_eventPreload"); m(h); n(r, "text/javascript", false, 3000) })(this, this.document, "className", "jf-loading", "jf-active", "jf-inactive", "//ds.justfont.com/js/stable/v/4.9.1/id/150211050413");</script>
<body style="background-color: #000; font-family: Microsoft JhengHei; font-weight:bolder;" >
    <form id="form1" runat="server" >
        <div id ="Header">
            <div id="Head_inside">
                     <div style ="float:left; height:60px; width: 350px;">
                         
                         <asp:Label ID="Show_Ward_Name_Label" runat="server" Text="Label"></asp:Label>
                         
                     </div>
                     <div style= "float:left; height:60px; width:600px; text-align:left;font-size:40px;line-height:60px;" >
                         <div id="Time_update">
                          更新時間:<asp:Label ID="UpdateTimeLabel" runat="server" Text=""></asp:Label>
                         </div>
                     </div>
                     <div style ="float:left; height:60px; width:900px; text-align:right; font-size:40px; line-height:60px;">
                         <div id="Time">
                              <script>/*時間顯示*/
                                  document.getElementById('Time').innerHTML = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                                  setInterval("document.getElementById('Time').innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());", 1000);
                             </script>
                         </div>
                     </div>
            </div>
        </div>
        <div id ="Body" class="btn-group btn-group-justified">
            <div id="Body1"  >
                <div id = "Body1-1">
                    <!--護士及實習醫師資料-->
                     <asp:Label ID="NurseListLabel" runat="server" Text="Label"></asp:Label>
                    <asp:Label ID="InternListLabel" runat="server" Text=""></asp:Label>
                </div>

            </div>
            <div id="Body2" >
                <div id="doctor1" style="float:left; width:500px; height:900px;">
                    <div style="vertical-align: bottom; text-align: center; height:60px; color: #000;"><!--主治醫師表格抬頭-->
                    	<div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel1" runat="server" Text="主治醫師"></asp:Label></div>
                    	<div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel2" runat="server" Text="住院醫師"></asp:Label></div>
                        <div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel3" runat="server" Text="醫師(二)"></asp:Label></div>
                        <div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel4" runat="server" Text="代班醫師"></asp:Label></div>
                    </div>
                    <div style="width:500px;height:840px;">
<!--
            <div style="width:500px;height:80px;border-bottom-style:groove;border-width:1px;margin:2px;">
                 <div style="height:80px;width:80px;float:left;">
                        <div style="height:50px; width:80px; float:left; font-size:26px; line-height:50px; text-align:center; background-color:#F0A75F;">主治名</div>
                        <div style="height:28px; width:80px; float:left; font-size:24px; line-height:28px; text-align:center; background-color:#F0A75F; font-family:Arial;">4556</div>
                 </div>
                 <div style="height:80px;width:140px;float:left;">
                        <div style="height:50px; width:140px; float:left; font-size:46px; line-height:50px; text-align:center; background-color:#E0FFFF;">住院名</div>
                        <div style="height:28px; width:140px; float:left; font-size:32px; line-height:28px; text-align:center; background-color:#E0FFFF; font-family:Arial;">5678555</div>
                 </div>
                <div style="height:80px;width:140px;float:left;">
                        <div style="height:50px; width:140px; float:left; font-size:46px; line-height:50px; text-align:center; background-color:#BAFDFD;">實習名</div>
                        <div style="height:28px; width:140px; float:left; font-size:32px; line-height:28px; text-align:center; background-color:#BAFDFD; font-family:Arial;">5678555</div>
                 </div>
                <div style="height:80px;width:140px;float:left;">
                        <div style="height:50px; width:140px; float:left; font-size:46px; line-height:50px; text-align:center; background-color:#87FDFD;">代班名</div>
                        <div style="height:28px; width:140px; float:left; font-size:32px; line-height:28px; text-align:center; background-color:#87FDFD; font-family:Arial;">5678555</div>
                 </div>
                 
            </div>
    -->

                        <asp:Label ID="Doctor_Info_Label1" runat="server"></asp:Label>
                        <!--顯示醫師負責病床1-->
                        
                    </div>
                </div>
                <div id="doctor2" style="float:left; width:500px; height:900px;">
                    <div style="vertical-align: bottom; text-align: center; height:60px; color: #000;"><!--主治醫師表格抬頭-->
                    	<div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel5" runat="server" Text="主治醫師"></asp:Label></div>
                    	<div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel6" runat="server" Text="住院醫師"></asp:Label></div>
                        <div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel7" runat="server" Text="醫師(二)"></asp:Label></div>
                        <div class="Doctor_title" style="width:125px; height:60px;">
                            <asp:Label ID="DTitleLabel8" runat="server" Text="代班醫師"></asp:Label></div>
                    </div>
                    <div style="width:500px;height:840px;">
                        
                        <!--顯示醫生負責病床2-->
                        <asp:Label ID="Doctor_Info_Label2" runat="server"></asp:Label>
                     </div>
                        
                </div>
                
            </div>
            <div id="Body3"  >
              <!--
              <div  style="height:76px; vertical-align:top; border-radius:15px; border-style:solid; border-width:1px; margin-bottom:1px;">
                      <div style="height:38px; font-size:24px; text-align:center; line-height:38px; font-weight:bold;">病房主任  黃查某</div>
                      <div style="height:38px; font-size:24px; text-align:center; line-height:38px; font-weight:bold;">5553333</div>
              </div>
              -->
              
                <asp:Label ID="ContactListLabel" runat="server" Text="Label"></asp:Label>
              
            </div>
        </div>
        <div id ="Footer">
             <div style="height:100px; width:1860px;background-color:#FFFFFF;border-radius:50px;">
                 <div style = "width:50px; height:100px; float:left;"></div>
                  <div id = "footer1" style= "width:440px;height:100px; float:left;">
                             <div style = "width:120px; height:100px; float:left;">
                                  <div style = "width:120px; height:50px; text-align:center; font-size:32px; font-weight:bold; color:#6F6A05; line-height:50px;">
                                      <asp:Label ID="MRDTitleLabel1" runat="server" Text="總值班"></asp:Label>
                                  </div>
                                  <div style = "width:120px; height:50px; text-align:center; font-size:32px; font-weight:bold; color:#6F6A05; line-height:50px;">
                                      <asp:Label ID="MRDTitleLabel2" runat="server" Text="醫師"></asp:Label>
                                  </div>  
                             </div>
                             <div style = "width:320px; height:100px; float:left;">
                                 <!--
                                  <div style="width:150px; height:50px; float:left; text-align:center; font-size:40px">0000</div>
                                  <div style="width:200px; height:50px; float:left; text-align:left; font-size:40px">廖添丁</div>
                                  <div style="width:150px; height:50px; float:left; text-align:center; font-size:40px">0233</div>
                                  <div style="width:200px; height:50px; float:left; text-align:left; font-size:40px">紅龜</div>  
                                 -->
                                 <asp:Label ID="MRD_ImfoLabe" runat="server"></asp:Label>                                   
                             </div>
                  </div>  
                  <div id = "footer2" style= "width:440px;height:100px; float:left;">
                             <div style = "width:120px; height:100px; float:left;">
                                  <div style = "width:120px; height:50px; text-align:center; font-size:32px; font-weight:bold; color:#9D6817; line-height:50px;">
                                      <asp:Label ID="RDTitleLabel1" runat="server" Text="值班"></asp:Label>
                                  </div>
                                  <div style = "width:120px; height:50px; text-align:center; font-size:32px; font-weight:bold; color:#9D6817; line-height:50px; ">
                                      <asp:Label ID="RDTitleLabel2" runat="server" Text="醫師"></asp:Label>
                                  </div>  
                             </div>
                             <div style = "width:320px; height:100px; float:left;">
                                 <!--
                                  <div style="width:200px; height:100px; float:left; text-align:center; line-height:100px; font-size:60px">0000</div>
                                  <div style="width:200px; height:100px; float:left; text-align:left; line-height:100px; font-size:60px">沒有人</div>
                                 -->
                                 <asp:Label ID="R_InfoLabel" runat="server"></asp:Label>
                             </div>
                  </div> 
                  <div id = "footer3" style= "width:440px;height:100px; float:left;">
                             <div style = "width:120px; height:100px; float:left;">
                                  <div style = "width:120px; height:50px; text-align:center; font-size:26px; font-weight:bold; color:#465E62; line-height:50px;">
                                      <asp:Label ID="INTTitleLabel1" runat="server" Text=""></asp:Label>
                                  </div>
                                  <div style = "width:120px; height:50px; text-align:center; font-size:26px; font-weight:bold; color:#465E62; line-height:50px;">
                                      <asp:Label ID="INTTitleLabel2" runat="server" Text=""></asp:Label>
                                  </div>  
                             </div>
                             <div style = "width:320px; height:100px; float:left;">
                                  <div style="width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:40px; font-family:Arial;">
                                      <asp:Label ID="INTEmpnolabel" runat="server"></asp:Label></div>
                                  <div style="width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:50px">
                                      <asp:Label ID="INTNAMElabel" runat="server"></asp:Label></div>
                             </div>

                  </div> 
                  <div id = "footer4" style= "width:440px;height:100px; float:left;">
                             <div style = "width:120px; height:100px; float:left;">
                                  <div style = "width:120px; height:50px; text-align:center; font-size:28px; font-weight:bold; color:#22845B; line-height:50px;">
                                      <asp:Label ID="NSPTitleLabel1" runat="server" Text=""></asp:Label>
                                  </div>
                                  <div style = "width:120px; height:50px; text-align:center; font-size:28px; font-weight:bold; color:#22845B; line-height:50px;">
                                      <asp:Label ID="NSPTitleLabel2" runat="server" Text=""></asp:Label>
                                  </div>  
                             </div>
                             <div style = "width:320px; height:100px; float:left;">
 

                                 <asp:Label ID="LAST_RD_INFO_Label" runat="server" Text=""></asp:Label>
 

                             </div>
                  </div> 
             </div>
        </div>
           <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        <asp:GridView ID="GridView2" runat="server"></asp:GridView>
        <asp:GridView ID="GridView3" runat="server">
        </asp:GridView>
    </form>
    
</body>
</html>
