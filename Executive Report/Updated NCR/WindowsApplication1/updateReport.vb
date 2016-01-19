Imports System.Data.SqlClient
Public Class updateReport
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents dg As System.Windows.Forms.DataGrid
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(updateReport))
        Me.dg = New System.Windows.Forms.DataGrid
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dg
        '
        Me.dg.AlternatingBackColor = System.Drawing.Color.LightSalmon
        Me.dg.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.dg.CaptionVisible = False
        Me.dg.DataMember = ""
        Me.dg.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dg.Location = New System.Drawing.Point(8, 8)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.RowHeadersVisible = False
        Me.dg.Size = New System.Drawing.Size(624, 336)
        Me.dg.TabIndex = 0
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(536, 360)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(80, 32)
        Me.cmdClose.TabIndex = 16
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(440, 360)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(88, 32)
        Me.cmdSave.TabIndex = 15
        Me.cmdSave.Text = "&Update"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'updateReport
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(640, 398)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.dg)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "updateReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Update Report"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub updateReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            Dim sql As String = "SELECT * FROM tbl_Reports_V3"
            Dim ds As DataSet = New DataSet
            Dim data_adapter As SqlDataAdapter

            data_adapter = New SqlDataAdapter(sql, ls_connectionStringLocal)
            data_adapter.TableMappings.Add("Table", "tbl_Reports_V3")
            data_adapter.Fill(ds, "tbl_Reports_V3")
            dg.SetDataBinding(ds, "tbl_Reports_V3")
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try


    End Sub
    Private Sub dg_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.CurrentCellChanged
        dg.Select(dg.CurrentRowIndex)
    End Sub

    'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    '    Dim name, Class_01, server, db, Classification, Uname, Pass, ParamDate, ParamClass_01, ParamClass_03, ParamClass_04 As String
    '    Dim sched, schedDate As Integer
    '    Dim frm As New AddReport
    '    name = Me.dg.Item(dg.CurrentRowIndex, 1)
    '    Class_01 = Trim(Me.dg.Item(dg.CurrentRowIndex, 3))
    '    server = Trim(Me.dg.Item(dg.CurrentRowIndex, 4))
    '    db = Trim(Me.dg.Item(dg.CurrentRowIndex, 5))
    '    Classification = Me.dg.Item(dg.CurrentRowIndex, 6)
    '    Uname = Trim(Me.dg.Item(dg.CurrentRowIndex, 7))
    '    Pass = Trim(Me.dg.Item(dg.CurrentRowIndex, 8))
    '    ParamDate = Trim(Me.dg.Item(dg.CurrentRowIndex, 9))
    '    ParamClass_01 = Trim(Me.dg.Item(dg.CurrentRowIndex, 10))
    '    ParamClass_03 = Trim(Me.dg.Item(dg.CurrentRowIndex, 11))
    '    ParamClass_04 = Trim(Me.dg.Item(dg.CurrentRowIndex, 12))
    '    sched = Trim(Me.dg.Item(dg.CurrentRowIndex, 13))
    '    schedDate = Trim(Me.dg.Item(dg.CurrentRowIndex, 14))
    '    frm.txtReportName.Text = name
    '    frm.cmbClassification.Text = Classification
    '    frm.cmbArea.Text = Class_01
    '    frm.txtServer.Text = server
    '    frm.txtDb.Text = db
    '    frm.txtUsername.Text = Uname
    '    frm.txtPassword.Text = Pass

    '    If ParamClass_01 <> "" Then
    '        frm.chkclass_01.Checked = True
    '        frm.txtClass_01.Text = ParamClass_01
    '    End If
    '    If ParamDate <> "" Then
    '        frm.chkDate.Checked = True
    '        frm.txtDate.Text = ParamClass_01
    '    End If
    '    If ParamClass_03 <> "" Then
    '        frm.chkRegion.Checked = True
    '        frm.txtRegion.Text = ParamClass_03
    '    End If
    '    If ParamClass_04 <> "" Then
    '        frm.chkArea.Checked = True
    '        frm.txtArea.Text = ParamClass_04
    '    End If
    '    If schedDate = 0 Then
    '        frm.rdDaily.Checked = True
    '    Else
    '        If sched = 2 Then
    '            frm.rdWeekly.Checked = True
    '            frm.txtWeekly.Text = schedDate
    '        Else
    '            frm.rdMonthly.Checked = True
    '            frm.txtMonthly.Text = schedDate
    '        End If
    '    End If

    '    frm.Text = "Update Report"
    '    frm.ShowDialog()
    'End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
End Class
