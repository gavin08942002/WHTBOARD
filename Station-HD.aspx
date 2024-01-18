﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Station-HD.aspx.vb" Inherits="Station" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title></title>
    <link href="Content/station.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/html5shiv.min.js"></script>
    <script src="Scripts/respond.js"></script>
    <script src="Scripts/ServerDate.js"></script>
    <script language="JavaScript">
        function QueryString(name) {
            var AllVars = window.location.search.substring(1);
            var Vars = AllVars.split("&");
            for (i = 0; i < Vars.length; i++) {
                var Var = Vars[i].split("=");
                if (Var[0] == name) return Var[1];
            }
            return "";
        }
        function myrefresh() {
            var Hospcode = QueryString("Hospcode")
            var Wardcode = QueryString("Wardcode")
            window.location.href = 'station.aspx?Hospcode=' + Hospcode + '&Wardcode=' + Wardcode;
        }



        setTimeout('myrefresh()', 605000); //指定11分鐘刷新一次
    </script>

<link href="Content/Patient.css" rel="stylesheet" type="text/css"/></head>
    <script type="text/javascript">var _jf = _jf || []; _jf.push(['p', '32786']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'css', '.sourcehansans-tc-bold']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'alias', 'sourcehansans-tc-bold']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'weight', 700]); (function (f, q, c, h, e, i, r, d) { var k = f._jf; if (k.constructor === Object) { return } var l, t = q.getElementsByTagName("html")[0], a = function (u) { for (var v in k) { if (k[v][0] == u) { if (false === k[v][1].call(k)) { break } } } }, j = /\S+/g, o = /[\t\r\n\f]/g, b = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, g = "".trim, s = g && !g.call("\uFEFF\xA0") ? function (u) { return u == null ? "" : g.call(u) } : function (u) { return u == null ? "" : (u + "").replace(b, "") }, m = function (y) { var w, z, v, u, x = typeof y === "string" && y; if (x) { w = (y || "").match(j) || []; z = t[c] ? (" " + t[c] + " ").replace(o, " ") : " "; if (z) { u = 0; while ((v = w[u++])) { if (z.indexOf(" " + v + " ") < 0) { z += v + " " } } t[c] = s(z) } } }, p = function (y) { var w, z, v, u, x = arguments.length === 0 || typeof y === "string" && y; if (x) { w = (y || "").match(j) || []; z = t[c] ? (" " + t[c] + " ").replace(o, " ") : ""; if (z) { u = 0; while ((v = w[u++])) { while (z.indexOf(" " + v + " ") >= 0) { z = z.replace(" " + v + " ", " ") } } t[c] = y ? s(z) : "" } } }, n; k.push(["_eventActived", function () { p(h); m(e) }]); k.push(["_eventInactived", function () { p(h); m(i) }]); k.addScript = n = function (u, A, w, C, E, B) { E = E || function () { }; B = B || function () { }; var x = q.createElement("script"), z = q.getElementsByTagName("script")[0], v, y = false, D = function () { x.src = ""; x.onerror = x.onload = x.onreadystatechange = null; x.parentNode.removeChild(x); x = null; a("_eventInactived"); B() }; if (C) { v = setTimeout(function () { D() }, C) } x.type = A || "text/javascript"; x.async = w; x.onload = x.onreadystatechange = function (G, F) { if (!y && (!x.readyState || /loaded|complete/.test(x.readyState))) { y = true; if (C) { clearTimeout(v) } x.src = ""; x.onerror = x.onload = x.onreadystatechange = null; x.parentNode.removeChild(x); x = null; if (!F) { setTimeout(function () { E() }, 200) } } }; x.onerror = function (H, G, F) { if (C) { clearTimeout(v) } D(); return true }; x.src = u; z.parentNode.insertBefore(x, z) }; a("_eventPreload"); m(h); n(r, "text/javascript", false, 3000) })(this, this.document, "className", "jf-loading", "jf-active", "jf-inactive", "//ds.justfont.com/js/stable/v/4.9.1/id/150211050413");</script>
