using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKGLXT.DBFanWen
{
    public static class StaicLei
    {
        /// <summary>
        /// 货物状态
        /// </summary>
        public static Dictionary<int, string> HWZhuangTai = new Dictionary<int, string>();

        /// <summary>
        /// 货物类型
        /// </summary>
        public static Dictionary<int, string> HuWuLeiXing = new Dictionary<int, string>();

        static StaicLei()
        {
            HuWuLeiXing.Add(1,"其他类");
            HWZhuangTai.Add(1,"可用");
            HWZhuangTai.Add(2, "待维修");
            HWZhuangTai.Add(3, "报废");
            HWZhuangTai.Add(4, "借出");
            HWZhuangTai.Add(5, "预约");
        }

        public static string GetZhuanTai(int id)
        {
            if (HWZhuangTai.ContainsKey(id))
            {
                return HWZhuangTai[id];
            }
            return "";
        }
        public static int GetZhuanTaiID(string value)
        {
            foreach (var item in HWZhuangTai.Keys)
            {
                if (HWZhuangTai[item].Equals(value))
                {
                    return item;
                }
            }
            
            return 1;
        }
    }
}
