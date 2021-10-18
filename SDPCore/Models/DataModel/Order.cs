using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Order
    {
        public Order( int orderNo, ICustomer user, List<OrderLine> oLList, Delivery delivery, decimal netPrice, decimal taxRate, decimal totalPrice, bool paymentStatus, DateTime paymentDate, string orderStatus)
        {
            this.orderId = Guid.NewGuid();
            this.orderNo = orderNo;
            this.user = user;
            OLList = oLList;
            this.delivery = delivery;
            this.netPrice = netPrice;
            this.taxRate = taxRate;
            this.totalPrice = totalPrice;
            this.paymentStatus = paymentStatus;
            this.paymentDate = paymentDate;
            this.orderStatus = orderStatus;
        }

        public Order()
        {
        }

        public Guid orderId { get; set; }
        public int orderNo { get; set; }
        public ICustomer user { get; set; }
        public List<OrderLine> OLList { get; set; }
        public Delivery delivery { get; set; }
        public decimal netPrice { get; set; }
        public decimal taxRate { get; set; }
        public decimal totalPrice { get; set; }
        public bool paymentStatus { get; set; }
        public DateTime paymentDate { get; set; }
        public string orderStatus { get; set; }
    }
}