<body style="background-color: #000; font-family: Microsoft JhengHei; font-weight:bolder;" >
    <form id="form1" runat="server" >
        <div id ="Header">
            <div id="Head_inside">
                     <div style ="float:left; height:60px; width: 350px;">
                         
                         <asp:Label ID="Show_Ward_Name_Label" runat="server" Text="Label"></asp:Label>
                     </div>
                     <div style= "float:left; height:60px; width:900px; text-align:left;font-size:40px;line-height:60px;" >
                     <!-- 隱藏輕中重交班顯示-->
                         <div style="visibility:hidden; width:100px;height:60px;float:left;">
                             <div style=" width:50px;height:60px;float:left;"><asp:Label ID="URL_Change_Label" runat="server" Text=""></asp:Label></div>
                             <div style=" width:50px;height:60px;float:left;background-color:rgb(155, 187, 89);color:black;text-align:center;">
                                 <asp:Label ID="LightLabel" runat="server" Text="0"></asp:Label>
                             </div>
                         </div>
                         <div style="visibility:hidden; width:100px;height:60px;float:left;">
                             <div style=" width:50px;height:60px;float:left;"><asp:Label ID="URL_Reset_Label" runat="server" Text=""></asp:Label></div>
                             <div style=" width:50px;height:60px;float:left;background-color:yellow;color:black;text-align:center;">
                                 <asp:Label ID="MediumLabel" runat="server" Text="0"></asp:Label>
                             </div>
                         </div>
                         <div style="visibility:hidden; width:100px;height:60px;float:left;">
                             <div style=" width:50px;height:60px;float:left;">重</div>
                             <div style=" width:50px;height:60px;float:left;background-color:rgb(255, 51, 51);color:black;text-align:center;">
                                 <asp:Label ID="HeaveyLabel" runat="server" Text="0"></asp:Label>
                             </div>
                         </div>





                         <div style=" width:200px;height:60px;float:left;">
                                <div style=" width:100px;height:60px;float:left;font-size:30px;visibility:hidden;">佔床率</div>
                               <div style=" width:100px;height:60px;float:left;background-color:#FFFFFF;color:#000;text-align:center;visibility:hidden;">
                                   <asp:Label ID="Take_up_rate_Label" runat="server" Text="Label"></asp:Label>
                               </div>
                         </div>
                         <div style=" width:200px;height:60px;float:left;">
                                <div style=" width:50px;height:60px;float:left;"></div>
                                <div style=" width:100px;height:60px;float:left;visibility:hidden;">空床</div>
                               <div style=" width:50px;height:60px;float:left;background-color:wheat;color:black;text-align:center;visibility:hidden;">
                                   <asp:Label ID="Empty_Bed_Label" runat="server" Text="Label"></asp:Label>
                               </div>
                         </div>
                         <div style=" width:200px;height:60px;float:left;">
                                <div style=" width:50px;height:60px;float:left;"></div>
                                <div style=" width:100px;height:60px;float:left;visibility:hidden;">全部</div>
                               <div style=" width:50px;height:60px;float:left;background-color:silver;color:black;text-align:center;visibility:hidden;">
                                   <asp:Label ID="All_bed_Label" runat="server" Text="Label"></asp:Label>
                               </div>
                         </div>
                     </div>
                     <div style ="float:left; height:60px; width:600px; text-align:right; font-size:40px; line-height:60px;">
                         <div id="Time">
                              <script>                                  /*SERVER時間顯示*/
                                  document.getElementById('Time').innerHTML =date.toLocaleString() + ' 星期' + '日一二三四五六'.charAt(date.getDay());
                                  setInterval("date.setSeconds(date.getSeconds()+1); document.getElementById('Time').innerHTML= date.toLocaleString()+' 星期'+'日一二三四五六'.charAt(date.getDay());var time = new date;   if ( time.getMinutes()%10 == 0 && time.getSeconds() == 5) myrefresh();", 1000);
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
            <div id="Body2" style ="width:750px;"  >
              <!--<iframe id="iframeHD" src="" height="900" width="750"  style="border: 0px;padding:0px; "  ></iframe>  
              -->  
              <asp:Label ID="LabelHD" runat="server" Text="Label"></asp:Label>
                
            </div>
            <div id="Body3" style ="width:510px;" >
                <asp:Label ID="ContactListLabel" runat="server"></asp:Label>
            </div>
        </div>
        <div id ="Footer">
             <div style="height:100px; width:1860px;background-color:#FFFFFF;border-radius:50px;">
                 <div style = "width:30px; height:100px; float:left;"></div>
                  <div id = "footer1" style= "width:366px;height:100px; float:left;">
                             <div style = "width:100px; height:100px; float:left;">
                                  <div style = "width:100px; height:100px; text-align:center; font-size:32px; font-weight:bold; color:#6F6A05; line-height:50px;">
                                      <asp:Label ID="TitleLabel1" runat="server"></asp:Label>
                                  </div>
                             </div>
                             <div style = "width:266px; height:100px; float:left;">
  
                                 <asp:Label ID="DUTY_InfoLabe1" runat="server"></asp:Label>                                   
                             </div>
                  </div>  
                  <div id = "footer2" style= "width:366px;height:100px; float:left;">
                             <div style = "width:100px; height:100px; float:left;">
                                  <div style = "width:100px; height:100px; text-align:center; font-size:32px; font-weight:bold; color:#9D6817; line-height:50px;">
                                      <asp:Label ID="TitleLabel2" runat="server"></asp:Label>
                                  </div>

                             </div>
                             <div style = "width:266px; height:100px; float:left;">
                                 <asp:Label ID="DUTY_InfoLabe2" runat="server"></asp:Label>
                             </div>
                  </div> 
                    <div id = "footer5" style= "width:366px;height:100px; float:left;">
                             <div style = "width:100px; height:100px; float:left;">
                                  <div style = "width:100px; height:100px; text-align:center; font-size:32px; font-weight:bold; color:#162c79; line-height:50px;">
                                      <asp:Label ID="TitleLabel5" runat="server"></asp:Label>
                                  </div>

                             </div>
                             <div style = "width:266px; height:100px; float:left;">
                                 <asp:Label ID="DUTY_InfoLabe5" runat="server"></asp:Label>
                             </div>
                  </div> 




                  <div id = "footer3" style= "width:366px;height:100px; float:left;">
                             <div style = "width:100px; height:100px; float:left;">
                                  <div style = "width:100px; height:100px; text-align:center; font-size:32px; font-weight:bold; color:#465E62; line-height:50px;">
                                      <asp:Label ID="TitleLabel3" runat="server"></asp:Label>
                                  </div>
                              </div>
                             <div style = "width:266px; height:100px; float:left;">
                                 <asp:Label ID="DUTY_InfoLabe3" runat="server"></asp:Label>
                             </div>

                  </div> 
                  <div id = "footer4" style= "width:366px;height:100px; float:left;">
                             <div style = "width:100px; height:100px; float:left;">
                                  <div style = "width:100px; height:100px; text-align:center; font-size:32px; font-weight:bold; color:#22845b; line-height:50px;">
                                      <asp:Label ID="TitleLabel4" runat="server"></asp:Label>
                                  </div>
                             </div>
                             <div style = "width:266px; height:100px; float:left;">
                                 <asp:Label ID="DUTY_InfoLabe4" runat="server"></asp:Label>
                             </div>
                  </div> 
             </div>
          
        </div>

           <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        <asp:GridView ID="GridView2" runat="server"></asp:GridView>
    </form>
    
</body>
</html>
