using System;
using EcpayPaymentGenerator.Arguments;
using EcpayPaymentGenerator.ViewModels;

namespace EcpayPaymentGenerator.Configurations
{
    public class EcpayConfiguration
    {
        private readonly PaymentViewModel _payment = new PaymentViewModel();
        private readonly ReturnArgs _returnArgs = new ReturnArgs();
        private ServiceArgs _serviceArgs;

        public EcpayServiceConfiguration SendTo { get; internal set; }
        public EcpayReturnConfiguration ReturnTo { get; internal set; }
        public EcpaySettingsConfiguration ReadFrom { get; internal set; }   // TODO: ASP.NET Core Configuration

        public EcpayConfiguration()
        {
            SendTo = new EcpayServiceConfiguration(this, args => _serviceArgs = args);
            ReturnTo = new EcpayReturnConfiguration(this,
                returnUrl => _returnArgs.ReturnURL = returnUrl,
                clientBackUrl => _returnArgs.ClientBackURL = clientBackUrl,
                orderResultUrl => _returnArgs.OrderResultURL = orderResultUrl);
            ReadFrom = new EcpaySettingsConfiguration(this);
        }

        public PaymentViewModel CreatePayment(string description, string items, string ClientBackURL, string remark, string ignorePayment)
        {
            throw new NotImplementedException("Not Implemented!");
        }
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