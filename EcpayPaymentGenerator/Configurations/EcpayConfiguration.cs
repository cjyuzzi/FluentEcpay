using System;
using System.Collections.Generic;
using EcpayPaymentGenerator.Arguments;
using EcpayPaymentGenerator.Models;
using EcpayPaymentGenerator.Enums;
using EcpayPaymentGenerator.Extensions;

namespace EcpayPaymentGenerator.Configurations
{
    public class EcpayConfiguration
    {
        private ReturnArgs _returnArgs;
        private ServiceArgs _serviceArgs;

        public EcpayServiceConfiguration SendTo { get; internal set; }
        public EcpayReturnConfiguration ReturnTo { get; internal set; }
        public EcpaySettingsConfiguration ReadFrom { get; internal set; }   // TODO: ASP.NET Core Configuration

        public EcpayConfiguration()
        {
            _returnArgs = new ReturnArgs();
            SendTo = new EcpayServiceConfiguration(this, args => _serviceArgs = args);
            ReturnTo = new EcpayReturnConfiguration(this,
                returnUrl => _returnArgs.ReturnURL = returnUrl,
                clientBackUrl => _returnArgs.ClientBackURL = clientBackUrl,
                orderResultUrl => _returnArgs.OrderResultURL = orderResultUrl);
            ReadFrom = new EcpaySettingsConfiguration(this);
        }

        public Payment CreatePayment(string tradeTitle, string description, IEnumerable<Item> items, string itemUrl, string remark = null, IgnorePayment ignorePayment = IgnorePayment.Default)
        {
            var now = DateTime.Now;
            var tradeNo = ValidateTradeTitle(tradeTitle) ? GenerateTradeNo(tradeTitle, now) : throw new ArgumentOutOfRangeException(nameof(tradeTitle));

            var payment = new Payment
            {
                URL = _serviceArgs.URL,
                MerchantID = _serviceArgs.MerchantID,
                MerchantTradeNo = tradeNo,
                MerchantTradeDate = now.ToString("yyyy/MM/dd HH:mm:ss"),
                CheckMacValue = "", // TODO:
                ReturnURL = _returnArgs.ReturnURL,
                ClientBackURL = _returnArgs.ClientBackURL,
                TradeDesc = description,
                ItemURL = itemUrl,
                Remark = remark,
                ItemName = items.ConvertToItemName(),
                TotalAmount = items.TotalAmount(),
                IgnorePayment = ignorePayment.ToString().Replace(", ", "#"),
                ChoosePayment = "ALL",
                PaymentType = "aio",
                EncryptType = 1
            };

            return payment;
        }
        private bool ValidateTradeTitle(string tradeTitle) => !(string.IsNullOrEmpty(tradeTitle) || string.IsNullOrWhiteSpace(tradeTitle) || tradeTitle.Length > 5);
        private string GenerateTradeNo(string tradeTitle, DateTime now) => (tradeTitle + now.ToString("MMddHHmmssfff")).PadRight(20, '0');
    }

    public class EcpayServiceConfiguration
    {
        private readonly EcpayConfiguration _ecpayConfiguration;
        private readonly Action<ServiceArgs> _setArgs;
        public EcpayServiceConfiguration(EcpayConfiguration configuration, Action<ServiceArgs> setArgs)
        {
            _ecpayConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _setArgs = setArgs ?? throw new ArgumentNullException(nameof(setArgs));
        }

        public EcpayConfiguration Api(string url, string merchantID, string hashKey, string hashIV)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException(nameof(url));
            if (string.IsNullOrEmpty(merchantID)) throw new ArgumentException(nameof(merchantID));
            if (string.IsNullOrEmpty(hashKey)) throw new ArgumentException(nameof(hashKey));
            if (string.IsNullOrEmpty(hashIV)) throw new ArgumentException(nameof(hashIV));

            _setArgs(new ServiceArgs
            {
                URL = url,
                MerchantID = merchantID,
                HashKey = hashKey,
                HashIV = hashIV
            });

            return _ecpayConfiguration;
        }
        public EcpayConfiguration Api(ServiceArgs args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));

            _setArgs(args);

            return _ecpayConfiguration;
        }
    }
    public class EcpayReturnConfiguration
    {
        private readonly EcpayConfiguration _ecpayConfiguration;
        private readonly Action<string> _setReturnUrl;
        private readonly Action<string> _setClientBackUrl;
        private readonly Action<string> _setOrderResultUrl;

        public EcpayReturnConfiguration(EcpayConfiguration configuration,
            Action<string> setReturnUrl,
            Action<string> setClientBackUrl,
            Action<string> setOrderResultUrl
        )
        {
            _ecpayConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _setReturnUrl = setReturnUrl ?? throw new ArgumentNullException(nameof(setReturnUrl));
            _setClientBackUrl = setClientBackUrl ?? throw new ArgumentNullException(nameof(setClientBackUrl));
            _setOrderResultUrl = setOrderResultUrl ?? throw new ArgumentNullException(nameof(setOrderResultUrl));
        }

        public EcpayConfiguration Action(Uri uri) => Action(uri.ToString());
        public EcpayConfiguration Action(string uri)
        {
            if (string.IsNullOrEmpty(uri)) throw new ArgumentNullException(nameof(uri));

            _setReturnUrl(uri);

            return _ecpayConfiguration;
        }
        public EcpayConfiguration Client(Uri uri, bool needExtraPaidInfo = false) => Client(uri.ToString(), needExtraPaidInfo);
        public EcpayConfiguration Client(string uri, bool needExtraPaidInfo = false)
        {
            if (string.IsNullOrEmpty(uri)) throw new ArgumentException(nameof(uri));

            if (needExtraPaidInfo)
            {
                _setClientBackUrl(string.Empty);
                _setOrderResultUrl(uri);
            }
            else
            {
                _setOrderResultUrl(string.Empty);
                _setClientBackUrl(uri);
            }

            return _ecpayConfiguration;
        }
    }
    public class EcpaySettingsConfiguration
    {
        private readonly EcpayConfiguration _ecpayConfiguration;
        public EcpaySettingsConfiguration(EcpayConfiguration configuration)
        {
            _ecpayConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}