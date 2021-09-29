using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Interface
{
    public interface IPayment
    {
        //registered customer and guest to be processed differently
        public abstract Task payment(/*paymentdetail place holder*/ string info);
    }
}
