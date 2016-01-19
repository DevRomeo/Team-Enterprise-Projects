Imports System.IO
Imports System.Data
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient
Imports System.Drawing.Printing


Partial Class main
    Inherits System.Web.UI.Page
    Dim var As New myData
    Dim user_class As String
    Dim oRpt As ReportDocument
    Dim forgen As ForGenerating




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            mouseover()
            'GetPrinters()
            var = Me.Session("var")
            'AddingITem(var)

            If var.user_Fullname Is Nothing Then
                Response.Write("<script language=javascript>")
                Response.Write("alert('" & "Your session expired. Please log-in again." & "')")
                Response.Write("</script>")
                Me.Session("var") = Nothing
                Response.Write("<script language=javascript>")
                Response.Write("window.location = 'Login.aspx'")
                Response.Write("</script>")
            End If

            If var.user_Fullname = "" Then
                Response.Write("<script language=javascript>")
                Response.Write("alert('" & "You must be logged-in first." & "')")
                Response.Write("</script>")
                Me.Session("var") = Nothing
                Response.Write("<script language=javascript>")
                Response.Write("window.location = 'Login.aspx'")
                Response.Write("</script>")
            End If

            If Not IsPostBack Then
                Me.TextBox1.Text = DateAdd(DateInterval.Day, -1, Now.Date)
                ''DateAdd(IntervalType, Months, SecondDate)
                'var.zcode = Me.DropDownList1.Text
                'GenerateReport()
                forgen = New ForGenerating()
                Dim x = forgen.allFolders()
                If x.Count > 0 Then
                    DrpReportType.DataSource = x
                    DrpReportType.DataBind()
                    Dim x1 = forgen.allFiles(DrpReportType.Text, TextBox1.Text)
                    'Dim x1 = forgen.allFiles("ExecReport")
                    If x1.Count > 0 Then
                        GridView1.DataSource = x1
                        GridView1.DataBind()
                    Else
                        lblNodata.Text = "NO DATA FOUND"
                    End If
                Else
                    lblNodata.Text = "NO DATA FOUND"
                End If

            Else
                'Me.CrystalReportViewer1.ReportSource = Me.Session("oRpt")
                'Me.CrystalReportViewer1.Visible = True
            End If


        Catch ex As Exception
            Response.Write("<script language=javascript>")
            Response.Write("alert('" & "Your session expired. Please log-in again." & "')")
            Response.Write("</script>")
            Me.Session("var") = Nothing
            Response.Write("<script language=javascript>")
            Response.Write("window.location = 'Login.aspx'")
            Response.Write("</script>")
        End Try
        Me.Session.Add("curPage", "main")

    End Sub
    Private Sub Execution(ByVal rptType As String, ByVal dt As String)
        Dim xdt As String = Convert.ToDateTime(dt).ToString("M-d-yyyy")
        forgen = New ForGenerating()
        Dim x1 = forgen.allFilesMonthly(rptType, xdt)
        'Dim x1 = forgen.allFiles("ExecReport")
        If x1.Count > 0 Then
            GridView1.DataSource = x1
            GridView1.DataBind()
        Else
            lblNodata.Text = "NO DATA FOUND"
        End If
    End Sub

    Private Sub mouseover()
        ' Me.btnGo.Attributes.Add("onmouseover", "this.src='image/forward active2.png'")
        ' Me.btnGo.Attributes.Add("onmouseout", "this.src='image/forward2.png'")
        'Me.btnPrint.Attributes.Add("onmouseover", "this.src='image/printer_blue.png'")
        'Me.btnPrint.Attributes.Add("onmouseout", "this.src='image/printer_gray.png'")
    End Sub
    Private Sub storeFolder()

    End Sub

    'Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGo.Click
    '    ' var.zcode = Me.DropDownList1.Text
    '    'GenerateReport()
    'End Sub
    'Private Sub AddingITem(ByRef var As myData)
    '    If var.zcode = "VISMIN" Then
    '        drpZone.Items.Add("VISAYAS")
    '        drpZone.Items.Add("MINDANAO")
    '    ElseIf var.zcode = "LUZON" Then
    '        drpZone.Items.Add("LUZON")
    '        drpZone.Enabled = False
    '    End If
    'End Sub

    'Private Sub GenerateReport()
    '    If DropDownList1.Text <> "Luzon/NCR" And TextBox1.Text <> "" Then
    '        oRpt = Nothing
    '        oRpt = New ReportDocument
    '        Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions

    '        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
    '        Dim crlogin As New TableLogOnInfo
    '        Dim crConnectionInfo As New ConnectionInfo
    '        Dim report As String
    '        Dim oCOM As New SqlCommand
    '        Dim ServerName, DatabaseName, Password, UserID As String
    '        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
    '        Dim variable As New newFunction
    '        Dim res As GetServerInfo
    '        Dim varZNAME As String

    '        res = variable.GetData(var.zcode, var)

    '        ServerName = res.serverName
    '        DatabaseName = res.dbname
    '        Password = res.password
    '        UserID = res.username
    '        varZNAME = res.ZName
    '        Try
    '            If Not IsDate(Me.TextBox1.Text) Then
    '                ' Me.CrystalReportViewer1.Visible = False
    '                Me.lblError.Visible = True
    '                Me.lblError.Text = Me.TextBox1.Text & " is not a valid date."
    '                Exit Sub
    '            Else
    '                'Me.CrystalReportViewer1.Visible = True
    '                Me.lblError.Visible = False
    '            End If

    '            report = AppDomain.CurrentDomain.BaseDirectory + "HO_DailyExecutiveSummary.rpt"



    '            oRpt.Load(report, OpenReportMethod.OpenReportByTempCopy)
    '            For Each crTable In oRpt.Database.Tables
    '                crlogin = crTable.LogOnInfo
    '                crlogin = New TableLogOnInfo
    '                crlogin.ConnectionInfo.ServerName = ServerName
    '                crlogin.ConnectionInfo.DatabaseName = DatabaseName
    '                crlogin.ConnectionInfo.Password = Password
    '                crlogin.ConnectionInfo.UserID = UserID
    '                crTable.ApplyLogOnInfo(crlogin)
    '                crTable.Location = DatabaseName + ".dbo." + crTable.Name
    '            Next

    '            oRpt.SetParameterValue("@ldt_headoffice", varZNAME)
    '            oRpt.SetParameterValue("@ldt_rptDate", Me.TextBox1.Text)
    '            'Me.CrystalReportViewer1.ReportSource = oRpt
    '            'Me.CrystalReportViewer1.Visible = True
    '            Me.Session.Add("orpt", oRpt)

    '            oRpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "ExecR19_" & Replace(Me.TextBox1.Text, "/", "-") & ".pdf")


    '        Catch ex As Exception
    '            'Response.Write(ex.ToString)
    '        End Try

    '    ElseIf DropDownList1.Text = "Luzon/NCR" And TextBox1.Text <> "" Then
    '        forgen = New ForGenerating
    '        Dim nf As New newFunction
    '        Dim dt As String = Convert.ToDateTime(TextBox1.Text).ToString("M-d-yyyy")


    '        lblError.Visible = True

    '        lblError.Text = forgen.downloadPDF(dt, DropDownList1.Text)

    '    ElseIf TextBox1.Text = "" Then

    '        lblError.Visible = True
    '        lblError.Text = "Date is required"
    '    End If
    'End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrint.Click
    '    oRpt = Me.Session("orpt")
    '    If oRpt Is Nothing Then
    '        Exit Sub
    '    End If
    '    oRpt.PrintOptions.PrinterName = Me.cboPrinter.SelectedValue
    '    oRpt.PrintToPrinter(1, False, 1, 4)
    'End Sub
    'Private Sub GetPrinters()
    '    Dim fwPrintSetting As System.Drawing.Printing.PrinterSettings
    '    Dim nCnt As Integer

    '    fwPrintSetting = New System.Drawing.Printing.PrinterSettings
    '    cboPrinter.Items.Clear()
    '    With cboPrinter.Items
    '        For nCnt = 0 To (PrinterSettings.InstalledPrinters.Count - 1)
    '            .Add(PrinterSettings.InstalledPrinters.Item(nCnt))
    '            fwPrintSetting.PrinterName = PrinterSettings.InstalledPrinters.Item(nCnt)
    '        Next
    '    End With

    'End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        lblNodata.Text = ""

        GridView1.DataSource = Nothing
        GridView1.DataBind()

        If CheckBox1.Checked = False Then
            Execution(DrpReportType.Text, TextBox1.Text)
        Else
            forgen = New ForGenerating()
            Dim x = forgen.allFilesInFolder()
            If x.Count > 0 Then
                GridView1.DataSource = x
                GridView1.DataBind()
            Else
           
                lblNodata.Text = "NO DATA FOUND"
            End If
        End If
    End Sub

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            DrpReportType.Enabled = False
            TextBox1.Enabled = False
        Else
            DrpReportType.Enabled = True
            TextBox1.Enabled = True
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        forgen = New ForGenerating()
        Dim a As Integer = GridView1.SelectedIndex
        Dim fname As String = GridView1.Rows(a).Cells(0).Text
        If CheckBox1.Checked = False Then
            lblNodata.Text = forgen.downloadPDF(DrpReportType.Text, fname)
        Else
            forgen.downloadFileAllFolder(fname)
        End If

    End Sub
End Class
