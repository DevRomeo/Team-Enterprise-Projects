Friend Class IniFile

    Private Path As String = ""
    Public Sub New(ByVal INIPath As String)
        Me.Path = INIPath
    End Sub

    Private Declare Auto Function WritePrivateProfileString Lib "kernel32" (ByVal section As String, ByVal key As String, ByVal val As String, ByVal FilePath As String) As Long
    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As System.Text.StringBuilder, ByVal size As Integer, ByVal FilePath As String) As Long

    Friend Function ReadINI(ByVal Section As String, ByVal Key As String) As String
        Dim temp As New System.Text.StringBuilder(225)
        Dim i As Long = GetPrivateProfileString(Section, Key, "", temp, temp.Capacity, Me.Path)
        Return temp.ToString
    End Function

    Friend Sub WriteINI(ByVal Section As String, ByVal Key As String, ByVal Value As String)
        WritePrivateProfileString(Section, Key, Value, Me.Path)
    End Sub

    Friend Function PasswordINI(ByVal Section As String, ByVal Key As String) As String
        Dim temp As New System.Text.StringBuilder(225)
        Dim i As Integer = GetPrivateProfileString(Section, Key, "", temp, 255, Me.Path)
        Return decryptPass(temp.ToString)
    End Function

    Friend Function decryptPass(ByVal RawStr As String) As String
        Dim i As Integer = 3
        Dim decryptedPass As String = Nothing

        While i < RawStr.Length
            decryptedPass = decryptedPass + RawStr.Substring(i - 1, 1)
            i = NextPrime(i)
        End While
        decryptPass = decryptedPass

    End Function

    Friend Function NextPrime(ByVal i As Integer) As Integer
        Dim ctr As Integer
        ctr = i + 1
        While Not isPrime(ctr)
            ctr = ctr + 1
        End While
        NextPrime = ctr
    End Function

    Friend Function isPrime(ByVal i As Integer) As Boolean
        If i = 3 Or i = 5 Then Return True
        If i Mod 2 = 0 Then Return False
        If i Mod 3 = 0 Then Return False
        If i Mod 5 = 0 Then Return False
        Return True
    End Function

    Friend Function Synergy() As String
        Dim db, user, pw, server As String
        db = ReadINI("MAIN", "db")
        user = ReadINI("MAIN", "userid")
        pw = PasswordINI("MAIN", "password")
        server = ReadINI("MAIN", "server")
        Return String.Format("user id= {0} ;password= {1} ;data source= {2} ;persist security info=False;initial catalog= {3}", user, pw, server, db)
    End Function
End Class