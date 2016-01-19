Imports System.Web.Mail
Imports System.Threading
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Public Class frmMain
    Inherits System.Windows.Forms.Form
    Private myThread As Thread
    Private isRunning, isRegenerate As Boolean
    Dim mymsg As String

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        '
        ' Required for Windows Form Designer support
        '
        'InitializeComponent()

        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnSetup As System.Windows.Forms.Button
    Friend WithEvents rbox As System.Windows.Forms.RichTextBox
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Timer As System.Windows.Forms.Timer
    ' Dim reportnamep, titlep, Class_01p, Serverp, Dbp, Unamep, Passwordp, Paramdatep, ParamClass_01p, ParamClass_03p, ParamClass_04p, areacodep, emailAddressp, Class_03p, classificationp, specificp, layoutp, Areap As String
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblLastconfigure As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.btnSetup = New System.Windows.Forms.Button
        Me.rbox = New System.Windows.Forms.RichTextBox
        Me.lblTime = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.Label5 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblLastconfigure = New System.Windows.Forms.Label
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'btnSetup
        '
        Me.btnSetup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSetup.BackColor = System.Drawing.Color.Gainsboro
        Me.btnSetup.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSetup.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSetup.Image = CType(resources.GetObject("btnSetup.Image"), System.Drawing.Image)
        Me.btnSetup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSetup.Location = New System.Drawing.Point(426, 353)
        Me.btnSetup.Name = "btnSetup"
        Me.btnSetup.Size = New System.Drawing.Size(168, 23)
        Me.btnSetup.TabIndex = 7
        Me.btnSetup.Text = "       SYSTEM SETTINGS"
        Me.btnSetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSetup.UseVisualStyleBackColor = False
        '
        'rbox
        '
        Me.rbox.BackColor = System.Drawing.Color.White
        Me.rbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbox.Location = New System.Drawing.Point(8, 50)
        Me.rbox.Name = "rbox"
        Me.rbox.ReadOnly = True
        Me.rbox.Size = New System.Drawing.Size(578, 296)
        Me.rbox.TabIndex = 8
        Me.rbox.Text = ""
        '
        'lblTime
        '
        Me.lblTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTime.BackColor = System.Drawing.Color.Transparent
        Me.lblTime.Image = CType(resources.GetObject("lblTime.Image"), System.Drawing.Image)
        Me.lblTime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblTime.Location = New System.Drawing.Point(426, 3)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(156, 36)
        Me.lblTime.TabIndex = 1
        Me.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(170, 23)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Daily System Log:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.Color.Gainsboro
        Me.Button1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(512, 453)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 33)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "&Close"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
        Me.Label2.Location = New System.Drawing.Point(134, 379)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 44)
        Me.Label2.TabIndex = 8
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label2.Visible = False
        '
        'Timer
        '
        Me.Timer.Enabled = True
        Me.Timer.Interval = 1000
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(181, 382)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(321, 47)
        Me.Label5.TabIndex = 18
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label5.Visible = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "Email Scheduler  V3.0"
        Me.NotifyIcon1.Visible = True
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(7, 448)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(125, 24)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Last Configured Date:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLastconfigure
        '
        Me.lblLastconfigure.BackColor = System.Drawing.Color.Transparent
        Me.lblLastconfigure.Location = New System.Drawing.Point(143, 450)
        Me.lblLastconfigure.Name = "lblLastconfigure"
        Me.lblLastconfigure.Size = New System.Drawing.Size(269, 21)
        Me.lblLastconfigure.TabIndex = 22
        Me.lblLastconfigure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer2
        '
        Me.Timer2.Interval = 50000
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(597, 489)
        Me.Controls.Add(Me.lblLastconfigure)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.rbox)
        Me.Controls.Add(Me.btnSetup)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Email Scheduler  V3.0"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Overloads Shared Sub Main(ByVal args() As String)
        Application.Run(New frmMain)
    End Sub
    'Main
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.NotifyIcon1.Visible = True
            Me.NotifyIcon1.Icon = New Icon(Application.StartupPath & "\IMAGE\logo03.ico")

            Call LoadSettings()
            If objDBLocal.isConnected = True Then
                objDBLocal.CloseConnection()
            End If

            If Not objDBLocal.ConnectDB(ls_connectionStringLocal) Then
                MsgBox("Error:Can not connect to local server. Please contact database admin.", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
            End If
            objDBLocal.CloseConnection()



            Me.Timer.Enabled = True
            Timer1.Enabled = True

            Timer2.Enabled = True
            Timer2.Start()

            Dim li_ret As Integer
            Dim ls_startup As String
            Dim ls_buff As New System.Text.StringBuilder(255)

            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "STARTUP", "0", ls_buff, 256, ps_ini)
            ls_startup = ls_buff.ToString
            ls_startup = ls_startup.Substring(0, li_ret)

            lblLastconfigure.Text = Common.ps_date + " " + Common.ps_time
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer.Tick

        If ps_date <= Now.Date And ps_time = Now.ToShortTimeString And isRunning = False Then
            Timer.Enabled = False
            'GetDaily()
            'myThread = New Thread(AddressOf GetDailyVismin)
            'myThread.Start()
            myThread = New Thread(AddressOf GetDailyLuzon)
            myThread.Start()
            'myThread = New Thread(AddressOf GetWeekly)
            'myThread.Start()
            'myThread = New Thread(AddressOf GetMonthly)
            'myThread.Start()
            isRunning = True
        Else
            isRunning = False
        End If


    End Sub
    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If (Me.WindowState = FormWindowState.Minimized) Then
            Me.Hide()
            Me.NotifyIcon1.Visible = True
            Me.NotifyIcon1.Icon = New Icon("IMAGE\logo03.ico")
        End If
    End Sub
    Private Sub m_trayIcon_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Me.Activate()
        Me.Show()
        Me.Refresh()
        Me.WindowState = FormWindowState.Normal
    End Sub
    Private Sub GetDailyLuzon()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader


        Dim sql As String = "BEGIN Select DISTINCT EmailAddress,Classification,Specific,ReportID,CC,valueParamClass_01,valueParamClass_02,valueParamClass_03,valueParamClass_04,valueParamBranchCode from tbl_Recipients_V3 where Schedule='Daily' AND ValueParamClass_01='Luzon' ORDER BY reportID END"
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Dim rpt As Generate
        Dim t As Thread
        Try
            While dr.Read
                rpt = New Generate

                rpt.eadd = dr.Item("EmailAddress").ToString
                rpt.clas = dr.Item("Classification").ToString
                rpt.id = dr.Item("ReportID").ToString
                rpt.spec = dr.Item("specific").ToString
                rpt.cc = dr.Item("CC").ToString
                rpt.vClass_01 = dr.Item("valueParamClass_01").ToString
                rpt.vClass_02 = dr.Item("valueParamClass_02").ToString
                rpt.vClass_03 = dr.Item("valueParamClass_03").ToString
                rpt.vClass_04 = dr.Item("valueParamClass_04").ToString
                rpt.vbcode = dr.Item("valueParamBranchCode").ToString
                rpt.vdate = CStr(CDate(Now.ToShortDateString).AddDays(-1))

                'rpt.GenerateReport()
                If report_counter > 0 Then
                    totalRunningReports()
                End If

                t = New Thread(AddressOf rpt.GenerateReport)
                ' If report_counter > 4 Then
                'rpt.SaveUnsentReport(rpt.eadd, rpt.clas, rpt.spec, rpt.id, Now.Date, rpt.vClass_01, rpt.vClass_02, rpt.vClass_03, rpt.vClass_04, rpt.vbcode)
                'Delay5(20)
                ' End If

                t.Start()
                Delay5(2)


            End While
            dr.Close()
        Catch ex As Exception

        End Try
        db.CloseConnection()
    End Sub
    Private Sub totalRunningReports()
        If report_counter >= 4 Then
            Delay5(10)
            If report_counter >= 4 Then
                totalRunningReports()
            End If
        End If
    End Sub
    Private Sub GetDailyVismin()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader

        Dim sql As String = "BEGIN Select DISTINCT EmailAddress,Classification,Specific,ReportID,CC,valueParamClass_01,valueParamClass_02,valueParamClass_03,valueParamClass_04,valueParamBranchCode from tbl_Recipients_V3 where Schedule='Daily' AND (ValueParamClass_01='Visayas/Mindanao' OR ValueParamClass_01='ALL') ORDER BY reportID END"
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Dim rpt As Generate
        Dim t As Thread
        Try
            While dr.Read
                rpt = New Generate

                rpt.eadd = dr.Item("EmailAddress").ToString
                rpt.clas = dr.Item("Classification").ToString
                rpt.id = dr.Item("ReportID").ToString
                rpt.spec = dr.Item("specific").ToString
                rpt.cc = dr.Item("CC").ToString
                rpt.vClass_01 = dr.Item("valueParamClass_01").ToString
                rpt.vClass_02 = dr.Item("valueParamClass_02").ToString
                rpt.vClass_03 = dr.Item("valueParamClass_03").ToString
                rpt.vClass_04 = dr.Item("valueParamClass_04").ToString
                rpt.vbcode = dr.Item("valueParamBranchCode").ToString
                rpt.vdate = CStr(CDate(Now.ToShortDateString).AddDays(-1))

                t = New Thread(AddressOf rpt.GenerateReport)
                
                totalRunningReports()
                t.Start()
                Delay5(5)



            End While
            dr.Close()
        Catch ex As Exception

        End Try
        db.CloseConnection()
    End Sub
    Private Sub Regenerate()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader


        Dim sql As String = "BEGIN Select DISTINCT EmailAddress,Classification,Specific,ReportID,CC,valueParamdate,valueParamClass_01,valueParamClass_02,valueParamClass_03,valueParamClass_04,valueParamBranchCode from tbl_UnsentReports_V3 ORDER BY reportID END"
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Dim rpt As Generate
        Dim t As Thread
        Try

            While dr.Read
                rpt = New Generate

                rpt.eadd = dr.Item("EmailAddress").ToString
                rpt.clas = dr.Item("Classification").ToString
                rpt.id = dr.Item("ReportID").ToString
                rpt.spec = dr.Item("specific").ToString
                rpt.cc = dr.Item("CC").ToString
                rpt.vdate = CDate(dr.Item("valueParamdate").ToString)
                rpt.vClass_01 = dr.Item("valueParamClass_01").ToString
                rpt.vClass_02 = dr.Item("valueParamClass_02").ToString
                rpt.vClass_03 = dr.Item("valueParamClass_03").ToString
                rpt.vClass_04 = dr.Item("valueParamClass_04").ToString
                rpt.vbcode = dr.Item("valueParamBranchCode").ToString

                t = New Thread(AddressOf rpt.RegenerateReport)
                
                totalRunningReports()
                t.Start()
                Delay5(5)


            End While
            dr.Close()
        Catch ex As Exception

        End Try
        db.CloseConnection()
    End Sub
    Private Sub GetWeekly()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader


        Dim sql As String = "BEGIN Select DISTINCT EmailAddress,Classification,Specific,ReportID,CC,valueParamClass_01,valueParamClass_02,valueParamClass_03,valueParamClass_04,valueParamBranchCode from tbl_Recipients_V3 where Schedule='Weekly' and scheduleDate='" & CInt(Now.DayOfWeek) & "' ORDER BY reportID END"
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Dim rpt As Generate
        Dim t As Thread
        Try
            While dr.Read
                rpt = New Generate

                rpt.eadd = dr.Item("EmailAddress").ToString
                rpt.clas = dr.Item("Classification").ToString
                rpt.id = dr.Item("ReportID").ToString
                rpt.spec = dr.Item("specific").ToString
                rpt.cc = dr.Item("CC").ToString
                rpt.vClass_01 = dr.Item("valueParamClass_01").ToString
                rpt.vClass_02 = dr.Item("valueParamClass_02").ToString
                rpt.vClass_03 = dr.Item("valueParamClass_03").ToString
                rpt.vClass_04 = dr.Item("valueParamClass_04").ToString
                rpt.vbcode = dr.Item("valueParamBranchCode").ToString
                rpt.vdate = CStr(CDate(Now.ToShortDateString).AddDays(-1))

                t = New Thread(AddressOf rpt.GenerateReport)
               
                totalRunningReports()
                t.Start()
                Delay5(5)


            End While
            dr.Close()
        Catch ex As Exception

        End Try
        db.CloseConnection()
    End Sub
    Private Sub GetMonthly()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader

        Dim sql As String = "BEGIN Select DISTINCT EmailAddress,Classification,Specific,ReportID,CC,valueParamClass_01,valueParamClass_02,valueParamClass_03,valueParamClass_04,valueParamBranchCode from tbl_Recipients_V3 where Schedule='Monthly' and scheduledate='" & Now.Day & "' ORDER BY reportID END"
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Dim rpt As Generate
        Dim t As Thread
        Try
            While dr.Read
                rpt = New Generate

                rpt.eadd = dr.Item("EmailAddress").ToString
                rpt.clas = dr.Item("Classification").ToString
                rpt.id = dr.Item("ReportID").ToString
                rpt.spec = dr.Item("specific").ToString
                rpt.cc = dr.Item("CC").ToString
                rpt.vClass_01 = dr.Item("valueParamClass_01").ToString
                rpt.vClass_02 = dr.Item("valueParamClass_02").ToString
                rpt.vClass_03 = dr.Item("valueParamClass_03").ToString
                rpt.vClass_04 = dr.Item("valueParamClass_04").ToString
                rpt.vbcode = dr.Item("valueParamBranchCode").ToString
                rpt.vdate = CStr(CDate(Now.ToShortDateString).AddDays(-1))

                t = New Thread(AddressOf rpt.GenerateReport)
               
                totalRunningReports()
                t.Start()
                Delay5(5)

            End While
            dr.Close()
        Catch ex As Exception

        End Try
        db.CloseConnection()
    End Sub
    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.WindowState = FormWindowState.Minimized
        Me.NotifyIcon1.Visible = True
        e.Cancel = True
        Me.Visible = False

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.NotifyIcon1.Visible = False
        End
    End Sub

    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        Dim frmlog As New login
        frmlog.ShowDialog()
    End Sub
    Private Sub LoadSettings()
        Try
            Dim li_ret As Integer
            Dim ls_buff As New System.Text.StringBuilder(255)
            Dim ls_settings As String

            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "TIME", Now.ToShortTimeString, ls_buff, 256, ps_ini)
            ps_time = ls_buff.ToString
            ps_time = ps_time.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "DATE", Now.ToShortTimeString, ls_buff, 256, ps_ini)
            ps_date = ls_buff.ToString
            ps_date = ps_date.Substring(0, li_ret)

            'LOCAL SERVER
            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "SERVER", "", ls_buff, 256, ps_ini)
            ps_DBServerLocal = ls_buff.ToString
            ps_DBServerLocal = ps_DBServerLocal.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "DBNAME", "", ls_buff, 256, ps_ini)
            ps_DBNameLocal = ls_buff.ToString
            ps_DBNameLocal = ps_DBNameLocal.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "USERNAME", "", ls_buff, 256, ps_ini)
            ps_DBUsernameLocal = ls_buff.ToString
            ps_DBUsernameLocal = ps_DBUsernameLocal.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "PASSWORD", "", ls_buff, 256, ps_ini)
            ps_DBPasswordLocal = ls_buff.ToString
            ps_DBPasswordLocal = ps_DBPasswordLocal.Substring(0, li_ret)

            ls_connectionStringLocal = "user id=" & ps_DBUsernameLocal & ";password=" & ps_DBPasswordLocal & ";data source=" & ps_DBServerLocal & ";persist security info=False;initial catalog=" & ps_DBNameLocal

            'MAIL SERVER
            li_ret = GetPrivateProfileString("MAIL SERVER", "MAILSERVER", "", ls_buff, 256, ps_ini)
            ps_MailServer = ls_buff.ToString
            ps_MailServer = ps_MailServer.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("MAIL SERVER", "MAILADMIN", "", ls_buff, 256, ps_ini)
            ps_MailAdmin = ls_buff.ToString
            ps_MailAdmin = ps_MailAdmin.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("MAIL SERVER", "MAILUSERNAME", "", ls_buff, 256, ps_ini)
            ps_MailUsername = ls_buff.ToString
            ps_MailUsername = ps_MailUsername.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("MAIL SERVER", "MAILPASSWORD", "", ls_buff, 256, ps_ini)
            ps_MailPassword = ls_buff.ToString
            ps_MailPassword = ps_MailPassword.Substring(0, li_ret)

            'LUZON SERVER
            li_ret = GetPrivateProfileString("LUZON SERVER", "SERVER", "", ls_buff, 256, ps_ini)
            ps_DBServer = ls_buff.ToString
            ps_DBServer = ps_DBServer.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("LUZON SERVER", "DBNAME", "", ls_buff, 256, ps_ini)
            ps_DBName = ls_buff.ToString
            ps_DBName = ps_DBName.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("LUZON SERVER", "USERNAME", "", ls_buff, 256, ps_ini)
            ps_DBUsername = ls_buff.ToString
            ps_DBUsername = ps_DBUsername.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("LUZON SERVER", "PASSWORD", "", ls_buff, 256, ps_ini)
            ps_DBPassword = ls_buff.ToString
            ps_DBPassword = ps_DBPassword.Substring(0, li_ret)

            'VISMIN SERVER
            li_ret = GetPrivateProfileString("VISMIN SERVER", "SERVER", "", ls_buff, 256, ps_ini)
            ps_DBServerV = ls_buff.ToString
            ps_DBServerV = ps_DBServerV.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("VISMIN SERVER", "DBNAME", "", ls_buff, 256, ps_ini)
            ps_DBNameV = ls_buff.ToString
            ps_DBNameV = ps_DBNameV.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("VISMIN SERVER", "USERNAME", "", ls_buff, 256, ps_ini)
            ps_DBUsernameV = ls_buff.ToString
            ps_DBUsernameV = ps_DBUsernameV.Substring(0, li_ret)

            li_ret = GetPrivateProfileString("VISMIN SERVER", "PASSWORD", "", ls_buff, 256, ps_ini)
            ps_DBPasswordV = ls_buff.ToString
            ps_DBPasswordV = ps_DBPasswordV.Substring(0, li_ret)

            ls_connectionStringVismin = "user id=" & ps_DBUsernameV & ";password=" & ps_DBPasswordV & ";data source=" & ps_DBServerV & ";persist security info=False;initial catalog=" & ps_DBNameV

            If objDBLuzon.isConnected Then
                objDBLuzon.CloseConnection()
            End If
            If objDBVismin.isConnected Then
                objDBVismin.CloseConnection()
            End If


            ls_connectionStringLuzon = "user id=" & ps_DBUsername & ";password=" & ps_DBPassword & ";data source=" & ps_DBServer & ";persist security info=False;initial catalog=" & ps_DBName
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        lblTime.Text = Now.ToLongDateString + " " + Now.ToLongTimeString
        If mymsg <> msg Then
            Me.rbox.AppendText(vbNewLine + msg)
            mymsg = msg
        End If

        If report_counter > 0 Then
            Me.btnSetup.Enabled = False
            Me.Label2.Visible = True
            Me.Label5.Visible = True
            Me.Label5.Text = mymsg
        Else
            Me.btnSetup.Enabled = True
            Me.Label2.Visible = False
            Me.Label5.Visible = False
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        'Dim rpt As Generate
        Me.Timer2.Stop()
        Me.Timer2.Enabled = False
        Regenerate()
        Me.Timer2.Enabled = True
        Me.Timer2.Start()
    End Sub

    Private Sub frmMain_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Me.NotifyIcon1.Visible = False
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick

    End Sub
End Class
