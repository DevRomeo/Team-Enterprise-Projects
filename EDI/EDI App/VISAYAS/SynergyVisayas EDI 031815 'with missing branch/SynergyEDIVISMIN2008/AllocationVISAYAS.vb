Imports Microsoft.Win32
Imports Newtonsoft.Json
Imports EDIdataClass


Public Class AllocationVISAYAS

    Inherits System.Windows.Forms.Form
    Dim folder1 As String = "C:\SynergFolder1\"
    Dim missingfile As String = "SynergyEDILogs.txt"
    Dim f As New clsFunc
    Dim d As Date
    Dim g_month As New Date
    Dim sqltxt As String = Nothing
    Private g_Row As Integer = Nothing
    Private sqlmsg As String = Nothing
    Private g_errlog As String = Nothing
    Private g_col As Integer = Nothing
    Private g_setup As Boolean = Nothing
    Private sTrbc As String = Nothing
    Private g_errbc As String = Nothing
    Private errCtr As Integer = 0
    Dim datenow As DateTime = Now
    Dim db As New clsDBconnection
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
       Main.Show()
    End Sub

    Private Sub AllocationLuzon_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub
    Private Sub Allocation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Allocation-" + ediname + " " & gs_serverloc3 & " " & gs_version
        Dim yn As Integer = Now.Year
        cboSet.Text = "False"
        Call Me.Init_Combo()
        btnSave.Hide()
        picGif.Hide()
        'cboYear.Items.Add(yn)
        'cboYear.Items.Add(yn - 1)
        Format(Date.Now, "MM/dd/yyyy")
        grd.Hide()
        btnSetup.Enabled = False
        btnEnter.Enabled = False
        grp.Hide()
        Me.makefolder()
        Me.makefile()
        CenterToScreen()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click

        Dim ec As New clsExcelConnection
        Dim sheet() As String = Nothing

        grd.Clear()
        cboSheet.Items.Clear()
        'filter by Excel Worksheets
        ofdExcel.Filter = "Excel Worksheets|*.xls"
        ofdExcel.ShowDialog()

        txtLocation.Text = ofdExcel.FileName
        If Trim(txtLocation.Text) = "" Then Exit Sub
        If Not ec.Excel_Connection(txtLocation.Text) Then Exit Sub
        If ec.Error_Get_Sheets_Numbers(sheet) Then Exit Sub
        initialize_cbo_sheets(sheet)
        ec.dispose()
        ec = Nothing


    End Sub
    Private Sub initialize_cbo_sheets(ByVal sheet() As String)
        Dim li_sheet As Integer = Nothing
        For li_sheet = 0 To UBound(sheet) - 1
            cboSheet.Items.Add(sheet(li_sheet))
        Next
    End Sub
    Private Sub makefolder()
        If Not IO.Directory.Exists(folder1) Then
            IO.Directory.CreateDirectory(folder1)
        End If
    End Sub
    Private Sub makefile()
        If Not IO.File.Exists(folder1 & missingfile) Then
            IO.File.Create(folder1 & missingfile)
        End If
        Exit Sub
    End Sub
    Private Sub cboSheet_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSheet.SelectedIndexChanged
        Dim ec As New clsExcelConnection
        Dim ds As New DataSet
        Dim squery As String = "Select * from [" + cboSheet.Text + "$]"
        dg.Show()
        dg.ReadOnly = True
        If Not ec.Excel_Connection(txtLocation.Text) Then Exit Sub
        If ec.Error_SetDS(squery, ds) Then Exit Sub
        dg.DataSource = ds.Tables(0)
        g_Row = ds.Tables(0).Rows.Count
        g_col = ds.Tables(0).Columns.Count
        btnEnter.Enabled = True
    End Sub
    Private Sub Init_Combo()
        Dim d As DateTime = Now
        Dim n As Integer = Nothing
        cboPeriod.DropDownStyle = ComboBoxStyle.DropDownList
        cboYear.DropDownStyle = ComboBoxStyle.DropDownList
        f.Initialize_Combo(cboYear, callyear, Format(Now, "yyyy"))
        'NEW ELI CODE-------------/
        'f.Initialize_Combo(cboyear, 2009, Format(d.Now, "yyyy"))
        'DEVELOPMENT---------------
        f.Initialize_Combo(cboPeriod, 1, 12)
    End Sub
    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged
        g_month = Trim(cboYear.Text) & "-" & Trim(cboPeriod.Text) & "-1"
        lblMonth.BorderStyle = BorderStyle.Fixed3D
        lblMonth.Text = Format(g_month, "MMMM")
        txtdesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
    End Sub
    Private Sub btnEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnter.Click
        If txtLocation.Text = "" Then
            MsgBox("Browse File First", 16)
            Exit Sub
        End If
        MsgBox("You're about to save this data in " & lblMonth.Text & " " & cboYear.Text, 64, lblMonth.Text & " " & cboYear.Text)
        btnEnter.Enabled = False
        If f.Error_Nonumeric(txtStart) Then Exit Sub
        If CheckUnder() = False Then
            Exit Sub
        End If

        If Format(CDate(g_month), "yyyy-MM-dd") > Format(CDate(d.Now), "yyyy-MM-dd") Then
            MsgBox("Invalid date", MsgBoxStyle.Critical, "Incorrect Date")
            Exit Sub
        End If
        Call fillnull()
        If fillnullbcode() = False Then
            GoTo EXT
        End If
        Call GetNewSetup()
        If missingbranch() = True Then
            btnEnter.Enabled = True
            GoTo EXT
        End If
        If errorcostcenter() = True Then
            btnEnter.Enabled = True
            GoTo EXT
        End If
        If dualbranch() = True Then
            btnEnter.Enabled = True
            GoTo EXT
        End If
        btnSave.Show()
        g_setup = True
        btnEnter.Enabled = True
        btnSetup.Enabled = True
