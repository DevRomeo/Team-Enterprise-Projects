using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using log4net;


namespace EDIAgent.query
{
    class TransactionsPending
    {
        ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        String baseSynergyStatus = "0";
        String updateSynergyStatus = "1";

        
        public TransactionsPending() 
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public TransactionsPending(String baseSynergyStatus, String updateSynergyStatus)
        {
            log4net.Config.XmlConfigurator.Configure();
            this.baseSynergyStatus = baseSynergyStatus;
            this.updateSynergyStatus = updateSynergyStatus;
        }
        private List<dataClass.TransactionsPending> getPendingTransactions() 
        {
            string sql = "    select    paymentmethod,freefield4,freefield5, CompanyCode, TransactionType, TransactionDate,  "+
                        "    FinYear,FinPeriod, [Description], CompanyAccountCode,EntryNumber,CurrencyAliasAC,AmountDebitAC,AmountCreditAC,  "+
                        "    CurrencyAliasFC,AmountDebitFC,AmountCreditFC,VATCode,ProcessNumber,ProcessLineCode, res_id, oorsprong,docdate,  "+
                        "    vervdatfak,faktuurnr,coalesce(syscreator,''),sysmodifier,EntryGuid,companycostcentercode,batchNo   " +
                        "    FROM TransactionsPending  " +
                        "    where synergy_status="+baseSynergyStatus;
            List<dataClass.TransactionsPending> data = new List<dataClass.TransactionsPending>();
            using (SqlConnection ediconnection = new util.DBConnections().getEDIConnection()) 
            {
                dataClass.TransactionsPending row = new dataClass.TransactionsPending();
                using (SqlCommand edicommand = new SqlCommand(sql, ediconnection)) 
                {
                    ediconnection.Open();
                    SqlDataReader reader = edicommand.ExecuteReader();
                    while (reader.Read()) 
                    {
                        row = new dataClass.TransactionsPending();
                        row.paymentmethod = reader.GetValue(0).ToString();
                        row.freefield4 = reader.GetValue(1).ToString();
                        row.freefield5 = reader.GetValue(2).ToString();
                        row.CompanyCode = reader.GetValue(3).ToString();
                        row.TransactionType = reader.GetValue(4).ToString();
                        row.TransactionDate = reader.GetValue(5).ToString();
                        row.FinYear = reader.GetValue(6).ToString();
                        row.FinPeriod = reader.GetValue(7).ToString();
                        row.Description = reader.GetValue(8).ToString();
                        row.CompanyAccountCode = reader.GetValue(9).ToString();
                        row.EntryNumber = reader.GetValue(10).ToString();
                        row.CurrencyAliasAC = reader.GetValue(11).ToString();
                        row.AmountDebitAC = reader.GetValue(12).ToString();
                        row.AmountCreditAC = reader.GetValue(13).ToString();
                        row.CurrencyAliasFC = reader.GetValue(14).ToString();
                        row.AmountDebitFC = reader.GetValue(15).ToString();
                        row.AmountCreditFC = reader.GetValue(16).ToString();
                        row.VATCode = reader.GetValue(17).ToString();
                        row.ProcessNumber = reader.GetValue(18).ToString();
                        row.ProcessLineCode = reader.GetValue(19).ToString();
                        row.res_id = reader.GetValue(20).ToString();
                        row.oorsprong = reader.GetValue(21).ToString();
                        row.docdate = reader.GetValue(22).ToString();
                        row.vervdatfak = reader.GetValue(23).ToString();
                        row.faktuurnr = reader.GetValue(24).ToString();
                        row.syscreator = reader.GetValue(25).ToString();
                        row.sysmodifier = reader.GetValue(26).ToString();
                        row.EntryGuid = reader.GetValue(27).ToString();
                        row.companycostcentercode = reader.GetValue(28).ToString();
                        row.batchNo = reader.GetString(29);
                        data.Add(row);
                    }
                    ediconnection.Close();
                }
            }
            
            return data;
        }

