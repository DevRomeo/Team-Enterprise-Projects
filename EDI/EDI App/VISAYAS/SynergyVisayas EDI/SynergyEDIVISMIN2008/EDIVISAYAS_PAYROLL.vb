Imports Newtonsoft.Json
Imports EDIdataClass

Public Class EDIVISAYAS_PAYROLL
    Dim f As New clsFunc
    Dim D As Date = Now
    Private g_Row As Integer = Nothing
    Private g_col As Integer = Nothing
    Private g_setup As Boolean = False
    Private g_Log As String = Nothing
    Private g_BC As String = Nothing
    Private g_errLog As String = Nothing
    Private g_errBC As String = Nothing
    Private sqlmsg As String = Nothing
    Private sqltxt As String = Nothing
    Private g_month As Date = Nothing
    Private errCtr As Integer = 0
    Private arrErrBC() As String = Nothing
    Private strBC As String = Nothing
    Dim folder1 As String = "C:\SynergFolder1\"
    Dim missingfile As String = "eli.txt"
    Dim LI_COUNTCOLUMN As Integer = Nothing
    Dim datenow As DateTime = Now
    Dim db As New clsDBconnection

    Private Sub EDIVISMIN_PAYROLL_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub
    Private Sub EDIVISMIN_PAYROLL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Payroll-" & gs_serverloc & " " & gs_version
        grp.Hide()
        txtDesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
        cmbSt.Text = "False"
        Call Me.Init_Combo()
        btnSave.Hide()
        btnGIF.Hide()
        Format(Date.Now, "MM/dd/yyyy")
        grd.Hide()
        Me.makefolder()
        Me.makefile()
        Me.CenterToScreen()
    End Sub
    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        Dim ec As New clsExcelConnection
        Dim sheet() As String = Nothing
        Dim ds As DataSet = Nothing
        Dim arrCtr() As String = Nothing
        grd.Clear()
        dg.Hide()
        cboSheet.Items.Clear()
        'filter by Excel Worksheets
        OpenFile.Filter = "Excel Worksheets|*.xls"
        OpenFile.ShowDialog()
        TEXTBOX1.Text = OpenFile.FileName
        If Trim(TEXTBOX1.Text) = "" Then Exit Sub
        If Not ec.Excel_Connection(TEXTBOX1.Text) Then Exit Sub
        If ec.Error_Get_Sheets_Numbers(sheet) Then Exit Sub
        Me.initialize_cbo_sheets(sheet)
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
        '"select * from [" + cbotxt1 & "$]", ds)
        Dim ec As New clsExcelConnection
        Dim ds As New DataSet

        Dim squery As String = "Select * from [" + cboSheet.Text + "$]"
        dg.ReadOnly = True
        dg.Show()
        If Not ec.Excel_Connection(TEXTBOX1.Text) Then Exit Sub
        If ec.Error_SetDS(squery, ds) Then Exit Sub
        dg.DataSource = ds.Tables(0)
        g_Row = ds.Tables(0).Rows.Count
        g_col = ds.Tables(0).Columns.Count
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

    Private Sub btnEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnter.Click
        If cmbdesc.Text = "" Then
            MsgBox("Fill Payroll date description", 16, "Synergy EDI-" + ediname)
            Exit Sub
        End If
        If Me.TEXTBOX1.Text = "" Then
            MsgBox("Browse File First", 16, "Synergy EDI-" + ediname)
            Exit Sub
        End If
        If txtStart.Text = "" Then
            MsgBox("START ROW NOT FILLED", 64, "Payroll - Visayas")
            Exit Sub
        End If
        MsgBox("You're about to save this data in " & lblMonth.Text & " " & cboYear.Text, 64, lblMonth.Text & " " & cboYear.Text)
        If f.Error_Nonumeric(txtStart) Then Exit Sub

        If CheckUnder() = False Then
            Exit Sub
        End If
        If Format(CDate(g_month), "yyyy-MM-dd") > Format(CDate(D.Now), "yyyy-MM-dd") Then
            MsgBox("Invalid date", MsgBoxStyle.Critical, "Incorrect Date")
            Exit Sub
        End If
        'If COUNTCOLLESSER() = True Then
        '    GoTo EXT
        'End If
        Call fillnull()
        Call GetNewSetup()
        If fillnullbcode() = False Then
            GoTo EXT
        End If
        'If missingbranch() = True Then
        '    Me.btnSave.Hide()
        '    GoTo ext
        'End If
        If dualbranch() = True Then
            Me.btnSave.Hide()
            GoTo EXT
        End If
        btnSave.Show()
        '_refHC(Me.btnEnter)
        g_setup = True
        gbExplore.Enabled = False