EXT:
    End Sub
    Private Function CheckUnder() As Boolean
        'Dim service As New EDIService.Service
        Dim response As Response

        If Val(txtStart.Text) > g_Row Then
            f.Emsg("Rows is up to " & g_Row & " only.", "out of range")
            CheckUnder = False
            Exit Function
        End If

        Dim n As Integer = Nothing

        For n = Val(txtStart.Text) - 2 To 0 Step -1

            If Not Trim(dg.Item(n, 0).ToString) = "" Then
                Dim g As String = dg.Item(n, 0)


                If g.Length <= 3 Then
                    response = JsonConvert.DeserializeObject(service.getBranch(g, edi), GetType(Response))
                    If response.responseCode = ResponseCode.OK Then
                        MessageBox.Show("Some datas have not included. Please Check Excel File ")
                        Return False
                    ElseIf response.responseCode = ResponseCode.Error Then
                        MessageBox.Show("Some datas have not included. Please Check Excel File ")
                        Return False
                    End If

                End If
            End If

        Next

        CheckUnder = True

    End Function
    Private Sub fillnull()
        Dim n As Integer = Nothing
        Dim i As Integer = Nothing
        Dim c As Integer = Nothing
        Dim l_ctr As Integer = 0
        Dim r As Integer = Nothing
        Dim s As String
        For r = Val(txtStart.Text) - 1 To g_Row - 1
            For c = 0 To g_col - 1
                s = dg.Item(r, c).ToString
                If s = "" Then
                    dg.Item(r, c) = 0
                    'MsgBox(dg.Item(r, c))
                End If
            Next
        Next
    End Sub
    Private Sub GetNewSetup()
        Try
            Dim R As Integer = Nothing
            Dim c As Integer = Nothing
            Dim l_ctr As Integer = 0
            dg.Hide()
            f.Grp_Visible(grp, pb, "Importing data...")
            Application.DoEvents()
            Call Set_Grid()
            For R = Val(txtStart.Text) - 1 To g_Row - 1
                If Not Trim(LCase(dg.Item(R, 2).ToString)) = "grand total" Then

                    For c = 0 To g_col - 1
                        If c >= 4 Then
                            Dim s As String = Nothing
                            grd.set_TextMatrix(l_ctr, c, Format(dg.Item(R, c), "Standard"))
                        Else
                            grd.set_TextMatrix(l_ctr, c, dg.Item(R, c).ToString)
                        End If
                    Next
                Else
                    For c = 0 To g_col - 1
                        If c >= 4 Then
                            grd.set_TextMatrix(l_ctr, c, Format(dg.Item(R, c), "##,##0.00"))
                        Else
                            grd.set_TextMatrix(l_ctr, c, dg.Item(R, c).ToString)
                        End If
                    Next
                    Exit For
                End If

                l_ctr += 1
                If l_ctr >= 2 Then
                    grd.Rows += 1
                End If

                pb.Value = ((R / (g_Row - 1)) * 100)

            Next
            '  Call Check_Total_Good()
            grp.Hide()
            picGif.Hide()
        Catch ex As Exception
            MsgBox("None Existing Data" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            grp.Hide()
        End Try
    End Sub
    Private Sub Set_Grid()
        Dim n As Integer

        With grd
            .Clear()
            .Rows = 2
            .FixedCols = 0
            .FixedRows = 0
            .Visible = True
            .Cols = g_col
            For n = 0 To grd.Cols - 1
                .set_ColWidth(n, 1800)
            Next
            .Width = dg.Size.Width
            .Height = dg.Size.Height
        End With

    End Sub
    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        btnSave.Hide()
        dg.Show()
        grd.Hide()
        picGif.Hide()
        btnEnter.Enabled = True
        btnSetup.Enabled = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        'Dim service As New EDIService.Service
        errCtr = 0
        Dim transactiondata As New List(Of transactionsPending)
        Dim editabledata As New List(Of EdiDesc)
        Dim response As New Response
        savedAllBranchError = False
        If txtdesc.Text = "" Then
            MsgBox("Please fill description text box", 16, "Synergy EDI-" + ediname)
            Exit Sub
        End If

        Dim n As Integer = Nothing
        Dim r As Integer = Nothing

        Dim Entryguid As String = Nothing
        Dim CompanyAccountCode = Nothing

        Dim rBC As String = Nothing
        Dim rBCnm As String = Nothing
        Dim rEntryNumber As String = Nothing
        Dim rFaktuurnr As String = Nothing

        Dim SysCreator As Integer = ps_resid
        Dim SysModifier As Integer = ps_resid
        Dim descr As String = Label8.Text
        Dim eMsg As String = Nothing

        Dim nFn As Boolean = False
        Dim fn As Integer = Nothing
        Dim ctrFn As Integer = Nothing

        '-------> ELI code
        If alreadysave() = True Then
            Dim li_input As Integer = Nothing
            li_input = MsgBox("This data is already been saved, Would you like to save it anyway?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
            If li_input = vbYes Then
                GoTo continuesave
            Else
                GoTo extalreadysave
            End If

        End If

continuesave:

        If balancedata() = True Then
            MsgBox("Some files in the excel appear to be not balanced, Please check Excel file and try again", 48, "File Error")
            GoTo extalreadysave
        End If
        '<------ Eli code

        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data/s")
            Exit Sub
        End If
        _refHC(btnEnter)
        f.Grp_Visible(grp, pb, "Now saving data..")
        picGif.Show()

        For n = 0 To grd.Rows - 2

            Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 0)), "000")

            Try
                response = JsonConvert.DeserializeObject(service.getBranch(bcode, edi), GetType(Response))
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                grp.Hide()
                picGif.Hide()
                Exit Sub
            End Try

            If (response.responseCode = ResponseCode.Error) Then
                MessageBox.Show(response.responseMessage)
                Exit Sub
            End If

            If response.responseCode <> ResponseCode.NotFound Then            'rdr.close - x
                Dim branch As New Branch
                branch = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(Branch))
                rBC = branch.bedrnr
                rBCnm = branch.bedrnm
                grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                Application.DoEvents()

                Try
                    response = JsonConvert.DeserializeObject(service.isActionedWu_kebot(rBC, cboPeriod.Text, Format(g_month, "yyyy").ToString, edi), GetType(Response))
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error")
                    grp.Hide()
                    picGif.Hide()
                    Exit Sub
                End Try


                If response.responseCode = ResponseCode.Error Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If

                If response.responseData Then                            'rdr.close - ok
                    Dim ls_rep As MsgBoxResult
                    Dim ls_de As String = "Branch code " & rBC & " already actioned  " & _
                    "Do you want to go forward with another branch code?" & vbCrLf & _
                    vbCrLf & "Yes - proceed to the next branch code." & vbCrLf & _
                    "No  - Cancel/Stop all transactions."


                    ls_rep = MsgBox(ls_de, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton1, "REPETITION")

                    If ls_rep = MsgBoxResult.Yes Then
                        GoTo extloop

                    ElseIf ls_rep = MsgBoxResult.No Then
                        GoTo Extsub
                    End If


                End If
                Try
                    Entryguid = service.getGuid()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error")
                    Exit Sub
                End Try


                For r = 1 To 3
                    Dim amount As Double = 0
                    Dim acfc As Double = 0
                    Dim cccc As String = ""

                    Select Case r
                        Case 1
                            amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "6000001"
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "6000001"
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            acfc = Format(grd.get_TextMatrix(n, 4)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "3100002"
                                cccc = grd.get_TextMatrix(n, 5)
                            Else
                                GoTo extcase
                            End If
                    End Select

                    CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
                    Dim Looponce As Integer
                    Dim batchNo As String = ""
                    If Looponce = 0 Then
                        batchNo = Entryguid
                        Looponce = Looponce + 1
                    End If

                    Dim temptransactionData As New transactionsPending
                    temptransactionData.paymentmethod = ""
                    temptransactionData.freefield4 = ""
                    temptransactionData.freefield5 = ""
                    temptransactionData.CompanyCode = Trim(rBC)
                    temptransactionData.TransactionType = "5"
                    temptransactionData.TransactionDate = Trim(Format(g_month, "yyyy") & "-" & Format(g_month, "MM") & "-" & g_month.DaysInMonth(Format(g_month, "yyyy"), Format(g_month, "MM")))
                    temptransactionData.FinYear = Format(g_month, "yyyy")
                    temptransactionData.FinPeriod = Format(g_month, "MM")
                    temptransactionData.Description = Trim(descr)
                    temptransactionData.CompanyAccountCode = CompanyAccountCode
                    temptransactionData.EntryNumber = Trim(rEntryNumber)
                    temptransactionData.CurrencyAliasAC = "PHP"
                    temptransactionData.AmountDebitAC = amount
                    temptransactionData.AmountCreditAC = acfc
                    temptransactionData.CurrencyAliasFC = "PHP"
                    temptransactionData.AmountDebitFC = amount
                    temptransactionData.AmountCreditFC = acfc
                    temptransactionData.VATCode = "0"
                    temptransactionData.ProcessNumber = "1"
                    temptransactionData.ProcessLineCode = "B"
                    temptransactionData.res_id = "0"
                    temptransactionData.oorsprong = "Z"
                    temptransactionData.docdate = Now.ToString("yyyy-MM-dd") ' Format(d.Now, "yyyy-MM-dd")
                    temptransactionData.vervdatfak = g_month.ToString("yyyy-MM-dd") ' Format(g_month, "yyyy-MM-dd")
                    temptransactionData.faktuurnr = rFaktuurnr
                    temptransactionData.syscreator = SysCreator
                    temptransactionData.sysmodifier = SysModifier
                    temptransactionData.EntryGuid = Entryguid
                    temptransactionData.companycostcentercode = cccc
                    temptransactionData.batchNo = batchNo
                    transactiondata.Add(temptransactionData)

extcase:
                Next r 'alter ends'

                Dim tempeditabledata As New BaseEdi
                tempeditabledata.bcode = Trim(rBC)
                ''tempeditabledata.desc = "Allocation"
                tempeditabledata.month_eli = Format(g_month, "MM").ToString
                tempeditabledata.year_eli = Format(g_month, "yyyy")
                tempeditabledata.date_time = Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                tempeditabledata.sys_creator = SysCreator
                editabledata.Add(tempeditabledata)

            Else
                errCtr += 1
                savedAllBranchError = True

                Try
                    response = JsonConvert.DeserializeObject(service.insertToWu_unknownbc(bcode, Format(CDbl(grd.get_TextMatrix(n, 3)), "##0.00"), Format(CDbl(grd.get_TextMatrix(n, 4)), "##0.00"), edi), GetType(Response))
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error")
                    grp.Hide()
                    picGif.Hide()
                    Exit Sub
                End Try

                If (response.responseCode = ResponseCode.Error) Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If
                f.Error_Log(grd.get_TextMatrix(n, 1) & " - " & grd.get_TextMatrix(n, 4) & " - " & _
                            grd.get_TextMatrix(n, 5) & " - " & grd.get_TextMatrix(n, 6), g_errbc)
                errCtr += 1
                sTrbc = sTrbc + "." + bcode
            End If

extloop:

            pb.Value = ((n / (grd.Rows - 2)) * 100)

        Next

        Try
            response = JsonConvert.DeserializeObject(service.insertTransactionBase(JsonConvert.SerializeObject(transactiondata), JsonConvert.SerializeObject(editabledata), "smart3_corp", edi), GetType(Response))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error in Saving")
            grp.Hide()
            picGif.Hide()
            Exit Sub
        End Try

        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)

        End If

        Dim task As String = Nothing
        Dim name As String = Nothing

        Try
            response = JsonConvert.DeserializeObject(service.validateUserById(ls_userid), GetType(Response))
        Catch ex As Exception
            grp.Hide()
            picGif.Hide()
            Exit Sub
        End Try

        If (response.responseCode = ResponseCode.Error) Then
            Exit Sub
        End If
        task = ""
        name = ""

        If response.responseCode = ResponseCode.OK Then
            Dim userCred As New User
            userCred = JsonConvert.DeserializeObject(response.responseData.ToString(), GetType(User))
            task = userCred.department
            name = userCred.fullname

        End If

        Dim act As String = "EDI" & " " & "Allocation" & " " & "for" & " " & txtdesc.Text
        Call CheckBC_Errors()
        Try
            response = JsonConvert.DeserializeObject(service.logdb(datenow, "EDI", act, name, task, "DONE"), GetType(Response))
        Catch ex As Exception
            grp.Hide()
            picGif.Hide()
            Exit Sub
        End Try

        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        End If

