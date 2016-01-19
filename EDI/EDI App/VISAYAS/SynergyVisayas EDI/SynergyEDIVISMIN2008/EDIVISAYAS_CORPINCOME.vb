Imports Newtonsoft.Json
Imports EDIdataClass

Public Class EDIVISAYAS_CORPINCOME
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
    Dim LI_CORPPARTNERS As String = Nothing
    Dim ls_gldebit As String = Nothing
    Dim ls_glcredit As String = Nothing
    Dim datenow As DateTime = Now
    Dim db As New clsDBconnection

    Private Sub EDIVISMIN_CORPINCOME_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Main.Show()
    End Sub

    Private Sub EDIVISAYAS_CORPINCOME_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles Me.GiveFeedback

    End Sub
    Private Sub EDIVISMIN_CORPINCOME_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "KPCorp Income-" + ediname + " " & gs_serverloc & " " & gs_version
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
        'development----------------
        Dim i As Integer = Nothing
        Dim column_col As Data.DataColumnCollection
        column_col = ds.Tables(0).Columns
        ListView1.Items.Clear()
        Dim column As Data.DataColumn
        '    Dim enumerator As IEnumerator = ds.Tables(0).Columns.GetEnumerator
        For i = 2 To column_col.Count - 1
            column = column_col(i)
            ListView1.Items.Add(column.ColumnName.ToString)
            'MsgBox(column.ColumnName.ToString)
            'li_ar.Add(column.ColumnName.ToString)
            i = i + 1
        Next
        txtcorpnumber.Text = ListView1.Items.Count - 0
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
        If txtStart.Text = "" Then
            MsgBox("START ROW NOT FILLED", 64, "KPCorp Income -" + ediname)
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
    Private Sub corpname()
        Dim response As Response
        response = toResponse(service.getCorpPartners(LI_CORPPARTNERS, edi))

        If response.responseCode = ResponseCode.Error Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        End If
        If response.responseCode <> ResponseCode.NotFound Then
            Dim partner As New Corporate_Partners
            partner = JsonConvert.DeserializeObject(response.responseData.ToString, GetType(Corporate_Partners))

            ls_gldebit = Trim(partner.GLDEBIT) 'debit
            ls_glcredit = Trim(partner.GLCREDIT) 'credit
        Else
            MsgBox(LI_CORPPARTNERS & " " & "THIS COMPANY DOESN'T EXIST IN THE DATABASE ")

        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        grp.Hide()
        btnSave.Hide()
        gbExplore.Enabled = True
        dg.Show()
        grd.Hide()
        btnGIF.Hide()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtDesc.Text = "" Then
            MsgBox("Please fill description text box", 16, "Synergy EDI-" + ediname)
            Exit Sub
        End If
        Dim response As New Response
        'Dim service As New EDIService.Service
        Dim batchNo As String = ""
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
        Dim descr As String = Nothing
        Dim eMsg As String = Nothing
        Dim nFn As Boolean = False
        Dim fn As Integer = Nothing
        Dim ctrFn As Integer = Nothing
        Dim errormsg As String = Nothing
        '-----> Already Save Function
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

        'If balancedata() = True Then
        '    MsgBox("Data not balance!", 48, "Not Balanced")

        'End If
        '<----- Already Save Function
        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data/s")
            Exit Sub
        End If
        _refHC(btnEnter)

        btnGIF.Show()


        'If jpoy.errorConnectionWriting Then Exit Sub
        f.Grp_Visible(grp, pb, "Now saving data...")
        Dim SW As New IO.StreamWriter(folder1 & missingfile)
        Dim transactiondata As New List(Of transactionsPending)
        Dim editabledata As New List(Of BaseEdi)
        For n = 0 To grd.Rows - 2
            If Not LCase(grd.get_TextMatrix(n, 0).ToString) = "totals" Then
                Dim totalAmount As Double = Nothing
                'Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 1)), "000")
                Try
                    response = toResponse(service.getBranchBOSKP(grd.get_TextMatrix(n, 1), edi))
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    btnGIF.Hide()
                    grp.Hide()
                    Exit Sub
                End Try


                If response.responseCode = ResponseCode.Error Then
                    MessageBox.Show(response.responseMessage)
                    Exit Sub
                End If
                If response.responseCode <> ResponseCode.NotFound Then
                    Dim boskp As New Boskp
                    boskp = JsonConvert.DeserializeObject(response.responseData.ToString(), GetType(Boskp))
                    rBC = Trim(boskp.boscode)
                    rBCnm = Trim(boskp.kpcode)

                    grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                    Application.DoEvents()

                    Entryguid = service.getGuid()
                    rFaktuurnr = ""
                    rEntryNumber = ""

                    For r = 1 To 248
                        Dim amount As Double = 0
                        Dim acfc As Double = 0
                        Dim cccc As String = ""

                        Select Case r
                            Case 1
                                LI_CORPPARTNERS = ListView1.Items(0).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 2
                                LI_CORPPARTNERS = ListView1.Items(0).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 3
                                LI_CORPPARTNERS = ListView1.Items(1).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 4
                                LI_CORPPARTNERS = ListView1.Items(1).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 5
                                LI_CORPPARTNERS = ListView1.Items(2).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 6
                                LI_CORPPARTNERS = ListView1.Items(2).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 7
                                LI_CORPPARTNERS = ListView1.Items(3).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 8
                                LI_CORPPARTNERS = ListView1.Items(3).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 9
                                LI_CORPPARTNERS = ListView1.Items(4).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 10
                                LI_CORPPARTNERS = ListView1.Items(4).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/------------old
                            Case 11
                                LI_CORPPARTNERS = ListView1.Items(5).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 12
                                LI_CORPPARTNERS = ListView1.Items(5).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 13)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 13
                                LI_CORPPARTNERS = ListView1.Items(6).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 14)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 14
                                LI_CORPPARTNERS = ListView1.Items(6).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 15)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '\old--------------------
                            Case 15
                                LI_CORPPARTNERS = ListView1.Items(7).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 16)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 16
                                LI_CORPPARTNERS = ListView1.Items(7).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 17)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 17
                                LI_CORPPARTNERS = ListView1.Items(8).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 18)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 18
                                LI_CORPPARTNERS = ListView1.Items(8).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 19)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 19
                                LI_CORPPARTNERS = ListView1.Items(9).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 20)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 20
                                LI_CORPPARTNERS = ListView1.Items(9).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 21)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 21
                                LI_CORPPARTNERS = ListView1.Items(10).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 22)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 22
                                LI_CORPPARTNERS = ListView1.Items(10).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 23)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 23
                                LI_CORPPARTNERS = ListView1.Items(11).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 24)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 24
                                LI_CORPPARTNERS = ListView1.Items(11).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 25)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 25
                                LI_CORPPARTNERS = ListView1.Items(12).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 26)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 26
                                LI_CORPPARTNERS = ListView1.Items(12).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 27)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 27
                                LI_CORPPARTNERS = ListView1.Items(13).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 28)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 28
                                LI_CORPPARTNERS = ListView1.Items(13).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 29)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 29
                                LI_CORPPARTNERS = ListView1.Items(14).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 30)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 30
                                LI_CORPPARTNERS = ListView1.Items(14).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 31)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 31
                                LI_CORPPARTNERS = ListView1.Items(15).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 32))
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 32
                                LI_CORPPARTNERS = ListView1.Items(15).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 33)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/-----old
                            Case 33
                                LI_CORPPARTNERS = ListView1.Items(16).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 34))
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 34
                                LI_CORPPARTNERS = ListView1.Items(16).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 35)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '-----------old/
                            Case 35
                                LI_CORPPARTNERS = ListView1.Items(17).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 36)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 36
                                LI_CORPPARTNERS = ListView1.Items(17).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 37)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 37
                                LI_CORPPARTNERS = ListView1.Items(18).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 38)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 38
                                LI_CORPPARTNERS = ListView1.Items(18).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 39)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 39
                                LI_CORPPARTNERS = ListView1.Items(19).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 40)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 40
                                LI_CORPPARTNERS = ListView1.Items(19).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 41)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 41
                                LI_CORPPARTNERS = ListView1.Items(20).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 42)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 42
                                LI_CORPPARTNERS = ListView1.Items(20).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 43)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 43
                                LI_CORPPARTNERS = ListView1.Items(21).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 44)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 44
                                LI_CORPPARTNERS = ListView1.Items(21).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 45)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 45
                                LI_CORPPARTNERS = ListView1.Items(22).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 46)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 46
                                LI_CORPPARTNERS = ListView1.Items(22).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 47)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 47
                                LI_CORPPARTNERS = ListView1.Items(23).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 48)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 48
                                LI_CORPPARTNERS = ListView1.Items(23).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 49)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 49
                                LI_CORPPARTNERS = ListView1.Items(24).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 50)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 50
                                LI_CORPPARTNERS = ListView1.Items(24).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 51)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 51
                                LI_CORPPARTNERS = ListView1.Items(25).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 52)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 52
                                LI_CORPPARTNERS = ListView1.Items(25).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 53)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                    'cccc = grd.get_TextMatrix(n, 43)
                                Else
                                    GoTo extcase
                                End If
                            Case 53
                                LI_CORPPARTNERS = ListView1.Items(26).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 54)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 54
                                LI_CORPPARTNERS = ListView1.Items(26).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 55)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 55
                                LI_CORPPARTNERS = ListView1.Items(27).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 56)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 56
                                LI_CORPPARTNERS = ListView1.Items(27).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 57)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                                '/old------------------
                            Case 57
                                LI_CORPPARTNERS = ListView1.Items(28).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 58)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 58
                                LI_CORPPARTNERS = ListView1.Items(28).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 59)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'old---------/
                            Case 59
                                LI_CORPPARTNERS = ListView1.Items(29).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 60)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 60
                                LI_CORPPARTNERS = ListView1.Items(29).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 61)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 61
                                LI_CORPPARTNERS = ListView1.Items(30).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 62)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    ''descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 62
                                LI_CORPPARTNERS = ListView1.Items(30).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 63)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 63
                                LI_CORPPARTNERS = ListView1.Items(31).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 64)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 64
                                LI_CORPPARTNERS = ListView1.Items(31).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 65)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 65
                                LI_CORPPARTNERS = ListView1.Items(32).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 66)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 66
                                LI_CORPPARTNERS = ListView1.Items(32).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 67)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 67
                                LI_CORPPARTNERS = ListView1.Items(33).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 68)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 68
                                LI_CORPPARTNERS = ListView1.Items(33).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 69)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 69
                                LI_CORPPARTNERS = ListView1.Items(34).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 70)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 70
                                LI_CORPPARTNERS = ListView1.Items(34).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 71)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 71
                                LI_CORPPARTNERS = ListView1.Items(35).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 72)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 72
                                LI_CORPPARTNERS = ListView1.Items(35).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 73)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 73
                                LI_CORPPARTNERS = ListView1.Items(36).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 74)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 74
                                LI_CORPPARTNERS = ListView1.Items(36).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 75)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 75
                                LI_CORPPARTNERS = ListView1.Items(37).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 76)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 76
                                LI_CORPPARTNERS = ListView1.Items(37).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 77)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 77
                                LI_CORPPARTNERS = ListView1.Items(38).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 78)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 78
                                LI_CORPPARTNERS = ListView1.Items(38).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 79)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 79
                                LI_CORPPARTNERS = ListView1.Items(39).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 80)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 80
                                LI_CORPPARTNERS = ListView1.Items(39).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 81)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 81
                                LI_CORPPARTNERS = ListView1.Items(40).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 82)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 82
                                LI_CORPPARTNERS = ListView1.Items(40).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 83)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/-----old
                            Case 83
                                LI_CORPPARTNERS = ListView1.Items(41).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 84)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 84
                                LI_CORPPARTNERS = ListView1.Items(41).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 85)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'old------/
                            Case 85
                                LI_CORPPARTNERS = ListView1.Items(42).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 86)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 86
                                LI_CORPPARTNERS = ListView1.Items(42).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 87)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 87
                                LI_CORPPARTNERS = ListView1.Items(43).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 88)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 88
                                LI_CORPPARTNERS = ListView1.Items(43).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 89)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 89
                                LI_CORPPARTNERS = ListView1.Items(44).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 90)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 90
                                LI_CORPPARTNERS = ListView1.Items(44).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 91)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 91
                                LI_CORPPARTNERS = ListView1.Items(45).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 92)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 92
                                LI_CORPPARTNERS = ListView1.Items(45).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 93)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 93
                                LI_CORPPARTNERS = ListView1.Items(46).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 94)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 94
                                LI_CORPPARTNERS = ListView1.Items(46).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 95)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 95
                                LI_CORPPARTNERS = ListView1.Items(47).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 96)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 96
                                LI_CORPPARTNERS = ListView1.Items(47).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 97)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/---0ld
                            Case 97
                                LI_CORPPARTNERS = ListView1.Items(48).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 98)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 98
                                LI_CORPPARTNERS = ListView1.Items(48).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 99)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'old----/
                            Case 99
                                LI_CORPPARTNERS = ListView1.Items(49).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 100)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 100
                                LI_CORPPARTNERS = ListView1.Items(49).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 101)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If


                            Case 101
                                LI_CORPPARTNERS = ListView1.Items(50).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 102)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 102
                                LI_CORPPARTNERS = ListView1.Items(50).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 103)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 103
                                LI_CORPPARTNERS = ListView1.Items(51).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 104)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 104
                                LI_CORPPARTNERS = ListView1.Items(51).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 105)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 105
                                LI_CORPPARTNERS = ListView1.Items(52).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 106)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 106
                                LI_CORPPARTNERS = ListView1.Items(52).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 107)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 107
                                LI_CORPPARTNERS = ListView1.Items(53).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 108)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 108
                                LI_CORPPARTNERS = ListView1.Items(53).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 109)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 109
                                LI_CORPPARTNERS = ListView1.Items(54).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 110)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 110
                                LI_CORPPARTNERS = ListView1.Items(54).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 111)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 111
                                LI_CORPPARTNERS = ListView1.Items(55).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 112)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 112
                                LI_CORPPARTNERS = ListView1.Items(55).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 113)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 113
                                LI_CORPPARTNERS = ListView1.Items(56).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 114)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 114
                                LI_CORPPARTNERS = ListView1.Items(56).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 115)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 115
                                LI_CORPPARTNERS = ListView1.Items(57).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 116)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 116
                                LI_CORPPARTNERS = ListView1.Items(57).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 117)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE


                            Case 117
                                LI_CORPPARTNERS = ListView1.Items(58).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 118)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 118
                                LI_CORPPARTNERS = ListView1.Items(58).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 119)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 119
                                LI_CORPPARTNERS = ListView1.Items(59).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 120)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 120
                                LI_CORPPARTNERS = ListView1.Items(59).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 121)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 121
                                LI_CORPPARTNERS = ListView1.Items(60).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 122)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 122
                                LI_CORPPARTNERS = ListView1.Items(60).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 123)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 123
                                LI_CORPPARTNERS = ListView1.Items(61).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 124)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 124
                                LI_CORPPARTNERS = ListView1.Items(61).Text.ToString
                                amount = Format(grd.get_TextMatrix(n, 125)) 'new eli
                                If amount <> 0 Then
                                    corpname()
                                    acfc = 0
                                    CompanyAccountCode = ls_gldebit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 125
                                LI_CORPPARTNERS = ListView1.Items(0).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 2)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 126
                                LI_CORPPARTNERS = ListView1.Items(0).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 3)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 127
                                LI_CORPPARTNERS = ListView1.Items(1).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 4)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 128
                                LI_CORPPARTNERS = ListView1.Items(1).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 5)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 129
                                LI_CORPPARTNERS = ListView1.Items(2).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 6)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 130
                                LI_CORPPARTNERS = ListView1.Items(2).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 7)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 131
                                LI_CORPPARTNERS = ListView1.Items(3).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 8)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 132
                                LI_CORPPARTNERS = ListView1.Items(3).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 9)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 133
                                LI_CORPPARTNERS = ListView1.Items(4).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 10)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 134
                                LI_CORPPARTNERS = ListView1.Items(4).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 11)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 135
                                LI_CORPPARTNERS = ListView1.Items(5).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 12)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 136
                                LI_CORPPARTNERS = ListView1.Items(5).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 13)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/----old
                            Case 137
                                LI_CORPPARTNERS = ListView1.Items(6).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 14)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 138
                                LI_CORPPARTNERS = ListView1.Items(6).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 15)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 139
                                LI_CORPPARTNERS = ListView1.Items(7).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 16)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 140
                                LI_CORPPARTNERS = ListView1.Items(7).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 17)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                    'old-----------/
                                Else
                                    GoTo extcase
                                End If
                            Case 141
                                LI_CORPPARTNERS = ListView1.Items(8).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 18)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 142
                                LI_CORPPARTNERS = ListView1.Items(8).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 19)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 143
                                LI_CORPPARTNERS = ListView1.Items(9).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 20))
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 144
                                LI_CORPPARTNERS = ListView1.Items(9).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 21)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 145
                                LI_CORPPARTNERS = ListView1.Items(10).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 22)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 146
                                LI_CORPPARTNERS = ListView1.Items(10).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 23)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 147
                                LI_CORPPARTNERS = ListView1.Items(11).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 24)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 148
                                LI_CORPPARTNERS = ListView1.Items(11).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 25)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 149
                                LI_CORPPARTNERS = ListView1.Items(12).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 26)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 150
                                LI_CORPPARTNERS = ListView1.Items(12).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 27)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 151
                                LI_CORPPARTNERS = ListView1.Items(13).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 28)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 152
                                LI_CORPPARTNERS = ListView1.Items(13).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 29)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 153
                                LI_CORPPARTNERS = ListView1.Items(14).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 30)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 154
                                LI_CORPPARTNERS = ListView1.Items(14).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 31)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 155
                                LI_CORPPARTNERS = ListView1.Items(15).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 32)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 156
                                LI_CORPPARTNERS = ListView1.Items(15).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 33)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 157
                                LI_CORPPARTNERS = ListView1.Items(16).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 34)) 'new eli fsd37
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 158
                                LI_CORPPARTNERS = ListView1.Items(16).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 35)) 'new eli fsd37
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/-------old

                            Case 159
                                LI_CORPPARTNERS = ListView1.Items(17).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 36)) 'new eli fsd39
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 160
                                LI_CORPPARTNERS = ListView1.Items(17).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 37)) 'new eli fsd39
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'old-----/
                            Case 161
                                LI_CORPPARTNERS = ListView1.Items(18).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 38)) 'new eli FSD40
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 162
                                LI_CORPPARTNERS = ListView1.Items(18).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 39)) 'new eli FSD40
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 163
                                LI_CORPPARTNERS = ListView1.Items(19).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 40)) 'new eli FSD42
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 164
                                LI_CORPPARTNERS = ListView1.Items(19).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 41)) 'new eli FSD42
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                    'cccc = grd.get_TextMatrix(n, 43)
                                Else
                                    GoTo extcase
                                End If
                            Case 165
                                LI_CORPPARTNERS = ListView1.Items(20).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 42)) 'new eli FSD43
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 166
                                LI_CORPPARTNERS = ListView1.Items(20).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 43)) 'new eli FSD43
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 167
                                LI_CORPPARTNERS = ListView1.Items(21).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 44)) 'new eliFSD44
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 168
                                LI_CORPPARTNERS = ListView1.Items(21).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 45)) 'new eli FSD44
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 169
                                LI_CORPPARTNERS = ListView1.Items(22).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 46)) 'new eliFSD45
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 170
                                LI_CORPPARTNERS = ListView1.Items(22).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 47)) 'new eli FSD45
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 171
                                LI_CORPPARTNERS = ListView1.Items(23).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 48)) 'new eli FSD46
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 172
                                LI_CORPPARTNERS = ListView1.Items(23).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 49)) 'new eli FSD46
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 173
                                LI_CORPPARTNERS = ListView1.Items(24).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 50)) 'new eli FSD47
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 174
                                LI_CORPPARTNERS = ListView1.Items(24).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 51)) 'new eli FSD47
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 175
                                LI_CORPPARTNERS = ListView1.Items(25).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 52)) 'new eli FSD48
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 176
                                LI_CORPPARTNERS = ListView1.Items(25).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 53)) 'new eli FSD48
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 177
                                LI_CORPPARTNERS = ListView1.Items(26).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 54)) 'new eli FSD49
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 178
                                LI_CORPPARTNERS = ListView1.Items(26).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 55)) 'new eli FSD49
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 179
                                LI_CORPPARTNERS = ListView1.Items(27).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 56)) 'new eli FSD50
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 180
                                LI_CORPPARTNERS = ListView1.Items(27).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 57)) 'new eli FSD50
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 181
                                LI_CORPPARTNERS = ListView1.Items(28).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 58)) 'new eli FSD51
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 182
                                LI_CORPPARTNERS = ListView1.Items(28).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 59)) 'new eli FSD51
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                '/----old
                            Case 183
                                LI_CORPPARTNERS = ListView1.Items(29).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 60)) 'new eli FSD52
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 184
                                LI_CORPPARTNERS = ListView1.Items(29).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 61)) 'new eli FSD52
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'old---/
                            Case 185
                                LI_CORPPARTNERS = ListView1.Items(30).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 62)) 'new eli FSD6
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 186
                                LI_CORPPARTNERS = ListView1.Items(30).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 63)) 'new eli FSD6
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 187
                                LI_CORPPARTNERS = ListView1.Items(31).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 64)) 'new eli FSD7
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 188
                                LI_CORPPARTNERS = ListView1.Items(31).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 65)) 'new eli FSD7
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 189
                                LI_CORPPARTNERS = ListView1.Items(32).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 66)) 'new eli FSD8
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 190
                                LI_CORPPARTNERS = ListView1.Items(32).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 67)) 'new eli FSD8
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 191
                                LI_CORPPARTNERS = ListView1.Items(33).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 68)) 'new eli LDV 
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 192
                                LI_CORPPARTNERS = ListView1.Items(33).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 69)) 'new eli LDV
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 193
                                LI_CORPPARTNERS = ListView1.Items(34).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 70)) 'new eli MARUFX
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 194
                                LI_CORPPARTNERS = ListView1.Items(34).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 71)) 'new eli MARUFX
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If

                            Case 195
                                LI_CORPPARTNERS = ListView1.Items(35).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 72)) 'new eli MTRBNK
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 196
                                LI_CORPPARTNERS = ListView1.Items(35).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 73)) 'new eli MTR BNK 
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 197
                                LI_CORPPARTNERS = ListView1.Items(36).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 74)) 'new eli NYBAY
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 198
                                LI_CORPPARTNERS = ListView1.Items(36).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 75)) 'new eli NYBAY
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 199
                                LI_CORPPARTNERS = ListView1.Items(37).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 76)) 'new eli PERA
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 200
                                LI_CORPPARTNERS = ListView1.Items(37).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 77)) 'new eli PERA
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 201
                                LI_CORPPARTNERS = ListView1.Items(38).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 78)) 'new eli PNYEXT
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 202
                                LI_CORPPARTNERS = ListView1.Items(38).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 79)) 'new eli PNYEXT
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 203
                                LI_CORPPARTNERS = ListView1.Items(39).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 80)) 'new eli RIATEL
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 204
                                LI_CORPPARTNERS = ListView1.Items(39).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 81)) 'new eli RIATEL
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 205
                                LI_CORPPARTNERS = ListView1.Items(40).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 82)) 'new eli USLACA
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 206
                                LI_CORPPARTNERS = ListView1.Items(40).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 83)) 'new eli USLACA
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 207
                                LI_CORPPARTNERS = ListView1.Items(41).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 84)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 208
                                LI_CORPPARTNERS = ListView1.Items(41).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 85)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 209
                                LI_CORPPARTNERS = ListView1.Items(42).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 86)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 210
                                LI_CORPPARTNERS = ListView1.Items(42).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 87)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 211
                                LI_CORPPARTNERS = ListView1.Items(43).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 88)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 212
                                LI_CORPPARTNERS = ListView1.Items(43).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 89)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 213
                                LI_CORPPARTNERS = ListView1.Items(44).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 90)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 214
                                LI_CORPPARTNERS = ListView1.Items(44).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 91)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 215
                                LI_CORPPARTNERS = ListView1.Items(45).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 92)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 216
                                LI_CORPPARTNERS = ListView1.Items(45).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 93)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 217
                                LI_CORPPARTNERS = ListView1.Items(46).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 94)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 218
                                LI_CORPPARTNERS = ListView1.Items(46).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 95)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 219
                                LI_CORPPARTNERS = ListView1.Items(47).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 96)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 220
                                LI_CORPPARTNERS = ListView1.Items(47).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 97)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 221
                                LI_CORPPARTNERS = ListView1.Items(48).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 98)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 222
                                LI_CORPPARTNERS = ListView1.Items(48).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 99)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 223
                                LI_CORPPARTNERS = ListView1.Items(49).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 100)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 224
                                LI_CORPPARTNERS = ListView1.Items(49).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 101)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 225
                                LI_CORPPARTNERS = ListView1.Items(50).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 102)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 226
                                LI_CORPPARTNERS = ListView1.Items(50).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 103)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 227
                                LI_CORPPARTNERS = ListView1.Items(51).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 104)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 228
                                LI_CORPPARTNERS = ListView1.Items(51).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 105)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 229
                                LI_CORPPARTNERS = ListView1.Items(52).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 106)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 230
                                LI_CORPPARTNERS = ListView1.Items(52).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 107)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 231
                                LI_CORPPARTNERS = ListView1.Items(53).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 108)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 232
                                LI_CORPPARTNERS = ListView1.Items(53).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 109)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE
                                '/-----old
                            Case 233
                                LI_CORPPARTNERS = ListView1.Items(54).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 110)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 234
                                LI_CORPPARTNERS = ListView1.Items(54).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 111)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 235
                                LI_CORPPARTNERS = ListView1.Items(55).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 112)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 236
                                LI_CORPPARTNERS = ListView1.Items(55).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 113)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 237
                                LI_CORPPARTNERS = ListView1.Items(56).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 114)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 238
                                LI_CORPPARTNERS = ListView1.Items(56).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 115)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 239
                                LI_CORPPARTNERS = ListView1.Items(57).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 116)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 240
                                LI_CORPPARTNERS = ListView1.Items(57).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 117)) 'new eli WWDSLM
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                            Case 241
                                LI_CORPPARTNERS = ListView1.Items(58).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 118)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 242
                                LI_CORPPARTNERS = ListView1.Items(58).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 119)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 243
                                LI_CORPPARTNERS = ListView1.Items(59).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 120)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 244
                                LI_CORPPARTNERS = ListView1.Items(59).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 121)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 245
                                LI_CORPPARTNERS = ListView1.Items(60).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 122)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 246
                                LI_CORPPARTNERS = ListView1.Items(60).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 123)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 247
                                LI_CORPPARTNERS = ListView1.Items(61).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 124)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "PHP" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                                'ALTER STARTS HERE

                            Case 248
                                LI_CORPPARTNERS = ListView1.Items(61).Text.ToString
                                acfc = Format(grd.get_TextMatrix(n, 125)) 'new eli
                                If acfc <> 0 Then
                                    corpname()
                                    amount = 0
                                    CompanyAccountCode = ls_glcredit
                                    'descr = "Corp Income" & "USD" & " " & txtDesc.Text
                                    descr = Label7.Text
                                Else
                                    GoTo extcase
                                End If
                        End Select
                        'totalAmount = totalAmount + amount
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
                    Next r
                    Dim tempeditabledata As New BaseEdi
                    tempeditabledata.bcode = Trim(rBC)
                    tempeditabledata.month_eli = Format(g_month, "MM").ToString
                    tempeditabledata.year_eli = Format(g_month, "yyyy")
                    tempeditabledata.date_time = Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    tempeditabledata.sys_creator = SysCreator
                    editabledata.Add(tempeditabledata)
                Else
                    Try
                        response = JsonConvert.DeserializeObject(service.insertToWu_unknownbc(rBC, Format(CDbl(grd.get_TextMatrix(n, 3)), "##0.00"), Format(CDbl(grd.get_TextMatrix(n, 4)), "##0.00"), edi), GetType(Response))
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
                    SW.WriteLine(grd.get_TextMatrix(n, 1) & " Missing Branch Code")
                    errCtr += 1
                    strBC = strBC + "." + rBC
                End If
            End If
            pb.Value = ((n / (grd.Rows - 2)) * 100)
        Next

        Try
            response = JsonConvert.DeserializeObject(service.insertTransactionBase(JsonConvert.SerializeObject(transactiondata), JsonConvert.SerializeObject(editabledata), "CORPINCOME_corp", edi), GetType(Response))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error in Saving")
            btnGIF.Hide()
            grp.Hide()
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
            btnGIF.Hide()
            Exit Sub
        End Try

        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            btnGIF.Hide()
            grp.Hide()
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

        Dim act As String = "EDI" & " " & "CorpIncome" & " " & "for" & " " & lblMonth.Text.Trim & " " & cboYear.Text.Trim

        Call CheckBC_Errors()
        Try
            response = JsonConvert.DeserializeObject(service.logdb(datenow, "EDI", act, name, task, "DONE"), GetType(Response))
        Catch ex As Exception
            btnGIF.Hide()
            grp.Hide()
            Exit Sub
        End Try


        If (response.responseCode = ResponseCode.Error) Then
            MessageBox.Show(response.responseMessage)
            Exit Sub
        End If



        SW.Close()
