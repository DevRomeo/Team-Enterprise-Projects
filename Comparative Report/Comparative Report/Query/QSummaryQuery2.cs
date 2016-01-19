using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using log4net;

namespace Comparative_Report.Query
{
    class QSummaryQuery2
    {
        public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public QSummaryQuery2() 
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public Response getSummary2data(String currency, int mo, int yr, int pvyr)
        {
            log.Info("Fetching Data. Currency :" + currency + "Month :" + mo + " CurrentYear:" + yr + " Previous Year:" + pvyr);
            Response response = new Response();
            response.responseCode = ResponseCode.OK;
            log.Info("Fetching Partners Data");   
            Response partnersResponse = getReport2SummaryData(currency, mo, yr, pvyr, "Partners");

            log.Info("Fetching FileUpload Data");   
            Response fileUploadResponse = getReport2SummaryData(currency, mo, yr, pvyr, "FileUpload");

            log.Info("Fetching Billspay Data");   
            Response billsPayResponse = getReport2SummaryData(currency, mo, yr, pvyr, "BillsPay");
            if (partnersResponse.responseCode != ResponseCode.OK)
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage += partnersResponse.responseMessage;
                partnersResponse.responseData = new List<DataClass.DSummary_data2>();
            }
            else if (fileUploadResponse.responseCode != ResponseCode.OK)
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage += fileUploadResponse.responseMessage;
                fileUploadResponse.responseData = new List<DataClass.DSummary_data2>();
            }
            else if (billsPayResponse.responseCode != ResponseCode.OK)
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage += billsPayResponse.responseMessage;
                billsPayResponse.responseData = new List<DataClass.DSummary_data2>();
            }
            else
            {
             
                List<DataClass.DSummary_data2> partnersdata = (List<DataClass.DSummary_data2>)partnersResponse.responseData;
                List<DataClass.DSummary_data2> fileuploaddata = (List<DataClass.DSummary_data2>)fileUploadResponse.responseData;
                List<DataClass.DSummary_data2> billspaydata = (List<DataClass.DSummary_data2>)billsPayResponse.responseData;
                List<DataClass.DSummary_data2> temp = new List<DataClass.DSummary_data2>();
                if (partnersdata.Count > 0)
                {
                    temp = temp.Concat(partnersdata).ToList();
                }
                if (fileuploaddata.Count > 0)
                {
                    temp = temp.Concat(fileuploaddata).ToList();
                }
            
                if (billspaydata.Count > 0)
                {
             //       temp = temp.Concat(billspaydata).ToList();
                    var t = billspaydata.Select(xx => new DataClass.DSummary_data2
                    {
                        chargecurrent = xx.chargecurrent,
                        chargeprev = xx.chargeprev,
                        intType = "5",
                        partnercode = xx.partnercode,
                        partnername = xx.partnername,
                        per = xx.per,
                        princurrent = xx.princurrent,
                        prinprev = xx.prinprev,
                        txncurrent = xx.txncurrent,
                        txnprev = xx.txnprev,
                        var = xx.var

                    }).ToList();

                    temp = temp.Concat(t).ToList();

                }
                //var tesmp = billspaydata.Where(xx => xx.txncurrent > 0 || xx.txnprev > 0);
              //  var asdf = temp.Where(xx => xx.intType == "5");
               // var aadfasdf = temp.Where(xx => xx.intType == "5" && xx.partnercode == "MLBPP130023");
                response.responseData = temp;
            }

            
            return response;
        }
        public Response getReport2SummaryData(String currency, int mo, int cur_yr, int prev_yr, String DBConfig) 
        {
            dynamic temp = new System.Dynamic.ExpandoObject();
           
            string intype = "";
            string accounttype = "  a.accounttype IN('MLCIP','MLPSP') ";
            if (DBConfig.Equals("BillsPay"))
            {
                accounttype = " a.accounttype='MLBPP'";

            }
            else if (DBConfig.Equals("FileUpload"))
            {
                intype = " HAVING intType  in(2,4)  ";
            }
            else if (DBConfig.Equals("Partners"))
            {
                intype = " HAVING intType  in(1,3)  ";
            }
            
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Response response = new Response();
            String sql = new Util.SqlString().getSql("Query2")
                        .Replace("xxaccounttypexx", accounttype)
                        .Replace("xxintypexx ",intype);
            MySqlConnection connection = new Util.Connection().getConnection(DBConfig);
            try 
            {
                var data= connection.Query<DataClass.DSummary_data2>(sql, 
                       new 
                       {
                            currency=currency,
                            mo=mo,
                            cur_yr=cur_yr,
                            prev_yr=prev_yr 
                       }).ToList();
                response.responseCode = ResponseCode.OK;
                response.responseData = data;
                response.responseMessage = data.Count.ToString();

                log.Info(DBConfig + ":" + data.Count.ToString() + " records found");
            }
            catch (Exception e) 
            {
                log.Error(DBConfig + ": " + e.Message);
                response.responseCode = ResponseCode.Error;
                response.responseMessage =DBConfig+": "+ e.Message;
            }

            return response;
        }
    }
}
