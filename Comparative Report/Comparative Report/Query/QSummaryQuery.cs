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
    class QSummaryQuery
    {
        public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public QSummaryQuery() 
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public Response getSummary1data(String currency, int mo, int yr,int pvyr) 
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info("Fetching Data. Currency :"+currency+"Month :"+mo+" CurrentYear:"+yr+" Previous Year:"+pvyr);
            Response response = new Response();
            response.responseCode = ResponseCode.OK;

            log.Info("Fetching Partners Data");
           // setStatusLabel("Fetching data...");
            Response partnersResponse = getReport1SummaryData(currency,mo,yr,pvyr,"Partners");

            log.Info("Fetching FileUploadData  Data");
            Response fileUploadResponse =getReport1SummaryData(currency,mo,yr,pvyr,"FileUpload");

            log.Info("Fetching BillSpay  Data");
            Response billsPayResponse=getReport1SummaryData(currency,mo,yr,pvyr,"BillsPay");
            if (partnersResponse.responseCode != ResponseCode.OK)
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage += partnersResponse.responseMessage;
                partnersResponse.responseData = new List<DataClass.DSummary_data>();
             
            }
             if(fileUploadResponse.responseCode!=ResponseCode.OK) 
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage += fileUploadResponse.responseMessage;
                fileUploadResponse.responseData = new List<DataClass.DSummary_data>();
             
            }
             if (billsPayResponse.responseCode != ResponseCode.OK)
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage += billsPayResponse.responseMessage;
                billsPayResponse.responseData = new List<DataClass.DSummary_data>();
             
            }
            else 
            {
              
                List<DataClass.DSummary_data> partnersdata = (List<DataClass.DSummary_data>)partnersResponse.responseData;
                List<DataClass.DSummary_data> fileuploaddata = (List<DataClass.DSummary_data>)fileUploadResponse.responseData;
                List<DataClass.DSummary_data> billspaydata = (List<DataClass.DSummary_data>)billsPayResponse.responseData;
                List<DataClass.DSummary_data> temp = new List<DataClass.DSummary_data>();
                if (partnersdata !=null|| partnersdata.Count > 0)
                {
                    temp = temp.Concat(partnersdata).ToList();
                }
                if (fileuploaddata !=null|| fileuploaddata.Count > 0)
                {
                    temp = temp.Concat(fileuploaddata).ToList();
                }
                if (billspaydata !=null||billspaydata.Count > 0)
                {
                    temp = temp.Concat(billspaydata).ToList();
                }


                response.responseData = temp;


            }           
                return response;
        }
        public Response getReport1SummaryData(String currency,int mo,int cyr,int pvyr,String DBConfig) 
        {
            string intype = "";
            string accounttype = "  a.accounttype IN('MLCIP','MLPSP') ";
            if (DBConfig.Equals("BillsPay"))
            {
                accounttype = " a.accounttype='MLBPP'";
            
            }
            else if (DBConfig.Equals("FileUpload"))
            {
                intype = " HAVING intType  in(2,4) ";
            }
            else if (DBConfig.Equals("Partners"))
            {
                intype = " HAVING intType  in(1,3) ";
            }
            
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Response response = new Response();
            String sql = new Util.SqlString().getSql("Query")
                         .Replace("xxaccounttypexx", accounttype)
                         .Replace("xxintypexx", intype);
            try
            {
                MySqlConnection connection = new Util.Connection().getConnection(DBConfig);
                var data = connection.Query<DataClass.DSummary_data>(sql,new{mont=mo,cyear=cyr,pvyear=pvyr,curr=currency}).ToList();
                response.responseCode = ResponseCode.OK;
                response.responseData = data;
                response.responseMessage = data.Count.ToString();
                log.Info(DBConfig+":" +data.Count.ToString() + " records found");
            }
            catch (Exception e) 
            {
                log.Error(DBConfig + " " + e.Message);
                response.responseCode = ResponseCode.Error;
                response.responseMessage = e.Message;   
            }
            return response; 
        }

    }
}