extAlreadySave:
        grp.Hide()
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
        Dim ls_boscode As String = ""


        Dim response As New Response
        'Dim service As New EDIService.Service
        For n = 0 To grd.Rows - 2
            Dim ls_searchkpcode As String = grd.get_TextMatrix(n, 1)
            Try
                response = toResponse(service.getBranchBOSKP(ls_searchkpcode, edi))
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Me.Close()
            End Try

            If (response.responseCode = ResponseCode.Error) Then
                MessageBox.Show(response.responseMessage)
                Return False
            ElseIf response.responseCode = ResponseCode.OK Then
                Dim boskp As Boskp
                boskp = JsonConvert.DeserializeObject(response.responseData.ToString(), GetType(Boskp))
                ls_boscode = boskp.boscode
                Return True
            End If
            Try
                response = toResponse(service.alreadySavedBase(ls_boscode, cboPeriod.Text, cboYear.Text, "CORPINCOME_corp", edi))
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Me.Close()
            End Try

            If (response.responseCode = ResponseCode.Error) Then
                MessageBox.Show(response.responseMessage)
                Return False
            ElseIf response.responseCode = ResponseCode.OK Then
                Return True
            End If


        Next
        alreadysave = False
    End Function
    Private Function missingbranch() As Boolean
        'Dim service As New EDIService.Service
        Dim response As Response

        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 1)

            'response = JsonConvert.DeserializeObject(service.getBranch(g, edi), GetType(Response))
            response = JsonConvert.DeserializeObject(service.getBranchBOSKP(g, edi), GetType(Response))
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
                num1 = grd.get_TextMatrix(n, 1)
                num2 = grd.get_TextMatrix(i, 1)
ext:
                If num1 = num2 Then
                    MsgBox("Dual Branch" & " " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64, "Synergy EDI-" + ediname)
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

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        Label7.Text = txtDesc.Text & " " & "Kp Corp Income"

    End Sub
End Class