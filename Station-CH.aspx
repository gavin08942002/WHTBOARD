<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Station-CH.aspx.vb" Inherits="Station" %>

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
            window.location.href = 'station.aspx?Hospcode='+Hospcode+'&Wardcode='+Wardcode;
        }
        function QueryString(name) {
            var AllVars = window.location.search.substring(1);
            var Vars = AllVars.split("&");
            for (i = 0; i < Vars.length; i++) {
                var Var = Vars[i].split("=");
                if (Var[0] == name) return Var[1];
            }
            return "";
        }
                setTimeout('myrefresh()', 605000); //指定11分鐘刷新一次
    </script>

<link href="Content/Patient.css" rel="stylesheet" type="text/css"/></head>
    <script type="text/javascript">var _jf = _jf || []; _jf.push(['p', '32786']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'css', '.sourcehansans-tc-bold']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'alias', 'sourcehansans-tc-bold']); _jf.push(['_setFont', 'sourcehansans-tc-bold', 'weight', 700]); (function (f, q, c, h, e, i, r, d) { var k = f._jf; if (k.constructor === Object) { return } var l, t = q.getElementsByTagName("html")[0], a = function (u) { for (var v in k) { if (k[v][0] == u) { if (false === k[v][1].call(k)) { break } } } }, j = /\S+/g, o = /[\t\r\n\f]/g, b = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, g = "".trim, s = g && !g.call("\uFEFF\xA0") ? function (u) { return u == null ? "" : g.call(u) } : function (u) { return u == null ? "" : (u + "").replace(b, "") }, m = function (y) { var w, z, v, u, x = typeof y === "string" && y; if (x) { w = (y || "").match(j) || []; z = t[c] ? (" " + t[c] + " ").replace(o, " ") : " "; if (z) { u = 0; while ((v = w[u++])) { if (z.indexOf(" " + v + " ") < 0) { z += v + " " } } t[c] = s(z) } } }, p = function (y) { var w, z, v, u, x = arguments.length === 0 || typeof y === "string" && y; if (x) { w = (y || "").match(j) || []; z = t[c] ? (" " + t[c] + " ").replace(o, " ") : ""; if (z) { u = 0; while ((v = w[u++])) { while (z.indexOf(" " + v + " ") >= 0) { z = z.replace(" " + v + " ", " ") } } t[c] = y ? s(z) : "" } } }, n; k.push(["_eventActived", function () { p(h); m(e) }]); k.push(["_eventInactived", function () { p(h); m(i) }]); k.addScript = n = function (u, A, w, C, E, B) { E = E || function () { }; B = B || function () { }; var x = q.createElement("script"), z = q.getElementsByTagName("script")[0], v, y = false, D = function () { x.src = ""; x.onerror = x.onload = x.onreadystatechange = null; x.parentNode.removeChild(x); x = null; a("_eventInactived"); B() }; if (C) { v = setTimeout(function () { D() }, C) } x.type = A || "text/javascript"; x.async = w; x.onload = x.onreadystatechange = function (G, F) { if (!y && (!x.readyState || /loaded|complete/.test(x.readyState))) { y = true; if (C) { clearTimeout(v) } x.src = ""; x.onerror = x.onload = x.onreadystatechange = null; x.parentNode.removeChild(x); x = null; if (!F) { setTimeout(function () { E() }, 200) } } }; x.onerror = function (H, G, F) { if (C) { clearTimeout(v) } D(); return true }; x.src = u; z.parentNode.insertBefore(x, z) }; a("_eventPreload"); m(h); n(r, "text/javascript", false, 3000) })(this, this.document, "className", "jf-loading", "jf-active", "jf-inactive", "//ds.justfont.com/js/stable/v/4.9.1/id/150211050413");</script>
