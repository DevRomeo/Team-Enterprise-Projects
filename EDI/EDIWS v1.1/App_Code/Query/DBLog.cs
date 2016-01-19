using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using log4net;
using EDIdataClass;

/// <summary>
/// Summary description for DBLog
/// </summary>
public class DBLog
{
	public DBLog()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public EDIdataClass.Response insertToMaintainanceLogs(String datetimelog, String application, String activity, String resource, String department, String remarks)
    {
        Response response = new Response();
        String sql = "insert into edi_maintainance_logs (datetimelog, application, activity, resource, department, remarks) " +
            " values (@datetimelog, @application, @activity, @resource, @department, @remarks)";

        try 
        {
            using (SqlConnection connection = new DBConnection().getMaintenanceConnection()) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@datetimelog",datetimelog);
                    command.Parameters.AddWithValue("@application",application);
                    command.Parameters.AddWithValue("@activity",activity);
                    command.Parameters.AddWithValue("@resource",resource);
                    command.Parameters.AddWithValue("@department",department);
                    command.Parameters.AddWithValue("@remarks", remarks);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    response.responseCode = ResponseCode.OK;
                }
            }
        }
        catch (Exception e) 
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error( e.Message, e);
            return new Response(ResponseCode.Error, e.Message);
        }

        return response;
    }
}