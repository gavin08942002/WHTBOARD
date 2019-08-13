Imports System.Data
Imports System.Data.OleDb
Imports System.Web.Security
Imports System.DBNull
Public Class WebLogin
    Inherits System.Web.UI.Page
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents But_End As System.Web.UI.WebControls.Button
    'Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    'Protected WithEvents txtUser As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Dim strConn As String         '資料庫連接字串
    Dim strSQL As String
    ' Protected WithEvents But_Enter As System.Web.UI.WebControls.Button
    ' Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Dim strReader As OleDbDataReader

#Region " Web Form 設計工具產生的程式碼 "

    '此呼叫為 Web Form 設計工具的必要項。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: 此方法呼叫為 Web Form 設計工具的必要項
        '請勿使用程式碼編輯器來修改它。
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '在此加入要初始化頁面的使用者程式碼
        'Put user code to initialize the page here



        If Not Page.IsPostBack Then
            Session("OPUSER") = ""
            Dim weekdaynow As String
            If Weekday(Now) = 1 Then
                weekdaynow = "星期日"
            ElseIf Weekday(Now) = 2 Then
                weekdaynow = "星期一"
            ElseIf Weekday(Now) = 3 Then
                weekdaynow = "星期二"
            ElseIf Weekday(Now) = 4 Then
                weekdaynow = "星期三"
            ElseIf Weekday(Now) = 5 Then
                weekdaynow = "星期四"
            ElseIf Weekday(Now) = 6 Then
                weekdaynow = "星期五"
            ElseIf Weekday(Now) = 7 Then
                weekdaynow = "星期六"
            End If
            Label3.Text = "現在時間：" & weekdaynow & Now
            Session("weekdaynow") = weekdaynow
            '*************************************
            Dim strCom As OleDbCommand = New OleDbCommand()  'SQL command (oracle)
            '連接資料庫字串
            strConn = "Provider=msdaora.1;user id=dr;data source=opd_ord; persist security info=true;password=mmhedp"
            Dim sConn As OleDbConnection = New OleDbConnection(strConn)
            strCom.Connection = sConn
            strCom.Connection.Open()
            Session("strCom") = strCom
            Session("sConn") = sConn
            '*************************************
            strSQL = ""
            strSQL += "select * from phyini "
            strSQL += "where type='P' "
            strSQL += " and csdp='1911'"

            'Dim sConn As OleDbConnection = New OleDbConnection(strConn)
            'strCom.Connection = sConn
            'strCom.Connection.Open()

            strCom.CommandType = CommandType.Text
            strCom.CommandText = strSQL

            strReader = strCom.ExecuteReader()

            '顯示基本資料
            While strReader.Read = True
                If strReader("dbtype") = "1" Then
                    Session("strConn") = strReader("conn")
                    'Rbl_Hospid.Items.FindByValue(strReader("hospid")).Selected = True
                    'Session("Hospital") = strReader("hospid")
                    Session("Hospital") = 4

                ElseIf strReader("dbtype") = "2" Then
                    Session("strConn_SQL") = strReader("conn")
                End If
            End While
            strReader.Close()
        End If
    End Sub

    Private Sub But_Enter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles But_Enter.Click

        '連接資料庫字串
        'strConn = Session("strConn")

        'Dim sConn As OleDbConnection = New OleDbConnection(strConn)
        'strCom.Connection = sConn
        'strCom.Connection.Open()
        Dim UserId As String = UCase(txtUser.Text)
        Dim strCom As OleDbCommand
        strCom = Session("strCom")

        strSQL = ""
        strSQL = strSQL & "select name,password,COSTDP from usr"
        strSQL = strSQL & " where empno='" & UserId & "'"
        'If txtPassword.Text <> "" Then
        '    strSQL = strSQL & " and password='" & txtPassword.Text & "'"
        'Else
        '    strSQL = strSQL & " and password is null"
        'End If

        '判斷登入者
        'If UserId = "K029" Or UserId = "4805" Or UserId = "3005" Or UserId = "4068" Or UserId = "8484" Or UserId = "2145" Or UserId = "1254" Then

        strCom.CommandType = CommandType.Text
        strCom.CommandText = strSQL

        strReader = strCom.ExecuteReader()

        '顯示基本資料
        If strReader.Read = True Then
            Session("OPUSER") = Trim(txtUser.Text)
            Session("OPUSERNAME") = strReader("name")
            Session("OPUSERPASSWORD") = strReader("password")
            '成本中心
            Session("OPUSERCOSTDP") = strReader("COSTDP")
            strReader.Close()
            Dim PwdService As New MMHODBC_NEW.ODBC()
            Dim myPassWord As String

            If IsDBNull(Trim(txtPassword.Text)) Then
                myPassWord = ""
            Else
                myPassWord = Trim(txtPassword.Text)
            End If

            '**************
            If Session("OPUSER") = "mmh" Then

                Response.Redirect("add.aspx")
            End If
            '**************

            If PwdService.gfIsPassWordOk(myPassWord, Session("OPUSERPASSWORD"), Session("OPUSERCOSTDP")) Then
                FormsAuthentication.SignOut()
                FormsAuthentication.RedirectFromLoginPage("1039", False)
                Session("OPUSER") = Trim(UserId)
                'UserId = Session("OPUSER")

                Response.Redirect("/web/system.aspx")

            Else
                strReader.Close()
                lblMsg.Text = "使用者密碼輸入錯誤!!請重新輸入!!"
                txtUser.Text = ""
                txtPassword.Text = ""
            End If

        Else
            lblMsg.Text = UserId & " 使用者無使用權限!!"

        End If

    End Sub
End Class
