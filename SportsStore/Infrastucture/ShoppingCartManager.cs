using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Infrastucture
{
    public class ShoppingCartManager
    {
        private Entities db;
        private ISessionManager session;
        public const string CartSessionKey = "CartData";
        public ShoppingCartManager(ISessionManager session, Entities db)
        {
            this.session = session;
            this.db = db;
        }

        public void AddToCart(int productId)
        {
            var cart = this.GetCart();
            var cartItem = cart.Find(c => c.Product.Id == productId);

            if(cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                var productToAdd = db.Products.Where(p => p.Id == productId).SingleOrDefault();
                if(productToAdd != null)   
                {
                    var newCartItem = new CartItem()
                    {
                        Product = productToAdd,
                        Quantity = 1,
                        TotalPrice = productToAdd.Price
                    };

                    cart.Add(newCartItem);
                }
            }

            session.Set(CartSessionKey, cart);
        }

        public List<CartItem> GetCart()
        {
            List<CartItem> cart;
            if(session.Get<List<CartItem>>(CartSessionKey) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.Get<List<CartItem>>(CartSessionKey) as List<CartItem>;
            }
            return cart;
        }

        public int RemoveFromCart(int productId)
        {
            var cart = this.GetCart();
            var cartItem = cart.Find(c => c.Product.Id ==productId);
            if(cartItem != null)
            {
                if(cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    return cartItem.Quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }

            return 0;
        }

        public decimal GetCartTotalPrice()
        {
            var cart = this.GetCart();
            return cart.Sum(c => c.Quantity * c.Product.Price);
        }

        public int GetCartItemCount()
        {
            var cart = this.GetCart();
            var count = cart.Sum(c => c.Quantity);
            return count;
        }

        public Order CreateOrder(Order newOrder, string userId)
        {
            var cart = this.GetCart();
            newOrder.AspNetUser.Id = userId;
            this.db.Orders.Add(newOrder);

            if(newOrder.Order_Product == null)
            {
                newOrder.Order_Product = new List<Order_Product>();
            }

            decimal cartTotal = 0;
            foreach(var cartItem in cart)
            {
                var newOrderItem = new Order_Product()
                {
                    Amount = cartItem.Quantity,
                    ProductId = cartItem.Product.Id
                    
                    
                };

                cartTotal += (cartItem.Quantity * cartItem.TotalPrice);
                newOrder.Order_Product.Add(newOrderItem);
            }
            //newOrder. = cartTotal;
            this.db.SaveChanges();
            return newOrder;
        }

        public void EmptyCart()
        {
            session.Set<List<CartItem>>(CartSessionKey, null);
        }
    }
}