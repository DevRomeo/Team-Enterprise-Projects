using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using log4net;
namespace Comparative_Report.Query
{
    class Q356Query
    {
        
        public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Q356Query() 
        {
          
            
            log4net.Config.XmlConfigurator.Configure();
        }
        public Response startFetch(int mo, int cur_year, int prev_yr, String currency,String DBConfig) 
        {
          
            Response response = new Response();
            Response deleteResponse = deleteSummaryData(mo, cur_year, prev_yr, currency,DBConfig);
            if (deleteResponse.responseCode != ResponseCode.OK)
            {
                response = deleteResponse;
            }
            else 
            {
                int maxdate= cur_year==DateTime.Now.Year  &&  mo==DateTime.Now.Month?
                             DateTime.Now.Day:
                             DateTime.DaysInMonth(cur_year, mo);
                if (cur_year != prev_yr)
                {
                    for (int i = 1; i <= maxdate; i++)
                    {
                        String sotable = mo.ToString("00") + i.ToString("00");
                        String prev = prev_yr + "-" + mo.ToString("00") + "-" + i.ToString("00");
                        String cur = cur_year + "-" + mo.ToString("00") + "-" + i.ToString("00");


                        response = payoutToSummaryStored(cur, sotable, currency, DBConfig);
                        if (response.responseCode != ResponseCode.OK)
                            break;
                        response = payoutToSummaryStored(prev, sotable, currency, DBConfig);
                        if (response.responseCode != ResponseCode.OK)
                            break;
                    }
                }
                else 
                {
                    for (int i = 1; i <= maxdate; i++)
                    {
                        String sotable = mo.ToString("00") + i.ToString("00");
                        String prev = prev_yr + "-" + mo.ToString("00") + "-" + i.ToString("00");
                        String cur = cur_year + "-" + mo.ToString("00") + "-" + i.ToString("00");


                        response = payoutToSummaryStored(cur, sotable, currency, DBConfig);
                        if (response.responseCode != ResponseCode.OK)
                            break;
                     
                    }
                }
           
            }
            return response; 
        }

        private Response payoutToSummaryStored(String transdate,String sotable,String currency,String DBConfig)
        {
            Response response = new Response();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            MySqlConnection connection = new Util.Connection().getConnection(DBConfig);
            
            string sql = "INSERT INTO kpUploadArchive.partners_summary (yr, mo, txncount,principal,charge," +
                    " partnercode, partnername,int_type ,currency) VALUES (@yr, @mo, @txncount,@principal,@charge," +
                    " @partnercode, @partnername,@int_type ,@currency)";
            String mo = transdate.Split('-')[1];
            String yr = transdate.Split('-')[0];
            try 
            {
                string storedsql = DBConfig.Equals("BillsPay") ? "kpbillspayment.kpPartnersSummaryReport" : "kppartners.kpPartnersSummaryReport";
                var data = connection.Query(sql: storedsql, param: new
                {
                    transdate = transdate,
                    sotbl = sotable,
                    curr = currency
                }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0);

                var insertdata = data.Select(xx => new
                {
                    yr = yr,
                    mo = mo,
                    txncount =xx.SOCountDaily, //xx.SOtotalCount,
                    principal = xx.SOtotalAmountDaily,//xx.SOtotalAmount,
                    charge = xx.SOTotalCharge,
                    partnercode = xx.accountcode,
                    partnername = xx.accountname,
                    int_type = xx.integrationtype,
                    currency = xx.currency

                }).ToList();
                int count = connection.Execute(sql,insertdata);
                response.responseCode = ResponseCode.OK;
                response.responseData = count;
            }
            catch (Exception e)
            {
                log.Error(DBConfig + ": " + e.Message);
                response.responseCode = ResponseCode.Error;
                response.responseMessage = e.Message;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return response;
        }
       
        private Response deleteSummaryData(int mo, int cur_yr, int prev_yr, String currency, String DBConfig) 
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Response response = new Response();
            String sql = "DELETE FROM kpUploadArchive.partners_summary WHERE currency=@currency AND mo=@mo AND yr IN (@cur_yr,@prev_yr)";
            try 
            {
                MySqlConnection connection = new Util.Connection().getConnection(DBConfig);
             //   connection.Open();
               int count= connection.Execute(sql, new { currency = currency, mo = mo, cur_yr = cur_yr, prev_yr = prev_yr });
                response.responseCode = ResponseCode.OK;
                response.responseData = count;
            }
            catch (Exception e)
            {
                log.Error(DBConfig+": "+e.Message);
                response.responseCode = ResponseCode.Error;
                response.responseMessage = e.Message;
            }
            return response;
        }

    
    }
}
