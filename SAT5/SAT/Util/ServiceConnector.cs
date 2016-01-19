using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAT.Enum;
using SAT.Models;
namespace SAT.Util
{
    public static class ServiceConnector
    {
        public static CustomResponse getXoomCharge(double amount, String date)
        {
            return new CustomResponse { responseCode = ResponseCode.OK, responseData = 5.5 };
        }
        public static CustomResponse getMoneyGramRate(String legacyid, String date)
        {
            double[] rate = new double[2];//index 0 = payoutrate ,index 1= sendoutrate
            rate[0] = 5;
            rate[1] = 6;
            return new CustomResponse { responseCode = ResponseCode.OK, responseData = rate };
        }
        public static CustomResponse getPayoutRate(String date)
        {
            return new CustomResponse { responseCode = ResponseCode.OK, responseData = 5.5 };
        }
    }
}