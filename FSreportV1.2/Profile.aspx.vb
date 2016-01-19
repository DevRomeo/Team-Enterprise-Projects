Imports DB_DLL
Partial Class Profile
    Inherits System.Web.UI.Page
    Dim var As New myData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        var = Me.Session("var")
        If var Is Nothing Then
            Response.Write("<script language=javascript>")
            Response.Write("alert('" & "Your session expired. Please log-in again." & "')")
            Response.Write("</script>")
            Response.Write("<script language=javascript>")
            Response.Write("window.location = 'Login.aspx'")
            Response.Write("</script>")
        End If

        If Not IsPostBack Then
            Me.txtFname.Text = var.user_Fullname.Trim
            Me.txtTask.Text = var.strTask.Trim
            Me.txtUname.Text = var.uname
            Me.txtPass.Attributes.Add("value", var.passwrd)
            Me.txtConfirmPass.Attributes.Add("value", var.passwrd)

        End If
        Me.Session.Add("curPage", "Profile")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim mydb As New clsDBConnection
        Dim sql As String
        Dim isError As Boolean = False
        If (Me.txtPass.Text <> Me.txtConfirmPass.Text) Or (Me.txtPass.Text = "") Then
            Me.lblmsg.Text = "Confirm Password is not the same to New Password or New Password is empty."
            Exit Sub
        End If
        sql = "Update OnlineFundTransferUser SET pass='" & Me.txtPass.Text & "', fullname='" & Me.txtFname.Text & "'" & _
              ", task='" & Me.txtTask.Text.Trim & "' WHERE usr_name='" & Me.txtUname.Text.Trim & "'"

        If mydb.isConnected Then
            mydb.CloseConnection()
        End If

        Dim conStr As String = var.strCon_Web
        If Not mydb.ConnectDB(conStr) Then
            Me.lblmsg.Text = "Could not connect to server. Please contact the administrator."
        End If

        If mydb.Execute_SQLQuery(sql) < 1 Then
            Me.lblmsg.Text = "Unsuccessful in saving data. Please contact the administrator."
        Else
            var.user_Fullname = Me.txtFname.Text
            var.strTask = Me.txtTask.Text
            var.passwrd = Me.txtPass.Text
            Me.Session.Add("var", var)

            Response.Write("<script language=javascript>")
            Response.Write("alert('" & "Your information is successfully updated." & "')")
            Response.Write("</script>")
            Response.Write("<script language=javascript>")
            Response.Write("window.location = 'main.aspx'")
            Response.Write("</script>")
        End If

        mydb.CloseConnection()
        Me.Button1.Enabled = False
        Me.Button2.Enabled = True
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.txtConfirmPass.Enabled = True
        Me.txtFname.Enabled = True
        Me.txtPass.Enabled = True
        Me.txtTask.Enabled = True
        Me.Button1.Enabled = False
        Me.Button1.Enabled = True
        Me.Button2.Enabled = False
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("main.aspx")
    End Sub
End Class
