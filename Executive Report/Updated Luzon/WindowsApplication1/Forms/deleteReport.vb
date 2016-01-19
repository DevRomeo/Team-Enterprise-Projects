Imports System.Data.SqlClient

Public Class delete
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
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dg As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(delete))
        Me.cmdClose = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.dg = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(410, 273)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 39)
        Me.cmdClose.TabIndex = 18
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.Gainsboro
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(328, 274)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(79, 39)
        Me.btnDelete.TabIndex = 20
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dg
        '
        Me.dg.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.dg.FullRowSelect = True
        Me.dg.GridLines = True
        Me.dg.Location = New System.Drawing.Point(10, 27)
        Me.dg.Name = "dg"
        Me.dg.Size = New System.Drawing.Size(478, 232)
        Me.dg.TabIndex = 21
        Me.dg.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "ID"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Report Title"
        Me.ColumnHeader2.Width = 288
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Report File"
        Me.ColumnHeader3.Width = 125
        '
        'delete
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(494, 317)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.cmdClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "delete"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Delete Report"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub delete_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.btnDelete.Image = Image.FromFile(Application.StartupPath + "\IMAGE\delete.jpg")
        Dim sql As String = "Select ID,ReportTitle,ReportName FROM tbl_Reports_V3 order by ID "
        Dim db As New clsDBConnection
        Dim dr As SqlDataReader

        If db.isConnected = True Then
            db.CloseConnection()
            db.ConnectDB(ls_connectionStringLocal)
        Else
            db.ConnectDB(ls_connectionStringLocal)
        End If
        dr = db.Execute_SQL_DataReader(sql)
        Dim ctr As Integer = 1
        Try


            While dr.Read

                dg.Items.Add(Trim(CStr(dr.Item("ID"))))
                dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportTitle"))))
                dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportName"))))
                'dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("Status"))))
                If ctr Mod 2 = 0 Then
                    dg.Items(ctr - 1).BackColor = Color.Aquamarine
                End If
                ctr = ctr + 1

            End While
            dg.TopItem.Focused = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            Dim sql As String = "DELETE tbl_Reports_V3 WHERE ID='" & dg.FocusedItem.SubItems.Item(0).Text & "' AND ID NOT IN(SELECT reportID From tbl_Recipients_V3 where ReportID='" & dg.FocusedItem.SubItems.Item(0).Text & "') "
            Dim ans As MsgBoxResult = MsgBox("This item will be deleted in the database.Continue?", MsgBoxStyle.YesNo, "Email Scheduler  V3.0")
            If ans = MsgBoxResult.Yes Then
                If objDBLocal.isConnected Then
                    objDBLocal.CloseConnection()
                End If

                objDBLocal.ConnectDB(ls_connectionStringLocal)

                If objDBLocal.Execute_SQLQuery(sql) < 1 Then
                    MsgBox("Error in deleting data. May be report has still recipient/s. ", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
                Else
                    Dim sql2 As String = "Select ID,ReportTitle,ReportName FROM tbl_Reports_V3 order by ID"
                    Dim db As New clsDBConnection
                    Dim dr As SqlDataReader

                    If db.isConnected = True Then
                        db.CloseConnection()
                        db.ConnectDB(ls_connectionStringLocal)
                    Else
                        db.ConnectDB(ls_connectionStringLocal)
                    End If
                    dr = db.Execute_SQL_DataReader(sql2)
                    Me.dg.Items.Clear()

                    Dim ctr As Integer = 1
                    While dr.Read

                        dg.Items.Add(Trim(CStr(dr.Item("ID"))))
                        dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportTitle"))))
                        dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("ReportName"))))
                        'dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("Status"))))
                        If ctr Mod 2 = 0 Then
                            dg.Items(ctr - 1).BackColor = Color.Aquamarine
                        End If
                        ctr = ctr + 1

                    End While
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
