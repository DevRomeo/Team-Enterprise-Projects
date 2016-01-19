Imports Newtonsoft.Json
Imports EDIdataClass
Public Class EDIVISAYAS_13THMONTHBB
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

    Private Sub EDIVISMIN_13THMONTHBASEBRANCH_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub
    Private Sub EDIVISMIN_13THMONTHBASEBRANCH_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "13th Month-" + ediname + " " & gs_serverloc2 & " " & gs_version
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
            MsgBox("Browse File First", 16)
            Exit Sub
        End If
        If txtStart.Text = "" Then
            MsgBox("START ROW NOT FILLED", 64, "13th Month BaseBranch-" + ediname)
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

        retryCount = 0
        If missingbranch() = True Then
            Me.btnSave.Hide()
            GoTo ext
        End If
        retryCount = 0

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
        'Dim service As New EDIService.Service
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

        '/---------\
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

        '\----------/
        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data")
            Exit Sub
        End If
        _refHC(btnEnter)
        f.Grp_Visible(grp, pb, "Now saving data...")
        btnGIF.Show()

        For n = 0 To grd.Rows - 2

            Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 0)), "000")
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
            End Try
            retryCount = 0

            If (response.responseCode = ResponseCode.Error) Then
                MessageBox.Show(response.responseMessage)
                GoTo Extsub
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
                Entryguid = service.getGuid()

                ''alter starts

                For r = 1 To 112
                    Dim amount As Double = 0
                    Dim acfc As Double = 0
                    Dim cccc As String = ""
                    Select Case r
                        Case 1
                            amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 62))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 63))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 64))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 4
                            amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 65))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase

                            End If
                        Case 5
                            amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 66))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 6
                            amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 67))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 7
                            amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 68))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 8
                            amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 69))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 9
                            amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 70))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 10
                            amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 71))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 11
                            amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 72))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 12
                            amount = Format(grd.get_TextMatrix(n, 13)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 73))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 13
                            amount = Format(grd.get_TextMatrix(n, 14)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 74))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 14
                            amount = Format(grd.get_TextMatrix(n, 15)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 75))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 15
                            amount = Format(grd.get_TextMatrix(n, 16)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 76))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 16
                            amount = Format(grd.get_TextMatrix(n, 17)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 77))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 17
                            amount = Format(grd.get_TextMatrix(n, 18)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 78))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 18
                            amount = Format(grd.get_TextMatrix(n, 19)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 79))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 19
                            amount = Format(grd.get_TextMatrix(n, 20)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 80))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 20
                            amount = Format(grd.get_TextMatrix(n, 21)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 81))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase

                            End If
                        Case 21
                            amount = Format(grd.get_TextMatrix(n, 22)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 82))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 22
                            amount = Format(grd.get_TextMatrix(n, 23)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 83))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 23
                            amount = Format(grd.get_TextMatrix(n, 24)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 84))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 24
                            amount = Format(grd.get_TextMatrix(n, 25)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 85))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 25
                            amount = Format(grd.get_TextMatrix(n, 26)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 86))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 26
                            amount = Format(grd.get_TextMatrix(n, 27)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 87))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 27
                            amount = Format(grd.get_TextMatrix(n, 28)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 88))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 28
                            amount = Format(grd.get_TextMatrix(n, 29)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 89))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 29
                            amount = Format(grd.get_TextMatrix(n, 30)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 90))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 30
                            amount = Format(grd.get_TextMatrix(n, 31)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 91))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 31
                            amount = Format(grd.get_TextMatrix(n, 32)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 92))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 32
                            amount = Format(grd.get_TextMatrix(n, 33)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 93))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 33
                            amount = Format(grd.get_TextMatrix(n, 34)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 94))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 34
                            amount = Format(grd.get_TextMatrix(n, 35)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 95))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 35
                            amount = Format(grd.get_TextMatrix(n, 36)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 96))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 36
                            amount = Format(grd.get_TextMatrix(n, 37)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 97))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase

                            End If
                        Case 37
                            amount = Format(grd.get_TextMatrix(n, 38)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 98))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 38
                            amount = Format(grd.get_TextMatrix(n, 39)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 99))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 39
                            amount = Format(grd.get_TextMatrix(n, 40)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 100))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 40
                            amount = Format(grd.get_TextMatrix(n, 41)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 101))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 41
                            amount = Format(grd.get_TextMatrix(n, 42)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 102))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 42
                            amount = Format(grd.get_TextMatrix(n, 43)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 103))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 43
                            amount = Format(grd.get_TextMatrix(n, 44)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 104))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 44
                            amount = Format(grd.get_TextMatrix(n, 45)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 105))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 45
                            amount = Format(grd.get_TextMatrix(n, 46)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 106))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 46
                            amount = Format(grd.get_TextMatrix(n, 47)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 107))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 47
                            amount = Format(grd.get_TextMatrix(n, 48)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 108))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 48
                            amount = Format(grd.get_TextMatrix(n, 49)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 109))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If

                        Case 49
                            amount = Format(grd.get_TextMatrix(n, 50)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 110))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 50
                            amount = Format(grd.get_TextMatrix(n, 51)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 111))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 51
                            amount = Format(grd.get_TextMatrix(n, 52)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 112))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 52
                            amount = Format(grd.get_TextMatrix(n, 53)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 113))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 53
                            amount = Format(grd.get_TextMatrix(n, 54)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 114))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 54
                            amount = Format(grd.get_TextMatrix(n, 55)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 115))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 55
                            amount = Format(grd.get_TextMatrix(n, 56)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 116))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 56
                            amount = Format(grd.get_TextMatrix(n, 57)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 117))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 57
                            amount = Format(grd.get_TextMatrix(n, 58)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 118))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 58

                            amount = Format(grd.get_TextMatrix(n, 59)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                cccc = Format(grd.get_TextMatrix(n, 119))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If


                        Case 59
                            amount = Format(grd.get_TextMatrix(n, 60)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 120))
                            Else
                                GoTo extcase
                            End If
                        Case 60
                            amount = Format(grd.get_TextMatrix(n, 61)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & "  " & txtDesc.Text
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 121))
                            Else
                                GoTo extcase
                            End If
                        Case 61
                            amount = Format(grd.get_TextMatrix(n, 122)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD Allow"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))
                            Else
                                GoTo extcase
                            End If
                        Case 62
                            amount = Format(grd.get_TextMatrix(n, 123)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD OT"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))
                            Else
                                GoTo extcase

                            End If
                        Case 63
                            amount = Format(grd.get_TextMatrix(n, 124)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD Cola"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 64
                            amount = Format(grd.get_TextMatrix(n, 125)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD PB/Excess PB"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 65
                            amount = Format(grd.get_TextMatrix(n, 126)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD Sal Adj"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 66
                            amount = Format(grd.get_TextMatrix(n, 127)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD Rec Inc"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 67

                            amount = Format(grd.get_TextMatrix(n, 128)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech 13Month"
                                CompanyAccountCode = "2010002"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 68
                            amount = Format(grd.get_TextMatrix(n, 129)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech Allow"
                                CompanyAccountCode = "2010002"


                            Else
                                GoTo extcase
                            End If
                        Case 69
                            amount = Format(grd.get_TextMatrix(n, 130)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech OT"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                        Case 70
                            amount = Format(grd.get_TextMatrix(n, 131)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech Cola"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                        Case 71
                            amount = Format(grd.get_TextMatrix(n, 132)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech pb/excess pb"

                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                        Case 72
                            amount = Format(grd.get_TextMatrix(n, 133)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech Sal Adj"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                        Case 73
                            amount = Format(grd.get_TextMatrix(n, 134)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech Rec Inc"
                                CompanyAccountCode = "2010002"


                            Else
                                GoTo extcase
                            End If
                        Case 74
                            amount = Format(grd.get_TextMatrix(n, 135)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess 13Month"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                            ' MsgBox("yes", 4)
                        Case 75
                            amount = Format(grd.get_TextMatrix(n, 136)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess Allow"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                        Case 76
                            amount = Format(grd.get_TextMatrix(n, 137)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess OT"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                        Case 77
                            amount = Format(grd.get_TextMatrix(n, 138)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess pb/Excess pb"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))

                            Else
                                GoTo extcase
                            End If
                            '/-----------------------Last Entry For Debit\
                        Case 78
                            amount = Format(grd.get_TextMatrix(n, 139)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & "  " & "Mess Sal Adj"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                            '\------------------------Last Entry for Debit/
                        Case 79
                            '/------------------------Start at EI\
                            amount = Format(grd.get_TextMatrix(n, 140)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess Rec Inc"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                            '\-------------------------Start at EI/
                        Case 80
                            amount = Format(grd.get_TextMatrix(n, 141)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD Late"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                        Case 81
                            acfc = Format(grd.get_TextMatrix(n, 142)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "IAD Leave"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))
                            Else
                                GoTo extcase
                            End If
                        Case 82
                            acfc = Format(grd.get_TextMatrix(n, 143)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech Late"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))
                            Else
                                GoTo extcase
                            End If
                        Case 83
                            acfc = Format(grd.get_TextMatrix(n, 144)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Tech Leave"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                            '/--------------------Last Entry of EI\
                        Case 84
                            acfc = Format(grd.get_TextMatrix(n, 145)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess Late"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                            '\--------------------Last Entry of EI/
                        Case 85
                            acfc = Format(grd.get_TextMatrix(n, 146)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Mess Leave"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                        Case 86
                            acfc = Format(grd.get_TextMatrix(n, 147)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "WTAX"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 174))
                            Else
                                GoTo extcase
                            End If
                        Case 87
                            acfc = Format(grd.get_TextMatrix(n, 148)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "SSS Cont"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 88
                            acfc = Format(grd.get_TextMatrix(n, 149)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "SSS Loan"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 89
                            acfc = Format(grd.get_TextMatrix(n, 150)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Pag Cont"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 90
                            acfc = Format(grd.get_TextMatrix(n, 151)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Pag Loan"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 91
                            acfc = Format(grd.get_TextMatrix(n, 152)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Coated"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 92
                            acfc = Format(grd.get_TextMatrix(n, 153)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "HMO"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 93
                            acfc = Format(grd.get_TextMatrix(n, 154)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Canteen"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 94
                            acfc = Format(grd.get_TextMatrix(n, 155)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "DED 1"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 95
                            acfc = Format(grd.get_TextMatrix(n, 156)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "DED 2"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 96
                            acfc = Format(grd.get_TextMatrix(n, 157)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "ML Fund"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 97
                            acfc = Format(grd.get_TextMatrix(n, 158)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Opec"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))
                            Else
                                GoTo extcase
                            End If
                        Case 98
                            acfc = Format(grd.get_TextMatrix(n, 159)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Over Appraisal"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 99
                            acfc = Format(grd.get_TextMatrix(n, 160)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Coop Recla"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 100
                            acfc = Format(grd.get_TextMatrix(n, 161)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Filma Lending"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 101
                            acfc = Format(grd.get_TextMatrix(n, 162)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Install Acct"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If

                        Case 102
                            acfc = Format(grd.get_TextMatrix(n, 163)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Daltan Ticket"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 103
                            acfc = Format(grd.get_TextMatrix(n, 164)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Smart/PLDT"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 104
                            acfc = Format(grd.get_TextMatrix(n, 165)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Provi"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 105
                            acfc = Format(grd.get_TextMatrix(n, 166)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Comm"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 106
                            acfc = Format(grd.get_TextMatrix(n, 167)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Prime"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 107
                            acfc = Format(grd.get_TextMatrix(n, 168)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Emerg"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 108
                            acfc = Format(grd.get_TextMatrix(n, 169)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako PCash"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 109
                            acfc = Format(grd.get_TextMatrix(n, 170)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Capb"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 110
                            acfc = Format(grd.get_TextMatrix(n, 171)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Savings"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 111
                            acfc = Format(grd.get_TextMatrix(n, 172)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Sako Savings"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))

                            Else
                                GoTo extcase
                            End If
                        Case 112
                            acfc = Format(grd.get_TextMatrix(n, 173)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "13Month" & " " & cboYear.Text & " " & "Net Pay"
                                CompanyAccountCode = "3100001"
                                cccc = Format(grd.get_TextMatrix(n, 175))
                            Else
                                GoTo extcase
                            End If

                    End Select

                    CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
                    Dim Looponce As Integer
                    'Dim batchNo As String
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
                tempeditabledata.desc = "13th Month BaseBranch"
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
                    btnGIF.Hide()
                    Exit Sub
                End Try

                If (response.responseCode = ResponseCode.Error) Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If

                f.Error_Log(grd.get_TextMatrix(n, 1) & " - " & grd.get_TextMatrix(n, 4) & " - " & _
                            grd.get_TextMatrix(n, 5) & " - " & grd.get_TextMatrix(n, 6), g_errBC)
                errCtr += 1
                strBC = strBC + "." + bcode
            End If

extloop:

            pb.Value = ((n / (grd.Rows - 2)) * 100)

        Next

        '/4-23-2012
        retryCount = 0
        Try
saveRetry:
            response = JsonConvert.DeserializeObject(service.insertTransactionWithDesc(JsonConvert.SerializeObject(transactiondata), JsonConvert.SerializeObject(editabledata), "basethirmonth_corp", "paydesc", edi), GetType(Response))
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
        End Try
        retryCount = 0


        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)
        Else
            MsgBox("Successfully saved all branches.", MsgBoxStyle.Information, "EDI-" + ediname + "BatchNo: " + batchNo)
            btnSave.Hide()
            Button2.Enabled = True

        End If

Extsub:

        grp.Visible = False

extalreadysave:

        btnGIF.Hide()

    End Sub
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
alreadysavedRetry:
                response = JsonConvert.DeserializeObject(service.alreadySaved(g, cboPeriod.Text, cboYear.Text, "13thMonth BB", "basethirmonth_corp", "paydesc", edi), GetType(Response))
            Catch ex As Exception
                If (retryCount < maxretry) Then
                    retryCount += 1
                    GoTo alreadysavedRetry
                Else
                    Dim li_input As Integer = Nothing
                    li_input = MsgBox(ex.Message + ". Would you like to retry? ", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
                    If li_input = vbYes Then
                        GoTo alreadySavedRetry
                    Else
                        Application.Exit()
                    End If

                End If
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
        Dim response As New Response

        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)

            Try
getMissingBranch:

                response = JsonConvert.DeserializeObject(service.getBranch(g, edi), GetType(Response))
            Catch ex As Exception
                If (retryCount < maxretry) Then
                    retryCount += 1
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
                    MsgBox("Dual Branch" & " " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64)
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

    Private Sub btnClear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.Close()
    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        Label7.Text = txtDesc.Text & " " & "13thMonth BaseBranch"
    End Sub
End Class
