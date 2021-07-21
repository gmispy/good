using GongJuJiHe.ShuJuZhuanHuanGJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKGLXT.DBFanWen
{
    public class DanLiFanWenDB
    {
        private ABSSqlDBLianJie _ABSSqlDBLianJie;
        #region 单利
        private static DanLiFanWenDB _LogTxt = null;

        private readonly static object _DuiXiang = new object();




        private DanLiFanWenDB()
        {
            _ABSSqlDBLianJie = new MySqlLianJieQi();

        }

        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static DanLiFanWenDB Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new DanLiFanWenDB();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        public T GetDanGeT<T>(string sql)
        {
            try
            {
                string josn = _ABSSqlDBLianJie.GetDataTableJosn(sql);
                List<T> list = new List<T>();
                list = ShuJuZhuanHuan.HuoQuJsonToShiTi<List<T>>(josn);
                if (list.Count > 0)
                {
                    return list[0];
                }

            }
            catch
            {
                return default(T);
            }
            return default(T);
        }

        public List<T> GetLisT<T>(string sql)
        {
            try
            {
                string josn = _ABSSqlDBLianJie.GetDataTableJosn(sql);
                List<T> list = new List<T>();
                list = ShuJuZhuanHuan.HuoQuJsonToShiTi<List<T>>(josn);

                return list;


            }
            catch
            {
                return new List<T>();
            }

        }


        public int GetCount(string sql)
        {
            return _ABSSqlDBLianJie.GetCount(sql);
        }

        public int UpdateOrAdd(string sql)
        {
            return _ABSSqlDBLianJie.UpdateOrInsertdate(sql);
        }
        public bool ShiWu(List<string> lissql)
        {
            return _ABSSqlDBLianJie.ShiWuUpdate(lissql);
        }

        public object GetDanGeShu(string sql,string ziduan)
        {
            return _ABSSqlDBLianJie.GenJuSqlChaXunZhi(sql, ziduan);
        }
        public List<object> GetLisShu(string sql, string ziduan)
        {
            return _ABSSqlDBLianJie.GenJuSqlLisChaXunZhi(sql, ziduan);
        }

        public Dictionary<string, object> GetDuoGeShu(string sql, List<string> ziduan)
        {
            return _ABSSqlDBLianJie.GenJuSqlChaXunZhi(sql, ziduan);
        }
    }
}
