using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MyJetWallet.Sdk.GrpcSchema
{
    public class GrpcHelperService : IGrpcHelperService
    {
        public DateTime StringToDateTime(string str)
        {
            return DateTime.Parse(str);
        }

        public string DateTimeToString(DateTime time)
        {
            return $"{time:O}";
        }

        public decimal StringToDecimal(string str)
        {
            return decimal.Parse(str);
        }

        public string DecimalToString(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }

    [ServiceContract]
    public interface IGrpcHelperService
    {
        [OperationContract]
        DateTime StringToDateTime(string str);

        [OperationContract]
        string DateTimeToString(DateTime time);

        [OperationContract]
        decimal StringToDecimal(string str);

        [OperationContract]
        string DecimalToString(decimal value);
    }
}
