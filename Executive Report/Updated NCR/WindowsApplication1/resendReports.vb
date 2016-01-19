Imports System.Data.SqlClient
Imports System.Threading
Imports System.Net.Mail
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Imports Microsoft.VisualBasic
Imports SendToMail_DLL

Public Class resendReports
    Inherits System.Windows.Forms.Form
    Dim def_dir As String

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents lbldir As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lv As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lvEmail As System.Windows.Forms.ListView
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents btnHide As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnSendAll As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(resendReports))
        Me.cmdClose = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button1 = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.lbldir = New System.Windows.Forms.Label
        Me.lv = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtSubject = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lvEmail = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.btnHide = New System.Windows.Forms.Button
        Me.btnSendAll = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.cmdClose.Location = New System.Drawing.Point(814, 393)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(73, 47)
        Me.cmdClose.TabIndex = 18
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(928, 36)
        Me.Label3.TabIndex = 50
        Me.Label3.Text = "RESEND REPORTS"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Gainsboro
        Me.Button2.Enabled = False
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button2.Location = New System.Drawing.Point(661, 394)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(73, 46)
        Me.Button2.TabIndex = 51
        Me.Button2.Text = "S&end"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(60, 56)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 54
        Me.Button1.Text = "Open Directory"
        '
        'lbldir
        '
        Me.lbldir.Location = New System.Drawing.Point(144, 57)
        Me.lbldir.Name = "lbldir"
        Me.lbldir.Size = New System.Drawing.Size(762, 23)
        Me.lbldir.TabIndex = 55
        Me.lbldir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lv
        '
        Me.lv.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader1})
        Me.lv.FullRowSelect = True
        Me.lv.GridLines = True
        Me.lv.Location = New System.Drawing.Point(8, 103)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(925, 248)
        Me.lv.TabIndex = 56
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "File Name"
        Me.ColumnHeader3.Width = 457
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "          File Path"
        Me.ColumnHeader1.Width = 834
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(10, 359)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 23)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "Email Address:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(10, 389)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 23)
        Me.Label2.TabIndex = 59
        Me.Label2.Text = "Subject:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSubject
        '
        Me.txtSubject.Location = New System.Drawing.Point(98, 389)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(334, 20)
        Me.txtSubject.TabIndex = 60
        Me.txtSubject.Text = ""
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(435, 362)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(28, 23)
        Me.btnSearch.TabIndex = 61
        Me.btnSearch.Text = "..."
        '
        'lvEmail
        '
        Me.lvEmail.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lvEmail.FullRowSelect = True
        Me.lvEmail.Location = New System.Drawing.Point(450, 263)
        Me.lvEmail.Name = "lvEmail"
        Me.lvEmail.Size = New System.Drawing.Size(229, 97)
        Me.lvEmail.TabIndex = 62
        Me.lvEmail.View = System.Windows.Forms.View.Details
        Me.lvEmail.Visible = False
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = ""
        Me.ColumnHeader2.Width = 500
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(98, 363)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(334, 20)
        Me.txtEmail.TabIndex = 63
        Me.txtEmail.Text = ""
        '
        'btnHide
        '
        Me.btnHide.Location = New System.Drawing.Point(654, 264)
        Me.btnHide.Name = "btnHide"
        Me.btnHide.Size = New System.Drawing.Size(22, 18)
        Me.btnHide.TabIndex = 64
        Me.btnHide.Text = "X"
        Me.btnHide.Visible = False
        '
        'btnSendAll
        '
        Me.btnSendAll.BackColor = System.Drawing.Color.Gainsboro
        Me.btnSendAll.Enabled = False
        Me.btnSendAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSendAll.Image = CType(resources.GetObject("btnSendAll.Image"), System.Drawing.Image)
        Me.btnSendAll.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSendAll.Location = New System.Drawing.Point(737, 394)
        Me.btnSendAll.Name = "btnSendAll"
        Me.btnSendAll.Size = New System.Drawing.Size(73, 46)
        Me.btnSendAll.TabIndex = 65
        Me.btnSendAll.Text = "Send &All"
        Me.btnSendAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'resendReports
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(940, 456)
        Me.Controls.Add(Me.btnSendAll)
        Me.Controls.Add(Me.btnHide)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.lvEmail)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSubject)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.lbldir)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "resendReports"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Resend Reports"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub resendReports_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If openFile() = False Then
            MsgBox("You must select a directory of the file to be sent.", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
            Me.Close()
        End If

    End Sub
    Private Function openFile() As Boolean
        Dim FolderBrowserDialog1 As New FolderBrowserDialog
        Dim di As IO.DirectoryInfo
        Dim aryFi As IO.FileInfo()
        Dim fi As IO.FileInfo

        lv.Items.Clear()

        With FolderBrowserDialog1
            .RootFolder = Environment.SpecialFolder.Desktop
            .SelectedPath = Application.StartupPath & "\Export"
            .Description = "Select the source directory"
            If .ShowDialog = DialogResult.OK Then
                openFile = True
                def_dir = .SelectedPath
                lbldir.Text = def_dir
                Try
                    If def_dir <> "" Then
                        di = New IO.DirectoryInfo(def_dir)
                        aryFi = di.GetFiles("*.pdf")
                    End If
                    lbldir.Text = def_dir
                    For Each fi In aryFi
                        lv.Items.Add(fi.Name)
                        lv.Items(lv.Items.Count - 1).SubItems.Add(fi.FullName)
                    Next
                    If lv.Items.Count > 0 Then
                        Me.Button2.Enabled = True
                        Me.btnSendAll.Enabled = True
                    Else
                        MsgBox("The selected directory does not contain a file with .pdf extension.", MsgBoxStyle.Information, "Email Scheduler  V3.0")
                        Me.Button2.Enabled = False
                        Me.btnSendAll.Enabled = False
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End If
        End With
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        openFile()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Me.txtEmail.Text = "" Then
            MsgBox("Please enter an email address.", MsgBoxStyle.Critical, "Sending Error")
            Me.txtEmail.Focus()
            Me.txtEmail.Select()
            Exit Sub
        End If

        If Me.txtSubject.Text = "" Then
            MsgBox("Please enter a subject.", MsgBoxStyle.Critical, "Sending Error")
            Me.txtSubject.Focus()
            Me.txtSubject.Select()
            Exit Sub
        End If


        Dim email As New SendToMail_DLL.SendToEmail

        'If Not email.send(ps_MailAdmin, Me.txtEmail.Text, "", Me.txtSubject.Text, "This is an auto-email. Do not reply.", Me.lv.FocusedItem.SubItems.Item(1).Text, ps_MailServer, ps_MailUsername, ps_MailPassword) Then
        '    MsgBox("Not Send")
        '    MsgBox(email.GetError())
        'Else
        '    MsgBox("Success")
        'End If

        If ReSend(Me.txtEmail.Text, Me.txtSubject.Text, Me.lv.FocusedItem.SubItems.Item(1).Text) = False Then
            MsgBox("Sending error. Check error log for more details.", MsgBoxStyle.Information, "Error")
        Else
             MsgBox("Done")
            x = 0
            y = 0
        End If
    End Sub
    Public Function ReSend(ByVal rec As String, ByVal subj As String, ByVal attach As String) As Boolean
        Try

            Dim send As New SendToEmail
            If send.send(ps_MailAdmin, rec, "", subj, "", attach, ps_MailServer, ps_MailUsername, ps_MailPassword) Then
                Call Log(Now.ToString + ":" + "Successful Sending : " + subj + " to " + rec + vbCrLf)
                msg = Now.ToString & " : " & subj & " is successfully sent to " & rec
            End If

            ReSend = True
        Catch ex As Exception
            LogError(vbNewLine & Now.ToString & " : " & subj & " is UNSUCCESSFULY sent to " & rec & vbNewLine & ex.Message)
            ReSend = False
        End Try
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader
        Dim sql As String = "SELECT distinct EmailAddress from tbl_Recipients_V3"
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        lvEmail.Items.Clear()
        Try
            While dr.Read
                lvEmail.Items.Add(dr.Item("EmailAddress"))
            End While
        Catch ex As Exception

        End Try
        Me.btnHide.Visible = True
        Me.lvEmail.Visible = True
        db.CloseConnection()

    End Sub

    Private Sub lvEmail_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvEmail.SelectedIndexChanged
        Me.txtEmail.Text = lvEmail.FocusedItem.Text
    End Sub

    Private Sub btnHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHide.Click
        Me.btnHide.Visible = False
        Me.lvEmail.Visible = False
    End Sub

    Private Sub lv_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lv.SelectedIndexChanged
        Me.txtSubject.Text = lv.FocusedItem.SubItems.Item(0).Text
    End Sub

    Private Sub lvEmail_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvEmail.LostFocus
        Me.lvEmail.Visible = False
        Me.btnHide.Visible = False
    End Sub

    Private Sub lvEmail_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvEmail.MouseLeave
        Me.lvEmail.Visible = False
        Me.btnHide.Visible = False
    End Sub

    Private Sub btnSendAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendAll.Click
        If lv.Items.Count > 0 Then
            Dim ctr As Integer
            Dim subj As String
            Dim path As String
            Dim sp As New clsSplash
            Dim check As Boolean = True
            sp.ShowSplash("Please wait while sending.....")
            For ctr = 0 To lv.Items.Count - 1
                subj = Me.lv.Items(ctr).SubItems.Item(0).Text.Trim
                path = Me.lv.Items(ctr).SubItems.Item(1).Text.Trim
                If ReSend(Me.txtEmail.Text, subj, path) = False Then
                    check = False
                End If
            Next
          
            sp.CloseSplash()
            If check = False Then
                MsgBox("There is an error while sending. Please view log error for more details.", MsgBoxStyle.Information, "Error")
            Else
                MsgBox("Done")
                x = 0
                y = 0
            End If
        End If
    End Sub
End Class
