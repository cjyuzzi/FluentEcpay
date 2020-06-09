using System;
using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentReturnConfiguration : IPaymentReturnConfiguration
    {
        #region Private Fields
        private readonly PaymentConfiguration _configuration;
        private readonly Action<string> _setServerUrl;
        private readonly Action<string> _setClientUrl;
        private readonly Action<string> _setClientUrlWithExtraPaidInfo;
        #endregion

        #region CTOR
        public PaymentReturnConfiguration(
        PaymentConfiguration configuration,
        Action<string> setServerUrl,
        Action<string> setClientUrl,
        Action<string> setClientUrlWithExtraPaidInfo)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _setServerUrl = setServerUrl ?? throw new ArgumentNullException(nameof(setServerUrl));
            _setClientUrl = setClientUrl ?? throw new ArgumentNullException(nameof(setClientUrl));
            _setClientUrlWithExtraPaidInfo = setClientUrlWithExtraPaidInfo
                ?? throw new ArgumentNullException(nameof(setClientUrlWithExtraPaidInfo));
        }
        #endregion

        public IPaymentConfiguration ToClient(string url, bool needExtraPaidInfo = false)
        {
            if (url is null) throw new ArgumentNullException(nameof(url));

            if (needExtraPaidInfo)
                _setClientUrlWithExtraPaidInfo(url);

            else
                _setClientUrl(url);

            return _configuration;
        }

        public IPaymentConfiguration ToServer(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            _setServerUrl(url);

            return _configuration;
        }
    }
}