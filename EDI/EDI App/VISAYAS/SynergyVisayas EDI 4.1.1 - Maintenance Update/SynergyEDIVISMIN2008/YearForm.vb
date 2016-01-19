Public Class YearForm
    Dim yearnow As Integer = Nothing
    Public ListForm As New Dictionary(Of Int32, Object)

    Private Sub YearForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim MAIN As New Main
        MAIN.Show()
        Me.Hide()
    End Sub

    Private Sub YearForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Year " & gs_version
        yearnow = Now.Year
        Me.CenterToScreen()
        'ListForm.Add(1, New EDIVISAYAS_MIDYEAR)
        'ListForm.Add(2, New EDIVISAYAS_KPINCOME)
        'ListForm.Add(3, New EDIVISAYAS_RECURRING)
        'ListForm.Add(4, New EDIVISAYAS_MONEYGRAM)
        'ListForm.Add(5, New EDIVISAYAS_SSMI)
        'ListForm.Add(6, New EDIVISAYAS_INSURANCE)
        'ListForm.Add(7, New EDIVISAYAS_ICONNECT)
        'ListForm.Add(8, New EDIVISAYAS_GLOBESMART)
        'ListForm.Add(9, New EDIVISAYAS_UNUSED_SICKLEAVE)
        'ListForm.Add(10, New EDIVISAYAS_13THMONTH)
        'ListForm.Add(11, New EDIVISAYAS_PAYROLL)
        'ListForm.Add(12, New EDIVISAYAS_XOOM)
        'ListForm.Add(13, New EDIVISAYAS_PROVISION)
        'ListForm.Add(14, New EDIVISAYAS_SECURITYGUARD)
        'ListForm.Add(15, New EDIVISAYAS_ICONNECT_REPL)
        'ListForm.Add(16, New EDIVISAYAS_ALLOCATION)
        'ListForm.Add(17, New EDIVISAYAS_REMITTANCE)
        'ListForm.Add(18, New EDIVISAYAS_CORPINCOME)
        'ListForm.Add(19, New EDIVISAYAS_XPRESS_MONEY)
        'ListForm.Add(20, New EDIVISAYAS_MMD)
        'ListForm.Add(21, New EDIVISAYAS_WHOLDINGTAX)
        'ListForm.Add(22, New EDIVISAYAS_WHOLDINGTAX)
        'ListForm.Add(23, New EDIVISAYAS_PEA)
        'ListForm.Add(24, New EDIVISAYAS_PROVISIONBB)
        'ListForm.Add(25, New EDIVISAYAS_SMARTBASEBRANCH)
        'ListForm.Add(26, New EDIVISAYAS_REMITTANCEBB)
        'ListForm.Add(27, New EDIVISAYAS_PAYROLLBB)
        'ListForm.Add(28, New EDIVISAYAS_ALLOCATIONBB)
        'ListForm.Add(29, New EDIVISAYAS_SECURITYBB)
        'ListForm.Add(30, New EDIVISAYAS_13THMONTHBB)
        'ListForm.Add(31, New EDIVISAYAS_MIDYEARBB)
        'ListForm.Add(32, New EDIVISAYAS_UNUSEDSICKLEAVE)
        'ListForm.Add(33, New EDIVISAYAS_GlobeBillBB)
        'ListForm.Add(34, New EDIReportVISAYAS)
        'ListForm.Add(35, New ProvisionVISAYAS)
        'ListForm.Add(36, New RemittanceVISAYAS)
        'ListForm.Add(37, New AllocationVISAYAS)
        'ListForm.Add(38, New Insurance)
        'ListForm.Add(39, New VIS_FOREX)
        'ListForm.Add(40, New VIS_PRINCIPAL)
        'ListForm.Add(41, New VIS_INTEREST)
        'ListForm.Add(42, New VIS_SERVICECHARGE)
        'ListForm.Add(43, New MMDLuzon)
        'ListForm.Add(44, New VIS_AIRPHIL)
        'ListForm.Add(45, New EDIVISAYAS_ProtectPlus)
    End Sub

    Private Sub txtyear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtyear.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If
        If e.KeyChar = Chr(13) Then
            If callnull() = True Then
                MsgBox("Input correct year first before you proceed", 64, "EDI-" + ediname)
                Exit Sub
            Else
                callyear = txtyear.Text
                If callyear > yearnow Then
                    MsgBox("Year inputted is greater than existing year", 64, "EDI-" + ediname)
                    Exit Sub
                ElseIf callyear <= 2006 Then
                    MsgBox("Year inputted is lesser than accepted year reference", 64, "EDI-" + ediname)
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
            MsgBox("Input correct year first before you proceed", 64, "EDI-" + ediname)
            Exit Sub
        Else
            callyear = txtyear.Text
            If callyear > yearnow Then
                MsgBox("Year inputted is greater than existing year", 64, "EDI-" + ediname)
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
        'Dim LoadForm As Form = ListForm.Item(calledicategory)
        Me.Hide()
        'LoadForm.Show()
        Select Case calledicategory
            Case 1
                midyear()
                Exit Select
            Case 2
                kpincome()
                Exit Select
            Case 3
                recurring()
                Exit Select
            Case 4
                moneygram()
                Exit Select
            Case 5
                ssmi(strBtnName)
                Exit Select
            Case 6
                insurance()
                Exit Select
            Case 7
                iconnect()
                Exit Select
            Case 8
                globesmart()
                Exit Select
            Case 9
                unusedsickleave()
                Exit Select
            Case 10
                thirths()
                Exit Select
            Case 11
                payroll()
                Exit Select
            Case 12
                xoom()
                Exit Select
            Case 13
                provision()
                Exit Select
            Case 14
                sg()
                Exit Select
            Case 15
                ICONNECTReplenish()
                Exit Select
            Case 16
                allocation()
                Exit Select
            Case 17
                remittance()
                Exit Select
            Case 18
                kpcorp()
                Exit Select
            Case 19
                Moneyexpress()
                Exit Select
            Case 20
                mmd()
                Exit Select
            Case 21
                withholdingtax()
                Exit Select
            Case 22
                revolvingfund()
                Exit Select
            Case 23
                pea()
                Exit Select
            Case 24
                baseprovision()
                Exit Select
            Case 25
                basesmart()
                Exit Select
            Case 26
                baseremit()
                Exit Select
            Case 27
                basepayroll()
                Exit Select
            Case 28
                baseAllocation()
                Exit Select
            Case 29
                basesecurity()
                Exit Select
            Case 30
                base13thmonth()
                Exit Select
            Case 31
                basemidyear()
                Exit Select
            Case 32
                baseunusedsickleave()
                Exit Select
            Case 33
                GlobeBaseBranch()
                Exit Select
            Case 34
                JewEDIReport()
                Exit Select
            Case 35
                jewediprovision()
                Exit Select
            Case 36
                jewedirem()
                Exit Select
            Case 37
                luz_allocation()
                Exit Select
            Case 38
                luz_insurance()
                Exit Select
            Case 39
                forex()
                Exit Select
            Case 40
                NCL_Principal()
                Exit Select
            Case 41
                NCL_INTEREST()
                Exit Select
            Case 42
                ncl_servicecharge()
                Exit Select
            Case 43
                Luz_mmd()
                Exit Select
            Case 44
                airphil()
                Exit Select
            Case 45
                protectplus_insurance()
                Exit Select
        End Select
        
    End Sub
    Private Sub Luz_mmd()
        Dim mmdluz As New MMDLuzon
        mmdluz.Show()
        Me.Hide()
    End Sub
    Private Sub baseunusedsickleave()
        Dim bus As New EDIVISAYAS_UNUSEDSICKLEAVE
        bus.Show()
    End Sub
    Private Sub basemidyear()
        Dim bmy As New EDIVISAYAS_MIDYEARBB
        bmy.Show()
    End Sub
    Private Sub base13thmonth()
        Dim BTM As New EDIVISAYAS_13THMONTHBB
        BTM.Show()
    End Sub
    Private Sub basesecurity()
        Dim basesg As New EDIVISAYAS_SECURITYBB
        basesg.Show()
    End Sub
    Private Sub baseAllocation()
        Dim basealloc As New EDIVISAYAS_ALLOCATIONBB
        basealloc.Show()
    End Sub
    Private Sub basepayroll()
        Dim basepayroll As New EDIVISAYAS_PAYROLLBB
        basepayroll.Show()
    End Sub
    Private Sub baseremit()
        Dim baseremit As New EDIVISAYAS_REMITTANCEBB
        baseremit.Show()
    End Sub
    Private Sub basesmart()
        Dim s3 As New EDIVISAYAS_SMARTBASEBRANCH
        s3.Show()
    End Sub
    Private Sub baseprovision()
        Dim baseprovision As New EDIVISAYAS_PROVISIONBB
        baseprovision.Show()
    End Sub
    Private Sub pea()
        Dim pea As New EDIVISAYAS_PEA
        pea.Show()

    End Sub
    Private Sub revolvingfund()
        Dim m2 As New EDIVISAYAS_REVOLVING_FUND
        m2.Show()
    End Sub
    Private Sub withholdingtax()
        Dim wht As New EDIVISAYAS_WHOLDINGTAX
        wht.Show()
    End Sub
    Private Sub mmd()
        Dim mmd As New EDIVISAYAS_MMD
        mmd.Show()
    End Sub
    Private Sub Moneyexpress()
        Dim EP As New EDIVISAYAS_XPRESS_MONEY
        EP.Show()
    End Sub
    Private Sub kpcorp()
        Dim ki As New EDIVISAYAS_CORPINCOME
        ki.Show()
    End Sub
    Private Sub remittance()
        Dim remt As New EDIVISAYAS_REMITTANCE
        remt.Show()
    End Sub
    Private Sub allocation()
        Dim allocation As New EDIVISAYAS_ALLOCATION
        allocation.Show()
    End Sub
    Private Sub ICONNECTReplenish()
        Dim iconrep As New EDIVISAYAS_ICONNECT_REPL
        iconrep.Show()
    End Sub
    Private Sub sg()
        Dim sg As New EDIVISAYAS_SECURITYGUARD
        sg.Show()
    End Sub
    Private Sub provision()
        Dim pr As New EDIVISAYAS_PROVISION
        pr.Show()
    End Sub
    Private Sub xoom()
        Dim X As New EDIVISAYAS_XOOM
        X.Show()
    End Sub
    Private Sub payroll()
        Dim payroll As New EDIVISAYAS_PAYROLL
        payroll.Show()
    End Sub
    Private Sub thirths()
        Dim thirths As New EDIVISAYAS_13THMONTH
        thirths.Show()
    End Sub
    Private Sub unusedsickleave()
        Dim us As New EDIVISAYAS_UNUSED_SICKLEAVE
        us.Show()
    End Sub
    Private Sub globesmart()
        Dim gs As New EDIVISAYAS_GLOBESMART
        gs.Show()
        Me.Hide()
    End Sub
    Private Sub iconnect()
        Dim IC As New EDIVISAYAS_ICONNECT
        IC.Show()
        Me.Hide()
    End Sub
    Private Sub midyear()
        Dim myr As New EDIVISAYAS_MIDYEAR
        myr.Show()
    End Sub
    Private Sub kpincome()
        Dim cp As New EDIVISAYAS_KPINCOME
        cp.Show()
    End Sub
    Private Sub recurring()
        Dim Recurring As New EDIVISAYAS_RECURRING
        Recurring.Show()
    End Sub
    Private Sub moneygram()
        Dim MG As New EDIVISAYAS_MONEYGRAM
        MG.Show()
    End Sub
    Private Sub ssmi(ByVal str As String)
        Dim ssmi As New EDIVISAYAS_SSMI
        If str = "Button9" Then
            ssmi.Text = "SSMI-" & ediname & " " & gs_serverloc & " " & gs_version
        ElseIf str = "Button46" Then
            ssmi.Text = "SSMI-" & ediname & " " & gs_serverloc3 & " " & gs_version
        End If
        ssmi.Show()
    End Sub
    Private Sub insurance()
        Dim insurance As New EDIVISAYAS_INSURANCE
        insurance.Show()
    End Sub
    Private Sub GlobeBaseBranch()
        Dim gbs As New EDIVISAYAS_GlobeBillBB
        gbs.Show()
    End Sub
    Private Sub JewEDIReport()
        Dim edireport As New EDIReportVISAYAS
        edireport.Show()
        Me.Hide()
    End Sub
    Private Sub jewediprovision()
        Dim jewprovision As New ProvisionVISAYAS
        jewprovision.Show()
        Me.Hide()
    End Sub
    Private Sub jewedirem()
        Dim jewrem As New RemittanceVISAYAS
        jewrem.Show()
        Me.Hide()
    End Sub
    Private Sub luz_allocation()
        Dim luz_alloc As New AllocationVISAYAS
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
    Private Sub airphil()
        Dim airphil As New VIS_AIRPHIL
        airphil.Show()
        Me.Hide()
    End Sub
    Private Sub protectplus_insurance()
        Dim protectplus As New EDIVISAYAS_ProtectPlus
        protectplus.Show()
        Me.Hide()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim MAIN As New Main
        MAIN.Show()
        Me.Hide()
    End Sub
End Class