ext:
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
                    dg.Item(r, c) = "0"
                    'MsgBox(dg.Item(r, c))
                End If
            Next
        Next
    End Sub
    Private Function fillnullbcode() As Boolean
        Dim c As Integer = Nothing
        Dim r As Integer = Nothing
        Dim num1 As String = Nothing


        For r = 0 To grd.Rows - 2
            If grd.get_TextMatrix(r, 0).ToString = "0" Then
                num1 = InputBox(grd.get_TextMatrix(r, 1) & " " & "appears to have null branch code. Please input correct branchcode", "Fill Branchcode").ToString
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
                'grd.get_TextMatrix(r, 0).ToString = num1
            End If
        Next
        fillnullbcode = True
ext:
    End Function
    Private Sub GetNewSetup()
        Try
            Dim R As Integer = Nothing
            Dim c As Integer = Nothing
            Dim l_ctr As Integer = 0
            dg.Hide()
            btnGIF.Show()
            f.Grp_Visible(grp, pb, "Importing data...")
            Application.DoEvents()
            Call Set_Grid()
            For R = Val(txtStart.Text) - 1 To g_Row - 1
                '/--------------NEW ELI CODE
                Dim s1 As String = Nothing
                s1 = dg.Item(R, 1).ToString
                If s1 = "0" Then
                    grp.Visible = False
                    btnGIF.Hide()
                    GoTo ext
                End If
                'NEW ELI CODE ----------------/
                If Not Trim(LCase(dg.Item(R, 2).ToString)) = "grand total" Then

                    For c = 0 To g_col - 1
                        If c >= 17 Then
                            Dim s As String = Nothing
                            grd.set_TextMatrix(l_ctr, c, Format(dg.Item(R, c), "Standard"))
                        Else
                            grd.set_TextMatrix(l_ctr, c, dg.Item(R, c).ToString)
                        End If
                    Next
                Else
                    For c = 0 To g_col - 1
                        If c >= 17 Then
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
            grp.Visible = False
            btnGIF.Hide()
        Catch ex As Exception
            MsgBox("None Existing Data" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            grp.Visible = False
        End Try
ext:
    End Sub
    Private Sub Set_Grid()
        Dim n As Integer
        With grd
            .Clear()
            .Rows = 2
            .FixedCols = 0
            .FixedRows = 0
            .Visible = True
            .Cols = g_col 'STANDARD
            For n = 0 To grd.Cols - 1 'standard
                .set_ColWidth(n, 1800)
                '.Cols = 19 'DEVELOPMENT
            Next
            .Width = dg.Size.Width
            .Height = dg.Size.Height
        End With
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        btnSave.Hide()
        gbExplore.Enabled = True
        dg.Show()
        grd.Hide()
        btnGIF.Hide()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim transactiondata As New List(Of transactionsPending)
        Dim editabledata As New List(Of EdiDesc)
        Dim response As New Response
        Dim batchNo As String = ""


        If txtDesc.Text = "" Then
            MsgBox("Please fill description text box", 16, "Synergy EDI " + ediname)
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
        Dim descr As String = Label7.Text
        Dim eMsg As String = Nothing

        Dim nFn As Boolean = False
        Dim fn As Integer = Nothing
        Dim ctrFn As Integer = Nothing

        If alreadysave() = True Then
            Dim li_input As Integer = Nothing
            li_input = MsgBox("This data is already been saved, Would you like to save it anyway?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
            If li_input = vbYes Then
                GoTo continuesave
            Else
                GoTo alreadysaveext
            End If

        End If
continuesave:
        If getbalance() = True Then
            GoTo alreadysaveext
        End If

        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data/s")
            Exit Sub
        End If
        _refHC(btnEnter)
        f.Grp_Visible(grp, pb, "Now saving data...")
        btnGIF.Show()

        For n = 0 To grd.Rows - 2

            'Dim totalAmount As Double = Nothing
            'Dim totalCredit As Double = Nothing
            'Dim totalDebit As Double = Nothing

            Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 0)), "000")
            Try
                response = JsonConvert.DeserializeObject(service.getBranch(bcode, edi), GetType(Response))
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                btnGIF.Hide()
                grp.Hide()
                Exit Sub
            End Try

            If (response.responseCode = ResponseCode.Error) Then
                MessageBox.Show(response.responseMessage)
                Exit Sub
            End If

            If response.responseCode = ResponseCode.OK Then            'rdr.close - x
                Dim branch As Branch
                branch = toBranch(response.responseData.ToString())
                rBC = Trim(branch.bedrnr)
                rBCnm = Trim(branch.bedrnm)

                grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                Application.DoEvents()
                Try
                    response = JsonConvert.DeserializeObject(service.isActionedWu_kebot(rBC, cboPeriod.Text, Format(g_month, "yyyy").ToString, edi), GetType(Response))
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    btnGIF.Hide()
                    grp.Hide()
                    Exit Sub
                End Try



                'sqltxt = "SELECT * FROM wu_kebot " & _
                '         "WHERE (BCODE = '" & Trim(rBC) & _
                '         "' AND (MONTH_ERN = " & cboPeriod.Text & _
                '         " AND YEAR_ERN = " & Format(g_month, "yyyy").ToString & "))"

                'response = toResponse(service.isActionedWu_kebot(rBC, cboPeriod.Text, Format(g_month, "yyyy").ToString, edi))

                If (response.responseCode = ResponseCode.Error) Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If


                If response.responseCode = ResponseCode.OK Then                            'rdr.close - ok
                    Dim ls_rep As MsgBoxResult
                    Dim ls_de As String = "Branch code " & rBC & " already actioned " & _
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
                Entryguid = service.getGuid()

                ''alter starts

                For r = 1 To 16
                    Dim amount As Double = 0
                    Dim acfc As Double = 0
                    Dim cccc As String = ""
                    Select Case r
                        Case 1
                            amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100001"
                                descr = Label7.Text & " " & "Basic Pay Reg"
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100002"
                                descr = Label7.Text & " " & "Basic Pay Trainee"
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100001"
                                descr = Label7.Text & " " & "Allowance"
                            Else
                                GoTo extcase
                            End If
                        Case 4
                            amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5212001"
                                descr = Label7.Text & " " & "BM Allowance"
                            Else
                                GoTo extcase
                            End If
                        Case 5
                            amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100003"
                                descr = Label7.Text & " " & "Overtime Reg"
                            Else
                                GoTo extcase
                            End If
                        Case 6
                            amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100003"
                                descr = Label7.Text & " " & "Overtime Trainee"
                            Else
                                GoTo extcase
                            End If
                        Case 7
                            amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5210001"
                                descr = Label7.Text & " " & "Cola"
                            Else
                                GoTo extcase
                            End If
                        Case 8
                            amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5220002"
                                descr = Label7.Text & " " & "PB / Excess PB"
                            Else
                                GoTo extcase
                            End If
                        Case 9
                            amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5220001"
                                descr = Label7.Text & " " & "Receivable Incentives"
                            Else
                                GoTo extcase
                            End If
                        Case 10
                            amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100001"
                                descr = Label7.Text & " " & "Salary Adjustment"
                            Else
                                GoTo extcase
                            End If
                        Case 11
                            amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5212001"
                                descr = Label7.Text & " " & "Graveyard"
                            Else
                                GoTo extcase
                            End If
                        Case 12
                            acfc = Format(grd.get_TextMatrix(n, 13))
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "5100001"
                                descr = Label7.Text & " " & "Late Reg"
                            Else
                                GoTo extcase
                            End If
                        Case 13
                            acfc = Format(grd.get_TextMatrix(n, 14))
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "5100002"
                                descr = Label7.Text & " " & "Late Trainee"
                            Else
                                GoTo extcase
                            End If

                        Case 14
                            acfc = Format(grd.get_TextMatrix(n, 15))
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "5100001"
                                descr = Label7.Text & " " & "Leave Reg"
                            Else
                                GoTo extcase
                            End If
                        Case 15
                            acfc = Format(grd.get_TextMatrix(n, 16))
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "5100002"
                                descr = Label7.Text & " " & "Leave Trainee"
                            Else
                                GoTo extcase
                            End If
                            '------><-------
                        Case 16
                            acfc = Format(grd.get_TextMatrix(n, 17))
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "3100001"
                                descr = Label7.Text & " " & "Pyrll"
                                cccc = Format(grd.get_TextMatrix(n, 18))
                            Else
                                GoTo extcase

                            End If
                    End Select

                    'totalDebit = (totalDebit + amount) - (totalCredit - acfc)
                    'totalCredit = totalCredit + acfc
                    'totalAmount = totalDebit - totalCredit
                    '' MsgBox(totalAmount)
                    CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
                    'rFaktuurnr = Space(8 - rFaktuurnr.Length) & rFaktuurnr
                    Dim Looponce As Integer

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
                    temptransactionData.docdate = Format(D.Now, "yyyy-MM-dd")
                    temptransactionData.vervdatfak = Format(g_month, "yyyy-MM-dd")
                    temptransactionData.faktuurnr = rFaktuurnr
                    temptransactionData.syscreator = SysCreator
                    temptransactionData.sysmodifier = SysModifier
                    temptransactionData.EntryGuid = Entryguid
                    temptransactionData.companycostcentercode = cccc
                    temptransactionData.batchNo = batchNo
                    transactiondata.Add(temptransactionData)

extcase:
                Next r 'alter ends'
                Dim tempeditabledata As New EdiDesc
                tempeditabledata.bcode = Trim(rBC)
                tempeditabledata.desc = cmbdesc.Text
                tempeditabledata.month_eli = Format(g_month, "MM").ToString
                tempeditabledata.year_eli = Format(g_month, "yyyy")
                tempeditabledata.date_time = Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                tempeditabledata.sys_creator = SysCreator
                editabledata.Add(tempeditabledata)

            Else
                Try
                    response = toResponse(service.insertToWu_unknownbc(bcode, Format(CDbl(grd.get_TextMatrix(n, 5)), "##0.00"), Format(CDbl(grd.get_TextMatrix(n, 6)), "##0.00"), edi))
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    btnGIF.Hide()
                    grp.Hide()
                    Exit Sub
                End Try


                f.Error_Log(grd.get_TextMatrix(n, 1) & " - " & grd.get_TextMatrix(n, 4) & " - " & _
                            grd.get_TextMatrix(n, 5) & " - " & grd.get_TextMatrix(n, 6), g_errBC)

                errCtr += 1

                strBC = strBC + "." + bcode
            End If

extloop:

            pb.Value = ((n / (grd.Rows - 2)) * 100)

        Next
        Try
            response = JsonConvert.DeserializeObject(service.insertTransactionWithDesc(JsonConvert.SerializeObject(transactiondata), JsonConvert.SerializeObject(editabledata), "sp_corp", "paydesc", edi), GetType(Response))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error in saving")
            btnGIF.Hide()
            grp.Hide()
            Exit Sub
        End Try


        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)
        End If

        Dim task As String = ""
        Dim name As String = ""

        Try
            response = toResponse(service.validateUserById(ls_userid))
        Catch ex As Exception
            grp.Hide()
            btnGIF.Hide()
            Exit Sub
        End Try

        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        End If

        If response.responseCode = ResponseCode.OK Then
            Dim userCred As New User
            userCred = JsonConvert.DeserializeObject(response.responseData.ToString(), GetType(User))
            task = userCred.department
            name = userCred.fullname
        End If

        Dim rdr1 As SqlClient.SqlDataReader = Nothing
        Dim act As String = "EDI" & " " & "Payroll" & " " & "for" & " " & lblMonth.Text.Trim & " " & cboYear.Text.Trim

        Try
            response = toResponse(service.logdb(Now, "EDI", act, name, task, "DONE"))
        Catch ex As Exception
            grp.Hide()

        End Try

        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        End If

        Call CheckBC_Errors()

