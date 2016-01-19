﻿Public Class ClsDataEDIMINDANAO
    Dim connW As SqlClient.SqlConnection
    Dim connR As SqlClient.SqlConnection

    Dim server, ui, db, pass, ls_jobtitle1, ls_jobtitle2 As String
    Dim bt As SqlClient.SqlTransaction
    Public Sub New()
        connR = New SqlClient.SqlConnection
        connW = New SqlClient.SqlConnection
    End Sub
    Public Sub DisposeR()
        If connR.State Then
            connR.Close()
        End If
    End Sub
    Public Sub DisposeW()
        If connW.State Then
            connW.Close()
        End If
    End Sub
    Public Function errorConnectionWriting() As Boolean
        Try
            If connW.State Then
                connW.Close()
            End If
            connW.ConnectionString = "data source = " & server & "; initial catalog = " & db & _
                "; user id = " & ui & "; password = " & pass
            connW.Open()
            bt = connW.BeginTransaction
            errorConnectionWriting = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Connection failed")
            errorConnectionWriting = True
        End Try
    End Function

    Public Function ErrorConnectionReading(ByVal write As Boolean) As Boolean
        Try
            If connR.State Then
                connR.Close()
            End If
            connR.ConnectionString = "data source = " & server & "; initial catalog = " & db & _
                "; user id = " & ui & "; password = " & pass
            If write Then
                connR.Open()
                bt = connR.BeginTransaction()
            Else
                connR.Open()
            End If
            ErrorConnectionReading = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Connection failed")
            ErrorConnectionReading = True
        End Try
    End Function


    Public Function Error_Inititalize_INI() As Boolean
        Try
            Dim sr As New IO.StreamReader(Application.StartupPath & "\EDIMINDANAO.ini")
            Dim line As String = Nothing
            line = sr.ReadLine
            While Not line Is Nothing
                line = line.Replace(" =", "=").Replace("= ", "=")
                If line.StartsWith("[server]=") Then
                    server = Replace(line, "[server]=", "")
                End If
                If line.StartsWith("[db]=") Then
                    db = Replace(line, "[db]=", "")
                    'Added 3/21/2011
                    database = db
                End If
                If line.StartsWith("[userid]=") Then
                    ui = Replace(line, "[userid]=", "")
                End If
                If line.StartsWith("[password]=") Then
                    pass = Replace(line, "[password]=", "")
                End If
                If line.StartsWith("[job1]=") Then
                    ls_jobtitle1 = Replace(line, "[job1]=", "")
                    MODLS_JOBTITLE = ls_jobtitle1
                End If
                line = sr.ReadLine
            End While
            sr.Close()
            Error_Inititalize_INI = False
        Catch ex As SqlClient.SqlException
            MessageBox.Show("Error Message: " & ex.Message & vbCrLf & "Error Number: " & _
            ex.Number & vbCrLf & "Source: " & _
            ex.Source & vbCrLf & "Stack Trace: " & ex.StackTrace, "Error in connection to database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Error_Inititalize_INI = True
        End Try
    End Function
    Public Function Error_SetDS(ByRef ds As DataSet, ByVal sqltxt As String) As Boolean
        Try
            Dim oda As SqlClient.SqlDataAdapter
            ds = New DataSet
            oda = New SqlClient.SqlDataAdapter(sqltxt, connR)
            oda.Fill(ds)
            Error_SetDS = False
        Catch ex As Exception
            MsgBox(ex.Message)
            Error_SetDS = True
        End Try
    End Function
    Public Function Error_SetComm(ByVal sqlTxt As String, ByVal bcr As Boolean, ByRef sqlmsg As String) As Boolean
        Try
            Dim comm As New SqlClient.SqlCommand
            comm = connW.CreateCommand
            comm.CommandText = sqlTxt
            comm.Transaction = bt
            comm.ExecuteNonQuery()
            If bcr Then
                bt.Commit()
                bt.Dispose()
            End If
            Error_SetComm = False
        Catch ex As Exception
            MsgBox(ex.Message)
            sqlmsg = ex.Message
            Error_SetComm = True
            bt.Rollback()
            bt.Dispose()
        End Try
    End Function
    Public Function Error_SetRdr(ByVal sqlTxt As String, ByRef rdr As SqlClient.SqlDataReader, ByRef sqlmsg As String) As Boolean

        Try
            Dim commR As SqlClient.SqlCommand = Nothing
            commR = connR.CreateCommand
            'commR.CommandTimeout = 50
            commR.CommandTimeout = 0 ' NEW ELY CODE
            commR.CommandText = sqlTxt
            rdr = commR.ExecuteReader
            Error_SetRdr = False

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, sqlTxt)
            sqlmsg = ex.Message
            Error_SetRdr = True
        End Try
    End Function

    Public Function dr(ByVal sqltxt As String, ByRef rdr As SqlClient.SqlDataReader) As Boolean
        'Dim F1 As New form1
        Try
            Dim com As SqlClient.SqlCommand
            com = connR.CreateCommand
            com.CommandText = sqltxt

            com.CommandTimeout = 1500 'ELI CODE
            rdr = com.ExecuteReader
            'F1.GRPLOADING.Hide() 'NEW ELI CODE
            'MsgBox("STANBY", 64, "PER BRANCH") 'ELI CODE
            dr = True
        Catch ex As Exception
            MsgBox(ex.Message)
            dr = False
        End Try
    End Function
    'Added 7142010 by Ching2: Another INITIALIZE_INI function
    Public Sub INITIALIZE_INI()
        Try
            Dim sr As New IO.StreamReader(Application.StartupPath & "\EDIMINDANAO.ini")
            Dim line As String = Nothing
            Dim server As String = ""
            Dim db As String = ""
            Dim pass As String = ""
            Dim ui As String = ""

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
                    pass = Replace(line, "[password]=", "")
                End If
                If line.StartsWith("[job1]=") Then
                    ls_jobtitle1 = Replace(line, "[job1]=", "")
                    MODLS_JOBTITLE = ls_jobtitle1
                End If
                line = sr.ReadLine
            End While

            ls_connection = "data source = " & server & "; initial catalog = " & db & _
                "; user id = " & ui & "; password = " & pass


            sr.Close()

        Catch ex As SqlClient.SqlException
            MessageBox.Show("Error Message: " & ex.Message & vbCrLf & "Error Number: " & _
            ex.Number & vbCrLf & "Source: " & _
            ex.Source & vbCrLf & "Stack Trace: " & ex.StackTrace, "Error in connection to database", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
