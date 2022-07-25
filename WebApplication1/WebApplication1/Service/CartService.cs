using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class CartService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Bakery"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public string GetCartSave(string Account)
        {
            CartSave Data = new CartSave();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Member d on m.Account = d.Account where m.Account='{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Member.Name = dr["Name"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }

            if (Data != null)
            {
                return Data.Cart_Id;
            }
            else
            {
                return null;
            }
        }

        #region 取得購物車內商品陣列
        public List<CartBuy> GetItemFromCart(string Cart)
        {
            List<CartBuy> datalist = new List<CartBuy>();
            string sql = $@"SELECT * FROM CartBuy m INNER JOIN Product d on m.Item_Id = d.Id WHERE m.Cart_Id = '{Cart}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CartBuy data = new CartBuy();
                    data.Cart_Id = dr["Cart_Id"].ToString();
                    data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
                    data.Item.Id = Convert.ToInt32(dr["Id"]);
                    data.Item.Image = dr["Image"].ToString();
                    data.Item.Name = dr["Name"].ToString();
                    data.Item.Price = Convert.ToInt32(dr["Price"]);
                    datalist.Add(data);
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
        #region 確認購物車是否有保存
        public bool CheckCartSave(string Account, string Cart)
        {
            CartSave Data = new CartSave();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Member d on m.Account = d.Account WHERE m.Account ='{Account}' AND Cart_Id = '{Cart}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Member.Name = dr["Name"].ToString();
            }
            catch(Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return (Data != null);
        }
        #endregion
        #region 將商品放入購物車
        public void AddtoCart(string Cart, int Item_Id)
        {
            string sql = $@"Insert into CartBuy(Cart_Id, Item_Id) values ('{Cart}', {Item_Id})";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 將商品取出購物車
        public void RemoveForCart(string Cart, int Item_Id)
        {
            string sql = $@"Delete From CartBuy WHERE Cart_Id = '{Cart}' AND Item_Id = {Item_Id}";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        #region 保存購物車
        public void SaveCart(string Account, string Cart)
        {
            string sql = $@"Insert into CartSave(Account, Cart_Id) values('{Account}', '{Cart}')";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        #region 取消保存購物車
        public void SaveCartRemove(string Account)
        {
            string sql = $@"DELETE FROM CartSave WHERE Account = '{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        

    }
}