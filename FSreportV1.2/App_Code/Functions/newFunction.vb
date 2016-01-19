Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Web.Services
Imports System.Web.Services.Protocols

Public Class newFunction
    Public Function GetData(ByVal zone As String, ByRef var As myData) As GetServerInfo

        Dim response As New GetServerInfo
        If UCase(zone).Contains("LUZON") Then
            response.serverName = var.sy_Server
            response.dbname = var.sy_Name
            response.username = var.sy_Uname
            response.password = var.sy_Pass
            response.ZName = "Luzon/NCR"
        Else
            response.serverName = var.sy_Server1
            response.dbname = var.sy_Name1
            response.username = var.sy_Uname1
            response.password = var.sy_Pass1
            response.ZName = "Visayas/Mindanao"
        End If

        Return response
    End Function
    'Public Function GeneratePDF(ByVal dt As String) As String
    '    Dim resonse As New Response
    '    Dim forgen As New ForGenerating
    '    Dim cn As ConnectionFileServer = New ConnectionFileServer()
    '    GeneratePDF = Nothing
    '    Dim newDir As String() = Nothing
    '    cn.MapFileServerfunct()
    '    If CreateDirectory(cn.directory, cn.mapserver, cn.mappass, cn.mapuser) = False Then
    '        resonse.respcode = 0
    '        Return resonse.msg = "Unable to connect in file server"
    '    End If
    '    Dim str As String = cn.mapserver & "\" & cn.directory
    '    newDir = CheckServerDirectory(cn.mapserver, cn.mappass, cn.mapuser)
    '    If newDir.Contains(str) Then
    '        resonse.respcode = 1
    '        Return forgen.dlfromWeb(dt, str)
    '    Else
    '        resonse.respcode = 0
    '        Return resonse.msg = "Folder does not exist"
    '    End If
    'End Function

    Public Function CreateDirectory(ByVal AccId As String, ByVal mapserver As String, ByVal mappassword As String, ByVal mapusername As String) As Boolean
        CreateDirectory = False

        Dim nr As New NETRESOURCE

        nr.dwType = RESOURCETYPE_DISK
        nr.lpRemoteName = mapserver

        If WNetAddConnection2(nr, mappassword, mapusername, 0) <> NO_ERROR Then
            Throw New Exception("WNetAddConnection2 failed.")
        End If
        'Code to use connection here.'
        If WNetCancelConnection2(mapserver, 0, True) <> NO_ERROR Then
            Throw New Exception("WNetCancelConnection2 failed.")
        End If

        Dim dir As String = mapserver & "\" & AccId

        If Not System.IO.Directory.Exists(dir) Then
            System.IO.Directory.CreateDirectory(dir)
        End If

        If System.IO.Directory.Exists(dir) Then
            CreateDirectory = True
        End If

    End Function
    '<WebMethod()> _
    Public Function CheckServerDirectory(ByVal mapserver As String, ByVal mappassword As String, ByVal mapusername As String) As String()

        CheckServerDirectory = Nothing
        ' ReadINI()

        Dim nr As New NETRESOURCE

        nr.dwType = RESOURCETYPE_DISK
        nr.lpRemoteName = mapserver

        If WNetAddConnection2(nr, mappassword, mapusername, 0) <> NO_ERROR Then
            Throw New Exception("WNetAddConnection2 failed.")
        End If
        'Code to use connection here.'
        If WNetCancelConnection2(mapserver, 0, True) <> NO_ERROR Then
            Throw New Exception("WNetCancelConnection2 failed.")
        End If


        If System.IO.Directory.Exists(mapserver) Then
            CheckServerDirectory = System.IO.Directory.GetDirectories(mapserver)
        End If
    End Function

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure NETRESOURCE
        Public dwScope As UInteger
        Public dwType As UInteger
        Public dwDisplayType As UInteger
        Public dwUsage As UInteger
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpLocalName As String
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpRemoteName As String
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpComment As String
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public lpProvider As String
    End Structure

    Private Const NO_ERROR As UInteger = 0
    Private Const RESOURCETYPE_DISK As UInteger = 1

    <DllImport("mpr.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function WNetAddConnection2(ByRef lpNetResource As NETRESOURCE, <[In](), MarshalAs(UnmanagedType.LPTStr)> ByVal lpPassword As String, <[In](), MarshalAs(UnmanagedType.LPTStr)> ByVal lpUserName As String, ByVal dwFlags As UInteger) As UInteger
    End Function

    <DllImport("mpr.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function WNetCancelConnection2(<[In](), MarshalAs(UnmanagedType.LPTStr)> ByVal lpName As String, ByVal dwFlags As UInteger, <MarshalAs(UnmanagedType.Bool)> ByVal fForce As Boolean) As UInteger
    End Function

End Class
