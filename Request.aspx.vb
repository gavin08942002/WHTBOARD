Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData
Partial Class Request
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim Requst_data As NameValueCollection = Request.QueryString
        Dim Wardcode As String = "17"
        Dim Ward_keys As Array
        Dim Ward_data As Array = Requst_data.GetValues("id")
        Dim test As String = Request.QueryString("is")
        Response.Write(test)


    End Sub
End Class
