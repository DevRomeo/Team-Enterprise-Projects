Imports EDIdataClass
Imports Newtonsoft.Json
Public Class MAINTENANCE_BOSKP
    Private SQLTXT As String = Nothing
    Private sqlmsg As String = Nothing
    Dim LS_BCODE As String = Nothing
    Dim datenow As DateTime = Now

    Private Sub MAINTENANCE_BOSKP_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub
    Private Sub MAINTENANCE_BOSKP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "BOSKP- " & ediname & gs_version2
        Me.CenterToScreen()
        btnUpdate.Hide()
    End Sub

    Private Sub btnViewAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewAll.Click
        'VIEW ALL BUTTON

        ListView1.Show()

        ListView1.Items.Clear()
        Dim response As New Response
        Dim listdata As List(Of Boskp)
        Try
            response = toResponse(service.getAllBoskp(edi))
        Catch ex As Exception

        End Try

        If (response.responseCode = ResponseCode.OK) Then
            listdata = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(List(Of Boskp)))
            For i As Integer = 0 To listdata.Count - 1

                ListView1.Items.Add(Trim(listdata(i).boscode))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(listdata(i).kpcode))
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Trim(listdata(i).description))
            Next
        End If
        'ListView1.OwnerDraw = True
        'ListView1.Refresh()
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        LS_BCODE = InputBox("Please input branch code.", "BOSKP - Visayas Maintenance")
        If LS_BCODE = "" Then
        Else
            findbranch()
        End If
        'ListView1.Hide()
    End Sub
    Public Function DATANOTCOMPLETE() As Boolean
        If Me.txtBosCode.Text = "" Then
            MsgBox("Some Data is not filled", 16, "BOSKP")
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtBranchName.Text = "" Then
            MsgBox("Some Data is not filled", 16, "BOSKP")
            DATANOTCOMPLETE = True
            Exit Function
        ElseIf Me.txtKpCode.Text = "" Then
            MsgBox("Some Data is not filled", 16, "BOSKP")
            DATANOTCOMPLETE = True
            Exit Function
        End If
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'ADD BUTTON SAVE
        If Me.DATANOTCOMPLETE = True Then
            Exit Sub
        Else
            If Me.alreadysave = False Then
                Dim rply As Integer = Nothing
                rply = MsgBox("Are you sure you want to save this Branch", MsgBoxStyle.YesNo, "BOSKP")
                If rply = vbYes Then
                    Dim response As New Response
                    Dim branch As New Boskp
                    Try
                        branch.boscode = txtBosCode.Text.Trim
                        branch.kpcode = txtKpCode.Text.Trim
                        branch.description = txtBranchName.Text.Trim
                        branch.class_01 = ediname
                        branch.class_02 = ediname
                        branch.class_03 = ediname
                        branch.class_04 = ediname

                        response = toResponse(service.addBoskp(JsonConvert.SerializeObject(branch), edi))

                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                        Exit Sub
                    End Try
                    If (response.responseCode = ResponseCode.Error) Then
                        MessageBox.Show(response.responseMessage)
                    Else
                        MsgBox("Branch Save in ", 64, "BOSKP")
                    End If


                    Exit Sub
                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Public Function alreadysave() As Boolean
   
        Dim response As New Response
        Try
            response = toResponse(service.getBoskpviaBoscode(txtBosCode.Text, edi))
        Catch ex As Exception
            alreadysave = True
        End Try
        If (response.responseCode = ResponseCode.Error) Then
            MsgBox("Something went wrong")
            alreadysave = True
        End If

        If (response.responseCode <> ResponseCode.NotFound) Then
            MsgBox("Branch already exist", 16, "BOSKP")
            alreadysave = True
        Else
            alreadysave = False
        End If
        Exit Function
    End Function
    Public Sub findbranch()
        Dim response As New Response
        Try
            response = toResponse(service.getBoskpviaBoscode(LS_BCODE, edi))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return
        End Try
        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        End If
        If (response.responseCode = ResponseCode.NotFound) Then
            MsgBox("Branch Code does not exist", 16, "BOSKP")
            Exit Sub
        Else
            Dim brnch As Boskp = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(Boskp))
            txtBosCode.Text = brnch.boscode
            txtKpCode.Text = brnch.kpcode
            txtBranchName.Text = brnch.description
            btnUpdate.Show()

        End If
        
    End Sub

    Private Sub txtBosCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBosCode.KeyPress
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

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim response As New Response
        
        Dim RPLY As Integer = Nothing
        RPLY = MsgBox("Are you sure you want to edit this branch", MsgBoxStyle.YesNo, "BOSKP")
        If RPLY = vbYes Then
            Try
                Dim bskp As New Boskp
                bskp.boscode = txtBosCode.Text
                bskp.kpcode = txtKpCode.Text
                bskp.description = txtBranchName.Text
                response = toResponse(service.updateBoskp(JsonConvert.SerializeObject(bskp), LS_BCODE, edi))
            Catch ex As Exception

            End Try

            If (response.responseCode = ResponseCode.OK) Then
                MsgBox("Branch updated in Synergy Visayas", 64, "BOSKP")
            Else
                MsgBox("Something went wrong")
            End If
            Exit Sub
        Else
            Exit Sub
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Me.Close()
    End Sub
End Class