Public Class KPIncome
    Dim bcode As String = Nothing 'ely code 7-4-2012
    Dim folder1 As String = "C:\SynergFolder1LUZON\"
    Dim missingfile As String = "eli.txt"
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

    Private Sub KPIncome_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim main As New EDI_Luzon_Main
        main.Show()
    End Sub

    Private Sub KPIncome_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "KP Income-" & gs_serverloc & " " & gs_version
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

    Private Sub Init_Combo()
        Dim d As DateTime = Now
        Dim n As Integer = Nothing
        Dim LS_YEAR As String = Nothing
        Dim LS_NOW As String = Format(Date.Now, "yyyy")
        cboperiod.DropDownStyle = ComboBoxStyle.DropDownList
        cboyear.DropDownStyle = ComboBoxStyle.DropDownList
        f.Initialize_Combo(cboYear, callyear, Format(Date.Now, "yyyy"))

        f.Initialize_Combo(cboperiod, 1, 12)
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
        If txtdesc.Text = "" Then
            MsgBox("Please fill description text box", 16, gs_version)
        Else
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
            Dim errormsg As String = Nothing
            '/-------> ELI code
            If cboSet.Text = "False" Then
                'For n = 0 To grd.Rows - 2
                '    f.Grp_Visible(grp, pb, "Now saving data...")
                '    pb.Value = ((n / (grd.Rows - 2)) * 100)
                'Next
                'MsgBox("Successfuly saved all branches.", MsgBoxStyle.Information, gs_version)
                MsgBox("No data to be save please set as TRUE.", MsgBoxStyle.Information, gs_version) '---added by Arthur 05-12-2014
                Exit Sub
            End If
            If alreadysave() = True Then
                Dim li_input As Integer = Nothing
                li_input = MsgBox("This data is already been save, Would you like to save it anyway?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information, gs_version)
                If li_input = vbYes Then
                    GoTo continuesave
                Else
                    GoTo extalreadysave
                End If

            End If
continuesave:
            '----------->ELI Code/

            If Me.notbalancedata = True Then
                MsgBox("Data not balance!", 48, gs_version)

            End If
            '<----- Already Save Function
            If g_setup = False Then
                _errHC(btnEnter)
                f.Imsg("New Setup is needed", "undecided data/s")
                Exit Sub
            End If
            _refHC(btnEnter)
            Dim jpoy As New ClsDataEDILuzon
            Dim rdr As SqlClient.SqlDataReader = Nothing
            picGif.Show()
            sqlmsg = "select top 1 number from numbers where companycode = " + rBC + " and used = 0"

            If db.isConnected Then                  '--------------7142010-------------------'
                db.CloseConnection()                '--------------7142010-------------------'
            End If                                  '--------------7142010-------------------'
            jpoy.INITIALIZE_INI()                   '--------------7142010-------------------'
            db.ConnectDB(ls_connection)             '--------------7142010-------------------'
            rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7142010-------------------'
            'If jpoy.Error_Inititalize_INI Then Exit Sub
            'If jpoy.ErrorConnectionReading(False) Then Exit Sub
            sqltxt = "select * from kp_income where bcode = 000 and date_time > '" & _
                                     Format(g_month, "yyyy-MM-dd") & "'"
            If db.isConnected Then                  '--------------7142010-------------------'
                db.CloseConnection()                '--------------7142010-------------------'
            End If                                  '--------------7142010-------------------'
            jpoy.INITIALIZE_INI()                   '--------------7142010-------------------'
            db.ConnectDB(ls_connection)             '--------------7142010-------------------'
            rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7142010-------------------'

            'If jpoy.Error_SetRdr(sqltxt, rdr, sqlmsg) Then
            '    'MsgBox("Touch move ka mam sir!")
            '    f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errlog)
            '    Exit Sub
            'End If
            'If rdr.Read Then
            '    MsgBox("Date specified was already actioned using e-Synergy.", MsgBoxStyle.Critical, Format(g_month, "yyyy-MM-dd"))
            '    Exit Sub
            'End If
            rdr.Close()
            db.CloseConnection()                    '--------------7142010-------------------'

            'If jpoy.errorConnectionWriting Then Exit Sub
            f.Grp_Visible(grp, pb, "Now saving data...")
            Dim SW As New IO.StreamWriter(folder1 & missingfile)
            For n = 0 To grd.Rows - 2
                If Not LCase(grd.get_TextMatrix(n, 0).ToString) = "totals" Then
                    Dim totalAmount As Double = Nothing
                    Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 1)), "000")
                    sqlmsg = "select bedrnr,bedrnm from bedryf where bedrnr = '" + bcode + "'"


                    If db.isConnected Then                  '--------------7142010-------------------'
                        db.CloseConnection()                '--------------7142010-------------------'
                    End If                                  '--------------7142010-------------------'
                    jpoy.INITIALIZE_INI()                   '--------------7142010-------------------'
                    db.ConnectDB(ls_connection)             '--------------7142010-------------------'
                    rdr = db.Execute_SQL_DataReader(sqlmsg) '--------------7142010-------------------'

                    If rdr.Read Then
                        rBC = Trim(rdr(0))
                        rBCnm = Trim(rdr(1))
                        rdr.Close()
                        db.CloseConnection()                    '--------------7142010-------------------'

                        grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                        Application.DoEvents()

                        sqltxt = "select top 1 newid()"

                        If db.isConnected Then                          '--------------7142010-------------------'
                            db.CloseConnection()                        '--------------7142010-------------------'
                        End If                                          '--------------7142010-------------------'
                        jpoy.INITIALIZE_INI()                           '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                        rdr = db.Execute_SQL_DataReader(sqltxt)         '--------------7142010-------------------'

                        rdr.Read()
                        Entryguid = rdr(0).ToString
                        rdr.Close()
                        db.CloseConnection()                    '--------------7142010-------------------'

                        sqltxt = "select top 1 number from numbers where companycode = '" + rBC + "' and used = 0"
                        sqltxt = sqltxt + " order by number asc"

                        If db.isConnected Then                          '--------------7142010-------------------'
                            db.CloseConnection()                        '--------------7142010-------------------'
                        End If                                          '--------------7142010-------------------'
                        jpoy.INITIALIZE_INI()                           '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                        rdr = db.Execute_SQL_DataReader(sqltxt)         '--------------7142010-------------------'

                        If rdr.Read Then
                            rFaktuurnr = rdr(0)
                        Else
                            eMsg = ". Error in reading numbers table."
                            f.Emsg(eMsg, "Branch Code: " & rBC)
                            rdr.Close()
                            db.CloseConnection()                         '--------------7142010-------------------'
                            Exit Sub

                        End If
                        rdr.Close()
                        db.CloseConnection()                         '--------------7142010-------------------'
                        If Not nFn Then
                            sqltxt = "select freenumber from bacofreenumbers " & _
                                     "where numberkey = 'rpfinentry'"
                            If db.isConnected Then                      '--------------7142010-------------------'
                                db.CloseConnection()                    '--------------7142010-------------------'
                            End If                                      '--------------7142010-------------------'
                            jpoy.INITIALIZE_INI()                          '--------------7142010-------------------'
                            db.ConnectDB(ls_connection)                 '--------------7142010-------------------'
                            rdr = db.Execute_SQL_DataReader(sqltxt)     '--------------7142010-------------------'

                            If rdr.Read Then
                                fn = rdr(0)
                                nFn = True
                            Else
                                eMsg = "Error in reading bacofreenumbers table"
                                f.Emsg(eMsg, "bacofreenumbers")
                                f.Error_Log(eMsg, g_errlog)
                                rdr.Close()
                                db.CloseConnection()                         '--------------7142010-------------------'
                                Exit Sub

                            End If
                            rdr.Close()
                            db.CloseConnection()                             '--------------7142010-------------------'
                        End If
                        fn = fn + 1
                        rEntryNumber = fn
                        ctrFn = ctrFn + 1
                        For r = 1 To 6
                            Dim amount As Double = Nothing
                            Dim acfc As Double = Nothing
                            Dim cccc As String = Nothing
                            Select Case r
                                Case 1
                                    amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                                    If amount <> 0 Then
                                        acfc = 0
                                        CompanyAccountCode = "3110005"
                                        descr = "KP INCOME" & " " & txtdesc.Text.Trim
                                    Else
                                        GoTo extcase
                                    End If
                                Case 2
                                    amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                                    If amount <> 0 Then
                                        acfc = 0
                                        CompanyAccountCode = "3110005"
                                        descr = "RIA INCOME" & " " & txtdesc.Text.Trim
                                    Else
                                        GoTo extcase
                                    End If
                                Case 3
                                    amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                                    If amount <> 0 Then
                                        acfc = 0
                                        CompanyAccountCode = "3110005"
                                        descr = "KP INCOME" & " " & txtdesc.Text.Trim
                                    Else
                                        GoTo extcase
                                    End If
                                Case 4
                                    amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                                    If amount <> 0 Then
                                        acfc = 0
                                        descr = "RIA INCOME" & " " & txtdesc.Text.Trim
                                        CompanyAccountCode = "3110005"
                                    Else
                                        GoTo extcase
                                    End If
                                Case 5
                                    acfc = Format(grd.get_TextMatrix(n, 12)) 'new eli
                                    If acfc <> 0 Then
                                        amount = 0
                                        CompanyAccountCode = "4210001"
                                        descr = "KP INCOME" & " " & txtdesc.Text.Trim
                                    Else
                                        GoTo extcase
                                    End If
                                Case 6
                                    acfc = Format(grd.get_TextMatrix(n, 13)) 'new eli
                                    If acfc <> 0 Then
                                        amount = 0
                                        CompanyAccountCode = "4220053"
                                        descr = "RIA INCOME" & " " & txtdesc.Text.Trim
                                    Else
                                        GoTo extcase
                                    End If
                            End Select

                            totalAmount = totalAmount + amount
                            CompanyAccountCode = Space(9 - CompanyAccountCode.Length) & CompanyAccountCode
                            rFaktuurnr = Space(8 - rFaktuurnr.Length) & rFaktuurnr

                            sqltxt = "INSERT INTO " & database & ".[dbo].[TransactionsPending]([paymentmethod],[freefield4],[freefield5], [CompanyCode], [TransactionType], [TransactionDate]," & _
                                                        "[FinYear],[FinPeriod], [Description], [CompanyAccountCode],[EntryNumber],[CurrencyAliasAC],[AmountDebitAC],[AmountCreditAC]," & _
                                                        "[CurrencyAliasFC],[AmountDebitFC],[AmountCreditFC],[VATCode],[ProcessNumber],[ProcessLineCode], [res_id], [oorsprong],[docdate]," & _
                                                        "[vervdatfak],[faktuurnr],[syscreator],[sysmodifier],[EntryGuid],[companycostcentercode])" & _
                                                        "values(null,'0.0','0.0','" & Trim(rBC) & "'," & 5 & ",'" & Trim(Format(g_month, "yyyy") & "-" & Format(g_month, "MM") & "-" & g_month.DaysInMonth(Format(g_month, "yyyy"), Format(g_month, "MM"))) & "'," & _
                                                        Format(g_month, "yyyy") & "," & Format(g_month, "MM") & ",'" & Trim(descr) & "','" & CompanyAccountCode & "','" & Trim(rEntryNumber) & "','PHP'," & amount & _
                                                        "," & acfc & ",'PHP'," & amount & "," & acfc & ",0, 1, 'B', 0, 'Z','" & Format(d.Now, "yyyy-MM-dd") & "','" & Format(g_month, "yyyy-MM-dd") & "','" & rFaktuurnr & "','" _
                                                        & SysCreator & "','" & SysModifier & "','" & Entryguid & "', '" & cccc & "')"
                            jpoy.INITIALIZE_INI()                                                              '--------------7142010-------------------'
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
                        sqltxt = "Insert into kp_income(bcode, month_eli, year_eli, " & _
                                                "date_time, sys_creator) values('" & Trim(rBC) & _
                                                "','" & Format(g_month, "MM").ToString & "', '" & _
                                                Format(g_month, "yyyy") & "', '" & _
                                                Format(Date.Now, "yyyy-MM-dd") & "', '" & _
                                                SysCreator & "')"
                        jpoy.INITIALIZE_INI()                                                           '--------------7142010-------------------'
                        db.ConnectDB(ls_connection)                                                  '--------------7142010-------------------'
                        If db.Execute_SQLQuery(sqltxt) < 1 Then                                      '--------------7142010-------------------'
                            db.RollbackTransaction()                                                 '--------------7142010-------------------'
                            db.CloseConnection()                                                     '--------------7142010-------------------'
                            MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version) '--------------7142010-------------------'
                            Exit Sub                                                                 '--------------7142010-------------------'
                        End If                                                                       '--------------7142010-------------------'
                        db.CloseConnection()

                        If n = grd.Rows - 3 Then
                            sqltxt = "update numbers " & _
                                    "set used = 1 " & _
                                    "where number = (select top 1 number from numbers " & _
                                    "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                                    "and companycode = '" & rBC & "'"
                            jpoy.INITIALIZE_INI()                           '--------------7142010-------------------'
                            db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                            db.Execute_SQLQuery(sqltxt)                     '--------------7142010-------------------'
                            db.CloseConnection()                            '--------------7142010-------------------'

                            sqltxt = "update bacofreenumbers set " & _
                                     "freenumber = freenumber + " & ctrFn & _
                                    " where numberkey = 'rpfinentry'"

                            '-------->New Eli Code
                            Dim settrue As Boolean

                            If cboSet.Text = "True" Then
                                settrue = True
                            Else
                                settrue = False
                            End If

                            jpoy.INITIALIZE_INI()                              '--------------7142010-------------------'
                            db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                            db.Execute_SQLQuery(sqltxt)                     '--------------7142010-------------------'
                            db.CloseConnection()                            '--------------7142010-------------------'


                        Else
                            sqltxt = "update numbers " & _
                            "set used = 1 " & _
                            "where number = (select top 1 number from numbers " & _
                            "where companycode = '" & rBC & "' and used = 0 order by number asc) " & _
                            "and companycode = '" & rBC & "'"
                            jpoy.INITIALIZE_INI()                              '--------------7142010-------------------'
                            db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                            db.Execute_SQLQuery(sqltxt)                     '--------------7142010-------------------'
                            db.CloseConnection()                            '--------------7142010-------------------'


                        End If
                    Else
                        SW.WriteLine(grd.get_TextMatrix(n, 1) & " Missing Branch Code")

                        rdr.Close()
                        jpoy.DisposeR()
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

            Dim cls As New LogsEDILUZON
            Dim rdr1 As SqlClient.SqlDataReader = Nothing
            Dim act As String = "EDI" & " " & "KP Income" & gs_serverloc & "for" & " " & txtdesc.Text.Trim
            Dim syndev As String = "insert into edi_maintainance_logs (datetimelog, application, activity, resource, department, remarks)values" & _
                                   "('" + datenow + "', 'EDI', '" + act + "', '" & name & "', '" & task & "', 'DONE')"

            If cls.Error_Inititalize_INI() Then Exit Sub
            If cls.ErrorConnectionReading(False) Then Exit Sub
            If Not cls.dr(syndev, rdr1) Then Exit Sub

            picGif.Hide()

            Call CheckBC_Errors()

            grp.Hide() 'HIDE PROGRESS BAR
            SW.Close()
