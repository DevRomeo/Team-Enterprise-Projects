Imports System.Data.SqlClient
Public Class DeleteUpdate
    Inherits System.Windows.Forms.Form
    Private m_DataSet As DataSet

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
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvDel As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(DeleteUpdate))
        Me.cmdClose = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.lvDel = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(696, 320)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 43)
        Me.cmdClose.TabIndex = 16
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.Gainsboro
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(616, 320)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(80, 43)
        Me.btnDelete.TabIndex = 15
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lvDel
        '
        Me.lvDel.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lvDel.FullRowSelect = True
        Me.lvDel.GridLines = True
        Me.lvDel.Location = New System.Drawing.Point(8, 24)
        Me.lvDel.Name = "lvDel"
        Me.lvDel.Size = New System.Drawing.Size(760, 288)
        Me.lvDel.TabIndex = 17
        Me.lvDel.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Email Address"
        Me.ColumnHeader1.Width = 200
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Report Title"
        Me.ColumnHeader2.Width = 300
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Head Office"
        Me.ColumnHeader3.Width = 100
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Specific"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Schedule"
        Me.ColumnHeader5.Width = 81
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = ""
        Me.ColumnHeader6.Width = 1
        '
        'DeleteUpdate
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(784, 375)
        Me.Controls.Add(Me.lvDel)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "DeleteUpdate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Delete Recipients"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub DeleteUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sql As String = "SELECT EmailAddress,ReportTitle,valueParamClass_01,r.Specific,valueParamdate,CONVERT(varchar(50),r.rowguid) as rowguid FROM tbl_Recipients_V3 r INNER  JOIN tbl_Reports_V3 ON reportid=ID order by EmailAddress"
        Dim dr As SqlClient.SqlDataReader
        Dim db As New clsDBConnection
        If db.isConnected = True Then
            db.CloseConnection()
        End If
        db.ConnectDB(ls_connectionStringLocal)
        dr = db.Execute_SQL_DataReader(sql)
        Dim ctr As Integer = 0
        Try
            While dr.Read

                lvDel.Items.Add(CStr(dr.Item("EmailAddress")).Trim)
                lvDel.Items(lvDel.Items.Count - 1).SubItems.Add(CStr(dr.Item("ReportTitle")).Trim)
                lvDel.Items(lvDel.Items.Count - 1).SubItems.Add(CStr(dr.Item("valueParamClass_01")).Trim)
                lvDel.Items(lvDel.Items.Count - 1).SubItems.Add(CStr(dr.Item("specific")).Trim)
                lvDel.Items(lvDel.Items.Count - 1).SubItems.Add(CStr(dr.Item("valueParamdate")).Trim)
                lvDel.Items(lvDel.Items.Count - 1).SubItems.Add(CStr(dr.Item("rowguid")).Trim)

            End While
            dr.Close()
            lvDel.TopItem.Focused = True
        Catch ex As Exception

        End Try
        db.CloseConnection()

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try

            If lvDel.Items.Count > 0 Then
                Dim email, guid, sql As String
                Dim db As New clsDBConnection
                email = lvDel.FocusedItem.SubItems.Item(0).Text
                guid = lvDel.FocusedItem.SubItems.Item(5).Text
                sql = "Delete tbl_Recipients_V3 where EmailAddress ='" & email & "'" & _
                " AND rowguid='" & guid & "'"
                If db.isConnected = True Then
                    db.CloseConnection()
                End If
                db.ConnectDB(ls_connectionStringLocal)
                If db.Execute_SQLQuery(sql) < 1 Then
                    MsgBox("Can not delete the selected report.", MsgBoxStyle.Critical, "Error")
                Else
                    MsgBox("Done")
                    lvDel.FocusedItem.ForeColor = Color.Red
                End If
                db.CloseConnection()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
