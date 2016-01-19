Imports Microsoft.Win32


Public Class AllocationLuzon

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
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        EDI_Luzon_Main.Show()
    End Sub

    Private Sub AllocationLuzon_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim main As New EDI_Luzon_Main
        main.Show()
    End Sub
    Private Sub Allocation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Allocation-" & gs_serverloc & " " & gs_version
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
            MsgBox("Browse File First", 16, gs_version)
            Exit Sub
        End If
        MsgBox("You're about to save this data in " & lblMonth.Text & " " & cboYear.Text, 64, gs_version)
        btnEnter.Enabled = False
        If f.Error_Nonumeric(txtStart) Then Exit Sub
        If CheckUnder() = False Then
            Exit Sub
        End If

        If Format(CDate(g_month), "yyyy-MM-dd") > Format(CDate(Date.Now), "yyyy-MM-dd") Then
            MsgBox("Invalid date", MsgBoxStyle.Critical, gs_version)
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
EXT:
    End Sub
    Private Function CheckUnder() As Boolean
        If Val(txtStart.Text) > g_Row Then
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
                    ' f.Error_Log(f.ErrTxt(sqltxt, sqlmsg), g_errlog)
                    CheckUnder = False
                    Exit Function

                Else

                    If rdr.Read Then
                        MsgBox("Some datas have not included. Please Check Excel File", 64, gs_version)
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
            MsgBox("None Existing Data" & vbCrLf & ex.Message, MsgBoxStyle.Critical, gs_version)
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
        If txtdesc.Text = "" Then
            MsgBox("Please fill description text box", 16, gs_version)
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

        Dim SysCreator As Integer = res_id
        Dim SysModifier As Integer = res_id
        Dim descr As String = Label8.Text
        Dim eMsg As String = Nothing

        Dim nFn As Boolean = False
        Dim fn As Integer = Nothing
        Dim ctrFn As Integer = Nothing

        '-------> ELI codeMsgBox("Successfuly saved all branches.", MsgBoxStyle.Information, gs_version)
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
            li_input = MsgBox("This data has already been saved, Would you like to save it again?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information, gs_version)
            If li_input = vbYes Then
                GoTo continuesave
            Else
                GoTo extalreadysave
            End If

        End If

continuesave:

        If balancedata() = True Then
            MsgBox("Some files in the excel appear to be not balanced, Please check Excel file and try again", 48, gs_version)
            GoTo extalreadysave
        End If
        '<------ Eli code

        If g_setup = False Then
            _errHC(btnEnter)
            f.Imsg("New Setup is needed", "undecided data/s")
            Exit Sub
        End If
        _refHC(btnEnter)

        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing


        sqltxt = "select * from allocation_corp where bcode = 000 and date_time > '" & _
                          Format(g_month, "yyyy-MM-dd") & "'"

        If db.isConnected Then                  '--------------7142010-------------------'
            db.CloseConnection()                '--------------7142010-------------------'
        End If                                  '--------------7142010-------------------'
        c.INITIALIZE_INI()                      '--------------7142010-------------------'
        db.ConnectDB(ls_connection)             '--------------7142010-------------------'
        rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7142010-------------------'

        rdr.Close()
        'New code added by ching2
        db.CloseConnection() '--------------7142010-------------------'
        'If c.errorConnectionWriting Then Exit Sub

        f.Grp_Visible(grp, pb, "Now saving data..")
        picGif.Show()

        For n = 0 To grd.Rows - 2

            Dim totalAmount As Double = Nothing
            Dim totalCredit As Double = Nothing
            Dim totalDebit As Double = Nothing

            Dim bcode As String = Format(Int(grd.get_TextMatrix(n, 0)), "000")
            sqltxt = "select bedrnr, bedrnm from BEDRYF where bedrnr = '" & _
                      bcode & "'"

            'New code added by ching2
            If db.isConnected Then                  '--------------7142010-------------------'
                db.CloseConnection()                '--------------7142010-------------------'
            End If                                  '--------------7142010-------------------'
            c.INITIALIZE_INI()                      '--------------7142010-------------------'
            db.ConnectDB(ls_connection)             '--------------7142010-------------------'
            rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7142010-------------------'


            If rdr.Read Then            'rdr.close - x
                rBC = Trim(rdr(0))
                rBCnm = Trim(rdr(1))
                rdr.Close()
                'New code added by ching2
                db.CloseConnection() '--------------7142010-------------------'

                grp.Text = "now saving branch code " & rBC & "(" & rBCnm & ")..."
                Application.DoEvents()

                sqltxt = "SELECT * FROM wu_kebot " & _
                        "WHERE (BCODE = '" & Trim(rBC) & _
                         "' AND (MONTH_ERN = " & cboPeriod.Text & _
                         " AND YEAR_ERN = " & Format(g_month, "yyyy").ToString & "))"

                'New code added by ching2
                If db.isConnected Then                  '--------------7142010-------------------'
                    db.CloseConnection()                '--------------7142010-------------------'
                End If                                  '--------------7142010-------------------'
                c.INITIALIZE_INI()                      '--------------7142010-------------------'
                db.ConnectDB(ls_connection)             '--------------7142010-------------------'
                rdr = db.Execute_SQL_DataReader(sqltxt) '--------------7142010-------------------'


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
                        'New code added by ching2
                        db.CloseConnection() '--------------7142010-------------------'
                        GoTo extloop

                    ElseIf ls_rep = MsgBoxResult.No Then
                        rdr.Close()
                        'New code added by ching2
                        db.CloseConnection() '--------------7142010-------------------'
                        GoTo Extsub
                    End If

                End If
                rdr.Close()
                'New code added by ching2
                db.CloseConnection() '--------------7142010-------------------'

                sqltxt = "select top 1 newid()"

                'New code added by ching2
                If db.isConnected Then                          '--------------7142010-------------------'
                    db.CloseConnection()                        '--------------7142010-------------------'
                End If                                          '--------------7142010-------------------'
                c.INITIALIZE_INI()                              '--------------7142010-------------------'
                db.ConnectDB(ls_connection)                     '--------------7142010-------------------'
                rdr = db.Execute_SQL_DataReader(sqltxt)         '--------------7142010-------------------'


                rdr.Read()
                Entryguid = rdr(0).ToString
                rdr.Close()
                'New code added by ching2
                db.CloseConnection() '--------------7142010-------------------'

                '------------EDITED WITH AGENT-------'

                For r = 1 To 3
                    Dim amount As Double = Nothing
                    Dim acfc As Double = Nothing
                    Dim cccc As String = Nothing
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

                    totalDebit = (totalDebit + amount) - (totalCredit - acfc)
                    totalCredit = totalCredit + acfc
                    totalAmount = totalDebit - totalCredit
                    ' MsgBox(totalAmount)
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

                    'New code added by ching2
                    c.INITIALIZE_INI()                                                              '--------------7142010-------------------'
                    db.ConnectDB(ls_connection)                                                     '--------------7142010-------------------'
                    If db.Execute_SQLQuery(sqltxt) < 1 Then                                         '--------------7142010-------------------'
                        db.RollbackTransaction()                                                    '--------------7142010-------------------'
                        db.CloseConnection()                                                        '--------------7142010-------------------'
                        MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)    '--------------7142010-------------------'
                        Exit Sub                                                                    '--------------7142010-------------------'
                    End If                                                                          '--------------7142010-------------------'
                    db.CloseConnection()

