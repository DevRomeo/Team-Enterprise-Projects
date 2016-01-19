Public Class setup
    Inherits System.Windows.Forms.Form
    Private ctrMin As Integer


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
    Friend WithEvents btnAddReport As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbHour As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMin As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbSession As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtMailServer As System.Windows.Forms.TextBox
    Friend WithEvents txtMailAdmin As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtMailUName As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtMailPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtUname As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtDb As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents dbdate As System.Windows.Forms.GroupBox
    Friend WithEvents dbMail As System.Windows.Forms.GroupBox
    Friend WithEvents dbDB As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(setup))
        Me.btnAddReport = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.dbdate = New System.Windows.Forms.GroupBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbSession = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbMin = New System.Windows.Forms.ComboBox
        Me.cmbHour = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.dbMail = New System.Windows.Forms.GroupBox
        Me.txtMailPassword = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtMailUName = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtMailAdmin = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtMailServer = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.dbDB = New System.Windows.Forms.GroupBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtUname = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtDb = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button7 = New System.Windows.Forms.Button
        Me.dbdate.SuspendLayout()
        Me.dbMail.SuspendLayout()
        Me.dbDB.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAddReport
        '
        Me.btnAddReport.BackColor = System.Drawing.Color.Gainsboro
        Me.btnAddReport.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnAddReport.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddReport.Image = CType(resources.GetObject("btnAddReport.Image"), System.Drawing.Image)
        Me.btnAddReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddReport.Location = New System.Drawing.Point(166, 4)
        Me.btnAddReport.Name = "btnAddReport"
        Me.btnAddReport.Size = New System.Drawing.Size(139, 23)
        Me.btnAddReport.TabIndex = 10
        Me.btnAddReport.Text = "       ADD REPORT"
        Me.btnAddReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.Gainsboro
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnDelete.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(166, 31)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(139, 23)
        Me.btnDelete.TabIndex = 12
        Me.btnDelete.Text = "       DELETE REPORT"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dbdate
        '
        Me.dbdate.BackColor = System.Drawing.Color.Transparent
        Me.dbdate.Controls.Add(Me.DateTimePicker1)
        Me.dbdate.Controls.Add(Me.Label4)
        Me.dbdate.Controls.Add(Me.cmbSession)
        Me.dbdate.Controls.Add(Me.Label3)
        Me.dbdate.Controls.Add(Me.cmbMin)
        Me.dbdate.Controls.Add(Me.cmbHour)
        Me.dbdate.Controls.Add(Me.Label2)
        Me.dbdate.Enabled = False
        Me.dbdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dbdate.Location = New System.Drawing.Point(277, 69)
        Me.dbdate.Name = "dbdate"
        Me.dbdate.Size = New System.Drawing.Size(294, 114)
        Me.dbdate.TabIndex = 13
        Me.dbdate.TabStop = False
        Me.dbdate.Text = "DATE and TIME Set-up"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "MM/dd/yyyy"
        Me.DateTimePicker1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(62, 82)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(128, 20)
        Me.DateTimePicker1.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 29)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Date:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbSession
        '
        Me.cmbSession.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSession.Items.AddRange(New Object() {"AM", "PM"})
        Me.cmbSession.Location = New System.Drawing.Point(221, 36)
        Me.cmbSession.Name = "cmbSession"
        Me.cmbSession.Size = New System.Drawing.Size(68, 21)
        Me.cmbSession.TabIndex = 4
        Me.cmbSession.Text = "-session-"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(130, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(10, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = ":"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbMin
        '
        Me.cmbMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMin.Location = New System.Drawing.Point(144, 36)
        Me.cmbMin.Name = "cmbMin"
        Me.cmbMin.Size = New System.Drawing.Size(68, 21)
        Me.cmbMin.TabIndex = 2
        Me.cmbMin.Text = "-minutes-"
        '
        'cmbHour
        '
        Me.cmbHour.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbHour.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.cmbHour.Location = New System.Drawing.Point(63, 36)
        Me.cmbHour.Name = "cmbHour"
        Me.cmbHour.Size = New System.Drawing.Size(64, 21)
        Me.cmbHour.TabIndex = 1
        Me.cmbHour.Text = "-hour-"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 23)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Time:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(477, 304)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 40)
        Me.cmdClose.TabIndex = 16
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdSave.Enabled = False
        Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(324, 305)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(72, 40)
        Me.cmdSave.TabIndex = 15
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dbMail
        '
        Me.dbMail.BackColor = System.Drawing.Color.Transparent
        Me.dbMail.Controls.Add(Me.txtMailPassword)
        Me.dbMail.Controls.Add(Me.Label8)
        Me.dbMail.Controls.Add(Me.txtMailUName)
        Me.dbMail.Controls.Add(Me.Label6)
        Me.dbMail.Controls.Add(Me.txtMailAdmin)
        Me.dbMail.Controls.Add(Me.Label5)
        Me.dbMail.Controls.Add(Me.txtMailServer)
        Me.dbMail.Controls.Add(Me.Label7)
        Me.dbMail.Enabled = False
        Me.dbMail.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dbMail.Location = New System.Drawing.Point(9, 70)
        Me.dbMail.Name = "dbMail"
        Me.dbMail.Size = New System.Drawing.Size(262, 135)
        Me.dbMail.TabIndex = 17
        Me.dbMail.TabStop = False
        Me.dbMail.Text = "Mail Server Set-up"
        '
        'txtMailPassword
        '
        Me.txtMailPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailPassword.Location = New System.Drawing.Point(96, 96)
        Me.txtMailPassword.Name = "txtMailPassword"
        Me.txtMailPassword.Size = New System.Drawing.Size(160, 20)
        Me.txtMailPassword.TabIndex = 27
        Me.txtMailPassword.Text = ""
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 96)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 16)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Password:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMailUName
        '
        Me.txtMailUName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailUName.Location = New System.Drawing.Point(96, 72)
        Me.txtMailUName.Name = "txtMailUName"
        Me.txtMailUName.Size = New System.Drawing.Size(160, 20)
        Me.txtMailUName.TabIndex = 25
        Me.txtMailUName.Text = ""
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 72)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 16)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "User Name:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMailAdmin
        '
        Me.txtMailAdmin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailAdmin.Location = New System.Drawing.Point(96, 48)
        Me.txtMailAdmin.Name = "txtMailAdmin"
        Me.txtMailAdmin.Size = New System.Drawing.Size(160, 20)
        Me.txtMailAdmin.TabIndex = 23
        Me.txtMailAdmin.Text = ""
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 48)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 16)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Mail Admin:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMailServer
        '
        Me.txtMailServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailServer.Location = New System.Drawing.Point(96, 24)
        Me.txtMailServer.Name = "txtMailServer"
        Me.txtMailServer.Size = New System.Drawing.Size(160, 20)
        Me.txtMailServer.TabIndex = 21
        Me.txtMailServer.Text = ""
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(24, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Server:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dbDB
        '
        Me.dbDB.BackColor = System.Drawing.Color.Transparent
        Me.dbDB.Controls.Add(Me.txtPassword)
        Me.dbDB.Controls.Add(Me.Label9)
        Me.dbDB.Controls.Add(Me.txtUname)
        Me.dbDB.Controls.Add(Me.Label10)
        Me.dbDB.Controls.Add(Me.txtDb)
        Me.dbDB.Controls.Add(Me.Label11)
        Me.dbDB.Controls.Add(Me.txtServer)
        Me.dbDB.Controls.Add(Me.Label12)
        Me.dbDB.Enabled = False
        Me.dbDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dbDB.Location = New System.Drawing.Point(13, 212)
        Me.dbDB.Name = "dbDB"
        Me.dbDB.Size = New System.Drawing.Size(258, 134)
        Me.dbDB.TabIndex = 28
        Me.dbDB.TabStop = False
        Me.dbDB.Text = "Email Schedueler Server Set-up"
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(96, 96)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(120, 20)
        Me.txtPassword.TabIndex = 27
        Me.txtPassword.Text = ""
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 96)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 16)
        Me.Label9.TabIndex = 26
        Me.Label9.Text = "Password:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtUname
        '
        Me.txtUname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUname.Location = New System.Drawing.Point(96, 72)
        Me.txtUname.Name = "txtUname"
        Me.txtUname.Size = New System.Drawing.Size(120, 20)
        Me.txtUname.TabIndex = 25
        Me.txtUname.Text = ""
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(8, 72)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(80, 16)
        Me.Label10.TabIndex = 24
        Me.Label10.Text = "User Name:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDb
        '
        Me.txtDb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDb.Location = New System.Drawing.Point(96, 48)
        Me.txtDb.Name = "txtDb"
        Me.txtDb.Size = New System.Drawing.Size(120, 20)
        Me.txtDb.TabIndex = 23
        Me.txtDb.Text = ""
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(16, 48)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 16)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "DB Name:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtServer
        '
        Me.txtServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServer.Location = New System.Drawing.Point(96, 24)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(120, 20)
        Me.txtServer.TabIndex = 21
        Me.txtServer.Text = ""
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(24, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 16)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Server:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Gainsboro
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(310, 5)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(144, 23)
        Me.Button2.TabIndex = 30
        Me.Button2.Text = "       ADD RECIPIENT"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.Gainsboro
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(309, 32)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(145, 23)
        Me.Button3.TabIndex = 30
        Me.Button3.Text = "       DELETE  RECIPIENT"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Gainsboro
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(458, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(129, 23)
        Me.Button1.TabIndex = 31
        Me.Button1.Text = "       ADD USER"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.Gainsboro
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button4.Location = New System.Drawing.Point(2, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(159, 23)
        Me.Button4.TabIndex = 32
        Me.Button4.Text = "      RESEND REPORT"
        Me.Button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.Gainsboro
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button5.Location = New System.Drawing.Point(458, 32)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(128, 23)
        Me.Button5.TabIndex = 33
        Me.Button5.Text = "       DELETE USER"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.Gainsboro
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.Image = CType(resources.GetObject("Button6.Image"), System.Drawing.Image)
        Me.Button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button6.Location = New System.Drawing.Point(2, 30)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(159, 23)
        Me.Button6.TabIndex = 34
        Me.Button6.Text = "      REGENERATE REPORT"
        Me.Button6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.Gainsboro
        Me.Button7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.Image = CType(resources.GetObject("Button7.Image"), System.Drawing.Image)
        Me.Button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button7.Location = New System.Drawing.Point(400, 305)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(72, 40)
        Me.Button7.TabIndex = 35
        Me.Button7.Text = "    &Edit"
        '
        'setup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(591, 381)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.dbMail)
        Me.Controls.Add(Me.dbdate)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAddReport)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.dbDB)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "setup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set-up"
        Me.dbdate.ResumeLayout(False)
        Me.dbMail.ResumeLayout(False)
        Me.dbDB.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnAddReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddReport.Click
        Dim frmlog As New AddReport
        frmlog.Show()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Call SaveSettings()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim frmdelete As New delete
        frmdelete.Show()
    End Sub
    Private Sub SaveSettings()

        Dim ls_receipients As String
        Dim ls_time As String

        Dim li_count As Long

        Dim li_ret As Integer

        Dim ls_headoffice As String
        Dim ls_region As String
        Dim ls_area As String
        Try
            Dim strDate = Me.DateTimePicker1.Value
            Dim strHour As String = Me.cmbHour.Text ' Me.cmbHour.Items.Item(Me.cmbHour.SelectedIndex)+
            Dim strMin As String = Me.cmbMin.Text ' Me.cmbMin.Items.Item(Me.cmbMin.SelectedIndex)
            Dim strSession As String = Me.cmbSession.Text ' Me.cmbSession.Items.Item(Me.cmbSession.SelectedIndex)


            ls_time = strHour + ":" + strMin + " " + strSession


            If ls_time <> "" Then
                If Not isValidTime(ls_time) Then
                    MsgBox("Incorrect Time Specified. Format(hh:mm AM/PM)", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                    Exit Sub
                ElseIf CDate(strDate) < Now.ToShortDateString Then
                    MsgBox("Please check your date settings..", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                ElseIf Me.txtDb.Text = "" Or Me.txtMailAdmin.Text = "" Or Me.txtMailPassword.Text = "" Or Me.txtMailServer.Text = "" Or Me.txtMailUName.Text = "" Or Me.txtPassword.Text = "" Or Me.txtServer.Text = "" Or Me.txtUname.Text = "" Then
                    MsgBox("Please check your Servers settings..", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                Else
                    'DATE AND TIME
                    li_ret = WritePrivateProfileString("EMAIL SCHEDULER", "DATE", strDate, ps_ini)
                    li_ret = WritePrivateProfileString("EMAIL SCHEDULER", "TIME", ls_time, ps_ini)
                    li_ret = WritePrivateProfileString("EMAIL SCHEDULER", "SERVER", Me.txtServer.Text, ps_ini)
                    li_ret = WritePrivateProfileString("EMAIL SCHEDULER", "USERNAME", Me.txtUname.Text, ps_ini)
                    li_ret = WritePrivateProfileString("EMAIL SCHEDULER", "PASSWORD", Me.txtPassword.Text, ps_ini)
                   
                    'MAIL SERVER
                    li_ret = WritePrivateProfileString("MAIL SERVER", "MAILSERVER", Me.txtMailServer.Text, ps_ini)
                    li_ret = WritePrivateProfileString("MAIL SERVER", "MAILADMIN", Me.txtMailAdmin.Text, ps_ini)
                    li_ret = WritePrivateProfileString("MAIL SERVER", "MAILUSERNAME", Me.txtMailUName.Text, ps_ini)
                    li_ret = WritePrivateProfileString("MAIL SERVER", "MAILPASSWORD", Me.txtMailPassword.Text, ps_ini)

                    MsgBox("Successfully saving of settings", MsgBoxStyle.Information, "Email Scheduler  V3.0")
                    ps_date = strDate
                    ps_time = ls_time
                    Me.Close()
                End If
            Else
                MsgBox("Please input scheduled time for email", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Please input time and date correctly...", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
        End Try

    End Sub
    Private Function isValidTime(ByVal as_time) As Boolean
        Try
            If IsDate(as_time) Then
                isValidTime = True
            Else
                isValidTime = False
            End If
        Catch ex As Exception
            isValidTime = False
        End Try

    End Function

    Private Sub setup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Me.Button3.Image = Image.FromFile(Application.StartupPath + "\IMAGE\deleteUser.jpg")
        Dim ctr As Integer
        For ctr = 0 To 59
            If ctr < 10 Then
                Me.cmbMin.Items.Add("0" + CStr(ctr))
            Else
                Me.cmbMin.Items.Add(ctr)
            End If
        Next
        Me.DateTimePicker1.Value = CDate(ps_date)
        Dim strTime As String = ps_time
        Dim time() As String
        Dim time2() As String
        time = Split(strTime, ":")
        Me.cmbHour.Text = time(0)
        time2 = Split(time(1), " ")
        Me.cmbMin.Text = time2(0)
        Me.cmbSession.Text = time2(1)
        Me.txtDb.Text = ps_DBNameLocal
        Me.txtPassword.Text = ps_DBPasswordLocal
        Me.txtServer.Text = ps_DBServerLocal
        Me.txtUname.Text = ps_DBUsernameLocal
        Me.txtMailAdmin.Text = ps_MailAdmin
        Me.txtMailPassword.Text = ps_MailPassword
        Me.txtMailServer.Text = ps_MailServer
        Me.txtMailUName.Text = ps_MailUsername
        '"user id=" & ps_DBUsername & ";password=" & ps_DBPassword & ";data source=" & ps_DBServer & ";persist security info=False;initial catalog=" & ps_DBName
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frmupdate As New updateReport
        frmupdate.ShowDialog()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim frm As New AddRecipients
        frm.ShowDialog()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim frm As New DeleteUpdate
        frm.ShowDialog()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim frm As New addUser
        frm.ShowDialog()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'Dim frm As New AddRecipients
        'isResend = True
        'frm.ShowDialog()
        'isResend = False
        Dim frm As New resendReports
        frm.ShowDialog()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim frm As New deleteUser
        frm.Show()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim frm As New Form1
        frm.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        dbMail.Enabled = True
        dbDB.Enabled = True
        dbdate.Enabled = True
        Me.cmdSave.Enabled = True
        Me.Button7.Enabled = False
    End Sub

    Private Sub Button4_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.MouseEnter, Button1.MouseEnter, Button2.MouseEnter, Button3.MouseEnter, Button5.MouseEnter, Button6.MouseEnter, btnAddReport.MouseEnter, btnDelete.MouseEnter
        DirectCast(sender, Button).BackColor = Color.Aqua
        sender.focus()
    End Sub
    Private Sub Button4_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.MouseLeave, Button1.MouseLeave, Button2.MouseLeave, Button3.MouseLeave, Button5.MouseLeave, Button6.MouseLeave, btnAddReport.MouseLeave, btnDelete.MouseLeave
        DirectCast(sender, Button).BackColor = Color.Gainsboro
    End Sub

End Class
