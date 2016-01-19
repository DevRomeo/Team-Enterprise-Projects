Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic
Imports System.Threading.Timer
Module Common


    Public ps_MailAdmin As String
    Public ps_MailServer As String
    Public ps_MailReceipient As String
    Public ps_MailUsername As String
    Public ps_MailPassword As String
    Public reportType As String
    Public strMessage As String
    Public strBoxMessage As String
    Public strError As String
    Public isFinished As Boolean

    Public ps_DBUsername As String
    Public ps_DBPassword As String
    Public ps_DBName As String
    Public ps_DBServer As String

    Public ps_DBUsernameLocal As String
    Public ps_DBPasswordLocal As String
    Public ps_DBNameLocal As String
    Public ps_DBServerLocal As String

    Public isResend As Boolean = False

    Public ps_DBUsernameV As String
    Public ps_DBPasswordV As String
    Public ps_DBNameV As String
    Public ps_DBServerV As String

    Public ps_time As String
    Public ps_date As String
    Public ps_receipient As String
    Public ps_receipients As String
    Public ls_connectionStringLuzon As String
    Public ls_connectionStringVismin As String
    Public ls_connectionStringLocal As String '= "user id=sa;password=ml;data source=(local);persist security info=False;initial catalog=EmailScheduler"
    Public reportId As Integer

    Public ps_reportName As String
    Public ps_ReportPath As String = Application.StartupPath & "\Reports\"

    Public ps_Resend As String
    Public isType As String

    Public ps_HeadOffice As String
    Public ps_Region As String
    Public ps_Area As String
    Public rptNum As Integer
    Public notLink As Boolean = False
    Public isvismin As Boolean = False
    Public sending As Boolean = False

    Public ps_Mode As String = "0"
    Public isupdate As Boolean = False
    ''''---Adding of Recipients------
    Public Id, reportName, spec, classification, class_01, paramDate, paramClass_01, paramClass_02, paramClass_03, paramClass_04, paramBranchCode As String
    Public strId, strreportName, strclassification, strparamDate, strParamdateDesc, strparamClass_01, strparamClass_02, strparamClass_03, strparamClass_04, strparamBranchCode As String
    Public isParamOk As Boolean

    'report counter
    Public report_counter As Integer

    Public ps_type As String = "0"

    Public Const csSchedulerIni = "Scheduler.ini"
    Public ps_ini As String = Application.StartupPath & "\" & csSchedulerIni

    Public splash As New clsSplash

    Public ds_Class_01, ds_Class_02, ds_Class_03, ds_Class_04 As DataSet


    Public objDBLuzon As New clsDBConnection
    Public objDBVismin As New clsDBConnection
    Public objDBLocal As New clsDBConnection

    Public Declare Ansi Function GetPrivateProfileString _
        Lib "kernel32.dll" Alias "GetPrivateProfileStringA" _
        (ByVal lpApplicationName As String, _
        ByVal lpKeyName As String, ByVal lpDefault As String, _
        ByVal lpReturnedString As System.Text.StringBuilder, _
        ByVal nSize As Integer, ByVal lpFileName As String) _
        As Integer
    ''Auto Close message BOX
    Private Const BM_CLICK As Integer = &HF5
    Private Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal parent As IntPtr, ByVal child As IntPtr, ByVal className As String, ByVal caption As String) As IntPtr
    Public Declare Unicode Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32

  

    Public Declare Ansi Function WritePrivateProfileString _
        Lib "kernel32.dll" Alias "WritePrivateProfileStringA" _
        (ByVal lpApplicationName As String, _
        ByVal lpKeyName As String, ByVal lpString As String, _
        ByVal lpFileName As String) As Integer
    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Public flag As Integer = 0
    Public ctrReport As Integer = 0
    Public asReport, email As String
    Public chkResend As Boolean

    Public sendReport As String
    Public sendRep As String
    Public s_flag As Integer = 0
    Public b_name As String
    Public t_flag As Integer = 0
    Public timer As Integer = 0
    Public ctrSend As Integer = 0
    Public chkTimer As Boolean = True
    Public flagHO As Integer = 0
    Public genThread As Thread
    Public WithEvents tmr As New System.Windows.Forms.Timer
    'for pop-up module
    Public y As Integer = 0
    Public x As Integer = 0
    'for displaying message
    Public msg, e_msg As String
    Public Sub Delay5(ByVal iDelay As Integer)
        Dim dt1 As DateTime = now
        Do
            If Now > dt1.AddSeconds(iDelay) Then Exit Do
        Loop
    End Sub

    Private Sub tmr_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmr.Tick
        tmr.Stop()
        Dim dialog As IntPtr = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "#32770", String.Empty)
        If Not dialog.Equals(IntPtr.Zero) Then
            Dim btnOK As IntPtr = FindWindowEx(dialog, IntPtr.Zero, "Button", "OK")
            If Not btnOK.Equals(IntPtr.Zero) Then
                SendMessage(btnOK, BM_CLICK, 0, 0)
            End If
        End If
        tmr.Start()
    End Sub

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

    Public Sub Log(ByVal as_log As String)
        Dim strPath As String = Application.StartupPath & "\Logs\System"
        AutoDirectory(strPath)
        Call Log_Ex(as_log, strPath & "\" & Replace(CStr(Now.ToShortDateString), "/", "-") + ".log")
    End Sub

    Public Sub Main()

        'Dim li_ret As Integer
        'Dim ls_startup As String
        'Dim ls_buff As New System.Text.StringBuilder(255)
        'Dim frmStartup As Object

        'li_ret = GetPrivateProfileString("EMAIL SCHEDULER", "STARTUP", "0", ls_buff, 256, ps_ini)
        'ls_startup = ls_buff.ToString
        'ls_startup = ls_startup.Substring(0, li_ret)

        'If ls_startup = "0" Then
        '    frmStartup = New frmSetup
        'Else
        '    frmStartup = New frmScheduler
        'End If

        'frmStartup.showdialog()


    End Sub

    Public Function checkFile(ByVal dir As String) As Boolean
        Dim isFound As Boolean = False
        Dim d As File
        If d.Exists(dir) And chkResend = False Then
            isFound = True
            ctrReport += 1
        End If
        Return isFound
    End Function
    Public Function ReadLog(ByVal file As String, ByVal setDate As Date) As Integer
        'Dim FR As New StreamReader(file)
        'Dim dline As String
        'Dim logDate As String = ""
        'Dim objSplash As New clsSplash
        'frmLog.rboxlog.Clear()
        'dline = FR.ReadLine()
        'objSplash.ShowSplash("Reading data from the Source....")

        'While Not dline Is Nothing
        '    If dline.StartsWith("ERROR LOG:") Then
        '        logDate = dline.Substring(11, 9)
        '    End If
        '    If dline.StartsWith("LOG") Then
        '        logDate = dline.Substring(5, 9)
        '    End If
        '    If CDate(logDate) = setDate Then
        '        frmLog.rboxlog.AppendText(vbNewLine + dline + vbNewLine)
        '    End If

        '    dline = FR.ReadLine()
        'End While
        'frmLog.rboxlog.WordWrap = False
        'frmLog.lblDate.Text = Now.ToShortDateString
        'frmLog.rboxlog.Refresh()
        'FR.Close()
        'objSplash.CloseSplash()
        'frmLog.Show()
        'frmLog.TopMost = True

    End Function
    Public Function checkSend(ByVal sReport As String, ByVal sRec As String) As Boolean
        Dim sendArray() As String
        Dim recArray() As String
        Dim report As String
        Dim rec As String
        Dim ctrReport As Integer
        Dim isSend As Boolean = False
        Dim SendToRep As String
        Dim sendToRec

        report = sendReport
        rec = sendRep
        Try
            If Mid(report, report.Length) = "," And Mid(rec, rec.Length) = "," Then
                report = report.Substring(0, report.Length - 1)
                rec = rec.Substring(0, rec.Length - 1)
                sendArray = Split(report, ",")
                recArray = Split(rec, ",")
                For ctrReport = 0 To (sendArray.Length - 1)
                    sendRep = sendArray(ctrReport)
                    sendToRec = recArray(ctrReport)
                    If (sendRep = sReport) And (sendToRec = sRec) Then
                        Call Log(sReport + "already sent...")
                        isSend = True
                    End If

                Next
            End If
        Catch ex As Exception
            isSend = False
        End Try
        Return isSend
    End Function
    Public Sub btnColor(ByVal source As Button)
        source.BackColor = Color.AliceBlue
    End Sub
    Public Sub btnRefresh(ByVal source As Button)
        source.BackColor = Color.Gainsboro
    End Sub
    Public Sub AutoDirectory(ByVal dr As String)
        Try
            Dim dir As Directory
            If dir.Exists(dr) = False Then
                dir.CreateDirectory(dr)
            Else
            End If
        Catch ex As Exception

        End Try

    End Sub
End Module