Extsub:

        grp.Visible = False

extalreadysave:
        picGif.Hide()
    End Sub
    Private Function GetCostCenter(ByVal code As String) As String
        GetCostCenter = "0001-" & code
    End Function
    Private Sub CheckBC_Errors()
        Dim n As Integer = Nothing

        If errCtr <= 0 Then
            MsgBox("Successfully saved all branches.", MsgBoxStyle.Information, "EDI-" + ediname)
            btnSave.Hide()
            btnSetup.Enabled = True
        Else
            MsgBox(Replace(sTrbc, ".", vbCrLf), MsgBoxStyle.Exclamation, errCtr & " branch code/s, not found!")
            btnSave.Hide()
            btnSetup.Enabled = False
        End If
    End Sub
    Private Sub _errHC(ByVal control As Object)
        control.backcolor = Color.PeachPuff
        control.forecolor = Color.Red
    End Sub
    Private Sub _refHC(ByVal control As Object)
        control.backcolor = Color.Empty
        control.forecolor = Color.Black
    End Sub
    Private Function alreadysave() As Boolean

        Dim n As Integer = Nothing
        Dim month As String = Nothing
        Dim response As New Response
        Dim branch As New Branch
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)
            Try
                response = JsonConvert.DeserializeObject(service.alreadySavedBase(g, cboPeriod.Text, cboYear.Text, "allocation_corp", edi), GetType(Response))
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error")
                Me.Close()
            End Try

            If response.responseCode = ResponseCode.OK Then
                If response.responseData Then
                    alreadysave = response.responseData
                    Exit Function


                End If
            Else
                MessageBox.Show(response.responseMessage)
                Exit Function
            End If

        Next
        alreadysave = False
    End Function
    Private Function errorcostcenter() As Boolean
        Dim n As Integer = Nothing
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 5)
            If Len(g).ToString < 8 Then
                MsgBox(grd.get_TextMatrix(n, 1) & " " & grd.get_TextMatrix(n, 2) & " Please fill correct cost center", 64, "Cost Center Error")
                errorcostcenter = True
                Exit Function
            End If
        Next
    End Function
    Private Function missingbranch() As Boolean
        'Dim service As New EDIService.Service
        Dim response As Response

        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)

            response = JsonConvert.DeserializeObject(service.getBranch(g, edi), GetType(Response))
            If response.responseCode = ResponseCode.Error Then
                MessageBox.Show(response.responseMessage + "\n please try again or contact administrator if problem continues ")
                Return True
            End If
            If response.responseCode = ResponseCode.NotFound Then
                Return True

            Else
            End If

        Next
        Return False
    End Function
    Private Function dualbranch() As Boolean
        Dim n As Integer = Nothing
        Dim i As Integer = Nothing
        Dim num1 As String = Nothing
        Dim num2 As String = Nothing
        For n = 0 To grd.Rows - 1
            Dim loc1 As String = grd.get_TextMatrix(n, 1)
            For i = 1 To grd.Rows - 1
                If i = grd.Rows - 1 Then
                    GoTo ext
                End If
                If n = i Then
                    i = i + 1
                End If
                Dim loc2 As String = grd.get_TextMatrix(i, 1)
                num1 = grd.get_TextMatrix(n, 0)
                num2 = grd.get_TextMatrix(i, 0)
