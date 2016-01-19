'041213 not final
'041613 ok na
Imports Newtonsoft.Json
Imports EDIdataClass
Public Class Main
    Private sqlmsg As String = Nothing
    Public Shared uname As String
    Dim ls_jobtitle As String = Nothing

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    
    End Sub

    

    Private Function jobtitle() As Boolean
        If ls_jobtitle = MODLS_JOBTITLE Then
            jobtitle = True
        Else
            jobtitle = False
        End If
    End Function
    Private Function validateLoginProgdesc() As Response
        Dim response As New Response
        Try
validretry:

            response = JsonConvert.DeserializeObject(service.validateUserByProgdesc(ediprogdesc), GetType(Response))
        Catch ex As Exception
            response.responseCode = ResponseCode.Error
            If (retryCount < maxretry) Then
                retryCount += 1
                GoTo validretry
            End If
            Dim li_input As Integer = Nothing
            li_input = MsgBox("Application timeout. \n Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
            If li_input = vbYes Then
                response = validateLoginProgdesc()
            Else
                Me.Close()

            End If
        End Try

        Return response
    End Function
    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ini As IniFile = New IniFile(AppDomain.CurrentDomain.BaseDirectory + "SynergyMaintenance.ini")

        ediname = ini.ReadINI("Config", "name")
        ediprogdesc = ini.ReadINI("Config", "progdesc")
        edi = ini.ReadINI("Config", "edi")
        '  setServiceUrl(ini.ReadINI("webservice", "url"))

        Me.Text = "Main-" + ediname & gs_version
        'Dim adz As New clsdatalog
        'Dim q As String = "SELECT RTRIM(username) AS usrName,RTRIM([password]) AS pw FROM usersat WHERE progdesc = 'EDIVISAYAS' ORDER BY syscreated DESC; "
        'Dim RDR1 As SqlClient.SqlDataReader = Nothing
        'If Not adz.clsConnect Then Exit Sub
        'If adz.ErrorConnectionReading(False) Then Exit Sub
        'If Not adz.dr(q, RDR1) Then Exit Sub
        'If RDR1.HasRows Then
        '    If RDR1.Read Then
        '        ls_username = RDR1.GetString(0)
        '        ls_userid = RDR1.GetString(1)
        '    End If
        'Else
        '    MsgBox("You are not Authorized to access this application", MsgBoxStyle.Critical, "Synergy EDI-" + ediname)
        '    Application.Exit()
        'End If
        Dim response As Response
        ''Dim service As New EDIService.Service
        'Dim s As String = ini.ReadINI("webservice", "url")
        Dim timeout As Integer
        'timeout = 0
        Try
            timeout = 60000 * Convert.ToInt32(ini.ReadINI("webservice", "timeout"))
            service.Timeout = timeout
        Catch ex As Exception

        End Try
        service.Timeout = System.Threading.Timeout.Infinite

        service.Url = ini.ReadINI("webservice", "url")

        retryCount = 0
        response = validateLoginProgdesc()
        retryCount = 0
        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)
            Me.Close()
            Exit Sub
        End If
        If response.responseCode = ResponseCode.NotFound Then
            MsgBox("You are not Authorized to access this application", MsgBoxStyle.Critical, "Synergy EDI" + ediname)
            Application.Exit()
        Else
            Dim ediuser As User = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(User))

            ls_username = ediuser.username
            ls_userid = ediuser.password
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
        strBtnName = "Button9"
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
        'Dim adz1 As New clsdatalog
        'Dim q1 As String = "UPDATE usersat SET progdesc = 'NULL',syscreated= GETDATE() WHERE username = '" + ls_username + "';"
        'Dim RDR2 As SqlClient.SqlDataReader = Nothing
        'If Not adz1.clsConnect Then Exit Sub
        'If adz1.ErrorConnectionReading(False) Then Exit Sub
        'If Not adz1.dr(q1, RDR2) Then Exit Sub
        Me.Dispose()
        Process.GetCurrentProcess.Kill()
        Me.Close()
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
    Private Sub airphil()
        calledicategory = 44
        Dim airphil As New VIS_AIRPHIL
        airphil.Show()
        Me.Hide()
    End Sub
    Private Sub Button41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button41.Click
        calledicategory = 43
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub AirPhilBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AirPhilBtn.Click
        calledicategory = 44
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProtectPlus.Click
        calledicategory = 45
        Dim CALLYEAFORM As New YearForm
        CALLYEAFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button46_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
        calledicategory = 5
        strBtnName = "Button46"
        Dim CALLYEARFORM As New YearForm
        CALLYEARFORM.Show()
        Me.Hide()
    End Sub

    Private Sub Button47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        calledicategory = 5
        Dim CALLYEARFORM As New YearForm
        CALLYEARFORM.Show()
        Me.Hide()
    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

    End Sub
End Class