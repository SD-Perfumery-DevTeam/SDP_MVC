using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Interfaces
{
    public interface IPayment
    {
        //registered customer and guest to be processed differently
        public abstract Task payment(/*paymentdetail place holder*/ string info);
    }
}
