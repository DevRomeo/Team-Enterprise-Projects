Imports System
Public Class clsFunc
    Private sw As IO.StreamWriter
    Private d As DateTime = Now

    Public Sub Grp_Visible(ByVal grp As Object, ByVal pb As Object, ByVal cap As String)
        grp.text = cap
        grp.BringToFront()
        grp.Visible = True
        pb.Value = 0
    End Sub

    Public Sub ShowForm(ByVal frm As Object)
        frm.show()
    End Sub

    Public Sub HideForm(ByVal frm As Object)
        frm.hide()
    End Sub
    Public Function ErrTxt(ByVal a_sqltxt1 As String, ByVal a_sqltxt2 As String) As String
        ErrTxt = "Error log(SQL): " & a_sqltxt1 & " SQL error message: " & a_sqltxt2
    End Function
    Public Sub Error_Log(ByVal errorTxt As String, ByVal loc As String)
        Dim fs As New IO.FileStream(loc, IO.FileMode.Append)
        sw = New IO.StreamWriter(fs)
        sw.WriteLine(errorTxt)
        sw.Flush()
        sw.Close()
        fs.Close()
    End Sub
    Public Sub Imsg(ByVal s1 As String, ByVal s2 As String)
        MsgBox(s1, MsgBoxStyle.Information, s2)
    End Sub
    Public Function Form_Running(ByVal ao_frm As Object) As Boolean
        Try
            If g_bol Then
                MsgBox("Another program is currently running. You can not perform multiple task." & _
                vbCrLf & "Close the other program.", MsgBoxStyle.Exclamation, "another program running")
                Form_Running = True
            Else
                g_bol = True
                ao_frm.show()
                Form_Running = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Form_Running = False
        End Try
    End Function
    Public Sub Form_Closed()
        g_bol = False
    End Sub
    Public Sub Initialize_Combo(ByVal a_cbo As Object, ByVal a_start As Integer, ByVal a_end As Integer)
        Dim n As Integer = Nothing
        With a_cbo
            .Items.Clear()
            For n = a_start To a_end
                .Items.Add(n)
            Next
            .SelectedIndex = 0
        End With
    End Sub
    Public Sub Tray_Icon(ByVal a_ti As NotifyIcon, ByVal frm As Object, ByVal v_frm As Boolean, ByVal v_ti As Boolean)
        frm.visible = v_frm
        a_ti.Visible = v_ti
    End Sub

    Public Sub NameForm(ByVal name As String, ByVal frm As Object)
        frm.text = name
    End Sub

    Public Sub Hide_form(ByVal frm As Object)
        frm.hide()
    End Sub

    Public Sub Show_Form(ByVal frm As Object)
        frm.show()
    End Sub

    Public Sub Emsg(ByVal s1 As String, ByVal s2 As String)
        MsgBox(s1, MsgBoxStyle.Critical, s2)
    End Sub

    Public Function Error_Encr(ByVal txtlen As Integer, ByVal strtxt As String, ByRef enc As String) As Boolean
        Try
            Dim n As Integer = Nothing
            For n = 1 To txtlen
                enc = enc & Chr(Asc(Mid(strtxt, n, 1)) + 15)
            Next
            Error_Encr = False
        Catch ex As Exception
            Error_Encr = True
        End Try
    End Function

    Public Function Error_Decr(ByVal txtlen As Integer, ByVal strtxt As String, ByRef dec As String) As Boolean
        Try
            Dim n As Integer = Nothing
            For n = 1 To txtlen
                dec = dec & Chr(Asc(Mid(strtxt, n, 1)) - 15)
            Next
            Error_Decr = False
        Catch ex As Exception
            Error_Decr = True
        End Try
    End Function

    Public Function Error_Nonumeric(ByVal ot As Object) As Boolean
        Dim n As Integer
        ot.text = Trim(ot.text)
        For n = 1 To Len(ot.text)
            If Not IsNumeric(Mid(ot.text, n, 1)) Then
                Error_Nonumeric = True
                ot.forecolor = Color.Red
                Exit Function
            Else
                ot.forecolor = Color.Black
                Error_Nonumeric = False
            End If
        Next
    End Function

    Public Function IsEmptySv(ByVal maxL As Integer, ByVal ot As Object, ByVal ol As Object) As Boolean
        Dim n As Integer
        For n = 0 To maxL
            ot(n).text = Trim(UCase(ot(n).text))
            If ot(n).text = "" Then
                ol(n).forecolor = Color.Red
                IsEmptySv = True
            Else
                ol(n).forecolor = Color.Black
            End If
        Next
        If IsEmptySv = True Then
            IsEmptySv = True
        Else
            IsEmptySv = False
        End If
    End Function
End Class
