using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

using DataAccess.Repository;
using BusinessObject;
using eStore.Filters;


namespace eStore.Controllers
{
    [AdminOnlyFilter]
    public class OrdersController : Controller
    {
        IOrderRepository orderRepository;
        IOrderDetailRepository orderDetailRepository;
        IProductRepository productRepository;
        IMemberRepository memberRepository;
        
        public OrdersController()
        {
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
            productRepository = new ProductRepository();
            memberRepository = new MemberRepository();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(orderRepository.GetAll());
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            return View(orderRepository.GetById(id.Value));
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View(productRepository.GetAllProducts());
        }
        [HttpPost]
        public IActionResult AddToCart(int quantity, int productId, string discount)
        {
            try
            {
                if(GetCart() == null)
                {
                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(new List<CartItem>()));
                }

                List<CartItem> cart = GetCart();

                var product = productRepository.GetProductById(productId);
                var productInCart = cart.Find(cartItem => cartItem.ProductId == productId);
                if(product.UnitsInStock < quantity)
                {
                    throw new Exception("The unit in stock of product is less than quantity");
                }    

                if (productInCart == null)
                {
                    cart.Add(new CartItem
                    {
                        Quantity = quantity,
                        CategoryId = product.CategoryId,
                        ProductId = product.ProductId,
                        UnitPrice = product.UnitPrice,
                        ProductName = product.ProductName,
                        Weight = product.Weight,
                        Discount = double.Parse(discount)
                    });
                }
                else
                {
                    productInCart.Quantity += quantity;
                    productInCart.Discount = double.Parse(discount);
                }
                product.UnitsInStock -= quantity;
                productRepository.UpdateProduct(product);
                TempData["message"] = $"Added {product.ProductName} - Quantity: {quantity} successfully";
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            } catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return RedirectToAction(nameof(AddProduct));
        }
        [HttpGet]
        public IActionResult Create()
        {
            var allMembers = memberRepository.GetAllMembers().Select(member => new MemberDTO() { Id = member.MemberId, Name = member.Email }).ToList();
            TempData["allMembers"] = JsonSerializer.Serialize(allMembers);
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind(nameof(Order.MemberId), nameof(Order.OrderDate), nameof(Order.RequiredDate), nameof(Order.ShippedDate), nameof(Order.Freight))] Order order, string action)
        {
            if (!TryValidateModel(nameof(Order)))
            {
                return View();
            }
            try
            {
                if (order != null && action.Equals("Create"))
                {
                    var orderAfterAdd = orderRepository.Add(order);
                    TempData.Clear();
                    List<CartItem> cart = GetCart();

                    if (cart == null)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (CartItem pr in cart)
                    {
                        HttpContext.Session.Remove("Cart");
                        orderDetailRepository.Add(new OrderDetail
                        {
                            OrderId = orderAfterAdd.OrderId,
                            ProductId = pr.ProductId,
                            Quantity = pr.Quantity,
                            Discount = pr.Discount,
                            UnitPrice = pr.UnitPrice
                        });

                    }
                    
                }
                else if (action.Equals("Add Product"))
                {
                    if (order != null)
                    {
                        TempData["memberId"] = order.MemberId;
                        TempData["orderDate"] = order.OrderDate == null ? null : order.OrderDate.Value.ToString("yyyy-MM-dd");
                        TempData["requiredDate"] = order.RequiredDate?.ToString("yyyy-MM-dd");
                        TempData["shippedDate"] = order.ShippedDate == null ? null : order.ShippedDate.Value.ToString("yyyy-MM-dd");
                        TempData["freight"] = order.Freight.ToString();
                    }

                    return RedirectToAction("AddProduct");
                }
                return RedirectToAction("Index");
            } catch (Exception ex)
            {
                TempData["message"] = $"Error: {ex.Message}";
                return RedirectToAction("Create");
            }
            
        }

        public ActionResult Update(int id)
        {
            var order = orderRepository.GetById(id);
            var listOrderDetails = orderRepository.GetOrderDetailsById(id);
            order.OrderDetails = (ICollection<OrderDetail>) listOrderDetails;
            return View(orderRepository.GetById(id));
        }
        [HttpPost]
        public ActionResult Update(int id, [Bind(nameof(Order.MemberId), nameof(Order.OrderDate), nameof(Order.RequiredDate), nameof(Order.ShippedDate), nameof(Order.Freight))] Order order)
        {
            try
            {
                if (TryValidateModel(nameof(Order)))
                {
                    var orderUpdate = orderRepository.GetById(id);
                    orderUpdate.MemberId = order.MemberId;
                    orderUpdate.OrderDate = order.OrderDate;
                    orderUpdate.RequiredDate = order.RequiredDate;
                    orderUpdate.ShippedDate = order.ShippedDate;
                    orderUpdate.Freight = order.Freight;
                    orderRepository.Update(orderUpdate);
                    return RedirectToAction("Index");
                }
                return View(orderRepository.GetById(id));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("Update", routeValues: new { Id = id });
            }
            
        }
        public IActionResult Delete(int id)
        {
            var order = orderRepository.GetById(id);
            var listOrderDetails = orderRepository.GetOrderDetailsById(id);
            order.OrderDetails = (ICollection<OrderDetail>)listOrderDetails;
            return View(orderRepository.GetById(id));
        }
        [HttpPost]
        public ActionResult Delete(int id, [Bind(nameof(Order.MemberId), nameof(Order.OrderDate), nameof(Order.RequiredDate), nameof(Order.ShippedDate), nameof(Order.Freight))] Order order)
        {
            try
            {
                if (TryValidateModel(nameof(Order)))
                {
                    //orderRepository = new OrderRepository();
                    var orderDelete = orderRepository.GetById(id);
                    orderRepository.Delete(orderDelete);
                    return RedirectToAction("Index");
                }
                return View(orderRepository.GetById(id));
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("Delete", routeValues: new { Id = id });
            }
        }
        public IActionResult DeleteProduct(int id)
        {
            List<CartItem> cart = GetCart();
            var cartItem = cart.Where(item => item.ProductId == id).FirstOrDefault();
            if(cartItem != null)
            {
                RemoveCartItemAndReserveQuantity(cartItem, cart);
            }
            return RedirectToAction("Create");
        }
        private void RemoveCartItemAndReserveQuantity(CartItem cartItem, List<CartItem> cart)
        {
            ReserveQuantityAfterDeleteItem(cartItem);
            cart.Remove(cartItem);
            SetCart(cart);
        }
        private void ReserveQuantityAfterDeleteItem(CartItem cartItem)
        {
            var product = productRepository.GetProductById(cartItem.ProductId);
            product.UnitsInStock += cartItem.Quantity;
            productRepository.UpdateProduct(product);
        }
        public IActionResult ClearProduct()
        {
            var cart = GetCart();
            if(cart != null)
            {
                foreach(CartItem item in cart)
                {
                    ReserveQuantityAfterDeleteItem(item);
                }    
            }
            SetCart(new List<CartItem>());
            return RedirectToAction("Create");
        }
        private List<CartItem> GetCart()
        {
            string get = HttpContext.Session.GetString("Cart");
            if (get == null) return null;
            return JsonSerializer.Deserialize<List<CartItem>>(get);
        }
        private void SetCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }
    }
}
