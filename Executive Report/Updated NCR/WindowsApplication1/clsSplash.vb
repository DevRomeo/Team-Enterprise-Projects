Public Class clsSplash
    Private frmSplashScreen As New frmSplash

    Public Sub ShowSplash(ByVal as_message As String)
        If frmSplashScreen Is Nothing Then
            frmSplashScreen = New frmSplash
        End If
        frmSplashScreen.lblMessage.Text = as_message
        frmSplashScreen.Show()
        'frmSplashScreen.TopMost = True
        frmSplashScreen.Activate()
    End Sub

    Public Function CloseSplash()
        If Not frmSplashScreen Is Nothing Then
            frmSplashScreen.Close()
            frmSplashScreen.Dispose()
            frmSplashScreen = Nothing
        End If
    End Function

End Class