<body style="background-color: #000; font-family: Microsoft JhengHei; font-weight:bolder;height:1020px;" >
    <form id="form1" runat="server" >
        <div id ="Header">
            <div id="Head_inside">
                     <div style ="float:left; height:60px; width: 350px;">
                         
                         <asp:Label ID="Show_Ward_Name_Label" runat="server" Text="Label"></asp:Label>
                     </div>
                     <div style= "float:left; height:60px; width:900px; text-align:left;font-size:40px;line-height:60px;" >
                         <div style=" width:100px;height:60px;float:left;">
                             <div style=" width:50px;height:60px;float:left;"><asp:Label ID="URL_Change_Label" runat="server" Text=""></asp:Label></div>
                             <div style=" width:50px;height:60px;float:left;background-color:rgb(155, 187, 89);color:black;text-align:center;">

                                 <asp:Label ID="LightLabel" runat="server" Text="0"></asp:Label>
                             </div>
                         </div>
                         <div style=" width:100px;height:60px;float:left;">
                             <div style=" width:50px;height:60px;float:left;"><asp:Label ID="URL_Reset_Label" runat="server" Text="Label"></asp:Label></div>
                             <div style=" width:50px;height:60px;float:left;background-color:yellow;color:black;text-align:center;">

                                 <asp:Label ID="MediumLabel" runat="server" Text="0"></asp:Label>
                             </div>
                         </div>
                         <div style=" width:100px;height:60px;float:left;">
                             <div style=" width:50px;height:60px;float:left;">重</div>
                             <div style=" width:50px;height:60px;float:left;background-color:rgb(255, 51, 51);color:black;text-align:center;">

                                 <asp:Label ID="HeaveyLabel" runat="server" Text="0"></asp:Label>
                             </div>
                         </div>
                         

                         <div style=" width:200px;height:60px;float:left;">
                                
                                <div style=" width:100px;height:60px;float:left;font-size:30px;">佔床率</div>
                               <div style=" width:100px;height:60px;float:left;background-color:#FFFFFF;color:#000;text-align:center;">

                                   <asp:Label ID="Take_up_rate_Label" runat="server" Text="Label"></asp:Label>
                               </div>
                         </div>
                         <div style=" width:200px;height:60px;float:left;">
                                <div style=" width:50px;height:60px;float:left;"></div>
                                <div style=" width:100px;height:60px;float:left;">空床</div>
                               <div style=" width:50px;height:60px;float:left;background-color:wheat;color:black;text-align:center;">
                                   <asp:Label ID="Empty_Bed_Label" runat="server" Text="Label"></asp:Label>
                               </div>
                         </div>
                         <div style=" width:200px;height:60px;float:left;">
                                <div style=" width:50px;height:60px;float:left;"></div>
                                <div style=" width:100px;height:60px;float:left;">全部</div>
                               <div style=" width:50px;height:60px;float:left;background-color:silver;color:black;text-align:center;">
                                   <asp:Label ID="All_bed_Label" runat="server" Text="Label"></asp:Label>
                               </div>
                         </div>
                     </div>
                     <div style ="float:left; height:60px; width:600px; text-align:right; font-size:40px; line-height:60px;">
                         <div id="Time">
                              <script>                                  /*時間顯示*/
                                  var Hospcode = QueryString("Hospcode");
                                  if (Hospcode != '2')  {
                                  document.getElementById('Time').innerHTML = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                                  setInterval("document.getElementById('Time').innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());var time = new Date();   if ( time.getMinutes()%10 == 0 && time.getSeconds() == 5) myrefresh();", 1000);
                                  } 

                                  
                             </script>
                         </div>
                     </div>
            </div>
        </div>
        <div id ="Body" class="btn-group btn-group-justified">
            <div id="Body1" style="height:1020px;" >
                <div id = "Body1-1" style ="height:1020px;">
                    <!--護士及實習醫師資料-->
                     <asp:Label ID="NurseListLabel" runat="server" Text="Label"></asp:Label>
                    <asp:Label ID="InternListLabel" runat="server" Text=""></asp:Label>
                </div>

            </div>
            <div id="Body2" style ="width:750px;height:1020px;"  >
                <div id="doctor1" style="float:left; width:750px; height:900px;">
                    <div style="vertical-align: bottom; text-align: center; height:60px; color: #000;"><!--主治醫師表格抬頭-->
                    	<div class="Doctor_title" >
                            <asp:Label ID="DTitleLabel1" runat="server" Text="主治醫師"></asp:Label></div>
                    	<div class="Doctor_title" style ="font-size:30px;width:625px;" >
                            <asp:Label ID="DTitleLabel2" runat="server" Text="病床"></asp:Label></div>

                    </div>
                    <div style="width:750px;height:960px;">
                        <!-- 主治醫師病床列表-->
                        <asp:Label ID="Doctor_Info_Label" runat="server" Text=""></asp:Label>
                        <!--
                           <div style="width:750px;height:64px;border-bottom-style:groove;border-width:1px;margin:0px;">
                                     <div style="height:64px;width:125px;float:left;">
                                             <div style="height:42px; width:125px; float:left; font-size:40px; line-height:34px; text-align:center; background-color:#F0A75F;">宋聿翔</div>
                                             <div style="height:20px; width:125px; float:left; font-size:30px; line-height:16px; text-align:center; background-color:#F0A75F; font-family:Arial;">6190</div>
                                     </div>
                                     <div style="height:64px;width:625px;float:left">
                                            <div class ="Nurse_ward_7">01A</div>
                                            <div class ="Nurse_ward_7">02A</div>
                                            <div class ="Nurse_ward_7">01B</div>
                                            <div class ="Nurse_ward_7">01C</div>
                                            <div class ="Nurse_ward_7">03A</div>
                                            <div class ="Nurse_ward_7">03B</div>
                                            <div class ="Nurse_ward_7">03C</div>
                                            <div class ="Nurse_ward_7">09A</div>
                                            <div class ="Nurse_ward_7">09B</div>
                                     </div>
                           </div>
                         -->  
                           
                        
                        <!--顯示醫師負責病床1-->
                    </div>
                </div>
                
                
            </div>
            <div id="Body3" style ="width:510px;height:1020px;" >
                <asp:Label ID="ContactListLabel" runat="server"></asp:Label>
            </div>
        </div>
                

           <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        <asp:GridView ID="GridView2" runat="server"></asp:GridView>
    </form>
    
</body>
</html>
