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
        ' GridView1.Caption = "病患資料"
        ' GridView1.DataSource = DT_OP_PNO
        '  GridView1.DataBind()
        '########################################################
        Dim Dr_Code As String = ""
        Dim Dr_Name As String = ""
        Dim Patient_pno As String = ""
        Dim Patient_Name As String = ""
        Dim Bedno As String = ""
        Dim Caseno As Integer = 0


        If DT_OP_PNO.Rows.Count > 0 Then
            Patient_Name = DT_OP_PNO.Rows(0)("name").ToString
            Patient_pno = DT_OP_PNO.Rows(0)("pno").ToString
            Dr_Code = DT_OP_PNO.Rows(0)("Sch_Opdr").ToString
            Dr_Name = DT_OP_PNO.Rows(0)("Drname").ToString
            Bedno = DT_OP_PNO.Rows(0)("Sch_bedno").ToString
            If Not IsDBNull(DT_OP_PNO.Rows(0)("Sch_Caseno")) Then
                Caseno = DT_OP_PNO.Rows(0)("Sch_Caseno")
            End If

            Opdt = DT_OP_PNO.Rows(0)("Sch_Opdt")
        End If
        Label_hospcode.Text = Request.QueryString("hospcode")
        Label_name.Text = Patient_Name
        Label_pno.Text = Patient_pno
        Label_Drcode.Text = Dr_Code
        Label_Drname.Text = Dr_Name
        Label_Bedno.Text = Bedno
        Label_caseno.Text = Caseno
        Label_Empno.Text = Empno
        Label_opdt.Text = Opdt


    End Function
    'B------病史、術式初始化
    Public Function init_pi(Hospcode As String, pno As String, nowdate As String) As Boolean

        '判斷是否已經輸入過資料
        Dim ifhaddata As Boolean = False
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = ""
        cmd += String.Format(" SELECT pno,opdt,or_effect,or_pi1, or_pi2, or_pi3, or_pi4, or_pi5 ,or_pi6, or_pi7, or_pi8, or_scd1, or_scd2, or_scd3, or_scd4, or_scd5, or_scd6, or_scd6c,Ar_ASA,Ar_AR,Ar_suga1,Ar_suga2,Ar_suga3,Ar_suga4,Ar_suga5,Ar_sugac5,Ar_sugb1,Ar_sugb2,Ar_sugb3,Ar_sugbc3,Ar_sugc1,Ar_Rem,Tr_prp1,Tr_prp2,Tr_prp3,Tr_prp4,Tr_prpc4")
        cmd += String.Format(" FROM trm_SCH  ")
        cmd += String.Format(" WHERE  pno = {0} AND opdt = To_Date('{1}','yyyy/MM/DD HH24:MI:SS')  AND or_effect= 'Y' ", pno, nowdate)

        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_PN_INFO As New DataTable
        DA.Fill(DT_PN_INFO)
        '已輸入過資料，初始化畫面
        'GridView3.Caption = "trm_sch病患資料"
        'GridView3.DataSource = DT_PN_INFO
        'GridView3.DataBind()
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

            '一.ASA

            DropDownList1.SelectedValue = DT_PN_INFO.Rows(0)("ar_ASA").ToString()

            '二.麻醫回覆
            DR_TextArea.Text = DT_PN_INFO.Rows(0)("ar_ar").ToString()


            '三.麻醉科回覆
            '---------請會診
            If DT_PN_INFO.Rows(0)("ar_suga1").ToString = "1" Then
                C1.Checked = True
            Else
                C1.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("ar_suga2").ToString = "1" Then
                C2.Checked = True
            Else
                C2.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("ar_suga3").ToString = "1" Then
                C3.Checked = True
            Else
                C3.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("ar_suga4").ToString = "1" Then
                C4.Checked = True
            Else
                C4.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("ar_suga5").ToString = "1" Then
                C5.Checked = True
            Else
                C5.Checked = False
            End If
            '----------檢驗檢查
            If DT_PN_INFO.Rows(0)("ar_sugb1").ToString = "1" Then
                D1.Checked = True
            Else
                D1.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("ar_sugb2").ToString = "1" Then
                D2.Checked = True
            Else
                D2.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("ar_sugb3").ToString = "1" Then
                D3.Checked = True
            Else
                D3.Checked = False
            End If
            '---------術後轉加護病房

            RadioButtonList1.SelectedValue = DT_PN_INFO.Rows(0)("Ar_sugc1").ToString

            '四備註
            Remark_TextArea.Text = DT_PN_INFO.Rows(0)("Ar_Rem").ToString
            '術前備物
            If DT_PN_INFO.Rows(0)("Tr_prp1").ToString = "1" Then
                G1.Checked = True
            Else
                G1.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("Tr_prp2").ToString = "1" Then
                G2.Checked = True
            Else
                G2.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("Tr_prp3").ToString = "1" Then
                G3.Checked = True
            Else
                G3.Checked = False
            End If
            If DT_PN_INFO.Rows(0)("Tr_prp4").ToString = "1" Then
                G4.Checked = True
                G4C.Text = DT_PN_INFO.Rows(0)("Tr_prpc4").ToString
            Else
                G4.Checked = False
                G4C.Text = ""
            End If
        End If



        Return ifhaddata
    End Function
    '判斷病患是否已離開、

    '權限控管
    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        If Not IsPostBack Then
            If Request.Cookies("UserCookie") Is Nothing Then
                Response.Write("沒有使用權限!!")
                Response.End()
            Else
                Dim User_info As HttpCookie = Request.Cookies("UserCookie")
                Dim Empno As String = User_info.Values("empno")
                Dim Secu As Integer = User_info.Values("secu")
                If Secu < 5 Then
                    Response.Write("沒有使用權限!!!!")
                    Response.End()
                End If
            End If
        End If
    End Sub
    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim User_info As HttpCookie = Request.Cookies("UserCookie")
        Dim HospCode As String = Request.QueryString("hospcode")
        Dim pno As String = Request.QueryString("pno")
        Dim opdt As Date = Request.QueryString("opdt")
        Dim Empno As String = User_info.Values("name")
        Dim haddata As Boolean = False  '是否已上傳資料

        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", Now.Year(), Now.Month(), Now.Day()) ' 2015/07/13 00:00:00
        'Dim SerchDate As String = Request.QueryString("opdt")

        'A----資訊欄資料初始化
        init_info(HospCode, pno, Empno, opdt)
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        'B------病史、術式初始化
        haddata = init_pi(HospCode, pno, opdt)
        '員工姓名
        Dim Empno_Name As String = User_info.Values("name")
        'D------------------DropdownList初始化
        Label_User.Text = Empno_Name

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Hospcode As String = Label_hospcode.Text
        Dim Pno As String = Label_pno.Text
        Dim User_info As HttpCookie = Request.Cookies("UserCookie")
        Dim Empno As String = User_info.Values("empno")
        Dim Opdt As String = FormatDateTime(Label_opdt.Text, DateFormat.ShortDate)
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", Now.Year(), Now.Month(), Now.Day()) ' 2015/07/13 00:00:00
        'ASA 分類
        Dim Ar_ASA As String = ""
        '麻醫回覆內容
        Dim Ar_Ar As String = ""
        '請會診：
        Dim Ar_Suga1 As String = ""
        Dim Ar_Suga2 As String = ""
        Dim Ar_Suga3 As String = ""
        Dim Ar_Suga4 As String = ""
        Dim Ar_Suga5 As String = ""
        Dim Ar_Sugac5 As String = ""
        '檢驗、檢查
        Dim Ar_Sugb1 As String = ""
        Dim Ar_Sugb2 As String = ""
        Dim Ar_Sugb3 As String = ""

        '術後轉加護病房
        Dim Ar_Sugc1 As String = ""
        '麻醫備註
        Dim Ar_Rem As String = ""

        '術前備物
        Dim Tr_Prp1 As String = ""
        Dim Tr_Prp2 As String = ""
        Dim Tr_Prp3 As String = ""
        Dim Tr_Prp4 As String = ""
        Dim Tr_Prpc4 As String = ""



        Ar_ASA = DropDownList1.SelectedValue
        Ar_Ar = DR_TextArea.Text

        Ar_Suga1 = IIf(C1.Checked, "1", "0")
        Ar_Suga2 = IIf(C2.Checked, "1", "0")
        Ar_Suga3 = IIf(C3.Checked, "1", "0")
        Ar_Suga4 = IIf(C4.Checked, "1", "0")
        Ar_Suga5 = IIf(C5.Checked, "1", "0")


        Ar_Sugb1 = IIf(D1.Checked, "1", "0")
        Ar_Sugb2 = IIf(D2.Checked, "1", "0")
        Ar_Sugb3 = IIf(D3.Checked, "1", "0")

        Ar_Sugc1 = RadioButtonList1.SelectedValue
        Ar_Rem = Remark_TextArea.Text

        Tr_Prp1 = IIf(G1.Checked, "1", "0")
        Tr_Prp2 = IIf(G2.Checked, "1", "0")
        Tr_Prp3 = IIf(G3.Checked, "1", "0")
        Tr_Prp4 = IIf(G4.Checked, "1", "0")
        Tr_Prpc4 = G4C.Text

        Dim ConnStr As String = SELECT_ORACLE_HC(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()

        Dim Insert_Update_CMD As String = String.Format("UPDATE  TRM_SCH  SET  ")
        Insert_Update_CMD += String.Format(" Ar_Udate = to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'),Ar_Effect='Y', ")
        Insert_Update_CMD += String.Format(" Ar_User='{0}',", Empno)
        Insert_Update_CMD += String.Format(" Ar_ASA='{0}',Ar_AR='{1}',", Ar_ASA, Ar_Ar)
        Insert_Update_CMD += String.Format(" Ar_suga1='{0}',Ar_suga2='{1}',Ar_suga3='{2}',Ar_suga4='{3}',Ar_suga5='{4}',", Ar_Suga1, Ar_Suga2, Ar_Suga3, Ar_Suga4, Ar_Suga5)
        Insert_Update_CMD += String.Format(" Ar_sugb1='{0}',Ar_sugb2='{1}',Ar_sugb3='{2}', ", Ar_Sugb1, Ar_Sugb2, Ar_Sugb3)
        Insert_Update_CMD += String.Format(" Ar_sugc1='{0}',", Ar_Sugc1)
        Insert_Update_CMD += String.Format(" Ar_Rem='{0}',", Ar_Rem)
        Insert_Update_CMD += String.Format(" Tr_Prp1='{0}',Tr_Prp2='{1}',Tr_Prp3='{2}',Tr_Prp4='{3}',Tr_Prpc4='{4}' ", Tr_Prp1, Tr_Prp2, Tr_Prp3, Tr_Prp4, Tr_Prpc4)
        Insert_Update_CMD += String.Format(" WHERE PNO = '{0}' AND opdt='{1}'", Pno, Opdt)
        Dim Insert_Update_DC As OleDbCommand = New OleDbCommand(Insert_Update_CMD, Conn)
        Insert_Update_DC.ExecuteNonQuery()

        Response.Write("<script language=javascript>alert('資料已儲存!!');location.reload;</script>")
    End Sub

End Class
