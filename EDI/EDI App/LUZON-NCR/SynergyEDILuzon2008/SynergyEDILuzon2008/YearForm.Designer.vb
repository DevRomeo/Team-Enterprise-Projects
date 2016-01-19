<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YearForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(YearForm))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnEnter = New System.Windows.Forms.Button
        Me.txtYear = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(241, 41)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "CANCEL"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnEnter
        '
        Me.btnEnter.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEnter.Image = CType(resources.GetObject("btnEnter.Image"), System.Drawing.Image)
        Me.btnEnter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEnter.Location = New System.Drawing.Point(168, 40)
        Me.btnEnter.Name = "btnEnter"
        Me.btnEnter.Size = New System.Drawing.Size(67, 23)
        Me.btnEnter.TabIndex = 6
        Me.btnEnter.Text = "ENTER"
        Me.btnEnter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEnter.UseVisualStyleBackColor = True
        '
        'txtYear
        '
        Me.txtYear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYear.Location = New System.Drawing.Point(93, 14)
        Me.txtYear.MaxLength = 4
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(223, 20)
        Me.txtYear.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 14)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "INPUT YEAR:"
        '
        'YearForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(317, 68)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnEnter)
        Me.Controls.Add(Me.txtYear)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "YearForm"
        Me.Text = "YEAR"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnEnter As System.Windows.Forms.Button
    Friend WithEvents txtYear As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