Extsub:

        grp.Visible = False
alreadysaveext:
        btnGIF.Hide()
    End Sub
    Private Function getbalance() As Boolean
        Dim n As Integer
        Dim msg1 As String = "There's an existing data in " & grd.get_TextMatrix(n, 0) & " " & grd.get_TextMatrix(n, 1)
        Dim msg2 As String = " which is not Balanced "
        Dim i As Integer = Nothing
        Dim total As Double = Nothing
        Dim r As Integer = Nothing
        Dim total_debit As Double
        Dim total_credit As Double
        Dim amount As Double = 0 'debit 
        Dim acfc As Double = 0   'credit
        Dim cccc As String = ""
        For n = 0 To grd.Rows - 2
            For r = 1 To 16
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
                        amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 4
                        amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase

                        End If
                    Case 5
                        amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 6
                        amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 7
                        amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 8
                        amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 9
                        amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 10
                        amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 11
                        amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 12
                        acfc = Format(grd.get_TextMatrix(n, 13))
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 13
                        acfc = Format(grd.get_TextMatrix(n, 14))
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If

                    Case 14
                        acfc = Format(grd.get_TextMatrix(n, 15))
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 15
                        acfc = Format(grd.get_TextMatrix(n, 16))
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                        '------><-------
                    Case 16
                        acfc = Format(grd.get_TextMatrix(n, 17))
                        If acfc <> 0 Then
                            amount = 0
                            'cccc = Format(grd.get_TextMatrix(n, 18))
                        Else
                            GoTo extcase

                        End If
                End Select
                total_debit = total_debit + amount
                total_credit = total_credit + acfc
                Dim d As Double
                Dim c As Double
                If r = 16 Then
                    d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                    c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                    If r = 16 And d <> c Then
                        Dim sqq1 As String = "There's an entry in " & _
                                             grd.get_TextMatrix(n, 0) & " " & grd.get_TextMatrix(n, 1) & _
                                             " that is not balanced" & _
                                             vbCrLf & vbCrLf & "Total Debit:" & d & _
                                             vbCrLf & "Total Credit:" & c & _
                                             vbCrLf & vbCrLf & "Please check excel file entries again "

                        MsgBox(sqq1, 16, "Unbalanced Data")
                        getbalance = True
                        Exit Function
                        'MsgBox(msg1 & msg2 & "Total Debit:" & total_debit & " Total Credit:" & total_credit, 16, "UnBalanced Data")
                        End
                    End If
                    total_debit = Nothing
                    total_credit = Nothing
                End If
