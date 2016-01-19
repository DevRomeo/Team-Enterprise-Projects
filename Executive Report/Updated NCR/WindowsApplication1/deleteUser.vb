Imports System.Data.SqlClient
Public Class deleteUser
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
    Friend WithEvents dg As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(deleteUser))
        Me.dg = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.btnDelete = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'dg
        '
        Me.dg.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.dg.FullRowSelect = True
        Me.dg.GridLines = True
        Me.dg.Location = New System.Drawing.Point(8, 56)
        Me.dg.Name = "dg"
        Me.dg.Size = New System.Drawing.Size(520, 260)
        Me.dg.TabIndex = 24
        Me.dg.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "User Name"
        Me.ColumnHeader1.Width = 113
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Password"
        Me.ColumnHeader2.Width = 125
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Full Name"
        Me.ColumnHeader3.Width = 277
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.Gainsboro
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(368, 328)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(80, 39)
        Me.btnDelete.TabIndex = 23
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(456, 328)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 39)
        Me.cmdClose.TabIndex = 22
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(72, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(376, 40)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "DELETE SYSTEM USER"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'deleteUser
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(544, 374)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.cmdClose)
        Me.MaximizeBox = False
        Me.Name = "deleteUser"
        Me.Text = "Delete User"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub deleteUser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sql As String = "Select username,password,fullname FROM tbl_ReportUser_V3 order by username "
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
        While dr.Read

            dg.Items.Add(Trim(CStr(dr.Item("username"))))
            dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("password"))))
            dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("fullname"))))
            'dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("Status"))))
            If ctr Mod 2 = 0 Then
                dg.Items(ctr - 1).BackColor = Color.Aquamarine
            End If
            ctr = ctr + 1

        End While

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim sql As String = "DELETE tbl_ReportUser_V3 WHERE username='" & dg.FocusedItem.SubItems.Item(0).Text & "' "
        Dim ans As MsgBoxResult = MsgBox("This item will be deleted in the database.Continue?", MsgBoxStyle.YesNo, "Email Scheduler  V3.0")
        If ans = MsgBoxResult.Yes Then

            If objDBLocal.Execute_SQLQuery(sql) < 1 Then
                MsgBox("Error in deleting data.", MsgBoxStyle.Critical, "Email Scheduler  V3.0")
            Else
                Dim sql2 As String = "Select username,password,fullname FROM tbl_ReportUser_V3 order by username "
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
                    dg.Items.Add(Trim(CStr(dr.Item("username"))))
                    dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("password"))))
                    dg.Items(dg.Items.Count - 1).SubItems.Add(Trim(CStr(dr.Item("fullname"))))
                    If ctr Mod 2 = 0 Then
                        dg.Items(ctr - 1).BackColor = Color.Aquamarine
                    End If
                    ctr = ctr + 1
                End While
            End If
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
End Class
