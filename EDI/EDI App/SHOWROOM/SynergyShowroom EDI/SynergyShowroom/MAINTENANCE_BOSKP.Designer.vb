<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MAINTENANCE_BOSKP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MAINTENANCE_BOSKP))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.txtBranchName = New System.Windows.Forms.TextBox
        Me.txtKpCode = New System.Windows.Forms.TextBox
        Me.txtBosCode = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnViewAll = New System.Windows.Forms.Button
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.btnclear = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnEdit)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.btnUpdate)
        Me.GroupBox1.Controls.Add(Me.txtBranchName)
        Me.GroupBox1.Controls.Add(Me.txtKpCode)
        Me.GroupBox1.Controls.Add(Me.txtBosCode)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(6, 138)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(578, 127)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "BOSKP - Vismin"
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.White
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(515, 91)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(56, 31)
        Me.btnEdit.TabIndex = 5
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.White
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(448, 91)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(61, 31)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.White
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Image = CType(resources.GetObject("btnUpdate.Image"), System.Drawing.Image)
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(371, 91)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(71, 31)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'txtBranchName
        '
        Me.txtBranchName.Location = New System.Drawing.Point(99, 76)
        Me.txtBranchName.MaxLength = 45
        Me.txtBranchName.Name = "txtBranchName"
        Me.txtBranchName.Size = New System.Drawing.Size(100, 20)
        Me.txtBranchName.TabIndex = 3
        '
        'txtKpCode
        '
        Me.txtKpCode.Location = New System.Drawing.Point(99, 50)
        Me.txtKpCode.MaxLength = 15
        Me.txtKpCode.Name = "txtKpCode"
        Me.txtKpCode.Size = New System.Drawing.Size(100, 20)
        Me.txtKpCode.TabIndex = 2
        '
        'txtBosCode
        '
        Me.txtBosCode.Location = New System.Drawing.Point(99, 24)
        Me.txtBosCode.MaxLength = 3
        Me.txtBosCode.Name = "txtBosCode"
        Me.txtBosCode.Size = New System.Drawing.Size(100, 20)
        Me.txtBosCode.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(197, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 14)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "(numeric only)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 14)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Branch Name :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 14)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "KP Code          :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "BOS Code       :"
        '
        'btnViewAll
        '
        Me.btnViewAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewAll.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewAll.Image = CType(resources.GetObject("btnViewAll.Image"), System.Drawing.Image)
        Me.btnViewAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnViewAll.Location = New System.Drawing.Point(6, 269)
        Me.btnViewAll.Name = "btnViewAll"
        Me.btnViewAll.Size = New System.Drawing.Size(81, 23)
        Me.btnViewAll.TabIndex = 7
        Me.btnViewAll.Text = "View All"
        Me.btnViewAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnViewAll.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(6, 298)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(578, 186)
        Me.ListView1.TabIndex = 9
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "BOS Code"
        Me.ColumnHeader1.Width = 77
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "KP Code"
        Me.ColumnHeader2.Width = 159
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Branch Name"
        Me.ColumnHeader3.Width = 320
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(6, 10)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(425, 122)
        Me.PictureBox1.TabIndex = 65
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(424, 10)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(160, 122)
        Me.PictureBox2.TabIndex = 67
        Me.PictureBox2.TabStop = False
        '
        'btnclear
        '
        Me.btnclear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclear.Image = CType(resources.GetObject("btnclear.Image"), System.Drawing.Image)
        Me.btnclear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnclear.Location = New System.Drawing.Point(494, 489)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(91, 54)
        Me.btnclear.TabIndex = 68
        Me.btnclear.Text = "Close"
        Me.btnclear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnclear.UseVisualStyleBackColor = True
        '
        'MAINTENANCE_BOSKP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(602, 547)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnViewAll)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MAINTENANCE_BOSKP"
        Me.Text = "BOSKP - SHOWROOM"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
    Friend WithEvents txtKpCode As System.Windows.Forms.TextBox
    Friend WithEvents txtBosCode As System.Windows.Forms.TextBox
    Friend WithEvents btnViewAll As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnclear As System.Windows.Forms.Button
End Class
