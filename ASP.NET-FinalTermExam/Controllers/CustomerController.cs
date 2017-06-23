using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_FinalTermExam.Controllers
{
    public class CustomerController : Controller
    {
        Models.CustomerService CustomerService = new Models.CustomerService();
        // GET: Customer
        [HttpGet()]
        public ActionResult Index()
        {
            ViewBag.EmpCodeData = this.CustomerService.GetContactTitle();
            return View();
        }

        /// <summary>
        /// 取得客戶查詢結果
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Customer(Models.CustomerSearchArg arg)
        {
            ViewBag.EmpCodeData = this.CustomerService.GetContactTitle();
            Models.CustomerService CustomerService = new Models.CustomerService();
            return Json(CustomerService.GetCustomerByCondtioin(arg));
        }

        /// <summary>
        /// 刪除訂單功能
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>json true or false</returns>
        [HttpPost]
        public JsonResult DeleteCustomer(string CustomerID)
        {
            try
            {
                Models.CustomerService CustomerService = new Models.CustomerService();
                CustomerService.DeleteCustomerById(CustomerID);
                return this.Json(true);//成功回傳TRUE
            }
            catch (Exception)
            {

                return this.Json(false);//失敗回傳FALSE
            }

        }

        /// <summary>
        /// 新增顧客頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertCustomer()
        {
            ViewBag.Emp = this.CustomerService.GetContactTitle();
            Models.Customer Customer = new Models.Customer();
            return View(Customer);
        }

        /// <summary>
        /// 新增客戶功能
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertCustomer(Models.Customer Customer)
        {
            Models.CustomerService orderService = new Models.CustomerService();
            int CustomerID = orderService.InsertCustomer(Customer);

            //檢查是否驗證成功
            if (ModelState.IsValid)
            {
                TempData["ok"] = "成功新增訂單";
                return RedirectToAction("Index", new { CustomerID = CustomerID });

            }


            return View("InsertOrder", Customer);

        }

        /// <summary>
        /// 更新訂單頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateCustomer(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Emp = this.CustomerService.GetContactTitle();
            Models.Customer model = CustomerService.GetCustomerById(id);
            model.CustomerID = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Models.Customer Customer)
        {
            Models.CustomerService orderService = new Models.CustomerService();


            //檢查是否驗證成功
            if (ModelState.IsValid)
            {
                CustomerService.UpdateOrder(Customer);
                TempData["ok"] = "成功更新訂單";
                return RedirectToAction("Index");

            }
            ViewBag.Emp = this.CustomerService.GetContactTitle();
            return View("UpdateOrder", Customer);
        }



    }
}