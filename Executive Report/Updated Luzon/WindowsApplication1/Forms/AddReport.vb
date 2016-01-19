Public Class AddReport
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
    Friend WithEvents txtReportName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbClassification As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents txtDb As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkclass_01 As System.Windows.Forms.CheckBox
    Friend WithEvents chkRegion As System.Windows.Forms.CheckBox
    Friend WithEvents chkArea As System.Windows.Forms.CheckBox
    Friend WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents txtRegion As System.Windows.Forms.TextBox
    Friend WithEvents gbox As System.Windows.Forms.GroupBox
    Friend WithEvents txtClass_01 As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rdPortrait As System.Windows.Forms.RadioButton
    Friend WithEvents rdLandscape As System.Windows.Forms.RadioButton
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtReportTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtPArea As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents OpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents txtPBCode As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkBcode As System.Windows.Forms.CheckBox
    Friend WithEvents chkTeritory As System.Windows.Forms.CheckBox
    Friend WithEvents txtPteritory As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents rbLegal As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents rdExcel As System.Windows.Forms.RadioButton
    Friend WithEvents rdPdf As System.Windows.Forms.RadioButton
    Friend WithEvents rbLetter As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddReport))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtReportName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbClassification = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.txtDb = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.gbox = New System.Windows.Forms.GroupBox
        Me.txtPteritory = New System.Windows.Forms.TextBox
        Me.chkTeritory = New System.Windows.Forms.CheckBox
        Me.txtPBCode = New System.Windows.Forms.TextBox
        Me.chkBcode = New System.Windows.Forms.CheckBox
        Me.txtPArea = New System.Windows.Forms.TextBox
        Me.txtRegion = New System.Windows.Forms.TextBox
        Me.txtClass_01 = New System.Windows.Forms.TextBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.chkArea = New System.Windows.Forms.CheckBox
        Me.chkRegion = New System.Windows.Forms.CheckBox
        Me.chkclass_01 = New System.Windows.Forms.CheckBox
        Me.chkDate = New System.Windows.Forms.CheckBox
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rdLandscape = New System.Windows.Forms.RadioButton
        Me.rdPortrait = New System.Windows.Forms.RadioButton
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtReportTitle = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog
        Me.FolderBrowser = New System.Windows.Forms.FolderBrowserDialog
        Me.Label6 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.rbLegal = New System.Windows.Forms.RadioButton
        Me.rbLetter = New System.Windows.Forms.RadioButton
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.rdPdf = New System.Windows.Forms.RadioButton
        Me.rdExcel = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.gbox.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(36, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Report Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtReportName
        '
        Me.txtReportName.Location = New System.Drawing.Point(117, 32)
        Me.txtReportName.Name = "txtReportName"
        Me.txtReportName.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtReportName.Size = New System.Drawing.Size(288, 20)
        Me.txtReportName.TabIndex = 2
        Me.txtReportName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(445, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(154, 24)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "(must be Cystal Report file name)"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbClassification
        '
        Me.cmbClassification.Location = New System.Drawing.Point(117, 62)
        Me.cmbClassification.Name = "cmbClassification"
        Me.cmbClassification.Size = New System.Drawing.Size(153, 21)
        Me.cmbClassification.TabIndex = 5
        Me.cmbClassification.Text = "------"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(7, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Report Classification"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.txtPassword)
        Me.GroupBox1.Controls.Add(Me.txtUsername)
        Me.GroupBox1.Controls.Add(Me.txtDb)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtServer)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(18, 102)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(264, 162)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Server"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(89, 113)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(155, 20)
        Me.txtPassword.TabIndex = 16
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(89, 83)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(155, 20)
        Me.txtUsername.TabIndex = 15
        '
        'txtDb
        '
        Me.txtDb.Location = New System.Drawing.Point(90, 52)
        Me.txtDb.Name = "txtDb"
        Me.txtDb.Size = New System.Drawing.Size(155, 20)
        Me.txtDb.TabIndex = 14
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(21, 112)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(59, 24)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Password"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(10, 78)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(69, 24)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "User Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(20, 46)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 24)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Database"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(21, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 24)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Server"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(90, 22)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(155, 20)
        Me.txtServer.TabIndex = 13
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(677, 17)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(71, 43)
        Me.cmdClose.TabIndex = 14
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
        Me.cmdSave.Location = New System.Drawing.Point(602, 18)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(70, 43)
        Me.cmdSave.TabIndex = 13
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'gbox
        '
        Me.gbox.BackColor = System.Drawing.Color.Transparent
        Me.gbox.Controls.Add(Me.txtPteritory)
        Me.gbox.Controls.Add(Me.chkTeritory)
        Me.gbox.Controls.Add(Me.txtPBCode)
        Me.gbox.Controls.Add(Me.chkBcode)
        Me.gbox.Controls.Add(Me.txtPArea)
        Me.gbox.Controls.Add(Me.txtRegion)
        Me.gbox.Controls.Add(Me.txtClass_01)
        Me.gbox.Controls.Add(Me.txtDate)
        Me.gbox.Controls.Add(Me.chkArea)
        Me.gbox.Controls.Add(Me.chkRegion)
        Me.gbox.Controls.Add(Me.chkclass_01)
        Me.gbox.Controls.Add(Me.chkDate)
        Me.gbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbox.Location = New System.Drawing.Point(284, 102)
        Me.gbox.Name = "gbox"
        Me.gbox.Size = New System.Drawing.Size(328, 188)
        Me.gbox.TabIndex = 15
        Me.gbox.TabStop = False
        Me.gbox.Text = "PARAMETERS(name of the parameters)"
        '
        'txtPteritory
        '
        Me.txtPteritory.Location = New System.Drawing.Point(113, 70)
        Me.txtPteritory.Name = "txtPteritory"
        Me.txtPteritory.Size = New System.Drawing.Size(180, 20)
        Me.txtPteritory.TabIndex = 11
        Me.txtPteritory.Visible = False
        '
        'chkTeritory
        '
        Me.chkTeritory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTeritory.Location = New System.Drawing.Point(15, 68)
        Me.chkTeritory.Name = "chkTeritory"
        Me.chkTeritory.Size = New System.Drawing.Size(85, 24)
        Me.chkTeritory.TabIndex = 10
        Me.chkTeritory.Text = "Territory"
        '
        'txtPBCode
        '
        Me.txtPBCode.Location = New System.Drawing.Point(112, 153)
        Me.txtPBCode.Name = "txtPBCode"
        Me.txtPBCode.Size = New System.Drawing.Size(180, 20)
        Me.txtPBCode.TabIndex = 9
        Me.txtPBCode.Visible = False
        '
        'chkBcode
        '
        Me.chkBcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBcode.Location = New System.Drawing.Point(15, 152)
        Me.chkBcode.Name = "chkBcode"
        Me.chkBcode.Size = New System.Drawing.Size(91, 25)
        Me.chkBcode.TabIndex = 8
        Me.chkBcode.Text = "Branch Code"
        '
        'txtPArea
        '
        Me.txtPArea.Location = New System.Drawing.Point(112, 126)
        Me.txtPArea.Name = "txtPArea"
        Me.txtPArea.Size = New System.Drawing.Size(180, 20)
        Me.txtPArea.TabIndex = 7
        Me.txtPArea.Visible = False
        '
        'txtRegion
        '
        Me.txtRegion.Location = New System.Drawing.Point(112, 98)
        Me.txtRegion.Name = "txtRegion"
        Me.txtRegion.Size = New System.Drawing.Size(180, 20)
        Me.txtRegion.TabIndex = 6
        Me.txtRegion.Visible = False
        '
        'txtClass_01
        '
        Me.txtClass_01.Location = New System.Drawing.Point(113, 42)
        Me.txtClass_01.Name = "txtClass_01"
        Me.txtClass_01.Size = New System.Drawing.Size(181, 20)
        Me.txtClass_01.TabIndex = 5
        Me.txtClass_01.Visible = False
        '
        'txtDate
        '
        Me.txtDate.Location = New System.Drawing.Point(113, 17)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.Size = New System.Drawing.Size(182, 20)
        Me.txtDate.TabIndex = 4
        Me.txtDate.Visible = False
        '
        'chkArea
        '
        Me.chkArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkArea.Location = New System.Drawing.Point(15, 122)
        Me.chkArea.Name = "chkArea"
        Me.chkArea.Size = New System.Drawing.Size(58, 25)
        Me.chkArea.TabIndex = 3
        Me.chkArea.Text = "Area"
        '
        'chkRegion
        '
        Me.chkRegion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRegion.Location = New System.Drawing.Point(15, 94)
        Me.chkRegion.Name = "chkRegion"
        Me.chkRegion.Size = New System.Drawing.Size(65, 24)
        Me.chkRegion.TabIndex = 2
        Me.chkRegion.Text = "Region"
        '
        'chkclass_01
        '
        Me.chkclass_01.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkclass_01.Location = New System.Drawing.Point(15, 39)
        Me.chkclass_01.Name = "chkclass_01"
        Me.chkclass_01.Size = New System.Drawing.Size(92, 24)
        Me.chkclass_01.TabIndex = 1
        Me.chkclass_01.Text = "Head Office"
        '
        'chkDate
        '
        Me.chkDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(15, 16)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(66, 24)
        Me.chkDate.TabIndex = 0
        Me.chkDate.Text = "Date"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.rdLandscape)
        Me.GroupBox2.Controls.Add(Me.rdPortrait)
        Me.GroupBox2.Location = New System.Drawing.Point(618, 102)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(120, 88)
        Me.GroupBox2.TabIndex = 16
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "LAY-OUT"
        '
        'rdLandscape
        '
        Me.rdLandscape.Location = New System.Drawing.Point(24, 53)
        Me.rdLandscape.Name = "rdLandscape"
        Me.rdLandscape.Size = New System.Drawing.Size(81, 24)
        Me.rdLandscape.TabIndex = 1
        Me.rdLandscape.Text = "Landscape"
        '
        'rdPortrait
        '
        Me.rdPortrait.Checked = True
        Me.rdPortrait.Location = New System.Drawing.Point(22, 23)
        Me.rdPortrait.Name = "rdPortrait"
        Me.rdPortrait.Size = New System.Drawing.Size(73, 24)
        Me.rdPortrait.TabIndex = 0
        Me.rdPortrait.TabStop = True
        Me.rdPortrait.Text = "Portrait"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(33, 3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(79, 24)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "Report Title"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtReportTitle
        '
        Me.txtReportTitle.Location = New System.Drawing.Point(117, 5)
        Me.txtReportTitle.Name = "txtReportTitle"
        Me.txtReportTitle.Size = New System.Drawing.Size(312, 20)
        Me.txtReportTitle.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(406, 32)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(28, 20)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "..."
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(273, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(158, 24)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = " (just type if not in the list)"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox3.Controls.Add(Me.rbLegal)
        Me.GroupBox3.Controls.Add(Me.rbLetter)
        Me.GroupBox3.Location = New System.Drawing.Point(619, 194)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(120, 93)
        Me.GroupBox3.TabIndex = 24
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Size"
        '
        'rbLegal
        '
        Me.rbLegal.Location = New System.Drawing.Point(24, 48)
        Me.rbLegal.Name = "rbLegal"
        Me.rbLegal.Size = New System.Drawing.Size(81, 24)
        Me.rbLegal.TabIndex = 1
        Me.rbLegal.Text = "Legal"
        '
        'rbLetter
        '
        Me.rbLetter.Checked = True
        Me.rbLetter.Location = New System.Drawing.Point(24, 16)
        Me.rbLetter.Name = "rbLetter"
        Me.rbLetter.Size = New System.Drawing.Size(73, 24)
        Me.rbLetter.TabIndex = 0
        Me.rbLetter.TabStop = True
        Me.rbLetter.Text = "Letter"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.rdExcel)
        Me.GroupBox4.Controls.Add(Me.rdPdf)
        Me.GroupBox4.Location = New System.Drawing.Point(18, 282)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(264, 46)
        Me.GroupBox4.TabIndex = 25
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Exported As"
        '
        'rdPdf
        '
        Me.rdPdf.AutoSize = True
        Me.rdPdf.Checked = True
        Me.rdPdf.Location = New System.Drawing.Point(53, 19)
        Me.rdPdf.Name = "rdPdf"
        Me.rdPdf.Size = New System.Drawing.Size(41, 17)
        Me.rdPdf.TabIndex = 0
        Me.rdPdf.TabStop = True
        Me.rdPdf.Text = "Pdf"
        Me.rdPdf.UseVisualStyleBackColor = True
        '
        'rdExcel
        '
        Me.rdExcel.AutoSize = True
        Me.rdExcel.Location = New System.Drawing.Point(137, 19)
        Me.rdExcel.Name = "rdExcel"
        Me.rdExcel.Size = New System.Drawing.Size(51, 17)
        Me.rdExcel.TabIndex = 1
        Me.rdExcel.Text = "Excel"
        Me.rdExcel.UseVisualStyleBackColor = True
        '
        'AddReport
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(788, 340)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtReportTitle)
        Me.Controls.Add(Me.txtReportName)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.gbox)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmbClassification)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AddReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Report"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbox.ResumeLayout(False)
        Me.gbox.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim sql As String
        Dim strDate, strClass_01, strClass_02, strClass_03, strClass_04, strBcode, strsize, exportAs As String
        Dim layout As Integer

        If Me.chkDate.Checked = True Then
            strDate = Me.txtDate.Text
        End If
        If Me.chkclass_01.Checked = True Then
            strClass_01 = Me.txtClass_01.Text
        End If
        If Me.chkRegion.Checked = True Then
            strClass_03 = Me.txtRegion.Text
        End If
        If Me.chkArea.Checked = True Then
            strClass_04 = Me.txtPArea.Text
        End If
        If Me.chkBcode.Checked = True Then
            strBcode = Me.txtPBCode.Text
        End If
        If Me.chkTeritory.Checked = True Then
            strClass_02 = Me.txtPteritory.Text
        End If

        If Me.rdPortrait.Checked = True Then
            layout = 1
        Else
            layout = 2
        End If
        If Me.rbLetter.Checked Then
            strsize = 1
        Else
            strsize = 2
        End If
        If rdPdf.Checked Then
            exportAs = "pdf"
        Else
            exportAs = "excel"
        End If

        Try

            If (Me.txtReportTitle.Text = "") Or (Me.cmbClassification.Text = "") Or (Me.txtDb.Text = "") Or (Me.txtPassword.Text = "") Or (Me.txtReportName.Text = "") Or (Me.txtServer.Text = "") Or (Me.txtUsername.Text = "") Then
                MsgBox("Please fill up all fields.", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
            Else
                Dim maxID, rows As Integer
                Dim ds_ID As DataSet
                Try
                    Dim sqlID As String = "SELECT max(ID) FROM tbl_Reports_V3"
                    ds_ID = objDBLocal.Execute_SQL_DataSet(sqlID, "ID")
                    If Not ds_ID Is Nothing Then
                        Dim sqlRow As String = "SELECT COUNT(ID) FROM tbl_Reports_V3"
                        Dim ds_Rows As DataSet = objDBLocal.Execute_SQL_DataSet(sqlRow, "ID")
                        rows = ds_Rows.Tables(0).Rows(0).ItemArray(0).ToString()
                        maxID = ds_ID.Tables(0).Rows(0).ItemArray(0).ToString()
                        If rows <> maxID Then
                            Dim ctr As Integer
                            Dim sqlReport As String = " Select ID FROM tbl_Reports_V3"
                            Dim ds_Report As DataSet = objDBLocal.Execute_SQL_DataSet(sqlReport, "ID")
                            Dim id As Integer
                            For ctr = 1 To rows
                                id = ds_Report.Tables(0).Rows(ctr).ItemArray(0).ToString
                                Dim sqlUpdate As String = "UPDATE tbl_Reports_V3 SET ID='" & ctr & "' WHERE ID= '" & id & "' "
                                If objDBLocal.Execute_SQLQuery(sqlUpdate) < 1 Then
                                    MsgBox("Can not Update Report's ID: " + ctr, MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                                End If
                            Next
                        End If
                        maxID = maxID + 1
                    Else
                        maxID = 1

                    End If
                Catch ex As Exception
                    maxID = 1

                End Try

                sql = "INSERT INTO tbl_Reports_V3 (ID,ReportName,ReportTitle,Layout,sizes,Classification,Server,Db,UName,Pass,ParamDate,ParamClass_01,Paramclass_02,Paramclass_03,ParamClass_04,ParamBranchCode,ExportAs) VALUES('" & maxID & "','" & Me.txtReportName.Text & "','" & Me.txtReportTitle.Text & "','" & layout & "','" & strsize & "','" & Me.cmbClassification.Text & "','" & Me.txtServer.Text & "','" & Me.txtDb.Text & "','" & Me.txtUsername.Text & "','" & Me.txtPassword.Text & "','" & strDate & "','" & strClass_01 & "','" & strClass_02 & "','" & strClass_03 & "','" & strClass_04 & "','" & strBcode & "','" & exportAs & "')"

                If objDBLocal.Execute_SQLQuery(sql) < 1 Then
                    MsgBox("Error in inserting data.", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                    Exit Sub
                Else
                    MsgBox("Report is succesfully inserted..", MsgBoxStyle.Information, "Email Scheduler  V3.0")
                End If

                Call ClearEntry()
                objDBLocal.CloseConnection()

            End If
        Catch ex As Exception
            MsgBox("Please fill up all fields.", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
        End Try

    End Sub
    Private Sub ClearEntry()
        Me.txtReportTitle.Text = ""
        Me.cmbClassification.Text = ""
        Me.txtDb.Text = ""
        Me.txtPassword.Text = ""
        Me.txtReportName.Text = ""
        Me.txtServer.Text = ""
        Me.txtUsername.Text = ""
        If Me.txtPArea.Text <> "" Then
            Me.chkArea.Checked = False
            Me.txtPArea.Text = ""
        End If
        If Me.txtDate.Text <> "" Then
            Me.chkDate.Checked = False
            Me.txtDate.Text = ""
        End If
        If Me.txtClass_01.Text <> "" Then
            Me.chkclass_01.Checked = False
            Me.txtClass_01.Text = ""
        End If
        If Me.txtPteritory.Text <> "" Then
            Me.chkTeritory.Checked = False
            Me.txtPteritory.Text = ""
        End If
        If Me.txtRegion.Text <> "" Then
            Me.chkRegion.Checked = False
            Me.txtRegion.Text = ""
        End If
        If Me.txtPBCode.Text <> "" Then
            Me.chkBcode.Checked = False
            Me.txtPBCode.Text = ""
        End If

    End Sub
    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        If Me.chkDate.Checked = True Then
            Me.txtDate.Visible = True
            Me.txtDate.Focus()
            Me.txtDate.Select()
        Else
            Me.txtDate.Text = ""
            Me.txtDate.Visible = False
        End If
    End Sub

    Private Sub chkclass_01_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkclass_01.CheckedChanged
        If Me.chkclass_01.Checked = True Then
            Me.txtClass_01.Visible = True
            Me.txtClass_01.Focus()
            Me.txtClass_01.Select()
        Else
            Me.txtClass_01.Text = ""
            Me.txtClass_01.Visible = False
        End If
    End Sub

    Private Sub chkRegion_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRegion.CheckedChanged
        If Me.chkRegion.Checked = True Then
            Me.txtRegion.Visible = True
            Me.txtRegion.Focus()
            Me.txtRegion.Select()
        Else
            Me.txtRegion.Visible = False
            Me.txtRegion.Text = ""
        End If
    End Sub

    Private Sub chkArea_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkArea.CheckedChanged
        If Me.chkArea.Checked = True Then
            Me.txtPArea.Visible = True
            Me.txtPArea.Visible = True
            Me.txtPArea.Focus()
            Me.txtPArea.Select()
        Else
            Me.txtPArea.Visible = False
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.OpenFile.InitialDirectory = Application.StartupPath ' + "\Reports\"

        Me.OpenFile.Filter = "Report Files (*.rpt)|*.rpt"
        Me.OpenFile.ShowDialog()
        Dim filename As String = System.IO.Path.GetFileName(OpenFile.FileName)
        Me.txtReportName.Text = filename
        Me.txtReportName.Focus()
        ' Me.txtReportName.Select(Me.txtReportName.TextLength, 0)
        Me.OpenFile.Dispose()
    End Sub
    Private Sub GetClassification()
        Dim sql As String
        Dim ds As DataSet
        Dim db As New clsDBConnection
        If db.isConnected Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        sql = "Select distinct Classification from tbl_Reports_V3"
        ds = objDBLocal.Execute_SQL_DataSet(sql, "Classification")
        If Not ds Is Nothing Then
            Dim ctr As Integer = 0
            For ctr = 0 To ds.Tables(0).Rows.Count - 1
                Dim str As String = ds.Tables(0).Rows(ctr).Item(0)
                Me.cmbClassification.Items.Add(str.Trim)
                Me.cmbClassification.Text = str.Trim
            Next
        End If
        db.CloseConnection()
    End Sub
    'Private Sub GetReportFor()
    '    Dim sql As String
    '    Dim ds As DataSet
    '    Dim db As New clsDBConnection

    '    If db.isConnected Then
    '        db.CloseConnection()
    '    End If
    '    db.ConnectDB(ls_connectionStringLocal)

    '    sql = "Select distinct Specific from tbl_Reports_V3"
    '    ds = objDBLocal.Execute_SQL_DataSet(sql, "specific")
    '    If Not ds Is Nothing Then
    '        Dim ctr As Integer = 0
    '        For ctr = 0 To ds.Tables(0).Rows.Count - 1
    '            Dim str As String = ds.Tables(0).Rows(ctr).Item(0)
    '            Me.cmbReportFor.Items.Add(str.Trim)
    '            Me.cmbReportFor.Text = str.Trim
    '        Next
    '        ds.Dispose()
    '    End If
    '    db.CloseConnection()

    'End Sub

    Private Sub AddReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call GetClassification()
        ' Call GetReportFor()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBcode.CheckedChanged
        If Me.chkBcode.Checked = True Then
            Me.txtPBCode.Visible = True
            Me.txtPBCode.Text = ""
            Me.txtPBCode.Focus()
            Me.txtPBCode.Select()
        Else
            Me.txtPBCode.Visible = False
        End If
    End Sub
    Private Sub chkTeritory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTeritory.CheckedChanged
        If chkTeritory.Checked = True Then
            Me.txtPteritory.Visible = True
            Me.txtPteritory.Text = ""
            Me.txtPteritory.Focus()
            Me.txtPteritory.Select()
        Else
            Me.txtPteritory.Visible = False
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub
End Class