extcase:
                Next r 'alter ends'

                sqltxt = "Insert into allocation_corp(bcode, month_eli, year_eli, " & _
                         "date_time, sys_creator) values('" & Trim(rBC) & _
                         "','" & Format(g_month, "MM").ToString & "', '" & _
                         Format(g_month, "yyyy") & "', '" & _
                         Format(Date.Now, "yyyy-MM-dd") & "', '" & _
                         SysCreator & "')"

                'New code added by ching2
                c.INITIALIZE_INI()                                                           '--------------7142010-------------------'
                db.ConnectDB(ls_connection)                                                  '--------------7142010-------------------'
                If db.Execute_SQLQuery(sqltxt) < 1 Then                                      '--------------7142010-------------------'
                    db.RollbackTransaction()                                                 '--------------7142010-------------------'
                    db.CloseConnection()                                                     '--------------7142010-------------------'
                    MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version) '--------------7142010-------------------'
                    Exit Sub                                                                 '--------------7142010-------------------'
                End If                                                                       '--------------7142010-------------------'
                db.CloseConnection()

              

                '----------------EDITED WITH AGENT-----------'                

                sqltxt = "Insert into wu_unknownbc(bcode, dr1, dr2) values('" & _
                                  bcode & "'," & Format(CDbl(grd.get_TextMatrix(n, 5)), "##0.00") & ", " & _
                                  Format(CDbl(grd.get_TextMatrix(n, 6)), "##0.00") & ")"

                'New code by ching2
                c.INITIALIZE_INI()
                db.ConnectDB(ls_connection)
                If db.Execute_SQLQuery(sqltxt) < 1 Then
                    db.RollbackTransaction()
                    db.CloseConnection()
                    MsgBox("Error in processing data", MsgBoxStyle.Critical, gs_version)
                    Exit Sub
                End If
                db.CloseConnection()


                errCtr += 1

                sTrbc = sTrbc + "." + bcode

            End If

