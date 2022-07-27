using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Service
{
    public class ItemService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Bakery"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        #region 取得單一商品資料
        public Item GetDataById(int Id)
        {
            Item Data = new Item();
            string sql = $@"SELECT * FROM Product WHERE Id = '{Id}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Name = dr["Name"].ToString();
                Data.Id = Convert.ToInt32(dr["Id"]);
                Data.Image = dr["Image"].ToString();
                Data.Price = Convert.ToInt32(dr["Price"]);
                Data.Description = dr["Description"].ToString();
                Data.Unit = dr["Unit"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion
        #region 取得商品編號陣列
        public List<int> GetIdList(ForPaging Paging)
        {
            SetMaxPaging(Paging);
            List<int> IdList = new List<int>();
            string sql = $@"SELECT Id FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) as sort, * FROM Product) m WHERE m.sort BETWEEN {(Paging.NowPage - 1) * Paging.ItemNum +1} AND {Paging.NowPage * Paging.ItemNum}";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    IdList.Add(Convert.ToInt32(dr["Id"]));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return IdList;
        }
        #endregion
        #region 取得所有商品資料
        public List<Item> GetAllItem()
        {
            
            List<Item> ItemList = new List<Item>();
            string sql = $@"select * from Product";
            try
            {               
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //***
                    Item data = new Item();
                    data.Id = Convert.ToInt32(dr["Id"]);
                    data.Name = dr["Name"].ToString();
                    data.Image = dr["Image"].ToString();
                    data.Price = Convert.ToInt32(dr["Price"]);
                    data.Description = dr["Description"].ToString();
                    ItemList.Add(data);
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
            return ItemList;
        }
        #endregion
        #region 取得商品分類編號陣列
        public List<int> GetIdListByCatalog(ForPaging Paging, int Catalog)
        {
            SetMaxPaging(Paging);
            List<int> IdList = new List<int>();
            string sql = $@"SELECT Id FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) as sort, * FROM Product where Catalog = {Catalog}) m WHERE m.sort BETWEEN  {(Paging.NowPage - 1) * Paging.ItemNum + 1} AND {Paging.NowPage * Paging.ItemNum}";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    IdList.Add(Convert.ToInt32(dr["Id"]));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return IdList;
        }
        #endregion
        #region 設定最大頁數
        public void SetMaxPaging(ForPaging Paging)
        {
            int row = 0;
            string sql = $@"select * from Product;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    row++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Paging.ItemNum));
            Paging.SetRightPage();
        }
        #endregion
        #region 新增商品
        public void Insert(Item NewData)
        {
            NewData.Id = LastItemFinder();
            string sql = $@"insert into Product(Id, Name, Price, Image, Description, Catalog, Unit) values({NewData.Id}, '{NewData.Name}', {NewData.Price}, '{NewData.Image}', '{NewData.Description}', {NewData.Catalog}, '{NewData.Unit}');";
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
        #region 刪除商品
        public void Delete(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($@"delete from CartBuy where Item_Id = {Id}");
            sql.AppendLine($@"delete from Product where Id = {Id}");
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
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
        #region 計算商品的最新一筆id
        public int LastItemFinder()
        {
            int Id;
            string sql = $@"select top 1 *from Product order by Id desc";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Id = Convert.ToInt32(dr["Id"]);
            }
            catch (Exception e)
            {
                Id = 0;
            }
            finally
            {
                conn.Close();
            }
            return Id + 1;
        }
        #endregion
       
    }
}