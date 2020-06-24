using System;
using FluentEcpay.Interfaces;
using FluentEcpay.Configurations;

namespace FluentEcpay
{
    public class PaymentConfiguration : IPaymentConfiguration
    {
        #region Private Fields
        private readonly string _paymentType = "aio";
        private EHashAlgorithm _encryptType = EHashAlgorithm.SHA256;
        private string _url;
        private string _merchantId;
        private string _serverUrl;
        private string _clientUrl;
        private string _clientUrlWithExtraPaidInfo;
        private IPayment _payment;
        private ICheckMac _checkMac;
        private string _storeId;
        private bool _isPlatform;
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
                checkMac => _checkMac = checkMac,
                type => _encryptType = type,
                id => _storeId = id,
                isPlatform => _isPlatform = isPlatform
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
            if (_payment is null)
                throw new ArgumentNullException(nameof(_payment), "Transaction.New() must be set.");
            if (_checkMac is null)
                throw new ArgumentNullException(nameof(_checkMac), "Send.UsingHash() must be set.");

            // TODO: InvoiceMark, Language

            _payment.URL = _url;
            _payment.MerchantID = _merchantId;
            _payment.PaymentType = _paymentType;
            _payment.ReturnURL = _serverUrl;
            _payment.ClientBackURL = _clientUrl;
            _payment.EncryptType = Convert.ToInt32(_encryptType);
            _payment.StoreID = _storeId;
            _payment.OrderResultURL = _clientUrlWithExtraPaidInfo;
            _payment.NeedExtraPaidInfo = string.IsNullOrEmpty(_clientUrlWithExtraPaidInfo) ? null : "Y";
            _payment.PlatformID = _isPlatform ? _merchantId : null;
            _payment.CheckMacValue = _checkMac.GetValue(_payment, _encryptType);

            VerifyRequiredParameters(_payment);

            return _payment;
        }

        private void VerifyRequiredParameters(IPayment payment)
        {
            if (string.IsNullOrEmpty(payment.URL))
                throw new ArgumentNullException(nameof(payment.URL), "Send.ToApi(url:) must be set.");
            if (string.IsNullOrEmpty(payment.MerchantID))
                throw new ArgumentNullException(nameof(payment.MerchantID), "Send.ToMerchant(merchantId:) must be set.");
            if (string.IsNullOrEmpty(payment.MerchantTradeNo))
                throw new ArgumentNullException(nameof(payment.MerchantTradeNo), "Transaction.New(no:) must be set.");
            if (string.IsNullOrEmpty(payment.MerchantTradeDate))
                throw new ArgumentNullException(nameof(payment.MerchantTradeDate), "Transaction.New(date:) must be set.");
            if (!payment.TotalAmount.HasValue)
                throw new ArgumentNullException(nameof(payment.TotalAmount), "Transaction.WithItems(itmes:) must be set.");
            if (string.IsNullOrEmpty(payment.TradeDesc))
                throw new ArgumentNullException(nameof(payment.TradeDesc), "Transaction.New(description:) must be set.");
            if (string.IsNullOrEmpty(payment.ItemName))
                throw new ArgumentNullException(nameof(payment.ItemName), "Transaction.WithItems(itmes:) must be set.");
            if (string.IsNullOrEmpty(payment.ReturnURL))
                throw new ArgumentNullException(nameof(payment.ReturnURL), "Return.ToServer(url:) must be set.");
            if (string.IsNullOrEmpty(payment.ClientBackURL) && string.IsNullOrEmpty(payment.OrderResultURL))
                throw new ArgumentNullException($"{nameof(payment.ClientBackURL)} or {nameof(payment.OrderResultURL)}", "Return.ToClient(url:) must be set.");
            if (string.IsNullOrEmpty(payment.ChoosePayment))
                throw new ArgumentNullException(nameof(payment.ChoosePayment), "Transaction.UseMethod(method:) must be set.");
        }
    }
}