﻿Public Class EDIMIN_SECURITYBB
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
    Dim db As New clsDBconnection

    Private Sub EDIVISMIN_SECURITYBASEBRANCH_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim main As New Main
        Me.Hide()
        main.Show()
    End Sub
    Private Sub EDIVISMIN_SECURITYBASEBRANCH_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Security Guard-" & gs_serverloc2 & " " & gs_version
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
        If Not ec.Excel_Connection(TextBox1.Text) Then Exit Sub
        If ec.Error_SetDS(squery, ds) Then Exit Sub
        dg.DataSource = ds.Tables(0)
        g_Row = ds.Tables(0).Rows.Count
        g_col = ds.Tables(0).Columns.Count
    End Sub
    Private Sub Init_Combo()
        Dim d As DateTime = Now
        Dim n As Integer = Nothing
        cboperiod.DropDownStyle = ComboBoxStyle.DropDownList
        cboyear.DropDownStyle = ComboBoxStyle.DropDownList
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
            MsgBox("START ROW NOT FILLED", 64, "Mid Year -Mindanao")
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
        If missingbranch() = True Then
            Me.btnSave.Hide()
            GoTo ext
        End If
        btnSave.Show()
        '_refHC(Me.btnEnter)
        g_setup = True
        gbExplore.Enabled = False
