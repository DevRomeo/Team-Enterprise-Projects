
Imports Newtonsoft.Json
Imports EDIdataClass

Public Class EDIMIN_SSMI
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
    Dim folder1 As String = "C:\SynergyMindanao\"
    Dim missingfile As String = "eli.txt"
    Dim LI_COUNTCOLUMN As Integer = Nothing
    Dim datenow As DateTime = Now

    Private Sub EDIVISMIN_SSMI_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub
    Private Sub EDIVISMIN_SSMI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grp.Hide()
        txtDesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
        cmbSt.Text = "False"
        Call Me.Init_Combo()
        btnSave.Hide()
        btnGIF.Hide()
        Format(Date.Now, "MM/dd/yyyy")
        GRD.Hide()
        Me.makefolder()
        Me.makefile()
        Me.CenterToScreen()
    End Sub


    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        Dim ec As New clsExcelConnection
        Dim sheet() As String = Nothing
        Dim ds As DataSet = Nothing
        Dim arrCtr() As String = Nothing
        GRD.Clear()
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

        If Me.TEXTBOX1.Text = "" Then
            MsgBox("Browse File First", 16, "Synergy EDI-" + ediname)
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
        'MsgBox(g_month)
        Call fillnull()
        'If fillnullbcode() = False Then
        '    GoTo EXT
        'End If

        Call GetNewSetup()

        retryCount = 0
        If missingbranch() = True Then
            Me.btnSave.Hide()
            GoTo EXT
        End If
        retryCount = 0

        If dualbranch() = True Then
            Me.btnSave.Hide()
            GoTo EXT
        End If

        btnSave.Show()
        '_refHC(Me.btnEnter)
        'grp.Visible = True
        gbExplore.Enabled = False
        grp.Visible = False
        g_setup = True

