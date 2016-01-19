using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using Newtonsoft.Json;
using EDIdataClass;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{

    public static ILog log;
    public Service()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent.Config.XmlConfigurator();
        
        log4net.Config.XmlConfigurator.Configure();
        log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        log.Info("res");
    }

    [WebMethod]
    public String getBranch(String bcode,String edi)
    {        
        String temp=JsonConvert.SerializeObject(new Branch().getBranchInfo(bcode,edi));
        return temp;
    }
    [WebMethod]
    public String getBranchBOSKP(String kpcode, String edi)
    {
        
        return JsonConvert.SerializeObject(new Branch().getBranchInfoBOSKP(kpcode, edi));
    }
    [WebMethod]
    public String getCorpPartners(String CORPCODE, String edi)
    {

        return JsonConvert.SerializeObject(new Branch().getCorporatePartner(CORPCODE, edi));
    }

    [WebMethod]
    public String insertToWu_unknownbc(String bcode, String dr1, String dr2, String edi)
    {
        return JsonConvert.SerializeObject( new EDITablInsert().insertToWu_unknownbc(bcode, dr1, dr2,  edi)) ;
    }
    [WebMethod]
    public String isActionedWu_kebot(String bcode, String month_ern, String year_ern, String edi) 
    {
        return JsonConvert.SerializeObject(new Validate().isActionedWu_kebot(bcode,month_ern, year_ern, edi)) ;
    }

    [WebMethod]
    public String isActionedEdiTable(String tablename, String g_month, String edi) 
    {
        return JsonConvert.SerializeObject(new Validate().isActionedEdiTable(tablename,g_month,edi) );
    }

   
    [WebMethod]
    public string logdb(String datetimelog, String application, String activity, String resource, String department, String remarks)
    {
        return JsonConvert.SerializeObject(new DBLog().insertToMaintainanceLogs(datetimelog, application, activity, resource, department, remarks));
    }
    
    [WebMethod]
    public string getGuid() 
    {
        return Guid.NewGuid().ToString();
    }
    [WebMethod]
    public String alreadySaved(String bcode, String month, String year, String descValue,String tableName,String descColl, String edi)
    {
        return JsonConvert.SerializeObject(new Validate().ifAlreadySaved(bcode, month, year, descValue, tableName, descColl, edi)); 
    }
    [WebMethod]
    public String alreadySavedBase(String bcode, String month, String year,  String tableName, String edi)
    {
        return JsonConvert.SerializeObject(new Validate().ifAlreadySavedBase(bcode, month, year,tableName, edi));
    }
    [WebMethod]
    public String insertTransactionWithDesc(String jsondata, String jsonediTableData, String tableName, String desc, String edi)
    {
        try 
        {
            List<transactionsPending> transactiondata = (List<transactionsPending>)JsonConvert.DeserializeObject(jsondata, typeof(List<transactionsPending>));
            List<EdiDesc> editabledata = (List<EdiDesc>)JsonConvert.DeserializeObject(jsonediTableData, typeof(List<EdiDesc>));
            return JsonConvert.SerializeObject(new TransactionsPending().insertDescEdi(transactiondata, editabledata, tableName, desc, edi));
        
        }
        catch (Exception e) 
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(edi + " " + e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    [WebMethod]
    public String insertTransactionBase(String jsondata, String jsonediTableData, String tableName,String edi)
    {
        try
        {
            List<transactionsPending> transactiondata = (List<transactionsPending>)JsonConvert.DeserializeObject(jsondata, typeof(List<transactionsPending>));
            List<BaseEdi> editabledata = (List<BaseEdi>)JsonConvert.DeserializeObject(jsonediTableData, typeof(List<BaseEdi>));
            return JsonConvert.SerializeObject(new TransactionsPending().insertBaseEdi(transactiondata, editabledata, tableName, edi));

        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(edi + " " + e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    [WebMethod]
    public String validateUserById(String userId) 
    {
        try 
        {
            return JsonConvert.SerializeObject(new Validate().validateUserByUserID(userId));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("UserID"+userId+ " " + e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    [WebMethod]
    public String validateUserByProgdesc(String progdesc)
    {
        try
        {
            return JsonConvert.SerializeObject(new Validate().validateUserByProgdesc(progdesc));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("progdesc: " + progdesc + " " + e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }

    [WebMethod]
    public String getAllBoskp(String edi) 
    {
        try
        {
           return JsonConvert.SerializeObject(new Branch().getAllBoskp(edi));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    [WebMethod]
    public String getAllCorpPartners(String edi)
    {
        try
        {
            return JsonConvert.SerializeObject(new Branch().getAllCorporatePartner(edi));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    [WebMethod]
    public String addBoskp(String data, String edi) 
    {
        try 
        {
            Boskp brnch =(Boskp) JsonConvert.DeserializeObject(data, typeof(Boskp));
            return JsonConvert.SerializeObject(new Branch().addBoskp(brnch, edi));
        }
        catch (Exception e) 
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }    
    }
    [WebMethod]
    public String addCorporate(String data, String edi) 
    {
        try
        {
            Corporate_Partners partner = (Corporate_Partners)JsonConvert.DeserializeObject(data, typeof(Corporate_Partners));
            return JsonConvert.SerializeObject(new Branch().addCorpPartner(partner, edi));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }

    [WebMethod]
    public String updateBoskp(string data,String bcode, String edi)
    {
        try
        {
            Boskp bskp = (Boskp)JsonConvert.DeserializeObject(data, typeof(Boskp));
            return JsonConvert.SerializeObject(new Branch().updateBoskp(bskp,bcode, edi));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    [WebMethod]
    public String updateCorporate(string data,String corpcode ,String edi)
    {
        try
        {
            Corporate_Partners partner = (Corporate_Partners)JsonConvert.DeserializeObject(data, typeof(Corporate_Partners));
            return JsonConvert.SerializeObject(new Branch().updateCorpPartner(partner,corpcode, edi));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }

    [WebMethod]
    public String getBoskpviaBoscode(String boscode, String edi) 
    {
        try
        {
            return JsonConvert.SerializeObject(new Branch().getBranchInfoBOSKPViaBoscode(boscode,edi));
        }
        catch (Exception e)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(e.Message, e);
            return JsonConvert.SerializeObject(new Response(ResponseCode.Error, e.Message));
        }
    }
    
}