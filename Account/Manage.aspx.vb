Imports System.Collections.Generic

Imports Microsoft.AspNet.Membership.OpenAuth

Partial Class Account_Manage
    Inherits System.Web.UI.Page

    Private successMessageTextValue As String
    Protected Property SuccessMessageText As String
        Get
            Return successMessageTextValue
        End Get
        Private Set(value As String)
            successMessageTextValue = value
        End Set
    End Property

    Private canRemoveExternalLoginsValue As Boolean
    Protected Property CanRemoveExternalLogins As Boolean
        Get
            Return canRemoveExternalLoginsValue
        End Get
        Set(value As Boolean)
            canRemoveExternalLoginsValue = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' 決定要呈現的區段
            Dim hasLocalPassword = OpenAuth.HasLocalPassword(User.Identity.Name)
            setPassword.Visible = Not hasLocalPassword
            changePassword.Visible = hasLocalPassword

            CanRemoveExternalLogins = hasLocalPassword

            ' 呈現成功訊息
            Dim message = Request.QueryString("m")
            If Not message Is Nothing Then
                ' 使查詢字串脫離動作
                Form.Action = ResolveUrl("~/Account/Manage.aspx")

                Select Case message
                    Case "ChangePwdSuccess"
                        SuccessMessageText = "您的密碼已變更。"
                    Case "SetPwdSuccess"
                        SuccessMessageText = "您的密碼已設定。"
                    Case "RemoveLoginSuccess"
                        SuccessMessageText = "已移除外部登入。"
                    Case Else
                        SuccessMessageText = String.Empty
                End Select

                successMessage.Visible = Not String.IsNullOrEmpty(SuccessMessageText)
            End If
        End If

         
    End Sub

    Protected Sub setPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If IsValid Then
            Dim result As SetPasswordResult = OpenAuth.AddLocalPassword(User.Identity.Name, password.Text)
            If result.IsSuccessful Then
                Response.Redirect("~/Account/Manage.aspx?m=SetPwdSuccess")
            Else
                
                ModelState.AddModelError("NewPassword", result.ErrorMessage)
                
            End If
        End If
    End Sub

    
    Public Function GetExternalLogins() As IEnumerable(Of OpenAuthAccountData)
        Dim accounts = OpenAuth.GetAccountsForUser(User.Identity.Name)
        CanRemoveExternalLogins = CanRemoveExternalLogins OrElse accounts.Count() > 1
        Return accounts
    End Function

    Public Sub RemoveExternalLogin(ByVal providerName As String, ByVal providerUserId As String)
        Dim m = If(OpenAuth.DeleteAccount(User.Identity.Name, providerName, providerUserId), "?m=RemoveLoginSuccess", String.Empty)
        Response.Redirect("~/Account/Manage.aspx" & m)
    End Sub
    

    Protected Shared Function ConvertToDisplayDateTime(ByVal utcDateTime As Nullable(Of DateTime)) As String
        ' 您可以變更這個方法，將 UTC 日期時間轉換為所需的顯示
        '位移和格式。我們在此將它轉換為伺服器時區並格式化
        '為短日期和長時間字串，並使用目前的執行緒文化特性。
        Return If(utcDateTime.HasValue, utcDateTime.Value.ToLocalTime().ToString("G"), "[從不]")
    End Function
End Class