EXT:

    End Sub
    Private Function CheckUnder() As Boolean
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
                    Response = JsonConvert.DeserializeObject(service.getBranch(g, edi), GetType(Response))
                    If Response.responseCode = ResponseCode.OK Then
                        MessageBox.Show("Some datas have not included. Please Check Excel File ")
                        Return False
                    ElseIf Response.responseCode = ResponseCode.Error Then
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


        For r = 0 To GRD.Rows - 2
            If GRD.get_TextMatrix(r, 0).ToString = "0" Then
                num1 = InputBox(GRD.get_TextMatrix(r, 1) & " " & "appears to have null branch code. Please input correct branchcode", "Fill Branchcode").ToString
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
                            GRD.set_TextMatrix(l_ctr, c, Format(dg.Item(R, c), "Standard"))
                        Else
                            GRD.set_TextMatrix(l_ctr, c, dg.Item(R, c).ToString)
                        End If
                    Next
                Else
                    For c = 0 To g_col - 1
                        If c >= 17 Then
                            GRD.set_TextMatrix(l_ctr, c, Format(dg.Item(R, c), "##,##0.00"))
                        Else
                            GRD.set_TextMatrix(l_ctr, c, dg.Item(R, c).ToString)
                        End If
                    Next
                    Exit For
                End If

                l_ctr += 1
                If l_ctr >= 2 Then
                    GRD.Rows += 1
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
        With GRD
            .Clear()
            .Rows = 2
            .FixedCols = 0
            .FixedRows = 0
            .Visible = True
            .Cols = g_col 'STANDARD
            For n = 0 To GRD.Cols - 1 'standard
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
        grp.Hide()
        dg.Show()
        GRD.Hide()
        btnGIF.Hide()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim transactiondata As New List(Of transactionsPending)
        Dim editabledata As New List(Of EdiDesc)
        Dim response As New Response
        Dim batchNo As String = ""



        If txtDesc.Text = "" Then
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
        Dim descr As String = Label7.Text
        Dim eMsg As String = Nothing
        Dim nFn As Boolean = False
        Dim fn As Integer = Nothing
        Dim ctrFn As Integer = Nothing

        '-------> ELI code
        retryCount = 0
        If alreadysave() = True Then
            Dim li_input As Integer = Nothing
            li_input = MsgBox("This data is already been saved, Would you like to save it anyway?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
            If li_input = vbYes Then
                GoTo continuesave
            Else
                GoTo extalreadysave
            End If

        End If
        retryCount = 0
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
        f.Grp_Visible(grp, pb, "Now saving data...")
        btnGIF.Show()

        For n = 0 To GRD.Rows - 2

            Dim bcode As String = Format(Int(GRD.get_TextMatrix(n, 0)), "000")
            retryCount = 0

            Try
getBranchRetry:
                response = JsonConvert.DeserializeObject(service.getBranch(bcode, edi), GetType(Response))
            Catch ex As Exception
                If (retryCount < maxretry) Then
                    retryCount += 1
                    GoTo getBranchRetry
                Else
                    Dim li_input As Integer = Nothing
                    li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                    If li_input = vbYes Then
                        GoTo getBranchRetry
                    Else
                        GoTo Extsub
                    End If

                End If
                grp.Hide()
                btnGIF.Hide()
            End Try

            retryCount = 0

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
                retryCount = 0
                Try
isActionedRetry:
                    response = JsonConvert.DeserializeObject(service.isActionedWu_kebot(rBC, cboPeriod.Text, Format(g_month, "yyyy").ToString, edi), GetType(Response))
                Catch ex As Exception
                    If (retryCount < maxretry) Then
                        retryCount += 1
                        GoTo isActionedRetry
                    Else
                        Dim li_input As Integer = Nothing
                        li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                        If li_input = vbYes Then
                            GoTo isActionedRetry
                        Else
                            GoTo Extsub
                        End If
                    End If
                End Try
                retryCount = 0


                If response.responseCode = ResponseCode.Error Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If

                If response.responseCode = ResponseCode.OK Then                            'rdr.close - ok
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
                retryCount = 0
                Try
getGuidRetry:
                    Entryguid = service.getGuid()
                Catch ex As Exception
                    If (retryCount < maxretry) Then
                        retryCount += 1
                        GoTo getGuidRetry
                    Else
                        Dim li_input As Integer = Nothing
                        li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                        If li_input = vbYes Then
                            GoTo getGuidRetry
                        Else
                            GoTo Extsub
                        End If
                    End If
                End Try

                retryCount = 0
                ''alter starts
                For r = 1 To 2
                    Dim amount As Double = 0
                    Dim acfc As Double = 0
                    Dim cccc As String = ""
                    Select Case r
                        Case 1
                            amount = Format(GRD.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5370003"
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            acfc = Format(GRD.get_TextMatrix(n, 3)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "3100005"
                                cccc = GRD.get_TextMatrix(n, 4)
                            Else
                                GoTo extcase
                            End If
                    End Select
                    CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
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
                tempeditabledata.desc = txtDesc.Text
                tempeditabledata.month_eli = Format(g_month, "MM").ToString
                tempeditabledata.year_eli = Format(g_month, "yyyy")
                tempeditabledata.date_time = Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                tempeditabledata.sys_creator = SysCreator
                editabledata.Add(tempeditabledata)

            Else
                retryCount = 0
                Try
unknownRetry:
                    response = JsonConvert.DeserializeObject(service.insertToWu_unknownbc(bcode, Format(CDbl(GRD.get_TextMatrix(n, 3)), "##0.00"), Format(CDbl(GRD.get_TextMatrix(n, 4)), "##0.00"), edi), GetType(Response))
                Catch ex As Exception
                    If (retryCount < maxretry) Then
                        retryCount += 1
                        GoTo unknownRetry
                    Else
                        Dim li_input As Integer = Nothing
                        li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                        If li_input = vbYes Then
                            GoTo unknownRetry
                        Else
                            GoTo Extsub
                        End If
                    End If

                    
                End Try
                retryCount = 0

                If (response.responseCode = ResponseCode.Error) Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If

                f.Error_Log(GRD.get_TextMatrix(n, 1) & " - " & GRD.get_TextMatrix(n, 4) & " - " & _
                            GRD.get_TextMatrix(n, 5) & " - " & GRD.get_TextMatrix(n, 6), g_errBC)
                errCtr += 1
                strBC = strBC + "." + bcode
            End If


extloop:
            pb.Value = ((n / (GRD.Rows - 2)) * 100)
        Next
        retryCount = 0
        Try
saveRetry:
            response = JsonConvert.DeserializeObject(service.insertTransactionWithDesc(JsonConvert.SerializeObject(transactiondata), JsonConvert.SerializeObject(editabledata), "SSMI_corp", "Monthdesc", edi), GetType(Response))
        Catch ex As Exception
            If (retryCount < maxretry) Then
                retryCount += 1
                GoTo saveRetry
            Else
                Dim li_input As Integer = Nothing
                li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                If li_input = vbYes Then
                    GoTo saveRetry
                Else
                    GoTo Extsub
                End If
            End If
            grp.Hide()
            btnGIF.Hide()
            Exit Sub
        End Try
        retryCount = 0

        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)
        Else
            MessageBox.Show("Success ! BatchNo " + batchNo)

        End If

Extsub:
        grp.Hide()
extalreadysave:
        btnGIF.Hide()
    End Sub
    Private Function GetCostCenter(ByVal code As String) As String
        GetCostCenter = "0001" & "-" & code
    End Function
    Private Sub CheckBC_Errors()
        Dim n As Integer = Nothing

        If errCtr <= 0 Then
            MsgBox("Successfuly saved all branches.", MsgBoxStyle.Information, "EDI -" + ediname)
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
            Dim g As String = GRD.get_TextMatrix(n, 0)
            Try
alreadysavedRetry:
                response = JsonConvert.DeserializeObject(service.alreadySavedBase(g, cboPeriod.Text, cboYear.Text, "ssmi_corp", edi), GetType(Response))
            Catch ex As Exception
                If (retryCount < maxretry) Then
                    retryCount += 1
                    GoTo alreadysavedRetry
                Else
                    Dim li_input As Integer = Nothing
                    li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                    If li_input = vbYes Then
                        GoTo alreadysavedRetry
                    Else
                        Application.Exit()
                    End If

                End If
                MessageBox.Show(ex.Message)
                Application.Exit()
            End Try

            If response.responseCode = ResponseCode.OK Then
                alreadysave = response.responseData
                Exit Function
            ElseIf response.responseCode = ResponseCode.Error Then
                MessageBox.Show(response.responseMessage)
                Application.Exit()
            End If
        Next
        alreadysave = False
    End Function

    Private Function missingbranch() As Boolean
        'Dim service As New EDIService.Service
        Dim response As New Response

        For n = 0 To grd.Rows - 2
            Dim g As String = GRD.get_TextMatrix(n, 0)

            Try
getMissingBranch:

                response = JsonConvert.DeserializeObject(service.getBranch(g, edi), GetType(Response))
            Catch ex As Exception
                If (retryCount < maxretry) Then
                    retryCount += 1
                    GoTo getMissingBranch
                Else
                    Dim li_input As Integer = Nothing
                    li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                    If li_input = vbYes Then
                        GoTo getMissingBranch
                    Else
                        Application.Exit()
                    End If
                End If
                response.responseCode = ResponseCode.Error
            End Try

            If response.responseCode = ResponseCode.Error Then
                MessageBox.Show(response.responseMessage + "\n please try again or contact administrator if problem continues ")
                Application.Exit()
            End If
            If response.responseCode = ResponseCode.NotFound Then
                MessageBox.Show("Branch code :" + g.ToString() + " Not found")
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
        For n = 0 To GRD.Rows - 1
            Dim loc1 As String = GRD.get_TextMatrix(n, 1)
            For i = 1 To GRD.Rows - 1
                If i = GRD.Rows - 1 Then
                    GoTo ext
                End If
                If n = i Then
                    i = i + 1
                End If
                loc2 = GRD.get_TextMatrix(i, 1)
                num1 = GRD.get_TextMatrix(n, 0)
                num2 = GRD.get_TextMatrix(i, 0)
ext:
                If num1 = num2 Then
                    MsgBox("Dual Branch" & " " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64, "Synergy EDI Mindanao")
                    dualbranch = True
                End If
            Next
        Next

    End Function
    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged
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
        MsgBox("Debit - 3131004, Credit - 4220054", 64, "Synergy EDI-" + ediname)
        Exit Sub
    End Sub
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
        Dim nn As Integer = 2

        If (edi.Equals("EDISHOWROOM")) And Main.TabControl1.SelectedIndex = 2 Then
            nn = 4
        End If

        For n = 0 To GRD.Rows - 2
            For r = 1 To n
                If (edi.Equals("EDISHOWROOM")) And Main.TabControl1.SelectedIndex = 2 Then

                    Select Case r
                        Case 1
                            amount = Format(GRD.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(GRD.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            amount = Format(GRD.get_TextMatrix(n, 4)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                            Else
                                GoTo extcase
                            End If
                        Case 4
                            acfc = Format(GRD.get_TextMatrix(n, 5)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                            Else
                                GoTo extcase
                            End If

                    End Select

                Else
                    Select Case r
                        Case 1
                            amount = Format(GRD.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            acfc = Format(GRD.get_TextMatrix(n, 3)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                            Else
                                GoTo extcase
                            End If
                End Select
                End If

                    total_debit = total_debit + amount
                    total_credit = total_credit + acfc
                    Dim d As Double
                    Dim c As Double
                    If r = 2 Then
                        d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                        c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                        If r = 2 And d <> c Then
                            Dim sqq1 As String = "There's an entry in " & _
                                                 GRD.get_TextMatrix(n, 1) & " " & GRD.get_TextMatrix(n, 2) & _
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
    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Me.Close()
    End Sub
    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        Label7.Text = txtDesc.Text & " " & "SSMI"

    End Sub

    Private Sub btnclear_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclear.DoubleClick
        'f()
    End Sub
End Class