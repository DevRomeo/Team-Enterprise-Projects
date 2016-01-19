Imports INI_DLL
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim var As New myData
    'Dim mainContent As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Me.Session("var") = Nothing
        Me.Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' btnLogout.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this record?');")
        var = Me.Session("var")
        If var Is Nothing Then
            Response.Write("<script language=javascript>")
            Response.Write("alert('" & "Your session expired. Please log-in again." & "')")
            Response.Write("</script>")
            Response.Write("<script language=javascript>")
            Response.Write("window.location = 'Login.aspx'")
            Response.Write("</script>")
        Else
            lblName.Text = var.user_Fullname.Trim & " - " & var.strTask.Trim
            Click()
        End If
       
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Response.Redirect("main.aspx")
    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProfile.Click
        Response.Redirect("Profile.aspx")
    End Sub
    Private Sub Click()
        'mainContent = New System.Web.UI.HtmlControls.HtmlGenericControl
        If Me.Session("curPage") = "main" Then
            Me.btnHome.ForeColor = Drawing.Color.Black
            Me.btnHome.Enabled = False
            Me.btnHome.BackColor = Drawing.Color.FromName("#99CCFF")
            Me.btnProfile.ForeColor = Drawing.Color.FromName("#268CCD")
            Me.btnProfile.Enabled = True
            mainContent.Style.Add("height", "1053px")



        Else
            Me.btnHome.ForeColor = Drawing.Color.FromName("#268CCD")
            Me.btnProfile.Enabled = False
            Me.btnProfile.BackColor = Drawing.Color.FromName("#99CCFF")
            Me.btnProfile.ForeColor = Drawing.Color.Black
            Me.btnHome.Enabled = True
            mainContent.Style.Add("height", "100%")
        End If

    End Sub

    Protected Sub btnRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRedirect.Click
        Dim usr = var.uname
        Dim pwrd = var.passwrd

        Dim str_Path As String = AppDomain.CurrentDomain.BaseDirectory + "ExecReport.ini"
        Dim rdr As New IniFile(str_Path)
        Dim kpurl = rdr.IniReadValue("kplink", "kpurl")
        Dim kpuname = rdr.IniReadValue("kplink", "kpuname")
        Dim kppass = rdr.IniReadValue("kplink", "kppass")
        Dim urlstr As String = kpurl + "?" + kpuname + "=" + usr + "&" + kppass + "=" + pwrd
        Response.Redirect(urlstr)
    End Sub
End Class

