Public Class clsdatalog
    Dim con As SqlClient.SqlConnection
    Dim conr As SqlClient.SqlConnection
    Dim server, ui, db, pass As String
    Dim bt As SqlClient.SqlTransaction

    Public Sub New()
        con = New SqlClient.SqlConnection
        conr = New SqlClient.SqlConnection
    End Sub
    Public Sub dispose()
        con.Close()
    End Sub
    Public Function clsConnect() As Boolean
        Try
            Dim sr As New IO.StreamReader(Application.StartupPath & "\synergymaintenance.ini")
            Dim line As String = Nothing
            line = sr.ReadLine
            While Not line Is Nothing
                line = line.Replace(" =", "=").Replace("= ", "=")
                If line.StartsWith("[server]=") Then
                    server = Replace(line, "[server]=", "")

                End If
                If line.StartsWith("[db]=") Then
                    db = Replace(line, "[db]=", "")
                End If
                If line.StartsWith("[userid]=") Then
                    ui = Replace(line, "[userid]=", "")
                End If
                If line.StartsWith("[password]=") Then
                    pass = decryptPass(Replace(line, "[password]=", ""))
                End If
                line = sr.ReadLine
            End While
            sr.Close()
            clsConnect = True
        Catch ex As SqlClient.SqlException
            MessageBox.Show("Error Message: " & ex.Message & vbCrLf & "Error Number: " & _
            ex.Number & vbCrLf & "Source: " & _
            ex.Source & vbCrLf & "Stack Trace: " & ex.StackTrace, "Error in connection to database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            clsConnect = False
        End Try
    End Function
    Shared Function decryptPass(ByVal RawStr As String) As String

        Dim i As Integer
        Dim decryptedPass As String
        i = 3
        decryptedPass = ""
        While i < RawStr.Length
            decryptedPass = decryptedPass + RawStr.Substring(i - 1, 1)
            i = NextPrime(i)
        End While
        decryptPass = decryptedPass

    End Function
    Shared Function NextPrime(ByVal i As Integer) As Integer
        Dim ctr As Integer
        ctr = i + 1
        While Not isPrime(ctr)
            ctr = ctr + 1
        End While
        NextPrime = ctr
    End Function
    Shared Function isPrime(ByVal i As Integer) As Boolean
        If i = 3 Or i = 5 Then Return True
        If i Mod 2 = 0 Then Return False
        If i Mod 3 = 0 Then Return False
        If i Mod 5 = 0 Then Return False
        Return True
    End Function
    Public Function errorConnectionWriting() As Boolean
        Try
            If con.State Then
                con.Close()
            End If
            con.ConnectionString = "data source = " & server & "; initial catalog = " & db & _
                "; user id = " & ui & "; password = " & pass
            con.Open()
            bt = con.BeginTransaction
            errorConnectionWriting = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Connection failed")
            errorConnectionWriting = True
        End Try
    End Function
    Public Function ErrorConnectionReading(ByVal write As Boolean) As Boolean
        Try
            If conr.State Then
                conr.Close()
            End If
            conr.ConnectionString = "data source = " & server & "; initial catalog = " & db & _
                "; user id = " & ui & "; password = " & pass
            If write Then
                conr.Open()
                bt = conr.BeginTransaction()
            Else
                conr.Open()
            End If
            ErrorConnectionReading = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Connection failed")
            ErrorConnectionReading = True
        End Try
    End Function
    Public Function dr(ByVal sqltxt As String, ByRef rdr As SqlClient.SqlDataReader) As Boolean
        Try
            Dim com As SqlClient.SqlCommand
            com = conr.CreateCommand
            com.CommandText = sqltxt

            com.CommandTimeout = 3600
            rdr = com.ExecuteReader
            dr = True
        Catch ex As Exception
            MsgBox(ex.Message)
            dr = False
        End Try
    End Function
End Class
