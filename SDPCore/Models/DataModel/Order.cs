using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.SDP.SDPCore.Consts;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Order
    {
        public Order( int orderNo, IdentityUser user,  Delivery delivery, decimal netPrice, decimal taxRate, decimal totalPrice, bool paymentStatus, DateTime paymentDate, OrderStatus orderStatus)
        {
            this.orderId = Guid.NewGuid();
            this.orderNo = orderNo;
            this.user = user;
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
        public IdentityUser user { get; set; }
        public Delivery delivery { get; set; }
        public decimal netPrice { get; set; }
        public decimal taxRate { get; set; }
        public decimal totalPrice { get; set; }
        public bool paymentStatus { get; set; }
        public DateTime paymentDate { get; set; }
        public OrderStatus orderStatus { get; set; }
    }
}
