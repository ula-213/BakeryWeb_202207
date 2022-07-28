using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Service
{
    public class OrderService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Bakery"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        #region 取得最近50筆訂單
        public List<Order> GetLastest50Order()
        {
            List<Order> datalist = new List<Order>();
            string sql = $@"Select top 50 * from Order1 Order by Time desc";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Order order = new Order();
                    order.Time = dr["Time"].ToString();
                    order.Account = dr["Account"].ToString();
                    order.Cart_Id = dr["Cart_Id"].ToString();
                    order.Name = dr["Name"].ToString();
                    order.PostCode = Convert.ToInt32(dr["PostCode"]);
                    order.Adr = dr["Adr"].ToString();
                    datalist.Add(order);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return datalist;
        }
        #endregion
        #region 取得單筆訂單購買內容
        public List<OrderItem> GetOrderItem(string Cart_Id)
        {
            List<OrderItem> dataList = new List<OrderItem>();
            string sql = $@"select m.Image, m.Name, d.Quantity, m.Price from Product m inner join CartBuy d on d.Item_Id = m.Id where d.Cart_Id = '{Cart_Id}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    OrderItem data = new OrderItem();
                    data.Image = dr["Image"].ToString();
                    data.Name = dr["Name"].ToString();
                    data.Price = Convert.ToInt32(dr["Price"]);
                    data.Quantity = Convert.ToInt32(dr["Quantity"]);
                    dataList.Add(data);
                }               

            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return dataList;
 
        }
        #endregion
    }
}