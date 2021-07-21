using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKGLXT.DBFanWen
{
    public abstract class ABSSqlDBLianJie
    {
        /// <summary>
        /// 更新或者增加语句的执行，返回大于0说明更新成功，返回负数更新失败
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract int UpdateOrInsertdate(string sql);

        /// <summary>
        /// 更新或者增加语句的执行，返回大于0说明更新成功，返回负数更新失败
        /// </summary>
        /// <param name="sql"></param>
        ///  <param name="sql"></param>
        /// <returns></returns>
        public abstract int UpdateOrInsertdate(string sql, Dictionary<string, object> Parameter);
        /// <summary>
        /// 用于事务更新，返回true表示更新成功，否则失败
        /// </summary>
        /// <param name="sqllist"></param>
        /// <returns></returns>
        public abstract bool ShiWuUpdate(List<string> sqllist);

        /// <summary>
        ///  用于事务更新，返回true表示更新成功，否则失败
        /// </summary>
        /// <param name="sqllist"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public abstract bool ShiWuUpdate(List<string> sqllist, List<Dictionary<string, object>> Parameter);
        /// <summary>
        /// 获取Datable,失败则是null
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract DataTable GetDataTable(string sql);

        /// <summary>
        /// 获取Datable,失败则是null
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public abstract DataTable GetDataTable(string sql, Dictionary<string, object> Parameter);
        /// <summary>
        /// 获取josn，通过josn来转，没有就是空
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract string GetDataTableJosn(string sql);

        /// <summary>
        /// 获取josn，通过josn来转，没有就是空
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract string GetDataTableJosn(string sql, Dictionary<string, object> Parameter);
        /// <summary>
        /// 获取一条语句的数量
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract int GetCount(string sql);
        /// <summary>
        /// 单独更新
        /// </summary>
        /// <param name="ziduanming"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduan"></param>
        /// <param name="ziduanshuju"></param>
        /// <param name="tiaojianshuju"></param>
        /// <returns></returns>
        public abstract int UpdateZiDuan(string ziduanming, string biaoming, string tiaojianziduan, object ziduanshuju, object tiaojianshuju);


        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="sPicPaht"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduanming"></param>
        /// <param name="ziduanming"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool XieRuTuPian(string sPicPaht, string biaoming, string tiaojianziduanming, string ziduanming, int id);

        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="sPicPaht"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduanming"></param>
        /// <param name="ziduanming"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool XieRuTuPian(string sPicPaht, string biaoming, string tiaojianziduanming, string ziduanming, string id);

        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="byteimage"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduanming"></param>
        /// <param name="ziduanming"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool XieRuTuPian(byte[] byteimage, string biaoming, string tiaojianziduanming, string ziduanming, string id);
        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="byteimage"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduanming"></param>
        /// <param name="ziduanming"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool XieRuTuPian(byte[] byteimage, string biaoming, string tiaojianziduanming, string ziduanming, int id);

        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="byteimage"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduanming"></param>
        /// <param name="ziduanming"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool XieRuTuPian(Image byteimage, string biaoming, string tiaojianziduanming, string ziduanming, string id);
        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="byteimage"></param>
        /// <param name="biaoming"></param>
        /// <param name="tiaojianziduanming"></param>
        /// <param name="ziduanming"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool XieRuTuPian(Image byteimage, string biaoming, string tiaojianziduanming, string ziduanming, int id);
      

        /// <summary>
        /// 根据sql语句查询字段的值，没有返回null
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ziduan"></param>
        /// <returns></returns>
        public abstract object GenJuSqlChaXunZhi(string sql, string ziduan);
        /// <summary>
        /// 根据sql查询一系列的字段的值key为字段，values为值，没有返回null
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ziduan"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> GenJuSqlChaXunZhi(string sql, List<string> ziduan);
        /// <summary>
        /// 根据sql查询一系列的字段的值，没有返回null
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ziduan"></param>
        /// <returns></returns>
        public abstract List<object> GenJuSqlLisChaXunZhi(string sql, string ziduan);
    }
}
