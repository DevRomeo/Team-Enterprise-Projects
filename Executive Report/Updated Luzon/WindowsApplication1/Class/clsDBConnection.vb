Public Class clsDBConnection

    Private cnConnection As System.Data.SqlClient.SqlConnection

    Private ps_SQLCommand As System.Data.SqlClient.SqlCommand
    Private ps_SQLTransaction As System.Data.SqlClient.SqlTransaction

    Private objCommon As New clsCommon

    Public Function ConnectDB(ByVal as_connectionString As String) As Boolean

        Try
            'check connection string
            If objCommon.Coalesce(as_connectionString, "") = "" Then
                ConnectDB = False
                Exit Function
            End If

            'close connection if open
            If isConnected() Then
                cnConnection.Close()
                cnConnection = Nothing
            End If

            cnConnection = New System.Data.SqlClient.SqlConnection
            cnConnection.ConnectionString = as_connectionString
            cnConnection.Open()

            ConnectDB = True
        Catch ex As System.Data.SqlClient.SqlException
            CloseConnection()
            ConnectDB = False
        End Try

    End Function

    Public Function isConnected() As Boolean

        If cnConnection Is Nothing Then
            isConnected = False
            Exit Function
        End If

        If cnConnection.State = System.Data.ConnectionState.Open Then
            isConnected = True
        Else
            isConnected = False
        End If

    End Function

    Public Function CloseConnection() As Boolean

        If cnConnection Is Nothing Then
            CloseConnection = True
            Exit Function
        End If

        If cnConnection.State <> System.Data.ConnectionState.Open Then
            cnConnection.Close()
            cnConnection = Nothing
            CloseConnection = True
        Else
            CloseConnection = False
        End If

    End Function

    Public Function Execute_SQL_DataReader(ByVal as_sql As String) As System.Data.SqlClient.SqlDataReader

        Dim sqlCommand As New System.Data.SqlClient.SqlCommand

        Try

            sqlCommand.CommandText = as_sql
            sqlCommand.Connection = cnConnection
            Execute_SQL_DataReader = sqlCommand.ExecuteReader

        Catch ex As Exception
            'MsgBox(ex.ToString)

            Execute_SQL_DataReader = Nothing

        End Try

        sqlCommand = Nothing

    End Function

    Public Function Execute_SQL_DataSet(ByVal as_sql As String, ByVal as_fieldname As String) As System.Data.DataSet

        Dim sqlCommand As New System.Data.SqlClient.SqlCommand
        Dim sqlReader As System.Data.SqlClient.SqlDataAdapter
        Dim ds_dataSet As New System.Data.DataSet


        Try

            sqlCommand.CommandType = System.Data.CommandType.Text
            sqlCommand.CommandText = as_sql
            sqlCommand.Connection = cnConnection

            sqlReader = New System.Data.SqlClient.SqlDataAdapter(sqlCommand)

            If as_fieldname <> "" Then
                sqlReader.Fill(ds_dataSet, as_fieldname)
            Else
                sqlReader.Fill(ds_dataSet)
            End If

            'FBY 20080702
            If ds_dataSet.Tables(0).Rows.Count <= 0 Then
                ds_dataSet = Nothing
            End If

            Execute_SQL_DataSet = ds_dataSet

        Catch ex As Exception
            Execute_SQL_DataSet = Nothing
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Email Scheduler  V3.0")
        End Try

        sqlCommand = Nothing
        sqlReader = Nothing
        ds_dataSet = Nothing

    End Function

    Public Function Execute_SQLQuery(ByVal as_sql As String) As Integer

        Try
            If isConnected() Then
                ps_SQLCommand = New System.Data.SqlClient.SqlCommand(as_sql, cnConnection)
                ps_SQLCommand.Transaction = ps_SQLTransaction
                ps_SQLCommand.CommandTimeout = 0
                Execute_SQLQuery = ps_SQLCommand.ExecuteNonQuery()
                Exit Function
            Else
                Execute_SQLQuery = 0
            End If
        Catch ex As Exception
            Call LogError(ex.ToString)
            Execute_SQLQuery = 0
        End Try

    End Function

    Public Function BeginTransaction() As Boolean
        Try
            If isConnected() Then
                ps_SQLTransaction = cnConnection.BeginTransaction()
                BeginTransaction = True
                Exit Function
            Else
                BeginTransaction = False
            End If
        Catch ex As Exception
            BeginTransaction = False
        End Try
    End Function

    Public Function CommitTransaction() As Boolean
        Try
            If isConnected() Then
                ps_SQLTransaction.Commit()
                CommitTransaction = True
                Exit Function
            Else
                CommitTransaction = False
            End If
        Catch ex As Exception

            CommitTransaction = False
        End Try

        ps_SQLCommand = Nothing
        ps_SQLTransaction = Nothing
    End Function

    Public Function RollbackTransaction() As Boolean
        Try
            If isConnected() Then
                ps_SQLTransaction.Rollback()
                RollbackTransaction = True
                Exit Function
            Else
                RollbackTransaction = False
            End If
        Catch ex As Exception

            RollbackTransaction = False
        End Try

        ps_SQLCommand = Nothing
        ps_SQLTransaction = Nothing
    End Function
End Class
