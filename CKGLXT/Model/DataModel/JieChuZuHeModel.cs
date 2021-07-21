using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKGLXT.Model.DataModel
{
    public class JieChuZuHeModel
    {
        public int JCID { get; set; }
        public int JCCount { get; set; }
        public string JCRen { get; set; }
        public string JCTime { get; set; }
        /// <summary>
        /// 借出状态 1表示借出 2表示归还
        /// </summary>
        public int JCState { get; set; }

      
        public string HWName { get; set; }
        public string HWType { get; set; }

        public string JCBuMen { get; set; }
        public string JCGongHao { get; set; }

        public string HWDanHao { get; set; }
        public string JCGHTime { get; set; }
    }
}
