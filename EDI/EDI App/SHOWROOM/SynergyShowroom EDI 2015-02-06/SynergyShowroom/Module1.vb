Module Module1
    Public cbotxt1 As String
    Public txt1 As String
    Public g_bol As Boolean
    Public res_id As String
    Public modifier As String
    Public ps_resid As String
    Public ls_userid As String = Nothing
    Public ls_username As String = Nothing

    'NEW ELI CODE <----- 2-6-2010
    Public MODLS_JOBTITLE As String = Nothing
    Public callyear As String = Nothing
    Public calledicategory As Integer = Nothing

    'New code added 2-19-2011
    Public database As String = Nothing

    Public ls_connection As String = Nothing

    'added 11-20-2014
    Public gs_serverloc As String = "Showroom PerBranch"
    Public gs_serverloc2 As String = "Showroom BaseBranch"
    Public gs_serverloc3 As String = "Showroom Jewellers"
    Public gs_version As String = "(EDI Version 4.1)"
    Public gs_version2 As String = "(Maintenance Version 4.1)"

End Module
