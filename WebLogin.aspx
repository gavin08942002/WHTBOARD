

<%@ Page Language="vb" AutoEventWireup="false" codefile="WebLogin.aspx.vb" Inherits="WebLogin" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>�@�e��t��</title>
		<meta content="True" name="vs_showGrid">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="vbscript" >
			sub gfclose()
				window.close()
			end sub
		</script>
	</HEAD>
	<body bgColor="#cccccc" background="image/mmh.gif" MS_POSITIONING="GridLayout">
		<form id="Form1" name="Form1" method="post" runat="server">
			<FONT face="�s�ө���">
				<asp:label id="Label1" style="Z-INDEX: 100; LEFT: 184px; POSITION: absolute; TOP: 216px" runat="server" Width="148px" Height="19px" ForeColor="#0000C0" Font-Bold="True" Font-Size="Medium" BackColor="Silver">���u�N���G</asp:label>
				<asp:textbox id="txtUser" style="Z-INDEX: 102; LEFT: 304px; POSITION: absolute; TOP: 208px" runat="server" Width="108px" Height="31px" ForeColor="Magenta" Font-Size="Medium"></asp:textbox><asp:label id="Label2" style="Z-INDEX: 101; LEFT: 184px; POSITION: absolute; TOP: 248px" runat="server" Width="126px" Height="28px" ForeColor="#0000C0" Font-Bold="True" Font-Size="Medium" BackColor="Silver">�K�@�@�X�G</asp:label><asp:textbox id="txtPassword" style="Z-INDEX: 103; LEFT: 304px; POSITION: absolute; TOP: 248px" runat="server" Width="108px" Height="31px" ForeColor="Magenta" Font-Size="Medium" TextMode="Password"></asp:textbox><asp:button id="But_Enter" style="Z-INDEX: 104; LEFT: 312px; POSITION: absolute; TOP: 296px" runat="server" Width="92px" Height="39px" Font-Bold="True" Font-Size="Medium" Text="�T�@�{"></asp:button><INPUT style="FONT-WEIGHT: bold; FONT-SIZE: medium; Z-INDEX: 105; LEFT: 7px; WIDTH: 91px; POSITION: absolute; TOP: 8px; HEIGHT: 37px" accessKey="e" onclick="window.close" type="button" value="���� ��"></FONT>
			<asp:label id="lblMsg" style="Z-INDEX: 106; LEFT: 512px; POSITION: absolute; TOP: 168px" runat="server" Width="211px" Height="136px" ForeColor="Red" Font-Size="Medium" BackColor="Transparent" Font-Names="�ө���"></asp:label><asp:label id="Label4" style="Z-INDEX: 107; LEFT: 120px; POSITION: absolute; TOP: 8px" runat="server" Width="501px" Height="48px" ForeColor="Blue" Font-Bold="True" Font-Size="XX-Large" Font-Names="�з���">�q�l�ժO�t��</asp:label><asp:label id="Label3" style="Z-INDEX: 108; LEFT: 199px; POSITION: absolute; TOP: 343px" runat="server" Height="20px" Font-Size="X-Small"></asp:label>
			<asp:Label id="Label5" style="Z-INDEX: 110; LEFT: 23px; POSITION: absolute; TOP: 105px" runat="server" Font-Size="Larger" Font-Bold="True" ForeColor="Green">�]��T�w�����D�A�Y��_���n�J�~�i�ϥΡA�b���K�X�P�}��t��</asp:Label></form>
		<script language="vbs">
                sub window_onload()
                    form1.txtuser.focus
                end sub
		</script>
	</body>
</HTML>
