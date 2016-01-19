Imports Newtonsoft.Json
Imports EDIdataClass

Public Class EDIReportVISAYAS

    Inherits System.Windows.Forms.Form
    Dim folder1 As String = "C:\SynergFolder1\"
    Dim missingfile As String = "silvher.txt"
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

    Private Sub EDIReportLuzon_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub

    Private Sub EDIReportLuzon_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "EDI Report-" + ediname + " " & gs_serverloc3 & " " & gs_version
        cboSet.Text = "False"
        Call Me.Init_Combo()
        btnSave.Hide()
        picGif.Hide()
        Format(Date.Now, "MM/dd/yyyy")
        grd.Hide()
        btnSetup.Enabled = False
        btnEnter.Enabled = False
        grp.Hide()
        Me.makefolder()
        Me.makefile()
        CenterToScreen()
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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''Dim service As New EDIService.Service
        Dim transactiondata As New List(Of transactionsPending)
        Dim editabledata As New List(Of EdiDesc)
        Dim response As New Response

        If txtdesc.Text = "" Then
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
        '----------->ELI Code/

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
                    MessageBox.Show(ex.Message)
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

                Entryguid = New Guid().NewGuid().ToString()

                ''alter starts
                For r = 1 To 24
                    Dim amount As Double = 0
                    Dim acfc As Double = 0
                    Dim cccc As String = ""

                    Select Case r
                        Case 1
                            amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100001"
                                descr = txtdesc.Text & " " & "SALARY REGULAR"
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100002"
                                descr = txtdesc.Text & " " & "SALARY TRAINEE"
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100001"
                                descr = txtdesc.Text & " " & "SALARY ADJUSTMENT REGULAR"

                            Else
                                GoTo extcase
                            End If
                        Case 4
                            amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100002"
                                descr = txtdesc.Text & " " & "SALARY ADJUSTMENT TRAINEE"
                            Else
                                GoTo extcase
                            End If
                        Case 5
                            amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5100003"
                                descr = txtdesc.Text & " " & "OVERTIME"
                            Else
                                GoTo extcase
                            End If
                        Case 6
                            amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5210001"
                                descr = txtdesc.Text & " " & "ECOLA"
                            Else
                                GoTo extcase
                            End If
                        Case 7
                            amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5211001"
                                descr = txtdesc.Text & " " & "MEAL ALLOW"
                            Else
                                GoTo extcase
                            End If
                        Case 8
                            amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5212001"
                                descr = txtdesc.Text & " " & "BM/ABM ALLOW"

                            Else
                                GoTo extcase
                            End If
                        Case 9
                            amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5215001"
                                descr = txtdesc.Text & " " & "STAFF HOUSE"
                            Else
                                GoTo extcase
                            End If
                        Case 10
                            amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5220001"
                                descr = txtdesc.Text & " " & "REC INCENTIVES"
                            Else
                                GoTo extcase
                            End If
                        Case 11
                            amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5220002"
                                descr = txtdesc.Text & " " & "PROD BONUS"
                            Else
                                GoTo extcase
                            End If
                        Case 12
                            amount = Format(grd.get_TextMatrix(n, 13)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5220003"
                                descr = txtdesc.Text & " " & "SALES INCENTIVES"

                            Else
                                GoTo extcase
                            End If
                        Case 13
                            amount = Format(grd.get_TextMatrix(n, 14)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5270001"
                                descr = txtdesc.Text & " " & "LEAVE CONVERSION"
                            Else
                                GoTo extcase
                            End If
                        Case 14
                            amount = Format(grd.get_TextMatrix(n, 15)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5530001"
                                descr = txtdesc.Text & " " & "INSURANCE INCENTIVES"
                            Else
                                GoTo extcase
                            End If
                        Case 15
                            amount = Format(grd.get_TextMatrix(n, 16)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                CompanyAccountCode = "5530002"
                                descr = txtdesc.Text & " " & "DRIED FRUITS INCENTIVES"
                            Else
                                GoTo extcase
                            End If
                        Case 16
                            acfc = Format(grd.get_TextMatrix(n, 17)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "5100001"
                                descr = txtdesc.Text & " " & "LEAVE"
                            Else
                                GoTo extcase
                            End If
                        Case 17
                            acfc = Format(grd.get_TextMatrix(n, 18)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "5100001"
                                descr = txtdesc.Text & " " & "LATES"

                            Else
                                GoTo extcase
                            End If
                        Case 18
                            acfc = Format(grd.get_TextMatrix(n, 19)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "2070102"
                                descr = txtdesc.Text & " " & "W/HOLDING TAX"

                            Else
                                GoTo extcase
                            End If
                        Case 19
                            acfc = Format(grd.get_TextMatrix(n, 20)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "2070104"
                                descr = txtdesc.Text & " " & "SSS"

                            Else
                                GoTo extcase
                            End If
                        Case 20
                            acfc = Format(grd.get_TextMatrix(n, 21)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "2070106"
                                descr = txtdesc.Text & " " & "PAG-IBIG PREMIUM"

                            Else
                                GoTo extcase
                            End If
                        Case 21
                            acfc = Format(grd.get_TextMatrix(n, 22)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "2070108"
                                descr = txtdesc.Text & " " & "PHILHEALTH PREMIUM"

                            Else
                                GoTo extcase
                            End If
                        Case 22
                            acfc = Format(grd.get_TextMatrix(n, 23)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "2070006"
                                descr = txtdesc.Text & " " & "HMO"

                            Else
                                GoTo extcase
                            End If
                        Case 23
                            acfc = Format(grd.get_TextMatrix(n, 24)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "1020401"
                                descr = txtdesc.Text & " " & "EMP ACCT"

                            Else
                                GoTo extcase
                            End If
                        Case 24
                            acfc = Format(grd.get_TextMatrix(n, 25)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                CompanyAccountCode = "3100002"
                                descr = txtdesc.Text & " " & "NET PAY"
                                cccc = Format(grd.get_TextMatrix(n, 26))
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
                    temptransactionData.docdate = Format(d.Now, "yyyy-MM-dd")
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
                tempeditabledata.desc = "EDI Report"
                tempeditabledata.month_eli = Format(g_month, "MM").ToString
                tempeditabledata.year_eli = Format(g_month, "yyyy")
                tempeditabledata.date_time = Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                tempeditabledata.sys_creator = SysCreator
                editabledata.Add(tempeditabledata)

            Else
                Try
                    response = JsonConvert.DeserializeObject(service.insertToWu_unknownbc(bcode, Format(CDbl(grd.get_TextMatrix(n, 3)), "##0.00"), Format(CDbl(grd.get_TextMatrix(n, 4)), "##0.00"), edi), GetType(Response))
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

                f.Error_Log(grd.get_TextMatrix(n, 1) & " - " & grd.get_TextMatrix(n, 4) & " - " & _
                            grd.get_TextMatrix(n, 5) & " - " & grd.get_TextMatrix(n, 6), g_errbc)
                errCtr += 1
                sTrbc = sTrbc + "." + bcode
            End If

extloop:

            pb.Value = ((n / (grd.Rows - 2)) * 100)

        Next
        Try
            response = JsonConvert.DeserializeObject(service.insertTransactionWithDesc(JsonConvert.SerializeObject(transactiondata), JsonConvert.SerializeObject(editabledata), "EDIREPORT_corp", "descs", edi), GetType(Response))
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

        'Try

        'Catch ex As Exception

        'End Try
        Try
            response = JsonConvert.DeserializeObject(service.validateUserById(ls_userid), GetType(Response))
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
        task = ""
        name = ""

        If response.responseCode = ResponseCode.OK Then
            Dim userCred As New User
            userCred = JsonConvert.DeserializeObject(response.responseData.ToString(), GetType(User))
            task = userCred.department
            name = userCred.fullname

        End If

        Dim act As String = "EDI" & " " & "EDI Report" & " " & "for" & " " & txtdesc.Text
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

        'c.DisposeR()
        'c.DisposeW()
        'c = Nothing
        grp.Visible = False

extalreadysave:

        picGif.Hide()
    End Sub
    Private Function alreadysave() As Boolean
        Dim n As Integer = Nothing
        Dim month As String = Nothing
        Dim response As New Response
        Dim branch As New Branch
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)

            Try
                response = JsonConvert.DeserializeObject(service.alreadySaved(g, cboPeriod.Text, cboYear.Text, "EDI Report", "EDIREPORT_corp", "descs", edi), GetType(Response))
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Me.Close()
            End Try

            If response.responseCode = ResponseCode.OK Then
                If response.responseData Then
                    alreadysave = response.responseData
                    Exit Function

                End If
            ElseIf response.responseCode = ResponseCode.Error Then
                MessageBox.Show(response.responseMessage)
                Exit Function
            End If

        Next
        alreadysave = False
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
            For r = 1 To 24
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
                        amount = Format(grd.get_TextMatrix(n, 13)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 13
                        amount = Format(grd.get_TextMatrix(n, 14)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 14
                        amount = Format(grd.get_TextMatrix(n, 15)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 15
                        amount = Format(grd.get_TextMatrix(n, 16)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 16
                        acfc = Format(grd.get_TextMatrix(n, 17)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 17
                        acfc = Format(grd.get_TextMatrix(n, 18)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 18
                        acfc = Format(grd.get_TextMatrix(n, 19)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 19
                        acfc = Format(grd.get_TextMatrix(n, 20)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 20
                        acfc = Format(grd.get_TextMatrix(n, 21)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 21
                        acfc = Format(grd.get_TextMatrix(n, 22)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 22
                        acfc = Format(grd.get_TextMatrix(n, 23)) 'new eli
                        If acfc <> 0 Then
                            amount = 0

                        Else
                            GoTo extcase
                        End If
                    Case 23
                        acfc = Format(grd.get_TextMatrix(n, 24)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 24
                        acfc = Format(grd.get_TextMatrix(n, 25)) 'new eli
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
                If r = 24 Then
                    d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                    c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                    If r = 24 And d <> c Then
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

    Private Sub _errHC(ByVal control As Object)
        control.backcolor = Color.PeachPuff
        control.forecolor = Color.Red
    End Sub

    Private Sub _refHC(ByVal control As Object)
        control.backcolor = Color.Empty
        control.forecolor = Color.Black
    End Sub

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

    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        btnSave.Hide()
        dg.Show()
        grd.Hide()
        'grpinf.Show()
        picGif.Hide()
    End Sub

    Private Sub btnEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnter.Click

        If txtStart.Text = "" Then
            MsgBox("Start row is empty", 16)
            Exit Sub
        End If
        If Me.txtLocation.Text = "" Then
            MsgBox("Browse File First", 16)
            Exit Sub
        End If
        MsgBox("You're about to save this data in " & lblMonth.Text & " " & cboYear.Text, 64, lblMonth.Text & " " & cboYear.Text)
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
            Me.btnSave.Hide()
            GoTo EXT
        End If
        If errorcostcenter() = True Then
            Me.btnSave.Hide()
            GoTo EXT
        End If
        If dualbranch() = True Then
            Me.btnSave.Hide()
            GoTo EXT
        End If
        btnSave.Show()
        '_refHC(Me.btnEnter)
        'grpinf.Visible = True
        g_setup = True

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
        For r = Val(txtStart.Text) - 1 To g_Row - 1
            If dg.Item(r, 0).ToString = 0 Then
                num1 = InputBox(dg.Item(r, 2) & " " & "appears to have null branch code. Please input correct branchcode", "Fill Branchcode").ToString
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

    Private Sub GetNewSetup()
        Try
            Dim R As Integer = Nothing
            Dim c As Integer = Nothing
            Dim l_ctr As Integer = 0
            dg.Hide()
            picGif.Show()
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
            grp.Visible = False
            picGif.Hide()
        Catch ex As Exception
            MsgBox("None Existing Data" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            grp.Visible = False
        End Try
    End Sub

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

    Private Function errorcostcenter() As Boolean
        Dim n As Integer = Nothing
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 26)
            If Len(g).ToString < 8 Then
                MsgBox(grd.get_TextMatrix(n, 1) & " " & grd.get_TextMatrix(n, 2) & " " & "Please fill correct cost center", 64, "Cost Center Error")
                errorcostcenter = True
                Exit Function
            End If
        Next
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
                    MsgBox("Dual Branch" & " " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64)
                    dualbranch = True
                End If
            Next
        Next
    End Function

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

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
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
        txtLocation.Text = OpenFile.FileName
        If Trim(txtLocation.Text) = "" Then Exit Sub
        If Not ec.Excel_Connection(txtLocation.Text) Then Exit Sub
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

    Private Sub cboSheet_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSheet.SelectedIndexChanged
        '"select * from [" + cbotxt1 & "$]", ds)
        dg.Show()
        Dim ec As New clsExcelConnection
        Dim ds As New DataSet
        Dim squery As String = "Select * from [" + cboSheet.Text + "$]"
        dg.ReadOnly = True
        If Not ec.Excel_Connection(txtLocation.Text) Then Exit Sub
        If ec.Error_SetDS(squery, ds) Then Exit Sub
        dg.DataSource = ds.Tables(0)
        g_Row = ds.Tables(0).Rows.Count
        g_col = ds.Tables(0).Columns.Count
        btnEnter.Enabled = True
    End Sub

    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged
        g_month = Trim(cboYear.Text) & "-" & Trim(cboPeriod.Text) & "-1"
        lblMonth.BorderStyle = BorderStyle.Fixed3D
        lblMonth.Text = Format(g_month, "MMMM")
        txtdesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim main As New Main
        main.Show()
        Me.Hide()
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        Dim sqq = "Debit        ,Credit" & _
                  vbCrLf & "5100001  ,5100001" & _
                  vbCrLf & "5100002  ,2070102" & _
                  vbCrLf & "5100003  ,2070104" & _
                  vbCrLf & "5210001  ,2070106" & _
                  vbCrLf & "5212001  ,2070108" & _
                  vbCrLf & "5215001  ,2070006" & _
                  vbCrLf & "5220001  ,1020401" & _
                  vbCrLf & "5220002  ,3100002" & _
                  vbCrLf & "5220003  " & _
                  vbCrLf & "5270001  " & _
                  vbCrLf & "5530001  " & _
                  vbCrLf & "5530002  " & _
                  vbCrLf & "5530001  "
        MsgBox(sqq, MsgBoxStyle.OkOnly, "Synergy" + ediname + "EDI Report")
    End Sub

    Private Sub txtStart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStart.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub txtStart_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStart.TextChanged

    End Sub

    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Me.Close()
    End Sub

    Private Sub gbUploadP_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbUploadP.Enter

    End Sub

    Private Sub txtdesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtdesc.TextChanged
        Label8.Text = txtdesc.Text & " " & "EDI Report"
    End Sub
End Class