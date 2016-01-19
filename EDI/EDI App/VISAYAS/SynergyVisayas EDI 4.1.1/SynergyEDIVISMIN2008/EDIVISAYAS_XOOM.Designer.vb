<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EDIVISAYAS_XOOM
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EDIVISAYAS_XOOM))
        Me.btnGIF = New System.Windows.Forms.PictureBox
        Me.grp = New System.Windows.Forms.GroupBox
        Me.pb = New System.Windows.Forms.ProgressBar
        Me.gbUploadP = New System.Windows.Forms.GroupBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.btnEnter = New System.Windows.Forms.Button
        Me.gbExplore = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.button1 = New System.Windows.Forms.Button
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.cmbSt = New System.Windows.Forms.ComboBox
        Me.cboSheet = New System.Windows.Forms.ComboBox
        Me.txtStart = New System.Windows.Forms.TextBox
        Me.TEXTBOX1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblMonth = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dg = New System.Windows.Forms.DataGrid
        Me.grd = New AxMSFlexGridLib.AxMSFlexGrid
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog
        Me.btnclear = New System.Windows.Forms.Button
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.DesignPix = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.btnGIF, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp.SuspendLayout()
        Me.gbUploadP.SuspendLayout()
        Me.gbExplore.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DesignPix, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnGIF
        '
        Me.btnGIF.Image = CType(resources.GetObject("btnGIF.Image"), System.Drawing.Image)
        Me.btnGIF.Location = New System.Drawing.Point(818, 235)
        Me.btnGIF.Name = "btnGIF"
        Me.btnGIF.Size = New System.Drawing.Size(32, 32)
        Me.btnGIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.btnGIF.TabIndex = 12
        Me.btnGIF.TabStop = False
        '
        'grp
        '
        Me.grp.Controls.Add(Me.pb)
        Me.grp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp.Location = New System.Drawing.Point(447, 223)
        Me.grp.Name = "grp"
        Me.grp.Size = New System.Drawing.Size(365, 54)
        Me.grp.TabIndex = 10
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
        Me.gbUploadP.Controls.Add(Me.Button2)
        Me.gbUploadP.Controls.Add(Me.btnEnter)
        Me.gbUploadP.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbUploadP.Location = New System.Drawing.Point(447, 134)
        Me.gbUploadP.Name = "gbUploadP"
        Me.gbUploadP.Size = New System.Drawing.Size(395, 83)
        Me.gbUploadP.TabIndex = 9
        Me.gbUploadP.TabStop = False
        Me.gbUploadP.Text = "Enter Set Up"
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(295, 21)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(94, 54)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(156, 21)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(133, 54)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "Old Setup"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = True
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
        Me.gbExplore.Controls.Add(Me.Label6)
        Me.gbExplore.Controls.Add(Me.txtDesc)
        Me.gbExplore.Controls.Add(Me.button1)
        Me.gbExplore.Controls.Add(Me.cboPeriod)
        Me.gbExplore.Controls.Add(Me.cboYear)
        Me.gbExplore.Controls.Add(Me.cmbSt)
        Me.gbExplore.Controls.Add(Me.cboSheet)
        Me.gbExplore.Controls.Add(Me.txtStart)
        Me.gbExplore.Controls.Add(Me.TEXTBOX1)
        Me.gbExplore.Controls.Add(Me.Label3)
        Me.gbExplore.Controls.Add(Me.Label5)
        Me.gbExplore.Controls.Add(Me.lblMonth)
        Me.gbExplore.Controls.Add(Me.Label4)
        Me.gbExplore.Controls.Add(Me.Label2)
        Me.gbExplore.Controls.Add(Me.Label1)
        Me.gbExplore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbExplore.Location = New System.Drawing.Point(12, 134)
        Me.gbExplore.Name = "gbExplore"
        Me.gbExplore.Size = New System.Drawing.Size(429, 144)
        Me.gbExplore.TabIndex = 8
        Me.gbExplore.TabStop = False
        Me.gbExplore.Text = "Data Process"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(210, 98)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(139, 14)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "View GL Accounts Used"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(210, 121)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 14)
        Me.Label7.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(21, 121)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 14)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Description :"
        '
        'txtDesc
        '
        Me.txtDesc.Location = New System.Drawing.Point(113, 118)
        Me.txtDesc.MaxLength = 15
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(94, 20)
        Me.txtDesc.TabIndex = 7
        '
        'button1
        '
        Me.button1.Image = CType(resources.GetObject("button1.Image"), System.Drawing.Image)
        Me.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.button1.Location = New System.Drawing.Point(322, 9)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(96, 23)
        Me.button1.TabIndex = 1
        Me.button1.Text = "Browse"
        Me.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.button1.UseVisualStyleBackColor = True
        '
        'cboPeriod
        '
        Me.cboPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(213, 65)
        Me.cboPeriod.MaxDropDownItems = 12
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(94, 23)
        Me.cboPeriod.TabIndex = 5
        '
        'cboYear
        '
        Me.cboYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(113, 65)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(94, 23)
        Me.cboYear.TabIndex = 4
        '
        'cmbSt
        '
        Me.cmbSt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSt.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSt.ForeColor = System.Drawing.Color.Red
        Me.cmbSt.FormattingEnabled = True
        Me.cmbSt.Items.AddRange(New Object() {"True"})
        Me.cmbSt.Location = New System.Drawing.Point(318, 36)
        Me.cmbSt.Name = "cmbSt"
        Me.cmbSt.Size = New System.Drawing.Size(100, 22)
        Me.cmbSt.TabIndex = 2
        '
        'cboSheet
        '
        Me.cboSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSheet.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSheet.FormattingEnabled = True
        Me.cboSheet.Location = New System.Drawing.Point(113, 36)
        Me.cboSheet.MaxDropDownItems = 21
        Me.cboSheet.Name = "cboSheet"
        Me.cboSheet.Size = New System.Drawing.Size(94, 23)
        Me.cboSheet.TabIndex = 3
        '
        'txtStart
        '
        Me.txtStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStart.Location = New System.Drawing.Point(113, 93)
        Me.txtStart.MaxLength = 2
        Me.txtStart.Name = "txtStart"
        Me.txtStart.Size = New System.Drawing.Size(94, 21)
        Me.txtStart.TabIndex = 6
        '
        'TEXTBOX1
        '
        Me.TEXTBOX1.BackColor = System.Drawing.Color.White
        Me.TEXTBOX1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TEXTBOX1.Location = New System.Drawing.Point(113, 11)
        Me.TEXTBOX1.Name = "TEXTBOX1"
        Me.TEXTBOX1.ReadOnly = True
        Me.TEXTBOX1.Size = New System.Drawing.Size(203, 21)
        Me.TEXTBOX1.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(213, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Set True/False :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(31, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 14)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Start Row :"
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonth.Location = New System.Drawing.Point(313, 72)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(46, 14)
        Me.lblMonth.TabIndex = 0
        Me.lblMonth.Text = "MONTH"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(20, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Year/Period :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(18, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Sheet Name :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Excel Location :"
        '
        'dg
        '
        Me.dg.DataMember = ""
        Me.dg.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dg.Location = New System.Drawing.Point(12, 284)
        Me.dg.Name = "dg"
        Me.dg.Size = New System.Drawing.Size(830, 317)
        Me.dg.TabIndex = 13
        '
        'grd
        '
        Me.grd.Location = New System.Drawing.Point(12, 284)
        Me.grd.Name = "grd"
        Me.grd.OcxState = CType(resources.GetObject("grd.OcxState"), System.Windows.Forms.AxHost.State)
        Me.grd.Size = New System.Drawing.Size(207, 23)
        Me.grd.TabIndex = 14
        '
        'btnclear
        '
        Me.btnclear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclear.Image = CType(resources.GetObject("btnclear.Image"), System.Drawing.Image)
        Me.btnclear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnclear.Location = New System.Drawing.Point(759, 607)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(91, 55)
        Me.btnclear.TabIndex = 33
        Me.btnclear.Text = "Close"
        Me.btnclear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnclear.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(430, 6)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(66, 122)
        Me.PictureBox2.TabIndex = 66
        Me.PictureBox2.TabStop = False
        '
        'DesignPix
        '
        Me.DesignPix.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DesignPix.Image = CType(resources.GetObject("DesignPix.Image"), System.Drawing.Image)
        Me.DesignPix.Location = New System.Drawing.Point(487, 6)
        Me.DesignPix.Name = "DesignPix"
        Me.DesignPix.Size = New System.Drawing.Size(353, 123)
        Me.DesignPix.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.DesignPix.TabIndex = 65
        Me.DesignPix.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(425, 122)
        Me.PictureBox1.TabIndex = 64
        Me.PictureBox1.TabStop = False
        '
        'EDIVISAYAS_XOOM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(856, 664)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.DesignPix)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.grd)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.btnGIF)
        Me.Controls.Add(Me.grp)
        Me.Controls.Add(Me.gbUploadP)
        Me.Controls.Add(Me.gbExplore)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "EDIVISAYAS_XOOM"
        Me.Text = "XOOM - Visayas"
        CType(Me.btnGIF, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp.ResumeLayout(False)
        Me.gbUploadP.ResumeLayout(False)
        Me.gbExplore.ResumeLayout(False)
        Me.gbExplore.PerformLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DesignPix, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGIF As System.Windows.Forms.PictureBox
    Friend WithEvents grp As System.Windows.Forms.GroupBox
    Friend WithEvents pb As System.Windows.Forms.ProgressBar
    Friend WithEvents gbUploadP As System.Windows.Forms.GroupBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnEnter As System.Windows.Forms.Button
    Friend WithEvents gbExplore As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents button1 As System.Windows.Forms.Button
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSt As System.Windows.Forms.ComboBox
    Friend WithEvents cboSheet As System.Windows.Forms.ComboBox
    Friend WithEvents txtStart As System.Windows.Forms.TextBox
    Friend WithEvents TEXTBOX1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGrid
    Friend WithEvents grd As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents OpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnclear As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents DesignPix As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
