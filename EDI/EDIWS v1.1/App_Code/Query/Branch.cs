using System;
using System.Data.SqlClient;
using log4net;
using EDIdataClass;
using System.Collections.Generic;
//using Dapper;
/// <summary>
/// v1.1 add dapper .dll for new query (testing)
/// </summary>
public class Branch
{
	public Branch()	{}
    public Response getAllBoskp(String edi) 
    {
        Response response = new Response();
        string sql = "select  boscode,kpcode,description,class_01,class_02, " +
        " class_03, class_04 from boskp order by boscode";
        List<Boskp> kpbranch = new List<Boskp>();
        try 
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi)) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) 
                    {
                        var boskp = new Boskp();
                        boskp.boscode = reader.GetString(0); boskp.kpcode = reader.GetString(1);                   
                        boskp.description = reader.GetString(2); boskp.class_01 = reader.GetString(3);
                        boskp.class_03 = reader.GetString(4); boskp.class_04 = reader.GetString(5);
                        kpbranch.Add(boskp);
                    }
                    reader.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            //    kpbranch =(List<Boskp>)connection.Query<Boskp>(sql);
               // Console.WriteLine("asdf");  
               
            }
            response.responseCode = ResponseCode.OK;
            response.responseData = kpbranch;

        }
        catch (Exception e) 
        {
            response.responseCode = ResponseCode.Error;
            response.responseMessage = e.Message;
        }
        
        return response;
    }//end of getAllBoskp
    public Response getBoskp(String kpcode,String edi) 
    {
        Response response = new Response();
        Boskp boskp = new Boskp();
        String sql="select top 1 * from boskp where kpcode like @kpcode";
        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@kpcode", kpcode);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        boskp.boscode = reader.GetString(0);
                        boskp.kpcode = reader.GetString(1);
                        boskp.description = reader.GetString(2);
                        boskp.class_01 = reader.GetString(3);
                        boskp.class_02 = reader.GetString(4);
                        boskp.class_03 = reader.GetString(5);
                        boskp.class_04 = reader.GetString(6);
                        response = new Response(ResponseCode.Error, "Record Found", boskp);

                    }
                    else
                    {
                        response.responseCode = ResponseCode.NotFound;
                    }
                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            response = new Response(ResponseCode.Error, e.Message);
        }
        
        return response;
    }//end of getBoskp
    public Response getBranchInfo(String bcode, String edi) 
    {
        Response response = new Response();
        try
        {
        bcode = Convert.ToInt32(bcode).ToString("000");
 
        EDIdataClass.Branch branch = new EDIdataClass.Branch();
        String sql = "select bedrnr, bedrnm from BEDRYF where bedrnr =@bedrnr ";
       
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi)) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@bedrnr", bcode);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        branch.bedrnr = reader.GetString(0);
                        branch.bedrnm = reader.GetString(1);
                        response = new Response(ResponseCode.OK, "Record Found", branch);
                    }
                    else 
                    {
                        response = new Response(ResponseCode.NotFound, "Branch not found");
                    }

                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            response = new Response(ResponseCode.Error, e.Message);
        }

        return response;
    }//end ofgetBranchInfo
    public Response getBranchInfoBOSKPViaBoscode(String bcode, String edi) 
    {
        Response response = new Response();
        String sql = "select boscode,kpcode,[description] as description,class_01,class_02,class_03,class_04 from boskp  where boscode  =@boscode";
        Boskp branch = new Boskp();
        try 
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi)) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@boscode", bcode);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        response.responseCode = ResponseCode.OK;
                        reader.Read();
                        branch.boscode = reader.GetString(0);
                        branch.kpcode = reader.GetString(1);
                        branch.description = reader.GetString(2);
                        branch.class_01= reader.GetString(3);
                        branch.class_02 = reader.GetString(4);
                        branch.class_03 = reader.GetString(5);
                        branch.class_04 = reader.GetString(6);
                        response.responseData = branch;
                    }
                    else
                    {
                        response.responseCode = ResponseCode.NotFound;

                    }
                    reader.Dispose();
                    connection.Close();
                }

            }
            response.responseData = branch;
            
        }
        catch (Exception e) 
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            response = new Response(ResponseCode.Error, e.Message);
        }
        return response;
    }//
    public Response getBranchInfoBOSKP(String kpcode, String edi)
    {
        Response response = new Response();
        EDIdataClass.Boskp branch = new EDIdataClass.Boskp();
        String sql = "select boscode,kpcode,[description],class_01,class_02,class_03,class_04 from boskp  where kpcode  =@kpcode  ";
        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@kpcode ", kpcode);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                      
                        branch.boscode = reader.GetString(0);
                        branch.kpcode = reader.GetString(1);
                        branch.description = reader.GetString(2);
                        branch.class_01 = reader.GetString(3);
                        branch.class_02 = reader.GetString(4);
                        branch.class_03 = reader.GetString(5);
                        branch.class_04 = reader.GetString(6);
                        response = new Response(ResponseCode.OK, "Record Found", branch);
                    }
                    else
                    {
                        response = new Response(ResponseCode.NotFound, "Branch not found");
                    }

                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            response = new Response(ResponseCode.Error, e.Message);
        }

        return response;
    }//end ofgetBranchInfo
    public Response getAllCorporatePartner(String edi) 
    {
        Response response = new Response();
        List<Corporate_Partners> corpPartners= new List<Corporate_Partners>();
        string sql = "select CORPCODE,CORPNAME,GLDEBIT,GLCREDIT " +
                    "  from corporate_partners order by corpcode asc ";
        try 
        {
            using(SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using(SqlCommand command = new SqlCommand(sql,connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader() ;
                    while (reader.Read())
                    {
                        var corpartners =new Corporate_Partners();
                        corpartners.CORPCODE= reader.GetString(0);
                        corpartners.CORPNAME=reader.GetString(1);
                        corpartners.GLDEBIT=reader.GetString(2);
                        corpartners.GLCREDIT = reader.GetString(3);
                        corpPartners.Add(corpartners);
                    }


                }
             //   corpPartners = (List<Corporate_Partners>)connection.Query<Corporate_Partners>(sql);
            }
            response.responseCode =ResponseCode.OK;
            response.responseData = corpPartners;
        }
        catch (Exception e) 
        {
            response.responseCode = ResponseCode.Error;
            response.responseMessage = e.Message;
        }
        return response;
    } //end of getAllCorporatePartner
    public Response getCorporatePartner(String CORPCODE, String edi)
    {
        Response response = new Response();
        EDIdataClass.Corporate_Partners branch = new EDIdataClass.Corporate_Partners();
        String sql = "Select CORPCODE, CORPNAME , GLDEBIT  ,   GLCREDIT  FROM Corporate_partners where CORPCODE=@CORPCODE";
        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CORPCODE ", CORPCODE);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();

                        branch.CORPCODE = reader.GetString(0);
                        branch.CORPNAME = reader.GetString(1);
                        branch.GLDEBIT = reader.GetString(2);
                        branch.GLCREDIT = reader.GetString(3);
                        
                        response = new Response(ResponseCode.OK, "Record Found", branch);
                    }
                    else
                    {
                        response = new Response(ResponseCode.NotFound, "Branch not found");
                    }

                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            response = new Response(ResponseCode.Error, e.Message);
        }

        return response;
    }//end ofgetBranchInfo
    public Response addBoskp(Boskp data, String edi) 
    {
        Response response = new Response();
        String sql = "INSERT INTO BOSKP (BOSCODE,KPCODE,DESCRIPTION,CLASS_01,CLASS_02,CLASS_03,CLASS_04 ,rowguid) " +
                    " VALUES (@BOSCODE,@KPCODE,@DESCRIPTION,@CLASS_01,@CLASS_02,@CLASS_03,@CLASS_04,newid() )";
        try 
        {
            int count = 0;
          
            using(SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                //count =connection.Execute(sql,
                //    new {data.boscode,data.kpcode,data.description,data.class_01,
                //        data.class_02,data.class_03,data.class_04 });
                using (SqlCommand command = new SqlCommand(sql,connection)) 
                {
                    command.Parameters.AddWithValue("@BOSCODE", data.boscode);
                    command.Parameters.AddWithValue("@KPCODE", data.kpcode);
                    command.Parameters.AddWithValue("@DESCRIPTION", data.description);
                    command.Parameters.AddWithValue("@CLASS_01", data.class_01);
                    command.Parameters.AddWithValue("@CLASS_02", data.class_02);
                    command.Parameters.AddWithValue("@CLASS_03", data.class_03);
                    command.Parameters.AddWithValue("@CLASS_04", data.class_04);
                    connection.Open();

                    var x = command.ExecuteNonQuery();

                    count = x;
                    connection.Close();
               
                }
            }
            
           
            if (count > 0) 
            {
                response.responseCode = ResponseCode.OK;
            }
            else
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage = "Unable to add ";
            }
        }
        catch (Exception e) 
        {
            response.responseCode = ResponseCode.Error;
            response.responseMessage = e.Message;
        }
        return response;
    }//end of addBoskp
    public Response addCorpPartner(Corporate_Partners data,String edi)
    {
        Response response = new Response();

        String sql = "INSERT INTO  corporate_partners(corpcode,corpname,gldebit,glcredit) " +
                    " VALUES (@corpcode,@corpname,@gldebit,@glcredit)";
        try
        {
            int count = 0;

            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                using(SqlCommand command = new SqlCommand(sql,connection))
                {
                    command.Parameters.AddWithValue("@corpcode",data.CORPCODE);
                    command.Parameters.AddWithValue("@corpname",data.CORPNAME);
                    command.Parameters.AddWithValue("@gldebit", data.GLDEBIT );
                    command.Parameters.AddWithValue("@glcredit", data.GLCREDIT);
                    connection.Open();
                    count = command.ExecuteNonQuery();
                    connection.Close();

                }
             
            }
            if (count > 0)
            {
                response.responseCode = ResponseCode.OK;
            }

            else
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage = "Unable to add corporate partner";
            }
        }
        catch (Exception e)
        {
            response.responseCode = ResponseCode.Error;
            response.responseMessage = e.Message;
        }
        return response;
    }//end of addCorpPartner
    public Response updateBoskp(Boskp data,String bcode,String edi) 
    {
        Response response = new Response();
        String sql = "UPDATE  BOSKP SET BOSCODE =@BOSCODE ,KPCODE = @KPCODE,DESCRIPTION=@DESCRIPTION " +
                    " WHERE  BOSCODE =@bcode";
        try
        {
            int count =0;
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
               // count = connection.Execute(sql, new { data.boscode, data.kpcode, data.description, bcode });
                using(SqlCommand command  = new SqlCommand(sql,connection))
                {
                    command.Parameters.AddWithValue("@BOSCODE",data.boscode);
                    command.Parameters.AddWithValue("@KPCODE",data.kpcode);
                    command.Parameters.AddWithValue("@DESCRIPTION",data.description);
                    command.Parameters.AddWithValue("@bcode", bcode);
                    connection.Open();
                    count = command.ExecuteNonQuery();
                    connection.Close();
                }
                
            }
            if (count > 0)
            {
                response.responseCode = ResponseCode.OK;
            }
            else 
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage = "Something went wrong";
            }

        }
        catch (Exception e) 
        {
            response.responseMessage = e.Message;
            response.responseCode = ResponseCode.Error;
        }
        
        return response;
    } //end of updateBoskp
    public Response updateCorpPartner(Corporate_Partners data, String corpcode, String edi) 
    {
        Response response = new Response();
        String sql = "Update corporate_partners set corpname=@corpname, " +
                " gldebit=@gldebit, glcredit=@glcredit where corpcode=@corpcode";

        try 
        {
            int count = 0;
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
              //  count=   connection.Execute(sql,new {data.CORPNAME,data.GLDEBIT,data.GLCREDIT,corpcode});//
                using(SqlCommand command  = new SqlCommand(sql,connection))
                {
                    command.Parameters.AddWithValue("@corpname", data.CORPNAME);
                    command.Parameters.AddWithValue("@gldebit",data.GLDEBIT);
                    command.Parameters.AddWithValue("@glcredit",data.GLCREDIT);
                    command.Parameters.AddWithValue("@corpcode",corpcode);
                    connection.Open();
                    count = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            if (count > 0)
            {
                response.responseCode = ResponseCode.OK;
            }
            else
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage = "Something went wrong";
            }
        }
        catch (Exception e) 
        {
            response.responseCode = ResponseCode.Error;
            response.responseMessage = e.Message;
        }
        return response;
    } //end of updateCorpPartner
    
}