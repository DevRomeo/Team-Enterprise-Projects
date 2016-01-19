Public Class EDI_Log_In_Luzon

    Inherits System.Windows.Forms.Form

    Private sqlmsg As String = Nothing
    Dim ls_jobtitle As String = Nothing

    Private Sub EDI_Log_In_Luzon_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Login " & gs_version
        Me.rempass()
        Me.CenterToScreen()
        Me.TopMost = True
    End Sub

    Private Sub rempass()
        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim s As String = "select * from checks where stats = '1'"
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If Not c.Error_SetRdr(s, rdr, sqlmsg) Then
            If rdr.Read Then
                If rdr(0) = 1 Then
                    txtUser.Text = rdr(1)
                    txtPass.Text = rdr(2)
                    res_id = txtPass.Text
                End If
            End If
            rdr.Close()
        End If
    End Sub

    Private Sub SaveUser()
        Dim sCheck As String = Nothing
        Dim frmmain As New EDI_Luzon_Main
        Dim sUser As String = "select JOB_TITLE,fullname,RES_ID from humres where usr_id = '" + txtUser.Text + "' and res_id like '" + txtPass.Text + "' and blocked = '0'"
       
        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If Not c.Error_SetRdr(sUser, rdr, sqlmsg) Then
            If rdr.Read Then
                ls_jobtitle = Trim(rdr(0)).ToString
                ps_resid = rdr(2).ToString

                If jobtitle() = True Then
                    frmmain.Show()
                    Me.Hide()
                    rdr.Close()
                Else
                    MsgBox("You are not authorize to login ", 16, gs_version)
                    rdr.Close()
                    GoTo ext
                End If

            Else
                MsgBox("Invalid Username or Password", 48, gs_version)
                GoTo ext
            End If
        End If
        Exit Sub
ext:
    End Sub

    Private Sub form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim adz1 As New clsdatalog
        Dim q1 As String = "update usersat set progdesc = 'null',syscreated='" + DateTime.Now + "' where username like '" + ls_username + "'"
        Dim RDR2 As SqlClient.SqlDataReader = Nothing
        If Not adz1.clsConnect Then Exit Sub
        If adz1.ErrorConnectionReading(False) Then Exit Sub
        If Not adz1.dr(q1, RDR2) Then Exit Sub
        End
    End Sub
    Private Sub txtPass_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPass.GotFocus
        txtPass.SelectionStart = 0
        txtPass.SelectionLength = Len(txtPass.Text)
    End Sub

    Private Sub txtPass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPass.KeyPress
        If e.KeyChar = ChrW(13) Then
            Me.SaveUser()
        End If
        If (Not (Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar))) Then
            e.Handled = True
        Else
            e.Handled = False
        End If
    End Sub

    Private Function jobtitle() As Boolean
        If modls_jobtitle1 = ls_jobtitle Then
            jobtitle = True
        End If
        If modls_jobtitle2 = ls_jobtitle Then
            jobtitle = True
        End If

    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        End
    End Sub

    Private Sub btnLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.SaveUser()
    End Sub

    Private Sub txtPass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPass.TextChanged

    End Sub

    Private Sub OkBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkBtn.Click
        Me.SaveUser()
    End Sub

    Private Sub CancelBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
        End
    End Sub
End Class