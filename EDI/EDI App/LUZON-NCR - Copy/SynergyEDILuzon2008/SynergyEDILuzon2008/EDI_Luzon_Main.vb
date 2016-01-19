Public Class EDI_Luzon_Main
    Private sqlmsg As String = Nothing
    Public Shared uname As String
    Dim ls_jobtitle As String = Nothing

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        calledicategory = 1
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub EDI_Luzon_Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim adz1 As New clsdatalog
        Dim q1 As String = "update usersat set progdesc = 'null',syscreated='" + DateTime.Now + "' where username like '" + ls_username + "'"
        Dim RDR2 As SqlClient.SqlDataReader = Nothing
        If Not adz1.clsConnect Then Exit Sub
        If adz1.ErrorConnectionReading(False) Then Exit Sub
        If Not adz1.dr(q1, RDR2) Then Exit Sub
        End
    End Sub

    Private Sub EDI_Luzon_Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Main " & gs_version
        TabControl1.Enabled = False
        Dim adz As New clsdatalog
        Dim q As String = "select USERNAME,[password] from usersat where progdesc IN ('EDILuzon','EDINCR') order by syscreated desc"
        Dim RDR1 As SqlClient.SqlDataReader = Nothing
        If Not adz.clsConnect Then Exit Sub
        If adz.ErrorConnectionReading(False) Then Exit Sub
        If Not adz.dr(q, RDR1) Then Exit Sub
        If RDR1.Read Then
            ls_username = Trim(RDR1(0).ToString)
            ls_userid = Trim(RDR1(1).ToString)
        End If
        ps_resid = ls_userid 'ely code 7-4-2012
        Me.CenterToScreen()
        btnProceed.Enabled = False
    End Sub

    Private Function jobtitle() As Boolean
        If modls_jobtitle1 = ls_jobtitle Then
            jobtitle = True
        End If
        If modls_jobtitle2 = ls_jobtitle Then
            jobtitle = True
        End If

    End Function

    Private Sub SaveUser()
        Dim sCheck As String = Nothing
        Dim sUser As String = "select JOB_TITLE,fullname,RES_ID from humres where usr_id = '" + ls_username + "' and res_id like '" + ls_userid + "' and blocked = '0'"

        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If Not c.Error_SetRdr(sUser, rdr, sqlmsg) Then
            If rdr.Read Then
                ls_jobtitle = Trim(rdr(0)).ToString
                ps_resid = rdr(2).ToString

                If jobtitle() = True Then
                    rdr.Close()
                Else
                    MsgBox("You are not authorize to enter this program ", 16, gs_version)
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        calledicategory = 33
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        calledicategory = 8
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        calledicategory = 31
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        calledicategory = 5
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        calledicategory = 9
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        calledicategory = 13
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        calledicategory = 17
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        calledicategory = 28
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        calledicategory = 24
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        calledicategory = 20
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        calledicategory = 30
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        calledicategory = 14
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        calledicategory = 34
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        calledicategory = 25
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        calledicategory = 16
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        calledicategory = 15
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        calledicategory = 4
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        calledicategory = 12
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        Dim bkp As New BOSKPLuzon
        bkp.Show()
        Me.Hide()
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        calledicategory = 22
        Dim CALLYEARFORM As New YearForm
        CALLYEARFORM.Show()
        Me.Hide()
    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Dim adz1 As New clsdatalog
        Dim q1 As String = "update usersat set progdesc = 'null',syscreated='" + DateTime.Now + "' where username like '" + ls_username + "'"
        Dim RDR2 As SqlClient.SqlDataReader = Nothing
        If Not adz1.clsConnect Then Exit Sub
        If adz1.ErrorConnectionReading(False) Then Exit Sub
        If Not adz1.dr(q1, RDR2) Then Exit Sub
        End
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        Dim corppartners As New MAINTENANCE_CORP_PARTNERS
        corppartners.Show()
        Me.Hide()
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        calledicategory = 35
        Dim CALLYEARFORM As New YearForm
        CALLYEARFORM.Show()
        Me.Hide()
    End Sub


    Private Sub btnProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProceed.Click
        If cmbserver.Text = "" Then
            MsgBox("Input server location first, then proceed", 16, gs_version)
        Else
            TabControl1.Enabled = True
            cmbserver.Enabled = True
            btnProceed.Enabled = False
            gs_serverloc = cmbserver.Text
        End If
        
    End Sub

    Private Sub cmbserver_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbserver.SelectedIndexChanged
        btnProceed.Enabled = True
        TabControl1.Enabled = False
    End Sub
End Class