        public Response insertByBatchByBranch(List<dataClass.TransactionsPending> data) 
        {
            Response response = new Response();
            String batchNo = data.Select(xx => xx.batchNo).First().ToString();
            String comCode = data.Select(xx => xx.CompanyCode).First().ToString();
            int   fakn = getNumber(comCode);
            String edisql = "update TransactionsPending SET synergy_status=@updateSynergyStatus ," +
                  "synergy_linkdate = getdate() where synergy_status=@baseSynergyStatus " +
                  "and BatchNo=@BatchNo";
            string synergySql = "Insert into TransactionsPending" +
                             "(paymentmethod,freefield4,freefield5, CompanyCode, TransactionType, TransactionDate, " +
                             "FinYear,FinPeriod, Description, CompanyAccountCode,EntryNumber,CurrencyAliasAC,AmountDebitAC,AmountCreditAC,  " +
                             "CurrencyAliasFC,AmountDebitFC,AmountCreditFC,VATCode,ProcessNumber,ProcessLineCode, res_id, oorsprong,docdate," +
                             "vervdatfak,faktuurnr,syscreator,sysmodifier,EntryGuid,companycostcentercode )  " +
                             "values " +
                             "(@paymentmethod,@freefield4,@freefield5,@CompanyCode, @TransactionType, @TransactionDate, " +
                             "@FinYear,@FinPeriod, @Description, @CompanyAccountCode,@EntryNumber,@CurrencyAliasAC,@AmountDebitAC,@AmountCreditAC,  " +
                             "@CurrencyAliasFC,@AmountDebitFC,@AmountCreditFC,@VATCode,@ProcessNumber,@ProcessLineCode, @res_id, @oorsprong,@docdate," +
                             "@vervdatfak,@faktuurnr,@syscreator,@sysmodifier,@EntryGuid,@companycostcentercode )  ";

            SqlConnection ediConnection = new util.DBConnections().getEDIConnection();
            SqlConnection synergyConnection = new util.DBConnections().getSynergyConnection();
            SqlTransaction editransaction = null;
            SqlTransaction synergyTransaction = null;
            try 
            {
                
                    using (SqlCommand synergyCommand = new SqlCommand(synergySql, synergyConnection))
                    {
                        synergyCommand.Parameters.Add("@paymentmethod", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@freefield4", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@freefield5", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@CompanyCode", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@TransactionType", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@TransactionDate", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@FinYear", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@FinPeriod", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@CompanyAccountCode", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@EntryNumber", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@CurrencyAliasAC", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@AmountDebitAC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@AmountCreditAC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@CurrencyAliasFC", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@AmountDebitFC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@AmountCreditFC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@VATCode", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@ProcessNumber", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@ProcessLineCode", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@res_id", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@oorsprong", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@docdate", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@vervdatfak", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@faktuurnr", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@syscreator", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@sysmodifier", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@EntryGuid", System.Data.SqlDbType.UniqueIdentifier);
                        synergyCommand.Parameters.Add("@companycostcentercode", System.Data.SqlDbType.VarChar);
                        synergyCommand.CommandTimeout = 0;
                        synergyConnection.Open();
                        synergyTransaction = synergyConnection.BeginTransaction();
                        synergyCommand.Transaction = synergyTransaction;

                          SqlCommand ediCommand = new SqlCommand(edisql, ediConnection);
                        ediConnection.Open();
                        editransaction = ediConnection.BeginTransaction();
                        ediCommand.Transaction = editransaction;
                    }

                    
            }
            catch (Exception e) 
            {
                if(synergyTransaction!=null)
                    synergyTransaction.Rollback();
                if(editransaction!=null)
                    editransaction.Rollback();
                response.responseCode = ResponseCode.Error;
                response.responseMessage = e.Message;
                
            }
            finally
            {
                if(synergyConnection.State == System.Data.ConnectionState.Open)
                    synergyConnection.Close();
                if(ediConnection.State == System.Data.ConnectionState.Open)
                    ediConnection.Close();
            }

            return response;
        }
        public void start() 
        {

            List<dataClass.TransactionsPending> data = getPendingTransactions();
            if (data.Count < 1)
                return;

            log.Info("New entries found starting  synergy insert.");




            var batchNos = data
                .GroupBy(btch => btch.batchNo)
                .Select(grp => grp.First().batchNo)
                .ToArray();

            log.Info(data.Count.ToString() + " transactions, " + batchNos.Length.ToString() + " entries");


            SqlTransaction synergytransaction=null;
            SqlTransaction editransaction = null;
            int synergyUpdate = 0;
            int ediupdate = 0;
            string synergySql = "Insert into TransactionsPending" +
                                "(paymentmethod,freefield4,freefield5, CompanyCode, TransactionType, TransactionDate, " +
                                "FinYear,FinPeriod, Description, CompanyAccountCode,EntryNumber,CurrencyAliasAC,AmountDebitAC,AmountCreditAC,  " +
                                "CurrencyAliasFC,AmountDebitFC,AmountCreditFC,VATCode,ProcessNumber,ProcessLineCode, res_id, oorsprong,docdate," +
                                "vervdatfak,faktuurnr,syscreator,sysmodifier,EntryGuid,companycostcentercode )  " +
                                "values " +
                                "(@paymentmethod,@freefield4,@freefield5,@CompanyCode, @TransactionType, @TransactionDate, " +
                                "@FinYear,@FinPeriod, @Description, @CompanyAccountCode,@EntryNumber,@CurrencyAliasAC,@AmountDebitAC,@AmountCreditAC,  " +
                                "@CurrencyAliasFC,@AmountDebitFC,@AmountCreditFC,@VATCode,@ProcessNumber,@ProcessLineCode, @res_id, @oorsprong,@docdate," +
                                "@vervdatfak,@faktuurnr,@syscreator,@sysmodifier,@EntryGuid,@companycostcentercode )  ";

            string edisql = "";/* "update TransactionsPending SET synergy_status=" + updateSynergyStatus + ", " +
                         "synergy_linkdate = getdate() where synergy_status="+baseSynergyStatus;*/
            for (int i = 0; i < batchNos.Length; i++)
            {
                var ddata = data.Where(xx => xx.batchNo == batchNos[i]).ToList();

                using (SqlConnection synergyConnection = new util.DBConnections().getSynergyConnection())
                {

                    using (SqlCommand synergyCommand = new SqlCommand(synergySql, synergyConnection))
                    {
                        synergyCommand.Parameters.Add("@paymentmethod", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@freefield4", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@freefield5", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@CompanyCode", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@TransactionType", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@TransactionDate", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@FinYear", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@FinPeriod", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@CompanyAccountCode", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@EntryNumber", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@CurrencyAliasAC", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@AmountDebitAC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@AmountCreditAC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@CurrencyAliasFC", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@AmountDebitFC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@AmountCreditFC", System.Data.SqlDbType.Float);
                        synergyCommand.Parameters.Add("@VATCode", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@ProcessNumber", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@ProcessLineCode", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@res_id", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@oorsprong", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@docdate", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@vervdatfak", System.Data.SqlDbType.VarChar);
                        synergyCommand.Parameters.Add("@faktuurnr", System.Data.SqlDbType.Char);
                        synergyCommand.Parameters.Add("@syscreator", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@sysmodifier", System.Data.SqlDbType.Int);
                        synergyCommand.Parameters.Add("@EntryGuid", System.Data.SqlDbType.UniqueIdentifier);
                        synergyCommand.Parameters.Add("@companycostcentercode", System.Data.SqlDbType.VarChar);
                        synergyCommand.CommandTimeout = 0;
                        synergyConnection.Open();

                        synergytransaction = synergyConnection.BeginTransaction();
                        synergyCommand.Transaction = synergytransaction;
                        //synergyCommand.Prepare();
                        SqlConnection ediConnection = new util.DBConnections().getEDIConnection();
                        edisql = "update TransactionsPending SET synergy_status=" + updateSynergyStatus + ", " +
                       "synergy_linkdate = getdate() where synergy_status=" + baseSynergyStatus + " " +
                       "and BatchNo='" + batchNos[i]+"'";

                        SqlCommand ediCommand = new SqlCommand(edisql, ediConnection);
                        ediConnection.Open();
                        editransaction = ediConnection.BeginTransaction();
                        ediCommand.Transaction = editransaction;
                        try
                        {

                            var compCodes = ddata
                                .GroupBy(compCode => compCode.CompanyCode)
                                .Select(grp => grp.First().CompanyCode)
                                .ToArray();

                           
                            for (int j = 0; j < compCodes.Length; j++)
                            {
                                var comCodeData = ddata.Where(xx => xx.CompanyCode == compCodes[j]).ToList();
                                int fakn = 0;
                                int fn = 0;
       
                                String fkupdatesql = "";
                                String fnsql = "Select FreeNumber from bacofreenumbers " +
                                         "where NumberKey ='RPFinEntry'";

                                String fnUpdatesql = "Update bacofreenumbers " +
                                         "set FreeNumber = FreeNumber + 1 " + //cnt.ToString() + " " +
                                         "where NumberKey ='RPFinEntry'";

                                fakn = getNumber(compCodes[j]);

                                using (SqlConnection numberConnection = new util.DBConnections().getSynergyConnection())
                                {
                                   
                                    using (SqlCommand freeNumberCommand = new SqlCommand(fnsql, numberConnection))
                                    {
                                        numberConnection.Open();
                                        SqlDataReader reader = freeNumberCommand.ExecuteReader();
                                        if (reader.HasRows)
                                        {
                                            reader.Read();
                                            fn = reader.GetInt32(0);
                                        }
                                        else
                                        {
                                            log.Error("Error reading bacoFreenumbers. CompanyCode =" + compCodes[i] + ", BatchNo = " + batchNos[i]);
                                            invalidBacoNumber(compCodes[j], batchNos[i]);
                                            return;
                                        }
                                        reader.Dispose();

                                        numberConnection.Close();
                                    }
                                }
                                fkupdatesql = "update numbers " +
                                                   " set used =1 " +
                                                   "where number =" + fakn + " " +
                                                   "and CompanyCode =" + compCodes[j];
                                ////
                                for (int k = 0; k < comCodeData.Count; k++)
                                {
                                    var temp = comCodeData[k];
                                    synergyCommand.Parameters["@paymentmethod"].Value = temp.paymentmethod;
                                    synergyCommand.Parameters["@freefield4"].Value = temp.freefield4;
                                    synergyCommand.Parameters["@freefield5"].Value = temp.freefield5;
                                    synergyCommand.Parameters["@CompanyCode"].Value = temp.CompanyCode;
                                    synergyCommand.Parameters["@TransactionType"].Value = temp.TransactionType;
                                    synergyCommand.Parameters["@TransactionDate"].Value = temp.TransactionDate;
                                    synergyCommand.Parameters["@FinYear"].Value = temp.FinYear;
                                    synergyCommand.Parameters["@FinPeriod"].Value = temp.FinPeriod;
                                    synergyCommand.Parameters["@Description"].Value = temp.Description;
                                    synergyCommand.Parameters["@CompanyAccountCode"].Value = temp.CompanyAccountCode;
                                    synergyCommand.Parameters["@EntryNumber"].Value = fn.ToString();
                                    synergyCommand.Parameters["@CurrencyAliasAC"].Value = temp.CurrencyAliasAC;
                                    synergyCommand.Parameters["@AmountDebitAC"].Value = temp.AmountDebitAC;
                                    synergyCommand.Parameters["@AmountCreditAC"].Value = temp.AmountCreditAC;
                                    synergyCommand.Parameters["@CurrencyAliasFC"].Value = temp.CurrencyAliasFC;
                                    synergyCommand.Parameters["@AmountDebitFC"].Value = temp.AmountDebitFC;
                                    synergyCommand.Parameters["@AmountCreditFC"].Value = temp.AmountCreditFC;
                                    synergyCommand.Parameters["@VATCode"].Value = temp.VATCode;
                                    synergyCommand.Parameters["@ProcessNumber"].Value = temp.ProcessNumber;
                                    synergyCommand.Parameters["@ProcessLineCode"].Value = temp.ProcessLineCode;
                                    synergyCommand.Parameters["@res_id"].Value = temp.res_id;
                                    synergyCommand.Parameters["@oorsprong"].Value = temp.oorsprong;
                                    synergyCommand.Parameters["@docdate"].Value = temp.docdate;
                                    synergyCommand.Parameters["@vervdatfak"].Value = temp.vervdatfak;
                                    synergyCommand.Parameters["@faktuurnr"].Value = fakn.ToString();
                                    synergyCommand.Parameters["@syscreator"].Value = temp.syscreator;
                                    synergyCommand.Parameters["@sysmodifier"].Value = temp.sysmodifier;
                                    synergyCommand.Parameters["@EntryGuid"].Value = new Guid(temp.EntryGuid);
                                    synergyCommand.Parameters["@companycostcentercode"].Value = temp.companycostcentercode;
                                    synergyUpdate += synergyCommand.ExecuteNonQuery();
                                    log.Info("Loading CompanyCode: " + temp.CompanyCode + ", EntryNumber: " + fn.ToString() + ",faktuurnr: " + fakn.ToString());


                                }
                                ///update
                                ///
                                using (SqlConnection numberConnection = new util.DBConnections().getSynergyConnection())
                                {
                                    using (SqlCommand numberCommand = new SqlCommand(fkupdatesql, numberConnection))
                                    {
                                        numberConnection.Open();
                                        numberCommand.ExecuteNonQuery();
                                        numberConnection.Close();
                                    }
                                    using (SqlCommand freeNumberCommand = new SqlCommand(fnUpdatesql, numberConnection))
                                    {
                                        numberConnection.Open();
                                        freeNumberCommand.ExecuteNonQuery();
                                        numberConnection.Close();
                                        //    cnt++;
                                    }
                                }


                            }



                          
                            ediupdate = ediCommand.ExecuteNonQuery();
                            if (synergyUpdate == ediupdate)
                            {
                                synergytransaction.Commit();
                                editransaction.Commit();
                                log.Info("Insert to synergy success " + synergyUpdate.ToString() + " transactions inserted .");
                            }
                            else
                            {
                                synergytransaction.Rollback();
                                editransaction.Rollback();
                                log.Warn("Something went wrong transaction insert rollback");

                            }
                            synergyConnection.Close();
                            ediConnection.Close();
                        }
                        catch (Exception e)
                        {
                            string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
                            util.INI ini = new util.INI(inipath);

                            log.Error(e.Message + " " + ini.IniReadValue("SYS", "AgentName"), e);
                            editransaction.Rollback();
                            synergytransaction.Rollback();
                            synergyConnection.Close();
                            ediConnection.Close();

                        }
                        
                    }
                }//end of usingconnection
            }//end of batchnoLoop
        }//end of start
        private void invalidNumber(String companyCode, String batchNo)
        {
            String sql = "update TransactionsPending SET synergy_status= 9 ," +
                       "synergy_linkdate = getdate() " +
                       "where BatchNo=@BatchNo and CompanyCode = @CompanyCode ";
            using (SqlConnection connection = new util.DBConnections().getEDIConnection())
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@BatchNo", batchNo);
                    command.Parameters.AddWithValue("@CompanyCode", companyCode);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }//end of invalid number
        private void invalidBacoNumber(String companyCode, String batchNo)
        {
            String sql = "update TransactionsPending SET synergy_status= 10 ," +
                       "synergy_linkdate = getdate() " +
                       "where BatchNo=@BatchNo and CompanyCode = @CompanyCode ";
            using (SqlConnection connection = new util.DBConnections().getEDIConnection())
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@BatchNo", batchNo);
                    command.Parameters.AddWithValue("@CompanyCode", companyCode);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }//end of invalid baconumber    

        private int getLastNumber(String companyCode)
        {
            String sql = "Select top 1 number from numbers where companyCode = @companyCode "+
                          " order by number desc";
            int number = 0;
            using (SqlConnection connection = new util.DBConnections().getSynergyConnection())
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@companyCode", companyCode);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        number = reader.GetInt32(0);
                    }
                        
                    connection.Close();
                }

            }
            return number;
            
        }//end of getLastNumber
       
        private void addNumber(String companyCode)
        {
            String sql = "Insert into numbers(companyCode,Type,Number,Used,rowguid) " +
                         " Values(@companyCode,1,@Number,0,newid()) ";
            int newnumber = getLastNumber(companyCode)+1;
            using (SqlConnection connection = new util.DBConnections().getSynergyConnection()) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@companyCode", companyCode);
                    command.Parameters.AddWithValue("@Number", newnumber);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }//end of addNumber

        private int getNumber(String companyCode) 
        {
            int number = 0;
            String sql = " select top 1 number from numbers " +
                         " where CompanyCode =@companyCode " +
                         " and used =0 order by number asc";
            using (SqlConnection connection = new util.DBConnections().getSynergyConnection()) 
            {
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@companyCode", companyCode);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        number = reader.GetInt32(0);
                    }
                    connection.Close();
                }
            }
            if (number == 0)
            {
                log.Info("No available number. CompanyCode =" + companyCode + "adding new number ");
                addNumber(companyCode);
                number = getNumber(companyCode);
            }
            return number;
        }//end of getNumber


    }//end of class
}//end of namespace
