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
using Excel;
using System.Globalization;

namespace SAT.Util
{
    public static class XpressMoneyUtil
    {

        public static CustomResponse getSheets(HttpPostedFileBase file)
        {
            List<String> data = new List<string>();
            CustomResponse response = new CustomResponse();
            IExcelDataReader reader = null;
            try
            {
                if (Path.GetExtension(file.FileName).Equals(".xls"))
                    reader = ExcelReaderFactory.CreateBinaryReader(file.InputStream);
                else
                    reader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream);


                do
                {
                    if (reader.Read())
                        data.Add(reader.Name);
                } while (reader.NextResult());
                if (data.Count > 0)
                {
                    response.responseCode = ResponseCode.OK;
                    response.responseData = data;
                }
                else
                {
                    response.responseCode = ResponseCode.NotFound;
                    response.responseMessage = "No record found";
                }
            }
            catch (Exception e)
            {
                response.responseCode = ResponseCode.Error;
                response.responseData = e.Message;

            }

            return  response;

        }
        public static CustomResponse getAllUploadedFile()
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            CustomResponse response = new CustomResponse();
            List<UploadedFilesModel> files = new List<UploadedFilesModel>();
            response.responseData = files;
            try
            {
                using (MySqlConnection connection = SATConnection.getXpressMoneyConnection())
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
                using (MySqlConnection connection = SATConnection.getXpressMoneyConnection())
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
                using (MySqlConnection connection = SATConnection.getXpressMoneyConnection())
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
        private static decimal getFee(string effectivedate)
        {
            decimal fee = 0;
            string sql = "select fee from charge where effectivedate <= @effectivedate order by effectivedate desc limit 1";
            using (MySqlConnection conn = SATConnection.getXpressMoneyConnection())
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@effectivedate", effectivedate);
                    conn.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    fee = reader.GetDecimal("fee");
                    conn.Close();
                }
            }

