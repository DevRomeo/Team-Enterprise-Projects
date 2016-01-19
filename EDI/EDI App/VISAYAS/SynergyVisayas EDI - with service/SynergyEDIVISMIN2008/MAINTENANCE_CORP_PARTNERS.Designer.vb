<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MAINTENANCE_CORP_PARTNERS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MAINTENANCE_CORP_PARTNERS))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.txtGLCredit = New System.Windows.Forms.TextBox
        Me.txtGLDebit = New System.Windows.Forms.TextBox
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.txtCorpcode = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.btnViewAll = New System.Windows.Forms.Button
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
        Me.GroupBox1.Controls.Add(Me.txtGLCredit)
        Me.GroupBox1.Controls.Add(Me.txtGLDebit)
        Me.GroupBox1.Controls.Add(Me.txtDesc)
        Me.GroupBox1.Controls.Add(Me.txtCorpcode)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 140)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(578, 142)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CORPORATE PARTNERS"
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.SystemColors.Control
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(508, 103)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(56, 31)
        Me.btnEdit.TabIndex = 7
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.SystemColors.Control
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(441, 103)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(61, 31)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.btnUpdate.Image = CType(resources.GetObject("btnUpdate.Image"), System.Drawing.Image)
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(364, 103)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(71, 31)
        Me.btnUpdate.TabIndex = 5
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'txtGLCredit
        '
        Me.txtGLCredit.Location = New System.Drawing.Point(87, 101)
        Me.txtGLCredit.MaxLength = 10
        Me.txtGLCredit.Name = "txtGLCredit"
        Me.txtGLCredit.Size = New System.Drawing.Size(100, 20)
        Me.txtGLCredit.TabIndex = 4
        '
        'txtGLDebit
        '
        Me.txtGLDebit.Location = New System.Drawing.Point(87, 75)
        Me.txtGLDebit.MaxLength = 10
        Me.txtGLDebit.Name = "txtGLDebit"
        Me.txtGLDebit.Size = New System.Drawing.Size(100, 20)
        Me.txtGLDebit.TabIndex = 3
        '
        'txtDesc
        '
        Me.txtDesc.Location = New System.Drawing.Point(87, 49)
        Me.txtDesc.MaxLength = 50
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(161, 20)
        Me.txtDesc.TabIndex = 2
        '
        'txtCorpcode
        '
        Me.txtCorpcode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCorpcode.Location = New System.Drawing.Point(87, 23)
        Me.txtCorpcode.MaxLength = 10
        Me.txtCorpcode.Name = "txtCorpcode"
        Me.txtCorpcode.Size = New System.Drawing.Size(100, 20)
        Me.txtCorpcode.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 77)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "G/L Debit     :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "G/L Credit   :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Corp Code  :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Corp Name :"
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(11, 317)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(578, 186)
        Me.ListView1.TabIndex = 11
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Corp Code"
        Me.ColumnHeader1.Width = 77
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Description"
        Me.ColumnHeader2.Width = 133
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "GL Debit"
        Me.ColumnHeader3.Width = 165
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "GL / Credit"
        Me.ColumnHeader4.Width = 201
        '
        'btnViewAll
        '
        Me.btnViewAll.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewAll.Image = CType(resources.GetObject("btnViewAll.Image"), System.Drawing.Image)
        Me.btnViewAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnViewAll.Location = New System.Drawing.Point(11, 288)
        Me.btnViewAll.Name = "btnViewAll"
        Me.btnViewAll.Size = New System.Drawing.Size(81, 23)
        Me.btnViewAll.TabIndex = 8
        Me.btnViewAll.Text = "View All"
        Me.btnViewAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnViewAll.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(11, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(425, 122)
        Me.PictureBox1.TabIndex = 66
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(430, 12)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(160, 122)
        Me.PictureBox2.TabIndex = 68
        Me.PictureBox2.TabStop = False
        '
        'btnclear
        '
        Me.btnclear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclear.Image = CType(resources.GetObject("btnclear.Image"), System.Drawing.Image)
        Me.btnclear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnclear.Location = New System.Drawing.Point(499, 509)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(91, 54)
        Me.btnclear.TabIndex = 69
        Me.btnclear.Text = "Close"
        Me.btnclear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnclear.UseVisualStyleBackColor = True
        '
        'MAINTENANCE_CORP_PARTNERS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(602, 568)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.btnViewAll)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MAINTENANCE_CORP_PARTNERS"
        Me.Text = "MAINTENANCE CORP PARTNERS - Visayas"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtGLCredit As System.Windows.Forms.TextBox
    Friend WithEvents txtGLDebit As System.Windows.Forms.TextBox
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtCorpcode As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnViewAll As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnclear As System.Windows.Forms.Button
End Class
