Public Class EDIVISAYAS_PAYROLLBB
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

    Private Sub EDIVISMIN_PAYROLLBASEBRANCH_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim main As New Main
        Me.Hide()
        main.Show()
    End Sub
    Private Sub EDIVISMIN_PAYROLLBASEBRANCH_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Payroll-" & gs_serverloc2 & " " & gs_version
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
    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

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
    Private Sub cboSheet_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
            MsgBox("START ROW NOT FILLED", 64, "Mid Year -Visayas")
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
    Private Function alreadysave() As Boolean
        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader
        Dim n As Integer = Nothing
        Dim month As String

        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)
            Dim paydesc As String = cmbdesc.Text
            Dim squery As String = "Select * from basecorpallo_corp where paydesc = '" & paydesc & "' and month_eli = '" & cboPeriod.Text & "' and year_eli = '" & cboYear.Text & "' and bcode = '" & g & "'  "
            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function
            If Not c.Error_SetRdr(squery, rdr, sqlmsg) Then
                While rdr.Read
                    MsgBox("This data is Already save Last " & month & " " & rdr(3) & " this Process will be aborted", 48, "Data Exits")
                    alreadysave = True
                    Exit Function
                End While
                rdr.Close()
                c.DisposeR()
            End If
        Next
    End Function
    Private Function balancedata() As Boolean
        Dim n As Integer
        Dim total As Double = Nothing
        Dim r As Integer = Nothing
        Dim total_debit As Double
        Dim total_credit As Double

        For n = 0 To grd.Rows - 2
            For r = 1 To 112
                Dim amount As Double = Nothing 'debit 
                Dim acfc As Double = Nothing   'credit
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
                    Case 53
                        amount = Format(grd.get_TextMatrix(n, 54)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 54
                        amount = Format(grd.get_TextMatrix(n, 55)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 55
                        amount = Format(grd.get_TextMatrix(n, 56)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 56
                        amount = Format(grd.get_TextMatrix(n, 57)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 57
                        amount = Format(grd.get_TextMatrix(n, 58)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 58
                        amount = Format(grd.get_TextMatrix(n, 59)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 59
                        amount = Format(grd.get_TextMatrix(n, 60)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 60
                        amount = Format(grd.get_TextMatrix(n, 61)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 61
                        amount = Format(grd.get_TextMatrix(n, 122)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If

                    Case 62
                        amount = Format(grd.get_TextMatrix(n, 123)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase

                        End If
                    Case 63
                        amount = Format(grd.get_TextMatrix(n, 124)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 64
                        amount = Format(grd.get_TextMatrix(n, 125)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 65
                        amount = Format(grd.get_TextMatrix(n, 126)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 66
                        amount = Format(grd.get_TextMatrix(n, 127)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 67
                        amount = Format(grd.get_TextMatrix(n, 128)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 68
                        amount = Format(grd.get_TextMatrix(n, 129)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 69
                        amount = Format(grd.get_TextMatrix(n, 130)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 70
                        amount = Format(grd.get_TextMatrix(n, 131)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 71
                        amount = Format(grd.get_TextMatrix(n, 132)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 72
                        amount = Format(grd.get_TextMatrix(n, 133)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 73
                        amount = Format(grd.get_TextMatrix(n, 134)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 74
                        amount = Format(grd.get_TextMatrix(n, 135)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 75
                        amount = Format(grd.get_TextMatrix(n, 136)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 76
                        amount = Format(grd.get_TextMatrix(n, 137)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 77
                        amount = Format(grd.get_TextMatrix(n, 138)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                        '/-----------------------Last Entry For Debit\
                    Case 78
                        amount = Format(grd.get_TextMatrix(n, 139)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                        '\------------------------Last Entry for Debit/
                    Case 79
                        '/------------------------Start at EI\
                        amount = Format(grd.get_TextMatrix(n, 140)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                        '\-------------------------Start at EI/
                    Case 80
                        acfc = Format(grd.get_TextMatrix(n, 141)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 81
                        acfc = Format(grd.get_TextMatrix(n, 142)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 82
                        acfc = Format(grd.get_TextMatrix(n, 143)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 83
                        acfc = Format(grd.get_TextMatrix(n, 144)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                        '/--------------------Last Entry of EI\
                    Case 84
                        acfc = Format(grd.get_TextMatrix(n, 145)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                        '\--------------------Last Entry of EI/
                    Case 85
                        acfc = Format(grd.get_TextMatrix(n, 146)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 86
                        acfc = Format(grd.get_TextMatrix(n, 147)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 87
                        acfc = Format(grd.get_TextMatrix(n, 148)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 88
                        acfc = Format(grd.get_TextMatrix(n, 149)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 89
                        acfc = Format(grd.get_TextMatrix(n, 150)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 90
                        acfc = Format(grd.get_TextMatrix(n, 151)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 91
                        acfc = Format(grd.get_TextMatrix(n, 152)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 92
                        acfc = Format(grd.get_TextMatrix(n, 153)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 93
                        acfc = Format(grd.get_TextMatrix(n, 154)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 94
                        acfc = Format(grd.get_TextMatrix(n, 155)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 95
                        acfc = Format(grd.get_TextMatrix(n, 156)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 96
                        acfc = Format(grd.get_TextMatrix(n, 157)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 97
                        acfc = Format(grd.get_TextMatrix(n, 158)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 98
                        acfc = Format(grd.get_TextMatrix(n, 159)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 99
                        acfc = Format(grd.get_TextMatrix(n, 160)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 100
                        acfc = Format(grd.get_TextMatrix(n, 161)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 101
                        acfc = Format(grd.get_TextMatrix(n, 162)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If

                    Case 102
                        acfc = Format(grd.get_TextMatrix(n, 163)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 103
                        acfc = Format(grd.get_TextMatrix(n, 164)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 104
                        acfc = Format(grd.get_TextMatrix(n, 165)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 105
                        acfc = Format(grd.get_TextMatrix(n, 166)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 106
                        acfc = Format(grd.get_TextMatrix(n, 167)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 107
                        acfc = Format(grd.get_TextMatrix(n, 168)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 108
                        acfc = Format(grd.get_TextMatrix(n, 169)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 109
                        acfc = Format(grd.get_TextMatrix(n, 170)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 110
                        acfc = Format(grd.get_TextMatrix(n, 171)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 111
                        acfc = Format(grd.get_TextMatrix(n, 172)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 112
                        acfc = Format(grd.get_TextMatrix(n, 173)) 'new eli
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
                If r = 112 Then
                    d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                    c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                    If r = 112 And d <> c Then
                        Dim sqq1 As String = "There's an entry in " & _
                                             grd.get_TextMatrix(n, 0) & " " & grd.get_TextMatrix(n, 1) & _
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
            MsgBox("Please fill description text box", 16, "Synergy EDI Visayas")
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

        If alreadysave() = True Then
            Dim li_input As Integer = Nothing
            li_input = MsgBox("This data is already been saved, Would you like to save it anyway?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information)
            If li_input = vbYes Then
                GoTo continuesave
            Else
                GoTo ext
            End If
        End If

continuesave:

        If balancedata() = True Then
            btnSave.Hide()
            GoTo Ext
        End If
        '\----------/
        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data")
            Exit Sub
        End If
        _refHC(btnEnter)

        Dim c As New ClsDataEDIVISAYAS
        Dim rdr As SqlClient.SqlDataReader



        sqltxt = "select * from basepayroll_corp where bcode = 000 and date_time > '" & _
                          Format(g_month, "yyyy-MM-dd") & "'"

        If db.isConnected Then                  '--------------7152010-------------------'
            db.CloseConnection()                '--------------7152010-------------------'
        End If                                  '--------------7152010-------------------'
        If ls_connection is Nothing then 
 c.INITIALIZE_INI() 
        End If                      '--------------7152010-------------------'
        db.ConnectDB(ls_connection)             '--------------7152010-------------------'
        rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

        rdr.Close()
        db.CloseConnection()


        f.Grp_Visible(grp, pb, "Now saving data...")


        For n = 0 To grd.Rows - 2

            Dim totalAmount As Double = Nothing
            Dim totalCredit As Double = Nothing
            Dim totalDebit As Double = Nothing

            Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 0)), "000")
            sqltxt = "select bedrnr, bedrnm from BEDRYF where bedrnr = '" & _
                      bcode & "'"
            If db.isConnected Then                  '--------------7152010-------------------'
                db.CloseConnection()                '--------------7152010-------------------'
            End If                                  '--------------7152010-------------------'
            If ls_connection Is Nothing Then
                c.INITIALIZE_INI()
            End If                      '--------------7152010-------------------'
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
                End If                      '--------------7152010-------------------'
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
                If db.isConnected Then                  '--------------7152010-------------------'
                    db.CloseConnection()                '--------------7152010-------------------'
                End If                                  '--------------7152010-------------------'
                If ls_connection Is Nothing Then
                    c.INITIALIZE_INI()
                End If                      '--------------7152010-------------------'
                db.ConnectDB(ls_connection)             '--------------7152010-------------------'
                rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7152010-------------------'

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
                End If                      '--------------7152010-------------------'
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
                    End If                      '--------------7152010-------------------'
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

                For r = 1 To 112
                    Dim amount As Double = Nothing
                    Dim acfc As Double = Nothing
                    Dim cccc As String = Nothing
                    Select Case r
                        Case 1
                            amount = Format(grd.get_TextMatrix(n, 2)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 62)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 2
                            amount = Format(grd.get_TextMatrix(n, 3)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 63)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 3
                            amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 64)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 4
                            amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 65)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase

                            End If
                        Case 5
                            amount = Format(grd.get_TextMatrix(n, 6)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 66)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 6
                            amount = Format(grd.get_TextMatrix(n, 7)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 67)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 7
                            amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 68)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 8
                            amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 69)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 9
                            amount = Format(grd.get_TextMatrix(n, 10)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 70)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 10
                            amount = Format(grd.get_TextMatrix(n, 11)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 71)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 11
                            amount = Format(grd.get_TextMatrix(n, 12)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 72)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 12
                            amount = Format(grd.get_TextMatrix(n, 13)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 73)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 13
                            amount = Format(grd.get_TextMatrix(n, 14)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 74)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 14
                            amount = Format(grd.get_TextMatrix(n, 15)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 75)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 15
                            amount = Format(grd.get_TextMatrix(n, 16)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 76)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 16
                            amount = Format(grd.get_TextMatrix(n, 17)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 77)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 17
                            amount = Format(grd.get_TextMatrix(n, 18)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 78)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 18
                            amount = Format(grd.get_TextMatrix(n, 19)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 79)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 19
                            amount = Format(grd.get_TextMatrix(n, 20)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 80)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 20
                            amount = Format(grd.get_TextMatrix(n, 21)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 81)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase

                            End If
                        Case 21
                            amount = Format(grd.get_TextMatrix(n, 22)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 82)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 22
                            amount = Format(grd.get_TextMatrix(n, 23)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 83)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 23
                            amount = Format(grd.get_TextMatrix(n, 24)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 84)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 24
                            amount = Format(grd.get_TextMatrix(n, 25)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 85)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 25
                            amount = Format(grd.get_TextMatrix(n, 26)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 86)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 26
                            amount = Format(grd.get_TextMatrix(n, 27)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 87)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 27
                            amount = Format(grd.get_TextMatrix(n, 28)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 88)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 28
                            amount = Format(grd.get_TextMatrix(n, 29)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 89)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 29
                            amount = Format(grd.get_TextMatrix(n, 30)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 90)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 30
                            amount = Format(grd.get_TextMatrix(n, 31)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 91)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 31
                            amount = Format(grd.get_TextMatrix(n, 32)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 92)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 32
                            amount = Format(grd.get_TextMatrix(n, 33)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 93)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 33
                            amount = Format(grd.get_TextMatrix(n, 34)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 94)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 34
                            amount = Format(grd.get_TextMatrix(n, 35)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 95)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 35
                            amount = Format(grd.get_TextMatrix(n, 36)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 96)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 36
                            amount = Format(grd.get_TextMatrix(n, 37)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 97)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase

                            End If
                        Case 37
                            amount = Format(grd.get_TextMatrix(n, 38)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 98)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 38
                            amount = Format(grd.get_TextMatrix(n, 39)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 99)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 39
                            amount = Format(grd.get_TextMatrix(n, 40)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 100)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 40
                            amount = Format(grd.get_TextMatrix(n, 41)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 101)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 41
                            amount = Format(grd.get_TextMatrix(n, 42)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 102)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 42
                            amount = Format(grd.get_TextMatrix(n, 43)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 103)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 43
                            amount = Format(grd.get_TextMatrix(n, 44)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 104)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 44
                            amount = Format(grd.get_TextMatrix(n, 45)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 105)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 45
                            amount = Format(grd.get_TextMatrix(n, 46)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 106)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 46
                            amount = Format(grd.get_TextMatrix(n, 47)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 107)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 47
                            amount = Format(grd.get_TextMatrix(n, 48)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 108)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 48
                            amount = Format(grd.get_TextMatrix(n, 49)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 109)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If

                        Case 49
                            amount = Format(grd.get_TextMatrix(n, 50)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 110)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 50
                            amount = Format(grd.get_TextMatrix(n, 51)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 111)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 51
                            amount = Format(grd.get_TextMatrix(n, 52)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 112)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 52
                            amount = Format(grd.get_TextMatrix(n, 53)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 113)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 53
                            amount = Format(grd.get_TextMatrix(n, 54)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 114)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 54
                            amount = Format(grd.get_TextMatrix(n, 55)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 115)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 55
                            amount = Format(grd.get_TextMatrix(n, 56)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 116)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 56
                            amount = Format(grd.get_TextMatrix(n, 57)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 117)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 57
                            amount = Format(grd.get_TextMatrix(n, 58)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 118)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 58
                            amount = Format(grd.get_TextMatrix(n, 59)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                cccc = Trim(Format(grd.get_TextMatrix(n, 119)))
                                CompanyAccountCode = "3100001"
                            Else
                                GoTo extcase
                            End If
                        Case 59
                            amount = Format(grd.get_TextMatrix(n, 60)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 120)))
                            Else
                                GoTo extcase
                            End If
                        Case 60
                            amount = Format(grd.get_TextMatrix(n, 61)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "PYRL" & " " & txtDesc.Text & " " & cmbdesc.Text
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 121)))
                            Else
                                GoTo extcase
                            End If
                        Case 61
                            amount = Format(grd.get_TextMatrix(n, 122)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD SAL" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD Salaries"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase
                            End If

                        Case 62
                            amount = Format(grd.get_TextMatrix(n, 123)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD ALLOW" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD Allow"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase

                            End If
                        Case 63
                            amount = Format(grd.get_TextMatrix(n, 124)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD OT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD OT"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase
                            End If
                        Case 64
                            amount = Format(grd.get_TextMatrix(n, 125)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD COLA" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD COLA"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 65
                            amount = Format(grd.get_TextMatrix(n, 126)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD PB/EXCESS" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD PB/Excess PB"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 66
                            amount = Format(grd.get_TextMatrix(n, 127)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD SAL ADJ" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD Sal Adj"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 67
                            amount = Format(grd.get_TextMatrix(n, 128)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "IAD REC INC" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD Rec Inc"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 68
                            amount = Format(grd.get_TextMatrix(n, 129)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH SAL" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Sal"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 69
                            amount = Format(grd.get_TextMatrix(n, 130)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH ALLOW" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Allow"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 70
                            amount = Format(grd.get_TextMatrix(n, 131)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH OT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech OT"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 71
                            amount = Format(grd.get_TextMatrix(n, 132)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH COLA" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Cola"

                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 72
                            amount = Format(grd.get_TextMatrix(n, 133)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH PB EXCESS" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech pb/excess pb"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 73
                            amount = Format(grd.get_TextMatrix(n, 134)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH SAL ADJ" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Sal Adj"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 74
                            amount = Format(grd.get_TextMatrix(n, 135)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "TECH REC INC" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Rec Inc"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 75
                            amount = Format(grd.get_TextMatrix(n, 136)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "MESS SAL" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess Salaries"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 76
                            amount = Format(grd.get_TextMatrix(n, 137)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "MESS ALLOW" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess Allow"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                        Case 77
                            amount = Format(grd.get_TextMatrix(n, 138)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "MESS OT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess OT"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))

                            Else
                                GoTo extcase
                            End If
                            '/-----------------------Last Entry For Debit\
                        Case 78
                            amount = Format(grd.get_TextMatrix(n, 139)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "MESS PB/EXCESS PB" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess pb/Excess pb"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))
                            Else
                                GoTo extcase
                            End If
                            '\------------------------Last Entry for Debit/
                        Case 79
                            '/------------------------Start at EI\
                            amount = Format(grd.get_TextMatrix(n, 140)) 'new eli
                            If amount <> 0 Then
                                acfc = 0
                                descr = "MESS SAL ADJ" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess Sal Adj"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))
                            Else
                                GoTo extcase
                            End If
                            '\-------------------------Start at EI/
                        Case 80
                            acfc = Format(grd.get_TextMatrix(n, 141)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "MESS REC INC" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess Rec Inc"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase
                            End If
                        Case 81
                            acfc = Format(grd.get_TextMatrix(n, 142)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "IAD LATE" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD Late"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase
                            End If
                        Case 82
                            acfc = Format(grd.get_TextMatrix(n, 143)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "IAD REC LEAVE" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "IAD REC Leave"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase
                            End If
                        Case 83
                            acfc = Format(grd.get_TextMatrix(n, 144)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "TECH LATE" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Late"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))
                            Else
                                GoTo extcase
                            End If
                            '/--------------------Last Entry of EI\
                        Case 84
                            acfc = Format(grd.get_TextMatrix(n, 145)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "TECH LEAVE" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Tech Leave"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))
                            Else
                                GoTo extcase
                            End If
                            '\--------------------Last Entry of EI/
                        Case 85
                            acfc = Format(grd.get_TextMatrix(n, 146)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "MESS LATE" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess Late"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))
                            Else
                                GoTo extcase
                            End If
                        Case 86
                            acfc = Format(grd.get_TextMatrix(n, 147)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "MESS LEAVE" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Mess Leave"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 174)))
                            Else
                                GoTo extcase
                            End If
                        Case 87
                            acfc = Format(grd.get_TextMatrix(n, 148)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "WTAX" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "WTAX"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 88
                            acfc = Format(grd.get_TextMatrix(n, 149)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SSS CONT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "SSS Cont"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 89
                            acfc = Format(grd.get_TextMatrix(n, 150)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SSS LOAN" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "SSS Loan"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 90
                            acfc = Format(grd.get_TextMatrix(n, 151)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "PAG CONT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Pag Cont"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 91
                            acfc = Format(grd.get_TextMatrix(n, 152)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "PAG LOAN" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Pag Loan"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 92
                            acfc = Format(grd.get_TextMatrix(n, 153)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "COATED" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Coated"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 93
                            acfc = Format(grd.get_TextMatrix(n, 154)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "HMO" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "HMO"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 94
                            acfc = Format(grd.get_TextMatrix(n, 155)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "CANTEEN" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Canteen"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 95
                            acfc = Format(grd.get_TextMatrix(n, 156)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "DED 1" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "DED 1"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 96
                            acfc = Format(grd.get_TextMatrix(n, 157)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "DED 2" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "DED 2"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 97
                            acfc = Format(grd.get_TextMatrix(n, 158)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "ML FUND" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "ML Fund"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))
                            Else
                                GoTo extcase
                            End If
                        Case 98
                            acfc = Format(grd.get_TextMatrix(n, 159)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "OPEC" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "OPEC"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 99
                            acfc = Format(grd.get_TextMatrix(n, 160)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "OVER APP" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Over Appraisal"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 100
                            acfc = Format(grd.get_TextMatrix(n, 161)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "COOP RECLA" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Coop Recla"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 101
                            acfc = Format(grd.get_TextMatrix(n, 162)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "FILMA LENDING" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Filma Lending"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If

                        Case 102
                            acfc = Format(grd.get_TextMatrix(n, 163)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "INST ACCT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "INST ACCT"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 103
                            acfc = Format(grd.get_TextMatrix(n, 164)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "DALTAN TICKET" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Daltan Ticket"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 104
                            acfc = Format(grd.get_TextMatrix(n, 165)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SMART/PLDT" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Smart/PLDT"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 105
                            acfc = Format(grd.get_TextMatrix(n, 166)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO PROVI" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako Provi"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 106
                            acfc = Format(grd.get_TextMatrix(n, 167)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO COMM" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako Comm"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 107
                            acfc = Format(grd.get_TextMatrix(n, 168)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO PRIME" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako Prime"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 108
                            acfc = Format(grd.get_TextMatrix(n, 169)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO EMERG" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako Emerg"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 109
                            acfc = Format(grd.get_TextMatrix(n, 170)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO PCASH" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako PCash"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 110
                            acfc = Format(grd.get_TextMatrix(n, 171)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO CAPB" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako Capb"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 111
                            acfc = Format(grd.get_TextMatrix(n, 172)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "SAKO SAVINGS" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Sako Savings"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If
                        Case 112
                            acfc = Format(grd.get_TextMatrix(n, 173)) 'new eli
                            If acfc <> 0 Then
                                amount = 0
                                descr = "NET PAY" & " " & txtDesc.Text & " " & cmbdesc.Text & " " & "Net Pay"
                                CompanyAccountCode = "3100001"
                                cccc = Trim(Format(grd.get_TextMatrix(n, 175)))

                            Else
                                GoTo extcase
                            End If

                    End Select

                    totalDebit = (totalDebit + amount) - (totalCredit - acfc)
                    totalCredit = totalCredit + acfc
                    totalAmount = totalDebit - totalCredit
                    ' MsgBox(totalAmount)
                    CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
                    rFaktuurnr = Space(8 - rFaktuurnr.Length) & rFaktuurnr
                    Dim Looponce As Integer
                    Dim batchNo As String
                    If Looponce = 0 Then
                        batchNo = Entryguid
                        Looponce = Looponce + 1
                    End If

                    sqltxt = "INSERT INTO " & database & ".[dbo].[TransactionsPending]([paymentmethod],[freefield4],[freefield5], [CompanyCode], [TransactionType], [TransactionDate]," & _
                                                "[FinYear],[FinPeriod], [Description], [CompanyAccountCode],[EntryNumber],[CurrencyAliasAC],[AmountDebitAC],[AmountCreditAC]," & _
                                                "[CurrencyAliasFC],[AmountDebitFC],[AmountCreditFC],[VATCode],[ProcessNumber],[ProcessLineCode], [res_id], [oorsprong],[docdate]," & _
                                                "[vervdatfak],[faktuurnr],[syscreator],[sysmodifier],[EntryGuid],[companycostcentercode],[BatchNo])" & _
                                                "values(null,'0.0','0.0','" & Trim(rBC) & "'," & 5 & ",'" & Trim(Format(g_month, "yyyy") & "-" & Format(g_month, "MM") & "-" & g_month.DaysInMonth(Format(g_month, "yyyy"), Format(g_month, "MM"))) & "'," & _
                                                Format(g_month, "yyyy") & "," & Format(g_month, "MM") & ",'" & Trim(descr) & "','" & CompanyAccountCode & "','" & Trim(rEntryNumber) & "','PHP'," & amount & _
                                                "," & acfc & ",'PHP'," & amount & "," & acfc & ",0, 1, 'B', 0, 'Z','" & Format(D.Now, "yyyy-MM-dd") & "','" & Format(g_month, "yyyy-MM-dd") & "','" & rFaktuurnr & "','" _
                                               & SysCreator & "','" & SysModifier & "','" & Entryguid & "', '" & cccc & "','" & batchNo & "')"
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

                sqltxt = "Insert into basepayroll_corp(bcode,paydesc, month_eli, year_eli, " & _
                         "date_time, sys_creator) values('" & Trim(rBC) & _
                         "','" + cmbdesc.Text + "','" & Format(g_month, "MM").ToString & "', '" & _
                         Format(g_month, "yyyy") & "', '" & _
                         Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "', '" & _
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


                If n = grd.Rows - 2 Then

                    sqltxt = "update numbers " & _
                            "set used = 1 , " & _
                            "Synergy_Status = '2' " & _
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
                Else

                    sqltxt = "update numbers " & _
                            "set used = 1 , " & _
                            "Synergy_Status = '2' " & _
                            "where number = (select top 1 number from numbers " & _
                            "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                            "and companycode = '" & rBC & "'"

                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                              '--------------7152010-------------------'
                    db.ConnectDB(ls_connection)                     '--------------7152010-------------------'
                    db.Execute_SQLQuery(sqltxt)                     '--------------7152010-------------------'
                    db.CloseConnection()

                End If

            Else

                rdr.Close()
                c.DisposeR()

                sqltxt = "Insert into wu_unknownbc(bcode, dr1, dr2) values('" & _
                                  bcode & "'," & Format(CDbl(grd.get_TextMatrix(n, 5)), "##0.00") & ", " & _
                                  Format(CDbl(grd.get_TextMatrix(n, 6)), "##0.00") & ")"

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

        Dim cls As New LogsEDIVISAYAS
        Dim rdr1 As SqlClient.SqlDataReader = Nothing
        Dim act As String = "EDI" & " " & "Payroll Basebranch" & " " & "for" & " " & txtDesc.Text
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
ext:
    End Sub
    Private Function GetCostCenter(ByVal code As String) As String
        GetCostCenter = "0001" & "-" & code
    End Function
    Private Sub CheckBC_Errors()
        Dim n As Integer = Nothing

        If errCtr <= 0 Then
            MsgBox("Successfully saved all branches.", MsgBoxStyle.Information, "EDI - Visayas")
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
    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub txtStart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub Label8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MsgBox("Debit - 3131004, Credit - 4220054")
        Exit Sub
    End Sub

    Private Sub button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
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
    Public Sub DATEFUNC()
        Dim LS_DATE As Integer = Nothing
        Dim ls_date1 = cboperiod.Text
        Dim dt As DateTime
        'Dim li_month As Integer = 2008
        Dim LS_SAMPLEDATE As String = Nothing
        '/------date function
        Dim year As Integer = dt.Year
        year = cboyear.Text
        Dim month As Integer = dt.Month
        month = cboperiod.Text
        Dim days As Integer = DateTime.DaysInMonth(year, month)
        Dim lastDayOfMonth As New DateTime(year, month, days)
        LS_SAMPLEDATE = Format(lastDayOfMonth, "dd")
        cmbDesc.Items.Clear()
        cmbDesc.Items.Add("1 - 15")
        cmbDesc.Items.Add("16 - " & LS_SAMPLEDATE)
    End Sub
    Private Sub cboSheet_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSheet.SelectedIndexChanged
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

    Private Sub cboYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboYear.SelectedIndexChanged

    End Sub

    Private Sub cboPeriod_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged
        Call DATEFUNC()
        g_month = Trim(cboYear.Text) & "-" & Trim(cboPeriod.Text) & "-1"
        lblMonth.BorderStyle = BorderStyle.Fixed3D
        lblMonth.Text = Format(g_month, "MMMM")
        txtDesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
    End Sub

    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        Dim main As New Main
        main.Show()
        Me.Hide()
    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        Label7.Text = txtDesc.Text & " " & "Payroll Base Branch"
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click

    End Sub
End Class
