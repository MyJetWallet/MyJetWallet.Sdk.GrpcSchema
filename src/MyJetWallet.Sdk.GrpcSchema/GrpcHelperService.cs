using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MyJetWallet.Sdk.GrpcSchema
{
    public class GrpcHelperService : IGrpcHelperService
    {
        public Data<DateTime> StringToDateTime(Data<string> str)
        {
            return new Data<DateTime>(DateTime.Parse(str.Value));
        }

        public Data<string> DateTimeToString(Data<DateTime> time)
        {
            return new Data<string>($"{time.Value:O}");
        }

        public Data<decimal> StringToDecimal(Data<string> str)
        {
            return new Data<decimal>(decimal.Parse(str.Value));
        }

        public Data<string> DecimalToString(Data<decimal> value)
        {
            return new Data<string>(value.Value.ToString(CultureInfo.InvariantCulture));
        }

        [DataContract]
        public class Data<T>
        {
            public Data()
            {
            }

            public Data(T value)
            {
                Value = value;
            }

            [DataMember(Order = 1)] public T Value { get; set; }
        }
    }

    [ServiceContract]
    public interface IGrpcHelperService
    {
        [OperationContract]
        GrpcHelperService.Data<DateTime> StringToDateTime(GrpcHelperService.Data<string> str);

        [OperationContract]
        GrpcHelperService.Data<string> DateTimeToString(GrpcHelperService.Data<DateTime> time);

        [OperationContract]
        GrpcHelperService.Data<decimal> StringToDecimal(GrpcHelperService.Data<string> str);

        [OperationContract]
        GrpcHelperService.Data<string> DecimalToString(GrpcHelperService.Data<decimal> value);
    }

    
}
