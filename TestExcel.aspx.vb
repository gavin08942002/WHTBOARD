Imports System.IO
Imports System.Data

Imports NPOI
Imports NPOI.XSSF.UserModel
Imports NPOI.POIFS
Imports NPOI.POIFS.FileSystem
Imports NPOI.Util

Partial Class TestExcel
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SavePath As String = Request.PhysicalApplicationPath & "upload\0117.xlsx"
        '測試用
        Dim ExcelFile As FileStream = File.Open(SavePath, FileMode.Open)

        If (FileUpload1.HasFile) Then
            Dim FileName As String = FileUpload1.FileName
            SavePath = SavePath & FileName
            FileUpload1.SaveAs(SavePath)
            Label1.Text = "上傳成功"




            Dim WorkBook As XSSFWorkbook = New XSSFWorkbook(FileUpload1.FileContent)
            Dim U_sheet As XSSFSheet = WorkBook.GetSheetAt(0)
            Dim D_table As DataTable = New DataTable()
            Dim HeaderRow As XSSFRow = U_sheet.GetRow(0)
            '將標頭資料放入D_table裡面
            For k As Integer = HeaderRow.FirstCellNum To (HeaderRow.LastCellNum - 1)
                If Not HeaderRow.GetCell(k) Is Nothing Then
                    Dim D_column As DataColumn = New DataColumn(HeaderRow.GetCell(k).StringCellValue)
                    D_table.Columns.Add(D_column)

                    '列出所有表頭
                    Response.Write(D_column.ToString)
                End If
            Next
            For i As Integer = (U_sheet.FirstRowNum + 1) To U_sheet.LastRowNum
                Dim Row As XSSFRow = U_sheet.GetRow(i)
                Dim D_datarow As DataRow = D_table.NewRow()
                For j As Integer = Row.FirstCellNum To (Row.LastCellNum - 1)
                    If Not Row.GetCell(j) Is Nothing Then
                        D_datarow(j) = Row.GetCell(j).ToString()

                    End If
                Next
                D_table.Rows.Add(D_datarow)

            Next





            WorkBook = Nothing
            U_sheet = Nothing
        Else
            Label1.Text = "上傳失敗"
        End If
    End Sub
End Class
