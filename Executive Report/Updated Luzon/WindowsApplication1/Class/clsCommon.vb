Public Class clsCommon

    Public Function Coalesce(ByVal ao_object As Object, ByVal as_default As String) As String
        If IsDBNull(ao_object) Then
            Coalesce = as_default
        Else
            Coalesce = ao_object.ToString
        End If
    End Function

    Public Function Log(ByVal as_log As String, ByVal as_filename As String) As Boolean

        Dim objFile As System.IO.FileStream
        Dim objWriter As System.IO.StreamWriter

        Try

            If System.IO.File.Exists(as_filename) Then
                objFile = New System.IO.FileStream(as_filename, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.Write)
            Else
                objFile = New System.IO.FileStream(as_filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write)
            End If

            objWriter = New System.IO.StreamWriter(objFile)

            objWriter.BaseStream.Seek(0, System.IO.SeekOrigin.End)
            objWriter.WriteLine(as_log)
            objWriter.Close()

            Log = True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Email Scheduler  V3.0")
            Log = False
        End Try

    End Function

    Public Function Parse_Ex(ByRef as_string As String, ByVal as_delimiter As String) As String
        Dim li_index As Long = as_string.IndexOf(as_delimiter)
        If li_index >= 0 Then
            Parse_Ex = as_string.Substring(0, li_index)
            as_string = as_string.Substring(li_index + as_delimiter.Length, as_string.Length - (li_index + as_delimiter.Length))
        Else
            Parse_Ex = as_string
            as_string = ""
        End If
    End Function

    Public Function Parse(ByRef as_string As String) As String
        Parse = Parse_Ex(as_string, ",")
    End Function

    Public Function EscapeApostrophe(ByVal as_string As String) As String
        EscapeApostrophe = Replace(as_string, "'", "''")
    End Function


End Class
