Public Class YearForm
    Dim yearnow As Integer = Nothing

    Private Sub YearForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim MAIN As New Main
        MAIN.Show()
        Me.Hide()
    End Sub
    Private Sub YearForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Year " & gs_version
        yearnow = Now.Year
        Me.CenterToScreen()
    End Sub

    Private Sub txtyear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtyear.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
        If e.KeyChar = Chr(13) Then
            If callnull() = True Then
                MsgBox("Input correct year first before you proceed", 64, "EDI - VISMIN")
                Exit Sub
            Else
                callyear = txtyear.Text
                If callyear > yearnow Then
                    MsgBox("Year inputted is greater than existing year", 64, "EDI - VISMIN")
                    Exit Sub
                ElseIf callyear <= 2006 Then
                    MsgBox("Year inputted is lesser than accepted year reference", 64, "EDI - VISMIN")
                    Exit Sub
                Else
                    Me.Hide()
                    CALLEDIFUNC()
                End If
            End If
        End If
    End Sub
    Private Function callnull() As Boolean
        If txtyear.Text = "" Then
            callnull = True
        End If
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If callnull() = True Then
            MsgBox("Input correct year first before you proceed", 64, "EDI - VISMIN")
            Exit Sub
        Else
            callyear = txtyear.Text
            If callyear > yearnow Then
                MsgBox("Year inputted is greater than existing year", 64, "EDI - VISMIN")
                Exit Sub
            ElseIf callyear <= 2006 Then
                MsgBox("Year inputted is lesser than accepted year reference")
                Exit Sub
            Else
                Me.Hide()
                If calledicategory = 1 Then
                    midyear()
                End If
                CALLEDIFUNC()
            End If
        End If
    End Sub
    Private Sub CALLEDIFUNC()
        callyear = txtyear.Text
        Me.Hide()
        If calledicategory = 1 Then
            midyear()
        ElseIf calledicategory = 2 Then
            kpincome()
        ElseIf calledicategory = 3 Then
            recurring()
        ElseIf calledicategory = 4 Then
            moneygram()
        ElseIf calledicategory = 5 Then
            ssmi()
        ElseIf calledicategory = 6 Then
            insurance()
        ElseIf calledicategory = 7 Then
            iconnect()
        ElseIf calledicategory = 8 Then
            globesmart()
        ElseIf calledicategory = 9 Then
            unusedsickleave()
        ElseIf calledicategory = 10 Then
            thirths()
        ElseIf calledicategory = 11 Then
            payroll()
        ElseIf calledicategory = 12 Then
            xoom()
        ElseIf calledicategory = 13 Then
            provision()
        ElseIf calledicategory = 14 Then
            sg()
        ElseIf calledicategory = 15 Then
            ICONNECTReplenish()
        ElseIf calledicategory = 16 Then
            allocation()
        ElseIf calledicategory = 17 Then
            remittance()
        ElseIf calledicategory = 18 Then
            kpcorp()
        ElseIf calledicategory = 19 Then
            Moneyexpress()
        ElseIf calledicategory = 20 Then
            mmd()
        ElseIf calledicategory = 21 Then
            withholdingtax()
        ElseIf calledicategory = 22 Then
            revolvingfund()
        ElseIf calledicategory = 23 Then
            pea()
        ElseIf calledicategory = 24 Then
            baseprovision()
        ElseIf calledicategory = 25 Then
            basesmart()
        ElseIf calledicategory = 26 Then
            baseremit()
        ElseIf calledicategory = 27 Then
            basepayroll()
        ElseIf calledicategory = 28 Then
            baseAllocation()
        ElseIf calledicategory = 29 Then
            basesecurity()
        ElseIf calledicategory = 30 Then
            base13thmonth()
        ElseIf calledicategory = 31 Then
            basemidyear()
        ElseIf calledicategory = 32 Then
            baseunusedsickleave()
        ElseIf calledicategory = 33 Then
            GlobeBaseBranch()
        ElseIf calledicategory = 34 Then
            JewEDIReport()
        ElseIf calledicategory = 35 Then
            jewediprovision()
        ElseIf calledicategory = 36 Then
            jewedirem()
        ElseIf calledicategory = 37 Then
            luz_allocation()
        ElseIf calledicategory = 38 Then
            luz_insurance()
        ElseIf calledicategory = 39 Then
            forex()
        ElseIf calledicategory = 40 Then
            NCL_Principal()
        ElseIf calledicategory = 41 Then
            NCL_INTEREST()
        ElseIf calledicategory = 42 Then
            ncl_servicecharge()
        ElseIf calledicategory = 43 Then
            Luz_mmd()
        ElseIf calledicategory = 44 Then
            airphil()
        ElseIf calledicategory = 45 Then
            luz_ssmi()
        ElseIf calledicategory = 46 Then
            protectplus_insurance()
        End If
    End Sub
    Private Sub airphil()
        Dim airphil As New VIS_AIRPHIL
        airphil.Show()
        Me.Hide()
    End Sub

    Private Sub Luz_mmd()
        Dim mmdluz As New MMDLuzon
        mmdluz.Show()
        Me.Hide()
    End Sub
    Private Sub luz_ssmi()
        Dim luzssmi As New Luzon_SSMI
        luzssmi.Show()
        Me.Hide()
    End Sub
    Private Sub protectplus_insurance()
        Dim protectplus As New EDIVISAYAS_ProtectPlus
        protectplus.Show()
        Me.Hide()
    End Sub
    Private Sub baseunusedsickleave()
        Dim bus As New EDIVISMIN_UNUSEDSICKLEAVE_BASEBRANCH
        bus.Show()
    End Sub
    Private Sub basemidyear()
        Dim bmy As New EDIVISMIN_MIDYEARBASEBRANCH
        bmy.Show()
    End Sub
    Private Sub base13thmonth()
        Dim BTM As New EDIVISMIN_13THMONTHBASEBRANCH
        BTM.Show()
    End Sub
    Private Sub basesecurity()
        Dim basesg As New EDIVISMIN_SECURITYBASEBRANCH
        basesg.Show()
    End Sub
    Private Sub baseAllocation()
        Dim basealloc As New EDIVISMIN_ALLOCATIONBASEBRANCH
        basealloc.Show()
    End Sub
    Private Sub basepayroll()
        Dim basepayroll As New EDIVISMIN_PAYROLLBASEBRANCH
        basepayroll.Show()
    End Sub
    Private Sub baseremit()
        Dim baseremit As New EDIVISMIN_REMITTANCEBASEBRANCH
        baseremit.Show()
    End Sub
    Private Sub basesmart()
        Dim s3 As New EDIVISMIN_SMARTBASEBRANCH
        s3.Show()
    End Sub
    Private Sub baseprovision()
        Dim baseprovision As New EDIVISMIN_PROVISIONBASEBRANCH
        baseprovision.Show()
    End Sub
    Private Sub pea()
        Dim pea As New EDIVISMIN_PEA
        pea.Show()

    End Sub
    Private Sub revolvingfund()
        Dim m2 As New EDIVISMIN_REVOLVING_FUND
        m2.Show()
    End Sub
    Private Sub withholdingtax()
        Dim wht As New EDIVISMIN_WHOLDINGTAX
        wht.Show()
    End Sub
    Private Sub mmd()
        Dim mmd As New EDIVISMIN_MMD
        mmd.Show()
    End Sub
    Private Sub Moneyexpress()
        Dim EP As New EDIVISMIN_XPRESS_MONEY
        EP.Show()
    End Sub
    Private Sub kpcorp()
        Dim ki As New EDIVISMIN_CORPINCOME
        ki.Show()
    End Sub
    Private Sub remittance()
        Dim remt As New EDIVISMIN_REMITTANCE
        remt.Show()
    End Sub
    Private Sub allocation()
        Dim allocation As New EDIVISMIN_ALLOCATION
        allocation.Show()
    End Sub
    Private Sub ICONNECTReplenish()
        Dim iconrep As New EDIVISMIN_ICONNECT_REPL
        iconrep.Show()
    End Sub
    Private Sub sg()
        Dim sg As New EDIVISMIN_SECURITYGUARD
        sg.Show()
    End Sub
    Private Sub provision()
        Dim pr As New EDIVISMIN_PROVISION
        pr.Show()
    End Sub
    Private Sub xoom()
        Dim X As New EDIVISMIN_XOOM
        X.Show()
    End Sub
    Private Sub payroll()
        Dim payroll As New EDIVISMIN_PAYROLL
        payroll.Show()
    End Sub
    Private Sub thirths()
        Dim thirths As New EDIVISMIN_13THMONTH
        thirths.Show()
    End Sub
    Private Sub unusedsickleave()
        Dim us As New EDIVISMIN_UNUSED_SICKLEAVE
        us.Show()
    End Sub
    Private Sub globesmart()
        Dim gs As New EDIVISMIN_GLOBESMART
        gs.Show()
        Me.Hide()
    End Sub
    Private Sub iconnect()
        Dim IC As New EDIVISMIN_ICONNECT
        IC.Show()
        Me.Hide()
    End Sub
    Private Sub midyear()
        Dim my As New EDIVISMIN_MIDYEAR
        my.Show()
    End Sub
    Private Sub kpincome()
        Dim cp As New EDIVISMIN_KPINCOME
        cp.Show()
    End Sub
    Private Sub recurring()
        Dim Recurring As New EDIVISMIN_RECURRING
        Recurring.Show()
    End Sub
    Private Sub moneygram()
        Dim MG As New EDIVISMIN_MONEYGRAM
        MG.Show()
    End Sub
    Private Sub ssmi()
        Dim ssmi As New EDIVISMIN_SSMI
        ssmi.Show()
    End Sub
    Private Sub insurance()
        Dim insurance As New EDIVISMIN_INSURANCE
        insurance.Show()
    End Sub
    Private Sub GlobeBaseBranch()
        Dim gbs As New EDIVISMIN_GlobeBillBaseBranch
        gbs.Show()
    End Sub
    Private Sub JewEDIReport()
        Dim edireport As New EDIReportLuzon
        edireport.Show()
        Me.Hide()
    End Sub
    Private Sub jewediprovision()
        Dim jewprovision As New ProvisionLuzon
        jewprovision.Show()
        Me.Hide()
    End Sub
    Private Sub jewedirem()
        Dim jewrem As New RemittanceLuzon
        jewrem.Show()
        Me.Hide()
    End Sub
    Private Sub luz_allocation()
        Dim luz_alloc As New AllocationLuzon
        luz_alloc.Show()
        Me.Hide()
    End Sub
    Private Sub luz_insurance()
        Dim luz_insurance As New Insurance
        luz_insurance.Show()
        Me.Hide()
    End Sub
    Private Sub forex()
        Dim forex As New VIS_FOREX
        forex.Show()
        Me.Hide()
    End Sub
    Private Sub NCL_Principal()
        Dim principal As New VIS_PRINCIPAL
        principal.Show()
        Me.Hide()
    End Sub
    Private Sub NCL_INTEREST()
        Dim interest As New VIS_INTEREST
        interest.Show()
        Me.Hide()
    End Sub
    Private Sub ncl_servicecharge()
        Dim sc As New VIS_SERVICECHARGE
        sc.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim MAIN As New Main
        MAIN.Show()
        Me.Hide()
    End Sub
    Private Sub txtyear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtyear.TextChanged

    End Sub
End Class