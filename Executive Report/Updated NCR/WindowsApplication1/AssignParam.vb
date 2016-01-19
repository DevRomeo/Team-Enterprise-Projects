Public Class AssignParam
    Inherits System.Windows.Forms.Form
    Dim ctrMin As Integer
    Dim isload As Boolean

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmbDate As System.Windows.Forms.ComboBox
    Friend WithEvents lblSchedule As System.Windows.Forms.Label
    Friend WithEvents txtScheduleDate As System.Windows.Forms.TextBox
    Friend WithEvents cmbHO As System.Windows.Forms.ComboBox
    Friend WithEvents cmbClass_02 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbRegion As System.Windows.Forms.ComboBox
    Friend WithEvents txtBcode As System.Windows.Forms.TextBox
    Friend WithEvents cmbArea As System.Windows.Forms.ComboBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents rdHO As System.Windows.Forms.RadioButton
    Friend WithEvents rdArea As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AssignParam))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.cmbDate = New System.Windows.Forms.ComboBox
        Me.lblSchedule = New System.Windows.Forms.Label
        Me.txtScheduleDate = New System.Windows.Forms.TextBox
        Me.cmbHO = New System.Windows.Forms.ComboBox
        Me.cmbClass_02 = New System.Windows.Forms.ComboBox
        Me.cmbRegion = New System.Windows.Forms.ComboBox
        Me.cmbArea = New System.Windows.Forms.ComboBox
        Me.txtBcode = New System.Windows.Forms.TextBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label14 = New System.Windows.Forms.Label
        Me.rdHO = New System.Windows.Forms.RadioButton
        Me.rdArea = New System.Windows.Forms.RadioButton
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(104, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(104, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(136, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(104, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 23)
        Me.Label3.TabIndex = 2
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(104, 144)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(136, 23)
        Me.Label4.TabIndex = 5
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(104, 208)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(136, 23)
        Me.Label5.TabIndex = 4
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(104, 176)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(136, 23)
        Me.Label6.TabIndex = 3
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 145)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 23)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Region:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 209)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 23)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Branch Code:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 177)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 23)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Area:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(8, 113)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(80, 23)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Class_02:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 81)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(80, 23)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Head Office:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(8, 49)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(80, 23)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Date:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbDate
        '
        Me.cmbDate.Items.AddRange(New Object() {"Daily", "Weekly", "Monthly"})
        Me.cmbDate.Location = New System.Drawing.Point(248, 48)
        Me.cmbDate.Name = "cmbDate"
        Me.cmbDate.Size = New System.Drawing.Size(104, 21)
        Me.cmbDate.TabIndex = 12
        Me.cmbDate.Text = "Daily"
        '
        'lblSchedule
        '
        Me.lblSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSchedule.Location = New System.Drawing.Point(360, 48)
        Me.lblSchedule.Name = "lblSchedule"
        Me.lblSchedule.Size = New System.Drawing.Size(176, 16)
        Me.lblSchedule.TabIndex = 13
        Me.lblSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtScheduleDate
        '
        Me.txtScheduleDate.Location = New System.Drawing.Point(536, 48)
        Me.txtScheduleDate.MaxLength = 5
        Me.txtScheduleDate.Name = "txtScheduleDate"
        Me.txtScheduleDate.Size = New System.Drawing.Size(32, 20)
        Me.txtScheduleDate.TabIndex = 14
        Me.txtScheduleDate.Text = ""
        '
        'cmbHO
        '
        Me.cmbHO.Items.AddRange(New Object() {"ALL", "NCR", "Visayas/Mindanao"})
        Me.cmbHO.Location = New System.Drawing.Point(248, 80)
        Me.cmbHO.Name = "cmbHO"
        Me.cmbHO.Size = New System.Drawing.Size(144, 21)
        Me.cmbHO.TabIndex = 15
        Me.cmbHO.Text = "ALL"
        '
        'cmbClass_02
        '
        Me.cmbClass_02.Items.AddRange(New Object() {"Luzon", "NCR", "Luzon/NCR", "Mindanao", "Visayas", "Showrooms", "HO"})
        Me.cmbClass_02.Location = New System.Drawing.Point(248, 112)
        Me.cmbClass_02.Name = "cmbClass_02"
        Me.cmbClass_02.Size = New System.Drawing.Size(146, 21)
        Me.cmbClass_02.TabIndex = 16
        Me.cmbClass_02.Text = "Visayas"
        '
        'cmbRegion
        '
        Me.cmbRegion.Location = New System.Drawing.Point(247, 143)
        Me.cmbRegion.Name = "cmbRegion"
        Me.cmbRegion.Size = New System.Drawing.Size(280, 21)
        Me.cmbRegion.Sorted = True
        Me.cmbRegion.TabIndex = 17
        '
        'cmbArea
        '
        Me.cmbArea.Location = New System.Drawing.Point(247, 174)
        Me.cmbArea.Name = "cmbArea"
        Me.cmbArea.Size = New System.Drawing.Size(281, 21)
        Me.cmbArea.Sorted = True
        Me.cmbArea.TabIndex = 18
        '
        'txtBcode
        '
        Me.txtBcode.Location = New System.Drawing.Point(248, 206)
        Me.txtBcode.Name = "txtBcode"
        Me.txtBcode.Size = New System.Drawing.Size(34, 20)
        Me.txtBcode.TabIndex = 19
        Me.txtBcode.Text = ""
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(496, 336)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 32)
        Me.cmdClose.TabIndex = 21
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.Color.Gainsboro
        Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New System.Drawing.Point(425, 336)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(67, 32)
        Me.cmdSave.TabIndex = 20
        Me.cmdSave.Text = "&OK"
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(9, 8)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(571, 23)
        Me.Label13.TabIndex = 22
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(5, 251)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(83, 23)
        Me.Label14.TabIndex = 23
        Me.Label14.Text = "Report For:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'rdHO
        '
        Me.rdHO.Location = New System.Drawing.Point(175, 252)
        Me.rdHO.Name = "rdHO"
        Me.rdHO.TabIndex = 24
        Me.rdHO.Text = "Head Office"
        '
        'rdArea
        '
        Me.rdArea.Checked = True
        Me.rdArea.Location = New System.Drawing.Point(99, 252)
        Me.rdArea.Name = "rdArea"
        Me.rdArea.Size = New System.Drawing.Size(63, 24)
        Me.rdArea.TabIndex = 25
        Me.rdArea.TabStop = True
        Me.rdArea.Text = "AM/RM"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(397, 79)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(20, 20)
        Me.Button1.TabIndex = 26
        Me.Button1.Text = "..."
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(530, 142)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(20, 20)
        Me.Button3.TabIndex = 28
        Me.Button3.Text = "..."
        '
        'AssignParam
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(586, 375)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.rdArea)
        Me.Controls.Add(Me.rdHO)
        Me.Controls.Add(Me.txtBcode)
        Me.Controls.Add(Me.txtScheduleDate)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmbArea)
        Me.Controls.Add(Me.cmbRegion)
        Me.Controls.Add(Me.cmbClass_02)
        Me.Controls.Add(Me.cmbHO)
        Me.Controls.Add(Me.lblSchedule)
        Me.Controls.Add(Me.cmbDate)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "AssignParam"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Assign Value to Parameters"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub AssignParam_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            isload = True
            Me.Label13.Text = reportName
            If paramDate.Length > 0 Then
                Me.Label1.Text = paramDate
            Else
                Me.Label1.Text = "N/A"
                Me.cmbDate.Enabled = False
            End If
            If paramClass_01.Length > 0 Then
                Me.Label2.Text = paramClass_01
            Else
                Me.Label2.Text = "N/A"
                Me.cmbHO.Enabled = False
            End If

            If paramClass_02.Length > 0 Then
                Me.Label3.Text = paramClass_02
            Else
                Me.Label3.Text = "N/A"
                Me.cmbClass_02.Enabled = False
            End If

            If paramClass_03.Length > 0 Then
                Me.Label4.Text = paramClass_03
            Else
                Me.Label4.Text = "N/A"
                Me.cmbRegion.Enabled = False
            End If

            If paramClass_04.Length > 0 Then
                Me.Label5.Text = paramClass_04
            Else
                Me.Label5.Text = "N/A"
                Me.cmbArea.Enabled = False
            End If

            If paramBranchCode.Length > 0 Then
                Me.Label6.Text = paramBranchCode
            Else
                Me.Label6.Text = "N/A"
                Me.txtBcode.Enabled = False
            End If

            If Me.cmbRegion.Enabled = True Then
                getRegion()
            End If
        Catch ex As Exception

        End Try
        isload = False
    End Sub
    Private Sub getHO()
        Dim sql As String = "SELECT distinct Class_01 FROM tbl_Reports_V3 where Classification='" & reportType & "'"

        If ds_Class_01 Is Nothing Then
            If objDBLocal.isConnected Then
                objDBLocal.CloseConnection()
            End If
            objDBLocal.ConnectDB(ls_connectionStringLocal)
            ds_Class_01 = objDBLocal.Execute_SQL_DataSet(sql, "Class_01")
            objDBLocal.CloseConnection()
        End If
        Dim str As String
        Try
            Dim ctr As Integer
            For ctr = 0 To ds_Class_01.Tables(0).Rows.Count - 1
                str = ds_Class_01.Tables(0).Rows(ctr).ItemArray(0)
                Me.cmbHO.Items.Add(str.Trim)
                Me.cmbHO.Text = str.Trim
            Next
            ds_Class_01 = Nothing
        Catch ex As Exception

        End Try


    End Sub

    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged

        Me.lblSchedule.Visible = True
        Me.txtScheduleDate.Visible = True
        Me.txtScheduleDate.Text = ""

        If Me.cmbDate.Text.Trim = "Weekly" Then
            Me.lblSchedule.Text = "Enter 0-6(Sun-Sat):"
            Me.txtScheduleDate.MaxLength = 1
            Me.txtScheduleDate.Text = 0


        ElseIf Me.cmbDate.Text.Trim = "Monthly" Then
            Me.lblSchedule.Text = "Enter day of the month (1-28):"
            Me.txtScheduleDate.MaxLength = 2
            Me.txtScheduleDate.Text = 1
        Else
            Me.lblSchedule.Visible = False
            Me.txtScheduleDate.Visible = False
        End If
    End Sub

    Private Sub getRegion()
        Dim sql As String = "SELECT distinct Class_03 From bedryf where Class_01='" & Me.cmbHO.Text.Trim & "' Order by Class_03 desc"
        Dim db As New clsDBConnection

        Try
            If ds_Class_03 Is Nothing Then
                If db.isConnected Then
                    db.CloseConnection()
                End If

                If Me.cmbHO.Text.Trim = "NCR" Then
                    db.ConnectDB(ls_connectionStringLuzon)
                ElseIf Me.cmbHO.Text = "Visayas/Mindanao" Or Me.cmbHO.Text = "ALL" Then
                    db.ConnectDB(ls_connectionStringVismin)
                End If
                ds_Class_03 = db.Execute_SQL_DataSet(sql, "Class_03")
                db.CloseConnection()
            End If

            Dim str As String
            cmbRegion.Items.Clear()
            Dim ctr As Integer
            For ctr = 0 To ds_Class_03.Tables(0).Rows.Count - 1
                str = ds_Class_03.Tables(0).Rows(ctr).ItemArray(0)
                Me.cmbRegion.Items.Add(str.Trim)
                Me.cmbRegion.Text = str.Trim
            Next
            ds_Class_03 = Nothing
        Catch ex As Exception

        End Try

    End Sub
    Private Sub getArea()
        Dim sql As String = "SELECT distinct Class_04 From bedryf where Class_03='" & Me.cmbRegion.Text.Trim & "' Order by Class_04 desc"
        Dim db As New clsDBConnection

        Try
            If ds_Class_04 Is Nothing Then
                If db.isConnected Then
                    db.CloseConnection()
                End If

                If Me.cmbHO.Text.Trim = "NCR" Then
                    db.ConnectDB(ls_connectionStringLuzon)
                ElseIf Me.cmbHO.Text = "Visayas/Mindanao" Or Me.cmbHO.Text = "ALL" Then
                    db.ConnectDB(ls_connectionStringVismin)
                End If
                ds_Class_04 = db.Execute_SQL_DataSet(sql, "Class_04")
                db.CloseConnection()
            End If

            Dim str As String
            cmbArea.Items.Clear()
            Dim ctr As Integer
            For ctr = 0 To ds_Class_04.Tables(0).Rows.Count - 1
                str = ds_Class_04.Tables(0).Rows(ctr).ItemArray(0)
                Me.cmbArea.Items.Add(str.Trim)
                Me.cmbArea.Text = str.Trim
            Next
            ds_Class_04 = Nothing
        Catch ex As Exception

        End Try


    End Sub
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        isParamOk = False
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        isParamOk = True

        strParamdateDesc = ""
        strparamClass_01 = ""
        strparamClass_03 = ""
        strparamClass_04 = ""
        strparamBranchCode = ""
        spec = ""
        If Me.cmbDate.Enabled = True Then
            strparamDate = Me.cmbDate.Text
            If Me.txtScheduleDate.Visible = True Then
                strParamdateDesc = Me.txtScheduleDate.Text
            End If
        End If

        If Me.cmbHO.Enabled = True Then
            strparamClass_01 = cmbHO.Text
        End If

        If Me.cmbRegion.Enabled = True Then
            strparamClass_03 = Me.cmbRegion.Text
        End If
        If Me.cmbArea.Enabled = True Then
            strparamClass_04 = Me.cmbArea.Text
        End If

        If Me.txtBcode.Enabled = True Then
            strparamBranchCode = txtBcode.Text
        End If

        If rdHO.Checked Then
            spec = "HO"
        Else
            spec = "AM"
        End If

        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        getRegion()
    End Sub

    Private Sub cmbClass_02_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbClass_02.SelectedIndexChanged

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Me.cmbArea.Enabled = True Then
            Call getArea()
        End If
    End Sub

    Private Sub cmbArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArea.SelectedIndexChanged

    End Sub
End Class
