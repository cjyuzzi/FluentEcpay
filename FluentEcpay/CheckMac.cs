using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using FluentEcpay.Interfaces;

namespace FluentEcpay
{
    public class CheckMac : ICheckMac
    {
        private readonly string _hashKey;
        private readonly string _hashIV;
        public CheckMac(string hashKey, string hashIV)
        {
            _hashKey = hashKey ?? throw new System.ArgumentNullException(nameof(hashKey));
            _hashIV = hashIV ?? throw new System.ArgumentNullException(nameof(hashIV));
        }
        public string GetValue(IPayment payment, EHashAlgorithm encryptType = EHashAlgorithm.SHA256)
        {
            var parameters = GeneratePaymentDictionary(payment);
            return GetValue(parameters, _hashKey, _hashIV, encryptType);
        }
        public static string GetValue(IDictionary<string, string> parameters, string key, string iv, EHashAlgorithm encryptType = EHashAlgorithm.SHA256)
        {
            string checkMacValue = string.Empty;
            var sortedParameters = parameters
                .Where(o => !string.IsNullOrEmpty(o.Value))
                .OrderBy(o => o.Key)
                .Select(param => $"{param.Key}={param.Value}");
            var parameterString = string.Join("&", sortedParameters);
            checkMacValue = $"HashKey={key}&{parameterString}&HashIV={iv}";
            checkMacValue = HttpUtility.UrlEncode(checkMacValue).ToLower();
            switch (encryptType)
            {
                case EHashAlgorithm.SHA256:
                    checkMacValue = SHA256ComputeHash(checkMacValue);
                    break;
                default:
                    checkMacValue = MD5ComputeHash(checkMacValue);
                    break;
            }
            return checkMacValue.ToUpper();
        }
        public static string GetValue(IPaymentResult result, string key, string iv, EHashAlgorithm encryptType = EHashAlgorithm.SHA256)
        {
            var properties = typeof(IPaymentResult).GetProperties();
            var parameters = properties
                .Where(property => property.Name != nameof(PaymentResult.CheckMacValue))
                .Where(property => property.GetValue(result) != null)
                .ToDictionary(property => property.Name, property => property.GetValue(result).ToString());
            return CheckMac.GetValue(parameters, key, iv, encryptType);
        }
        private static string SHA256ComputeHash(string toHash)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.UTF8.GetBytes(toHash);
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                var array = sha256.ComputeHash(bytes);
                for (int i = 0; i < array.Length; i++)
                {
                    sb.Append(array[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }
        private static string MD5ComputeHash(string toHash)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.UTF8.GetBytes(toHash);
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var array = md5.ComputeHash(bytes);
                for (int i = 0; i < array.Length; i++)
                {
                    sb.Append(array[i].ToString("X").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }
        private IDictionary<string, string> GeneratePaymentDictionary(IPayment payment) => new Dictionary<string, string>() { { nameof (payment.MerchantID), payment.MerchantID }, { nameof (payment.MerchantTradeNo), payment.MerchantTradeNo }, { nameof (payment.MerchantTradeDate), payment.MerchantTradeDate }, { nameof (payment.PaymentType), payment.PaymentType }, { nameof (payment.TotalAmount), payment.TotalAmount.ToString () }, { nameof (payment.TradeDesc), payment.TradeDesc }, { nameof (payment.ItemName), payment.ItemName }, { nameof (payment.ReturnURL), payment.ReturnURL }, { nameof (payment.ClientBackURL), payment.ClientBackURL }, { nameof (payment.ChoosePayment), payment.ChoosePayment }, { nameof (payment.EncryptType), payment.EncryptType.ToString () }, { nameof (payment.StoreID), payment.StoreID }, { nameof (payment.ItemURL), payment.ItemURL }, { nameof (payment.Remark), payment.Remark }, { nameof (payment.ChooseSubPayment), payment.ChooseSubPayment }, { nameof (payment.OrderResultURL), payment.OrderResultURL }, { nameof (payment.NeedExtraPaidInfo), payment.NeedExtraPaidInfo }, { nameof (payment.IgnorePayment), payment.IgnorePayment }, { nameof (payment.PlatformID), payment.PlatformID }, { nameof (payment.InvoiceMark), payment.InvoiceMark }, { nameof (payment.CustomField1), payment.CustomField1 }, { nameof (payment.CustomField2), payment.CustomField2 }, { nameof (payment.CustomField3), payment.CustomField3 }, { nameof (payment.CustomField4), payment.CustomField4 }, { nameof (payment.Language), payment.Language }, { nameof (payment.UnionPay), payment.UnionPay.ToString () }
        };
    }
}