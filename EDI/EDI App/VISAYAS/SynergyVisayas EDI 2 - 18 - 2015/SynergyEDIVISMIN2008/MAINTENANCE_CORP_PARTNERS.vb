Public Class MAINTENANCE_CORP_PARTNERS
    Private sqlmsg As String = Nothing
    Dim LS_BCODE As String = Nothing
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim RPLY As Integer = Nothing
        RPLY = MsgBox("Are you sure you want to edit this Corporate", MsgBoxStyle.YesNo, "Corporate Maintenance")
        If RPLY = vbYes Then
            Dim c As New clsData
            Dim SQLTXT As String = "UPDATE  corporate_partners SET corpcode = '" + Me.txtCorpcode.Text + "' " & _
            " ,corpname = '" + Me.txtDesc.Text + "' ,gldebit = '" + Me.txtGLDebit.Text + "'" & _
            ", glcredit = '" + Me.txtGLCredit.Text + "'" & _
            "where corpcode = '" + Me.txtCorpcode.Text + "'"
            Dim rdr As SqlClient.SqlDataReader = Nothing
            If c.Error_Inititalize_INI Then Exit Sub
            If c.ErrorConnectionReading(False) Then Exit Sub
            If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
            MsgBox("Corporate updated in Synergy", 64, "Corporate Maintenance")
            rdr.Close()
            c.DisposeR()
            Exit Sub
        Else
            Exit Sub
        End If
    End Sub

    Private Sub MAINTENANCE_CORP_PARTNERS_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim main As New Main
        main.Show()
        Me.Hide()
    End Sub

    Private Sub MAINTENANCE_CORP_PARTNERS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "CORP PARTNERS-Visayas " & gs_version2
        btnUpdate.Hide()
        txtCorpcode.ReadOnly = False
        Me.CenterToScreen()
    End Sub

    Private Sub btnViewAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewAll.Click
        'VIEW ALL BUTTON
        txtCorpcode.ReadOnly = False
        ListView1.Show()
        ListView1.Items.Clear()
        Dim c As New clsData
        Dim SQLTXT As String = "select * from corporate_partners order by corpcode asc"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
        While rdr.Read
            ListView1.Items.Add(Trim(rdr(0)))
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(rdr(1)))
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(rdr(2)))
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(rdr(3)))
        End While
        rdr.Close()
        c.DisposeR()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'ADD BUTTON SAVE
        If Me.DATANOTCOMPLETE = True Then
            Exit Sub
        Else
            If Me.alreadysave = False Then
                Dim rply As Integer = Nothing
                rply = MsgBox("Are you sure you want to save this Corporate Partner", MsgBoxStyle.YesNo, "Corporate Partners")
                If rply = vbYes Then
                    Dim c As New clsData
                    Dim SQLTXT As String = "INSERT INTO corporate_partners " & _
                    "(corpcode,corpname,gldebit,glcredit)" & _
                    " VALUES ('" + Me.txtCorpcode.Text + "','" + Me.txtDesc.Text + "','" + Me.txtGLDebit.Text + "','" + Me.txtGLCredit.Text + "')"
                    Dim rdr As SqlClient.SqlDataReader = Nothing
                    If c.Error_Inititalize_INI Then Exit Sub
                    If c.ErrorConnectionReading(False) Then Exit Sub
                    If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
                    MsgBox("Corporate Save in Synergy Visayas", 64, "Corporate Maintenance")
                    rdr.Close()
                    c.DisposeR()
                    Exit Sub
                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Public Function alreadysave() As Boolean
        Dim c As New clsData
        Dim SQLTXT As String = "SELECT * FROM corporate_partners WHERE corpcode = '" + Me.txtCorpCode.Text + "' "
        Dim rdr As SqlClient.SqlDataReader
        If c.Error_Inititalize_INI Then Exit Function
        If c.ErrorConnectionReading(False) Then Exit Function
        If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Function
        If rdr.Read Then
            MsgBox("Corporate already exist", 16, "Corporate Partners")
            alreadysave = True
        Else
            alreadysave = False
        End If
        rdr.Close()
        c.DisposeR()
        Exit Function
    End Function
    Public Function DATANOTCOMPLETE() As Boolean
        If Me.txtCorpCode.Text = "" Then
            MsgBox("Some Data is not filled", 16, "Corporate Maintenance")
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtDesc.Text = "" Then
            MsgBox("Some Data is not filled", 16, "Corporate Maintenance")
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtGLDebit.Text = "" Then
            MsgBox("Some Data is not filled", 16, "Corporate Maintenance")
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtGLCredit.Text = "" Then
            MsgBox("Some Data is not filled", 16, "Corporate Maintenance")
        End If
    End Function

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        LS_BCODE = InputBox("Please input Corp-Code to edit", "Corporate Partners Maintenance")
        If LS_BCODE = "" Then

        Else
            findbranch()
            btnUpdate.Show()
        End If
    End Sub
    Public Sub findbranch()
        Dim c As New clsData
        Dim SQLTXT As String = "SELECT * FROM CORPORATE_PARTNERS WHERE corpcode = '" + LS_BCODE + "' "
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If c.Error_Inititalize_INI Then Exit Sub
        If c.ErrorConnectionReading(False) Then Exit Sub
        If c.Error_SetRdr(SQLTXT, rdr, sqlmsg) Then Exit Sub
        If rdr.Read Then
            txtCorpcode.ReadOnly = True
            txtCorpcode.Text = Trim(rdr(0))
            txtDesc.Text = Trim(rdr(1))
            txtGLDebit.Text = Trim(rdr(2))
            txtGLCredit.Text = Trim(rdr(3))
            btnUpdate.Show()
            rdr.Close()
            c.DisposeR()
            Exit Sub
        Else
            MsgBox("Corporate Code does not exist", 16, "Corporate Maintenance")
            Exit Sub
        End If
    End Sub
    Private Sub txtGLDebit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGLDebit.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub


    Private Sub txtGLCredit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGLCredit.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        LS_BCODE = Me.ListView1.FocusedItem.Text
        findbranch()
        btnUpdate.Show()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Dim main As New Main
        main.Show()
        Me.Hide()
    End Sub
End Class