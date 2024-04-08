using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.Api;
using PaypalCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using TechStore.ViewUser.ExtenstionMethods;
using TechStore.Dtos.ProductDtos;
using TechStore.Application.Services;
using System.Security.Claims;
using TechStore.Dtos.OrderDtos;

namespace PaypalCore.Controllers
{
    public class PayPalController : Controller
    {
        private readonly ILogger<PayPalController> _logger;
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration _configuration;
        private readonly IOrderService _orderService;

        public PayPalController(ILogger<PayPalController> logger, IHttpContextAccessor context, IConfiguration iconfiguration, IOrderService orderService)
        {
            _logger = logger;
            httpContextAccessor = context;
            _configuration = iconfiguration;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PaymentWithPaypal(OrderDto order,string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            //getting the apiContext  
            var ClientID = _configuration.GetValue<string>("PayPal:Key");
            var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            var mode = _configuration.GetValue<string>("PayPal:mode");
            APIContext apiContext = PaypalConfigurationDto.GetAPIContext(ClientID, ClientSecret, mode);
            try
            {
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/PayPal/PaymentWithPayPal?";
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);
                    var links = createdPayment.links.GetEnumerator();
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    decimal TotalPrice = 0;
                    var orderItems = new List<OrderItemDto>();
                    foreach (var item in cart)
                    {
                        var orderItem = new OrderItemDto { ProductId = item.ProductId, Quantity = item.Quantity };
                        TotalPrice += (item.Price * item.Quantity);
                        orderItems.Add(orderItem);
                    }

                    var orderdto = new OrderDto
                    {
                        UserId = order.UserId,
                        ShippingAddress = order.ShippingAddress,
                        Phone = order.Phone,
                        //ShippingMethod = createdPayment.ShippingMethod,
                        
                        OrderStatus = "Pending",
                        OrderItems = orderItems,
                        TotalPrice = TotalPrice,
                    };
                    var orders = await _orderService.CreateOrderAsync(orderdto);

                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId);//as string
                    if (executedPayment.state.ToLower() != "approved")
                    {

                        return View("PaymentFailed");
                    }
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;


                    return View("PaymentSuccess");
                }
            }
            catch (Exception ex)
            {
                return View("PaymentFailed");
            }
            return View("SuccessView");
        }


        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();


            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var item in cart)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Description,
                    currency = "USD",
                    price = item.Price.ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = "asd"
                });
            }


            var payer = new Payer()
            {
                payment_method = "paypal"
            };


            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            decimal subtotal = cart.Sum(item => item.Price * item.Quantity);

            decimal tax = subtotal * 0.07m;
            decimal shipping = 5.00m;

            var details = new Details()
            {
                tax = tax.ToString("0.00"),
                shipping = shipping.ToString("0.00"),
                subtotal = subtotal.ToString("0.00")
            };

            decimal total = subtotal + tax + shipping;

            var amount = new Amount()
            {
                currency = "USD",
                total = total.ToString("0.00"),
                details = details
            };


            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList,

            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
