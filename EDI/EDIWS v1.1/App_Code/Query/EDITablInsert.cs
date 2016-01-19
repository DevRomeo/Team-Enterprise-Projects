using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDIdataClass;
using log4net;
using System.Data.SqlClient;

/// <summary>
/// Summary description for EDITablInsert
/// </summary>
public class EDITablInsert
{
	public EDITablInsert()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Response insertBaseEDI(List<BaseEdi> data,String tableName, String edi)
    {
        Response response = new Response();
        SqlTransaction transaction = null;
        string sql = "Insert into "+tableName+"(bcode, month_eli, year_eli, " +
                               "date_time, sys_creator,rowguid) " +
                               "values " +
                               "(@bcode, @month_eli, @year_eli, " +
                               "@date_time, @sys_creator) ";
        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@bcode", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@month_eli", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@year_eli", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@date_time", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@sys_creator", System.Data.SqlDbType.VarChar);
                    //     command.Parameters.Add("@rowguid", System.Data.SqlDbType.UniqueIdentifier);
                    command.CommandTimeout = 0;
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    try
                    {
                        int i = 0;
                        for (i = 0; i < data.Count; i++)
                        {
                            var temp = data[i];

                            command.Parameters["@bcode"].Value = temp.bcode;
                            command.Parameters["@month_eli"].Value = temp.month_eli;
                            command.Parameters["@year_eli"].Value = temp.year_eli;
                            command.Parameters["@date_time"].Value = temp.date_time;
                            command.Parameters["@sys_creator"].Value = temp.sys_creator;
                            //   command.Parameters["@rowguid"].Value = temp.rowguid;
                            command.ExecuteNonQuery();

                        }
                        if (i == data.Count)
                        {
                            transaction.Commit();
                            response.responseCode = ResponseCode.OK;
                            response.responseMessage = i.ToString() + " transactions inserted";
                        }
                        else
                        {
                            transaction.Rollback();
                            response.responseCode = ResponseCode.Error;
                            response.responseMessage = "Something went wrong ";
                        }

                        connection.Close();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        connection.Close();
                        Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                        Service.log.Error(edi + " " + e.Message, e);
                        return new Response(ResponseCode.Error, e.Message);
                    }

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
    }
    public Response insertWithDesc(List<EdiDesc> data,String tableName,String desc, String edi)
    {
        Response response = new Response();
        SqlTransaction transaction = null;
        string sql = "Insert into "+tableName+"(bcode, "+desc+",month_eli, year_eli, " +
                                "date_time, sys_creator,rowguid) " +
                                "values " +
                                "(@bcode,@" + desc + ", @month_eli, @year_eli, " +
                                "@date_time, @sys_creator) "; ;
        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@bcode", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@" + desc, System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@month_eli", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@year_eli", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@date_time", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@sys_creator", System.Data.SqlDbType.VarChar);
                    //command.Parameters.Add("@rowguid", System.Data.SqlDbType.UniqueIdentifier);
                    command.CommandTimeout = 0;
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    try
                    {
                        int i = 0;
                        for (i = 0; i < data.Count; i++)
                        {
                            var temp = data[i];


                            command.Parameters["@bcode"].Value = temp.bcode;
                            command.Parameters["@" + desc].Value = temp.desc;
                            command.Parameters["@month_eli"].Value = temp.month_eli;
                            command.Parameters["@year_eli"].Value = temp.year_eli;
                            command.Parameters["@date_time"].Value = temp.date_time;
                            command.Parameters["@sys_creator"].Value = temp.sys_creator;
                            //      command.Parameters["@rowguid"].Value = temp.rowguid;
                            command.ExecuteNonQuery();

                        }
                        if (i == data.Count)
                        {
                            transaction.Commit();
                            response.responseCode = ResponseCode.OK;
                            response.responseMessage = i.ToString() + " transactions inserted";
                        }
                        else
                        {
                            transaction.Rollback();
                            response.responseCode = ResponseCode.Error;
                            response.responseMessage = "Something went wrong ";
                        }

                        connection.Close();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        connection.Close();
                        Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                        Service.log.Error(edi + " " + e.Message, e);
                        return new Response(ResponseCode.Error, e.Message);
                    }

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
    }

    public Response insertToWu_unknownbc(String bcode, String dr1, String dr2, String edi) 
    {
        Response response = new Response();
        try 
        {
            String sql = "Insert into wu_unknownbc(bcode, dr1, dr2) " +
                            " values(@bcode, @dr1, @dr2) ";
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@bcode", bcode);
                    command.Parameters.AddWithValue("@dr1", dr1);
                    command.Parameters.AddWithValue("@dr2", dr2);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    response.responseCode = ResponseCode.OK;
                    response.responseMessage = "Success";
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
    }
}