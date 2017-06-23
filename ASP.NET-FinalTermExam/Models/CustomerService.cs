using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ASP.NET_FinalTermExam.Models
{
    public class CustomerService
    {

        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 取得聯絡人職稱
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetContactTitle()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT CodeType, CodeId ,CodeVal AS CodeName FROM dbo.CodeTable Where CodeType = 'TITLE';";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);

        }

        private List<SelectListItem> MapCodeData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();


            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CodeName"].ToString(),
                    Value = row["CodeId"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Customer> GetCustomerByCondtioin(Models.CustomerSearchArg arg)
        {

            DataTable dt = new DataTable();
            string sql = @"select * FROM Sales.Customers WHERE (CustomerID Like '%' + @CustomerID + '%' Or CustomerID='' ) And  (Companyname Like '%' + @CustName + '%' Or @CustName='') And 
                            (ContactName Like '%' + @ContactName + '%' Or @ContactName='') And　( ContactTitle=@ContactTitle Or @ContactTitle='')";



            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(new SqlParameter("@CustomerID", arg.CustomerID == null ? string.Empty : arg.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@CustName", arg.CompanyName == null ? string.Empty : arg.CompanyName));
                cmd.Parameters.Add(new SqlParameter("@ContactName", arg.ContactName == null ? string.Empty : arg.ContactName));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", arg.ContactTitle == null ? string.Empty : arg.ContactTitle));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapCustomerDataToList(dt);
        }

        private List<Models.Customer> MapCustomerDataToList(DataTable orderData)
        {
            List<Models.Customer> result = new List<Customer>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Customer()
                {
                    CustomerID = row["CustomerID"].ToString(),
                    CompanyName = row["CompanyName"].ToString(),
                    ContactName = row["ContactName"].ToString(),
                    ContactTitle = row["ContactTitle"].ToString(),
                    CreationDate = row["CreationDate"] == DBNull.Value ? null : String.Format("{0:M/d/yyyy HH:mm:ss}", (DateTime)row["CreationDate"]),
                    Address = row["Address"].ToString(),
                    City = row["City"].ToString(),
                    Region = row["Region"].ToString(),
                    PostalCode = row["PostalCode"].ToString(),
                    Country = row["Country"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Fax = row["Fax"].ToString(),
                });
            }

            return result;
        }

        /// <summary>
		/// 刪除訂單
		/// </summary>
		public void DeleteCustomerById(string CustomerID)
        {
            try
            {
                string sql = "Delete FROM Sales.Customers Where CustomerID=@CustomerID";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
                    cmd.ExecuteNonQuery();///回傳0,-1
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增訂單處理
        /// </summary>
        /// <param name=""></param>
        /// <returns>訂單編號</returns>
        public int InsertCustomer(Models.Customer Customer)
        {

            string sql = @"Insert INTO Sales.Customers
						 (
							CompanyName,ContactName,ContactTitle,CreationDate,Address,City,Region,PostalCode,Country,Phone,Fax
						)
						VALUES
						(
							@CompanyName,@ContactName,@ContactTitle,@CreationDate,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax
						)
					    select @@IDENTITY
						";
            int CustomerID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))

            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction("InsertOrder");
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", Customer.CompanyName == null ? string.Empty : Customer.CompanyName));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", Customer.ContactName == null ? string.Empty : Customer.ContactName));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", Customer.ContactTitle == null ? string.Empty : Customer.ContactTitle));
                        cmd.Parameters.Add(new SqlParameter("@CreationDate", Customer.CreationDate == null ? string.Empty : Customer.CreationDate));
                        cmd.Parameters.Add(new SqlParameter("@Address", Customer.Address == null ? string.Empty : Customer.Address));
                        cmd.Parameters.Add(new SqlParameter("@City", Customer.City == null ? string.Empty : Customer.City));
                        cmd.Parameters.Add(new SqlParameter("@Region", Customer.Region == null ? string.Empty : Customer.Region));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", Customer.PostalCode == null ? string.Empty : Customer.PostalCode));
                        cmd.Parameters.Add(new SqlParameter("@Country", Customer.Country == null ? string.Empty : Customer.Country));
                        cmd.Parameters.Add(new SqlParameter("@Phone", Customer.Phone == null ? string.Empty : Customer.Phone));
                        cmd.Parameters.Add(new SqlParameter("@Fax", Customer.Fax == null ? string.Empty : Customer.Fax));
                        CustomerID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    transaction.Commit();
                }
                catch (Exception Exception)
                {
                    transaction.Rollback();
                    throw Exception;
                }
                finally
                {
                    conn.Close();
                }

            }
            return CustomerID;
        }

        /// <summary>
        /// 用CustomerID取得資料
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>order</returns>
        public Models.Customer GetCustomerById(string CustomerID)
        {
            DataTable dt = new DataTable();
            DataTable dtDetails = new DataTable();
            string sql = @"select * FROM Sales.Customers WHERE CustomerID = @CustomerID";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID == null ? string.Empty : CustomerID));
                    sqlAdapter = new SqlDataAdapter(cmd);
                    sqlAdapter.Fill(dt);
                }
              
                conn.Close();
            }


            return this.MapCustomerDataToList(dt).FirstOrDefault();
        }
        public void UpdateOrder(Models.Customer Customer)
        {
            string UpdateOrderSql = @"UPDATE Sales.Customers　SET
							CompanyName=@CompanyName,
							ContactName=@ContactName,
							ContactTitle=@ContactTitle,
							CreationDate=@CreationDate,
							Address=@Address,
							City=@City,
							Region=@Region,
							PostalCode=@PostalCode,
							Country=@Country,
							Phone=@Phone,
							Fax=@Fax
							WHERE CustomerID = @CustomerID
						;";
           
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction("UpdateOrder");
                try
                {
                    using (SqlCommand cmd = new SqlCommand(UpdateOrderSql, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", Customer.CompanyName));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", Customer.ContactName));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", Customer.ContactTitle));
                        cmd.Parameters.Add(new SqlParameter("@CreationDate", Customer.CreationDate));
                        cmd.Parameters.Add(new SqlParameter("@Address", Customer.Address));
                        cmd.Parameters.Add(new SqlParameter("@City", Customer.City));
                        cmd.Parameters.Add(new SqlParameter("@Region", Customer.Region));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", Customer.PostalCode));
                        cmd.Parameters.Add(new SqlParameter("@Country", Customer.Country));
                        cmd.Parameters.Add(new SqlParameter("@Phone", Customer.Phone));
                        cmd.Parameters.Add(new SqlParameter("@Fax", Customer.Fax));
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", Customer.CustomerID));
                        cmd.ExecuteNonQuery();
                    }

                  

                    transaction.Commit();
                }
                catch (Exception Exception)
                {
                    transaction.Rollback();
                    throw Exception;
                }
                finally
                {
                    conn.Close();
                }
            }
        }



    }

}