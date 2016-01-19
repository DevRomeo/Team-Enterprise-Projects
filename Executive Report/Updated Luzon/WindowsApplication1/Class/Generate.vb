Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports SendToMail_DLL
Public Class Generate
    Public eadd, cc, clas, spec, id, vClass_01, vClass_02, vClass_03, vClass_04, vbcode, rptName, rptTitle As String
    Public server, db, uname, pass, pdate, pClass_01, pClass_02, pClass_03, pClass_04, pbcode, exportAs As String
    Public layout, sizes As Integer
    Public vdate As String
    Public Sub GenerateReport()


        report_counter = report_counter + 1

        Dim connect_db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader

        If connect_db.isConnected Then
            connect_db.CloseConnection()
        End If

        connect_db.ConnectDB(ls_connectionStringLocal)

        Dim sql As String = "Select distinct reportname,reportTitle,layout,sizes,server,db,uname,pass,paramdate,paramClass_01,paramClass_02,paramClass_03,paramClass_04,paramBranchCode,exportAs from tbl_Reports_V3 where ID='" & id & "'"

        dr = connect_db.Execute_SQL_DataReader(sql)

        Try

            While dr.Read
                rptName = dr.Item("reportname")
                rptTitle = Replace(dr.Item("reportTitle"), ":", "")
                layout = CInt(dr.Item("layout"))
                server = dr.Item("server")
                db = dr.Item("db")
                uname = dr.Item("uname")
                pass = dr.Item("pass")
                pdate = dr.Item("paramdate")
                pClass_01 = dr.Item("paramClass_01")
                pClass_02 = dr.Item("paramClass_02")
                pClass_03 = dr.Item("paramClass_03")
                pClass_04 = dr.Item("paramClass_04")
                pbcode = dr.Item("paramBranchCode")
                sizes = dr.Item("sizes")
                exportAs = dr.Item("exportAs")
            End While
            dr.Close()
        Catch ex As Exception
            Call LogError(Now.ToString + ":" + ex.Message)
            connect_db.CloseConnection()
            Exit Sub
        End Try
        connect_db.CloseConnection()


        Dim reportPath As String
        If UCase(vClass_01.Trim) = "LUZON" Then
            reportPath = Application.StartupPath & "\Reports\LUZON\" & rptName.Trim
        Else
            reportPath = Application.StartupPath & "\Reports\VISMIN\" & rptName.Trim
        End If
        Dim ExportPath As String
        If vClass_01.Trim <> "" Then
            ExportPath = Application.StartupPath + "\Export\" + clas.Trim + "\" + Replace(vClass_01.Trim, "/", "-") + "\" + spec.Trim + "\" + CStr(Replace(Now.ToShortDateString, "/", "-")) + "\"
        Else
            ExportPath = Application.StartupPath + "\Export\" + clas.Trim + "\" + spec.Trim + "\" + CStr(Replace(Now.ToShortDateString, "/", "-")) + "\"
        End If

        AutoDirectory(ExportPath)

        Dim ls_exportfile As String
        Dim ls_attachment As String
        Dim ls_filename As String
        Dim ls_subject As String
        Dim ls_date As String

        'email address,servr,db,uname,password
        Dim ls_recipient As String = eadd.Trim
        Dim ls_CC As String = cc.Trim
        Dim report_server As String = server.Trim
        Dim report_db As String = db.Trim
        Dim report_uname As String = uname.Trim
        Dim report_password As String = pass.Trim

        ' report's parameters
        Dim report_paramdate As String = pdate.Trim
        Dim report_paramClass_01 As String = pClass_01.Trim
        Dim report_paramClass_02 As String = pClass_02.Trim
        Dim report_paramClass_03 As String = pClass_03.Trim
        Dim report_paramClass_04 As String = pClass_04.Trim
        Dim report_parambcode As String = pbcode.Trim

        ' report's values
        Dim report_valueClass_01 As String = vClass_01.Trim
        Dim report_valueClass_02 As String = vClass_02.Trim
        Dim report_valueClass_03 As String = vClass_03.Trim
        Dim report_valueClass_04 As String = vClass_04.Trim
        Dim report_valuebcode As String = vbcode.Trim

        Dim rptDocument As New ReportDocument
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crLogin As CrystalDecisions.Shared.TableLogOnInfo
        Dim objDiskOpt As New DiskFileDestinationOptions
        Dim objExOpt As ExportOptions
        Dim objOrientation As PaperOrientation = CInt(layout)
        Dim errorExport As Boolean = True

        Try

            ls_date = vdate.Trim
            Dim extenFile As String

            If LCase(exportAs.Trim) = "pdf" Then
                extenFile = ".pdf"
            Else
                extenFile = ".xls"
            End If

            'export file name
            If report_parambcode <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vbcode.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_04 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_04.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_03 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_03.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_02 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_02.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_01 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_01.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            Else
                ls_exportfile = rptTitle.Trim + " " + Replace(ls_date, "/", "-") + extenFile
            End If

            ls_subject = ls_exportfile
            msg = Now.ToString & " Generating " & ls_exportfile
            ls_filename = ExportPath + ls_subject
            ls_attachment = ls_filename

            If checkFile(ls_attachment) = False Then
                rptDocument.Load(reportPath, OpenReportMethod.OpenReportByTempCopy)
                For Each crTable In rptDocument.Database.Tables
                    crLogin = crTable.LogOnInfo
                    crLogin = New TableLogOnInfo
                    crLogin.ConnectionInfo.ServerName = report_server
                    crLogin.ConnectionInfo.DatabaseName = report_db
                    crLogin.ConnectionInfo.Password = report_password
                    crLogin.ConnectionInfo.UserID = report_uname
                    crTable.ApplyLogOnInfo(crLogin)
                    crTable.Location = report_db + ".dbo." + crTable.Name
                Next

                If sizes = 2 Then
                    rptDocument.PrintOptions.PaperSize = PaperSize.PaperLegal
                Else
                    rptDocument.PrintOptions.PaperSize = PaperSize.PaperLetter
                End If

                rptDocument.PrintOptions.PaperOrientation = objOrientation


                'assigns value to parameters
                If report_paramdate <> "" Then
                    rptDocument.SetParameterValue(report_paramdate, ls_date)
                End If

                If report_paramClass_01 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_01, IIf(report_valueClass_01 = "Luzon", "Luzon/NCR", report_valueClass_01))
                End If
                If report_paramClass_03 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_03, report_valueClass_03)
                End If
                If report_paramClass_04 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_04, report_valueClass_04)
                End If

                If report_parambcode <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_04, report_valuebcode)
                End If

                If report_paramClass_02 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_01, report_valueClass_02)
                End If

                objDiskOpt.DiskFileName = ls_filename
                objExOpt = rptDocument.ExportOptions
                objExOpt.ExportDestinationType = ExportDestinationType.DiskFile

                'epxort option (pdf/excel)
                If LCase(exportAs.Trim) = "pdf" Or exportAs.Trim = "" Then
                    objExOpt.ExportFormatType = ExportFormatType.PortableDocFormat
                Else
                    objExOpt.ExportFormatType = ExportFormatType.Excel
                End If

                objExOpt.DestinationOptions = objDiskOpt
                rptDocument.Export()

                rptDocument.Close()
                rptDocument.Dispose()
            End If

            Dim send As New SendToEmail
            If send.send(ps_MailAdmin, ls_recipient, ls_CC, ls_subject, "", ls_attachment, ps_MailServer, ps_MailUsername, ps_MailPassword) Then
                Call Log(Now.ToString + ":" + "Successful Sending : " + ls_subject + " to " + ls_recipient + vbCrLf)
                msg = Now.ToString & " : " & ls_exportfile & " is successfully sent to " & ls_recipient
            End If

        Catch ex As Exception
            rptDocument.Close()
            rptDocument.Dispose()

            e_msg = Now.ToString & " : " & ls_exportfile & " is UNSUCCESSFULY sent to " & ls_recipient
            Call LogError(Now.ToString + ":" + ls_subject + vbNewLine + ex.ToString)
            Call SaveUnsentReport(ls_recipient, clas.Trim, spec.Trim, id.Trim, CDate(ls_date), vClass_01.Trim, vClass_02.Trim, vClass_03.Trim, vClass_04.Trim, vbcode.Trim)

        End Try

        report_counter = report_counter - 1
        Delay5(2)

    End Sub
    Public Sub SaveUnsentReport(ByVal eaddress As String, ByVal clas As String, ByVal spec As String, ByVal id As String, ByVal vdate As Date, ByVal vClass_01 As String, ByVal vClass_02 As String, ByVal vClass_03 As String, ByVal vClass_04 As String, ByVal vbcode As String)
        Dim sql As String = "INSERT INTO tbl_UnsentReports_V3(EmailAddress,Classification,specific,ReportId,ValueParamDate,ValueParamClass_01,ValueParamClass_02,ValueParamClass_03,ValueParamClass_04,ValueParamBranchCode)" & _
        "VALUES('" & eaddress & "','" & clas & "','" & spec & "','" & id & "','" & vdate & "','" & vClass_01 & "','" & vClass_02 & "','" & vClass_03 & "','" & vClass_04 & "','" & vbcode & "')"

        Dim mydb As New clsDBConnection

        If mydb.isConnected Then
            mydb.CloseConnection()
        End If

        mydb.ConnectDB(ls_connectionStringLocal)

        If mydb.Execute_SQLQuery(sql) < 1 Then
            Call LogError(Now.ToString + ":" + "INSERT ERROR:" + sql)
        End If
        mydb.RollbackTransaction()
        mydb.CloseConnection()
    End Sub
    Public Sub RegenerateReport()
        report_counter = report_counter + 1

        Dim connect_db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader

        If connect_db.isConnected Then
            connect_db.CloseConnection()
        End If

        connect_db.ConnectDB(ls_connectionStringLocal)

        Dim sql As String = "Select distinct reportname,reportTitle,layout,sizes,server,db,uname,pass,paramdate,paramClass_01,paramClass_02,paramClass_03,paramClass_04,paramBranchCode,ExportAs from tbl_Reports_V3 where ID='" & id & "'"

        dr = connect_db.Execute_SQL_DataReader(sql)

        Try

            While dr.Read
                rptName = dr.Item("reportname")
                rptTitle = Replace(dr.Item("reportTitle"), ":", "")
                layout = CInt(dr.Item("layout"))
                server = dr.Item("server")
                db = dr.Item("db")
                uname = dr.Item("uname")
                pass = dr.Item("pass")
                pdate = dr.Item("paramdate")
                pClass_01 = dr.Item("paramClass_01")
                pClass_02 = dr.Item("paramClass_02")
                pClass_03 = dr.Item("paramClass_03")
                pClass_04 = dr.Item("paramClass_04")
                pbcode = dr.Item("paramBranchCode")
                sizes = dr.Item("sizes")
                exportAs = dr.Item("ExportAs")
            End While
            dr.Close()
        Catch ex As Exception
            Call LogError(Now.ToString + ":" + ex.Message)
            report_counter = report_counter - 1
            Exit Sub
        End Try
        connect_db.CloseConnection()

        connect_db.ConnectDB(ls_connectionStringLocal)

        sql = "DELETE tbl_UnsentReports_V3 where ReportId='" & id.Trim & "' and EmailAddress='" & eadd.Trim & "' and valueParamClass_04 ='" & vClass_04.Trim & "'"

        If connect_db.Execute_SQLQuery(sql) < 1 Then
            Call LogError(Now.ToString + ":" + "DELETE ERROR:" + sql)
            connect_db.CloseConnection()
            Exit Sub
        End If
        connect_db.RollbackTransaction()
        connect_db.CloseConnection()

        Dim reportPath As String
        If UCase(vClass_01.Trim) = "LUZON" Then
            reportPath = Application.StartupPath & "\Reports\LUZON\" & rptName.Trim
        Else
            reportPath = Application.StartupPath & "\Reports\VISMIN\" & rptName.Trim
        End If
        Dim ExportPath As String
        ExportPath = Application.StartupPath + "\Export\" + clas.Trim + "\" + spec.Trim + "\" + CStr(Replace(Now.ToShortDateString, "/", "-")) + "\"

        AutoDirectory(ExportPath)

        Dim ls_exportfile As String
        Dim ls_attachment As String
        Dim ls_filename As String
        Dim ls_subject As String
        Dim ls_date As String



        'email address,servr,db,uname,password
        Dim ls_recipient As String = eadd.Trim
        Dim ls_CC As String = cc.Trim
        Dim report_server As String = server.Trim
        Dim report_db As String = db.Trim
        Dim report_uname As String = uname.Trim
        Dim report_password As String = pass.Trim

        ' report's parameters
        Dim report_paramdate As String = pdate.Trim
        Dim report_paramClass_01 As String = pClass_01.Trim
        Dim report_paramClass_02 As String = pClass_02.Trim
        Dim report_paramClass_03 As String = pClass_03.Trim
        Dim report_paramClass_04 As String = pClass_04.Trim
        Dim report_parambcode As String = pbcode.Trim

        ' report's values
        Dim report_valueClass_01 As String = vClass_01.Trim
        Dim report_valueClass_02 As String = vClass_02.Trim
        Dim report_valueClass_03 As String = vClass_03.Trim
        Dim report_valueClass_04 As String = vClass_04.Trim
        Dim report_valuebcode As String = vbcode.Trim

        Dim rptDocument As New ReportDocument
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crLogin As CrystalDecisions.Shared.TableLogOnInfo
        Dim objDiskOpt As New DiskFileDestinationOptions
        Dim objExOpt As ExportOptions
        
        Dim objOrientation As PaperOrientation = CInt(layout)
        Dim reportFile As File
        Dim errorExport As Boolean = True
      
        Try

            ls_date = vdate
            Dim extenFile As String

            If LCase(exportAs.Trim) = "pdf" Then
                extenFile = ".pdf"
            Else
                extenFile = ".xls"
            End If

            'export file name
            If report_parambcode <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vbcode.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_04 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_04.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_03 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_03.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_02 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_02.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_01 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_01.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            Else
                ls_exportfile = rptTitle.Trim + " " + Replace(ls_date, "/", "-") + extenFile
            End If

            ls_subject = ls_exportfile
            msg = Now.ToString & " Generating " & ls_exportfile
            ls_filename = ExportPath + ls_subject
            ls_attachment = ls_filename

            If checkFile(ls_attachment) = False Then
                rptDocument.Load(reportPath, OpenReportMethod.OpenReportByTempCopy)
                For Each crTable In rptDocument.Database.Tables
                    crLogin = crTable.LogOnInfo
                    crLogin = New TableLogOnInfo
                    crLogin.ConnectionInfo.ServerName = report_server
                    crLogin.ConnectionInfo.DatabaseName = report_db
                    crLogin.ConnectionInfo.Password = report_password
                    crLogin.ConnectionInfo.UserID = report_uname
                    crTable.ApplyLogOnInfo(crLogin)
                    crTable.Location = report_db + ".dbo." + crTable.Name
                Next

                If sizes = 2 Then
                    rptDocument.PrintOptions.PaperSize = PaperSize.PaperLegal
                Else
                    rptDocument.PrintOptions.PaperSize = PaperSize.PaperLetter
                End If

                rptDocument.PrintOptions.PaperOrientation = objOrientation


                'assigns value to parameters
                If report_paramdate <> "" Then
                    rptDocument.SetParameterValue(report_paramdate, ls_date)

                End If

                If report_paramClass_01 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_01, IIf(report_valueClass_01 = "Luzon", "Luzon/NCR", report_valueClass_01))
                End If
                If report_paramClass_03 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_03, report_valueClass_03)
                End If
                If report_paramClass_04 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_04, report_valueClass_04)
                End If

                If report_parambcode <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_04, report_valuebcode)
                End If

                If report_paramClass_02 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_01, report_valueClass_02)
                End If

                objDiskOpt.DiskFileName = ls_filename
                objExOpt = rptDocument.ExportOptions
                objExOpt.ExportDestinationType = ExportDestinationType.DiskFile

                'epxort option (pdf/excel)
                If LCase(exportAs.Trim) = "pdf" Or exportAs.Trim = "" Then
                    objExOpt.ExportFormatType = ExportFormatType.PortableDocFormat
                Else
                    objExOpt.ExportFormatType = ExportFormatType.Excel
                End If

                objExOpt.DestinationOptions = objDiskOpt
                rptDocument.Export()
                rptDocument.Close()
                rptDocument.Dispose()
            End If

            Dim send As New SendToEmail
            If send.send(ps_MailAdmin, ls_recipient, ls_CC, ls_subject, "", ls_attachment, ps_MailServer, ps_MailUsername, ps_MailPassword) Then
                Call Log(Now.ToString + ":" + "Successful Sending : " + ls_subject + " to " + ls_recipient + vbCrLf)
                msg = Now.ToString & " : " & ls_exportfile & " is successfully sent to " & ls_recipient
            End If

        Catch ex As Exception
            rptDocument.Close()
            rptDocument.Dispose()
            e_msg = Now.ToString & " : " & ls_exportfile & " is UNSUCCESSFULY sent to " & ls_recipient
            Call LogError(Now.ToString + ":REGENERATE:" + ls_subject + vbNewLine + ex.Message)
            Call SaveUnsentReport(ls_recipient, clas.Trim, spec.Trim, id.Trim, Now.Date, vClass_01.Trim, vClass_02.Trim, vClass_03.Trim, vClass_04.Trim, vbcode.Trim)
        End Try

        report_counter = report_counter - 1
        Delay5(2)

    End Sub
    Public Sub RegenerateReport1()
        report_counter = report_counter + 1

        Dim connect_db As New clsDBConnection
        Dim dr As SqlClient.SqlDataReader

        If connect_db.isConnected Then
            connect_db.CloseConnection()
        End If

        connect_db.ConnectDB(ls_connectionStringLocal)

        Dim sql As String = "Select distinct reportname,reportTitle,layout,sizes,server,db,uname,pass,paramdate,paramClass_01,paramClass_02,paramClass_03,paramClass_04,paramBranchCode,exportAs from tbl_Reports_V3 where ID='" & id & "'"

        dr = connect_db.Execute_SQL_DataReader(sql)

        Try

            While dr.Read
                rptName = dr.Item("reportname")
                rptTitle = Replace(dr.Item("reportTitle"), ":", "")
                layout = CInt(dr.Item("layout"))
                server = dr.Item("server")
                db = dr.Item("db")
                uname = dr.Item("uname")
                pass = dr.Item("pass")
                pdate = dr.Item("paramdate")
                pClass_01 = dr.Item("paramClass_01")
                pClass_02 = dr.Item("paramClass_02")
                pClass_03 = dr.Item("paramClass_03")
                pClass_04 = dr.Item("paramClass_04")
                pbcode = dr.Item("paramBranchCode")
                sizes = dr.Item("sizes")
                exportAs = dr.Item("exportAs")
            End While
            dr.Close()
        Catch ex As Exception
            Call LogError(Now.ToString + ":" + ex.Message)
            connect_db.CloseConnection()
            Exit Sub
        End Try
        connect_db.CloseConnection()

        'connect_db.ConnectDB(ls_connectionStringLocal)

        'sql = "DELETE tbl_UnsentReports where ReportId='" & id.Trim & "' and EmailAddress='" & eadd.Trim & "'"

        'If connect_db.Execute_SQLQuery(sql) < 1 Then
        '    Call LogError(Now.ToString + ":" + "DELETE ERROR:" + sql)
        '    Exit Sub
        'End If

        Dim reportPath As String
        If UCase(vClass_01.Trim) = "LUZON" Then
            reportPath = Application.StartupPath & "\Reports\LUZON\" & rptName.Trim
        Else
            reportPath = Application.StartupPath & "\Reports\VISMIN\" & rptName.Trim
        End If
        Dim ExportPath As String
        ExportPath = Application.StartupPath + "\Export\" + clas.Trim + "\" + spec.Trim + "\" '+ CStr(Replace(Now.ToShortDateString, "/", "-")) + "\"

        AutoDirectory(ExportPath)

        Dim ls_exportfile As String
        Dim ls_attachment As String
        Dim ls_filename As String
        Dim ls_subject As String
        Dim ls_date As String



        'email address,servr,db,uname,password
        Dim ls_recipient As String = eadd.Trim
        Dim ls_CC As String = cc.Trim
        Dim report_server As String = server.Trim
        Dim report_db As String = db.Trim
        Dim report_uname As String = uname.Trim
        Dim report_password As String = pass.Trim

        ' report's parameters
        Dim report_paramdate As String = pdate.Trim
        Dim report_paramClass_01 As String = pClass_01.Trim
        Dim report_paramClass_02 As String = pClass_02.Trim
        Dim report_paramClass_03 As String = pClass_03.Trim
        Dim report_paramClass_04 As String = pClass_04.Trim
        Dim report_parambcode As String = pbcode.Trim

        ' report's values
        Dim report_valueClass_01 As String = vClass_01.Trim
        Dim report_valueClass_02 As String = vClass_02.Trim
        Dim report_valueClass_03 As String = vClass_03.Trim
        Dim report_valueClass_04 As String = vClass_04.Trim
        Dim report_valuebcode As String = vbcode.Trim

        Dim rptDocument As ReportDocument
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crLogin As CrystalDecisions.Shared.TableLogOnInfo
        Dim objDiskOpt As New DiskFileDestinationOptions
        Dim objExOpt As ExportOptions
        Dim objOrientation As PaperOrientation = CInt(layout)
        Dim reportFile As File
        Dim errorExport As Boolean = True

        Try

            ls_date = vdate

            rptDocument = New ReportDocument

            Dim extenFile As String

            If LCase(exportAs.Trim) = "pdf" Then
                extenFile = ".pdf"
            Else
                extenFile = ".xls"
            End If

            'export file name
            If report_parambcode <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vbcode.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_04 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_04.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_03 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_03.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_02 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_02.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            ElseIf report_paramClass_01 <> "" Then
                ls_exportfile = rptTitle.Trim + " " + Replace(vClass_01.Trim, "/", "-") + " " + Replace(ls_date, "/", "-") + extenFile
            Else
                ls_exportfile = rptTitle.Trim + " " + Replace(ls_date, "/", "-") + extenFile
            End If

            ls_subject = ls_exportfile
            msg = Now.ToString & " Generating " & ls_exportfile
            ls_filename = ExportPath + ls_subject
            ls_attachment = ls_filename

            If checkFile(ls_attachment) = False Then
                rptDocument.Load(reportPath, OpenReportMethod.OpenReportByTempCopy)
                For Each crTable In rptDocument.Database.Tables
                    crLogin = crTable.LogOnInfo
                    crLogin = New TableLogOnInfo
                    crLogin.ConnectionInfo.ServerName = report_server
                    crLogin.ConnectionInfo.DatabaseName = report_db
                    crLogin.ConnectionInfo.Password = report_password
                    crLogin.ConnectionInfo.UserID = report_uname
                    crTable.ApplyLogOnInfo(crLogin)
                    crTable.Location = report_db + ".dbo." + crTable.Name
                Next
                If sizes = 2 Then
                    rptDocument.PrintOptions.PaperSize = PaperSize.PaperLegal
                Else
                    rptDocument.PrintOptions.PaperSize = PaperSize.PaperLetter
                End If
                rptDocument.PrintOptions.PaperOrientation = objOrientation


                'assigns value to parameters
                If report_paramdate <> "" Then
                    rptDocument.SetParameterValue(report_paramdate, CDate(ls_date))

                End If

                If report_paramClass_01 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_01, IIf(report_valueClass_01 = "Luzon", "Luzon/NCR", report_valueClass_01))
                End If
                If report_paramClass_03 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_03, report_valueClass_03)
                End If
                If report_paramClass_04 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_04, report_valueClass_04)
                End If

                If report_parambcode <> "" Then
                    rptDocument.SetParameterValue(report_parambcode, report_valuebcode)
                End If

                If report_paramClass_02 <> "" Then
                    rptDocument.SetParameterValue(report_paramClass_02, report_valueClass_02)
                End If

                objDiskOpt.DiskFileName = ls_filename
                objExOpt = rptDocument.ExportOptions
                objExOpt.ExportDestinationType = ExportDestinationType.DiskFile

                'epxort option (pdf/excel)
                If LCase(exportAs.Trim) = "pdf" Or exportAs.Trim = "" Then
                    objExOpt.ExportFormatType = ExportFormatType.PortableDocFormat
                Else
                    objExOpt.ExportFormatType = ExportFormatType.Excel
                End If

                objExOpt.DestinationOptions = objDiskOpt
                rptDocument.Export()
                rptDocument.Close()
                rptDocument.Dispose()

            End If

            Dim send As New SendToEmail
            If send.send(ps_MailAdmin, ls_recipient, ls_CC, ls_subject, "", ls_attachment, ps_MailServer, ps_MailUsername, ps_MailPassword) Then
                Call Log(Now.ToString + ":" + "Successful Sending : " + ls_subject + " to " + ls_recipient + vbCrLf)
                msg = Now.ToString & " : " & ls_exportfile & " is successfully sent to " & ls_recipient
            End If


        Catch ex As Exception
            rptDocument.Close()
            rptDocument.Dispose()
            'pop.show_popUp("Unuccessful Sending : " + ls_subject + " to " + ls_recipient, False)
            e_msg = Now.ToString & " : " & ls_exportfile & " is UNSUCCESSFULY sent to " & ls_recipient
            Call LogError(Now.ToString + ":" + ls_subject + vbNewLine + ex.Message)
            Call SaveUnsentReport(ls_recipient, clas.Trim, spec.Trim, id.Trim, Now.Date, vClass_01.Trim, vClass_02.Trim, vClass_03.Trim, vClass_04.Trim, vbcode.Trim)
        End Try

        report_counter = report_counter - 1
        Delay5(5)

    End Sub
End Class