Imports System.IO
Imports Newtonsoft.Json
Imports EDIdataClass

Module Module1
    Public cbotxt1 As String
    Public txt1 As String
    Public g_bol As Boolean
    Public res_id As String
    Public modifier As String
    Public ps_resid As String
    Public ls_userid As String = Nothing
    Public ls_username As String = Nothing
    Public edi As String
    Public ediname As String
    Public ediprogdesc As String
    Public service As New EDIService.Service
    Public savedAllBranchError As Boolean
    Public maxretry As Integer = 3
    Public retryCount As Integer = 0


    'NEW ELI CODE <----- 2-6-2010

    Public MODLS_JOBTITLE As String = Nothing

    Public callyear As String = Nothing
    Public calledicategory As Integer = Nothing

    'New code added 2-19-2011
    Public database As String = Nothing
    'Public gs_version As String = "EDI Version 3.0"
    Public ls_connection As String = Nothing
    'add 11-20-2014
    Public gs_serverloc As String = ediname + "PerBranch"
    Public gs_serverloc2 As String = ediname + "BaseBranch"
    Public gs_serverloc3 As String = ediname + "Jewellers"
    Public gs_version As String = "(EDI Version 4.1)"
    Public gs_version2 As String = "(Maintenance Version 4.1)"
    Public strBtnName As String = String.Empty
    Public Sub setServiceUrl(ByVal url As String)
        service.Url = url
    End Sub

    Public Sub Log(ByVal as_log As String)
        Dim strPath As String = Application.StartupPath & "\Logs\System"
        AutoDirectory(strPath)
        Call Log_Ex(as_log, strPath & "\" & Replace(CStr(Now.ToShortDateString), "/", "-") + ediname + ".log")
    End Sub
    Public Sub AutoDirectory(ByVal dr As String)
        Try
            'Dim dir As Directory
            If Directory.Exists(dr) = False Then
                Directory.CreateDirectory(dr)
            Else
            End If
        Catch ex As Exception

        End Try

    End Sub
    Public Function toBranch(ByVal json As String) As Branch
        Return JsonConvert.DeserializeObject(json, GetType(Branch))
    End Function
    Public Function toResponse(ByVal json As String) As Response
        Return JsonConvert.DeserializeObject(json, GetType(Response))
    End Function
    Public Function toUser(ByVal json As String) As User
        Return JsonConvert.DeserializeObject(json, GetType(User))
    End Function


    Public Sub Log_Ex(ByVal as_log As String, ByVal as_filename As String)
        Try

            Dim objFile As FileStream
            Dim objWriter As System.IO.StreamWriter


            If System.IO.File.Exists(as_filename) Then
                objFile = New FileStream(as_filename, FileMode.Append, FileAccess.Write, FileShare.Write)
            Else
                objFile = New FileStream(as_filename, FileMode.Create, FileAccess.Write, FileShare.Write)
            End If

            objWriter = New System.IO.StreamWriter(objFile)

            objWriter.BaseStream.Seek(0, SeekOrigin.End)
            objWriter.WriteLine(as_log)
            objWriter.Close()
        Catch ex As Exception

        End Try


    End Sub
    Public Sub LogError(ByVal as_log As String)
        Dim strPath As String = Application.StartupPath & "\Logs\Error"
        AutoDirectory(strPath)
        Call Log_Ex(as_log, strPath & "\" & Replace(CStr(Now.ToShortDateString), "/", "-") + ".log")
    End Sub
End Module
