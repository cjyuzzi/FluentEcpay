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
        private CheckMac _checkMac;
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
            _checkMac = new CheckMac();
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
            _payment.CheckMacValue = _checkMac.GetValue(_payment, _hashKey, _hashIV, _encryptType.Value);

            return _payment;
        }

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