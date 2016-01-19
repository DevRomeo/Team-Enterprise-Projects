using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAT.Models;
using MySql.Data.MySqlClient;
using Dapper;
using SAT.Enum;
using log4net;
using System.IO;
namespace SAT.Util
{
    public static  class MoneyGramUtil
    {


        public static CustomResponse getAllUploadedFile()
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            CustomResponse response = new CustomResponse();
            List<UploadedFilesModel> files = new List<UploadedFilesModel>();
            response.responseData = files;
            try
            {
                using (MySqlConnection connection = SATConnection.getMoneygramConnection())
                {
                    
                    files = connection.Query<UploadedFilesModel>("SELECT * FROM uploadedfiles ORDER BY SYSCREATED Desc").ToList();
                }

                response.responseCode = ResponseCode.OK;
                response.responseData = files;


            }
            catch (Exception e)
            {
                response = new CustomResponse { responseCode = ResponseCode.Error, responseMessage = e.Message };
                MySession.log.Error(e.Message);
            }

            return response;
        }
        public static CustomResponse getAllUploadedFile(DateTime date ) 
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            CustomResponse response = new CustomResponse();
            List<UploadedFilesModel> files = new List<UploadedFilesModel>();
            response.responseData = files;
            try
            {
                using (MySqlConnection connection = SATConnection.getMoneygramConnection())
                {
                    files = connection.Query<UploadedFilesModel>("SELECT * FROM uploadedfiles WHERE DATE(SYSCREATED)=DATE(@date) ORDER BY SYSCREATED Desc",
                        new { date = date }).ToList();
                }
            
                response.responseCode = ResponseCode.OK;
                response.responseData = files;
                

            }
            catch (Exception e)
            {
                response =new CustomResponse { responseCode = ResponseCode.Error, responseMessage = e.Message };
                MySession.log.Error(e.Message);
            }