extcase:
            Next r 'alter ends'
        Next
    End Function
    Private Function GetCostCenter(ByVal code As String) As String
        GetCostCenter = "0001" & "-" & code
    End Function
    Private Sub CheckBC_Errors()
        Dim n As Integer = Nothing

        If errCtr <= 0 Then
            MsgBox("Successfully saved all branches.", MsgBoxStyle.Information, "EDI-" + ediname)
        Else
            MsgBox(Replace(strBC, ".", vbCrLf), MsgBoxStyle.Exclamation, errCtr & " branch code/s, not found!")
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
                response = JsonConvert.DeserializeObject(service.alreadySaved(g, cboPeriod.Text, cboYear.Text, "Payroll", "sp_corp", "paydesc", edi), GetType(Response))
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Me.Close()
            End Try

            If response.responseCode = ResponseCode.OK Then

                alreadysave = response.responseData
                Exit Function



            ElseIf response.responseCode = ResponseCode.Error Then
                MessageBox.Show(response.responseMessage)
                Exit Function
            End If
        Next
        alreadysave = False
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
        Dim loc2 As String = Nothing
        For n = 0 To grd.Rows - 1
            Dim loc1 As String = grd.get_TextMatrix(n, 1)
            For i = 1 To grd.Rows - 1
                If i = grd.Rows - 1 Then
                    GoTo ext
                End If
                If n = i Then
                    i = i + 1
                End If
                loc2 = grd.get_TextMatrix(i, 1)
                num1 = grd.get_TextMatrix(n, 0)
                num2 = grd.get_TextMatrix(i, 0)
