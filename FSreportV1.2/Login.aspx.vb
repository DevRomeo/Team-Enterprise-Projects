Imports System.Data
Imports System.IO


Partial Class Login
    Inherits System.Web.UI.Page
    Dim var As New myData



    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Me.txtInfo.Text = var.ReturnError()
       
        If isValidEntry() Then
            If var.AutoValidateUser(var.Encrypt(Me.txtUserName.Text), var.Encrypt(Me.txtPassword.Text)) Then
                Me.Session.Add("var", var)
                Response.Redirect("main.aspx")

            ElseIf var.ValidateUser(Me.txtUserName.Text, Me.txtPassword.Text) Then
                Me.Session.Add("var", var)
                Response.Redirect("main.aspx")
            Else

            End If
            End If
    End Sub
    Private Function isValidEntry() As Boolean
        isValidEntry = False
        If Me.txtUserName.Text = "" Then
            Me.txtInfo.Text = "Please input user name."
            Exit Function
        End If

        If txtPassword.Text = "" Then
            Me.txtInfo.Text = "Please input password."
            Exit Function
        End If

        If Me.txtPassword.Text <> "" And Me.txtUserName.Text <> "" Then
            isValidEntry = True
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        var.ReadINI()

        Dim userid As String = Request.QueryString("usrid")
        Dim pass As String = Request.QueryString("pwd")

        If Not ((userid Is Nothing) Or (pass Is Nothing)) Then

            If var.AutoValidateUser(userid, pass) Then
                Me.Session.Add("var", var)
                Response.Redirect("main.aspx")
            Else
                Me.txtInfo.Text = var.ReturnError()
            End If

        End If

    End Sub
End Class