extAlreadySave:
        End If
    End Sub

    Private Function alreadysave() As Boolean
        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim n As Integer = Nothing
        Dim month As String = Nothing
        Dim squery As String = Nothing
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)
            squery = "Select * from kp_income where month_eli = '" & cboPeriod.Text & "' and year_eli = '" & cboYear.Text & "' and bcode = '" & bcode & "'"
            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function
            If Not c.Error_SetRdr(squery, rdr, sqlmsg) Then
                While rdr.Read
                    alreadysave = True
                    Exit Function
                End While
                rdr.Close()
            End If
        Next
    End Function

    Private Function notbalancedata() As Boolean
        Dim n As Integer
        Dim i As Integer = Nothing
        Dim total As Double = Nothing
        Dim r As Integer = Nothing
        Dim total_debit As Double
        Dim total_credit As Double
        Dim amount As Double = Nothing 'debit 
        Dim acfc As Double = Nothing   'credit
        Dim cccc As String = Nothing
        For n = 0 To grd.Rows - 2
            For r = 1 To 3
                Select Case r
                    Case 1
                        amount = Format(grd.get_TextMatrix(n, 4)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 2
                        amount = Format(grd.get_TextMatrix(n, 5)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 3
                        amount = Format(grd.get_TextMatrix(n, 8)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 4
                        amount = Format(grd.get_TextMatrix(n, 9)) 'new eli
                        If amount <> 0 Then
                            acfc = 0
                        Else
                            GoTo extcase
                        End If
                    Case 5
                        acfc = Format(grd.get_TextMatrix(n, 12)) 'new eli
                        If acfc <> 0 Then
                            amount = 0
                        Else
                            GoTo extcase
                        End If
                    Case 6
                        acfc = Format(grd.get_TextMatrix(n, 13)) 'new eli
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
                If r = 6 Then
                    d = Trim(CDbl(Format(total_debit, "#,##0.00")))
                    c = Trim(CDbl(Format(total_credit, "#,##0.00")))
                    If r = 3 And d <> c Then
                        Dim sqq1 As String = "There's an entry in " & _
                                             grd.get_TextMatrix(n, 1) & " " & grd.get_TextMatrix(n, 2) & _
                                             " that is not balanced" & _
                                             vbCrLf & vbCrLf & "Total Debit:" & d & _
                                             vbCrLf & "Total Credit:" & c & _
                                             vbCrLf & vbCrLf & "Please check excel file entries again "
                        MsgBox(sqq1, 16, gs_version)
                        notbalancedata = True
                        Exit Function
                    End If
                    total_debit = Nothing 'to refresh debit total
                    total_credit = Nothing ' to refresh credit total
                End If
extcase:
            Next r 'alter ends'
        Next
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
            MsgBox("Successfuly saved all branches.", MsgBoxStyle.Information, gs_version)
            btnSave.Hide()
            btnSetup.Enabled = True
        Else
            'MsgBox(Replace(sTrbc, ".", vbCrLf), MsgBoxStyle.Exclamation, errCtr & " branch code/s, not found!")
            MsgBox(Replace(sTrbc, ".", vbCrLf), MsgBoxStyle.Exclamation, gs_version)
            btnSave.Hide()
            btnSetup.Enabled = False
        End If
    End Sub

    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        btnSave.Hide()
        dg.Show()
        grd.Hide()
        grp.Hide()
        'grpinf.Show()
        picGif.Hide()
    End Sub

    Private Sub btnEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnter.Click
        If txtStart.Text = "" Then
            MsgBox("Start row is empty", 16, gs_version)
            Exit Sub
        End If
        If Me.txtLocation.Text = "" Then
            MsgBox("Browse File First", 16)
            Exit Sub
        End If
        MsgBox("You're about to save this data in " & lblMonth.Text & " " & cboYear.Text, 64, gs_version)
        If f.Error_Nonumeric(txtStart) Then Exit Sub
        If CheckUnder() = False Then
            Exit Sub
        End If

        If Format(CDate(g_month), "yyyy-MM-dd") > Format(CDate(d.Now), "yyyy-MM-dd") Then
            MsgBox("Invalid date", MsgBoxStyle.Critical, gs_version)
            Exit Sub
        End If
        Call GetNewSetup()
        If missingbranch() = True Then
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
        If Val(txtstart.Text) > g_Row Then
            f.Emsg("Rows is up to " & g_Row & " only.", gs_version)
            CheckUnder = False
            Exit Function
        End If
        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim n As Integer = Nothing
        If c.Error_Inititalize_INI Or c.ErrorConnectionReading(False) Then
            CheckUnder = False
            Exit Function
        End If
        For n = Val(txtstart.Text) - 2 To 0 Step -1

            If Not Trim(dg.Item(n, 1).ToString) = "" Then

                Dim bcode As String = (dg.Item(n, 1))

                sqltxt = "select KPCODE from BOSKP where KPCODE = '" & _
                                   bcode & "'"

                If c.Error_SetRdr(sqltxt, rdr, sqlmsg) Then
                    f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errlog)
                    CheckUnder = False
                    Exit Function

                Else

                    If rdr.Read Then
                        MsgBox("Some Files not Included. Please check Excel File and Try Again", MsgBoxStyle.Critical, gs_version)
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
            For R = Val(txtstart.Text) - 1 To g_Row - 1
                '/--------------NEW ELI CODE
                Dim s1 As String = Nothing
                s1 = dg.Item(R, 1).ToString
                If s1 = "0" Then
                    grp.Visible = False
                    picGif.Hide()
                    GoTo ext
                End If
                'NEW ELI CODE ----------------/
                If Not Trim(LCase(dg.Item(R, 2).ToString)) = "grand total" Then

                    For c = 0 To g_col - 1
                        If c >= 4 Then
                            grd.set_TextMatrix(l_ctr, c, Format(dg.Item(R, c), "##,##0.00"))
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
            MsgBox("None Existing Data" & vbCrLf & ex.Message, MsgBoxStyle.Critical, gs_version)
            grp.Visible = False
        End Try
EXT:
    End Sub

    Private Function missingbranch() As Boolean
        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim n As Integer
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 1)
            Dim a As String
            If c.Error_Inititalize_INI Then Exit Function
            If c.ErrorConnectionReading(False) Then Exit Function

            If Len(g).ToString < 3 Then
                bcode = "0" & (g)
                sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"

            Else
                bcode = (g)
                sqltxt = "select bedrnr from bedryf where bedrnr = '" + bcode + "'"
            End If
            If Len(g).ToString < 2 Then
                bcode = "00" & (g)
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

    Private Sub initialize_cbo_sheets(ByVal sheet() As String)
        Dim li_sheet As Integer = Nothing
        For li_sheet = 0 To UBound(sheet) - 1
            cboSheet.Items.Add(sheet(li_sheet))
        Next
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        EDI_Luzon_Main.Show()
    End Sub

    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged
        g_month = Trim(cboYear.Text) & "-" & Trim(cboPeriod.Text) & "-1"
        lblMonth.BorderStyle = BorderStyle.Fixed3D
        lblMonth.Text = Format(g_month, "MMMM")
        txtdesc.Text = lblMonth.Text.Substring(0, 3) & " " & cboYear.Text
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        Dim sqq = "Debit             ,Credit" & _
                  vbCrLf & "3110005       ,4210001" & _
                  vbCrLf & "                     ,4220053"
        MsgBox(sqq, MsgBoxStyle.OkOnly, gs_version)
    End Sub

    Private Sub txtStart_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStart.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub txtStart_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStart.TextChanged

    End Sub

    Private Sub cboSet_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSet.SelectedIndexChanged

    End Sub

    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        'Dim main As New EDI_Luzon_Main
        'main.Show()
        Me.Close()
    End Sub

    Private Sub txtdesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtdesc.TextChanged
        Label8.Text = txtdesc.Text & " " & "KP Income"
    End Sub
End Class