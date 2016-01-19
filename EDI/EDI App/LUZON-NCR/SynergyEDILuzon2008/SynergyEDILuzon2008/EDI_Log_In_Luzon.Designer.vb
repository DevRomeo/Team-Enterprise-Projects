<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EDI_Log_In_Luzon
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EDI_Log_In_Luzon))
        Me.txtPass = New System.Windows.Forms.TextBox
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.OkBtn = New System.Windows.Forms.Button
        Me.CancelBtn = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtPass
        '
        Me.txtPass.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPass.Location = New System.Drawing.Point(39, 121)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPass.Size = New System.Drawing.Size(204, 20)
        Me.txtPass.TabIndex = 2
        '
        'txtUser
        '
        Me.txtUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUser.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUser.Location = New System.Drawing.Point(39, 58)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(204, 20)
        Me.txtUser.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Controls.Add(Me.PictureBox1)
        Me.GroupBox2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, -1)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(289, 356)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(6, 17)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(277, 81)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(36, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Username :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(36, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 15)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Password :"
        '
        'OkBtn
        '
        Me.OkBtn.Image = CType(resources.GetObject("OkBtn.Image"), System.Drawing.Image)
        Me.OkBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.OkBtn.Location = New System.Drawing.Point(75, 170)
        Me.OkBtn.Name = "OkBtn"
        Me.OkBtn.Size = New System.Drawing.Size(62, 40)
        Me.OkBtn.TabIndex = 8
        Me.OkBtn.Text = "Ok"
        Me.OkBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.OkBtn.UseVisualStyleBackColor = True
        '
        'CancelBtn
        '
        Me.CancelBtn.Image = CType(resources.GetObject("CancelBtn.Image"), System.Drawing.Image)
        Me.CancelBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.CancelBtn.Location = New System.Drawing.Point(143, 170)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(62, 40)
        Me.CancelBtn.TabIndex = 9
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox3.Controls.Add(Me.CancelBtn)
        Me.GroupBox3.Controls.Add(Me.OkBtn)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.txtPass)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtUser)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 109)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(277, 240)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        '
        'EDI_Log_In_Luzon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(304, 367)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "EDI_Log_In_Luzon"
        Me.Text = "Log - In"
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtPass As System.Windows.Forms.TextBox
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents CancelBtn As System.Windows.Forms.Button
    Friend WithEvents OkBtn As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
