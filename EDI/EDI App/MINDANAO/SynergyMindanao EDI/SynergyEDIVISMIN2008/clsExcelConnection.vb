Imports System.Xml.Schema
Imports System.Data


Public Class clsExcelConnection
    Private connOledb As OleDb.OleDbConnection
    Private strConn As String = "provider = Microsoft.jet.oledb.4.0;"
    Public Function Excel_Connection(ByVal strFilename As String) As Boolean
        Try

            connOledb.ConnectionString = strConn & "data source = " & strFilename & _
            "; Extended Properties = " & ControlChars.Quote & _
            "Excel 8.0; HDR = no" & ControlChars.Quote

            If connOledb.State Then
                connOledb.Close()
            End If
            Excel_Connection = True
        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message)
            Excel_Connection = False
        End Try
    End Function
    Public Function Error_Get_Sheets_Numbers(ByRef xls() As String) As Boolean

        Dim dt As DataTable
        Dim row As DataRow
        Dim i As Integer = 0
        Dim stn As String = Nothing
        connOledb.Open()

        dt = connOledb.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, Nothing)

        ReDim xls(dt.Rows.Count)
        For Each row In dt.Rows
            stn = row("table_name").ToString
            xls(i) = stn.Substring(0, stn.Length - 1)
            i += 1
        Next

    End Function
    Public Sub dispose()
        If connOledb.State Then
            connOledb.Close()

        End If
    End Sub
    Public Sub New()
        connOledb = New OleDb.OleDbConnection
    End Sub
    Public Function Error_SetDS(ByVal sqltxt As String, ByRef ds As DataSet) As Boolean
        Try
            ds = New DataSet
            Dim oda As New OleDb.OleDbDataAdapter(sqltxt, connOledb)
            oda.Fill(ds)
            Error_SetDS = False
        Catch ex As Exception
            MsgBox(ex.Message)
            Error_SetDS = True
        End Try
    End Function
End Class
