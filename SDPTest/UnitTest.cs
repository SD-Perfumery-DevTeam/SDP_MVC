using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPInfrastructure.Services;
using System;
using Xunit;

namespace SDPTest
{
    public class UnitTest
    {
        Cart cart = new Cart();
       
         [Fact]
        public void TestGetCartList()
        {
            cart.cartList.Add("aea1a240-f92f-4b4e-8bfb-003c0beaa16c", new Cart.CartValuePair { discount = 0, quantity = 1 });
            Assert.Equal(cart.getCartList(), cart.cartList);
        }
        [Fact]
        public void TestveProductToCart()
        {
            cart.cartList.Add("aea1a240-f92f-4b4e-8bfb-003c0beaa16c", new Cart.CartValuePair { discount = 0, quantity = 1 });
            Assert.Equal("deleted" , cart.removeProductToCart("aea1a240-f92f-4b4e-8bfb-003c0beaa16c"));
        }
    }
}
