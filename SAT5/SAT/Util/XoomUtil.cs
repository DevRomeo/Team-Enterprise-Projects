using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAT.Enum;
using SAT.Models;
using MySql.Data.MySqlClient;
using Dapper;
using log4net;
using System.IO;
namespace SAT.Util
{
    public static class XoomUtil
    {
        public static CustomResponse getAllUploadedFile()
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            CustomResponse response = new CustomResponse();
            List<UploadedFilesModel> files = new List<UploadedFilesModel>();
            response.responseData = files;
            try
            {
                using (MySqlConnection connection = SATConnection.getXoomConnection())
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
        public static CustomResponse getAllUploadedFile(DateTime date)
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            CustomResponse response = new CustomResponse();
            List<UploadedFilesModel> files = new List<UploadedFilesModel>();
            response.responseData = files;
            try
            {
                using (MySqlConnection connection = SATConnection.getXoomConnection())
                {
                    files = connection.Query<UploadedFilesModel>("SELECT * FROM uploadedfiles WHERE DATE(SYSCREATED)=DATE(@date) ORDER BY SYSCREATED Desc",
                        new { date = date }).ToList();
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
        public static CustomResponse isUploaded(String fname)
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            CustomResponse response = new CustomResponse();
            String sql = "SELECT * FROM uploadedfiles WHERE LOWER(TRIM(fname)) = LOWER(TRIM(@fname)) LIMIT 1";
            try
            {
                using (MySqlConnection connection = SATConnection.getXoomConnection())
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
        public static CustomResponse readCsv(HttpPostedFileBase file)
        {
            
            CustomResponse response = new CustomResponse();
            String fname = file.FileName;

            List<XoomModel> xoomCsvData = new List<XoomModel>();
            XoomModel tempRow;
            int i = 0;

            try
            {
                StreamReader reader = new StreamReader(file.InputStream);

                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    tempRow = new XoomModel();


                    string line = reader.ReadLine();
                    String[] arr = line.Split('|');
                    String[] dateTemp = arr[0].Split(' ');
                    String[] dateTempDate = dateTemp[0].Split('/');
                    String[] dateTempTime = dateTemp[1].Split(':');
                    DateTime date = new DateTime(int.Parse(dateTempDate[2]),
                                                int.Parse(dateTempDate[0]),
                                                int.Parse(dateTempDate[1]),
                                                int.Parse(dateTempTime[0]),
                                                int.Parse(dateTempTime[1]),
                                                int.Parse(dateTempTime[2]));
                    String dt = date.ToString("MM/dd/yyyy HH:mm:ss");
                    String stdate = date.ToString("yyyyMM");
                    String sd = date.ToString("yyyy-MM-dd");
                    response = ServiceConnector.getPayoutRate(sd);
                    double nwrate = 0.0;
                    if (response.responseCode == ResponseCode.OK)
                        nwrate = (double)response.responseData;
                    else
                        return response;
                    if (nwrate == 0)
                    {
                        MySession.log.Error(sd + " rate not Found");
                        return new CustomResponse(ResponseCode.Error, sd + " rate not found", null);

                    }

                    tempRow.fname = fname;
                    tempRow.txndate = sd;
                    tempRow.Event_Date = date.ToString("MM-dd-yyyy HH:mm:ss"); ;
                    tempRow.Created = arr[1];
                    tempRow.Event_Type = arr[2];
                    tempRow.Disbursement_Type = arr[3];
                    tempRow.Partner = arr[4];
                    tempRow.blank1 = arr[5];
                    tempRow.blank2 = arr[6];
                    tempRow.blank3 = arr[7];
                    if (arr[8].Contains("-"))
                    {
                        tempRow.Branch_Number = arr[8];
                    }
                    else
                    {
                        tempRow.Branch_Number = "mlhu-121";
                    }
                    tempRow.Authorizer = arr[9];
                    tempRow.Country = arr[10];
                    tempRow.Recipient_City = arr[11];
                    tempRow.Xoom_Invoice = arr[12];
                    tempRow.Payout_Currency = arr[13];
                    if (arr[2].Equals("Disbursed"))
                    {
                        tempRow.Payout_Amount = Math.Abs(Decimal.Parse(arr[14]));
                    }
                    else
                    {
                        tempRow.Payout_Amount = Decimal.Parse(arr[14]) * -1;
                    }
                    tempRow.Payout_Currency1 = arr[15];
                    tempRow.Payout_Amount1 = Decimal.Parse(arr[16]);
                    tempRow.rate = nwrate;

                    {
                        double abscnvrtd = 0.00;
                        if (arr[15].Equals("USD"))
                        {
                            abscnvrtd = Math.Abs(Double.Parse(arr[14]));
                        }
                        else
                        {
                            abscnvrtd = Math.Abs(Double.Parse(arr[14]) / nwrate);
                        }
                        tempRow.convertedusdamt = abscnvrtd;
                        response = ServiceConnector.getXoomCharge(abscnvrtd, sd);
                        if (response.responseCode == ResponseCode.OK)
                            tempRow.charge = (double)response.responseData;
                        else
                            return response;

                        if (tempRow.charge == 0)
                        {
                            MySession.log.Error(sd + " charge not Found");
                            return new CustomResponse(ResponseCode.Error, sd + " Charge not found", null);
                        }
                    }

                    xoomCsvData.Add(tempRow);
                    i++;
                }


            }
            catch (Exception e)
            {

                MySession.log.Error(e.Message, e);
                return new CustomResponse(ResponseCode.Error, e.Message, null);
            }
            MySession.log.Info(i.ToString() + "  Records found");
            return new CustomResponse(ResponseCode.OK, i + "  records ", xoomCsvData);

            // return response;

        }
        public static CustomResponse insertToXoom(List<XoomModel> data, String filename)
        {
            CustomResponse response = new CustomResponse();
            String tablename = "xoom" + data[0].txndate.Split('-')[0] + data[0].txndate.Split('-')[1];
            string createifNotExist = "CREATE TABLE if not exists " + tablename + " ( " +
                                      "id bigint(11) NOT NULL auto_increment, " +
                                      "fname varchar(50) default NULL, " +
                                      "txndate date default NULL, " +
                                      "Event_Date datetime default NULL, " +
                                      "Created date default NULL, " +
                                      "Event_Type varchar(50) default NULL, " +
                                      "Disbursement_Type varchar(50) default NULL, " +
                                      "Partner varchar(50) default NULL, " +
                                      "blank1 varchar(50) default NULL,   " +
                                      "blank2 varchar(50) default NULL,   " +
                                      "blank3 varchar(50) default NULL,   " +
                                      "Branch_Number varchar(50) default NULL,   " +
                                      "Authorizer varchar(100) default NULL,    " +
                                      "Country varchar(50) default NULL,  " +
                                      "Recipient_City varchar(50) default NULL,  " +
                                      "Xoom_Invoice varchar(50) default NULL,    " +
                                      "Payout_Currency varchar(50) default NULL, " +
                                      "Payout_Amount decimal(16,2) default NULL,  " +
                                      "Payout_Currency1 varchar(50) default NULL, " +
                                      "Payout_Amount1 decimal(16,2) default NULL, " +
                                      "rate double default NULL,  " +
                                      "convertedusdamt decimal(16,2) default NULL, " +
                                      "charge decimal(16,2) default NULL, " +
                                      "PRIMARY KEY  (id), " +
                                      "KEY `txndate` (`txndate`), " +
                                      "KEY `Payout_Currency` (`Payout_Currency`), " +
                                      "KEY `Branch_Number` (`Branch_Number`) " +
                                      ")ENGINE=InnoDB DEFAULT CHARSET=latin1";
            String sql = "INSERT INTO " + tablename + "(fname,txndate,Event_Date," +
                    "Created,Event_Type,Disbursement_Type,Partner,blank1,blank2,blank3," +
                    "Branch_Number,Authorizer,Country,Recipient_City,Xoom_Invoice," +
                    "Payout_Currency,Payout_Amount,Payout_Currency1,Payout_Amount1," +
                    "rate,convertedusdamt,charge) " +
                    "VALUES(@fname,@txndate,@Event_Date," +
                    "@Created,@Event_Type,@Disbursement_Type,@Partner,@blank1,@blank2,@blank3," +
                    "@Branch_Number,@Authorizer,@Country,@Recipient_City,@Xoom_Invoice," +
                    "@Payout_Currency,@Payout_Amount,@Payout_Currency1,@Payout_Amount1," +
                    "@rate,@convertedusdamt,@charge);";
            String uploadfileString = "INSERT INTO uploadedfiles(fname,ftotal,syscreator) values(@fname,@ftotal,@syscreator)";


            MySqlTransaction transaction;
            MySqlConnection connection = SATConnection.getXoomConnection();
            connection.Execute(createifNotExist);
            connection.Open();
            transaction = connection.BeginTransaction();



            try
            {
                connection.Execute(uploadfileString, new { fname = filename, ftotal = data.Count, syscreator = MySession.userID });

                var count = connection.Execute(sql, data, commandTimeout: 0, transaction: transaction);
                if (count == data.Count)
                {
                    transaction.Commit();
                    response.responseCode = ResponseCode.OK;
                    response.responseMessage = count.ToString() + " transactions inserted";
                }
                else
                {
                    transaction.Rollback();
                    response.responseCode = ResponseCode.Error;
                    response.responseMessage = "Something went wrong";

                }
            }
            catch (Exception e)
            {
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

    }
}