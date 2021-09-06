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
    Protected Function Title_default(ByVal HospCode As String, ByVal WardCode As String, ByVal ConnStr As String) As String
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM NSTBL  WHERE NS='{0}' ", WardCode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataTable
        Dim ContactInfo As String = Nothing
        DA.Fill(DS) '連絡人資料
        If DS.Rows.Count > 0 Then
            Return DS.Rows(0)("nseng")
        End If
    End Function
    'MMH LOGO顯示控制
    Protected Sub Show_LOGO(ByVal Hospcode As String)
        Dim path As String = Nothing
        Select Case Hospcode
            Case 1, 2, 4
                path = "image/CHmmhlogo.jpg"
                LOGO_Image.Width = 240
                LOGO_Image.Height = 50
                LOGO_Image.ImageUrl = path
            Case 3
                path = "image/TTmmhlogo.jpg"
                LOGO_Image.Width = 210
                LOGO_Image.Height = 60
                LOGO_Image.ImageUrl = path

        End Select

    End Sub
    '顯示操作者姓名
    Protected Function Show_User() As String
        Dim ShowText As String = ""
        If Not (Request.Cookies("UserCookie") Is Nothing) Then
            If Not IsPostBack Then
                Dim show As HttpCookie = Request.Cookies("UserCookie")
                ShowText = show.Values("Name")
            End If
        End If
        Return ShowText
    End Function
    '超連結
    Protected Sub Hyperlink_Default(ByVal HospCode As String, ByVal WardCode As String)
        'HyperLink2.NavigateUrl = String.Format("Patients.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink3.NavigateUrl = String.Format("Operation.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink4.NavigateUrl = String.Format("Inout.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink5.NavigateUrl = String.Format("Nurses.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink6.NavigateUrl = String.Format("Doctors.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink7.NavigateUrl = String.Format("Contacts.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink8.NavigateUrl = String.Format("Info.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink1.NavigateUrl = String.Format("Index.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
    End Sub
    '手術排程
    Protected Sub OP_SCH(ByVal Hospcode As String, ByVal Wardcode As String)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        '##############################手術排程資料撈取
        Dim cmd As String = String.Format("SELECT B.Bedns, B.bedno,B.bedpno,I.name, T.sch_opdt ,T.sch_opdr,T.sch_optm ,D.name as DRname,T.sch_opfloor, T.sch_scd2  ")
        cmd += String.Format(" , DECODE(T.sch_actfg,1,'未報到',2,'報到',3,'手術中',4,'完成手術',5,'取消排程',6,'取消報到',7,'恢復室',8,'已離開') sch_actfg ")
        cmd += String.Format(" FROM  BedTBL B ,TOPR_SCH T ,DR D,IDP I")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" B.bedpno(+) = T.pno ")
        cmd += String.Format(" AND B.bedpno = I.pno ")
        'cmd += String.Format(" AND  B.bedcaseno(+) = T. sch_caseno ")
        cmd += String.Format(" AND T.sch_opdr = D.code")
        cmd += String.Format(" AND T.sch_opdt = To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd += String.Format("  AND B.BedNS(+)='{0}' ", Wardcode)
        cmd += String.Format(" AND B.Bedsts(+) = '1' ")
        cmd += String.Format("  AND B.bedroom(+) != '99' ")
        cmd += String.Format(" ORDER BY B.Bedno")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_SCH As New DataTable

        DA.Fill(DT_OP_SCH)
        DT_OP_SCH.Columns.Add("activity")   '新增行動能力欄位

        '##########################################行動能力資料撈取
        Dim cmd1 As String = String.Format("SELECT B.bedsts, B.Bedns, B.Bedno,TS.activity,B.bedcaseno ,T.caseno, T.pno,  T.MAXUdate   ")
        cmd1 += String.Format(" FROM BedTBL B, TPRSheet TS")
        cmd1 += String.Format("      ,(SELECT  T.pno,T.caseno,MAX(T.udate)as MAXUdate  FROM TPRSheet T  WHERE T.Activity is not null   GROUP BY  T.pno,T.caseno) T ")
        cmd1 += String.Format(" WHERE        ")
        cmd1 += String.Format(" TS.pno = T. pno AND ")
        cmd1 += String.Format(" TS.caseno = T.caseno AND ")
        cmd1 += String.Format(" TS.udate = T.maxudate AND ")
        cmd1 += String.Format(" B.Bedpno = T.pno AND ")
        cmd1 += String.Format(" B.bedcaseno = T.caseno  AND ")
        cmd1 += String.Format(" B.bedns = '{0}' AND ", Wardcode)
        cmd1 += String.Format(" (B.bedsts = '1' OR (B.bedsts = '2' AND B.udendtime > to_date('{0}','YYYY/MM/DD HH24:MI:SS') )) AND ", Nowdate)
        cmd1 += String.Format(" Bedroom != '99' ")
        cmd1 += String.Format(" AND TS.Activity is not null ")
        cmd1 += String.Format(" order by B.bedno")

        Dim DA1 As OleDbDataAdapter = New OleDbDataAdapter(cmd1, Conn)
        Dim DT1 As New DataTable
        DA1.Fill(DT1)
        'GridView7.Caption = "   行動能力資料表"
        'GridView7.DataSource = DT1
        'GridView7.DataBind()
        '###########################將病床行動能力資料加入手術排程列表中
        For j = 0 To DT_OP_SCH.Rows.Count - 1

            Dim SQL_S = String.Format("BEDNO = '{0}'", DT_OP_SCH.Rows(j)("bedno").ToString)

            DT1.DefaultView.RowFilter = SQL_S
            Dim DT_Temp As DataTable = DT1.DefaultView.ToTable

            If DT_Temp.Rows.Count > 0 Then
                DT_OP_SCH.Rows(j)("ACTIVITY") = DT_Temp.Rows(0)("activity").ToString
            End If
            'GridView1.Caption = "手術排程列表"
            'GridView1.DataSource = DT_OP_SCH
            'GridView1.DataBind()
        Next

        '####################將資料放入最後顯示資料表
        Dim Operation_final As String = ""
        For i = 0 To DT_OP_SCH.Rows.Count - 1
            '床號
            Dim bedno As String = Right(DT_OP_SCH.Rows(i)("bedno").ToString, 3)
            '病歷號碼
            Dim pno As String = DT_OP_SCH.Rows(i)("bedpno").ToString
            '病患姓名
            Dim name As String = DT_OP_SCH.Rows(i)("name").ToString
            '手術日期
            Dim sch_date As Date = DT_OP_SCH.Rows(i)("sch_opdt")
            Dim sch_opdt As String = String.Format("{0}/{1}", sch_date.Month.ToString, sch_date.Day.ToString)
            '預定報到時間
            Dim sch_optm As String = DT_OP_SCH.Rows(i)("sch_optm")
            '醫生員編,姓名
            Dim dr As String = DT_OP_SCH.Rows(i)("sch_opdr").ToString & " <br>" & DT_OP_SCH.Rows(i)("Drname").ToString
            '術式
            Dim sch_scd2 As String = DT_OP_SCH.Rows(i)("sch_scd2").ToString
            '狀態
            Dim sch_opactfg As String = DT_OP_SCH.Rows(i)("sch_actfg").ToString
            '行動能力
            Dim activity As String = DT_OP_SCH.Rows(i)("activity").ToString

            Operation_final += String.Format("<tr class =""Table_tr_style1"">")
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_opdt)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_optm)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", bedno)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", pno)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", name)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_scd2)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", dr)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_opactfg)
            Operation_final += String.Format("</tr>")


        Next
        If DT_OP_SCH.Rows.Count = 0 Then
            Operation_List.Text = "<tr class =""Table_tr_style1""><td class=""Table_td_style"" colspan=""7"">目前沒有資料!! </td></tr>"
        Else
            Operation_List.Text = Operation_final
        End If

    End Sub
    '生理排程
    Protected Sub PHY_SCH(ByVal Hospcode As String, ByVal Wardcode As String)
        '#####################生理排程資料撈取
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        Dim Nowdateadd As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.AddDays(1).Day)
        Dim cmd As String = String.Format("SELECT P.pno,I.name,  B.bedns, B.bedsts, B.bedroom,B.bedno,P.schtime,P.itemno,O.name as Itemname, R.loc, P.schseq   ")

        cmd += String.Format(" FROM  BedTBL B ,PHYEXEC P, IDP I ,ODRC O, PHYROOM R")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" B.bedpno = P.pno ")
        cmd += String.Format(" AND B.bedpno = I.pno ")
        cmd += String.Format(" AND P.Itemno = O.code ")
        cmd += String.Format(" AND P.csdp = R.csdp(+ ) ")
        ' cmd += String.Format(" AND B.bedcaseno = P.caseno ")
        cmd += String.Format(" AND  P.effect = 'Y' ")
        'cmd += String.Format(" AND  P.Endtime is null ") '將已做過檢查的排除
        '    cmd += String.Format(" AND T.sch_opdr = D.code")
        cmd += String.Format(" AND P.schtime >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd += String.Format(" AND P.schtime <= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdateadd)
        cmd += String.Format("  AND B.BedNS='{0}'", Wardcode)
        cmd += String.Format(" AND B.Bedsts = '1'")
        cmd += String.Format("  AND B.bedroom != '99'")
        cmd += String.Format(" ORDER BY B.Bedno")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_PHY_SCH As New DataTable
        DA.Fill(DT_PHY_SCH)

        ' GridView2.Caption = "檢查排程列表"
        'GridView2.DataSource = DT_PHY_SCH
        'GridView2.DataBind()

        '全部排程列表生成

        Dim PHY_INFO As DataTable = New DataTable
        PHY_INFO.Columns.Add("PHY_DATE") '排程日期
        PHY_INFO.Columns.Add("PHY_TIME") '排程時間
        PHY_INFO.Columns.Add("PHY_BEDNO") '床號
        PHY_INFO.Columns.Add("PHY_PNO") '病歷號
        PHY_INFO.Columns.Add("PHY_NAME") '姓名
        PHY_INFO.Columns.Add("PHY_LOCATION") '地點
        PHY_INFO.Columns.Add("PHY_DETAIL") ' 檢查項目
        PHY_INFO.Columns.Add("Bed_Qt", Type.GetType("System.Int16")) '將欄位宣告為Int,排序時才不會出錯

        '塞入生理檢查資料
        For i = 0 To DT_PHY_SCH.Rows.Count - 1
            Dim New_Row As DataRow = PHY_INFO.NewRow
            Dim DATE_TEMP As DateTime = FormatDateTime(DT_PHY_SCH.Rows(i)("schtime").ToString, DateFormat.ShortDate)
            Dim TIME_TEMP As DateTime = FormatDateTime(DT_PHY_SCH.Rows(i)("schtime").ToString, DateFormat.ShortTime)
            New_Row("PHY_DATE") = DATE_TEMP.Month & "/" & DATE_TEMP.Day
            '  New_Row("PHY_TIME") = DT_PHY_SCH.Rows(i)("schtime").ToString
            New_Row("PHY_TIME") = Format(TIME_TEMP.Hour, "00") & ":" & Format(TIME_TEMP.Minute, "00")
            New_Row("PHY_BEDNO") = Right(DT_PHY_SCH.Rows(i)("BEDNO").ToString, 3)
            New_Row("PHY_PNO") = DT_PHY_SCH.Rows(i)("PNO").ToString
            New_Row("PHY_NAME") = DT_PHY_SCH.Rows(i)("NAME").ToString
            New_Row("PHY_LOCATION") = DT_PHY_SCH.Rows(i)("LOC").ToString
            New_Row("PHY_DETAIL") = DT_PHY_SCH.Rows(i)("ITEMNAME").ToString
            PHY_INFO.Rows.Add(New_Row)
        Next

        '#####################血液透析資料撈取
        Dim cmd_hem As String = String.Format("SELECT B.bedns, B.bedno,B.bedpno,I.name,H.hemdate,H.hemseg ")
        cmd_hem += String.Format(" FROM BEDTBL B, HEMSCH H, IDP I ")
        cmd_hem += String.Format(" WHERE B.bedpno = H.pno")
        cmd_hem += String.Format(" AND I.pno = B.bedpno")
        cmd_hem += String.Format(" AND B.bedns = '{0}' ", Wardcode)
        cmd_hem += String.Format(" AND H.hemdate = To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd_hem += String.Format(" AND H.effect = 'Y'")
        cmd_hem += String.Format(" AND B.bedsts = 1")
        Dim DA_hem As OleDbDataAdapter = New OleDbDataAdapter(cmd_hem, Conn)
        Dim DT_HEM As New DataTable
        DA_hem.Fill(DT_HEM)
        '塞入血液透析列表

        For i = 0 To DT_HEM.Rows.Count - 1
            Dim New_Row As DataRow = PHY_INFO.NewRow

            New_Row("PHY_DATE") = Now.Month.ToString & "/" & Now.Day.ToString
            '  New_Row("PHY_TIME") = DT_PHY_SCH.Rows(i)("schtime").ToString
            New_Row("PHY_TIME") = ""
            New_Row("PHY_BEDNO") = Right(DT_HEM.Rows(i)("BEDNO").ToString, 3)
            New_Row("PHY_PNO") = DT_HEM.Rows(i)("BEDPNO").ToString
            New_Row("PHY_NAME") = DT_HEM.Rows(i)("NAME").ToString
            New_Row("PHY_LOCATION") = "血液透析"
            New_Row("PHY_DETAIL") = "班別：" & DT_HEM.Rows(i)("HEMSEG").ToString
            PHY_INFO.Rows.Add(New_Row)
        Next
        'GridView4.Caption = "血液透析列表"
        'GridView4.DataSource = DT_HEM
        'GridView4.DataBind()

        '#######################放射科檢查資料撈取 
        Dim cmd_XRY As String = String.Format("SELECT B.Bedpno, B.bedns, B.bedno,I.name, N.code,N.ddate, N.idate, O.name as Detail")
        cmd_XRY += String.Format(" FROM NRTEMP N,BEDTBL B ,ODRC O,IDP I ")
        cmd_XRY += String.Format(" WHERE  B.bedns = '{0}'", Wardcode)
        cmd_XRY += String.Format(" AND  B.bedpno = N.pno")
        cmd_XRY += String.Format(" AND N.code = O.mcode")
        cmd_XRY += String.Format(" AND I.pno = B.bedpno")
        cmd_XRY += String.Format(" AND  ((N.type = 'XRY1' AND N.io = 'I' AND N.proccnt = 0) OR (N.type = 'XRY3') OR (N.type = 'XRY2') OR (N.type = 'XRY4')OR (N.type = 'XRY'))")
        cmd_XRY += String.Format(" AND B.bedsts = 1")
        '   cmd_XRY += String.Format(" AND  O.stype = '12' ")
        cmd_XRY += String.Format(" AND N.idate >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd_XRY += String.Format(" AND N.idate < To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdateadd)
        ' cmd_XRY += String.Format(" AND N.ddate = '1060224' ")
        'cmd_hem += String.Format(" AND N.idate >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        Dim DA_XRY As OleDbDataAdapter = New OleDbDataAdapter(cmd_XRY, Conn)
        Dim DT_XRY As New DataTable
        DA_XRY.Fill(DT_XRY)
        'GridView6.Caption = "放射科列表"
        'GridView6.DataSource = DT_XRY
        'GridView6.DataBind()
        '塞入放射科檢查資料

        For i = 0 To DT_XRY.Rows.Count - 1
            Dim New_Row As DataRow = PHY_INFO.NewRow

            New_Row("PHY_DATE") = Now.Month.ToString & "/" & Now.Day.ToString

            New_Row("PHY_TIME") = Format(Date.Parse(DT_XRY.Rows(i)("idate").ToString).Hour, "00") & ":" & Format(Date.Parse(DT_XRY.Rows(i)("idate").ToString).Minute, "00")
            New_Row("PHY_BEDNO") = Right(DT_XRY.Rows(i)("BEDNO").ToString, 3)
            New_Row("PHY_PNO") = DT_XRY.Rows(i)("BEDPNO").ToString
            New_Row("PHY_NAME") = DT_XRY.Rows(i)("name").ToString
            New_Row("PHY_LOCATION") = "放射科"
            New_Row("PHY_DETAIL") = DT_XRY.Rows(i)("detail").ToString
            PHY_INFO.Rows.Add(New_Row)
        Next

        '####################核醫科-核磁共振造影資料撈取
        Dim cmd_IM As String = String.Format("SELECT B.bedns, B.bedno,B.bedpno, M.imdate, M.NAME,M.schexm as time, M.text,O.name, O.name as detail")
        cmd_IM += String.Format(" FROM BEDTBL B, IMREP M,ODRC O")
        cmd_IM += String.Format(" WHERE B.bedpno = M.pno")
        cmd_IM += String.Format(" AND M.icode =  SUBSTR(O.mcode,3)")
        cmd_IM += String.Format(" AND B.bedns = '{0}' ", Wardcode)
        cmd_IM += String.Format(" AND  B.bedsts = 1")
        cmd_IM += String.Format(" AND  O.mcode LIKE '63%' ")
        'cmd_IM += String.Format(" AND  O.type  'EXA1' ")
        '   cmd_IM += String.Format(" AND  O.stype = '12' ")
        cmd_IM += String.Format(" AND M.imdate = To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)

        Dim DA_IM As OleDbDataAdapter = New OleDbDataAdapter(cmd_IM, Conn)
        Dim DT_IM As New DataTable
        DA_IM.Fill(DT_IM)
        'GridView5.Caption = "核磁共振列表"
        'GridView5.DataSource = DT_IM
        'GridView5.DataBind()
        '塞入核醫科-核磁共振造影資料
        For i = 0 To DT_IM.Rows.Count - 1
            Dim New_Row As DataRow = PHY_INFO.NewRow

            New_Row("PHY_DATE") = Now.Month.ToString & "/" & Now.Day.ToString
            '  New_Row("PHY_TIME") = DT_PHY_SCH.Rows(i)("schtime").ToString
            New_Row("PHY_TIME") = DT_IM.Rows(i)("TIME").ToString
            New_Row("PHY_BEDNO") = Right(DT_IM.Rows(i)("BEDNO").ToString, 3)
            New_Row("PHY_PNO") = DT_IM.Rows(i)("BEDPNO").ToString
            New_Row("PHY_NAME") = DT_IM.Rows(i)("name").ToString
            New_Row("PHY_LOCATION") = "核醫科"
            New_Row("PHY_DETAIL") = DT_IM.Rows(i)("detail").ToString
            PHY_INFO.Rows.Add(New_Row)
        Next




        'GridView3.Caption = "生理檢查最終列表顯示"
        'GridView3.DataSource = PHY_INFO
        'GridView3.DataBind()
        Dim Phy_final As String = ""
        PHY_INFO.DefaultView.Sort = "PHY_BEDNO"
        PHY_INFO = PHY_INFO.DefaultView.ToTable
        For j = 0 To PHY_INFO.Rows.Count - 1
            Phy_final += String.Format("<tr class =""Table_tr_style1"">")
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_DATE").ToString)
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_TIME").ToString)
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_BEDNO").ToString)
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_PNO").ToString)
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_NAME").ToString)
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_LOCATION").ToString)
            Phy_final += String.Format("    <td class=""Table_td_style"">{0}</td>", PHY_INFO.Rows(j)("PHY_DETAIL").ToString)
            Phy_final += String.Format("</tr>")
        Next
        If PHY_INFO.Rows.Count = 0 Then
            Phy_LIST.Text = "<tr class =""Table_tr_style1""><td class=""Table_td_style"" colspan=""7"">目前沒有資料!! </td></tr>"
        Else
            Phy_LIST.Text = Phy_final
        End If


        '新增資料列表
        '   Dim New_Row As DataRow = PHY_INFO.NewRow
        '  PHY_INFO.Rows.Add(New_Row)
    End Sub
    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        'MMH LOGO顯示控制
        Show_LOGO(HospCode)
        '超連結設定
        Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        'Update_Time_Label.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        User_Label.Text = Show_User()
        '手術排程
        OP_SCH(HospCode, WardCode)
        '檢查排程
        PHY_SCH(HospCode, WardCode)

    End Sub

End Class
