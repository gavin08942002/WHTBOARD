
Partial Class AJAX_Default
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim data As String = "This is just a test aspx."
        Response.Write(data)
    End Sub
End Class
