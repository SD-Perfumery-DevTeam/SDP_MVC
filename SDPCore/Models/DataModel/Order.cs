using Microsoft.AspNetCore.Identity;
using Microsoft.SDP.SDPCore.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.SDP.SDPCore.Consts;

namespace Microsoft.SDP.SDPCore.Models
{
    public class Order
    {
        [Required]
        public Guid orderId { get; set; }
        [Required]
        public int orderNo { get; set; }
        public IdentityUser user { get; set; }
        public Delivery delivery { get; set; }
        public List <OrderLine> orderLine { get; set; }
        [Required]
        public decimal totalPrice { get; set; }
        [Required]
        public string paymentStatus { get; set; }
        public DateTime paymentDate { get; set; }
        [Required]
        public OrderStatus orderStatus { get; set; }

        public Order()
        {
        }

        public Order(int orderNo, IdentityUser user, Delivery delivery, decimal totalPrice, string paymentStatus, DateTime paymentDate, OrderStatus orderStatus)
        {
            this.orderId = Guid.NewGuid();
            this.orderNo = orderNo;
            this.user = user;
            this.delivery = delivery;
            this.totalPrice = totalPrice;
            this.paymentStatus = paymentStatus;
            this.paymentDate = paymentDate;
            this.orderStatus = orderStatus;
        }
    }
}
