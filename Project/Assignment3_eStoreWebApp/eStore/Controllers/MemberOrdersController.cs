using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;

using DataAccess.Repository;
using BusinessObject;
using Newtonsoft.Json;

namespace eStore.Controllers
{
    public class MemberOrdersController : Controller
    {
        IOrderRepository orderRepository;
        IOrderDetailRepository orderDetailRepository;
        IProductRepository productRepository;
        static int id;
        public MemberOrdersController()
        {
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
        }
        [HttpGet]
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                MemberOrdersController.id = (int)id;
                return View(orderRepository.GetAllOfMember(id.Value));
            }
            else
            {
                return View(orderRepository.GetAllOfMember(MemberOrdersController.id));
            }
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            
            return View(orderRepository.GetOrderDetailsById(id.Value));
        }
    }
}
