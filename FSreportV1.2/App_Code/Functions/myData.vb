Imports Microsoft.VisualBasic
Imports DB_DLL
Imports INI_DLL
Imports System.Data.SqlClient
Imports System.Web.HttpResponse
Imports System.Security.Cryptography

Public Class myData
    Public Const ini_Name As String = "ExecReport.ini"
    Public sy_Server, sy_Name, sy_Uname, sy_Pass As String
    Public sy_Server1, sy_Name1, sy_Uname1, sy_Pass1 As String
    Public sy_Server2, sy_Name2, sy_Uname2, sy_Pass2 As String
    Public web_Server, web_Name, web_Uname, web_Pass As String
    Public user_Fullname, strTask, strCon_Synergy, strCon_Web, errorMsg, strCon_Visayas, strCon_Mindanao As String
    Public uname, passwrd, zcode As String
    'Public encrypUname, encrypPass As String

    Public Sub ReadINI()
        Dim str_Path As String = AppDomain.CurrentDomain.BaseDirectory + ini_Name
        Dim rdr As New ReadWriteINI

        web_Server = rdr.readINI("WEB PROJECTS", "SERVER", False, str_Path)
        web_Name = rdr.readINI("WEB PROJECTS", "DBNAME", False, str_Path)
        web_Uname = rdr.readINI("WEB PROJECTS", "USERNAME", False, str_Path)
        web_Pass = rdr.readINI("WEB PROJECTS", "PASSWORD", False, str_Path)
        strCon_Web = "user id=" & web_Uname & ";password=" & web_Pass & ";data source=" & web_Server & ";persist security info=False;initial catalog=" & web_Name

        sy_Server = rdr.readINI("LUZON SERVER", "SERVER", False, str_Path)
        sy_Name = rdr.readINI("LUZON SERVER", "DBNAME", False, str_Path)
        sy_Uname = rdr.readINI("LUZON SERVER", "USERNAME", False, str_Path)
        sy_Pass = rdr.readINI("LUZON SERVER", "PASSWORD", False, str_Path)
        strCon_Synergy = "user id=" & sy_Uname & ";password=" & sy_Pass & ";data source=" & sy_Server & ";persist security info=False;initial catalog=" & sy_Name

        sy_Server1 = rdr.readINI("VISAYAS SERVER", "SERVER", False, str_Path)
        sy_Name1 = rdr.readINI("VISAYAS SERVER", "DBNAME", False, str_Path)
        sy_Uname1 = rdr.readINI("VISAYAS SERVER", "USERNAME", False, str_Path)
        sy_Pass1 = rdr.readINI("VISAYAS SERVER", "PASSWORD", False, str_Path)
        strCon_Visayas = "user id=" & sy_Uname1 & ";password=" & sy_Pass1 & ";data source=" & sy_Server1 & ";persist security info=False;initial catalog=" & sy_Name1

        'sy_Server2 = rdr.readINI("MINDANAO SERVER", "SERVER", False, str_Path)
        'sy_Name2 = rdr.readINI("MINDANAO SERVER", "DBNAME", False, str_Path)
        'sy_Uname2 = rdr.readINI("MINDANAO SERVER", "USERNAME", False, str_Path)
        'sy_Pass2 = rdr.readINI("MINDANAO SERVER", "PASSWORD", False, str_Path)
        'strCon_Mindanao = "user id=" & sy_Uname2 & ";password=" & sy_Pass2 & ";data source=" & sy_Server2 & ";persist security info=False;initial catalog=" & sy_Name2
    End Sub

    Public Function ValidateUser(ByVal username As String, ByVal pass As String) As Boolean
        Dim mydb As New clsDBConnection
        Dim dr As SqlDataReader

        Dim sql As String = "SELECT fullname,task,lastlogin,zone FROM OnlineFundTransferUser WHERE usr_name='" & username & "' AND pass='" & pass & "' AND oms25='ExecSummary'"


        dr = Nothing
        ValidateUser = False
        user_Fullname = ""

        If strCon_Web = "" Then
            Exit Function
        End If

        If mydb.isConnected Then
            mydb.CloseConnection()
        End If

        If Not mydb.ConnectDB(strCon_Web) Then
            errorMsg = "Could not connect to '" & web_Name & "'. "
            Exit Function
        End If

        dr = mydb.Execute_SQL_DataReader(sql)

        If dr Is Nothing Then
            errorMsg = "You are not an authorized user."
            Exit Function
        End If

        Try
            If dr.Read Then
                user_Fullname = dr.Item("fullname")
                strTask = dr.Item("task")
                zcode = dr.Item("zone")

            Else
                errorMsg = "You are not an authorized user."
                mydb.CloseConnection()
                Exit Function
            End If
            uname = username
            passwrd = pass

            ValidateUser = True
        Catch ex As Exception
            mydb.CloseConnection()
            errorMsg = ex.Message
        End Try
        mydb.CloseConnection()
    End Function

    'Public Function ValidateUserEVP(ByVal Uname As String, ByVal pword As String) As Boolean
    '    Dim mydb As New clsDBConnection
    '    Dim dr As SqlDataReader

    '    Dim sql As String = "SELECT coalesce(firstname,'') as fname, coalesce(Mi,'') as mi,coalesce(lastname,'')as lname,coalesce(jobdescription,'N/A') as job,coalesce(email,'N/A') as email,coalesce(createDate,'') as createdate,coalesce(lastlogin,'') as logdate FROM userProfile " & _
    '                        "Where username ='" & Encrypt(Uname.Trim) & "' And pass = '" & Encrypt(pword.Trim) & "' and Active=0 and Jobdescription = 'EVP'"

    '    ValidateUserEVP = False
    '    dr = Nothing
    '    user_Fullname = ""

    '    If strCon_Web = "" Then
    '        Exit Function
    '    End If

    '    If mydb.isConnected Then
    '        mydb.CloseConnection()
    '    End If
    '    If Not mydb.ConnectDB(strCon_Web) Then
    '        errorMsg = "Could not connect to '" & web_Name & "'. "
    '        Exit Function
    '    End If


    '    dr = mydb.Execute_SQL_DataReader(sql)

    '    If dr Is Nothing Then
    '        If mydb.isConnected Then
    '            mydb.CloseConnection()
    '        End If
    '        Exit Function


    '    End If
    '    Try

    '        If dr.Read Then

    '            user_Fullname = Trim(dr.Item("fname")) + " " + Trim(dr.Item("mi")) + ". " + Trim(dr.Item("lname"))
    '            strTask = dr.Item("job")


    '        Else

    '            mydb.CloseConnection()
    '            Exit Function
    '        End If

    '        Uname = Encrypt(Uname)
    '        passwrd = Encrypt(pword)
    '        ValidateUserEVP = True
    '    Catch ex As Exception
    '        mydb.CloseConnection()
    '        errorMsg = ex.Message
    '    End Try

    'End Function


    Public Function ReturnError() As String
        Return errorMsg
    End Function


    Public Function AutoValidateUser(ByVal username As String, ByVal pass As String) As Boolean
        Dim mydb As New clsDBConnection
        Dim dr As SqlDataReader

        Dim sql As String = "SELECT (firstname +' '+MI+' '+Lastname) as fullname,jobdescription as task,lastlogin,'' as zone FROM UserProfile WHERE username='" & username & "' AND pass='" & pass & "'"

        dr = Nothing
        AutoValidateUser = False
        user_Fullname = ""

        If strCon_Web = "" Then
            Exit Function
        End If

        If mydb.isConnected Then
            mydb.CloseConnection()
        End If

        If Not mydb.ConnectDB(strCon_Web) Then
            errorMsg = "Could not connect to '" & web_Name & "'. "
            Exit Function
        End If

        dr = mydb.Execute_SQL_DataReader(sql)

        If dr Is Nothing Then
            errorMsg = "You are not an authorized user."
            Exit Function
        End If

        Try
            If dr.Read Then
                user_Fullname = dr.Item("fullname")
                strTask = dr.Item("task")
                zcode = dr.Item("zone")

            Else
                errorMsg = "You are not an authorized user."
                mydb.CloseConnection()
                Exit Function
            End If
            uname = username
            passwrd = pass
            AutoValidateUser = True
        Catch ex As Exception
            mydb.CloseConnection()
            errorMsg = ex.Message
        End Try
        mydb.CloseConnection()
    End Function
    Public Function Encrypt(ByVal myString As String) As String

        Dim myKey As String

        Dim cryptDES3 As New TripleDESCryptoServiceProvider()

        Dim cryptMD5Hash As New MD5CryptoServiceProvider()

        myKey = New String("somekeyhere")

        cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(myKey))

        cryptDES3.Mode = CipherMode.ECB

        Dim desdencrypt As ICryptoTransform = cryptDES3.CreateEncryptor()

        Dim buff() As Byte = ASCIIEncoding.ASCII.GetBytes(myString)

        Encrypt = Convert.ToBase64String(desdencrypt.TransformFinalBlock(buff, 0, buff.Length))

    End Function
End Class
