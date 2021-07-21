using GongJuJiHe.ShuJuZhuanHuanGJ;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace CKGLXT.DBFanWen
{
    public class MySqlLianJieQi : ABSSqlDBLianJie
    {
        #region 变量区       
        /// <summary>
        /// 表示是否可以用
        /// </summary>
        private bool IsKeYiShiYong = true;
        private string _ConString = "";
        #endregion
        public MySqlLianJieQi()
        {
            ReadeXml();
        }

        private void ReadeXml()
        {
            string path = string.Format("{0}{1}", Path.GetFullPath("."), @"\PeiZhiDB.xml");
            
            if (!File.Exists(path))
            {
                IsKeYiShiYong = false;
              
                MessageBox.Show(string.Format("{0}", path));
                return;
            }
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                string IP = xml.SelectSingleNode("DB/数据库IP").InnerText;
                string DBName = xml.SelectSingleNode("DB/数据库MC").InnerText;
                string YHname = xml.SelectSingleNode("DB/数据库YH").InnerText;
                string mima = xml.SelectSingleNode("DB/数据库MIMA").InnerText;
                _ConString = string.Format("Database = {0}; datasource ={1}; port = 3306; user = {2}; pwd = {3}; charset='utf8';pooling=true;", DBName, IP, YHname, mima);
            }
            catch (Exception ex)
            {
                IsKeYiShiYong = false;
                FaSongFuWu(string.Format("配置文件有问题:{0}", ex.Message), 2);
                
            }

        }

        private void FaSongFuWu(string msg, int leixing)
        {
            
        }
     

        public override object GenJuSqlChaXunZhi(string sql, string ziduan)
        {
            FaSongFuWu(sql, 1);
            DataTable dt = GetDataTable(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    int count = dt.Rows.Count - 1;
                    try
                    {
                        return dt.Rows[count][ziduan];
                    }
                    catch
                    {
                        return null;

                    }

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public override Dictionary<string, object> GenJuSqlChaXunZhi(string sql, List<string> ziduan)
        {
            FaSongFuWu(sql, 1);
            DataTable dt = GetDataTable(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    int count = dt.Rows.Count - 1;
                    try
                    {
                        Dictionary<string, object> zidianzhi = new Dictionary<string, object>();
                        for (int i = 0; i < ziduan.Count; i++)
                        {
                            zidianzhi.Add(ziduan[i], dt.Rows[count][ziduan[i]]);

                        }
                        return zidianzhi;
                    }
                    catch
                    {
                        return null;

                    }

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public override List<object> GenJuSqlLisChaXunZhi(string sql, string ziduan)
        {
            FaSongFuWu(sql, 1);
            DataTable dt = GetDataTable(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    try
                    {
                        List<object> fanhui = new List<object>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            fanhui.Add(dt.Rows[i][ziduan]);
                        }
                        return fanhui;
                    }
                    catch
                    {
                        return new List<object>();

                    }

                }
                else
                {
                    return new List<object>();
                }
            }
            else
            {
                return new List<object>();
            }
        }

        public override int GetCount(string sql)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return -1;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    FaSongFuWu(sql, 1);

                    MySqlDataAdapter det = new MySqlDataAdapter(sql, con);
                    DataSet set = new DataSet();
                    det.Fill(set);
                    DataTable dt = set.Tables[0];
                    det.Dispose();
                    set.Dispose();
                    return dt.Rows.Count;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);

                    return -1;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public override DataTable GetDataTable(string sql)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return null;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    FaSongFuWu(sql, 1);
                    MySqlDataAdapter det = new MySqlDataAdapter(sql, con);
                    DataSet set = new DataSet();
                    det.Fill(set);
                    DataTable dt = set.Tables[0];
                    det.Dispose();
                    set.Dispose();
                    return dt;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);

                    return null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public override DataTable GetDataTable(string sql, Dictionary<string, object> Parameter)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return null;
            }
        
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    MySqlDataAdapter det = new MySqlDataAdapter(sql, con);
                    if (Parameter != null && Parameter.Count > 0)
                    {
                        MySqlParameter[] shuju = GetSqlParameter(Parameter);
                        det.SelectCommand.Parameters.AddRange(shuju);
                    }
                    FaSongFuWu(det.SelectCommand.CommandText, 1);
                    DataSet set = new DataSet();
                    det.Fill(set);
                    DataTable dt = set.Tables[0];
                    det.Dispose();
                    set.Dispose();
                    return dt;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);

                    return null;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        private MySqlParameter[] GetSqlParameter(Dictionary<string, object> ziDian)
        {
            if (ziDian == null)
            {
                return null;
            }
            if (ziDian.Count == 0)
            {
                return null;
            }
            List<MySqlParameter> lis = new List<MySqlParameter>();
            lis.Clear();
            foreach (string item in ziDian.Keys)
            {
                try
                {
                    MySqlParameter shuju = new MySqlParameter(string.Format("@{0}", item), ziDian[item]);
                    lis.Add(shuju);
                }
                catch
                {

                }


            }
            return lis.ToArray();
        }

        public override string GetDataTableJosn(string sql)
        {
           
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return "";
            }
           
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    FaSongFuWu(sql, 1);

                    MySqlDataAdapter det = new MySqlDataAdapter(sql, con);
                    DataSet set = new DataSet();
                    det.Fill(set);
                    DataTable dt = set.Tables[0];
                    string josn = ShuJuZhuanHuan.HuoQuJsonStr(dt);
                    det.Dispose();
                    set.Dispose();
                    return josn;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);

                    return "";
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public override string GetDataTableJosn(string sql, Dictionary<string, object> Parameter)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return "";
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    MySqlDataAdapter det = new MySqlDataAdapter(sql, con);
                    if (Parameter != null && Parameter.Count > 0)
                    {
                        MySqlParameter[] shuju = GetSqlParameter(Parameter);
                        det.SelectCommand.Parameters.AddRange(shuju);
                    }

                    FaSongFuWu(det.SelectCommand.CommandText, 1);

                    DataSet set = new DataSet();
                    det.Fill(set);
                    DataTable dt = set.Tables[0];
                    string josn = ShuJuZhuanHuan.HuoQuJsonStr(dt);
                    det.Dispose();
                    set.Dispose();
                    return josn;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);

                    return "";
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public override bool ShiWuUpdate(List<string> sqllist)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            if (sqllist.Count == 0)
            {
                FaSongFuWu(string.Format("没有sql语句"), 2);
                return false;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                con.Open();
                MySqlCommand com = new MySqlCommand();
                com.Connection = con;
                try
                {

                    com.Transaction = con.BeginTransaction();
                    foreach (var item in sqllist)
                    {
                        com.CommandText = item;
                        FaSongFuWu(com.CommandText, 1);
                        com.ExecuteNonQuery();
                    }
                    com.Transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (com.Transaction != null)
                    {
                        com.Transaction.Rollback();
                    }
                    FaSongFuWu(string.Format("执行事务失败:{0}", ex.Message), 2);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return false;
            }
        }

        public override bool ShiWuUpdate(List<string> sqllist, List<Dictionary<string, object>> Parameter)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            if (sqllist.Count == 0)
            {
                FaSongFuWu(string.Format("没有sql语句"), 2);
                return false;
            }
            if (Parameter != null && Parameter.Count > 0)
            {
                if (sqllist.Count != Parameter.Count)
                {
                    FaSongFuWu(string.Format("sql语句与参数不一致"), 2);
                    return false;
                }
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                con.Open();
                MySqlCommand com = new MySqlCommand();
                com.Connection = con;
                try
                {

                    com.Transaction = con.BeginTransaction();
                    for (int i = 0; i < sqllist.Count; i++)
                    {
                        com.CommandText = sqllist[i];
                        com.Parameters.Clear();
                        MySqlParameter[] cmdParms = null;
                        if (Parameter != null)
                        {
                            if (Parameter.Count > i)
                            {
                                cmdParms = GetSqlParameter(Parameter[i]);
                            }
                        }
                        if (cmdParms != null)
                        {
                            com.Parameters.AddRange(cmdParms);
                        }
                        FaSongFuWu(com.CommandText, 1);
                        com.ExecuteNonQuery();
                    }
                    com.Transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (com.Transaction != null)
                    {
                        com.Transaction.Rollback();
                    }
                    FaSongFuWu(string.Format("执行事务失败:{0}", ex.Message), 2);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return false;
            }
        }

        public override int UpdateOrInsertdate(string sql)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);

                return -2;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    MySqlCommand com = new MySqlCommand(sql, con);
                    int zhixing = com.ExecuteNonQuery();
                    FaSongFuWu(sql, 1);
                    com.Dispose();
                    return zhixing;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);
                    return -3;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public override int UpdateOrInsertdate(string sql, Dictionary<string, object> Parameter)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return -2;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                try
                {
                    con.Open();
                    MySqlCommand com = new MySqlCommand(sql, con);
                    if (Parameter != null && Parameter.Count > 0)
                    {
                        MySqlParameter[] shuju = GetSqlParameter(Parameter);
                        com.Parameters.AddRange(shuju);
                    }
                    FaSongFuWu(com.CommandText, 1);
                    int zhixing = com.ExecuteNonQuery();
                    com.Dispose();
                    return zhixing;
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);
                    return -3;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public override int UpdateZiDuan(string ziduanming, string biaoming, string tiaojianziduan, object ziduanshuju, object tiaojianshuju)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return -1;
            }
            string sql = string.Format(@"update {0} set {1}={2}{3}{4}  where {5}={6}{7}{8} ", biaoming, ziduanming, HuoQuLeiXing(ziduanshuju) == 1 ? "'" : "", ziduanshuju, HuoQuLeiXing(ziduanshuju) == 1 ? "'" : "", tiaojianziduan, HuoQuLeiXing(tiaojianshuju) == 1 ? "'" : "", tiaojianshuju, HuoQuLeiXing(tiaojianshuju) == 1 ? "'" : "");
            int shuliang = UpdateOrInsertdate(sql);
            FaSongFuWu(sql, 1);

            return shuliang;
        }

        public override bool XieRuTuPian(string sPicPaht, string biaoming, string tiaojianziduanming, string ziduanming, int id)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            using (FileStream fs = new FileStream(sPicPaht, FileMode.Open, FileAccess.Read))
            {
                using (MySqlConnection con = new MySqlConnection(_ConString))
                {
                    con.Open();
                    BinaryReader erjinzhi = new BinaryReader(fs);
                    byte[] imbyte = erjinzhi.ReadBytes(Convert.ToInt32(fs.Length));
                    string sq = string.Format(@"select * from {0} where {1}={2}", biaoming, tiaojianziduanming, id);
                    MySqlDataReader reader1 = GetReader(sq, con);
                    try
                    {
                        if (reader1.Read())
                        {
                            reader1.Close();
                            string sql = string.Format(@"update {0} set {1}=@{2}  where   {3}={4}", biaoming, ziduanming, "pic", tiaojianziduanming, id);
                            MySqlCommand com = new MySqlCommand();
                            com.CommandType = CommandType.Text;
                            com.CommandText = sql;
                            com.Connection = con;
                            FaSongFuWu(sql, 1);

                            com.Parameters.Add("pic", MySqlDbType.MediumBlob);
                            com.Parameters["pic"].Value = imbyte;

                            int shuliang = com.ExecuteNonQuery();
                            if (shuliang == 1)
                            {
                                com.Dispose();
                                return true;
                            }
                            else
                            {
                                com.Dispose();
                                return false;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);


                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                    reader1.Close();
                    return false;

                }

            }
        }

        public override bool XieRuTuPian(string sPicPaht, string biaoming, string tiaojianziduanming, string ziduanming, string id)
        {

            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            using (FileStream fs = new FileStream(sPicPaht, FileMode.Open, FileAccess.Read))
            {
                using (MySqlConnection con = new MySqlConnection(_ConString))
                {
                    con.Open();
                    BinaryReader erjinzhi = new BinaryReader(fs);
                    byte[] imbyte = erjinzhi.ReadBytes(Convert.ToInt32(fs.Length));
                    string sq = string.Format(@"select * from {0} where {1}='{2}'", biaoming, tiaojianziduanming, id);
                    MySqlDataReader reader1 = GetReader(sq, con);
                    try
                    {
                        if (reader1.Read())
                        {
                            reader1.Close();
                            string sql = string.Format(@"update {0} set {1}=@{2}  where  {3}='{4}'", biaoming, ziduanming, "pic", tiaojianziduanming, id);
                            MySqlCommand com = new MySqlCommand();
                            com.CommandType = CommandType.Text;
                            com.CommandText = sql;
                            com.Connection = con;
                            FaSongFuWu(sql, 1);

                            com.Parameters.Add("pic", MySqlDbType.MediumBlob);
                            com.Parameters["pic"].Value = imbyte;

                            int shuliang = com.ExecuteNonQuery();
                            if (shuliang == 1)
                            {


                                com.Dispose();
                                return true;
                            }
                            else
                            {

                                com.Dispose();
                                return false;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                    reader1.Close();
                    return false;

                }

            }
        }

        public override bool XieRuTuPian(byte[] byteimage, string biaoming, string tiaojianziduanming, string ziduanming, string id)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            if (byteimage == null)
            {
                FaSongFuWu(string.Format("传来的数据为null"), 2);
                return false;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                con.Open();
                string sq = string.Format(@"select * from {0} where {1}={2}", biaoming, tiaojianziduanming, id);
                MySqlDataReader reader1 = GetReader(sq, con);
                try
                {
                    if (reader1.Read())
                    {
                        reader1.Close();
                        string sql = string.Format(@"update {0} set {1}=@{2}  where   {3}={4}", biaoming, ziduanming, "pic", tiaojianziduanming, id);
                        MySqlCommand com = new MySqlCommand();
                        com.CommandType = CommandType.Text;
                        com.CommandText = sql;
                        com.Connection = con;
                        FaSongFuWu(sql, 1);

                        com.Parameters.Add("pic", MySqlDbType.MediumBlob);
                        com.Parameters["pic"].Value = byteimage;

                        int shuliang = com.ExecuteNonQuery();
                        if (shuliang == 1)
                        {
                            com.Dispose();
                            return true;
                        }
                        else
                        {
                            com.Dispose();
                            return false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);


                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                reader1.Close();
                return false;

            }
        }

        public override bool XieRuTuPian(byte[] byteimage, string biaoming, string tiaojianziduanming, string ziduanming, int id)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            if (byteimage == null)
            {
                FaSongFuWu(string.Format("传来的数据为null"), 2);
                return false;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                con.Open();

                string sq = string.Format(@"select * from {0} where {1}={2}", biaoming, tiaojianziduanming, id);
                MySqlDataReader reader1 = GetReader(sq, con);
                try
                {
                    if (reader1.Read())
                    {
                        reader1.Close();
                        string sql = string.Format(@"update {0} set {1}=@{2}  where   {3}={4}", biaoming, ziduanming, "pic", tiaojianziduanming, id);
                        MySqlCommand com = new MySqlCommand();
                        com.CommandType = CommandType.Text;
                        com.CommandText = sql;
                        com.Connection = con;
                        FaSongFuWu(sql, 1);

                        com.Parameters.Add("pic", MySqlDbType.MediumBlob);
                        com.Parameters["pic"].Value = biaoming;

                        int shuliang = com.ExecuteNonQuery();
                        if (shuliang == 1)
                        {
                            com.Dispose();
                            return true;
                        }
                        else
                        {
                            com.Dispose();
                            return false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);


                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                reader1.Close();
                return false;

            }
        }

        public override bool XieRuTuPian(Image byteimage, string biaoming, string tiaojianziduanming, string ziduanming, string id)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            if (byteimage == null)
            {
                FaSongFuWu(string.Format("传来的数据为null"), 2);
                return false;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                con.Open();
                byte[] imbyte = ImageToBytes(byteimage);
                string sq = string.Format(@"select * from {0} where {1}={2}", biaoming, tiaojianziduanming, id);
                MySqlDataReader reader1 = GetReader(sq, con);
                try
                {
                    if (reader1.Read())
                    {
                        reader1.Close();
                        string sql = string.Format(@"update {0} set {1}=@{2}  where   {3}={4}", biaoming, ziduanming, "pic", tiaojianziduanming, id);
                        MySqlCommand com = new MySqlCommand();
                        com.CommandType = CommandType.Text;
                        com.CommandText = sql;
                        com.Connection = con;
                        FaSongFuWu(sql, 1);

                        com.Parameters.Add("pic", MySqlDbType.MediumBlob);
                        com.Parameters["pic"].Value = imbyte;

                        int shuliang = com.ExecuteNonQuery();
                        if (shuliang == 1)
                        {
                            com.Dispose();
                            return true;
                        }
                        else
                        {
                            com.Dispose();
                            return false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);


                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                reader1.Close();
                return false;

            }
        }

        public override bool XieRuTuPian(Image byteimage, string biaoming, string tiaojianziduanming, string ziduanming, int id)
        {
            if (IsKeYiShiYong == false)
            {
                FaSongFuWu(string.Format("数据库未连接"), 2);
                return false;
            }
            if (byteimage == null)
            {
                FaSongFuWu(string.Format("传来的数据为null"), 2);
                return false;
            }
            using (MySqlConnection con = new MySqlConnection(_ConString))
            {
                con.Open();
                byte[] imbyte = ImageToBytes(byteimage);

                string sq = string.Format(@"select * from {0} where {1}={2}", biaoming, tiaojianziduanming, id);
                MySqlDataReader reader1 = GetReader(sq, con);
                try
                {
                    if (reader1.Read())
                    {
                        reader1.Close();
                        string sql = string.Format(@"update {0} set {1}=@{2}  where   {3}={4}", biaoming, ziduanming, "pic", tiaojianziduanming, id);
                        MySqlCommand com = new MySqlCommand();
                        com.CommandType = CommandType.Text;
                        com.CommandText = sql;
                        com.Connection = con;
                        FaSongFuWu(sql, 1);

                        com.Parameters.Add("pic", MySqlDbType.MediumBlob);
                        com.Parameters["pic"].Value = imbyte;

                        int shuliang = com.ExecuteNonQuery();
                        if (shuliang == 1)
                        {
                            com.Dispose();
                            return true;
                        }
                        else
                        {
                            com.Dispose();
                            return false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    FaSongFuWu(string.Format("数据库访问失败:{0}", ex.Message), 2);


                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                reader1.Close();
                return false;

            }

        }
        private int HuoQuLeiXing(object o)
        {
            int biaozhi = 1;
            if (o is int)
            {
                biaozhi = 2;
            }
            else if (o is float)
            {
                biaozhi = 2;
            }
            else if (o is double)
            {
                biaozhi = 2;
            }
            return biaozhi;
        }
        private MySqlDataReader GetReader(string sqlyuju, MySqlConnection con)
        {
            MySqlCommand com = new MySqlCommand(sqlyuju, con);
            MySqlDataReader reader = com.ExecuteReader();
            return reader;

        }

        private byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
