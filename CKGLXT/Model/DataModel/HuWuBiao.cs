using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKGLXT.Model.DataModel
{
    public class HuWuBiao
    {
        public string HWDanHao { get; set; }
        public string HWName { get; set; }
        public string HWType { get; set; }
        public int HWZhuanTai { get; set; }
        public int HWKYCount { get; set; }
        public int HWJieChuCount { get; set; }
        public int HWWHCount { get; set; }
        public int HWBFCount { get; set; }
        public int HWBYYCount { get; set; }
        public int HWCount { get; set; }
        public string HWWeiZhi { get; set; }

        public string HWTime { get; set; }


        public HuWuBiao()
        {
          
            HWName = "";
            HWType = "其他类";
            HWZhuanTai = 1;
            HWWeiZhi = "";
            HWTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
