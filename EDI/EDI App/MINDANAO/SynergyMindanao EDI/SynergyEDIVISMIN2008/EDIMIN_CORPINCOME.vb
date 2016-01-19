Public Class EDIMIN_CORPINCOME
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
    Dim folder1 As String = "C:\SynergMindanao\"
    Dim missingfile As String = "eli.txt"
    Dim LI_COUNTCOLUMN As Integer = Nothing
    Dim LI_CORPPARTNERS As String = Nothing
    Dim ls_gldebit As String = Nothing
    Dim ls_glcredit As String = Nothing
    Dim datenow As DateTime = Now
    Dim db As New clsDBconnection

    Private Sub EDIVISMIN_CORPINCOME_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim main As New Main
        Me.Hide()
        main.Show()
    End Sub
    Private Sub EDIVISMIN_CORPINCOME_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "KP Corp Income-" & gs_serverloc & " " & gs_version
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
            MsgBox("Browse File First", 16, "Synergy EDI Mindanao")
            Exit Sub
        End If
        If txtStart.Text = "" Then
            MsgBox("START ROW NOT FILLED", 64, "KP Corp Income -Mindanao")
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
        If Val(txtstart.Text) > g_Row Then
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

        For n = Val(txtstart.Text) - 2 To 0 Step -1

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
                        MsgBox("Some datas have not included. Please Check Excel File", 64, "Synergy EDI Mindanao")
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
        For r = Val(txtstart.Text) - 1 To g_Row - 1
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
            For R = Val(txtstart.Text) - 1 To g_Row - 1
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
        sqltxt = "select * from corporate_partners where corpCODE LIKE '" + LI_CORPPARTNERS + "'"
        Dim jpoy As New clsData
        Dim rdr As SqlClient.SqlDataReader = Nothing
        If jpoy.Error_Inititalize_INI Then Exit Sub
        If jpoy.ErrorConnectionReading(False) Then Exit Sub
        If Not jpoy.Error_SetRdr(sqltxt, rdr, sqlmsg) Then
            If rdr.Read Then
                ls_gldebit = Trim(rdr(2).ToString) 'debit
                ls_glcredit = Trim(rdr(3).ToString) 'credit
            Else
                MsgBox(LI_CORPPARTNERS & " " & "THIS COMPANY DOESN'T EXIST IN THE DATABASE ")

            End If
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
        Dim c As New clsData
        Dim rdr As SqlClient.SqlDataReader = Nothing

        btnGIF.Show()
        sqlmsg = "select top 1 number from numbers where companycode = " + rBC + " and used = 0"

        'If jpoy.Error_Inititalize_INI Then Exit Sub
        'If jpoy.ErrorConnectionReading(False) Then Exit Sub

        sqltxt = "select * from kp_income where bcode = 000 and date_time > '" & _
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
        '    Exit Sub
        'End If
        rdr.Close()
        db.CloseConnection()

        'If jpoy.errorConnectionWriting Then Exit Sub
        f.Grp_Visible(grp, pb, "Now saving data...")
        Dim SW As New IO.StreamWriter(folder1 & missingfile)
        For n = 0 To grd.Rows - 2
            If Not LCase(grd.get_TextMatrix(n, 0).ToString) = "totals" Then
                Dim totalAmount As Double = Nothing
                'Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 1)), "000")
                sqlmsg = "select * from boskp where kpcode = '" + grd.get_TextMatrix(n, 1) + "'" 'original code

                If db.isConnected Then                  '--------------7142010-------------------'
                    db.CloseConnection()                '--------------7142010-------------------'
                End If                                  '--------------7142010-------------------'
                If ls_connection Is Nothing Then
                    c.INITIALIZE_INI()
                End If                   '--------------7142010-------------------'
                db.ConnectDB(ls_connection)             '--------------7142010-------------------'
                rdr = db.Execute_SQL_DataReader(sqlmsg) '--------------7142010-------------------'

                If rdr.Read Then
                    rBC = Trim(rdr(0))
                    rBCnm = Trim(rdr(1))
                    rdr.Close()
                    db.CloseConnection()

                    grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                    Application.DoEvents()

                    sqltxt = "select top 1 newid()"

                    If db.isConnected Then                          '--------------7142010-------------------'
                        db.CloseConnection()                        '--------------7142010-------------------'
                    End If                                          '--------------7142010-------------------'
                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                           '--------------7142010-------------------'
                    db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                    rdr = db.Execute_SQL_DataReader(sqltxt)         '--------------7142010-------------------'

                    rdr.Read()
                    Entryguid = rdr(0).ToString
                    rdr.Close()
                    db.CloseConnection()

                    sqltxt = "select top 1 number from numbers where companycode = '" + rBC + "' and used = 0"
                    sqltxt = sqltxt + " order by number asc"

                    If db.isConnected Then                          '--------------7142010-------------------'
                        db.CloseConnection()                        '--------------7142010-------------------'
                    End If                                          '--------------7142010-------------------'
                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                           '--------------7142010-------------------'
                    db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                    rdr = db.Execute_SQL_DataReader(sqltxt)         '--------------7142010-------------------'

                    If rdr.Read Then
                        rFaktuurnr = rdr(0)
                    Else
                        eMsg = ". Error in reading numbers table."
                        f.Emsg(eMsg, "Branch Code: " & rBC)
                        'f.Error_Log(rBC & eMsg, g_errLog)
                        rdr.Close()
                        db.CloseConnection()
                        Exit Sub

                    End If
                    rdr.Close()
                    If Not nFn Then
                        sqltxt = "select freenumber from bacofreenumbers " & _
                                 "where numberkey = 'rpfinentry'"
                        If db.isConnected Then                      '--------------7142010-------------------'
                            db.CloseConnection()                    '--------------7142010-------------------'
                        End If                                      '--------------7142010-------------------'
                        If ls_connection Is Nothing Then
                            c.INITIALIZE_INI()
                        End If                          '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                 '--------------7142010-------------------'
                        rdr = db.Execute_SQL_DataReader(sqltxt)     '--------------7142010-------------------'

                        If rdr.Read Then
                            fn = rdr(0)
                            nFn = True
                        Else
                            eMsg = "Error in reading bacofreenumbers table"
                            f.Emsg(eMsg, "bacofreenumbers")
                            f.Error_Log(eMsg, g_errLog)
                            rdr.Close()
                            db.CloseConnection()
                            Exit Sub

                        End If
                        rdr.Close()
                        db.CloseConnection()
                    End If
                    fn = fn + 1
                    rEntryNumber = fn
                    ctrFn = ctrFn + 1
                    For r = 1 To 248
                        Dim amount As Double = Nothing
                        Dim acfc As Double = Nothing
                        Dim cccc As String = Nothing

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
                        totalAmount = totalAmount + amount
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
                        End If                                                              '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                                                     '--------------7142010-------------------'
                        If db.Execute_SQLQuery(sqltxt) < 1 Then                                         '--------------7142010-------------------'
                            db.RollbackTransaction()                                                    '--------------7142010-------------------'
                            db.CloseConnection()                                                        '--------------7142010-------------------'
                            MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)    '--------------7142010-------------------'
                            Exit Sub                                                                    '--------------7142010-------------------'
                        End If                                                                          '--------------7142010-------------------'
                        db.CloseConnection()
