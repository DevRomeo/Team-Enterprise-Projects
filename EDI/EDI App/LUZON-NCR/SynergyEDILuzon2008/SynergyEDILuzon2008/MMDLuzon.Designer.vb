<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MMDLuzon
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MMDLuzon))
        Me.dg = New System.Windows.Forms.DataGrid
        Me.picGif = New System.Windows.Forms.PictureBox
        Me.grp = New System.Windows.Forms.GroupBox
        Me.pb = New System.Windows.Forms.ProgressBar
        Me.gbUploadP = New System.Windows.Forms.GroupBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnSetup = New System.Windows.Forms.Button
        Me.btnEnter = New System.Windows.Forms.Button
        Me.gbExplore = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtdesc = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.cboSet = New System.Windows.Forms.ComboBox
        Me.cboSheet = New System.Windows.Forms.ComboBox
        Me.txtStart = New System.Windows.Forms.TextBox
        Me.txtLocation = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblMonth = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.grd = New AxMSFlexGridLib.AxMSFlexGrid
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog
        Me.btnclear = New System.Windows.Forms.Button
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.DesignPix = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picGif, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp.SuspendLayout()
        Me.gbUploadP.SuspendLayout()
        Me.gbExplore.SuspendLayout()
        CType(Me.grd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DesignPix, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dg
        '
        Me.dg.DataMember = ""
        Me.dg.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dg.Location = New System.Drawing.Point(12, 298)
        Me.dg.Name = "dg"
        Me.dg.Size = New System.Drawing.Size(838, 302)
        Me.dg.TabIndex = 42
        '
        'picGif
        '
        Me.picGif.Image = CType(resources.GetObject("picGif.Image"), System.Drawing.Image)
        Me.picGif.Location = New System.Drawing.Point(818, 254)
        Me.picGif.Name = "picGif"
        Me.picGif.Size = New System.Drawing.Size(32, 32)
        Me.picGif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picGif.TabIndex = 41
        Me.picGif.TabStop = False
        '
        'grp
        '
        Me.grp.Controls.Add(Me.pb)
        Me.grp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp.Location = New System.Drawing.Point(447, 238)
        Me.grp.Name = "grp"
        Me.grp.Size = New System.Drawing.Size(365, 54)
        Me.grp.TabIndex = 39
        Me.grp.TabStop = False
        '
        'pb
        '
        Me.pb.Location = New System.Drawing.Point(6, 21)
        Me.pb.Name = "pb"
        Me.pb.Size = New System.Drawing.Size(353, 23)
        Me.pb.TabIndex = 0
        '
        'gbUploadP
        '
        Me.gbUploadP.Controls.Add(Me.btnSave)
        Me.gbUploadP.Controls.Add(Me.btnSetup)
        Me.gbUploadP.Controls.Add(Me.btnEnter)
        Me.gbUploadP.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbUploadP.Location = New System.Drawing.Point(447, 141)
        Me.gbUploadP.Name = "gbUploadP"
        Me.gbUploadP.Size = New System.Drawing.Size(395, 91)
        Me.gbUploadP.TabIndex = 38
        Me.gbUploadP.TabStop = False
        Me.gbUploadP.Text = "Enter Set Up"
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(295, 21)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(94, 54)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnSetup
        '
        Me.btnSetup.Image = CType(resources.GetObject("btnSetup.Image"), System.Drawing.Image)
        Me.btnSetup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSetup.Location = New System.Drawing.Point(156, 21)
        Me.btnSetup.Name = "btnSetup"
        Me.btnSetup.Size = New System.Drawing.Size(133, 54)
        Me.btnSetup.TabIndex = 10
        Me.btnSetup.Text = "Old Setup"
        Me.btnSetup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSetup.UseVisualStyleBackColor = True
        '
        'btnEnter
        '
        Me.btnEnter.Image = CType(resources.GetObject("btnEnter.Image"), System.Drawing.Image)
        Me.btnEnter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEnter.Location = New System.Drawing.Point(6, 21)
        Me.btnEnter.Name = "btnEnter"
        Me.btnEnter.Size = New System.Drawing.Size(144, 54)
        Me.btnEnter.TabIndex = 8
        Me.btnEnter.Text = "Enter Setup"
        Me.btnEnter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEnter.UseVisualStyleBackColor = True
        '
        'gbExplore
        '
        Me.gbExplore.Controls.Add(Me.Label8)
        Me.gbExplore.Controls.Add(Me.Label7)
        Me.gbExplore.Controls.Add(Me.txtdesc)
        Me.gbExplore.Controls.Add(Me.Label6)
        Me.gbExplore.Controls.Add(Me.btnBrowse)
        Me.gbExplore.Controls.Add(Me.cboPeriod)
        Me.gbExplore.Controls.Add(Me.cboYear)
        Me.gbExplore.Controls.Add(Me.cboSet)
        Me.gbExplore.Controls.Add(Me.cboSheet)
        Me.gbExplore.Controls.Add(Me.txtStart)
        Me.gbExplore.Controls.Add(Me.txtLocation)
        Me.gbExplore.Controls.Add(Me.Label3)
        Me.gbExplore.Controls.Add(Me.Label5)
        Me.gbExplore.Controls.Add(Me.lblMonth)
        Me.gbExplore.Controls.Add(Me.Label4)
        Me.gbExplore.Controls.Add(Me.Label2)
        Me.gbExplore.Controls.Add(Me.Label1)
        Me.gbExplore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbExplore.Location = New System.Drawing.Point(12, 140)
        Me.gbExplore.Name = "gbExplore"
        Me.gbExplore.Size = New System.Drawing.Size(429, 152)
        Me.gbExplore.TabIndex = 37
        Me.gbExplore.TabStop = False
        Me.gbExplore.Text = "Data Process"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(261, 129)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(0, 14)
        Me.Label8.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(261, 100)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(139, 14)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "View GL Accounts Used"
        '
        'txtdesc
        '
        Me.txtdesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtdesc.Location = New System.Drawing.Point(113, 126)
        Me.txtdesc.MaxLength = 25
        Me.txtdesc.Name = "txtdesc"
        Me.txtdesc.Size = New System.Drawing.Size(136, 20)
        Me.txtdesc.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(24, 126)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 14)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Description:"
        '
        'btnBrowse
        '
        Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
        Me.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBrowse.Location = New System.Drawing.Point(322, 16)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(96, 23)
        Me.btnBrowse.TabIndex = 0
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'cboPeriod
        '
        Me.cboPeriod.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(215, 68)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(94, 22)
        Me.cboPeriod.TabIndex = 4
        '
        'cboYear
        '
        Me.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboYear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(113, 68)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(94, 22)
        Me.cboYear.TabIndex = 3
        '
        'cboSet
        '
        Me.cboSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSet.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSet.ForeColor = System.Drawing.Color.Red
        Me.cboSet.FormattingEnabled = True
        Me.cboSet.Items.AddRange(New Object() {"True", "False"})
        Me.cboSet.Location = New System.Drawing.Point(318, 41)
        Me.cboSet.Name = "cboSet"
        Me.cboSet.Size = New System.Drawing.Size(100, 22)
        Me.cboSet.TabIndex = 7
        '
        'cboSheet
        '
        Me.cboSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSheet.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSheet.FormattingEnabled = True
        Me.cboSheet.Location = New System.Drawing.Point(113, 41)
        Me.cboSheet.Name = "cboSheet"
        Me.cboSheet.Size = New System.Drawing.Size(94, 22)
        Me.cboSheet.TabIndex = 2
        '
        'txtStart
        '
        Me.txtStart.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStart.Location = New System.Drawing.Point(113, 97)
        Me.txtStart.MaxLength = 2
        Me.txtStart.Name = "txtStart"
        Me.txtStart.Size = New System.Drawing.Size(136, 20)
        Me.txtStart.TabIndex = 5
        '
        'txtLocation
        '
        Me.txtLocation.BackColor = System.Drawing.Color.White
        Me.txtLocation.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLocation.Location = New System.Drawing.Point(113, 16)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.ReadOnly = True
        Me.txtLocation.Size = New System.Drawing.Size(203, 20)
        Me.txtLocation.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(213, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Set True/False :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(31, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 14)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Start Row :"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonth.Location = New System.Drawing.Point(315, 75)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(46, 14)
        Me.lblMonth.TabIndex = 0
        Me.lblMonth.Text = "MONTH"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(20, 71)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Year/Period :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(18, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Sheet Name :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Excel Location :"
        '
        'grd
        '
        Me.grd.Location = New System.Drawing.Point(12, 298)
        Me.grd.Name = "grd"
        Me.grd.OcxState = CType(resources.GetObject("grd.OcxState"), System.Windows.Forms.AxHost.State)
        Me.grd.Size = New System.Drawing.Size(207, 23)
        Me.grd.TabIndex = 43
        '
        'btnclear
        '
        Me.btnclear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclear.Image = CType(resources.GetObject("btnclear.Image"), System.Drawing.Image)
        Me.btnclear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnclear.Location = New System.Drawing.Point(759, 606)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(91, 55)
        Me.btnclear.TabIndex = 44
        Me.btnclear.Text = "Close"
        Me.btnclear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnclear.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(432, 11)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(72, 122)
        Me.PictureBox2.TabIndex = 61
        Me.PictureBox2.TabStop = False
        '
        'DesignPix
        '
        Me.DesignPix.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DesignPix.Image = CType(resources.GetObject("DesignPix.Image"), System.Drawing.Image)
        Me.DesignPix.Location = New System.Drawing.Point(497, 11)
        Me.DesignPix.Name = "DesignPix"
        Me.DesignPix.Size = New System.Drawing.Size(353, 122)
        Me.DesignPix.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.DesignPix.TabIndex = 60
        Me.DesignPix.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 11)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(429, 123)
        Me.PictureBox1.TabIndex = 59
        Me.PictureBox1.TabStop = False
        '
        'MMDLuzon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(856, 673)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.DesignPix)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.grd)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.picGif)
        Me.Controls.Add(Me.grp)
        Me.Controls.Add(Me.gbUploadP)
        Me.Controls.Add(Me.gbExplore)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MMDLuzon"
        Me.Text = "MMD - LUZON"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picGif, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp.ResumeLayout(False)
        Me.gbUploadP.ResumeLayout(False)
        Me.gbExplore.ResumeLayout(False)
        Me.gbExplore.PerformLayout()
        CType(Me.grd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DesignPix, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dg As System.Windows.Forms.DataGrid
    Friend WithEvents picGif As System.Windows.Forms.PictureBox
    Friend WithEvents grp As System.Windows.Forms.GroupBox
    Friend WithEvents pb As System.Windows.Forms.ProgressBar
    Friend WithEvents gbUploadP As System.Windows.Forms.GroupBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnSetup As System.Windows.Forms.Button
    Friend WithEvents btnEnter As System.Windows.Forms.Button
    Friend WithEvents gbExplore As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtdesc As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents cboSet As System.Windows.Forms.ComboBox
    Friend WithEvents cboSheet As System.Windows.Forms.ComboBox
    Friend WithEvents txtStart As System.Windows.Forms.TextBox
    Friend WithEvents txtLocation As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grd As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents OpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnclear As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents DesignPix As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
