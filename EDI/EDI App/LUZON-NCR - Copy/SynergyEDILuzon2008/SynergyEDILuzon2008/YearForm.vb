Public Class YearForm
    Private nonNumberEntered As Boolean = False
    Private nonCharacterEntered As Boolean = False
    Dim yearnow As Integer = Nothing

    Private Sub YearForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub

    Private Sub YearForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Year- " & gs_version
        yearnow = Now.Year
        Me.CenterToScreen()
    End Sub

    Private Sub txtyear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtYear.KeyPress
        e.Handled = True
        If IsNumeric(e.KeyChar) Or e.KeyChar = Chr(&H8) Then
            e.Handled = False
        End If

        If e.KeyChar = Chr(13) Then
            If callnull() = True Then
                MsgBox("Input correct year first before you proceed", 16, gs_version)
                Exit Sub
            Else
                callyear = txtYear.Text
                If callyear > yearnow Then
                    MsgBox("Year inputted is greater than existing year", 16, gs_version)
                    Exit Sub
                ElseIf callyear <= 2006 Then
                    MsgBox("Year inputted is lesser than accepted year reference", 16, gs_version)
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

    Private Sub btnEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnter.Click
        If callnull() = True Then
            MsgBox("Input correct year first before you proceed", 16, gs_version)
            Exit Sub
        Else
            callyear = txtyear.Text
            If callyear > yearnow Then
                MsgBox("Year inputted is greater than existing year", 16, gs_version)
                Exit Sub
            ElseIf callyear <= 2006 Then
                MsgBox("Year inputted is lesser than accepted year reference", 16, gs_version)
                Exit Sub
            Else
                Me.Hide()
                CALLEDIFUNC()
            End If
        End If


    End Sub

    Private Sub CALLEDIFUNC()
        callyear = txtyear.Text
        Me.Hide()
        If calledicategory = 1 Then
            ALLOCATION()
            'ElseIf calledicategory = 2 Then
            '    PAYROLL()
            'ElseIf calledicategory = 3 Then
            '    WUINCOME2()
        ElseIf calledicategory = 4 Then
            XOOM()
        ElseIf calledicategory = 5 Then
            GlobeSmart()
            'ElseIf calledicategory = 6 Then
            '    pea()
            'ElseIf calledicategory = 7 Then
            '    smart3()
        ElseIf calledicategory = 8 Then
            fixasset()
        ElseIf calledicategory = 9 Then
            Insurance()
            'ElseIf calledicategory = 10 Then
            '    recurring()
            'ElseIf calledicategory = 11 Then
            '    smart2()
        ElseIf calledicategory = 12 Then
            XPRESSMONEY()
        ElseIf calledicategory = 13 Then
            KPCORP()
        ElseIf calledicategory = 14 Then
            REMITTANCE()
        ElseIf calledicategory = 15 Then
            withholding()
        ElseIf calledicategory = 16 Then
            taxrem()
        ElseIf calledicategory = 17 Then
            kpincome()
        ElseIf calledicategory = 20 Then
            nsoiconnect()
        ElseIf calledicategory = 22 Then
            ssmi()
        ElseIf calledicategory = 24 Then
            nsocomm()
        ElseIf calledicategory = 25 Then
            mmd()
        ElseIf calledicategory = 28 Then
            moneygram()
        ElseIf calledicategory = 30 Then
            provision()
        ElseIf calledicategory = 31 Then
            edireport()
        ElseIf calledicategory = 33 Then
            FASTPACKINSURANCE()
        ElseIf calledicategory = 34 Then
            RRF()
        ElseIf calledicategory = 35 Then
            kpcorpincome()

        End If

    End Sub
    Private Sub kpcorpincome()
        Dim kpcorpinc As New EDIVISMIN_CORPINCOME
        kpcorpinc.Show()
        Me.Hide()
    End Sub
    Private Sub RRF()
        Dim RF As New RRFLuzon
        RF.Show()
        Me.Hide()
    End Sub
    Private Sub FASTPACKINSURANCE()
        Dim MF As New FastPakLuzon
        MF.Show()
        Me.Hide()
    End Sub
    Private Sub edireport()
        Dim ED As New EDIReportLuzon
        ED.Show()
        Me.Hide()
    End Sub
    Private Sub provision()
        Dim PR As New ProvisionLuzon
        PR.Show()
        Me.Hide()
    End Sub
    Private Sub moneygram()
        Dim mg As New MoneyGramLuzon
        mg.Show()
        Me.Hide()
    End Sub
    Private Sub mmd()
        Dim mmd As New MMDLuzon
        mmd.Show()
        Me.Hide()
    End Sub
    Private Sub nsocomm()
        Dim NSC As New NSOCOMM
        NSC.Show()
        Me.Hide()
    End Sub
    Private Sub ssmi()
        Dim ssmi As New SSMI
        ssmi.Show()
        Me.Hide()
    End Sub
    Private Sub nsoiconnect()
        Dim NSO As New NSOiConnectRep
        NSO.Show()
        Me.Hide()
    End Sub

    Private Sub kpincome()
        Dim kp As New KPIncome
        kp.Show()
        Me.Hide()
    End Sub


    Private Sub taxrem()
        Dim tx As New TaxRemLuzon
        tx.Show()
        Me.Hide()
    End Sub
    Private Sub withholding()
        Dim wht As New WTaxLuzon
        wht.Show()
        Me.Hide()
    End Sub
    Private Sub REMITTANCE()
        Dim remt As New RemittanceLuzon
        remt.Show()
        Me.Hide()
    End Sub
    Private Sub KPCORP()
        Dim kpcorp As New KPCorpLuzon
        kpcorp.Show()
        Me.Hide()
    End Sub
    Private Sub XPRESSMONEY()
        Dim XM As New XpressMoneyLuzon
        XM.Show()
        Me.Hide()
    End Sub
    Private Sub insurance()
        Dim insurance As New Insurance
        insurance.Show()
        Me.Hide()
    End Sub
    Private Sub fixasset()
        Dim FX As New FixedAssetsLuzon
        FX.Show()
        Me.Hide()
    End Sub
    Private Sub globesmart()
        Dim gs As New GlobeSmart
        gs.Show()
        Me.Hide()
    End Sub
    Private Sub XOOM()
        Dim X As New XoomLuzon
        X.Show()
        Me.Hide()
    End Sub
    Private Sub ALLOCATION()
        Dim ALLOCATION As New AllocationLuzon
        ALLOCATION.Show()
        Me.Hide()
    End Sub
    Private Sub YearForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim MAIN As New EDI_Luzon_Main
        MAIN.Show()
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim MAIN As New EDI_Luzon_Main
        MAIN.Show()
        Me.Hide()
    End Sub

    Private Sub txtYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtYear.TextChanged

    End Sub
End Class