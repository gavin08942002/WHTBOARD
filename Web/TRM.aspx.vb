Imports System
Imports System.IO
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web.Services
Imports System.Web.DynamicData
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.Collections
Partial Class Web_Default
    Inherits System.Web.UI.Page
    Public Function SELECT_ORACLE(ByVal str As String) As String
        Dim OraStr As String = Nothing

        Select Case str
            Case "1"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tp_opd_ord; persist security info=true;password=his1160"
            Case "2"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=ts_opd_ord; persist security info=true;password=his1160"
            Case "3"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tt_opd_ord; persist security info=true;password=his1160"
            Case "4"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=hc_opd_ord; persist security info=true;password=his1160"
        End Select
        Return OraStr
    End Function
    'HC專用連接字串
    Public Function SELECT_ORACLE_HC(ByVal str As String) As String
        Dim OraStr As String = Nothing

        Select Case str
            Case "1"
                OraStr = "Provider=OraOLEDB.Oracle;user id=hcora;data source=tp_opd_ord; persist security info=true;password=hcora7330"
            Case "2"
                OraStr = "Provider=OraOLEDB.Oracle;user id=hcora;data source=ts_opd_ord; persist security info=true;password=hcora7330"
            Case "3"
                OraStr = "Provider=OraOLEDB.Oracle;user id=hcora;data source=tt_opd_ord; persist security info=true;password=hcora7330"
            Case "4"
                OraStr = "Provider=OraOLEDB.Oracle;user id=hcora;data source=hc_opd_ord; persist security info=true;password=hcora7330"
        End Select
        Return OraStr
    End Function
    'A----資訊欄資料初始化
    Public Function init_info(hospcode As String, pno As String, Empno As String, opdt As String) As String


        Dim ConnStr As String = SELECT_ORACLE(hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        '##############################手術病人資料撈取
        Dim cmd As String = String.Format("SELECT T.pno, I.name,T.Sch_bedno,T.Sch_Caseno, T.sch_opdt ,T.sch_opdr,D.name as DRname,T.sch_opfloor, T.sch_scd2  ")
        cmd += String.Format(" , DECODE(T.sch_actfg,1,'未報到',2,'報到',3,'手術中',4,'完成手術',5,'取消排程',6,'取消報到',7,'恢復室',8,'已離開') sch_actfg ")
        cmd += String.Format(" FROM   TOPR_SCH T ,DR D,IDP I")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" T.sch_opdr = D.code")
        cmd += String.Format(" AND T.pno = I.pno")
        cmd += String.Format(" AND T.pno = '{0}'", pno)
        cmd += String.Format(" AND T.sch_opdt = To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", opdt)

        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_PNO As New DataTable

        DA.Fill(DT_OP_PNO)
        'GridView1.Caption = "病患資料"
        ' GridView1.DataSource = DT_OP_PNO
        ' GridView1.DataBind()
        '########################################################
        Dim Dr_Code As String = ""
        Dim Dr_Name As String = ""
        Dim Patient_pno As String = ""
        Dim Patient_Name As String = ""
        Dim Bedno As String = ""
        Dim Caseno As Integer = 0
        Dim dt_opdt As String = ""



        If DT_OP_PNO.Rows.Count > 0 Then
            Patient_Name = DT_OP_PNO.Rows(0)("name").ToString
            Patient_pno = DT_OP_PNO.Rows(0)("pno").ToString
            Dr_Code = DT_OP_PNO.Rows(0)("Sch_Opdr").ToString
            Dr_Name = DT_OP_PNO.Rows(0)("Drname").ToString
            Bedno = DT_OP_PNO.Rows(0)("Sch_bedno").ToString
            If Not IsDBNull(DT_OP_PNO.Rows(0)("Sch_Caseno")) Then
                Caseno = DT_OP_PNO.Rows(0)("Sch_Caseno")
            End If

            dt_opdt = FormatDateTime(DT_OP_PNO.Rows(0)("Sch_Opdt"), DateFormat.ShortDate).ToString
        End If
        Label_hospcode.Text = Request.QueryString("hospcode")
        Label_name.Text = Patient_Name
        Label_pno.Text = Patient_pno
        Label_Drcode.Text = Dr_Code
        Label_Drname.Text = Dr_Name
        Label_Bedno.Text = Bedno
        Label_caseno.Text = Caseno
        Label_Empno.Text = Empno
        Label_opdt.Text = opdt
        Label_dt_opdt.Text = dt_opdt



    End Function
    'B------病史、術式初始化
    Public Function init_pi(Hospcode As String, pno As String, nowdate As String) As Boolean

        '判斷是否已經輸入過資料
        Dim ifhaddata As Boolean = False
        Dim ConnStr As String = SELECT_ORACLE_HC(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = ""
        cmd += String.Format(" SELECT pno,opdt,or_effect,or_pi1, or_pi2, or_pi3, or_pi4, or_pi5 ,or_pi6, or_pi7, or_pi8, or_scd1, or_scd2, or_scd3, or_scd4, or_scd5, or_scd6, or_scd6c")
        cmd += String.Format(" FROM trm_SCH  ")
        cmd += String.Format(" WHERE  pno = {0} AND opdt = To_Date('{1}','yyyy/MM/DD HH24:MI:SS')  AND or_effect= 'Y' ", pno, nowdate)

        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_PN_INFO As New DataTable
        DA.Fill(DT_PN_INFO)
        '已輸入過資料，初始化畫面
        'GridView3.Caption = "trm_sch病患資料"
        'GridView3.DataSource = DT_PN_INFO
        'GridView3.DataBind()
        If DT_PN_INFO.Rows.Count > 0 Then
            ifhaddata = True

            If Not IsPostBack Then

                '病史初始化
                If DT_PN_INFO.Rows(0)("or_pi1").ToString = "1" Then
                    A1.Checked = True
                Else
                    A1.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi2").ToString = "1" Then
                    A2.Checked = True
                Else
                    A2.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi3").ToString = "1" Then
                    A3.Checked = True
                Else
                    A3.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi4").ToString = "1" Then
                    A4.Checked = True
                Else
                    A4.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi5").ToString = "1" Then
                    A5.Checked = True
                Else
                    A5.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi6").ToString = "1" Then
                    A6.Checked = True
                Else
                    A6.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi7").ToString = "1" Then
                    A7.Checked = True
                Else
                    A7.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_pi8").ToString = "1" Then
                    A8.Checked = True
                Else
                    A8.Checked = False
                End If


                '術式初始化
                If DT_PN_INFO.Rows(0)("or_scd1").ToString = "1" Then
                    B1.Checked = True
                Else
                    B1.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_scd2").ToString = "1" Then
                    B2.Checked = True
                Else
                    B2.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_scd3").ToString = "1" Then
                    B3.Checked = True
                Else
                    B3.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_scd4").ToString = "1" Then
                    B4.Checked = True
                Else
                    B4.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_scd5").ToString = "1" Then
                    B5.Checked = True
                Else
                    B5.Checked = False
                End If
                If DT_PN_INFO.Rows(0)("or_scd6").ToString = "1" Then
                    B6.Checked = True
                    B6C.Text = DT_PN_INFO.Rows(0)("or_scd6c").ToString
                Else
                    B6.Checked = False
                End If
            End If
        End If

        Return ifhaddata
    End Function
    'C---------------員工姓名
    Public Function User_Name(Hospcode As String, Empno As String) As String
        Dim ConnStr As String = SELECT_ORACLE(hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = ""
        cmd = String.Format("SELECT * FROM psn WHERE Empno = 'K351' ", Empno)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_EMPNO_NAME As New DataTable
        DA.Fill(DT_EMPNO_NAME)


        '  GridView2.Caption = "操作者姓名"
        'GridView2.DataSource = DT_EMPNO_NAME
        '  GridView2.DataBind()


        Dim Empno_Name As String = ""
        If DT_EMPNO_NAME.Rows.Count > 0 Then
            Empno_Name = DT_EMPNO_NAME.Rows(0)("namec").ToString
        End If
        Return Empno_Name
    End Function
    'D-----------------是否有手術排程資料
    Public Function if_topr(Hospcode As String, Pno As String, opdt As String) As Boolean
        Dim flag As Boolean = False
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        '##############################手術病人資料撈取
        Dim cmd As String = String.Format("SELECT * ")
        cmd += String.Format(" FROM   TOPR_SCH ")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" pno = '{0}'", Pno)

        cmd += String.Format(" AND sch_opdt = To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", opdt)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_PNO As New DataTable
        DA.Fill(DT_OP_PNO)

        '  GridView1.DataSource = DT_OP_PNO
        '  GridView1.DataBind()

        If DT_OP_PNO.Rows.Count > 0 Then
            flag = True
        End If

        Return flag
    End Function
    'E----------------判斷是否排程是否小於當日,是則將BUTTON鎖住
    Private Sub DateLimitfn(opdt As String)
        If (DateTime.Parse(opdt) < Now.Date) Then
            Button1.Enabled = False
            A1.Enabled = False
            A2.Enabled = False
            A3.Enabled = False
            A4.Enabled = False
            A5.Enabled = False
            A6.Enabled = False
            A7.Enabled = False
            A8.Enabled = False

            B1.Enabled = False
            B2.Enabled = False
            B3.Enabled = False
            B4.Enabled = False
            B5.Enabled = False
            B6.Enabled = False
            B6C.Enabled = False

        End If
    End Sub

    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        '測試==================
        Dim HospCode As String = Request.QueryString("hospcode")
        Dim pno As String = Request.QueryString("pno")
        Dim Empno As String = Request.QueryString("empno")
        Dim opdt As String = Request.QueryString("opdt")


        Dim haddata As Boolean = False  '是否已上傳資料
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", Now.Year(), Now.Month(), Now.Day()) ' 2015/07/13 00:00:00
        '======================
        'A----資訊欄資料初始化
        init_info(HospCode, pno, Empno, opdt)
        'B------病史、術式初始化
        haddata = init_pi(HospCode, pno, opdt)

        'C---------------員工姓名
        Dim Empno_Name As String = User_Name(Hospcode, Empno)
        Label_User.Text = Empno_Name
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        'Dim pno As String = Request.QueryString("pno")
        'E----------------判斷是否排程是否小於當日,是則將BUTTON鎖住
        DateLimitfn(opdt)


    End Sub
    '送出資料
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", Now.Year(), Now.Month(), Now.Day()) ' 2015/07/13 00:00:00

        Dim Hospcode As String = Label_hospcode.Text
        Dim Or_Pi1 As String = ""  '年齡超過70歲
        Dim Or_Pi2 As String = ""  '罹患糖尿病或高血壓超過10年以上
        Dim Or_Pi3 As String = ""  '心臟疾病(心股梗塞、心律不整、心臟瓣膜、先天性心臟病)
        Dim Or_Pi4 As String = ""  '洗腎
        Dim Or_Pi5 As String = ""  '肝硬化
        Dim Or_Pi6 As String = ""  '近六個月內中風
        Dim Or_Pi7 As String = ""  '生命徵象不穩或目前住加護病房
        Dim Or_Pi8 As String = ""  '慢性阻塞性肺病或氣喘發作

        Dim OR_Scd1 As String = ""  '心臟手術
        Dim OR_Scd2 As String = ""  '脊椎手術
        Dim OR_Scd3 As String = ""  '腦部手術
        Dim OR_Scd4 As String = ""  '胸腔手術
        Dim OR_Scd5 As String = ""  '肝或胃切除手術
        Dim OR_Scd6 As String = ""  '其它
        Dim OR_Scd6c As String = ""  '(其它)內容
        '病史資料

        Or_Pi1 = IIf(A1.Checked, "1", "0")
        Or_Pi2 = IIf(A2.Checked, "1", "0")
        Or_Pi3 = IIf(A3.Checked, "1", "0")
        Or_Pi4 = IIf(A4.Checked, "1", "0")
        Or_Pi5 = IIf(A5.Checked, "1", "0")
        Or_Pi6 = IIf(A6.Checked, "1", "0")
        Or_Pi7 = IIf(A7.Checked, "1", "0")
        Or_Pi8 = IIf(A8.Checked, "1", "0")
        '術式
        OR_Scd1 = IIf(B1.Checked, "1", "0")
        OR_Scd2 = IIf(B2.Checked, "1", "0")
        OR_Scd3 = IIf(B3.Checked, "1", "0")
        OR_Scd4 = IIf(B4.Checked, "1", "0")
        OR_Scd5 = IIf(B5.Checked, "1", "0")
        OR_Scd6 = IIf(B6.Checked, "1", "0")
        If (B6.Checked) Then
            OR_Scd6c = B6C.Text
        Else
            OR_Scd6c = ""
        End If
        Dim Pno As String = Label_pno.Text
        Dim Bedno As String = Label_Bedno.Text
        Dim Or_User As String = Label_User.Text
        Dim Caseno As Integer = CInt(Label_caseno.Text)
        Dim Opdt As String = FormatDateTime(Label_opdt.Text, DateFormat.ShortDate)
        Dim Empno As String = Label_Empno.Text
        Dim Or_Effect As String = "Y"





        Dim ConnStr As String = SELECT_ORACLE_HC(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        '判斷是否已經上傳過資料
        If (init_pi(Hospcode, Pno, opdt)) Then
            '已上傳過資料,更新資料
            Dim Insert_Update_CMD As String = String.Format("UPDATE  TRM_SCH  SET  ")
            Insert_Update_CMD += String.Format("OR_Udate = to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'), ")
            Insert_Update_CMD += String.Format(" Or_pi1 = '{0}',Or_pi2 = '{1}',Or_pi3 = '{2}',Or_pi4 = '{3}',Or_pi5 = '{4}',Or_pi6 = '{5}',Or_pi7 = '{6}',Or_pi8 = '{7}',  ", Or_Pi1, Or_Pi2, Or_Pi3, Or_Pi4, Or_Pi5, Or_Pi6, Or_Pi7, Or_Pi8)
            Insert_Update_CMD += String.Format(" Or_scd1 = '{0}',Or_scd2 = '{1}',Or_scd3 = '{2}',Or_scd4 = '{3}',Or_scd5 = '{4}',Or_scd6 = '{5}',Or_scd6c = '{6}',", OR_Scd1, OR_Scd2, OR_Scd3, OR_Scd4, OR_Scd5, OR_Scd6, OR_Scd6c)
            Insert_Update_CMD += String.Format(" Or_user = '{0}'", Empno)
            Insert_Update_CMD += String.Format(" WHERE PNO = '{0}' AND opdt='{1}' AND Or_Effect = 'Y'", Pno, Opdt)
            Dim Insert_Update_DC As OleDbCommand = New OleDbCommand(Insert_Update_CMD, Conn)
            Insert_Update_DC.ExecuteNonQuery()

            '   Response.Write(String.Format("<script>alert('{0}')</script>", B6C.Text))

        Else
            '未上傳資料,插入資料
            Dim Insert_Intern_CMD As String = String.Format("INSERT INTO TRM_SCH (Pno,Bedno,Or_User,Caseno,Opdt, Or_Effect,Or_Udate,Or_pi1,Or_pi2,Or_pi3,Or_pi4,Or_pi5,Or_pi6,Or_pi7,Or_pi8 ,OR_Scd1 ,OR_Scd2 ,OR_Scd3 ,OR_Scd4 ,OR_Scd5 ,OR_Scd6 ,OR_Scd6c) VALUES ('{0}','{1}','{2}',{3},to_date('{4}','YYYY/MM/DD HH24:MI:SS'),'{5}',to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'),'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", Pno, Bedno, Empno, Caseno, Opdt, Or_Effect, Or_Pi1, Or_Pi2, Or_Pi3, Or_Pi4, Or_Pi5, Or_Pi6, Or_Pi7, Or_Pi8, OR_Scd1, OR_Scd2, OR_Scd3, OR_Scd4, OR_Scd5, OR_Scd6, OR_Scd6c)
            Dim Insert_Intern_DC As OleDbCommand = New OleDbCommand(Insert_Intern_CMD, Conn)
            Insert_Intern_DC.ExecuteNonQuery()
        End If
        ' Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        ' Dim DT_OP_PNO As New DataTable
        Response.Write("<script language=javascript>alert('資料已儲存!!');location.reload;</script>")

    End Sub


    '判斷是否有病患排程資料
    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Dim Hospcode As String = Request.QueryString("Hospcode")
        Dim pno As String = Request.QueryString("Pno")
        Dim opdt As String = Request.QueryString("opdt").ToString
        Dim Hasdata As Boolean = False
        Hasdata = if_topr(Hospcode, pno, opdt)
        If Not Hasdata Then
            Response.Write(String.Format("<script language='javascript'>alert('病患{0}於{1}無手術排程資料!!');window.close();</script>", pno, opdt))
            'Response.End()
        End If

    End Sub
End Class

