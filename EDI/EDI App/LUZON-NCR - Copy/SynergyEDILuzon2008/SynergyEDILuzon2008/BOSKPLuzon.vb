Public Class BOSKPLuzon

    Private SQLTXT As String = Nothing
    Private sqlmsg As String = Nothing
    Dim LS_BCODE As String = Nothing
    Private nonNumberEntered As Boolean = False
    Dim datenow As DateTime = Now

    Private Sub BOSKPLuzon_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim MAIN As New EDI_Luzon_Main
        MAIN.Show()
        Me.Hide()
    End Sub

    Private Sub BOSKPLuzon_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "BOSKP-" & gs_serverloc & " " & gs_version2
        Me.CenterToScreen()
        btnUpdate.Hide()
    End Sub

    Private Sub btnViewAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewAll.Click
        'VIEW ALL BUTTON
        ListView1.Show()
        ListView1.Items.Clear()
        Dim c As New ClsDataEDILuzon
        Dim SQLTXT As String = "select boscode,kpcode,description from boskp where class_01 not like 'Visayas/Mindanao' order by boscode asc"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
        While rdr.Read
            ListView1.Items.Add(Trim(rdr(0)))
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(rdr(1)))
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(rdr(2)))
        End While
        rdr.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'ADD BUTTON SAVE
        If Me.DATANOTCOMPLETE = True Then
            Exit Sub
        Else
            If Me.alreadysave = False Then
                Dim rply As Integer = Nothing
                rply = MsgBox("Are you sure you want to save this Branch", MsgBoxStyle.YesNo, gs_version)
                If rply = vbYes Then
                    Dim c As New ClsDataEDILuzon
                    Dim SQLTXT As String = "INSERT INTO BOSKP " & _
                    "(BOSCODE,KPCODE,DESCRIPTION,CLASS_01,CLASS_02,CLASS_03,CLASS_04)" & _
                    " VALUES ('" + Me.txtBosCode.Text + "','" + Me.txtKpCode.Text + "','" + Me.txtBranchName.Text + "','LUZON','LUZON','LUZON','LUZON')"
                    Dim rdr As SqlClient.SqlDataReader = Nothing
                    If c.Error_Inititalize_INI Then Exit Sub
                    If c.ErrorConnectionReading(False) Then Exit Sub
                    If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
                    MsgBox("Branch Save in Synergy Luzon", 64, gs_version)

                    Dim cls2 As New ClsDataEDILuzon
                    Dim task As String = Nothing
                    Dim name As String = Nothing
                    Dim rdr2 As SqlClient.SqlDataReader = Nothing
                    Dim dept As String = "select task, fullname from humres where res_id = '" & ps_resid & "'"
                    If cls2.Error_Inititalize_INI() Then Exit Sub
                    If cls2.ErrorConnectionReading(False) Then Exit Sub
                    If Not cls2.dr(dept, rdr2) Then Exit Sub
                    If rdr2.Read Then
                        task = rdr2.Item(0).ToString.Trim
                        name = rdr2.Item(1).ToString.Trim
                    End If
                    rdr.Close()

                    Dim cls As New LogsEDILUZON
                    Dim rdr1 As SqlClient.SqlDataReader = Nothing
                    Dim act As String = "BOSKP LUZON" & " " & txtBosCode.Text.Trim & " " & txtKpCode.Text.Trim & " " & txtBranchName.Text.Trim
                    Dim syndev As String = "insert into edi_maintainance_logs (datetimelog, application, activity, resource, department, remarks)values" & _
                                           "('" + datenow + "', 'BOSKP', '" + act + "', '" & name & "', '" & task & "', 'DONE')"

                    If cls.Error_Inititalize_INI() Then Exit Sub
                    If cls.ErrorConnectionReading(False) Then Exit Sub
                    If Not cls.dr(syndev, rdr1) Then Exit Sub
                    Exit Sub
                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Public Function DATANOTCOMPLETE() As Boolean
        If Me.txtBosCode.Text = "" Then
            MsgBox("Some Data is not filled", 16, gs_version)
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtBranchName.Text = "" Then
            MsgBox("Some Data is not filled", 16, gs_version)
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtKpCode.Text = "" Then
            MsgBox("Some Data is not filled", 16, gs_version)
            DATANOTCOMPLETE = True
            Exit Function
        End If
    End Function

    Public Function alreadysave() As Boolean
        Dim c As New ClsDataEDILuzon
        Dim SQLTXT As String = "SELECT * FROM BOSKP WHERE BOSCODE = '" + txtBosCode.Text + "' AND class_01 not like 'Visayas/Mindanao'"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Function
        If c.ErrorConnectionReading(False) Then Exit Function
        If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Function
        If rdr.Read Then
            MsgBox("Branch already exist", 16, gs_version)
            alreadysave = True
        Else
            alreadysave = False
        End If
        Exit Function
    End Function

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        LS_BCODE = InputBox("Please input Branch Code of the branch that you want to edit", gs_version)
        findbranch()
        btnUpdate.Show()
    End Sub

    Public Sub findbranch()
        Dim c As New ClsDataEDILuzon
        Dim SQLTXT As String = "SELECT * FROM BOSKP WHERE BOSCODE = '" + LS_BCODE + "' AND class_01 not like 'Visayas/Mindanao'"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
        If rdr.Read Then
            txtBosCode.Text = Trim(rdr(0))
            txtKpCode.Text = Trim(rdr(1))
            txtBranchName.Text = Trim(rdr(2))

            Exit Sub
        Else
            MsgBox("Branch Code does not exist", 16, gs_version)
            Exit Sub
        End If
        rdr.Close()
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        LS_BCODE = Me.ListView1.FocusedItem.Text
        findbranch()
        btnUpdate.Show()
    End Sub

    Private Sub txtBosCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBosCode.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim RPLY As Integer = Nothing
        RPLY = MsgBox("Are you sure you want to edit this branch", MsgBoxStyle.YesNo, gs_version)
        If RPLY = vbYes Then
            Dim c As New ClsDataEDILuzon
            Dim SQLTXT As String = "UPDATE  BOSKP SET BOSCODE = '" + Me.txtBosCode.Text + "' " & _
            " ,KPCODE = '" + Me.txtKpcode.Text + "' ,DESCRIPTION = '" + Me.txtBranchName.Text + "' " & _
            "where boscode = '" + Me.txtBosCode.Text + "'"
            Dim rdr As SqlClient.SqlDataReader = Nothing
            If c.Error_Inititalize_INI Then Exit Sub
            If c.ErrorConnectionReading(False) Then Exit Sub
            If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
            MsgBox("Branch updated in " & gs_serverloc, 64, gs_version)
            Exit Sub
        Else
            Exit Sub
        End If
    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Dim MAIN As New EDI_Luzon_Main
        MAIN.Show()
        Me.Dispose()
    End Sub
End Class