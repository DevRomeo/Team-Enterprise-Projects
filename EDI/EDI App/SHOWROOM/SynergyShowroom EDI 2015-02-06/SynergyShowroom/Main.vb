'041213 ok2 na
Public Class Main
    Private sqlmsg As String = Nothing
    Public Shared uname As String
    Dim ls_jobtitle As String = Nothing

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim adz1 As New clsdatalog
        Dim q1 As String = "update usersat set progdesc = 'NULL',syscreated= GETDATE() where username = '" + ls_username + "'"
        Dim RDR2 As SqlClient.SqlDataReader = Nothing
        If Not adz1.clsConnect Then Exit Sub
        If adz1.ErrorConnectionReading(False) Then Exit Sub
        If Not adz1.dr(q1, RDR2) Then Exit Sub
        End
    End Sub

    Private Sub SaveUser()
        Dim sCheck As String = Nothing
        'Dim frmmain As New Main
        Dim sUser As String = "select fullname,RES_ID,job_title from humres where usr_id = '" + ls_username + "' and res_id like '" + ls_userid + "' and blocked = '0'"
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
                    'Me.Hide()
                    'frmmain.Show()
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

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Main " & gs_version
        Dim adz As New clsdatalog
        Dim q As String = "select USERNAME,password from usersat where progdesc = 'EDISHOWROOM' order by syscreated desc"
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
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        calledicategory = 1
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        calledicategory = 2
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        calledicategory = 3
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        calledicategory = 4
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        calledicategory = 5
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        calledicategory = 6
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        calledicategory = 7
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        'Dim gs As New GlobeSmart
        'gs.Show()
        'Me.Hide()
        calledicategory = 8
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        calledicategory = 9
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        calledicategory = 10
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        calledicategory = 11
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        calledicategory = 12
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        calledicategory = 13
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        calledicategory = 14
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        calledicategory = 15
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        calledicategory = 16
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        calledicategory = 17
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        calledicategory = 18
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        calledicategory = 19
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        calledicategory = 20
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        calledicategory = 21
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        calledicategory = 22
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        calledicategory = 23
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        calledicategory = 24
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        calledicategory = 26
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        calledicategory = 25
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button33.Click
        calledicategory = 27
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        calledicategory = 28
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button28.Click
        calledicategory = 29
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button29.Click
        calledicategory = 30
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button30.Click
        calledicategory = 31
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        calledicategory = 32
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button32.Click
        Dim boskp As New MAINTENANCE_BOSKP
        boskp.Show()
        Me.Hide()
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        Dim corppartners As New MAINTENANCE_CORP_PARTNERS
        corppartners.Show()
        Me.Hide()
    End Sub

    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button35.Click
        calledicategory = 33
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button36.Click
        Dim adz1 As New clsdatalog
        Dim q1 As String = "update usersat set progdesc = 'NULL',syscreated= GETDATE() where username like '" + ls_username + "'"
        Dim RDR2 As SqlClient.SqlDataReader = Nothing
        If Not adz1.clsConnect Then Exit Sub
        If adz1.ErrorConnectionReading(False) Then Exit Sub
        If Not adz1.dr(q1, RDR2) Then Exit Sub
        End
    End Sub

    Private Sub JewEDIReportBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JewEDIReportBtn.Click
        calledicategory = 34
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        calledicategory = 35
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        calledicategory = 36
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button44.Click
        calledicategory = 37
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
        calledicategory = 38
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        calledicategory = 39
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        calledicategory = 40
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button40.Click
        calledicategory = 41
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        calledicategory = 42
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button41.Click
        calledicategory = 43
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
        calledicategory = 44
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub btnLuzonSSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLuzonSSMI.Click
        calledicategory = 45
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub

    Private Sub Button47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button47.Click
        calledicategory = 46
        Dim callyearform As New YearForm
        callyearform.Show()
        Me.Hide()
    End Sub
End Class