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
    '計算年齡
    Public Function Age_Count(ByVal BirthDate As String, ByVal NowDateTime As DateTime) As Integer

        Dim BirthS As String = BirthDate
        Dim birth As DateTime = FormatDateTime(BirthS, DateFormat.ShortDate)
        Dim YearNum As Integer = DateDiff("yyyy", birth, NowDateTime)
        Dim MonthNum As Integer = DateDiff("M", birth, NowDateTime)
        Dim DayNum As Integer = DateDiff("d", birth, NowDateTime)
        Dim Birth_m As Integer = birth.Month
        Dim now_m As Integer = NowDateTime.Month
        Dim Birth_d As Integer = birth.Day
        Dim Now_d As Integer = NowDateTime.Day
        Dim Age As Integer = DateDiff("yyyy", birth, NowDateTime)
        If now_m < Birth_m Then
            Age = Age - 1
        End If
        If now_m = Birth_m Then
            If Now_d < Birth_d Then
                Age = Age - 1
            End If
        End If

        Return Age
    End Function

    'A-----------手術病人列表評估-初始值
    Protected Sub OP_SCH(ByVal Hospcode As String, ByVal Wardcode As String)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = date1.ToString("yyyy/MM/dd 00:00:00") ' 2015/07/13 00:00:00
        '##############################手術排程資料撈取
        Dim cmd As String = String.Format("SELECT T.pno ,T.sch_bedno, I.name,I.birth ,TRM.ar_asa, T.sch_opdt, T.sch_dept ,T.sch_opdr,D.name as DRname,T.sch_opfloor, T.sch_scd2, TRM.ar_effect  ")
        cmd += String.Format(" , DECODE(T.sch_actfg,1,'未報到',2,'報到',3,'手術中',4,'完成手術',5,'取消排程',6,'取消報到',7,'恢復室',8,'已離開') sch_actfg ")
        cmd += String.Format(" FROM  TOPR_SCH T ,DR D,IDP I,TRM_SCH TRM")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" T.pno = I.pno ")
        cmd += String.Format(" AND TRM.Or_effect = 'Y'")
        cmd += String.Format(" AND TRM.pno  (+)= T.pno ")
        cmd += String.Format(" AND T.sch_opdr = D.code")
        cmd += String.Format(" AND TRM.opdt (+)= T.sch_opdt ")
        cmd += String.Format(" AND T.sch_actfg NOT IN  ('5','6','8') ") '排除5取消排程6取消報到8已離開
        cmd += String.Format(" AND T.sch_opdt >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd += String.Format(" ORDER BY T.sch_opdt,T.pno ")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_SCH As New DataTable
        DA.Fill(DT_OP_SCH)


        DT_OP_SCH.Columns.Add("activity")   '新增行動能力欄位

        'GridView1.Caption = "手術病患列表"
        'GridView1.DataSource = DT_OP_SCH
        'GridView1.DataBind()



        '####################將資料放入最後顯示資料表
        Dim Operation_final As String = ""
        For i = 0 To DT_OP_SCH.Rows.Count - 1
            '床號()
            Dim bedno As String = DT_OP_SCH.Rows(i)("sch_bedno").ToString
            '病歷號碼
            Dim pno As String = DT_OP_SCH.Rows(i)("pno").ToString
            '病患姓名
            Dim name As String = DT_OP_SCH.Rows(i)("name").ToString
            '年齡
            Dim birth As DateTime = DT_OP_SCH.Rows(i)("birth").ToString
            Dim Age As Integer = Age_Count(birth.ToString("yyyy/MM/dd"), Now)
            'ASA
            Dim ASA As String = DT_OP_SCH.Rows(i)("ar_asa").ToString
            
            '手術日期
            Dim sch_date As Date = DT_OP_SCH.Rows(i)("sch_opdt")
            Dim sch_opdt As String = String.Format("{0}/{1}", sch_date.Month.ToString, sch_date.Day.ToString)
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
            Operation_final += String.Format("    <td class=""Table_td_style""><button style=""width:50px;height:40px; font-size:16px;background-color:#95cfcf;""  type =""button"" onclick=""window.open('TRM-DR.aspx?Hospcode={0}&pno={1}&opdt={2}')"">回覆</butto</td>", Hospcode, pno, sch_date.ToShortDateString)

            '判斷麻醫是否有選擇ASA
            If Not IsDBNull(DT_OP_SCH.Rows(i)("ar_asa")) Then
                Select Case ASA
                    Case "P1", "P2"
                        Operation_final += String.Format("    <td class=""Table_td_style""><img alt="""" src=""image/light.png"" style=""width:30px;height:30px;""/></td>")
                    Case "P3", "P4", "P5", "P6"
                        Operation_final += String.Format("    <td class=""Table_td_style""><img alt="""" src=""image/heavy.png"" style=""width:30px;height:30px;""/></td>")
                End Select
            Else
                Operation_final += String.Format("    <td class=""Table_td_style""></td>")
            End If
            Operation_final += String.Format("   <td class=""Table_td_style""></td>")
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", bedno)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", pno)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", name)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", Age)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_scd2)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", dr)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_opactfg)
            Operation_final += String.Format("</tr>")


        Next
        If DT_OP_SCH.Rows.Count = 0 Then
            Operation_List.Text = "<tr class =""Table_tr_style1""><td class=""Table_td_style"" colspan=""11"">目前沒有資料!! </td></tr>"
        Else
            Operation_List.Text = Operation_final
        End If

    End Sub
    'B-----------醫師下拉選單列表列表
    Protected Sub DR_Drop(Hospcode As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        '##############################手術排程資料撈取
        Dim cmd As String = String.Format("SELECT distinct T.sch_opdr as Drcode,D.name as DRname,T.sch_opdr || '               '||D.name as drtext ")
        cmd += String.Format(" FROM  TOPR_SCH T ,DR D")
        cmd += String.Format(" WHERE ")
        cmd += String.Format("  T.sch_opdr = D.code")
        cmd += String.Format(" AND T.sch_actfg NOT IN  ('5','6','8') ") '排除5取消排程6取消報到8已離開
        cmd += String.Format(" AND T.sch_opdt >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd += String.Format(" ORDER BY drcode")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_SCH As New DataTable
        DA.Fill(DT_OP_SCH)

        DropDownList_DR.DataValueField = "Drcode"
        DropDownList_DR.DataTextField = "Drtext"
        DropDownList_DR.DataSource = DT_OP_SCH.DefaultView
        DropDownList_DR.DataBind()



        'GridView1.Caption = "醫師下拉選單列表"
        'GridView1.DataSource = DT_OP_SCH
        'GridView1.DataBind()
    End Sub
    'C-----------科別列表
    Protected Sub Dept_Drop(Hospcode As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        '##############################手術排程資料撈取
        Dim cmd As String = String.Format("SELECT distinct T.sch_dept,D.Cname1,D.cname2, T.sch_dept ||'        '||Cname1 as DeptText ")
        cmd += String.Format(" FROM  TOPR_SCH T ,Dept D")
        cmd += String.Format(" WHERE ")
        cmd += String.Format("  T.sch_dept = D.code")
        cmd += String.Format(" AND T.sch_actfg NOT IN  ('5','6','8') ") '排除5取消排程6取消報到8已離開
        cmd += String.Format(" AND T.sch_opdt >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", Nowdate)
        cmd += String.Format(" ORDER BY T.sch_dept")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_SCH As New DataTable
        DA.Fill(DT_OP_SCH)

        DropDownList_dept.DataValueField = "sch_dept"
        DropDownList_dept.DataTextField = "DeptText"
        DropDownList_dept.DataSource = DT_OP_SCH.DefaultView
        DropDownList_dept.DataBind()

    End Sub
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

        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        Dim User_info As HttpCookie = Request.Cookies("UserCookie")
        'Response.Write(User_info.Values("empno"))
        If Not IsPostBack Then
            'A-----------手術病人列表
            OP_SCH(HospCode, WardCode)
            'B-----------醫師下拉選單列表列表
            DR_Drop(HospCode)
            'C-----------科別列表
            Dept_Drop(HospCode)
            '操作者姓名
            User_Label.Text = User_info.Values("name")
        End If

        


    End Sub

    
    '查詢
    Protected Sub ButtonSerch_Click(sender As Object, e As EventArgs) Handles ButtonSerch.Click
        Dim Hospcode As String = Request.QueryString("Hospcode")
        Dim dept As String = DropDownList_dept.SelectedValue
        Dim DRcode As String = DropDownList_DR.SelectedValue
        Dim Re As String = DropDownList_Re.SelectedValue
        Dim Handover As Boolean = CheckBoxHandOver.Checked
        Dim Leave As Boolean = CheckBoxLeave.Checked
        Dim empno As String = Request.QueryString("empno")

        '搜尋開始時間
        Dim nowdate As Date = Now
        Dim nums As Integer = 7
        Dim SerchDateB As String = nowdate.ToString("yyyy/MM/dd 00:00:00")
        If Not String.IsNullOrEmpty(datepickerB.Text) Then
            SerchDateB = datepickerB.Text
            nums = 0
        End If
        Dim tempdate As Date = FormatDateTime(SerchDateB, DateFormat.ShortDate)
        '搜尋結束時間
        Dim SerchDateE As String = tempdate.AddDays(nums).ToString("yyyy/MM/dd 00:00:00")
        If Not String.IsNullOrEmpty(datepickerE.Text) Then
            SerchDateE = datepickerE.Text
        End If


        Dim SQLPlus As String = ""
        Dim AddDateNums As Integer = 0
        '科別
        If dept <> "0" Then
            SQLPlus += String.Format(" AND T.sch_dept = '{0}'", dept)
        End If
        '醫師
        If DRcode <> "0" Then
            SQLPlus += String.Format(" AND T.sch_opdr = '{0}'", DRcode)
        End If
        '回覆狀況
        Select Case Re
            Case "N"
                SQLPlus += "AND TRM.Ar_Effect is  NULL"
            Case "Y"
                SQLPlus += String.Format(" AND TRM.Ar_Effect = '{0}'", Re)
        End Select
        '是否顯示交班
        If (Handover) Then
        Else
            SQLPlus += " AND TRM.Or_effect = 'Y'"
        End If
        '是否顯示已離開
        If (Leave) Then
            SQLPlus += " AND T.sch_actfg NOT IN  ('5','6') "
        Else
            SQLPlus += " AND T.sch_actfg NOT IN  ('5','6','8') "
        End If


        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now

        '##############################手術排程資料撈取
        Dim cmd As String = String.Format("SELECT T.pno ,T.sch_bedno, I.name, I.birth,TRM.ar_asa, T.sch_opdt, T.sch_dept ,T.sch_opdr,D.name as DRname,T.sch_opfloor, T.sch_scd2, TRM.ar_effect,TRM.Or_Effect  ")
        cmd += String.Format(" , DECODE(T.sch_actfg,1,'未報到',2,'報到',3,'手術中',4,'完成手術',5,'取消排程',6,'取消報到',7,'恢復室',8,'已離開') sch_actfg ")
        cmd += String.Format(" FROM  TOPR_SCH T ,DR D,IDP I,TRM_SCH TRM")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" T.pno = I.pno ")
        '  cmd += String.Format(" AND TRM.Or_effect = 'Y'")
        cmd += String.Format(" AND TRM.pno (+)= T.pno ")
        cmd += String.Format(" AND T.sch_opdr = D.code")
        cmd += String.Format(" AND TRM.opdt (+)= T.sch_opdt ")
        'md += String.Format(" AND T.sch_actfg NOT IN  ('5','6','8') ") '排除5取消排程6取消報到8已離開
        cmd += String.Format(" AND T.sch_opdt >= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", SerchDateB)
        cmd += String.Format(" AND T.sch_opdt <= To_Date('{0}','yyyy/MM/DD HH24:MI:SS') ", SerchDateE)
        cmd += SQLPlus
        cmd += String.Format(" ORDER BY T.sch_opdt,T.pno ")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_OP_SCH As New DataTable
        DA.Fill(DT_OP_SCH)

        'GridView1.Caption = "手術排程列表"
        'GridView1.DataSource = DT_OP_SCH
        'GridView1.DataBind()


        '####################將資料放入最後顯示資料表
        Dim Operation_final As String = ""
        For i = 0 To DT_OP_SCH.Rows.Count - 1

            Dim Or_Effect As String = DT_OP_SCH.Rows(i)("Or_Effect").ToString
            '病歷
            '床號()
            Dim bedno As String = DT_OP_SCH.Rows(i)("sch_bedno").ToString
            '病歷號碼
            Dim pno As String = DT_OP_SCH.Rows(i)("pno").ToString
            '病患姓名
            Dim name As String = DT_OP_SCH.Rows(i)("name").ToString
            '年齡
            Dim birth As DateTime = DT_OP_SCH.Rows(i)("birth").ToString
            Dim Age As Integer = Age_Count(birth.ToString("yyyy/MM/dd"), Now)
            'ASA
            Dim ASA As String = DT_OP_SCH.Rows(i)("ar_asa").ToString
            '手術日期
            Dim sch_date As Date = DT_OP_SCH.Rows(i)("sch_opdt")
            Dim sch_opdt As String = String.Format("{0}/{1}", sch_date.Month.ToString, sch_date.Day.ToString)
            '醫生員編,姓名
            Dim dr As String = DT_OP_SCH.Rows(i)("sch_opdr").ToString & " <br>" & DT_OP_SCH.Rows(i)("Drname").ToString
            '術式
            Dim sch_scd2 As String = DT_OP_SCH.Rows(i)("sch_scd2").ToString
            '狀態
            Dim sch_opactfg As String = DT_OP_SCH.Rows(i)("sch_actfg").ToString
            '行動能力


            Operation_final += String.Format("<tr class =""Table_tr_style1"">")
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_opdt)

            '判斷手術室是否有輸入資料--回覆
            If Or_Effect = "Y" Then
                Operation_final += String.Format("    <td class=""Table_td_style""><button style=""width:50px;height:40px; font-size:16px;background-color:#95cfcf;""  type =""button"" onclick=""window.open('TRM-DR.aspx?Hospcode={0}&pno={1}&opdt={2}')"">回覆</butto</td>", Hospcode, pno, sch_date.ToShortDateString)
            Else
                Operation_final += String.Format(" <td class=""Table_td_style""></td>")
            End If
            '判斷麻醫是否有選擇ASA顯示燈號
            If Not IsDBNull(DT_OP_SCH.Rows(i)("ar_asa")) Then
                Select Case ASA
                    Case "P1", "P2"
                        Operation_final += String.Format("    <td class=""Table_td_style""><img alt="""" src=""image/light.png"" style=""width:30px;height:30px;""/></td>")
                    Case "P3", "P4", "P5", "P6"
                        Operation_final += String.Format("    <td class=""Table_td_style""><img alt="""" src=""image/heavy.png"" style=""width:30px;height:30px;""/></td>")
                End Select
            Else
                Operation_final += String.Format("    <td class=""Table_td_style""></td>")
            End If
            '檢驗
            'Operation_final += String.Format("    <td class=""Table_td_style""><button style=""width:30px;height:30px; font-size:16px;background-color:#95cfcf;""  type =""button"" onclick=""window.open('TRM-DR.aspx?Hospcode={0}&pno={1}&opdt={2}&Empno=K351')"">檢</button></td>", Hospcode, pno, sch_date.ToShortDateString)
            Operation_final += String.Format("   <td class=""Table_td_style""></td>")
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", bedno)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", pno)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", name)
            Operation_final += String.Format("   <td class=""Table_td_style"">{0}</td>", Age)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_scd2)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", dr)
            Operation_final += String.Format("    <td class=""Table_td_style"">{0}</td>", sch_opactfg)
            Operation_final += String.Format("</tr>")
        Next
        If DT_OP_SCH.Rows.Count = 0 Then
            Operation_List.Text = "<tr class =""Table_tr_style1""><td class=""Table_td_style"" colspan=""11"">目前沒有資料!! </td></tr>"
        Else
            Operation_List.Text = Operation_final
        End If

    End Sub
    '交班 Checkbox
    Protected Sub CheckBoxHandOver_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxHandOver.CheckedChanged
        Call ButtonSerch_Click(sender, e)


    End Sub
    '顯示已離開 Checkbox
    Protected Sub CheckBoxLeave_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxLeave.CheckedChanged
        Call ButtonSerch_Click(sender, e)
    End Sub

    
End Class
