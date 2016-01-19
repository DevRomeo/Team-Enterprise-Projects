Imports EDIdataClass
Imports Newtonsoft.Json
Public Class MAINTENANCE_CORP_PARTNERS
    Private sqlmsg As String = Nothing
    Dim LS_BCODE As String = Nothing
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim RPLY As Integer = Nothing
        RPLY = MsgBox("Are you sure you want to edit this Corporate", MsgBoxStyle.YesNo, "Corporate Maintenance")
        If RPLY = vbYes Then
            Dim response As New Response
            Dim partner As New Corporate_Partners
            Try
                partner.CORPCODE = LS_BCODE
                partner.CORPNAME = txtDesc.Text.Trim
                partner.GLCREDIT = txtGLCredit.Text.Trim
                partner.GLDEBIT = txtGLDebit.Text.Trim

                response = toResponse(service.updateCorporate(JsonConvert.SerializeObject(partner), LS_BCODE, edi))

            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Exit Sub
            End Try
            If (response.responseCode = ResponseCode.Error) Then
                MessageBox.Show(response.responseMessage)
                Exit Sub
            ElseIf response.responseCode = ResponseCode.OK Then
                MsgBox("Corporate updated in Synergy", 64, "Corporate Maintenance")
            End If
        
        End If
    End Sub

    Private Sub MAINTENANCE_CORP_PARTNERS_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub

    Private Sub MAINTENANCE_CORP_PARTNERS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "CORP PARTNERS- " & ediname & gs_version2
        btnUpdate.Hide()
        txtCorpcode.ReadOnly = False
        Me.CenterToScreen()
    End Sub

    Private Sub btnViewAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewAll.Click
        'VIEW ALL BUTTON
        txtCorpcode.ReadOnly = False
        ListView1.Show()
        ListView1.Items.Clear()
        Dim response As New Response
        Try
            response = toResponse(service.getAllCorpPartners(edi))

        Catch ex As Exception
            Exit Sub
        End Try
        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show("Something went wrong")
        Else
            Dim corpP As New List(Of Corporate_Partners)
            corpP = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(List(Of Corporate_Partners)))
            For i As Integer = 0 To corpP.Count - 1
                ListView1.Items.Add(Trim(corpP(i).CORPCODE))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(corpP(i).CORPNAME))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(corpP(i).GLDEBIT)
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(corpP(i).GLCREDIT)
            Next
        End If

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
                    Dim response As New Response
                    Dim partner As New Corporate_Partners
                    partner.CORPCODE = txtCorpcode.Text
                    partner.CORPNAME = txtDesc.Text
                    partner.GLDEBIT = txtGLDebit.Text
                    partner.GLCREDIT = txtGLCredit.Text
                    Try
                        response = toResponse(service.addCorporate(JsonConvert.SerializeObject(partner), edi))
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                        Exit Sub
                    End Try
                    If (response.responseCode = ResponseCode.Error) Then
                        MessageBox.Show(response.responseMessage)
                        Exit Sub
                    ElseIf (response.responseCode = ResponseCode.OK) Then
                        MsgBox("Corporate Save in Synergy Visayas", 64, "Corporate Maintenance")
                    End If


                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Public Function alreadysave() As Boolean
        Dim response As New Response
        Try
            response = toResponse(service.getCorpPartners(txtCorpcode.Text, edi))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Function
        End Try
        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            alreadysave = True
        End If
        If response.responseCode = ResponseCode.OK Then
            MsgBox("Corporate already exist", 16, "Corporate Partners")
            alreadysave = True
        ElseIf response.responseCode = ResponseCode.NotFound Then
            alreadysave = False
        End If

    End Function
    Public Function DATANOTCOMPLETE() As Boolean
        If Me.txtCorpcode.Text = "" Then
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

        End If
    End Sub
    Public Sub findbranch()
        Dim response As New Response
        Dim bskp As New Corporate_Partners
        Try
            response = toResponse(service.getCorpPartners(LS_BCODE, edi))
            bskp = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(Corporate_Partners))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        ElseIf (response.responseCode = ResponseCode.NotFound) Then
            MsgBox("Corporate Code does not exist", 16, "Corporate Maintenance")
            Exit Sub
        ElseIf (response.responseCode = ResponseCode.OK) Then
            txtCorpcode.ReadOnly = True
            txtCorpcode.Text = Trim(bskp.CORPCODE)
            txtDesc.Text = Trim(bskp.CORPNAME)
            txtGLDebit.Text = Trim(bskp.GLDEBIT)
            txtGLCredit.Text = Trim(bskp.GLCREDIT)
            btnUpdate.Show()
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
        Me.Close()
    End Sub
End Class