extloop:

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
        Dim act As String = "EDI " & "" & "Allocation" & gs_serverloc & " " & "for" & " " & txtdesc.Text.Trim
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
        picGif.Hide()
    End Sub
    Private Function GetCostCenter(ByVal code As String) As String
        GetCostCenter = "0001" & "-" & code
    End Function
    Private Sub CheckBC_Errors()
        Dim n As Integer = Nothing

        If errCtr <= 0 Then
            MsgBox("Successfuly saved all branches.", MsgBoxStyle.Information, gs_version)
            btnSave.Hide()
            btnSetup.Enabled = True
            grp.Hide()
            picGif.Hide()
        Else
            'MsgBox(Replace(sTrbc, ".", vbCrLf), MsgBoxStyle.Exclamation, errCtr & " branch code/s, not found!")
            MsgBox(Replace(sTrbc, ".", vbCrLf), MsgBoxStyle.Exclamation, gs_version)
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
        Dim c As New ClsDataEDILuzon
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim n As Integer = Nothing
        Dim month As String = Nothing
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 0)
            Dim s As String = "Select * from allocation_corp where month_eli = '" & cboPeriod.Text & "' and year_eli = '" & cboYear.Text & "' and bcode = " & g & ""
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
    Private Function errorcostcenter() As Boolean
        Dim n As Integer = Nothing
        For n = 0 To grd.Rows - 2
            Dim g As String = grd.get_TextMatrix(n, 5)
            If Len(g).ToString < 8 Then
                MsgBox(grd.get_TextMatrix(n, 1) & " " & grd.get_TextMatrix(n, 2) & " " & "Please fill correct cost center", 64, gs_version)
                errorcostcenter = True
                Exit Function
            End If
        Next
    End Function
    Private Function missingbranch() As Boolean
        Dim c As New ClsDataEDILuzon
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
                    MsgBox("Dual Branch" & " " & num1 & " " & loc1 & " " & num2 & " " & loc2, 64, gs_version)
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
                num1 = InputBox(dg.Item(r, 1) & " " & "appears to have null branch code. Please input correct branchcode", gs_version).ToString
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
        Dim amount As Double = Nothing 'debit 
        Dim acfc As Double = Nothing   'credit
        Dim cccc As String = Nothing
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
                        MsgBox(sqq1, 16, gs_version)
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
    'Private Sub Allocation_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
    '    End
    'End Sub
    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        MsgBox("Debit - 6000001, Credit - 3100002", MsgBoxStyle.OkOnly, gs_version)
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
        Label8.Text = txtdesc.Text & " " & gs_version
    End Sub
End Class