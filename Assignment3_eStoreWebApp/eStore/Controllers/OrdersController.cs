using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;

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

        
        public OrdersController()
        {
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
            productRepository = new ProductRepository();
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
                if(HttpContext.Session.GetString("Cart") == null)
                {
                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(new List<CartItem>()));
                }

                List<CartItem> cart = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("Cart"));

                var product = productRepository.GetProductById(productId);
                var productInCart = cart.Find(cartItem => cartItem.ProductId == productId);
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
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind(nameof(Order.MemberId), nameof(Order.OrderDate), nameof(Order.RequiredDate), nameof(Order.ShippedDate), nameof(Order.Freight))] Order order, string action)
        {  
            if(!TryValidateModel(nameof(Order)))
            {
                return View();
            }    
            if (order != null && action.Equals("Create"))
            {
                var orderAfterAdd = orderRepository.Add(order);
                if(HttpContext.Session.GetString("Cart") == null)
                {
                    return RedirectToAction("Index");
                }    
                List<CartItem> cart = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("Cart"));
                foreach(CartItem pr in cart)
                {
                    orderDetailRepository.Add(new OrderDetail
                    {
                        OrderId = orderAfterAdd.OrderId,
                        ProductId = pr.ProductId,
                        Quantity = pr.Quantity,
                        Discount = pr.Discount,
                        UnitPrice = pr.UnitPrice
                    });
                }
            } else if(action.Equals("Add Product"))
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
        }

        public ActionResult Update(int id)
        {
            return View(orderRepository.GetById(id));
        }
        [HttpPost]
        public ActionResult Update(int id, [Bind(nameof(Order.MemberId), nameof(Order.OrderDate), nameof(Order.RequiredDate), nameof(Order.ShippedDate), nameof(Order.Freight))] Order order)
        {
            if(TryValidateModel(nameof(Order)))
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
        public ActionResult Delete(int id)
        {
            var order = orderRepository.GetById(id);
            orderDetailRepository.Delete(order);
            return null;
        }
        [HttpPost]
        public ActionResult Delete(int id, [Bind(nameof(Order.MemberId), nameof(Order.OrderDate), nameof(Order.RequiredDate), nameof(Order.ShippedDate), nameof(Order.Freight))] Order order)
        {
            if (TryValidateModel(nameof(Order)))
            {
                var orderDelete = orderRepository.GetById(id);
                orderRepository.Delete(orderDelete);
                return RedirectToAction("Index");
            }
            return View(orderRepository.GetById(id));
        }
    }
}