ext:
    End Sub
    Private Function CheckUnder() As Boolean
        If Val(txtStart.Text) > g_Row Then
            f.Emsg("Rows is up to " & g_Row & " only.", "out of range")
            CheckUnder = False
            Exit Function
        End If

        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader = Nothing

        Dim n As Integer = Nothing

        If c.Error_Inititalize_INI Or c.ErrorConnectionReading(False) Then
            CheckUnder = False
            Exit Function
        End If

        For n = Val(txtStart.Text) - 2 To 0 Step -1

            If Not Trim(dg.Item(n, 0).ToString) = "" Then

                '----> this satement figures if item in datagrid 0 is 2 0r 3 characters
                Dim s As String = dg.Item(n, 0)
                If Len(s).ToString < 3 Then
                    Dim bcode As String = "0" & (dg.Item(n, 0))
                    sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"

                Else
                    Dim bcode As String = (dg.Item(n, 0))
                    sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"
                End If
                '----> this satement figures if item in datagrid 0 is 2 0r 3 characters

                If c.Error_SetRdr(sqltxt, rdr, sqlmsg) Then
                    f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errLog)
                    CheckUnder = False
                    Exit Function

                Else

                    If rdr.Read Then
                        MsgBox("Some datas have not included. Please Check Excel File", 64)
                        'MsgBox("Branch code " & bcode & "(" & Trim(rdr(1)) & ") was found in Bedryf table, " & vbCrLf & _
                        '"but will not be recorded because you started at row " & txtstart.Text & "." & _
                        'vbCrLf & "It is safe to check all datas in your excel file and try again. Program will be stopped." _
                        ', MsgBoxStyle.Exclamation, "some data missed")

                        CheckUnder = False

                        rdr.Close()

                        Exit Function

                    End If

                    rdr.Close()

                End If
            End If

        Next

        CheckUnder = True

        c.DisposeR()
        c = Nothing

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
    Private Function balancedata() As Boolean
        Dim n As Integer
        Dim total As Double = Nothing
        Dim r As Integer = Nothing
        Dim total_debit As Double
        Dim total_credit As Double
        Dim amount As Double = Nothing 'debit 
        Dim acfc As Double = Nothing   'credit
        For n = 0 To grd.Rows - 2
            For r = 1 To 56
                'MsgBox(grd.get_TextMatrix(n, 111))
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
                        amount = Format(grd.get_TextMatrix(n, 17)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 17
                        amount = Format(grd.get_TextMatrix(n, 18)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 18
                        amount = Format(grd.get_TextMatrix(n, 19)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 19
                        amount = Format(grd.get_TextMatrix(n, 20)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 20
                        amount = Format(grd.get_TextMatrix(n, 21)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 21
                        amount = Format(grd.get_TextMatrix(n, 22)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 22
                        amount = Format(grd.get_TextMatrix(n, 23)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 23
                        amount = Format(grd.get_TextMatrix(n, 24)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 24
                        amount = Format(grd.get_TextMatrix(n, 25)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 25
                        amount = Format(grd.get_TextMatrix(n, 26)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 26
                        amount = Format(grd.get_TextMatrix(n, 27)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 27
                        amount = Format(grd.get_TextMatrix(n, 28)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 28
                        amount = Format(grd.get_TextMatrix(n, 29)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 29
                        amount = Format(grd.get_TextMatrix(n, 30)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 30
                        amount = Format(grd.get_TextMatrix(n, 31)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 31
                        amount = Format(grd.get_TextMatrix(n, 32)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 32
                        amount = Format(grd.get_TextMatrix(n, 33)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 33
                        amount = Format(grd.get_TextMatrix(n, 34)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 34
                        amount = Format(grd.get_TextMatrix(n, 35)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 35
                        amount = Format(grd.get_TextMatrix(n, 36)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 36
                        amount = Format(grd.get_TextMatrix(n, 37)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 37
                        amount = Format(grd.get_TextMatrix(n, 38)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 38
                        amount = Format(grd.get_TextMatrix(n, 39)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 39
                        amount = Format(grd.get_TextMatrix(n, 40)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 40
                        amount = Format(grd.get_TextMatrix(n, 41)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 41
                        amount = Format(grd.get_TextMatrix(n, 42)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 42
                        amount = Format(grd.get_TextMatrix(n, 43)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 43
                        amount = Format(grd.get_TextMatrix(n, 44)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 44
                        amount = Format(grd.get_TextMatrix(n, 45)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 45
                        amount = Format(grd.get_TextMatrix(n, 46)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 46
                        amount = Format(grd.get_TextMatrix(n, 47)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 47
                        amount = Format(grd.get_TextMatrix(n, 48)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 48
                        amount = Format(grd.get_TextMatrix(n, 49)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 49
                        amount = Format(grd.get_TextMatrix(n, 50)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 50
                        amount = Format(grd.get_TextMatrix(n, 51)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 51
                        amount = Format(grd.get_TextMatrix(n, 52)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 52
                        amount = Format(grd.get_TextMatrix(n, 53)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                        '/-----------------End of Debit \
                    Case 53
                        amount = Format(grd.get_TextMatrix(n, 54)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If

                    Case 54
                        'MSGBOX(GRD.get_TextMatrix(N,108))
                        'amount = Format(grd.get_TextMatrix(n, 112))
                        amount = Format(grd.get_TextMatrix(n, 108))
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                        '\-----------------End of debit/
                    Case 55
                        'acfc = Format(grd.get_TextMatrix(n, 113))
                        acfc = Format(grd.get_TextMatrix(n, 109))
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 56
                        'acfc = Format(grd.get_TextMatrix(n, 115))
                        acfc = Format(grd.get_TextMatrix(n, 111))

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
                If r = 56 Then
                    d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                    c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                    If r = 56 And d <> c Then
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
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If txtDesc.Text = "" Then
            MsgBox("Please fill description text box", 16, "Synergy EDI Mindanao")
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
            btnSave.Hide()
            GoTo extalreadysave
        End If


        '<------ Eli code

        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data/s")
            Exit Sub
        End If
        _refHC(btnEnter)

        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader

        'If c.Error_Inititalize_INI Then Exit Sub

        'If c.ErrorConnectionReading(False) Then Exit Sub


        sqltxt = "select * from basecorpSg1_corp where bcode = 000 and date_time > '" & _
                          Format(g_month, "yyyy-MM-dd") & "'"

        If db.isConnected Then                  '--------------7152010-------------------'
            db.CloseConnection()                '--------------7152010-------------------'
        End If                                  '--------------7152010-------------------'
        If ls_connection Is Nothing Then
            c.INITIALIZE_INI()
        End If
        db.ConnectDB(ls_connection)             '--------------7152010-------------------'
        rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

        'If rdr.Read Then
        '    MsgBox("Date specified was already actioned using e-Synergy.", MsgBoxStyle.Critical, Format(g_month, "yyyy-MM-dd"))
        '    GoTo Extsub
        'End If
        rdr.Close()
        db.CloseConnection()

        'If c.errorConnectionWriting Then Exit Sub


        f.Grp_Visible(grp, pb, "Now saving data/s...")
        btnGIF.Show()

        For n = 0 To grd.Rows - 2

            Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 0)), "000")
            sqltxt = "select bedrnr, bedrnm from BEDRYF where bedrnr = '" & _
                      bcode & "'"
            If db.isConnected Then                  '--------------7152010-------------------'
                db.CloseConnection()                '--------------7152010-------------------'
            End If                                  '--------------7152010-------------------'
            If ls_connection Is Nothing Then
                c.INITIALIZE_INI()
            End If
            db.ConnectDB(ls_connection)             '--------------7152010-------------------'
            rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

            If rdr.Read Then            'rdr.close - x
                rBC = Trim(rdr(0))
                rBCnm = Trim(rdr(1))
                rdr.Close()
                db.CloseConnection()

                grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                Application.DoEvents()


                sqltxt = "SELECT * FROM wu_kebot " & _
                        "WHERE (BCODE = '" & Trim(rBC) & _
                         "' AND (MONTH_ERN = " & cboPeriod.Text & _
                         " AND YEAR_ERN = " & Format(g_month, "yyyy").ToString & "))"

                If db.isConnected Then                  '--------------7152010-------------------'
                    db.CloseConnection()                '--------------7152010-------------------'
                End If                                  '--------------7152010-------------------'
                If ls_connection Is Nothing Then
                    c.INITIALIZE_INI()
                End If
                db.ConnectDB(ls_connection)             '--------------7152010-------------------'
                rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

                If rdr.Read Then                            'rdr.close - ok
                    Dim ls_rep As MsgBoxResult
                    Dim ls_de As String = "Branch code " & rBC & " already actioned last " & _
                    Format(rdr(3), "yyyy-MM-dd") & "." & vbCrLf & _
                    "Do you want to go forward with another branch code?" & vbCrLf & _
                    vbCrLf & "Yes - proceed to the next branch code." & vbCrLf & _
                    "No  - Cancel/Stop all transactions."


                    ls_rep = MsgBox(ls_de, MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton1, "REPETITION")

                    If ls_rep = MsgBoxResult.Yes Then
                        rdr.Close()
                        db.CloseConnection()
                        GoTo extloop

                    ElseIf ls_rep = MsgBoxResult.No Then
                        rdr.Close()
                        db.CloseConnection()
                        GoTo Extsub
                    End If

                End If
                rdr.Close()
                db.CloseConnection()

                sqltxt = "select top 1 newid()"
                If c.Error_SetRdr(sqltxt, rdr, sqlmsg) Then
                    f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errLog)
                    GoTo extsub
                End If

                rdr.Read()
                Entryguid = rdr(0).ToString
                rdr.Close()
                db.CloseConnection()

                sqltxt = "select top 1 number from numbers where companycode = '" & rBC & _
                         "' and used = 0 order by number asc"

                If db.isConnected Then                  '--------------7152010-------------------'
                    db.CloseConnection()                '--------------7152010-------------------'
                End If                                  '--------------7152010-------------------'
                If ls_connection Is Nothing Then
                    c.INITIALIZE_INI()
                End If
                db.ConnectDB(ls_connection)             '--------------7152010-------------------'
                rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

                If rdr.Read Then
                    rFaktuurnr = rdr(0)
                Else
                    eMsg = ". Error in reading numbers table."
                    f.Emsg(eMsg, "Branch Code: " & rBC)
                    'f.Error_Log(rBC & eMsg, g_errLog)
                    rdr.Close()
                    db.CloseConnection()
                    GoTo Extsub
                End If
                rdr.Close()
                db.CloseConnection()

                If Not nFn Then
                    sqltxt = "select freenumber from bacofreenumbers " & _
                             "where numberkey = 'rpfinentry'"
                    If db.isConnected Then                  '--------------7152010-------------------'
                        db.CloseConnection()                '--------------7152010-------------------'
                    End If                                  '--------------7152010-------------------'
                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If
                    db.ConnectDB(ls_connection)             '--------------7152010-------------------'
                    rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

                    If rdr.Read Then
                        fn = rdr(0)
                        nFn = True
                    Else
                        eMsg = "Error in reading bacofreenumbers table"
                        f.Emsg(eMsg, "bacofreenumbers")
                        f.Error_Log(eMsg, g_errLog)
                        rdr.Close()
                        db.CloseConnection()
                        GoTo Extsub
                    End If
                    rdr.Close()
                    db.CloseConnection()

                End If
                fn = fn + 1
                rEntryNumber = fn
                ctrFn = ctrFn + 1
                ''alter starts


                For r = 1 To 56
                    Dim amount As Double = Nothing
                    Dim acfc As Double = Nothing
                    Dim cccc As String = Nothing
                    Select Case r
                        Case 1
                            amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 55)))
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then

                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 56)))
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 57)))
                            Else
                                GoTo extcase
                            End If
                        Case 4
                            amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 58)))

                            Else
                                GoTo extcase
                            End If
                        Case 5
                            amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 59)))
                            Else
                                GoTo extcase
                            End If
                        Case 6
                            amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 60)))

                            Else
                                GoTo extcase
                            End If
                        Case 7
                            amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 61)))

                            Else
                                GoTo extcase
                            End If
                        Case 8
                            amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 62)))

                            Else
                                GoTo extcase
                            End If
                        Case 9
                            amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 63)))

                            Else
                                GoTo extcase
                            End If
                        Case 10
                            amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 64)))

                            Else
                                GoTo extcase
                            End If
                        Case 11
                            amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 65)))

                            Else
                                GoTo extcase
                            End If
                        Case 12
                            amount = Format(grd.get_TextMatrix(n, 13)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 66)))

                            Else
                                GoTo extcase
                            End If
                        Case 13
                            amount = Format(grd.get_TextMatrix(n, 14)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 67)))

                            Else
                                GoTo extcase
                            End If
                        Case 14
                            amount = Format(grd.get_TextMatrix(n, 15)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 68)))

                            Else
                                GoTo extcase
                            End If
                        Case 15
                            amount = Format(grd.get_TextMatrix(n, 16)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 69)))

                            Else
                                GoTo extcase
                            End If
                        Case 16
                            amount = Format(grd.get_TextMatrix(n, 17)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 70)))

                            Else
                                GoTo extcase
                            End If
                        Case 17
                            amount = Format(grd.get_TextMatrix(n, 18)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 71)))

                            Else
                                GoTo extcase
                            End If
                        Case 18
                            amount = Format(grd.get_TextMatrix(n, 19)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 72)))

                            Else
                                GoTo extcase
                            End If
                        Case 19
                            amount = Format(grd.get_TextMatrix(n, 20)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 73)))

                            Else
                                GoTo extcase
                            End If
                        Case 20
                            amount = Format(grd.get_TextMatrix(n, 21)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 74)))

                            Else
                                GoTo extcase
                            End If
                        Case 21
                            amount = Format(grd.get_TextMatrix(n, 22)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 75)))

                            Else
                                GoTo extcase
                            End If
                        Case 22
                            amount = Format(grd.get_TextMatrix(n, 23)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 76)))

                            Else
                                GoTo extcase
                            End If
                        Case 23
                            amount = Format(grd.get_TextMatrix(n, 24)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 77)))

                            Else
                                GoTo extcase
                            End If
                        Case 24
                            amount = Format(grd.get_TextMatrix(n, 25)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 78)))

                            Else
                                GoTo extcase
                            End If
                        Case 25
                            amount = Format(grd.get_TextMatrix(n, 26)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 79)))

                            Else
                                GoTo extcase
                            End If
                        Case 26
                            amount = Format(grd.get_TextMatrix(n, 27)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 80)))

                            Else
                                GoTo extcase
                            End If
                        Case 27
                            amount = Format(grd.get_TextMatrix(n, 28)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 81)))

                            Else
                                GoTo extcase
                            End If
                        Case 28
                            amount = Format(grd.get_TextMatrix(n, 29)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 82)))

                            Else
                                GoTo extcase
                            End If
                        Case 29
                            amount = Format(grd.get_TextMatrix(n, 30)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 83)))

                            Else
                                GoTo extcase
                            End If
                        Case 30
                            amount = Format(grd.get_TextMatrix(n, 31)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 84)))

                            Else
                                GoTo extcase
                            End If
                        Case 31
                            amount = Format(grd.get_TextMatrix(n, 32)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 85)))

                            Else
                                GoTo extcase
                            End If
                        Case 32
                            amount = Format(grd.get_TextMatrix(n, 33)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 86)))

                            Else
                                GoTo extcase
                            End If
                        Case 33
                            amount = Format(grd.get_TextMatrix(n, 34)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 87)))

                            Else
                                GoTo extcase
                            End If
                        Case 34
                            amount = Format(grd.get_TextMatrix(n, 35)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 88)))

                            Else
                                GoTo extcase
                            End If
                        Case 35
                            amount = Format(grd.get_TextMatrix(n, 36)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 89)))

                            Else
                                GoTo extcase
                            End If
                        Case 36
                            amount = Format(grd.get_TextMatrix(n, 37)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 90)))

                            Else
                                GoTo extcase
                            End If
                        Case 37
                            amount = Format(grd.get_TextMatrix(n, 38)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 91)))

                            Else
                                GoTo extcase
                            End If
                        Case 38
                            amount = Format(grd.get_TextMatrix(n, 39)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 92)))

                            Else
                                GoTo extcase
                            End If
                        Case 39
                            amount = Format(grd.get_TextMatrix(n, 40)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 93)))

                            Else
                                GoTo extcase
                            End If
                        Case 40
                            amount = Format(grd.get_TextMatrix(n, 41)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 94)))

                            Else
                                GoTo extcase
                            End If
                        Case 41
                            amount = Format(grd.get_TextMatrix(n, 42)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 95)))

                            Else
                                GoTo extcase
                            End If
                        Case 42
                            amount = Format(grd.get_TextMatrix(n, 43)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 96)))

                            Else
                                GoTo extcase
                            End If
                        Case 43
                            amount = Format(grd.get_TextMatrix(n, 44)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 97)))

                            Else
                                GoTo extcase
                            End If
                        Case 44
                            amount = Format(grd.get_TextMatrix(n, 45)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 98)))

                            Else
                                GoTo extcase
                            End If
                        Case 45
                            amount = Format(grd.get_TextMatrix(n, 46)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 99)))

                            Else
                                GoTo extcase
                            End If
                        Case 46
                            amount = Format(grd.get_TextMatrix(n, 47)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 100)))

                            Else
                                GoTo extcase
                            End If
                        Case 47
                            amount = Format(grd.get_TextMatrix(n, 48)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 101)))

                            Else
                                GoTo extcase
                            End If
                        Case 48
                            amount = Format(grd.get_TextMatrix(n, 49)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 102)))

                            Else
                                GoTo extcase
                            End If
                        Case 49
                            amount = Format(grd.get_TextMatrix(n, 50)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 103)))

                            Else
                                GoTo extcase
                            End If
                        Case 50
                            amount = Format(grd.get_TextMatrix(n, 51)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 104)))

                            Else
                                GoTo extcase
                            End If
                        Case 51
                            amount = Format(grd.get_TextMatrix(n, 52)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 105)))
                            Else
                                GoTo extcase
                            End If
                        Case 52
                            amount = Format(grd.get_TextMatrix(n, 53)) 'new eli
                            If amount <> 0 Then
                                acfc = 0

                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 106)))
                            Else
                                GoTo extcase
                            End If
                            '/-----------------End of Debit \
                        Case 53
                            amount = Format(grd.get_TextMatrix(n, 54)) 'new eli
                            If amount <> 0 Then
                                acfc = 0

                                '/--------SG OCTOBER 1 - 15 , 2008\
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 107)))
                            Else
                                GoTo extcase
                            End If

                        Case 54
                            'FOR DRIVERS
                            amount = Format(grd.get_TextMatrix(n, 108))
                            'amount = Format(grd.get_TextMatrix(n, 112))
                            If amount <> 0 Then
                                acfc = 0
                                '/--------SG OCTOBER 1 - 15 , 2008 company driver\
                                descr = txtDesc.Text & " " & "Security Guard Base" & " " & " Company Driver"
                                '\--------SG OCTOBER 1 - 15 , 2008 company driver/
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 110)))

                            Else
                                GoTo extcase
                            End If
                            '\-----------------End of debit/
                        Case 55
                            'FOR DRIVERS
                            acfc = Format(grd.get_TextMatrix(n, 109))
                            'acfc = Format(grd.get_TextMatrix(n, 113))
                            If acfc <> 0.0 Then
                                amount = 0
                                descr = txtDesc.Text & " " & "Security Guard Base" & " " & " Company Driver"
                                CompanyAccountCode = "2070103"
                                'cccc = Format(grd.get_TextMatrix(n, 110))
                            Else
                                GoTo extcase
                            End If
                        Case 56
                            acfc = Format(grd.get_TextMatrix(n, 111))
                            'acfc = Format(grd.get_TextMatrix(n, 115))
                            If acfc <> 0 Then
                                amount = 0
                                '/--------SG OCTOBER 1 - 15 , 2008
                                descr = txtDesc.Text & " " & "Security Guard Base"
                                '\--------SG OCTOBER 1 - 15 , 2008 
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 112)))
                            Else
                                GoTo extcase
                            End If
                    End Select
                    ' MsgBox(totalAmount)
                    CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
                    rFaktuurnr = Space(8 - rFaktuurnr.Length) & rFaktuurnr
                    Dim Looponce As Integer
                    Dim BatchN As String
                    If Looponce = 0 Then
                        BatchN = Entryguid
                        Looponce = Looponce + 1
                    End If
                    sqltxt = "INSERT INTO " & database & ".[dbo].[TransactionsPending]([paymentmethod],[freefield4],[freefield5], [CompanyCode], [TransactionType], [TransactionDate]," & _
                                                "[FinYear],[FinPeriod], [Description], [CompanyAccountCode],[EntryNumber],[CurrencyAliasAC],[AmountDebitAC],[AmountCreditAC]," & _
                                                "[CurrencyAliasFC],[AmountDebitFC],[AmountCreditFC],[VATCode],[ProcessNumber],[ProcessLineCode], [res_id], [oorsprong],[docdate]," & _
                                                "[vervdatfak],[faktuurnr],[syscreator],[sysmodifier],[EntryGuid],[companycostcentercode],[BatchNo])" & _
                                                "values(null,'0.0','0.0','" & Trim(rBC) & "'," & 5 & ",'" & Trim(Format(g_month, "yyyy") & "-" & Format(g_month, "MM") & "-" & g_month.DaysInMonth(Format(g_month, "yyyy"), Format(g_month, "MM"))) & "'," & _
                                                Format(g_month, "yyyy") & "," & Format(g_month, "MM") & ",'" & Trim(descr) & "','" & CompanyAccountCode & "','" & Trim(rEntryNumber) & "','PHP'," & amount & _
                                                "," & acfc & ",'PHP'," & amount & "," & acfc & ",0, 1, 'B', 0, 'Z','" & Format(D.Now, "yyyy-MM-dd") & "','" & Format(g_month, "yyyy-MM-dd") & "','" & rFaktuurnr & "','" _
                                               & SysCreator & "','" & SysModifier & "','" & Entryguid & "', '" & cccc & "' , '" & BatchN & "')"

                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                                                              '--------------7152010-------------------'
                    db.ConnectDB(ls_connection)                                                     '--------------7152010-------------------'
                    If db.Execute_SQLQuery(sqltxt) < 1 Then                                         '--------------7152010-------------------'
                        db.RollbackTransaction()                                                    '--------------7152010-------------------'
                        db.CloseConnection()                                                        '--------------7152010-------------------'
                        MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)    '--------------7152010-------------------'
                        Exit Sub                                                                    '--------------7152010-------------------'
                    End If                                                                          '--------------7152010-------------------'
                    db.CloseConnection()
