using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for OnlineFundTransferUserQuery
/// </summary>
public class OnlineFundTransferUserQuery
{
	public OnlineFundTransferUserQuery()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Response addUser(User user) 
    {
        Response response = new Response();
        String sql ="INSERT INTO OnlineFundTransferUser " +
                    "VALUES(@usr_name,@pass,@fullname "+
                    ",@task,getdate(),@oms25,@zone)" ;
        try 
        {
            using (SqlConnection connection = new Connection().getConnection("WEB PROJECTS")) 
            {
                using(SqlCommand command = new SqlCommand(sql,connection))
                {
                    command.Parameters.AddWithValue("@usr_name", user.usr_name);
                    command.Parameters.AddWithValue("@pass", user.pass);
                    command.Parameters.AddWithValue("@fullname", user.fullname);
                    command.Parameters.AddWithValue("@task", user.task);
                    command.Parameters.AddWithValue("@oms25", user.oms25);
                    command.Parameters.AddWithValue("@zone", user.zone);
                    connection.Open();
                    command.Prepare();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            response.respcode = 0;
            response.msg = e.Message;
        }
        return response;
    }
    public Response deactivateUser(String usr_name) 
    {
        Response response = new Response();
        String sql = "update OnlineFundTransferUser set oms25='Deactivated' where usr_name =ltrim(rtrim('@usr_name'))";
        try
        {
            using(SqlConnection connection  =new Connection().getConnection("WEB PROJECTS") )
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.AddWithValue("@usr_name", usr_name);
                command.Connection = connection;
                connection.Open();
                int count = command.ExecuteNonQuery();
                if (count > 0)
                {
                    response.respcode = 1;
                    response.msg = "user :" + usr_name + " deactivated";
                }
                else 
                {
                    response.respcode = 0;
                    response.msg = "User not found";
                }
                command.Dispose();
                connection.Close();
            }
        }
        catch (Exception e) 
        {
            response.respcode =0;
            response.msg = e.Message;
        }
        return response;
    }
}
