using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_FinalTermExam.Models
{
    public class Customer
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        [DisplayName("客戶ID")]
        public string CustomerID { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        [DisplayName("公司名稱")]
        [Required()]
        public string CompanyName { get; set; }

        /// <summary>
        /// 聯絡人姓名
        /// </summary>
        [DisplayName("聯絡人姓名")]
        public string ContactName { get; set; }

        /// <summary>
        /// 聯絡人職稱
        /// </summary>
        [Required()]
        [DisplayName("公司名稱")]
        public string ContactTitle { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Required()]
        [DisplayName("建立日期")]
        public string CreationDate { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DisplayName("地址")]
        [Required()]
        public string Address { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Required()]
        [DisplayName("城市")]
        public string City { get; set; }

        /// <summary>
        /// 區域
        /// </summary>
        [DisplayName("區域")]
        public string Region { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        [DisplayName("郵遞區號")]
        public string PostalCode { get; set; }

        /// <summary>
        /// 國家
        /// </summary>
        [Required()]
        [DisplayName("國家")]
        public string Country { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        [Required()]
        [DisplayName("電話")]
        public string Phone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        [DisplayName("傳真")]
        public string Fax { get; set; }


    }
}