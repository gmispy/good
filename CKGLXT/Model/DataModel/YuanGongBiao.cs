using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKGLXT.Model.DataModel
{
    public class YuanGongBiao
    {
        public int YGID { get; set; }
        public string YGDLM { get; set; }
        public string YGMM { get; set; }
        public string YGDXM { get; set; }
        public int YGIsUse { get; set; }
        /// <summary>
        /// 职位 1表示普通职员没有删除权限 2表示管理员 有删除权限
        /// </summary>
        public int YGIsZhiWei { get; set; }
       

       
    }
}