extcase:
                    Next r
                    sqltxt = "Insert into CORPINCOME_CORP(bcode, month_eli, year_eli, " & _
                                            "date_time, sys_creator,rowguid) values('" & Trim(rBC) & _
                                            "','" & Format(g_month, "MM").ToString & "', '" & _
                                            Format(g_month, "yyyy") & "', '" & _
                                            Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "', '" & _
                                            SysCreator & "',newid())"
                    'Call SaveToOtherDataBase()
                    Log("Success insertion " + Me.Text + "--->'" + Trim(rBC) + "','" & Format(g_month, "MM").ToString & "','" & Format(g_month, "yyyy").ToString & "'," & Date.Now & ",'" & SysCreator & "'")
                    If ls_connection Is Nothing Then
                        c.INITIALIZE_INI()
                    End If                                                              '--------------7142010-------------------'
                    db.ConnectDB(ls_connection)                                                     '--------------7142010-------------------'
                    If db.Execute_SQLQuery(sqltxt) < 1 Then                                         '--------------7142010-------------------'
                        db.RollbackTransaction()                                                    '--------------7142010-------------------'
                        db.CloseConnection()                                                        '--------------7142010-------------------'
                        MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)    '--------------7142010-------------------'
                        Exit Sub                                                                    '--------------7142010-------------------'
                    End If                                                                          '--------------7142010-------------------'
                    db.CloseConnection()

                    If n = grd.Rows - 3 Then
                        sqltxt = "update numbers " & _
                                "set used = 1 , Synergy_Status = '2' " & _
                                "where number = (select top 1 number from numbers " & _
                                "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                                "and companycode = '" & rBC & "'"
                        If ls_connection Is Nothing Then
                            c.INITIALIZE_INI()
                        End If                           '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                        db.Execute_SQLQuery(sqltxt)                     '--------------7142010-------------------'
                        db.CloseConnection()                            '--------------7142010-------------------'
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
                        End If                           '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                        db.Execute_SQLQuery(sqltxt)                     '--------------7142010-------------------'
                        db.CloseConnection()                            '--------------7142010-------------------'
                        '<--------New Eli Code


                        'If jpoy.Error_SetComm(sqltxt, False, sqlmsg) Then              'true = to write inig save
                        '    f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errlog)
                        '    Exit Sub
                        'End If
                        'MsgBox("Data Save", MsgBoxStyle.Information, "Save")
                        'grd.Clear()

                    Else
                        sqltxt = "update numbers " & _
                        "set used = 1 , Synergy_Status = '2' " & _
                        "where number = (select top 1 number from numbers " & _
                        "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                        "and companycode = '" & rBC & "'"
                        If ls_connection Is Nothing Then
                            c.INITIALIZE_INI()
                        End If                           '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                        db.Execute_SQLQuery(sqltxt)                     '--------------7142010-------------------'
                        db.CloseConnection()                            '--------------7142010-------------------'

                    End If
                Else
                    SW.WriteLine(grd.get_TextMatrix(n, 1) & " Missing Branch Code")
                    rdr.Close()
                End If
            End If
            pb.Value = ((n / (grd.Rows - 2)) * 100)
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
        Dim act As String = "EDI" & " " & "CorpIncome" & " " & "for" & " " & lblMonth.Text.Trim & " " & cboYear.Text.Trim
        Dim syndev As String = "insert into edi_maintainance_logs (datetimelog, application, activity, resource, department, remarks)values" & _
                               "('" + datenow + "', 'EDI', '" + act + "', '" & name & "', '" & task & "', 'DONE')"

        If cls.Error_Inititalize_INI() Then Exit Sub
        If cls.ErrorConnectionReading(False) Then Exit Sub
        If Not cls.dr(syndev, rdr1) Then Exit Sub
        Call CheckBC_Errors()

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
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim n As Integer = Nothing
        Dim month As String = Nothing
        Dim ls_boscode As String = Nothing
        For n = 0 To grd.Rows - 2
            Dim ls_searchkpcode As String = grd.get_TextMatrix(n, 1)
            Dim tbl_boskp As String = "select boscode from boskp where kpcode like '" + ls_searchkpcode + "'"
            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function
            If Not c.Error_SetRdr(tbl_boskp, rdr, sqlmsg) Then
                If rdr.Read Then
                    ls_boscode = rdr(0)
                End If
                rdr.Close()
            End If
            Dim s As String = "Select * from CORPINCOME_corp where month_eli = '" & cboPeriod.Text & "' and year_eli = '" & cboYear.Text & "' and bcode = '" & ls_boscode & "'"
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
            Dim g As String = grd.get_TextMatrix(n, 1)
            Dim a As String

            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function

            If Len(g).ToString < 3 Then
                Dim bcode As String = "0" & (g)
                sqltxt = "select KPCODE from BOSKP where KPCODE = '" + bcode + "'"

            Else
                Dim bcode As String = (g)
                sqltxt = "select KPCODE from BOSKP where KPCODE = '" + bcode + "'"
            End If

            If Len(g).ToString < 2 Then
                Dim bcode As String = "00" & (g)
                sqltxt = "select KPCODE from BOSKP where KPCODE = '" + bcode + "'"
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
                num1 = grd.get_TextMatrix(n, 1)
                num2 = grd.get_TextMatrix(i, 1)
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

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim MAIN As New Main
        MAIN.Show()
        Me.Hide()

    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        Label7.Text = txtDesc.Text & " " & "Kp Corp Income"

    End Sub
End Class