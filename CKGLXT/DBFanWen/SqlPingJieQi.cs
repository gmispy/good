using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GongJuJiHe.ShuJuZhuanHuanGJ;

namespace CKGLXT.DBFanWen
{
    /// <summary>
    /// 拼接sql语句
    /// </summary>
    public class SqlYuJuPingJie
    {

        /// <summary>
        /// 增加语句的拼接有参数的@
        /// </summary>
        /// <param name="ZiDuanMing">字段</param>
        /// <param name="BiaoMing">表名</param>
        /// <returns></returns>
        public virtual string InsertParameterSql(List<string> ZiDuanMing, string BiaoMing)
        {
            if (ZiDuanMing.Count == 0)
            {
                return "";
            }
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("insert into {0} ", BiaoMing));
            sb.Append("(");
            sb.Append(string.Join(",", ZiDuanMing.Select((p) => { return p; })));
            sb.Append(")");
            sb.Append(" values ");
            sb.Append("( ");
            for (int i = 0; i < ZiDuanMing.Count; i++)
            {
                if (i == ZiDuanMing.Count - 1)
                {
                    sb.Append(string.Format("@{0}", ZiDuanMing[i]));
                }
                else
                {
                    sb.Append(string.Format("@{0},", ZiDuanMing[i]));
                }
            }
            sb.Append(")");
            return sb.ToString();

        }

