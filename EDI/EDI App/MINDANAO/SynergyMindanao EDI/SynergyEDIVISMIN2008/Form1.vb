Public Class Form1
    Private sqlmsg As String = Nothing
    Dim ls_jobtitle As String = Nothing

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Me.TopMost = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.SaveUser()
    End Sub
    Private Sub SaveUser()
        Dim sCheck As String = Nothing
        Dim frmmain As New Main
        Dim sUser As String = "select fullname,RES_ID,job_title from humres where usr_id = '" + txtUser.Text + "' and res_id like '" + txtPass.Text + "' and blocked = '0'"
        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If Not c.Error_SetRdr(sUser, rdr, sqlmsg) Then
            If rdr.Read Then
                modifier = Trim(rdr(0))
                ps_resid = Trim(rdr(1))
                ls_jobtitle = Trim(rdr(2))
                If jobtitle() = True Then '--------New Eli code 2-6-2010   
                    Me.Hide()
                    frmmain.Show()
                Else
                    MsgBox("You are not authorize to enter this program", 16, "Electronic Data Interchange")
                End If                    '--------New Eli code 2-6-2010   

                rdr.Close()
            Else
                MsgBox("Invalid Username or Password", 48, "Input Error")
                GoTo ext
            End If
        End If
        Exit Sub
ext:
    End Sub
    Private Function jobtitle() As Boolean
        If ls_jobtitle = MODLS_JOBTITLE Then
            jobtitle = True
        Else
            jobtitle = False
        End If
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub OkBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkBtn.Click
        Me.SaveUser()
    End Sub

    Private Sub CancelBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelBtn.Click
        End
    End Sub

    Private Sub txtPass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPass.TextChanged

    End Sub
End Class
