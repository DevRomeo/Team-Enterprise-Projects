<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BOSKPLuzon
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BOSKPLuzon))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.txtBranchName = New System.Windows.Forms.TextBox
        Me.txtKpcode = New System.Windows.Forms.TextBox
        Me.txtBosCode = New System.Windows.Forms.TextBox
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
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnEdit)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.btnUpdate)
        Me.GroupBox1.Controls.Add(Me.txtBranchName)
        Me.GroupBox1.Controls.Add(Me.txtKpcode)
        Me.GroupBox1.Controls.Add(Me.txtBosCode)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 134)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(568, 128)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Add Branch"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(178, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 14)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "(numeric only)"
        '
        'btnEdit
        '
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(487, 99)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 23)
        Me.btnEdit.TabIndex = 8
        Me.btnEdit.Text = "EDIT"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(406, 99)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "SAVE"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Image = CType(resources.GetObject("btnUpdate.Image"), System.Drawing.Image)
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(327, 99)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "UPDATE"
        Me.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'txtBranchName
        '
        Me.txtBranchName.Location = New System.Drawing.Point(72, 86)
        Me.txtBranchName.MaxLength = 40
        Me.txtBranchName.Name = "txtBranchName"
        Me.txtBranchName.Size = New System.Drawing.Size(221, 20)
        Me.txtBranchName.TabIndex = 5
        '
        'txtKpcode
        '
        Me.txtKpcode.Location = New System.Drawing.Point(72, 56)
        Me.txtKpcode.MaxLength = 10
        Me.txtKpcode.Name = "txtKpcode"
        Me.txtKpcode.Size = New System.Drawing.Size(100, 20)
        Me.txtKpcode.TabIndex = 4
        '
        'txtBosCode
        '
        Me.txtBosCode.Location = New System.Drawing.Point(72, 23)
        Me.txtBosCode.MaxLength = 3
        Me.txtBosCode.Name = "txtBosCode"
        Me.txtBosCode.Size = New System.Drawing.Size(100, 20)
        Me.txtBosCode.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 14)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "B Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 14)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "KP Code:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "BOS Code:"
        '
        'btnViewAll
        '
        Me.btnViewAll.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewAll.Image = CType(resources.GetObject("btnViewAll.Image"), System.Drawing.Image)
        Me.btnViewAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnViewAll.Location = New System.Drawing.Point(12, 273)
        Me.btnViewAll.Name = "btnViewAll"
        Me.btnViewAll.Size = New System.Drawing.Size(117, 23)
        Me.btnViewAll.TabIndex = 1
        Me.btnViewAll.Text = "View All"
        Me.btnViewAll.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(12, 302)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(568, 197)
        Me.ListView1.TabIndex = 2
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "BOS Code"
        Me.ColumnHeader1.Width = 74
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "KP Code"
        Me.ColumnHeader2.Width = 115
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Branch Name"
        Me.ColumnHeader3.Width = 373
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 10)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(429, 123)
        Me.PictureBox1.TabIndex = 36
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(431, 11)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(147, 122)
        Me.PictureBox2.TabIndex = 38
        Me.PictureBox2.TabStop = False
        '
        'btnclear
        '
        Me.btnclear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclear.Image = CType(resources.GetObject("btnclear.Image"), System.Drawing.Image)
        Me.btnclear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnclear.Location = New System.Drawing.Point(489, 505)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(91, 54)
        Me.btnclear.TabIndex = 69
        Me.btnclear.Text = "Close"
        Me.btnclear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnclear.UseVisualStyleBackColor = True
        '
        'BOSKPLuzon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(600, 561)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.btnViewAll)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "BOSKPLuzon"
        Me.Text = "BOSKPLuzon"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
    Friend WithEvents txtKpcode As System.Windows.Forms.TextBox
    Friend WithEvents txtBosCode As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnViewAll As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnclear As System.Windows.Forms.Button
End Class
