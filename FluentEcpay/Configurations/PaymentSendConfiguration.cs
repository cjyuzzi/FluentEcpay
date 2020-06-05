using System;
using FluentEcpay.Interfaces;
using FluentEcpay.Enums;

namespace FluentEcpay.Configurations
{
    public class PaymentSendConfiguration : IPaymentSendConfiguration
    {
        private readonly PaymentConfiguration _configuration;
        private readonly Action<string> _setUrl;
        private readonly Action<string> _setMerchantId;
        private readonly Action<string> _setHashKey;
        private readonly Action<string> _setHashIV;
        private readonly Action<int> _setEncryptType;

        public PaymentSendConfiguration(
            PaymentConfiguration configuration,
            Action<string> setUrl,
            Action<string> setMerchantId,
            Action<string> setHashKey,
            Action<string> setHashIV,
            Action<int> setEncryptType)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _setUrl = setUrl ?? throw new ArgumentNullException(nameof(setUrl));
            _setMerchantId = setMerchantId ?? throw new ArgumentNullException(nameof(setMerchantId));
            _setHashKey = setHashKey ?? throw new ArgumentNullException(nameof(setHashKey));
            _setHashIV = setHashIV ?? throw new ArgumentNullException(nameof(setHashIV));
            _setEncryptType = setEncryptType ?? throw new ArgumentNullException(nameof(setEncryptType));
        }

        public IPaymentConfiguration ToApi(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            _setUrl(url);
            return _configuration;
        }

        // TODO: StoreID, PlatformID
        public IPaymentConfiguration ToMerchant(string merChantId, string storeId = null, bool isPlatform = false)
        {
            if (string.IsNullOrEmpty(merChantId)) throw new ArgumentNullException(nameof(merChantId));
            _setMerchantId(merChantId);
            return _configuration;
        }

        public IPaymentConfiguration UsingHash(string key, string iv, HashAlgorithm? algorithm = HashAlgorithm.SHA256)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(iv)) throw new ArgumentNullException(nameof(iv));

            if (algorithm.HasValue)
            {
                switch (algorithm.Value)
                {
                    case HashAlgorithm.SHA256:
                        _setEncryptType(1);
                        break;
                }
            }
            else throw new ArgumentNullException(nameof(algorithm));

            _setHashKey(key);
            _setHashIV(iv);

            return _configuration;
        }
    }
}