extcase:
                Next r 'alter ends'

                sqltxt = "Insert into basecorpsg1_corp(bcode,paydesc,month_eli, year_eli, " & _
                         "date_time, sys_creator) values('" & Trim(rBC) & _
                         "','" + txtDesc.Text + "','" & Format(g_month, "MM").ToString & "', '" & _
                         Format(g_month, "yyyy") & "', '" & _
                         Format(D.Now, "yyyy-MM-dd") & "', '" & _
                         SysCreator & "')"
                Log("Success insertion " + Me.Text + "--->'" + Trim(rBC) + "','" & Format(g_month, "MM").ToString & "','" & Format(g_month, "yyyy").ToString & "'," & Date.Now & ",'" & SysCreator & "'")
                If ls_connection Is Nothing Then
                    c.INITIALIZE_INI()
                End If                                                              '--------------7152010-------------------'
                db.ConnectDB(ls_connection)                                                     '--------------7152010-------------------'
                If db.Execute_SQLQuery(sqltxt) < 1 Then                                         '--------------7152010-------------------'
                    db.RollbackTransaction()                                                    '--------------7152010-------------------'
                    db.CloseConnection()                                                        '--------------7152010-------------------'
                    MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)    '--------------7152010-------------------'
                    Exit Sub                                                                    '--------------7152010-------------------'
                End If                                                                          '--------------7152010-------------------'
                db.CloseConnection()
                'Call SaveToOtherDataBase()

                If n = grd.Rows - 2 Then
                    sqltxt = "update numbers " & _
                             "set used = 1 , Synergy_Status = '2' " & _
                             "where number = (select top 1 number from numbers " & _
                             "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                             "and companycode = '" & rBC & "'"
                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                              '--------------7152010-------------------'
                    db.ConnectDB(ls_connection)                     '--------------7152010-------------------'
                    db.Execute_SQLQuery(sqltxt)                     '--------------7152010-------------------'
                    db.CloseConnection()                            '--------------7152010-------------------'

                    sqltxt = "update bacofreenumbers set " & _
                             "Synergy_Status = '2' , freenumber = freenumber + " & ctrFn & _
                            " where numberkey = 'rpfinentry'"


                    '-------->New Eli Code
                    Dim settrue As Boolean

                    If cmbSt.Text = "True" Then
                        settrue = True
                    Else
                        settrue = False
                    End If

                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                              '--------------7152010-------------------'
                    db.ConnectDB(ls_connection)                     '--------------7152010-------------------'
                    db.Execute_SQLQuery(sqltxt)                     '--------------7152010-------------------'
                    db.CloseConnection()                            '--------------7152010-------------------'
                    '<--------New Eli Code



                    'If c.Error_SetComm(sqltxt, False, sqlmsg) Then              'true = to write (dont erase)
                    '    f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errlog)
                    '    GoTo Extsub
                    'End If

                Else

                    sqltxt = "update numbers " & _
                            "set used = 1 , Synergy_Status = '2' " & _
                            "where number = (select top 1 number from numbers " & _
                            "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                            "and companycode = '" & rBC & "'"
                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                              '--------------7152010-------------------'
                    db.ConnectDB(ls_connection)                     '--------------7152010-------------------'
                    db.Execute_SQLQuery(sqltxt)                     '--------------7152010-------------------'
                    db.CloseConnection()                            '--------------7152010-------------------'

                End If

            Else
                rdr.Close()
                c.DisposeR()
                sqltxt = "Insert into wu_unknownbc(bcode, dr1, dr2) values('" & _
                                 bcode & "'," & Format(CDbl(grd.get_TextMatrix(n, 3)), "##0.00") & ", " & _
                                  Format(CDbl(grd.get_TextMatrix(n, 4)), "##0.00") & ")"
                If ls_connection Is Nothing Then
                    c.INITIALIZE_INI()
                End If
                db.ConnectDB(ls_connection)
                If db.Execute_SQLQuery(sqltxt) < 1 Then
                    db.RollbackTransaction()
                    db.CloseConnection()
                    MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)
                    Exit Sub
                End If
                db.CloseConnection()

                f.Error_Log(grd.get_TextMatrix(n, 1) & " - " & grd.get_TextMatrix(n, 4) & " - " & _
                            grd.get_TextMatrix(n, 5) & " - " & grd.get_TextMatrix(n, 6), g_errBC)
                errCtr += 1
                strBC = strBC + "." + bcode
            End If

