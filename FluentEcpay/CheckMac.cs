using System;
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
        #region Fields
        private readonly string _hashKey;
        private readonly string _hashIV;
        #endregion
        #region CTOR
        public CheckMac(string hashKey, string hashIV)
        {
            _hashKey = hashKey ?? throw new ArgumentNullException(nameof(hashKey));
            _hashIV = hashIV ?? throw new ArgumentNullException(nameof(hashIV));
        }
        #endregion
        #region Public methods
        public string GetValue(IPayment payment, EHashAlgorithm encryptType = EHashAlgorithm.SHA256)
        {
            if (payment is null) throw new ArgumentNullException(nameof(payment));
            var properties = typeof(IPayment).GetProperties();
            var parameters = properties
                .Where(property => property.Name != "URL")
                .Where(property => property.GetValue(payment) != null)
                .ToDictionary(property => property.Name, property => property.GetValue(payment).ToString());
            return GetValue(parameters, _hashKey, _hashIV, encryptType);
        }
        public static bool PaymentResultIsValid(PaymentResult result, string hashKey, string hashIV, EHashAlgorithm encryptType = EHashAlgorithm.SHA256)
        {
            if (result is null) throw new ArgumentNullException(nameof(result));
            if (result.CheckMacValue is null) throw new ArgumentNullException(nameof(result.CheckMacValue));
            if (string.IsNullOrEmpty(hashKey)) throw new ArgumentNullException(nameof(result));
            if (string.IsNullOrEmpty(hashIV)) throw new ArgumentNullException(nameof(result));

            var properties = typeof(IPaymentResult).GetProperties();
            var parameters = properties
                .Where(property => property.Name != nameof(PaymentResult.CheckMacValue))
                .ToDictionary(property => property.Name, property =>
                {
                    var value = property.GetValue(result);
                    return value is null ? string.Empty : value.ToString();
                });
            var toCheck = CheckMac.GetValue(parameters, hashKey, hashIV, encryptType);
            return toCheck == result.CheckMacValue;
        }
        public static string GetValue(IDictionary<string, string> parameters, string key, string iv, EHashAlgorithm encryptType = EHashAlgorithm.SHA256)
        {
            string checkMacValue = string.Empty;
            var sortedParameters = parameters
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
        #endregion
        #region Private methods
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
        #endregion
    }
}