ext:
                If num1 = num2 Then
                    MsgBox("Dual Branch" & " " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64, "Synergy EDI-" + ediname)
                    dualbranch = True
                End If
            Next
        Next

    End Function
    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged
        Call DATEFUNC()
        g_month = Trim(cboYear.Text) & "-" & Trim(cboPeriod.Text) & "-1"
        lblMonth.BorderStyle = BorderStyle.Fixed3D
        lblMonth.Text = Format(g_month, "MMMM")
        txtDesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim main As New Main
        main.Show()
        Me.Hide()
    End Sub

    Private Sub txtStart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStart.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub Label8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label8.Click
        MsgBox("Debit - 5100001, Debit - 5100002 , Debit - 5100001" & vbCrLf & _
        vbCrLf & "Debit - 5212001, Debit - 5100003 , Debit - 5100003" & vbCrLf & _
        vbCrLf & "Debit - 5210001, Debit - 5220002, Debit - 5220001" & vbCrLf & _
        vbCrLf & "Debit - 5100001, Debit - 5212001, Debit - 5100001" & vbCrLf & _
        vbCrLf & "Debit - 5100002, Debit - 5100001, Debit - 5100002" & vbCrLf & _
        vbCrLf & "Credit - 3100001", 64, "Synergy EDI-" + ediname)
        Exit Sub
    End Sub
    Public Sub DATEFUNC()
        Dim LS_DATE As Integer = Nothing
        Dim ls_date1 = cboPeriod.Text
        Dim dt As DateTime
        'Dim li_month As Integer = 2008
        Dim LS_SAMPLEDATE As String = Nothing
        '/------date function
        Dim year As Integer = dt.Year
        year = cboYear.Text
        Dim month As Integer = dt.Month
        month = cboPeriod.Text
        Dim days As Integer = DateTime.DaysInMonth(year, month)
        Dim lastDayOfMonth As New DateTime(year, month, days)
        LS_SAMPLEDATE = Format(lastDayOfMonth, "dd")
        cmbdesc.Items.Clear()
        cmbdesc.Items.Add("1 - 15")
        cmbdesc.Items.Add("16 - " & LS_SAMPLEDATE)
    End Sub

    Private Sub cboYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboYear.SelectedIndexChanged

    End Sub

    Private Sub cmbdesc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbdesc.SelectedIndexChanged
        Label7.Text = txtDesc.Text & " " & cmbdesc.Text & " " & "Payroll"
    End Sub

    Private Sub txtStart_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStart.TextChanged

    End Sub

    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Me.Close()
    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        ' Label7.Text = txtDesc.Text & " " & cmbdesc.Text & " " & "Payroll"

    End Sub
End Class