            return response;
        }
        public static CustomResponse isUploaded(String fname)
        {
           MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            CustomResponse response = new CustomResponse();
            String sql = "SELECT * FROM uploadedfiles WHERE LOWER(TRIM(fname)) = LOWER(TRIM(@fname)) LIMIT 1";
            try
            {
                using (MySqlConnection connection = SATConnection.getMoneygramConnection())
                {
                    var data = connection.Query(sql, new { fname = fname });
                    if (data.Count() <= 0)
                    {
                        response.responseCode = ResponseCode.OK;
                    }
                    else
                    {
                        response.responseCode = ResponseCode.FileAlreadyUploaded;
                        response.responseMessage = "Unable to load file. File already uploaded";

                    }

                }
            }
            catch (Exception e)
            {
                MySession.log.Error(e.Message);
                response.responseCode = ResponseCode.Error;
                response.responseMessage = e.Message;
            }

            return response;
        }
        public static CustomResponse insertToMoneygram(List<MoneyGramModel> data, String filename)
        {

            CustomResponse response = new CustomResponse();
            MySqlTransaction transaction;
            MySqlConnection connection = SATConnection.getMoneygramConnection();
            String tablename = "moneygram" + data[0].trandate.Split('/')[2] + data[0].trandate.Split('/')[0];

            String sql = "INSERT INTO " + tablename + "(fname,trandate,refnum,trantype," +
                         "sendcntry,rcvcntry,legacyid,agentname,currency,fxratephp,margin,amount," +
                         "feeamount,shareamount,commandfxamount,rate,convertedcommfxamt)" +
                         " VALUES(@fname,@trandate,@refnum,@trantype," +
                         "@sendcntry,@rcvcntry,@legacyid,@agentname,@currency,@fxratephp,@margin,@amount," +
                         "@feeamount,@shareamount,@commandfxamount,@rate,@convertedcommfxamt)";
            String createTable = "CREATE TABLE if not exists " + tablename + " ( " +
                                "id bigint(11) NOT NULL auto_increment, " +
                                "fname varchar(200) default NULL, " +
                                "trandate date default NULL, " +
                                "refnum varchar(200) default NULL," +
                                "trantype varchar(5) default NULL, " +
                                "sendcntry varchar(50) default NULL, " +
                                "rcvcntry varchar(50) default NULL, " +
                                "legacyid varchar(200) default NULL, " +
                                "agentname varchar(200) default NULL, " +
                                "currency varchar(200) default NULL, " +
                                "fxratephp decimal(16,2) default NULL, " +
                                "margin decimal(16,2) default NULL, " +
                                "amount decimal(16,2) default NULL, " +
                                "feeamount decimal(16,2) default NULL, " +
                                "shareamount decimal(16,2) default NULL, " +
                                "commandfxamount decimal(16,2) default NULL, " +
                                "rate double default NULL, " +
                                "convertedcommfxamt decimal(16,2) default NULL,  " +
                                "PRIMARY KEY  (`id`), " +
                                "KEY `trandate` (`trandate`), " +
                                "KEY `currency` (`currency`), " +
                                "KEY `legacyid` (`legacyid`) " +
                                ") ENGINE=InnoDB DEFAULT CHARSET=latin1";
            connection.Open();
            connection.Execute(createTable);


            String uploadfileString = "INSERT INTO uploadedfiles(fname,ftotal,syscreator) values(@fname,@ftotal,@syscreator)";
          //  MySqlConnection filesConnection =SATConnection.getMoneygramConnection();
          //  filesConnection.Execute(uploadfileString, new { fname = filename, ftotal = data.Count, syscreator = ID });

            transaction = connection.BeginTransaction();
            try
            {
               // System.Threading.Thread.Sleep(10600);
                connection.Execute(uploadfileString, new { fname = filename, ftotal = data.Count, syscreator = MySession.userID });
       

                int count = connection.Execute(sql, data, transaction, 0);
                if (count == data.Count)
                {
                   // connection.Execute("UPDATE uploadedfiles set status=1 WHERE fname=@fname", new { fname = filename });
                    transaction.Commit();
                    response.responseCode = ResponseCode.OK;
                    response.responseMessage = count.ToString() + " transactions inserted";
                }
                else
                {
                   // filesConnection.Execute("UPDATE uploadedfiles set status=2 ,error=" + " insert count did not match with data count " + " WHERE fname=@fname", new { fname = filename });
                    transaction.Rollback();
                    response.responseCode = ResponseCode.Error;
                    response.responseMessage = "Something went wrong";

                }


            }
            catch (Exception e)
            {
               // filesConnection.Execute("UPDATE uploadedfiles set status=2 ,error=@error WHERE fname=@fname", new { fname = filename, error = e.Message });
                MySession.log.Error(e.Message);
                response.responseCode = ResponseCode.Error;
                response.responseMessage = e.Message;

                if (connection.State == System.Data.ConnectionState.Open)
                    transaction.Rollback();
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return response;
        }
        public static CustomResponse readCsv(HttpPostedFileBase file)
        {

            List<MoneyGramModel> monegramData = new List<MoneyGramModel>();
            MoneyGramModel tempRow;
            int i = 0;
            try
            {
                StreamReader reader = new StreamReader(file.InputStream);

                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    i++;
                    tempRow = new MoneyGramModel();
                    string line = reader.ReadLine();
                    String[] arr = line.Split(',');
                    //String[] dateTemp = arr[0].Split(' ');
                    String[] dateTempDate = arr[0].Split('/');
                    //  String[] dateTempTime = dateTemp[1].Split(':');
                    DateTime date = new DateTime(int.Parse(dateTempDate[2]),
                                                int.Parse(dateTempDate[0]),
                                                int.Parse(dateTempDate[1]));
                    // int.Parse(dateTempTime[0]),
                    // int.Parse(dateTempTime[1]),
                    // int.Parse(dateTempTime[2]));



                    String dt = date.ToString("MM/dd/yyyy");
                    String sdate = date.ToString("yyyyMM");
                    String sd = date.ToString("yyyy-MM-dd");
                    double[] rate;// (new DbClass.MoneyGram()).getRate(sd);
                    double forrate = 0;
                    CustomResponse getRateResponse = ServiceConnector.getMoneyGramRate(arr[5], dt);
                    if (getRateResponse.responseCode == ResponseCode.OK)
                    {
                        rate = (double[])getRateResponse.responseData;
                    }
                    else
                    {
                        return getRateResponse;
                    }
                    //tempRow.trandate = id;
                    tempRow.trandate = dt;
                    tempRow.fname = file.FileName;
                    tempRow.refnum = arr[1];
                    tempRow.trantype = arr[2];
                    tempRow.sendcntry = arr[3];
                    tempRow.rcvcntry = arr[4];
                    tempRow.legacyid = arr[5];

                    tempRow.agentname = arr[6];
                    tempRow.currency = arr[7];
                    tempRow.margin = Math.Abs(Decimal.Parse(arr[9]));
                    tempRow.amount = Math.Abs(Decimal.Parse(arr[10]));
                    tempRow.feeamount = Math.Abs(Decimal.Parse(arr[11]));
                    tempRow.shareamount = Math.Abs(Decimal.Parse(arr[12]));
                    tempRow.commandfxamount = Math.Abs(Decimal.Parse(arr[13]));
                    if (arr[2].Equals("rec"))
                    {
                        forrate = rate[0];
                        tempRow.rate = forrate;
                    }
                    else if (arr[2].Equals("sen"))
                    {
                        forrate = rate[1];
                        tempRow.rate = forrate;
                    }
                    if (arr[7].Equals("PHP"))
                    {
                        tempRow.convertedcommfxamt = Math.Abs(Decimal.Parse(arr[13]));
                    }
                    else if (arr[7].Equals("USD"))
                    {
                        tempRow.convertedcommfxamt = (Decimal)(Math.Abs(Double.Parse(arr[13])) * forrate);
                    }

                    monegramData.Add(tempRow);

                }

            }
            catch (Exception e)
            {
                MySession.log.Error(e.Message, e);
                return new CustomResponse(ResponseCode.Error, e.Message, null);
            }
            return new CustomResponse(ResponseCode.OK, i + " records found", monegramData);
        }
    }
}