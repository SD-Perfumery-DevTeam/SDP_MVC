using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Interface
{
    public interface IPromotionService
    {
        public string GetPromoProductId(string promoCode);
        public bool validatePromo(string promoCode);
    }
}