extloop:
            If grd.Row = 1 Then
                'pb.Value = ((n / (grd.Rows - 2)) * 100)
            Else
                pb.Value = ((n / (grd.Rows - 2)) * 100)
            End If
        Next

        '/4-23-2012
        Dim task As String = Nothing
        Dim name As String = Nothing
        Dim adz As New clsdatalog

        Dim q As String = "select department,fullname from user_profile where userid = '" + ls_userid + "'"
        Dim RDRxx As SqlClient.SqlDataReader = Nothing
        If Not adz.clsConnect Then Exit Sub
        If adz.ErrorConnectionReading(False) Then Exit Sub
        If Not adz.dr(q, RDRxx) Then Exit Sub
        If RDRxx.Read Then
            task = Trim(RDRxx(0).ToString)
            name = Trim(RDRxx(1).ToString)
        End If
        rdr.Close()
        adz.dispose()
        '\4-23-2012

        Dim cls As New LogsEDIMIN
        Dim rdr1 As SqlClient.SqlDataReader = Nothing
        Dim act As String = "EDI" & " " & "BASE SG" & " " & "for" & " " & txtDesc.Text.Trim
        Dim syndev As String = "insert into edi_maintainance_logs (datetimelog, application, activity, resource, department, remarks)values" & _
                               "('" + datenow + "', 'EDI', '" + act + "', '" & name & "', '" & task & "', 'DONE')"

        If cls.Error_Inititalize_INI() Then Exit Sub
        If cls.ErrorConnectionReading(False) Then Exit Sub
        If Not cls.dr(syndev, rdr1) Then Exit Sub

        Call CheckBC_Errors()