            return fee;
        }
        public static CustomResponse readSheet(HttpPostedFileBase file, String sheetNo)
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            List<XpressMoneyModel> xpressmoneyData = new List<XpressMoneyModel>();

            IExcelDataReader reader = null;
            try
            {
                // stream = File.Open(file, FileMode.Open, FileAccess.Read);
                //if(file.g)
                if (Path.GetExtension(file.FileName).Equals(".xls"))
                    reader = ExcelReaderFactory.CreateBinaryReader(file.InputStream);
                else
                    reader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream);

                XpressMoneyModel temp = new XpressMoneyModel();
                string xpresscode = "";
                string branchname = "";
                do
                {
                    if (reader.Name.Equals(sheetNo))
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                string c0 = "";

                                try
                                {
                                    c0 = reader.GetValue(0).ToString();
                                    c0 = DateTime.FromOADate(double.Parse(c0)).ToString();
                                }
                                catch (Exception)
                                {

                                    //return new CustomResponse(ResponseCode.Error,
                                    //    e.Message,null
                                    //    );
                                }

                                if (c0.Contains("Agent: ML") && c0.Length != 0)
                                {
                                    string[] t = c0.Split(' ');
                                    temp.xpresscode = c0.Split(' ')[1].Split(',')[0];
                                    string subs = "Inc - ";
                                    if (c0.IndexOf("Inc - ") == -1)
                                    {
                                        subs = "Inc. - ";
                                    }

                                    temp.branchname = (c0.Substring(c0.IndexOf(subs) + 6)).Substring(0, (((c0.Substring(c0.IndexOf(subs) + 6)).Length) - 2));
                                }
                                else
                                {
                                    if (c0.Contains("/"))
                                    {


                                        String[] spltc0 = c0.Split(' ');
                                        String[] dateTempDate = spltc0[0].Split('/');
                                        String[] dateTemptime = spltc0[1].Split(':');

                                        DateTime date = new DateTime(int.Parse(dateTempDate[2]),
                                                                    int.Parse(dateTempDate[0]),
                                                                    int.Parse(dateTempDate[1]),
                                                                    int.Parse(dateTemptime[0]),
                                                                    int.Parse(dateTemptime[1]), 0);
                                        String d = date.ToString("M/dd/yy HH:mm");
                                        String sdate = date.ToString("yyyyMM");
                                        temp.fname = sheetNo;
                                        temp.payoutdate = date.ToString("yyy-MM-dd");

                                        temp.amount = Convert.ToDecimal(reader.GetValue(2).ToString());
                                        temp.refund = Convert.ToDecimal(reader.GetValue(3).ToString());
                                        temp.tax = Convert.ToDecimal(reader.GetValue(4).ToString());
                                        temp.comm = Convert.ToDecimal(reader.GetValue(5).ToString());
                                        temp.amountpaid = Convert.ToDecimal(reader.GetValue(6).ToString());

                                        temp.fee = getFee(date.ToString("yyy-MM-dd"));

                                        temp.currency = reader.GetString(1);
                                        temp.beneficiary = reader.GetString(7);
                                        temp.xpn = string.Format("{0:F0}", long.Parse(reader.GetValue(8).ToString(),
                                         NumberStyles.Float,
                                         CultureInfo.InvariantCulture));
                                        temp.sendingagent = reader.GetString(9);
                                        temp.userid = reader.GetString(10);
                                        if (temp.xpresscode == null)
                                        {
                                            temp.xpresscode = xpresscode;
                                            temp.branchname = branchname;
                                        }
                                        else
                                        {
                                            xpresscode = temp.xpresscode;
                                            branchname = temp.branchname;
                                        }



                                        xpressmoneyData.Add(temp);
                                        temp = new XpressMoneyModel();





                                    }
                                    else
                                    {
                                        //no data
                                    }
                                }
                            }



                        }
                } while (reader.NextResult());

                reader.Close();
                reader.Dispose();

            }
            catch (Exception e)
            {
                reader.Close();
                reader.Dispose();

                MySession.log.Info(e.Message, e);
                return new CustomResponse(ResponseCode.Error, e.Message, null);
            }
            return new CustomResponse(ResponseCode.OK, xpressmoneyData.Count.ToString() + " records found", xpressmoneyData);
        }
        public static CustomResponse insetToXpressmoney(List<XpressMoneyModel> xpressMoneyData, string fname)
        {
            MySession.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            CustomResponse response = new CustomResponse();
            String uploadfileString = "INSERT INTO uploadedfiles(fname,ftotal,syscreator) values(@fname,@ftotal,@syscreator)";
            string tablename = "xpressmoney" + xpressMoneyData[0].payoutdate.Split('-')[0] + xpressMoneyData[0].payoutdate.Split('-')[1];
            string sql = "INSERT INTO " + tablename + "(xpresscode,branchname,payoutdate," +
                        "currency,amount,refund,tax,comm,amountpaid,beneficiary," +
                        "xpn,sendingagent,userid,fname,fee) VALUES" +
                        "(@xpresscode,@branchname,@payoutdate," +
                        "@currency,@amount,@refund,@tax,@comm,@amountpaid,@beneficiary," +
                        "@xpn,@sendingagent,@userid,@fname,@fee)";

            string tableCommand = "CREATE TABLE if not exists " + tablename + " (" +
                            "id bigint(11) NOT NULL auto_increment, " +
                            "xpresscode varchar(50) default NULL, " +
                            "branchname varchar(100) default NULL, " +
                            "payoutdate date default NULL, " +
                            "currency varchar(50) default NULL, " +
                            "amount decimal(16,2) default NULL,    " +
                            "refund decimal(16,2) default NULL,    " +
                            "tax decimal(16,2) default NULL,    " +
                            "comm decimal(16,2) default NULL,   " +
                            "amountpaid decimal(16,2) default NULL,    " +
                            "beneficiary varchar(100) default NULL, " +
                            "xpn varchar(50) default NULL, " +
                            "sendingagent varchar(50) default NULL, " +
                            "userid varchar(100) default NULL, " +
                            "fname varchar(50) default NULL,  " +
                            "fee decimal(10,2) default NULL, " +
                            "PRIMARY KEY  (`id`)," +
                            "KEY `payoutdate` (`payoutdate`,`currency`), " +
                            "KEY `xpresscode` (`xpresscode`) " +
                            ") ENGINE=InnoDB DEFAULT CHARSET=latin1";
            MySqlTransaction transaction;
            MySqlConnection connection = SATConnection.getXpressMoneyConnection();

            int i = 0;
            try
            {
                connection.Open();
                connection.Execute(tableCommand);
                transaction = connection.BeginTransaction();
                connection.Execute(uploadfileString, new { fname = fname, ftotal = xpressMoneyData.Count, syscreator = MySession.userID });
                var count = connection.Execute(sql,xpressMoneyData,transaction);
                if (count == xpressMoneyData.Count)
                {
                    transaction.Commit();
                    connection.Close();
                    response.responseCode = ResponseCode.OK;
                    response.responseData = i;
                    response.responseMessage = count.ToString() + " records inserted";
                    MySession.log.Info(i + " records inserted");
                }
                else
                {
                    transaction.Rollback();
                    connection.Close();
                    MySession.log.Error("Something went wrong" + "last insert index: " + i);
                    response = new CustomResponse(ResponseCode.Error, "Something went wrong" + "last insert index: " + i, null);
                }




            }
            catch (Exception e)
            {
                MySession.log.Error(e.Message, e);
                response = new CustomResponse(ResponseCode.Error, e.Message, null);
            }
            return response;


        }
    }
}