        /// <summary>
        /// 更新语句带@
        /// </summary>
        /// <param name="ZiDuanMing">更新字段</param>
        /// <param name="BiaoMing">表名</param>
        /// <param name="TiaoJianZiDuan">条件字段</param>
        /// <returns></returns>
        public virtual string UpateParameterSql(List<string> ZiDuanMing, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // string UpdateSql = string.Format("update XueShengBiao set   XS_KaoHao=@XS_KaoHao, XS_LianXiFangShi=@XS_LianXiFangShi, XS_XingMing=@XS_XingMing   where XS_Id=@XS_Id "); 
            if (ZiDuanMing.Count == 0)
            {
                return "";
            }
            if (TiaoJianZiDuan.Count == 0)
            {
                return "";
            }
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("update {0}", BiaoMing));
            sb.Append(" set ");
            for (int i = 0; i < ZiDuanMing.Count; i++)
            {
                if (i == ZiDuanMing.Count - 1)
                {
                    sb.Append(string.Format(" {0}=@{1}", ZiDuanMing[i], ZiDuanMing[i]));
                }
                else
                {
                    sb.Append(string.Format("{0}=@{1}, ", ZiDuanMing[i], ZiDuanMing[i]));
                }
            }
            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
            }
            for (int i = 0; i < TiaoJianZiDuan.Count; i++)
            {
                if (i == TiaoJianZiDuan.Count - 1)
                {
                    sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                }
                else
                {
                    sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                }
            }

            return sb.ToString();

        }
        /// <summary>
        /// 删除语句带@
        /// </summary>
        /// <param name="BiaoMing">表名</param>
        /// <param name="TiaoJianZiDuan">条件字段</param>
        /// <returns></returns>
        public virtual string DeleteParameterSql(string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // string UpdateSql = string.Format("update XueShengBiao set   XS_KaoHao=@XS_KaoHao, XS_LianXiFangShi=@XS_LianXiFangShi, XS_XingMing=@XS_XingMing   where XS_Id=@XS_Id ");           
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("delete from {0}", BiaoMing));
            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }


            return sb.ToString();

        }

        /// <summary>
        /// 删除语句不带@
        /// </summary>
        /// <param name="BiaoMing">表名</param>
        /// <param name="TiaoJian">条件</param>
        /// <returns></returns>
        public virtual string DeleteSql(string BiaoMing, List<string> TiaoJian)
        {

            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("delete from {0}", BiaoMing));
            if (TiaoJian.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJian.Count; i++)
                {
                    if (i == TiaoJian.Count - 1)
                    {
                        sb.Append(string.Format(" {0}", TiaoJian[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0} and  ", TiaoJian[i]));
                    }
                }
            }


            return sb.ToString();

        }

        /// <summary>
        /// 查询单表语句带@
        /// </summary>
        /// <param name="ZiDuanMing">查询字段，没有就查询所有</param>
        /// <param name="BiaoMing">表名</param>
        /// <param name="TiaoJianZiDuan">条件字段</param>
        /// <returns></returns>
        public virtual string SelectParameterSql(List<string> ZiDuanMing, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            //string SelectSql=string.Format("select XS_Id, XS_KaoHao, XS_LianXiFangShi, XS_XingMing  from XueShengBiao   where XS_Id=@XS_Id ");                         
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select "));
            if (ZiDuanMing.Count > 0)
            {
                for (int i = 0; i < ZiDuanMing.Count; i++)
                {
                    if (i == ZiDuanMing.Count - 1)
                    {
                        sb.Append(string.Format(" {0} ", ZiDuanMing[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}, ", ZiDuanMing[i]));
                    }
                }
            }
            else
            {
                sb.Append(string.Format(" * "));
            }
            sb.Append(string.Format(" from {0} ", BiaoMing));
            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 查询单表语句
        /// </summary>
        /// <param name="ZiDuanMing">查询字段，没有就查询所有</param>
        /// <param name="BiaoMing">表名</param>
        /// <param name="TiaoJian">条件</param>
        /// <returns></returns>
        public virtual string SelectSql(List<string> ZiDuanMing, string BiaoMing, List<string> TiaoJian)
        {
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select "));
            if (ZiDuanMing.Count > 0)
            {
                for (int i = 0; i < ZiDuanMing.Count; i++)
                {
                    if (i == ZiDuanMing.Count - 1)
                    {
                        sb.Append(string.Format(" {0} ", ZiDuanMing[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}, ", ZiDuanMing[i]));
                    }
                }
            }
            else
            {
                sb.Append(string.Format(" * "));
            }
            sb.Append(string.Format(" from {0} ", BiaoMing));
            if (TiaoJian.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJian.Count; i++)
                {
                    if (i == TiaoJian.Count - 1)
                    {
                        sb.Append(string.Format(" {0} ", TiaoJian[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0} and  ", TiaoJian[i]));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 查询带有@数量
        /// </summary>
        /// <param name="BiaoMing">表面</param>
        /// <param name="TiaoJianZiDuan">条件字段</param>
        /// <returns></returns>
        public virtual string SelectParameterCount(string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT count(*) from XueShengBiao;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select count(*) as MingCheng "));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="BiaoMing">表面</param>
        /// <param name="TiaoJian">条件</param>
        /// <returns></returns>
        public virtual string SelectCount(string BiaoMing, List<string> TiaoJian)
        {
            // SELECT count(*) from XueShengBiao;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select count(*) as MingCheng "));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJian.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJian.Count; i++)
                {
                    if (i == TiaoJian.Count - 1)
                    {
                        sb.Append(string.Format(" {0}", TiaoJian[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}} and ", TiaoJian[i]));
                    }
                }
            }
            return sb.ToString();
        }

        public virtual string SelectSqlAVG(string ZiDuan, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select avg({0}) as MingCheng ", ZiDuan));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

        public virtual string SelectSqlSum(string ZiDuan, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select sum({0}) as MingCheng ", ZiDuan));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

        public virtual string SelectSqlMin(string ZiDuan, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select min({0}) as MingCheng ", ZiDuan));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

        public virtual string SelectSqlMax(string ZiDuan, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select max({0}) as MingCheng ", ZiDuan));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

        public virtual string SelectSqlCountZuiZhi(string CountZiDuan, string AvgZiDuan, string MinZiDuan, string MaxZiDuan, string SumZiDuan, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select  "));
            if (!string.IsNullOrEmpty(CountZiDuan))
            {
                sb.Append(string.Format(" count(*) as MingCheng  "));
            }
            if (!string.IsNullOrEmpty(AvgZiDuan))
            {
                sb.Append(string.Format(" avg({0}) as MingCheng1  ", AvgZiDuan));
            }
            if (!string.IsNullOrEmpty(MinZiDuan))
            {
                sb.Append(string.Format(" min({0}) as MingCheng2  ", MinZiDuan));
            }
            if (!string.IsNullOrEmpty(MaxZiDuan))
            {
                sb.Append(string.Format(" max({0}) as MingCheng3  ", MaxZiDuan));
            }
            if (!string.IsNullOrEmpty(SumZiDuan))
            {
                sb.Append(string.Format(" sum({0}) as MingCheng4  ", SumZiDuan));
            }
            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 查询表的多少记录带参数的
        /// </summary>
        /// <param name="julushu"></param>
        /// <param name="BiaoMing"></param>
        /// <param name="TiaoJianZiDuan"></param>
        /// <returns></returns>
        public virtual string SelectSqlTop(int julushu, string BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (string.IsNullOrEmpty(BiaoMing))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select top {0}  *", julushu));

            sb.Append(string.Format(" from {0} ", BiaoMing));

            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}=@{1}", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}=@{1} and ", TiaoJianZiDuan[i], TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 多表查询 不带参数@
        /// </summary>
        /// <param name="ZiDuanMing">字段</param>
        /// <param name="BiaoMing">多表</param>
        /// <param name="TiaoJianZiDuan">条件</param>
        /// <returns></returns>
        public virtual string SelectSqlDuoBiaoChaXun(List<string> ZiDuanMing, List<string> BiaoMing, List<string> TiaoJianZiDuan)
        {
            // SELECT avg(second) from test;          
            if (BiaoMing.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select "));
            if (ZiDuanMing.Count > 0)
            {
                for (int i = 0; i < ZiDuanMing.Count; i++)
                {
                    if (i == ZiDuanMing.Count - 1)
                    {
                        sb.Append(string.Format(" {0} ", ZiDuanMing[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}, ", ZiDuanMing[i]));
                    }
                }
            }
            else
            {
                sb.Append(string.Format(" * "));
            }
            sb.Append(string.Format(" from  "));
            for (int i = 0; i < BiaoMing.Count; i++)
            {
                if (i == BiaoMing.Count - 1)
                {
                    sb.Append(string.Format(" {0} ", BiaoMing[i]));
                }
                else
                {
                    sb.Append(string.Format("{0}, ", BiaoMing[i]));
                }
            }
            if (TiaoJianZiDuan.Count > 0)
            {
                sb.Append(" where  ");
                for (int i = 0; i < TiaoJianZiDuan.Count; i++)
                {
                    if (i == TiaoJianZiDuan.Count - 1)
                    {
                        sb.Append(string.Format(" {0}", TiaoJianZiDuan[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0} and ", TiaoJianZiDuan[i]));
                    }
                }
            }
            return sb.ToString();
        }

   


        public virtual SqlParameter[] GetSqlParameter<T>(T model)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Clear();
            Type t = model.GetType();
            PropertyInfo[] shuxin = t.GetProperties();
            foreach (PropertyInfo item in shuxin)
            {
                try
                {
                    SqlParameter shuju = new SqlParameter(string.Format("@{0}", item.Name), item.GetValue(model, null));
                    lis.Add(shuju);
                }
                catch
                {

                }


            }
            return lis.ToArray();
        }

        public virtual SqlParameter[] GetSqlParameter(Dictionary<string, object> ziDian)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Clear();
            foreach (string item in ziDian.Keys)
            {
                try
                {
                    SqlParameter shuju = new SqlParameter(string.Format("@{0}", item), ziDian[item]);
                    lis.Add(shuju);
                }
                catch
                {

                }


            }
            return lis.ToArray();
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetBiaoMing<T>() where T : new()
        {
            Type t = typeof(T);
            string biaoming = t.Name;
            return biaoming;
        }
        /// <summary>
        /// 获取字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<string> GetZiDuan<T>() where T : new()
        {
            Type t = typeof(T);
            PropertyInfo[] shuxin = t.GetProperties();
            List<string> ziduan = new List<string>();
            foreach (PropertyInfo item in shuxin)
            {
                string name = item.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    ziduan.Add(name);
                }
            }
            return ziduan;
        }

        /// <summary>
        /// 获取list实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="josnstr"></param>
        /// <returns></returns>
        public List<T> GetListShiTi<T>(string josnstr)
        {
            if (string.IsNullOrEmpty(josnstr))
            {
                return new List<T>();
            }
            List<T> shuju = new List<T>();
            try
            {
                shuju = ShuJuZhuanHuan.HuoQuJsonToShiTi<List<T>>(josnstr);
            }
            catch
            {

            }
            return shuju;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="josnstr"></param>
        /// <returns></returns>
        public T GetDanGeShiTi<T>(string josnstr)
        {
            if (string.IsNullOrEmpty(josnstr))
            {
                return default(T);
            }
            List<T> shuju = new List<T>();
            try
            {
                shuju = ShuJuZhuanHuan.HuoQuJsonToShiTi<List<T>>(josnstr);
                if (shuju.Count > 0)
                {
                    return shuju[0];
                }
            }
            catch
            {

            }
            return default(T);
        }
    }
    /// <summary>
    /// 更新或者增加语句的标志
    /// </summary>
    public enum UpdateOrAdd
    {
        /// <summary>
        /// 增加语句
        /// </summary>
        Add = 0,
        /// <summary>
        /// 更新语句
        /// </summary>
        Update = 1,
    }
    /// <summary>
    ///字符与数字的枚举
    /// </summary>
    public enum ZiFuOrInt
    {
        /// <summary>
        /// 不需要加单引号的
        /// </summary>
        Int = 0,
        /// <summary>
        /// 需要加单引号的
        /// </summary>
        ZiFu = 1,
    }
    /// <summary>
    /// 增加与更新语句的地方
    /// </summary>
    public class AddOrUpdate : IDisposable
    {
        private UpdateOrAdd BiaoZhi = 0;
        private StringBuilder SbString = new StringBuilder();
        private List<string> LisZiDuan = new List<string>();
        private List<string> LisZiDuanCanShu = new List<string>();
        private List<string> LisZiDuanWhere = new List<string>();
        private List<string> LisZiDuanCanShuWhere = new List<string>();

        /// <summary>
        /// 1表示增加语句，2表示更新语句
        /// </summary>
        /// <param name="biaozhi"></param>
        public void SetBiaoZhi(UpdateOrAdd biaozhi, string biaoming)
        {
            Clear();
            BiaoZhi = biaozhi;
            if (biaozhi == UpdateOrAdd.Add)
            {
                SbString.Append(string.Format("insert into {0} ", biaoming));
            }
            else if (biaozhi == UpdateOrAdd.Update)
            {
                SbString.Append(string.Format("update {0} set ", biaoming));
            }


        }
        private void Clear()
        {
            SbString.Clear();
            LisZiDuan.Clear();
            LisZiDuanCanShu.Clear();
            LisZiDuanWhere.Clear();
            LisZiDuanCanShuWhere.Clear();
        }

        /// <summary>
        /// 用于insert语句和更新语句，通过GetSQLString方法获取 
        /// </summary>
        /// <param name="ziduan"></param>
        /// <param name="ziduanvalue"></param>
        /// <param name="biaozhi"></param>
        public void ZengJiaSql(string ziduan, object ziduanvalue, ZiFuOrInt biaozhi)
        {
            if (ziduanvalue != null)
            {
                LisZiDuan.Add(ziduan);
                string danyinhao = biaozhi == ZiFuOrInt.ZiFu ? "'" : "";
                LisZiDuanCanShu.Add(string.Format("{0}{1}{2}", danyinhao, ziduanvalue, danyinhao));
            }


        }

        /// <summary>
        /// 标志为1表示字符串，2表示int flaot类型
        /// </summary>
        /// <param name="ziduan"></param>
        /// <param name="ziduanvalue"></param>
        /// <param name="biaozhi"></param>
        public void Where(string ziduan, object ziduanvalue, ZiFuOrInt biaozhi)
        {
            if (ziduanvalue != null)
            {

                LisZiDuanWhere.Add(ziduan);
                string danyinhao = biaozhi == ZiFuOrInt.ZiFu ? "'" : "";
                LisZiDuanCanShuWhere.Add(string.Format("{0}{1}{2}", danyinhao, ziduanvalue, danyinhao));
            }
        }
        /// <summary>
        /// 组装之后获取sql语句
        /// </summary>
        /// <returns></returns>
        public string GetSQLString()
        {
            if (LisZiDuan.Count == 0 || LisZiDuanCanShu.Count == 0 || LisZiDuanCanShu.Count != LisZiDuan.Count)
            {
                return "";
            }
            switch (BiaoZhi)
            {
                case UpdateOrAdd.Add:
                    {
                        #region MyRegion
                        SbString.Append(" (");
                        for (int i = 0; i < LisZiDuan.Count; i++)
                        {
                            if (i == LisZiDuan.Count - 1)
                            {
                                SbString.Append(string.Format("{0}", LisZiDuan[i]));
                            }
                            else
                            {
                                SbString.Append(string.Format("{0},", LisZiDuan[i]));
                            }
                        }

                        SbString.Append(" )");

                        SbString.Append(" values ");
                        SbString.Append("( ");
                        for (int i = 0; i < LisZiDuanCanShu.Count; i++)
                        {
                            if (i == LisZiDuanCanShu.Count - 1)
                            {
                                SbString.Append(string.Format("{0}", LisZiDuanCanShu[i]));
                            }
                            else
                            {
                                SbString.Append(string.Format("{0},", LisZiDuanCanShu[i]));
                            }
                        }

                        SbString.Append(" )");
                        #endregion
                    }
                    break;
                case UpdateOrAdd.Update:
                    {
                        #region MyRegion
                        for (int i = 0; i < LisZiDuan.Count; i++)
                        {
                            if (i == LisZiDuan.Count - 1)
                            {
                                SbString.Append(string.Format(" {0}={1}", LisZiDuan[i], LisZiDuanCanShu[i]));
                            }
                            else
                            {
                                SbString.Append(string.Format(" {0}={1}, ", LisZiDuan[i], LisZiDuanCanShu[i]));
                            }
                        }
                        if (LisZiDuanWhere.Count > 0 && LisZiDuanWhere.Count == LisZiDuanCanShuWhere.Count)
                        {
                            SbString.Append(" where ");

                            for (int i = 0; i < LisZiDuanWhere.Count; i++)
                            {
                                if (i == LisZiDuanWhere.Count - 1)
                                {
                                    SbString.Append(string.Format(" {0}={1}", LisZiDuanWhere[i], LisZiDuanCanShuWhere[i]));
                                }
                                else
                                {
                                    SbString.Append(string.Format(" {0}={1} and ", LisZiDuanWhere[i], LisZiDuanCanShuWhere[i]));
                                }
                            }
                        }
                        #endregion
                    }
                    break;
                default:
                    break;
            }
            string sql = SbString.ToString();
            Clear();
            return sql;

        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