ext:
                If num1 = num2 Then
                    MsgBox("Dual Branch " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64)
                    dualbranch = True
                End If
            Next
        Next

    End Function
    Private Function fillnullbcode() As Boolean
        Dim c As Integer = Nothing
        Dim r As Integer = Nothing
        Dim num1 As String = Nothing
        For r = Val(txtStart.Text) - 1 To g_Row - 1
            If dg.Item(r, 0).ToString = 0 Then
                num1 = InputBox(dg.Item(r, 1) & " appears to have null branch code. Please input correct branchcode", "Fill Branchcode").ToString
                If num1 = "" Then
                    num1 = 0
                    fillnullbcode = False
                    GoTo ext
                End If
                If num1 = 0 Then
                    fillnullbcode = False
                    GoTo ext
                End If
                dg.Item(r, 0) = num1
            End If
        Next
        fillnullbcode = True
ext:
    End Function
    Private Function balancedata() As Boolean

        Dim n As Integer
        Dim i As Integer = Nothing
        Dim total As Double = Nothing
        Dim r As Integer = Nothing
        Dim total_debit As Double
        Dim total_credit As Double
        Dim amount As Double = 0 'debit 
        Dim acfc As Double = 0   'credit
        Dim cccc As String = ""
        For n = 0 To grd.Rows - 2
            For r = 1 To 3
                Select Case r
                    Case 1
                        amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 2
                        amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 3
                        acfc = Format(grd.get_TextMatrix(n, 4)) 'new eli
                        If acfc <> 0 Then
                            amount = 0

                        Else
                            GoTo extcase
                        End If
                End Select
                total_debit = total_debit + amount
                total_credit = total_credit + acfc
                Dim d As Double
                Dim c As Double
                If r = 3 Then
                    d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                    c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                    If r = 3 And d <> c Then
                        Dim sqq1 As String = "There's an entry in " & _
                                             grd.get_TextMatrix(n, 1) & " " & grd.get_TextMatrix(n, 2) & _
                                             " that is not balanced" & _
                                             vbCrLf & vbCrLf & "Total Debit:" & d & _
                                             vbCrLf & "Total Credit:" & c & _
                                             vbCrLf & vbCrLf & "Please check excel file entries again "
                        MsgBox(sqq1, 16, "Unbalanced Data")
                        balancedata = True
                        Exit Function
                    End If
                    total_debit = Nothing 'to refresh debit total
                    total_credit = Nothing ' to refresh credit total
                End If
extcase:
            Next r 'alter ends'
        Next
        balancedata = False
    End Function
    Private Sub Allocation_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        End
    End Sub
    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        MsgBox("Debit - 6000001, Credit - 3100002", MsgBoxStyle.OkOnly, "Synergy" + ediname + "Allocation")
    End Sub
    Private Sub txtStart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStart.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub
    Private Sub btnClear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.Close()
    End Sub

    Private Sub txtdesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtdesc.TextChanged
        Label8.Text = txtdesc.Text & " EDI Allocation"
    End Sub
End Class