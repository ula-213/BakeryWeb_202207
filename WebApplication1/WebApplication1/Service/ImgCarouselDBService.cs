using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class ImgCarouselDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Bakery"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        public List<ImgCarousel> GetDataList()
        {
            List<ImgCarousel> DataList = new List<ImgCarousel>();
            string sql = $@"SELECT * FROM ImgCarousel";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ImgCarousel Data = new ImgCarousel();
                    Data.id = Convert.ToInt32(dr["id"]);
                    Data.img = dr["img"].ToString();
                    if (!dr["title"].Equals(DBNull.Value))
                    {
                        Data.title = dr["title"].ToString();
                    }
                    if (!dr["info"].Equals(DBNull.Value))
                    {
                        Data.title = dr["info"].ToString();
                    }
                    DataList.Add(Data);

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
            return DataList;
        }
    }
}