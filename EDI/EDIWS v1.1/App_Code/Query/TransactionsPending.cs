using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDIdataClass;
using System.Data.SqlClient;
using log4net;
/// <summary>
/// Summary description for TransactionsPending
/// </summary>
public class TransactionsPending
{

	public TransactionsPending()
	{}
    public Response insert(List<transactionsPending> data, String edi) 
    {
        Response response = new Response();
        string sql = "Insert into TransactionsPending" +
                       "(paymentmethod,freefield4,freefield5, CompanyCode, TransactionType, TransactionDate, " +
                       "FinYear,FinPeriod, Description, CompanyAccountCode,EntryNumber,CurrencyAliasAC,AmountDebitAC,AmountCreditAC,  " +
                       "CurrencyAliasFC,AmountDebitFC,AmountCreditFC,VATCode,ProcessNumber,ProcessLineCode, res_id, oorsprong,docdate," +
                       "vervdatfak,faktuurnr,syscreator,sysmodifier,EntryGuid,companycostcentercode )  " +
                       "values " +
                       "(@paymentmethod,@freefield4,@freefield5,@CompanyCode, @TransactionType, @TransactionDate, " +
                       "@FinYear,@FinPeriod, @Description, @CompanyAccountCode,@EntryNumber,@CurrencyAliasAC,@AmountDebitAC,@AmountCreditAC,  " +
                       "@CurrencyAliasFC,@AmountDebitFC,@AmountCreditFC,@VATCode,@ProcessNumber,@ProcessLineCode, @res_id, @oorsprong,@docdate," +
                       "@vervdatfak,@faktuurnr,@syscreator,@sysmodifier,@EntryGuid,@companycostcentercode )  ";
        SqlTransaction transaction = null;

        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    
                        command.Parameters.Add("@paymentmethod", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@freefield4", System.Data.SqlDbType.Float);
                        command.Parameters.Add("@freefield5", System.Data.SqlDbType.Float);
                        command.Parameters.Add("@CompanyCode", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@TransactionType", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@TransactionDate", System.Data.SqlDbType.VarChar);
                        command.Parameters.Add("@FinYear", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@FinPeriod", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
                        command.Parameters.Add("@CompanyAccountCode", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@EntryNumber", System.Data.SqlDbType.VarChar);
                        command.Parameters.Add("@CurrencyAliasAC", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@AmountDebitAC", System.Data.SqlDbType.Float);
                        command.Parameters.Add("@AmountCreditAC", System.Data.SqlDbType.Float);
                        command.Parameters.Add("@CurrencyAliasFC", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@AmountDebitFC", System.Data.SqlDbType.Float);
                        command.Parameters.Add("@AmountCreditFC", System.Data.SqlDbType.Float);
                        command.Parameters.Add("@VATCode", System.Data.SqlDbType.VarChar);
                        command.Parameters.Add("@ProcessNumber", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@ProcessLineCode", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@res_id", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@oorsprong", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@docdate", System.Data.SqlDbType.VarChar);
                        command.Parameters.Add("@vervdatfak", System.Data.SqlDbType.VarChar);
                        command.Parameters.Add("@faktuurnr", System.Data.SqlDbType.Char);
                        command.Parameters.Add("@syscreator", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@sysmodifier", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@EntryGuid", System.Data.SqlDbType.UniqueIdentifier);
                        command.Parameters.Add("@companycostcentercode", System.Data.SqlDbType.VarChar);
                        command.CommandTimeout = 0;
                        connection.Open();

                        transaction =connection.BeginTransaction();
                        command.Transaction = transaction;
                        int i=0;
                       try
                       {
                        for ( i = 0; i < data.Count; i++) 
                        {
                            var temp = data[i];
                            command.Parameters["@paymentmethod"].Value = temp.paymentmethod;
                            command.Parameters["@freefield4"].Value = temp.freefield4;
                            command.Parameters["@freefield5"].Value = temp.freefield5;
                            command.Parameters["@CompanyCode"].Value = temp.CompanyCode;
                            command.Parameters["@TransactionType"].Value = temp.TransactionType;
                            command.Parameters["@TransactionDate"].Value = temp.TransactionDate;
                            command.Parameters["@FinYear"].Value = temp.FinYear;
                            command.Parameters["@FinPeriod"].Value = temp.FinPeriod;
                            command.Parameters["@Description"].Value = temp.Description;
                            command.Parameters["@CompanyAccountCode"].Value = temp.CompanyAccountCode;
                            command.Parameters["@EntryNumber"].Value = "";
                            command.Parameters["@CurrencyAliasAC"].Value = temp.CurrencyAliasAC;
                            command.Parameters["@AmountDebitAC"].Value = temp.AmountDebitAC;
                            command.Parameters["@AmountCreditAC"].Value = temp.AmountCreditAC;
                            command.Parameters["@CurrencyAliasFC"].Value = temp.CurrencyAliasFC;
                            command.Parameters["@AmountDebitFC"].Value = temp.AmountDebitFC;
                            command.Parameters["@AmountCreditFC"].Value = temp.AmountCreditFC;
                            command.Parameters["@VATCode"].Value = temp.VATCode;
                            command.Parameters["@ProcessNumber"].Value = temp.ProcessNumber;
                            command.Parameters["@ProcessLineCode"].Value = temp.ProcessLineCode;
                            command.Parameters["@res_id"].Value = temp.res_id;
                            command.Parameters["@oorsprong"].Value = temp.oorsprong;
                            command.Parameters["@docdate"].Value = temp.docdate;
                            command.Parameters["@vervdatfak"].Value = temp.vervdatfak;
                            command.Parameters["@faktuurnr"].Value = "";
                            command.Parameters["@syscreator"].Value = temp.syscreator;
                            command.Parameters["@sysmodifier"].Value = temp.sysmodifier;
                            command.Parameters["@EntryGuid"].Value = new Guid(temp.EntryGuid);
                            command.Parameters["@companycostcentercode"].Value = temp.companycostcentercode;
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
    public Response insertBaseEdi(List<transactionsPending> data,List<BaseEdi> ediTableData,String tableName, String edi)
    {
        if (data.Count <= 0)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + "No record found");
            return new Response(ResponseCode.Error, "No record found");
        }
        Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Service.log.Error(edi +" "+data.Count.ToString()+" Records Found" );
           
        Response response = new Response();
        string edisql = "Insert into " + tableName + "(bcode, month_eli, year_eli, " +
                             "date_time, sys_creator,rowguid) " +
                             "values " +
                             "(@bcode, @month_eli, @year_eli, " +
                             "@date_time, @sys_creator,NEWID()) ";
        string sql = "Insert into TransactionsPending" +
                       "(paymentmethod,freefield4,freefield5, CompanyCode, TransactionType, TransactionDate, " +
                       "FinYear,FinPeriod, Description, CompanyAccountCode,EntryNumber,CurrencyAliasAC,AmountDebitAC,AmountCreditAC,  " +
                       "CurrencyAliasFC,AmountDebitFC,AmountCreditFC,VATCode,ProcessNumber,ProcessLineCode, res_id, oorsprong,docdate," +
                       "vervdatfak,faktuurnr,syscreator,sysmodifier,EntryGuid,companycostcentercode,batchno )  " +
                       "values " +
                       "(@paymentmethod,@freefield4,@freefield5,@CompanyCode, @TransactionType, @TransactionDate, " +
                       "@FinYear,@FinPeriod, @Description, @CompanyAccountCode,@EntryNumber,@CurrencyAliasAC,@AmountDebitAC,@AmountCreditAC,  " +
                       "@CurrencyAliasFC,@AmountDebitFC,@AmountCreditFC,@VATCode,@ProcessNumber,@ProcessLineCode, @res_id, @oorsprong,@docdate," +
                       "@vervdatfak,@faktuurnr,@syscreator,@sysmodifier,@EntryGuid,@companycostcentercode,@batchno )  ";
        SqlTransaction transaction = null;

        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                int t = 0;
                int ed = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.Add("@paymentmethod", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@freefield4", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@freefield5", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@CompanyCode", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@TransactionType", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@TransactionDate", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@FinYear", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@FinPeriod", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@CompanyAccountCode", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@EntryNumber", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@CurrencyAliasAC", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@AmountDebitAC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@AmountCreditAC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@CurrencyAliasFC", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@AmountDebitFC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@AmountCreditFC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@VATCode", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@ProcessNumber", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@ProcessLineCode", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@res_id", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@oorsprong", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@docdate", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@vervdatfak", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@faktuurnr", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@syscreator", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@sysmodifier", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@EntryGuid", System.Data.SqlDbType.UniqueIdentifier);
                    command.Parameters.Add("@companycostcentercode", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@batchno", System.Data.SqlDbType.VarChar);
                    command.CommandTimeout = 0;
                    connection.Open();

                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    int i = 0;
                    try
                    {
                        for (i = 0; i < data.Count; i++)
                        {
                            var temp = data[i];
                            command.Parameters["@paymentmethod"].Value = temp.paymentmethod;
                            command.Parameters["@freefield4"].Value = (temp.freefield4.Trim() == "") ? 0 : Convert.ToDouble(temp.freefield4);
                            command.Parameters["@freefield5"].Value = temp.freefield5.Trim().Equals("") ? 0 : Convert.ToDouble(temp.freefield5);
                            command.Parameters["@CompanyCode"].Value = temp.CompanyCode;
                            command.Parameters["@TransactionType"].Value = Convert.ToInt32(temp.TransactionType);
                            command.Parameters["@TransactionDate"].Value = temp.TransactionDate;
                            command.Parameters["@FinYear"].Value = Convert.ToInt32(temp.FinYear);
                            command.Parameters["@FinPeriod"].Value = Convert.ToInt32(temp.FinPeriod);
                            command.Parameters["@Description"].Value = temp.Description;
                            command.Parameters["@CompanyAccountCode"].Value = temp.CompanyAccountCode;
                            command.Parameters["@EntryNumber"].Value = "";
                            command.Parameters["@CurrencyAliasAC"].Value = temp.CurrencyAliasAC;
                            command.Parameters["@AmountDebitAC"].Value = Convert.ToDouble(temp.AmountDebitAC);
                            command.Parameters["@AmountCreditAC"].Value = Convert.ToDouble(temp.AmountCreditAC);
                            command.Parameters["@CurrencyAliasFC"].Value = temp.CurrencyAliasFC;
                            command.Parameters["@AmountDebitFC"].Value = Convert.ToDouble(temp.AmountDebitFC);
                            command.Parameters["@AmountCreditFC"].Value = Convert.ToDouble(temp.AmountCreditFC);
                            command.Parameters["@VATCode"].Value = temp.VATCode;
                            command.Parameters["@ProcessNumber"].Value = Convert.ToInt32(temp.ProcessNumber);
                            command.Parameters["@ProcessLineCode"].Value = temp.ProcessLineCode;
                            command.Parameters["@res_id"].Value = Convert.ToInt32(temp.res_id);
                            command.Parameters["@oorsprong"].Value = temp.oorsprong;
                            command.Parameters["@docdate"].Value = temp.docdate;
                            command.Parameters["@vervdatfak"].Value = temp.vervdatfak;
                            command.Parameters["@faktuurnr"].Value = "";
                            command.Parameters["@syscreator"].Value = Convert.ToInt32(temp.syscreator);
                            command.Parameters["@sysmodifier"].Value = Convert.ToInt32(temp.sysmodifier);
                            command.Parameters["@EntryGuid"].Value = new Guid(temp.EntryGuid);
                            command.Parameters["@companycostcentercode"].Value = temp.companycostcentercode;
                            command.Parameters["@batchno"].Value = temp.batchNo;
                            
                            command.ExecuteNonQuery();
                        }
                        t = i;
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

                using (SqlCommand ediCommand = new SqlCommand(edisql, connection)) 
                {
                    ediCommand.Parameters.Add("@bcode", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@month_eli", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@year_eli", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@date_time", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@sys_creator", System.Data.SqlDbType.VarChar);
                    //     command.Parameters.Add("@rowguid", System.Data.SqlDbType.UniqueIdentifier);
                    ediCommand.CommandTimeout = 0;
                    ediCommand.Transaction = transaction;
                    try
                    {
                        int i = 0;
                        for (i = 0; i < ediTableData.Count; i++)
                        {
                            var temp = ediTableData[i];

                            ediCommand.Parameters["@bcode"].Value = temp.bcode;
                            ediCommand.Parameters["@month_eli"].Value = temp.month_eli;
                            ediCommand.Parameters["@year_eli"].Value = temp.year_eli;
                            ediCommand.Parameters["@date_time"].Value = temp.date_time;
                            ediCommand.Parameters["@sys_creator"].Value = temp.sys_creator;
                            //   command.Parameters["@rowguid"].Value = temp.rowguid;
                            ediCommand.ExecuteNonQuery();

                        }
                        ed = i;
                        if ((ed ==ediTableData.Count) && (t==data.Count))
                        {
                            transaction.Commit();
                            response.responseCode = ResponseCode.OK;
                            response.responseMessage = t.ToString() + " transactions inserted";
                            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                            Service.log.Info(edi + " " + t.ToString() + " Records Inserted");
                        }
                        else
                        {
                            transaction.Rollback();
                            response.responseCode = ResponseCode.Error;
                            response.responseMessage = "Something went wrong ";
                            Service.log.Info(edi + " Something went wrong transaction rollback");
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
    public Response insertDescEdi(List<transactionsPending> data, List<EdiDesc> ediTableData, String tableName, String desc,String edi)
    {
        if (data.Count <= 0) 
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + "No record found");
            return new Response(ResponseCode.Error, "No record found");
        }

        Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Service.log.Error(edi + " " +  data.Count.ToString() + " Records Found");

        Response response = new Response();
        string edisql =  "Insert into " + tableName + "(bcode, " + desc + ",month_eli, year_eli, " +
                         "date_time, sys_creator,rowguid) " +
                         "values " +
                         "(@bcode,@" + desc + ", @month_eli, @year_eli, " +
                         "@date_time, @sys_creator,newid()) "; 

        string sql = "Insert into TransactionsPending" +
                       "(paymentmethod,freefield4,freefield5, CompanyCode, TransactionType, TransactionDate, " +
                       "FinYear,FinPeriod, Description, CompanyAccountCode,EntryNumber,CurrencyAliasAC,AmountDebitAC,AmountCreditAC,  " +
                       "CurrencyAliasFC,AmountDebitFC,AmountCreditFC,VATCode,ProcessNumber,ProcessLineCode, res_id, oorsprong,docdate," +
                       "vervdatfak,faktuurnr,syscreator,sysmodifier,EntryGuid,companycostcentercode,batchno )  " +
                       "values " +
                       "(@paymentmethod,@freefield4,@freefield5,@CompanyCode, @TransactionType, @TransactionDate, " +
                       "@FinYear,@FinPeriod, @Description, @CompanyAccountCode,@EntryNumber,@CurrencyAliasAC,@AmountDebitAC,@AmountCreditAC,  " +
                       "@CurrencyAliasFC,@AmountDebitFC,@AmountCreditFC,@VATCode,@ProcessNumber,@ProcessLineCode, @res_id, @oorsprong,@docdate," +
                       "@vervdatfak,@faktuurnr,@syscreator,@sysmodifier,@EntryGuid,@companycostcentercode,@batchno)  ";
        SqlTransaction transaction = null;

        try
        {
            using (SqlConnection connection = new DBConnection().getEDIConnection(edi))
            {
                int t = 0;
                int ed = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.Add("@paymentmethod", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@freefield4", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@freefield5", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@CompanyCode", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@TransactionType", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@TransactionDate", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@FinYear", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@FinPeriod", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@CompanyAccountCode", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@EntryNumber", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@CurrencyAliasAC", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@AmountDebitAC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@AmountCreditAC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@CurrencyAliasFC", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@AmountDebitFC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@AmountCreditFC", System.Data.SqlDbType.Float);
                    command.Parameters.Add("@VATCode", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@ProcessNumber", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@ProcessLineCode", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@res_id", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@oorsprong", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@docdate", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@vervdatfak", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@faktuurnr", System.Data.SqlDbType.Char);
                    command.Parameters.Add("@syscreator", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@sysmodifier", System.Data.SqlDbType.Int);
                    command.Parameters.Add("@EntryGuid", System.Data.SqlDbType.UniqueIdentifier);
                    command.Parameters.Add("@companycostcentercode", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@batchno", System.Data.SqlDbType.VarChar);
                    command.CommandTimeout = 0;
                    connection.Open();

                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    int i = 0;
                    try
                    {
                        for (i = 0; i < data.Count; i++)
                        {
                            var temp = data[i];
                            //int f4 = 0;
                            //int f5 = 0;
                            //if (temp.freefield4.Trim().Equals("")) 
                            //{
                            //    Console.WriteLine("sdfsaaaaaaaa");
                            //}
                            //string sdf = "";
                            //sdf =temp.freefield4.Trim().Equals("") ? "asdf" : "2323232";
                            command.Parameters["@paymentmethod"].Value = temp.paymentmethod;
                            command.Parameters["@freefield4"].Value =(temp.freefield4.Trim()=="")? 0: Convert.ToDouble( temp.freefield4);
                            command.Parameters["@freefield5"].Value = temp.freefield5.Trim().Equals("") ? 0 : Convert.ToDouble(temp.freefield5);
                            command.Parameters["@CompanyCode"].Value = temp.CompanyCode;
                            command.Parameters["@TransactionType"].Value = Convert.ToInt32(temp.TransactionType);
                            command.Parameters["@TransactionDate"].Value = temp.TransactionDate;
                            command.Parameters["@FinYear"].Value = Convert.ToInt32(temp.FinYear);
                            command.Parameters["@FinPeriod"].Value = Convert.ToInt32(temp.FinPeriod);
                            command.Parameters["@Description"].Value = temp.Description;
                            command.Parameters["@CompanyAccountCode"].Value = temp.CompanyAccountCode;
                            command.Parameters["@EntryNumber"].Value = "";
                            command.Parameters["@CurrencyAliasAC"].Value = temp.CurrencyAliasAC;
                            command.Parameters["@AmountDebitAC"].Value = Convert.ToDouble(temp.AmountDebitAC);
                            command.Parameters["@AmountCreditAC"].Value = Convert.ToDouble(temp.AmountCreditAC);
                            command.Parameters["@CurrencyAliasFC"].Value = temp.CurrencyAliasFC;
                            command.Parameters["@AmountDebitFC"].Value = Convert.ToDouble(temp.AmountDebitFC);
                            command.Parameters["@AmountCreditFC"].Value = Convert.ToDouble(temp.AmountCreditFC);
                            command.Parameters["@VATCode"].Value = temp.VATCode;
                            command.Parameters["@ProcessNumber"].Value = Convert.ToInt32(temp.ProcessNumber);
                            command.Parameters["@ProcessLineCode"].Value = temp.ProcessLineCode;
                            command.Parameters["@res_id"].Value = Convert.ToInt32(temp.res_id);
                            command.Parameters["@oorsprong"].Value = temp.oorsprong;
                            command.Parameters["@docdate"].Value = temp.docdate;
                            command.Parameters["@vervdatfak"].Value = temp.vervdatfak;
                            command.Parameters["@faktuurnr"].Value = "";
                            command.Parameters["@syscreator"].Value = Convert.ToInt32(temp.syscreator);
                            command.Parameters["@sysmodifier"].Value = Convert.ToInt32(temp.sysmodifier);
                            command.Parameters["@EntryGuid"].Value = new Guid(temp.EntryGuid);
                            command.Parameters["@companycostcentercode"].Value = temp.companycostcentercode;
                            command.Parameters["@batchno"].Value = temp.batchNo;
                            command.ExecuteNonQuery();

                        }
                        t = i;
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

                using (SqlCommand ediCommand = new SqlCommand(edisql, connection))
                {
                    ediCommand.Parameters.Add("@bcode", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@" + desc, System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@month_eli", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@year_eli", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@date_time", System.Data.SqlDbType.VarChar);
                    ediCommand.Parameters.Add("@sys_creator", System.Data.SqlDbType.VarChar);
                    //     command.Parameters.Add("@rowguid", System.Data.SqlDbType.UniqueIdentifier);
                    ediCommand.CommandTimeout = 0;
                    ediCommand.Transaction = transaction;
                    try
                    {
                        int i = 0;
                        for (i = 0; i < ediTableData.Count; i++)
                        {
                            var temp = ediTableData[i];

                            ediCommand.Parameters["@bcode"].Value = temp.bcode;
                            ediCommand.Parameters["@" + desc].Value = temp.desc;
                            ediCommand.Parameters["@month_eli"].Value = temp.month_eli;
                            ediCommand.Parameters["@year_eli"].Value = temp.year_eli;
                            ediCommand.Parameters["@date_time"].Value = temp.date_time;
                            ediCommand.Parameters["@sys_creator"].Value = temp.sys_creator;
                            //   command.Parameters["@rowguid"].Value = temp.rowguid;
                            ediCommand.ExecuteNonQuery();

                        }
                        ed = i;
                        if ((ed == ediTableData.Count) && (t == data.Count))
                        {
                            transaction.Commit();
                            response.responseCode = ResponseCode.OK;
                            response.responseMessage = t.ToString() + " transactions inserted";
                            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                            Service.log.Info(edi + " " + t.ToString() + " Records Inserted");
                        }
                        else
                        {
                            transaction.Rollback();
                            response.responseCode = ResponseCode.Error;
                            response.responseMessage = "Something went wrong ";
                            Service.log.Info(edi + " Something went wrong transaction rollback");
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
}