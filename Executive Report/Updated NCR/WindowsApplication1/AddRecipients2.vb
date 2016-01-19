Imports System.Data.SqlClient
Public Class AddRecipient2
    Inherits System.Windows.Forms.Form
    Dim ctrMin As Integer

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
    Friend WithEvents txtEmailAdd As System.Windows.Forms.TextBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents lv As System.Windows.Forms.ListView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader18 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader19 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader21 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtCC As System.Windows.Forms.TextBox
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ColumnHeader20 As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbLuzon As System.Windows.Forms.RadioButton
    Friend WithEvents rbVismin As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddRecipient2))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtEmailAdd = New System.Windows.Forms.TextBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.lv = New System.Windows.Forms.ListView
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader21 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader15 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader16 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader17 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader18 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader19 = New System.Windows.Forms.ColumnHeader
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader20 = New System.Windows.Forms.ColumnHeader
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtCC = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbVismin = New System.Windows.Forms.RadioButton
        Me.rbLuzon = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(8, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Email Address:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtEmailAdd
        '
        Me.txtEmailAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmailAdd.Location = New System.Drawing.Point(112, 42)
        Me.txtEmailAdd.Name = "txtEmailAdd"
        Me.txtEmailAdd.Size = New System.Drawing.Size(226, 20)
        Me.txtEmailAdd.TabIndex = 1
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(895, 483)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 38)
        Me.cmdClose.TabIndex = 16
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(820, 484)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(72, 38)
        Me.cmdSave.TabIndex = 15
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'lv
        '
        Me.lv.Alignment = System.Windows.Forms.ListViewAlignment.[Default]
        Me.lv.AllowColumnReorder = True
        Me.lv.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader2, Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader14, Me.ColumnHeader21, Me.ColumnHeader15, Me.ColumnHeader16, Me.ColumnHeader17, Me.ColumnHeader18, Me.ColumnHeader19})
        Me.lv.FullRowSelect = True
        Me.lv.GridLines = True
        Me.lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lv.Location = New System.Drawing.Point(9, 327)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(965, 148)
        Me.lv.TabIndex = 17
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Report ID"
        Me.ColumnHeader7.Width = 70
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Report Name"
        Me.ColumnHeader2.Width = 353
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Classification"
        Me.ColumnHeader1.Width = 80
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Specific"
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "ValueParamDate"
        Me.ColumnHeader14.Width = 100
        '
        'ColumnHeader21
        '
        Me.ColumnHeader21.Text = "ScheduleDate"
        Me.ColumnHeader21.Width = 100
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "ValueParamClass_01"
        Me.ColumnHeader15.Width = 130
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "ValueParamClass_02"
        Me.ColumnHeader16.Width = 130
        '
        'ColumnHeader17
        '
        Me.ColumnHeader17.Text = "ValueParamClass_03"
        Me.ColumnHeader17.Width = 130
        '
        'ColumnHeader18
        '
        Me.ColumnHeader18.Text = "ValueParamClass_04"
        Me.ColumnHeader18.Width = 130
        '
        'ColumnHeader19
        '
        Me.ColumnHeader19.Text = "ValueParamBranchCode"
        Me.ColumnHeader19.Width = 130
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Gainsboro
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(892, 299)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(63, 25)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "&Add"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(968, 23)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "ASSIGN REPORTS"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ListView1
        '
        Me.ListView1.Alignment = System.Windows.Forms.ListViewAlignment.[Default]
        Me.ListView1.AllowColumnReorder = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader13, Me.ColumnHeader6, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader12, Me.ColumnHeader20})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView1.Location = New System.Drawing.Point(9, 100)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(965, 191)
        Me.ListView1.TabIndex = 52
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "ID"
        Me.ColumnHeader4.Width = 50
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Report Name"
        Me.ColumnHeader5.Width = 300
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Classification"
        Me.ColumnHeader13.Width = 80
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "ParamDate"
        Me.ColumnHeader6.Width = 100
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "ParamClass_01"
        Me.ColumnHeader8.Width = 100
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "ParamClass_02"
        Me.ColumnHeader9.Width = 100
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "ParamClass_03"
        Me.ColumnHeader10.Width = 92
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "ParamClass_04"
        Me.ColumnHeader11.Width = 100
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "ParamBranchCode"
        Me.ColumnHeader12.Width = 110
        '
        'ColumnHeader20
        '
        Me.ColumnHeader20.Text = "Server"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(4, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 24)
        Me.Label2.TabIndex = 53
        Me.Label2.Text = "Carbon Copy (CC):"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label2.Visible = False
        '
        'txtCC
        '
        Me.txtCC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCC.Location = New System.Drawing.Point(112, 68)
        Me.txtCC.Name = "txtCC"
        Me.txtCC.Size = New System.Drawing.Size(447, 20)
        Me.txtCC.TabIndex = 54
        Me.txtCC.Visible = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(352, 40)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 55
        Me.Button2.Text = "ADD CC"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbVismin)
        Me.GroupBox1.Controls.Add(Me.rbLuzon)
        Me.GroupBox1.Location = New System.Drawing.Point(585, 39)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(221, 52)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Location"
        '
        'rbVismin
        '
        Me.rbVismin.Location = New System.Drawing.Point(102, 20)
        Me.rbVismin.Name = "rbVismin"
        Me.rbVismin.Size = New System.Drawing.Size(104, 24)
        Me.rbVismin.TabIndex = 1
        Me.rbVismin.Text = "VISMIN"
        '
        'rbLuzon
        '
        Me.rbLuzon.Checked = True
        Me.rbLuzon.Location = New System.Drawing.Point(8, 19)
        Me.rbLuzon.Name = "rbLuzon"
        Me.rbLuzon.Size = New System.Drawing.Size(88, 24)
        Me.rbLuzon.TabIndex = 0
        Me.rbLuzon.TabStop = True
        Me.rbLuzon.Text = "NCR"
        '
        'AddRecipient2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(982, 524)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.txtCC)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.txtEmailAdd)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AddRecipient2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Assign Report"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private Sub AnalysisReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sql As String = "SELECT ID,ReportTitle,Classification,ISNULL(Paramdate,'') as ParamDate,ISNULL(ParamClass_01,'') as ParamClass_01,ISNULL(ParamClass_02,'') as ParamClass_02,ISNULL(ParamClass_03,'') as ParamClass_03,ISNULL(ParamClass_04,'') as ParamClass_04,ISNULL(ParamBranchCode,'') as ParamBranchCode,ISNULL(db,'') as server From tbl_Reports_V3 where RTRIM(Classification)='" & reportType.Trim & "' order by ID "
        getReports(sql)
    End Sub

    Private Sub getReports(ByVal sql As String)
        Dim dr As SqlClient.SqlDataReader
        If objDBLocal.isConnected Then
            objDBLocal.CloseConnection()
        End If
        objDBLocal.ConnectDB(ls_connectionStringLocal)
        dr = objDBLocal.Execute_SQL_DataReader(sql)
        Dim ctr As Integer = 0
        Try
            ListView1.Items.Clear()


            While dr.Read
                ListView1.Items.Add(Trim(CStr(dr.Item("ID"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportTitle"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("Classification"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamDate"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_01"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_02"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_03"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamClass_04"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ParamBranchCode"))))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("server"))))
                If ctr Mod 2 = 0 Then
                    Me.ListView1.Items(ctr).BackColor = Color.LightBlue
                End If
                ctr = ctr + 1
            End While
            dr.Close()
        Catch ex As Exception

        End Try
        objDBLocal.CloseConnection()
    End Sub
    Private Function getAssignReport() As String
        Dim ctr As Integer = 0
        Dim asReport As String = ","
        For ctr = 0 To lv.Items.Count - 1
            asReport = asReport + lv.Items(ctr).SubItems.Item(0).Text + ","
        Next
        getAssignReport = asReport
    End Function
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        reportType = ""
        Me.Close()
    End Sub
    Private Sub dg_Navigate_1(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs)

    End Sub
    Private Sub lv_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lv.KeyPress

        If UCase(e.KeyChar) = ChrW(68) Then
            Dim ans As MsgBoxResult = MsgBox("Delete selected item?", MsgBoxStyle.YesNo, "Email Scheduler V3.0")
            If ans = MsgBoxResult.Yes Then
                lv.Items.RemoveAt(lv.SelectedIndices(0))
            End If

        End If

    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

        Try
            Id = Me.ListView1.FocusedItem.SubItems.Item(0).Text.Trim
            reportName = Me.ListView1.FocusedItem.SubItems.Item(1).Text.Trim
            classification = Me.ListView1.FocusedItem.SubItems.Item(2).Text.Trim
            'class_01 = Me.ListView1.FocusedItem.SubItems.Item(3).Text.Trim
            paramDate = Me.ListView1.FocusedItem.SubItems.Item(3).Text.Trim
            paramClass_01 = Me.ListView1.FocusedItem.SubItems.Item(4).Text.Trim
            paramClass_02 = Me.ListView1.FocusedItem.SubItems.Item(5).Text.Trim
            paramClass_03 = Me.ListView1.FocusedItem.SubItems.Item(6).Text.Trim
            paramClass_04 = Me.ListView1.FocusedItem.SubItems.Item(7).Text.Trim
            paramBranchCode = Me.ListView1.FocusedItem.SubItems.Item(8).Text.Trim
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Dim frm As New AssignParam
        frm.ShowDialog()
        If isParamOk = True Then
            addReport()

        End If
    End Sub
    Private Sub addReport()
        Dim ctr As Integer

        For ctr = 0 To lv.Items.Count - 1
            If Id = Me.lv.Items(0).Text.Trim Then
                MsgBox("Report is already added.", MsgBoxStyle.Critical, "System Error")
                Exit Sub
            End If
        Next

        lv.Items.Add(Trim(CStr(Id)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(reportName)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(classification)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(spec)
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strparamDate)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strParamdateDesc)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strparamClass_01)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strparamClass_02)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strparamClass_03)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strparamClass_04)))
        lv.Items(lv.Items.Count - 1).SubItems.Add(Trim(CStr(strparamBranchCode)))

        For ctr = 0 To lv.Items.Count - 1
            If ctr Mod 2 = 0 Then
                Me.lv.Items(ctr).BackColor = Color.LightBlue
            End If
        Next



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim frm As New AssignParam
        frm.ShowDialog()
        If isParamOk = True Then
            addReport()

        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If Me.txtEmailAdd.Text = "" Then
            MsgBox("Please enter the email address.", MsgBoxStyle.Critical, "System Error")
            Me.txtEmailAdd.Focus()
            Me.txtEmailAdd.Select()
            Exit Sub
        End If
        If lv.Items.Count < 1 Then
            MsgBox("Please select report/s to be sent.", MsgBoxStyle.Critical, "System Error")
            Exit Sub
        End If
        If Me.txtCC.Visible = True Then
            If Me.txtCC.Text = "" Then
                MsgBox("Please enter recipient of the carbon copy(CC).", MsgBoxStyle.Critical, "System Error")
                Exit Sub
            End If
        End If
        Dim ctr As Integer
        Dim sql As String
        Dim clas, spec, CC, ID, vClass_01, vClass_02, vClass_03, vClass_04, vbcode, schedDAte, vdate As String

        Dim db As New clsDBConnection

        For ctr = 0 To lv.Items.Count - 1
            ID = lv.Items(ctr).SubItems(0).Text.Trim
            clas = lv.Items(ctr).SubItems(2).Text.Trim
            spec = lv.Items(ctr).SubItems(3).Text.Trim
            vdate = lv.Items(ctr).SubItems(4).Text.Trim
            schedDAte = lv.Items(ctr).SubItems(5).Text.Trim
            vClass_01 = lv.Items(ctr).SubItems(6).Text.Trim
            vClass_02 = lv.Items(ctr).SubItems(7).Text.Trim
            vClass_03 = lv.Items(ctr).SubItems(8).Text.Trim
            vClass_04 = lv.Items(ctr).SubItems(9).Text.Trim
            vbcode = lv.Items(ctr).SubItems(10).Text.Trim
            sql = "Insert into tbl_Recipients_V3(EmailAddress,Classification,Specific,reportID,CC,ValueParamDate,ValueParamClass_01,ValueParamClass_02,ValueParamClass_03,ValueParamClass_04,ValueParamBranchCode,Schedule,ScheduleDate)" & _
            "VALUES('" & Me.txtEmailAdd.Text.Trim & "','" & clas & "','" & spec & "','" & ID & "','" & Me.txtCC.Text.Trim & "','" & vdate & "','" & vClass_01 & "','" & vClass_02 & "','" & vClass_03 & "','" & vClass_04 & "','" & vbcode & "','" & vdate & "','" & schedDAte & "')"
            If db.isConnected Then
                db.CloseConnection()
            End If
            db.ConnectDB(ls_connectionStringLocal)
            If db.Execute_SQLQuery(sql) < 1 Then
                MsgBox("Can not save new data. Please contact the administrator.", MsgBoxStyle.Critical, "Saving Error")
                Exit Sub
            End If
            db.CloseConnection()
        Next
        MsgBox("Done")
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If txtEmailAdd.TextLength < 5 Then
            MsgBox("Please provide an Email Address..", MsgBoxStyle.Critical, "System Error")
            txtEmailAdd.Focus()
            txtEmailAdd.Select()
            Exit Sub
        End If

        If Button2.Text = "ADD CC" Then
            Button2.Text = "Remove CC"
            Label2.Visible = True
            txtCC.Visible = True
            txtCC.Focus()
            txtCC.Select()

        ElseIf Button2.Text = "Remove CC" Then
            Button2.Text = "ADD CC"
            Label2.Text = ""
            txtCC.Text = ""
            Label2.Visible = False
            txtCC.Visible = False
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub rbLuzon_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLuzon.CheckedChanged, rbVismin.CheckedChanged
        Dim sql As String
        If rbLuzon.Checked Then
            sql = "SELECT ID,ReportTitle,Classification,ISNULL(Paramdate,'') as ParamDate,ISNULL(ParamClass_01,'') as ParamClass_01,ISNULL(ParamClass_02,'') as ParamClass_02,ISNULL(ParamClass_03,'') as ParamClass_03,ISNULL(ParamClass_04,'') as ParamClass_04,ISNULL(ParamBranchCode,'') as ParamBranchCode,ISNULL(db,'') as server From tbl_Reports_V3 where db='synergyNCR'and RTRIM(Classification)='" & reportType.Trim & "' order by ID "
        Else
            sql = "SELECT ID,ReportTitle,Classification,ISNULL(Paramdate,'') as ParamDate,ISNULL(ParamClass_01,'') as ParamClass_01,ISNULL(ParamClass_02,'') as ParamClass_02,ISNULL(ParamClass_03,'') as ParamClass_03,ISNULL(ParamClass_04,'') as ParamClass_04,ISNULL(ParamBranchCode,'') as ParamBranchCode,ISNULL(db,'') as server From tbl_Reports_V3 where db='synergyvismin'and RTRIM(Classification)='" & reportType.Trim & "' order by ID "
        End If
        Call getReports(sql)

    End Sub
End Class
