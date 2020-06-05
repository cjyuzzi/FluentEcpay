using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentConfiguration : IPaymentConfiguration
    {
        #region Private Fields
        private readonly string _paymentType = "aio";
        private int? _encryptType;
        private string _url;
        private string _merchantId;
        private string _hashKey;
        private string _hashIV;
        private string _serverUrl;
        private string _clientUrl;
        private string _clientUrlWithExtraPaidInfo;
        private IPayment _payment;
        #endregion

        #region Public Propreties
        public IPaymentSendConfiguration Send { get; }

        public IPaymentReturnConfiguration Return { get; }

        public IPaymentTransactionConfiguration Transaction { get; }
        #endregion

        #region CTOR
        public PaymentConfiguration()
        {
            Send = new PaymentSendConfiguration(
                this,
                url => _url = url,
                id => _merchantId = id,
                key => _hashKey = key,
                iv => _hashIV = iv,
                type => _encryptType = type
            );
            Return = new PaymentReturnConfiguration(
                this,
                url => _serverUrl = url,
                url => _clientUrl = url,
                url => _clientUrlWithExtraPaidInfo = url
            );
            Transaction = new PaymentTransactionConfiguration(
                this,
                payment => _payment = payment
            );
        }
        #endregion

        public IPayment Generate()
        {
            ValidatePayment();

            _payment.URL = _url;
            _payment.MerchantID = _merchantId;
            _payment.ReturnURL = _serverUrl;
            _payment.ClientBackURL = _clientUrl;
            _payment.PaymentType = _paymentType;
            _payment.EncryptType = _encryptType.Value;
            //_payment.StoreID = 
            _payment.OrderResultURL = _clientUrlWithExtraPaidInfo;
            _payment.NeedExtraPaidInfo = string.IsNullOrEmpty(_clientUrlWithExtraPaidInfo) ? null : "Y";
            _payment.DeviceSource = null;
            //_payment.PlatformID = 
            //_payment.InvoiceMark =
            //_payment.Language = 
            _payment.CheckMacValue = GenerateCheckMacValue(_payment);

            return _payment;
        }

        private string GenerateCheckMacValue(IPayment payment)
        {
            string szCheckMacValue = String.Empty;
            var dict = GeneratePaymentDictionary(payment);
            var dictAsc = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            var parameters = string.Join("&", dictAsc.Select(param => $"{param.Key}={param.Value}"));
            szCheckMacValue = String.Format("HashKey={0}&{1}&HashIV={2}", _hashKey, parameters, _hashIV);
            szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();
            if (encryptType == 1)
            {
                szCheckMacValue = SHA256.Create().Hash..Encrypt(szCheckMacValue);
            }
            else { szCheckMacValue = MD5Encoder.Encrypt(szCheckMacValue); }
            return szCheckMacValue;
        }

        private Dictionary<string, string> GeneratePaymentDictionary(IPayment payment) => new Dictionary<string, string>(){
                {nameof(payment.MerchantID), payment.MerchantID},
                {nameof(payment.MerchantTradeNo), payment.MerchantTradeNo},
                {nameof(payment.StoreID), payment.StoreID},
                {nameof(payment.MerchantTradeDate), payment.MerchantTradeDate},
                {nameof(payment.PaymentType), payment.PaymentType},
                {nameof(payment.TotalAmount), payment.TotalAmount.ToString()},
                {nameof(payment.TradeDesc), payment.TradeDesc},
                {nameof(payment.ItemName), payment.ItemName},
                {nameof(payment.ReturnURL), payment.ReturnURL},
                {nameof(payment.ChoosePayment), payment.ChoosePayment},
                {nameof(payment.ClientBackURL), payment.ClientBackURL},
                {nameof(payment.ItemURL), payment.ItemURL},
                {nameof(payment.Remark), payment.Remark},
                {nameof(payment.ChooseSubPayment), payment.ChooseSubPayment},
                {nameof(payment.OrderResultURL), payment.OrderResultURL},
                {nameof(payment.NeedExtraPaidInfo), payment.NeedExtraPaidInfo},
                {nameof(payment.DeviceSource), payment.DeviceSource},
                {nameof(payment.IgnorePayment), payment.IgnorePayment},
                {nameof(payment.PlatformID), payment.PlatformID},
                {nameof(payment.InvoiceMark), payment.InvoiceMark},
                {nameof(payment.CustomField1), payment.CustomField1},
                {nameof(payment.CustomField2), payment.CustomField2},
                {nameof(payment.CustomField3), payment.CustomField3},
                {nameof(payment.CustomField4), payment.CustomField4},
                {nameof(payment.EncryptType), payment.EncryptType.ToString()},
                {nameof(payment.Language), payment.Language},
                {nameof(payment.UnionPay), payment.UnionPay.ToString()}
            };

        private void ValidatePayment()
        {
            if (!_encryptType.HasValue) throw new ArgumentNullException(nameof(_encryptType));
            if (string.IsNullOrEmpty(_url)) throw new ArgumentNullException(nameof(_url));
            if (string.IsNullOrEmpty(_merchantId)) throw new ArgumentNullException(nameof(_merchantId));
            if (string.IsNullOrEmpty(_hashKey)) throw new ArgumentNullException(nameof(_hashKey));
            if (string.IsNullOrEmpty(_hashIV)) throw new ArgumentNullException(nameof(_hashIV));
            if (string.IsNullOrEmpty(_serverUrl)) throw new ArgumentNullException(nameof(_serverUrl));
            if (_payment is null) throw new ArgumentNullException(nameof(_encryptType));
            if (string.IsNullOrEmpty(_payment.MerchantTradeNo)) throw new ArgumentNullException(nameof(_payment.MerchantTradeNo));
            if (string.IsNullOrEmpty(_payment.TradeDesc)) throw new ArgumentNullException(nameof(_payment.TradeDesc));
            if (string.IsNullOrEmpty(_payment.MerchantTradeDate)) throw new ArgumentNullException(nameof(_payment.MerchantTradeDate));
            if (string.IsNullOrEmpty(_payment.ChoosePayment)) throw new ArgumentNullException(nameof(_payment.ChoosePayment));
            if (string.IsNullOrEmpty(_payment.ItemName)) throw new ArgumentNullException(nameof(_payment.ItemName));
            if (!_payment.TotalAmount.HasValue) throw new ArgumentNullException(nameof(_payment.TotalAmount));
        }
    }
}