Extsub:

        c.DisposeR()
        c.DisposeW()
        c = Nothing
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
            MsgBox("Successfuly saved all branches.", MsgBoxStyle.Information, "EDI - Mindanao")
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
        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader
        Dim n As Integer = Nothing
        Dim month As String
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)
            Dim s As String = "Select * from basecorpsg1_corp where month_eli = '" & cboPeriod.Text & "' and year_eli = '" & cboYear.Text & "' and bcode = '" & g & "' and paydesc = '" + txtDesc.Text + "'"
            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function
            If Not c.Error_SetRdr(s, rdr, sqlmsg) Then
                While rdr.Read
                    alreadysave = True
                    Exit Function
                End While
                rdr.Close()
            End If
        Next
        alreadysave = False
    End Function
    Private Function missingbranch() As Boolean
        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim n As Integer
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)
            Dim a As String

            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function

            If Len(g).ToString < 3 Then
                Dim bcode As String = "0" & (g)
                sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"

            Else
                Dim bcode As String = (g)
                sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"
            End If

            If Len(g).ToString < 2 Then
                Dim bcode As String = "00" & (g)
                sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"
            End If

            If Not c.Error_SetRdr(sqltxt, rdr, sqlmsg) Then
                If rdr.Read Then
                    a = rdr(0)
                Else
                    MsgBox(g & " " & "Missing Branch")
                    missingbranch = True
                End If
            End If
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

    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Dim main As New Main
        main.Show()
        Me.Hide()
    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        Label7.Text = txtDesc.Text & " " & "Security Guard Base Branch"
    End Sub
End Class