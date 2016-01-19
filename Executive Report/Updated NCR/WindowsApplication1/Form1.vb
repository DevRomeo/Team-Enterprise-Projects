Imports System.Data.SqlClient
Imports System.Web.Mail
Imports System.Threading
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim id, rptname, rptpath, pdate, pClass_01, pClass_02, pClass_03, pClass_04, pbcode As String
    Dim isload As Boolean

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbEmailAdd As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbArea As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbRegion As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDivision As System.Windows.Forms.ComboBox
    Friend WithEvents lvReport As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmbClass_01 As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtBcode As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbEmailAdd = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmbArea = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtDate = New System.Windows.Forms.DateTimePicker
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbRegion = New System.Windows.Forms.ComboBox
        Me.cmbDivision = New System.Windows.Forms.ComboBox
        Me.cmbClass_01 = New System.Windows.Forms.ComboBox
        Me.lvReport = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtBcode = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(17, 202)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Email to: "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbEmailAdd
        '
        Me.cmbEmailAdd.Location = New System.Drawing.Point(98, 202)
        Me.cmbEmailAdd.Name = "cmbEmailAdd"
        Me.cmbEmailAdd.Size = New System.Drawing.Size(386, 21)
        Me.cmbEmailAdd.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(23, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Send Report: "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Gainsboro
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(473, 379)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(73, 34)
        Me.Button2.TabIndex = 53
        Me.Button2.Text = "&Send"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(549, 378)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(73, 35)
        Me.cmdClose.TabIndex = 52
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbArea
        '
        Me.cmbArea.Location = New System.Drawing.Point(99, 385)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.Size = New System.Drawing.Size(250, 21)
        Me.cmbArea.TabIndex = 55
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(15, 384)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 17)
        Me.Label3.TabIndex = 54
        Me.Label3.Text = "Area: "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(100, 226)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(199, 24)
        Me.Label4.TabIndex = 56
        Me.Label4.Text = "(You can modify the email address.)"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(18, 269)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 23)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "Date: "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDate
        '
        Me.txtDate.CustomFormat = "yyyy-MM-dd"
        Me.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.txtDate.Location = New System.Drawing.Point(100, 267)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.Size = New System.Drawing.Size(94, 20)
        Me.txtDate.TabIndex = 58
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(16, 301)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 23)
        Me.Label6.TabIndex = 59
        Me.Label6.Text = "Head Office:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(19, 329)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(69, 14)
        Me.Label7.TabIndex = 60
        Me.Label7.Text = "Division:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(15, 354)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 16)
        Me.Label8.TabIndex = 61
        Me.Label8.Text = "Region: "
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbRegion
        '
        Me.cmbRegion.Location = New System.Drawing.Point(99, 355)
        Me.cmbRegion.Name = "cmbRegion"
        Me.cmbRegion.Size = New System.Drawing.Size(249, 21)
        Me.cmbRegion.TabIndex = 62
        '
        'cmbDivision
        '
        Me.cmbDivision.Location = New System.Drawing.Point(99, 324)
        Me.cmbDivision.Name = "cmbDivision"
        Me.cmbDivision.Size = New System.Drawing.Size(248, 21)
        Me.cmbDivision.TabIndex = 63
        '
        'cmbClass_01
        '
        Me.cmbClass_01.Location = New System.Drawing.Point(100, 295)
        Me.cmbClass_01.Name = "cmbClass_01"
        Me.cmbClass_01.Size = New System.Drawing.Size(248, 21)
        Me.cmbClass_01.TabIndex = 64
        '
        'lvReport
        '
        Me.lvReport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.lvReport.FullRowSelect = True
        Me.lvReport.GridLines = True
        Me.lvReport.Location = New System.Drawing.Point(98, 12)
        Me.lvReport.Name = "lvReport"
        Me.lvReport.Size = New System.Drawing.Size(521, 181)
        Me.lvReport.TabIndex = 65
        Me.lvReport.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "ID"
        Me.ColumnHeader1.Width = 30
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Report Title"
        Me.ColumnHeader2.Width = 200
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Report Path"
        Me.ColumnHeader3.Width = 280
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "ParamDate"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "ParamClass_01"
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "ParamClass_02"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "ParamClass_03"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "ParamClass_04"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "ParamBcode"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(10, 415)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(77, 17)
        Me.Label9.TabIndex = 66
        Me.Label9.Text = "Branch Code: "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBcode
        '
        Me.txtBcode.Location = New System.Drawing.Point(99, 413)
        Me.txtBcode.MaxLength = 3
        Me.txtBcode.Name = "txtBcode"
        Me.txtBcode.Size = New System.Drawing.Size(46, 20)
        Me.txtBcode.TabIndex = 67
        Me.txtBcode.Text = ""
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(629, 456)
        Me.Controls.Add(Me.txtBcode)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lvReport)
        Me.Controls.Add(Me.cmbClass_01)
        Me.Controls.Add(Me.cmbDivision)
        Me.Controls.Add(Me.cmbRegion)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbArea)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbEmailAdd)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Regenerate Reports"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        isload = True
        Call displayReport()
        Call setValues()
        Call AssignParam()
        isload = False
    End Sub

    Private Sub displayEmailAddress(ByVal str As String)
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader
        Dim sql As String

        If db.isConnected = True Then
            db.CloseConnection()
        End If
        db.ConnectDB(Common.ls_connectionStringLocal)

        sql = "Select distinct EmailAddress from tbl_Recipients_V3 where ValueParamClass_01='" & str & "'"
        Me.cmbEmailAdd.Items.Clear()
        dr = db.Execute_SQL_DataReader(sql)
        If Not dr Is Nothing Then
            If dr.HasRows Then
                While dr.Read
                    Me.cmbEmailAdd.Items.Add(dr.Item("EmailAddress"))
                End While
                Me.cmbEmailAdd.Text = Me.cmbEmailAdd.Items.Item(0)
            Else
                MsgBox("No Email address found in database.", MsgBoxStyle.Information)
            End If
        Else
            MsgBox("No Email address found in database.", MsgBoxStyle.Information)
        End If
        dr.Close()
        db.CloseConnection()


    End Sub
    Private Sub displayReport()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader
        Dim sql As String

        If db.isConnected = True Then
            db.CloseConnection()
        End If
        db.ConnectDB(Common.ls_connectionStringLocal)

        sql = "Select ID, ReportTitle,reportName,Paramdate,ParamClass_01,ParamClass_02,ParamClass_03,ParamClass_04,ParamBranchCode from tbl_Reports_V3"
        dr = db.Execute_SQL_DataReader(sql)
        If Not dr Is Nothing Then
            If dr.HasRows Then
                While dr.Read
                    lvReport.Items.Add(Trim(CStr(dr.Item("ID"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportTitle"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportName"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamDate"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_01"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_02"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_03"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_04"))))
                    lvReport.Items(lvReport.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamBranchCode"))))
                End While
                If lvReport.Items.Count > 0 Then
                    lvReport.Items(0).Selected = True
                    lvReport.Select()
                    lvReport.Items(0).Focused = True
                End If
            Else
                MsgBox("No report found in database.", MsgBoxStyle.Information)
            End If
        Else
            MsgBox("No report found in database.", MsgBoxStyle.Information)
        End If
        dr.Close()
        db.CloseConnection()
    End Sub
    Private Sub displayArea()
        Dim db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader
        Dim sql As String

        If db.isConnected = True Then
            db.CloseConnection()
        End If
        db.ConnectDB(Common.ls_connectionStringLocal)
        cmbArea.Items.Clear()
        If cmbRegion.Enabled = True Then
            sql = "SELECT DISTINCT valueparamClass_04 FROM tbl_Recipients_V3 where valueParamClass_03='" & Me.cmbRegion.Text.Trim & "'"
        ElseIf cmbDivision.Enabled = True Then
            sql = "SELECT DISTINCT valueparamClass_04 FROM tbl_Recipients_V3 where valueParamClass_02='" & Me.cmbDivision.Text.Trim & "'"
        ElseIf cmbClass_01.Enabled = True Then
            sql = "SELECT DISTINCT valueparamClass_04 FROM tbl_Recipients_V3 where valueParamClass_01='" & Me.cmbClass_01.Text.Trim & "'"
        Else
            sql = "SELECT DISTINCT valueparamClass_04 FROM tbl_Recipients_V3 where reportID='" & id & "'"
        End If


        Me.cmbArea.Items.Clear()
        dr = db.Execute_SQL_DataReader(sql)
        If Not dr Is Nothing Then
            If dr.HasRows Then
                While dr.Read
                    Me.cmbArea.Items.Add(dr.Item("valueparamClass_04").trim)
                End While
                Me.cmbArea.Text = Me.cmbArea.Items.Item(0).Trim
            Else
                MsgBox("No area found in database.", MsgBoxStyle.Information)
            End If
        Else
            MsgBox("No area found in database.", MsgBoxStyle.Information)
        End If
        dr.Close()
        db.CloseConnection()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call SendReport()
    End Sub
    Private Sub SendReport()
        splash.ShowSplash("Please wait while generating/sending...")

        If Me.cmbEmailAdd.Text = "" Then
            MsgBox("Enter a valid email address.", MsgBoxStyle.Critical, "Sending Error")
            Me.cmbEmailAdd.Focus()
            Me.cmbEmailAdd.Select()
            Exit Sub
        End If

        If cmbClass_01.Enabled = True And cmbClass_01.Text = "" Then
            MsgBox("Please enter a valid value for paramter.", MsgBoxStyle.Critical, "Sending Error")
            cmbClass_01.Focus()
            cmbClass_01.Select()
            Exit Sub
        End If

        If cmbDivision.Enabled = True And cmbDivision.Text = "" Then
            MsgBox("Please enter a valid value for paramter.", MsgBoxStyle.Critical, "Sending Error")
            cmbDivision.Focus()
            cmbDivision.Select()
            Exit Sub
        End If

        If cmbRegion.Enabled = True And cmbRegion.Text = "" Then
            MsgBox("Please enter a valid value for paramter.", MsgBoxStyle.Critical, "Sending Error")
            cmbRegion.Focus()
            cmbRegion.Select()
            Exit Sub
        End If
        If cmbArea.Enabled = True And cmbArea.Text = "" Then
            MsgBox("Please enter a valid value for paramter.", MsgBoxStyle.Critical, "Sending Error")
            cmbArea.Focus()
            cmbArea.Select()
            Exit Sub
        End If

        If txtBcode.Enabled = True And IsNumeric(txtBcode.Text) = False Then
            MsgBox("Please enter a valid value for paramter.", MsgBoxStyle.Critical, "Sending Error")
            txtBcode.Focus()
            txtBcode.Select()
        End If

        Call GenerateReport()
        splash.CloseSplash()

    End Sub
    Private Function getRegion()
        Dim db As New clsDBConnection
        Dim sql As String
        If cmbDivision.Enabled = True Then
            sql = "SELECT DISTINCT valueparamClass_03 FROM tbl_Recipients_V3 where valueParamClass_02='" & Me.cmbDivision.Text.Trim & "'"
        ElseIf cmbClass_01.Enabled = True Then
            sql = "SELECT DISTINCT valueparamClass_03 FROM tbl_Recipients_V3 where valueParamClass_01='" & Me.cmbClass_01.Text.Trim & "'"
        Else
            sql = "SELECT DISTINCT valueparamClass_03 FROM tbl_Recipients_V3 where reportID='" & id & "'"
        End If
        Dim dr As SqlDataReader
        If db.isConnected Then
            db.CloseConnection()
        End If

        cmbRegion.Items.Clear()

        db.ConnectDB(Common.ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        If Not dr Is Nothing Then
            If dr.HasRows Then
                While dr.Read
                    cmbRegion.Items.Add(dr.Item("valueparamClass_03").trim)
                End While
                Me.cmbRegion.Text = Me.cmbRegion.Items.Item(0).trim
            End If
        End If

    End Function
    Private Function getCLass_01()
        Dim db As New clsDBConnection
        Dim sql As String = "SELECT DISTINCT valueparamClass_01 FROM tbl_Recipients_V3 where reportID='" & id & "'"
        Dim dr As SqlDataReader
        If db.isConnected Then
            db.CloseConnection()
        End If
        cmbClass_01.Items.Clear()
        db.ConnectDB(Common.ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        If Not dr Is Nothing Then
            If dr.HasRows Then
                While dr.Read
                    cmbClass_01.Items.Add(dr.Item("valueparamClass_01").trim)
                End While
                Me.cmbClass_01.Text = Me.cmbClass_01.Items.Item(0)
            End If
        End If

    End Function
    Private Function getCLass_02()
        Dim db As New clsDBConnection
        Dim sql As String

        If cmbClass_01.Enabled = True Then
            sql = "SELECT DISTINCT valueparamClass_02 FROM tbl_Recipients_V3 where valueParamClass_01='" & cmbClass_01.Text.Trim & "'"
        Else
            sql = "SELECT DISTINCT valueparamClass_02 FROM tbl_Recipients_V3 where reportID='" & id & "'"
        End If



        Dim dr As SqlDataReader
        If db.isConnected Then
            db.CloseConnection()
        End If
        cmbDivision.Items.Clear()
        db.ConnectDB(Common.ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Try


            If Not dr Is Nothing Then
                If dr.HasRows Then
                    While dr.Read
                        cmbDivision.Items.Add(dr.Item("valueparamClass_02").trim)
                    End While
                    Me.cmbDivision.Text = Me.cmbDivision.Items.Item(0).trim
                End If
            End If
        Catch ex As Exception

        End Try

    End Function

    Private Sub GenerateReport()

        Dim g As New Generate

        g.eadd = cmbEmailAdd.Text
        g.id = Me.id
        g.clas = "REGENERATED"
        g.spec = Replace(CStr(Now.Date), "/", "-")
        g.vClass_01 = cmbClass_01.Text
        g.vClass_02 = cmbDivision.Text
        g.vClass_03 = cmbRegion.Text
        g.vClass_04 = cmbArea.Text
        g.vdate = Me.txtDate.Text
        g.vbcode = txtBcode.Text
        g.cc = ""
        g.vdate = Me.txtDate.Text


        g.RegenerateReport1()




    End Sub
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub lvReport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvReport.SelectedIndexChanged
        If lvReport.Items.Count > 0 And isload = False Then
            Try
                id = Me.lvReport.FocusedItem.SubItems.Item(0).Text.Trim
                rptname = Me.lvReport.FocusedItem.SubItems.Item(1).Text.Trim
                rptpath = Me.lvReport.FocusedItem.SubItems.Item(2).Text.Trim
                pdate = Me.lvReport.FocusedItem.SubItems.Item(3).Text.Trim
                pClass_01 = Me.lvReport.FocusedItem.SubItems.Item(4).Text.Trim
                pClass_02 = Me.lvReport.FocusedItem.SubItems.Item(5).Text.Trim
                pClass_03 = Me.lvReport.FocusedItem.SubItems.Item(6).Text.Trim
                pClass_04 = Me.lvReport.FocusedItem.SubItems.Item(7).Text.Trim
                pbcode = Me.lvReport.FocusedItem.SubItems.Item(8).Text.Trim

                Call AssignParam()

            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub AssignParam()

        If pdate = "" Then
            txtDate.Enabled = False
        Else
            txtDate.Enabled = True
        End If

        If pClass_01 = "" Then
            cmbClass_01.Enabled = False
        Else
            cmbClass_01.Enabled = True
            getCLass_01()
        End If

        If pClass_02 = "" Then
            cmbDivision.Enabled = False
        Else
            cmbDivision.Enabled = True
            Call getCLass_02()
        End If
        If pClass_03 = "" Then
            cmbRegion.Enabled = False
        Else
            cmbRegion.Enabled = True
            getRegion()
        End If

        If pClass_04 = "" Then
            cmbArea.Enabled = False
        Else
            cmbArea.Enabled = True
            Call displayArea()
        End If
        If pbcode = "" Then
            txtBcode.Enabled = False
        Else
            txtBcode.Enabled = True
        End If
    End Sub
    Private Sub setValues()
        If lvReport.Items.Count > 0 Then
            id = Me.lvReport.Items(0).SubItems.Item(0).Text.Trim
            rptname = Me.lvReport.Items(0).SubItems.Item(1).Text.Trim
            rptpath = Me.lvReport.Items(0).SubItems.Item(2).Text.Trim
            pdate = Me.lvReport.Items(0).SubItems.Item(3).Text.Trim
            pClass_01 = Me.lvReport.Items(0).SubItems.Item(4).Text.Trim
            pClass_02 = Me.lvReport.Items(0).SubItems.Item(5).Text.Trim
            pClass_03 = Me.lvReport.Items(0).SubItems.Item(6).Text.Trim
            pClass_04 = Me.lvReport.Items(0).SubItems.Item(7).Text.Trim
            pbcode = Me.lvReport.Items(0).SubItems.Item(8).Text.Trim
        End If

    End Sub

    Private Sub cmbRegion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRegion.SelectedIndexChanged
        If cmbArea.Enabled = True Then
            displayArea()
        End If
    End Sub

    Private Sub cmbClass_01_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbClass_01.SelectedIndexChanged
        If cmbDivision.Enabled = True Then
            getCLass_02()
        ElseIf cmbRegion.Enabled = True Then
            getRegion()
        ElseIf cmbArea.Enabled = True Then
            displayArea()
        End If
    End Sub

    Private Sub cmbDivision_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDivision.SelectedIndexChanged
        If cmbRegion.Enabled = True Then
            getRegion()
        ElseIf cmbArea.Enabled = True Then
            displayArea()
        End If
    End Sub
End Class
