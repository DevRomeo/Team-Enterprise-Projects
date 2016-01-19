using System;
using System.Data.SqlClient;
using log4net;
using EDIdataClass;

/// <summary>
/// Summary description for Validate
/// </summary>
public class Validate
{
	public Validate()
	{}

    public Response validateUserByProgdesc(String progdesc) 
    {
        Response response = new Response();
        User resultUser = new User();
        String sql = "SELECT Top 1 RTRIM(username) AS usrName,RTRIM([password]) " +
            "AS pw FROM usersat WHERE progdesc = @progdesc ORDER BY syscreated DESC; ";
        try 
        {
            using (SqlConnection connection = new DBConnection().getMaintenanceConnection()) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@progdesc",progdesc);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        resultUser.username = reader.GetString(0);
                        resultUser.password = reader.GetString(1);
                        resultUser.progdesc = progdesc;
                        response = new Response(ResponseCode.OK, "User found", resultUser);
                    }
                    else 
                    {
                        response = new Response(ResponseCode.NotFound, "Invalid User");
                    }
                    reader.Dispose();
                    connection.Close();
                }
            }
            return response;
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }
    }//end of validateUserByProgdesc
    public Response validateUserByUserID(String userID)
    {
        Response response = new Response();
        User resultUser = new User();
        String sql = "select department,fullname from user_profile where userid = @userid";
        try
        {
            using (SqlConnection connection = new DBConnection().getMaintenanceConnection())
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userid", userID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        resultUser.department = reader.GetString(0);
                        resultUser.fullname= reader.GetString(1);
                        
                        response = new Response(ResponseCode.OK, "User found", resultUser);
                    }
                    else
                    {
                        response = new Response(ResponseCode.NotFound, "Invalid User");
                    }
                    reader.Dispose();
                    connection.Close();
                }
            }
            return response;
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }
    }//end of validateUserByProgdesc

    public Response ifAlreadySaved(String bcode, String month, String year, String descValue,String tableName,String descCol,String edi) 
    {
        Response response = new Response();
       
        //String temp = int.Parse(bcode).ToString("000");
        String sql = "Select top 1 * from "+tableName+" where month_eli =@month_eli and year_eli = @year_eli " +
                    " and bcode = @bcode and " + descCol + " = @" + descCol;
        try 
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@month_eli", month);
                    command.Parameters.AddWithValue("@year_eli", year);
                    command.Parameters.AddWithValue("@bcode",int.Parse( bcode).ToString("000"));
                    command.Parameters.AddWithValue("@" + descCol, descValue);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        response = new Response(ResponseCode.OK, "Found", true);
                        
                    }
                    else
                    {
                        response = new Response(ResponseCode.NotFound, "NotFound",false);
                    }
                    connection.Close();
                }
            }

        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }
        return response;
    }//end of ifAlreadySaved
    public Response ifAlreadySavedBase(String bcode, String month, String year,String tablename,  String edi)
    {
        Response response = new Response();
        //String temp = int.Parse(bcode).ToString("000");
        String sql = "Select top 1 * from "+tablename+" where month_eli =@month_eli and year_eli = @year_eli " +
                    " and bcode = @bcode ";
        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@month_eli", month);
                    command.Parameters.AddWithValue("@year_eli", year);
                    command.Parameters.AddWithValue("@bcode", int.Parse(bcode).ToString("000"));
                    
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        response = new Response(ResponseCode.OK, "Found", true);

                    }
                    else
                    {
                        response = new Response(ResponseCode.NotFound, "NotFound", false);
                    }
                    connection.Close();
                }
            }

        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }
        return response;
    }//end of ifAlreadySaved
    public Response isActionedEdiTable(String tablename,String g_month,String edi) 
    {

        
        Response response = new Response();
        
        String sql = "select top 1 * from "+tablename+" where bcode = 000 and date_time > @date_time";


        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@date_time", g_month);
                  
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                        response = new Response(ResponseCode.OK, "Date specified was already actioned using e-Synergy.", true);
                    else
                        response = new Response(ResponseCode.OK, "", false);
                    connection.Close();
                }
            }
            return response;
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }
        
    }
    public Response isActionedWu_kebot(String bcode, String month_ern, String year_ern, String edi) 
    {
        Response response = new Response();
      
        String sql = "SELECT TOP 1 * FROM wu_kebot WHERE (BCODE = @BCODE " +
                    " AND (MONTH_ERN = @MONTH_ERN AND YEAR_ERN = @YEAR_ERN)) ";
        
        try 
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@BCODE", bcode);
                    command.Parameters.AddWithValue("@MONTH_ERN", month_ern);
                    command.Parameters.AddWithValue("@YEAR_ERN", year_ern);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                        response = new Response(ResponseCode.OK, "Branch Code" + bcode + " already actioned", true);
                    else
                        response = new Response(ResponseCode.NotFound, "", false);
                    connection.Close();
                }
            }
            return response;    
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }
    
    }

}