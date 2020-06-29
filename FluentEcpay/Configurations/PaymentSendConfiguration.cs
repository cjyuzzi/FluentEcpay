using System;
using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentSendConfiguration : IPaymentSendConfiguration
    {

        #region Private Fields
        private readonly PaymentConfiguration _configuration;
        private readonly Action<string> _setUrl;
        private readonly Action<string> _setMerchantId;
        private readonly Action<ICheckMac> _setCheckMac;
        private readonly Action<EHashAlgorithm> _setEncryptType;
        private readonly Action<string> _setStoreId;
        private readonly Action<bool> _setIsPlatform;
        #endregion

        #region CTOR
        public PaymentSendConfiguration(
            PaymentConfiguration configuration,
            Action<string> setUrl,
            Action<string> setMerchantId,
            Action<ICheckMac> setCheckMac,
            Action<EHashAlgorithm> setEncryptType,
            Action<string> setStoreId,
            Action<bool> setIsPlatform)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _setUrl = setUrl ?? throw new ArgumentNullException(nameof(setUrl));
            _setMerchantId = setMerchantId ?? throw new ArgumentNullException(nameof(setMerchantId));
            _setCheckMac = setCheckMac ?? throw new ArgumentNullException(nameof(setCheckMac));
            _setEncryptType = setEncryptType ?? throw new ArgumentNullException(nameof(setEncryptType));
            _setStoreId = setStoreId ?? throw new ArgumentNullException(nameof(setStoreId));
            _setIsPlatform = setIsPlatform ?? throw new ArgumentNullException(nameof(setIsPlatform));
        }
        #endregion

        public IPaymentConfiguration ToApi(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            _setUrl(url);
            return _configuration;
        }

        public IPaymentConfiguration ToMerchant(string merchantId, string storeId = null, bool isPlatform = false)
        {
            if (string.IsNullOrEmpty(merchantId)) throw new ArgumentNullException(nameof(merchantId));

            _setMerchantId(merchantId);
            if (storeId != null) _setStoreId(storeId);
            _setIsPlatform(isPlatform);

            return _configuration;
        }

        public IPaymentConfiguration UsingHash(string key, string iv, EHashAlgorithm? algorithm = EHashAlgorithm.SHA256)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(iv)) throw new ArgumentNullException(nameof(iv));

            if (algorithm.HasValue)
            {
                _setEncryptType(algorithm.Value);
            }
            else throw new ArgumentNullException(nameof(algorithm));

            _setCheckMac(new CheckMac(key,iv));

            return _configuration;
        }
    }
}