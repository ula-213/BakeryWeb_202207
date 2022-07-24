using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication1.Models;
using System.Security.Cryptography;

namespace WebApplication1.Service
{
    public class MembersDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["Bakery"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
       
        #region 註冊
        public void Register(Members newMember)
        {
            newMember.Password = HashPassword(newMember.Password);
            string sql = $@"INSERT INTO Member (Account, Password, Name, Email, AuthCode, IsAdmin) VALUES ('{newMember.Account}', '{newMember.Password}', '{newMember.Name}', '{newMember.Email}', '{newMember.AuthCode}', '0')";
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
        #region 雜湊
        public string HashPassword(string Password)
        {
            string salt = "sf54sdfsd2f4ds5f4dsfd23s4fsd";
            string saltAndPassword = String.Concat(salt, Password);
            byte[] BeforData = Encoding.UTF8.GetBytes(saltAndPassword);

            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            byte[] AfterData = sha256Hasher.ComputeHash(BeforData);
            string result = Convert.ToBase64String(AfterData);
            return result;
        }
        #endregion
        #region 由帳號取得會員資料
        public Members GetDataByAccount(string Account) 
        {
            Members Data = new Members();
            string sql = $@"SELECT * FROM Member WHERE Account = '{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Email = dr["Email"].ToString();
                Data.AuthCode = dr["AuthCode"].ToString();
                Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
            }
            catch(Exception e)
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
        #region 帳號重複註冊
        public bool AccountCheck(string Account)
        {
            Members Member = GetDataByAccount(Account);
            bool result = (Member == null);
            return result;
        }
        #endregion
        #region 信箱驗證
        public string EmailValidate(string Account, string AuthCode)
        {
            Members Member = GetDataByAccount(Account);
            string ValidateStr = string.Empty;
            if(Member != null)
            {
                if(Member.AuthCode == AuthCode)
                {
                    string sql = $@"UPDATE Member SET AuthCode = '{string.Empty}' WHERE Account = '{Account}'";
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
                    ValidateStr = "帳號信箱驗證成功，請前往登入";
                }
                else
                {
                    ValidateStr = "驗證碼錯誤，請重新確認或在註冊";
                }
                
            }
            else
            {
                ValidateStr = "傳送資料錯誤，請重新確認或註冊";
            }

            return ValidateStr;
            

        }
        #endregion
        #region 登入確認
        public string LoginCheck(string Account, string Password)
        {
            Members member = GetDataByAccount(Account);
            if (member != null)
            {
                if (string.IsNullOrWhiteSpace(member.AuthCode))
                {
                    if (PasswordCheck(member, Password))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                }
                else
                {
                    return "尚未通過驗證，請前往收信";
                }
            }
            else
            {
                return "查無此帳號，請前往註冊";
            }
        }
        #endregion
        #region 密碼確認
        public bool PasswordCheck(Members LoginMember, string Password)
        {
            bool result = LoginMember.Password.Equals(HashPassword(Password));
            return result;
        }
        #endregion
        #region 取得角色
        public string GetRole(string Account)
        {
            string Role = "User";
            Members LoginMember = GetDataByAccount(Account);
            if (LoginMember.IsAdmin)
            {
                Role += ",Admin";
            }
            return Role;
        }
        #endregion
        #region 修改密碼
        public string ChangePassword(string Account, string Password, string newPassword)
        {
            Members LoginMember = GetDataByAccount(Account);
            if (PasswordCheck(LoginMember, Password))
            {
                LoginMember.Password = HashPassword(newPassword);
                string sql = $@"UPDATE Member SET Password = '{LoginMember.Password}' where Account = '{Account}'";
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
                return "密碼修改成功";
            }
            else
            {
                return "舊密碼輸入錯誤";
            }
        }
        #endregion

    }
}