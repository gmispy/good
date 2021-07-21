using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CKGLXT.DBFanWen;
using CKGLXT.Model.DataModel;
using FuChuanTiFrom;

namespace CKGLXT.XianShiData
{
    public partial class JieChuChaKanFrom : BaseFuFrom
    {
        public JieChuChaKanFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }

        /// <summary>
        /// id  为-1  差全部  大于1  差单个
        /// </summary>
        /// <param name="id"></param>
        public void SetCanShu(int id,string danhao)
        {
            this.dataGrid1.Rows.Clear();
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<JieChuZuHeModel>();

            List<string> biaoming = new List<string>();
            biaoming.Add(sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
            biaoming.Add(sqlYuJuPingJie.GetBiaoMing<JieChuBiao>());
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HuWuBiao.HWDanHao=JieChuBiao.HWDanHaoJ"));
            tiaojian.Add(string.Format("JCState=1"));
            if (id > 0)
            {
                tiaojian.Add(string.Format("HuWuBiao.HWDanHao='{0}'", danhao));
            }

            string sql = sqlYuJuPingJie.SelectSqlDuoBiaoChaXun(ziduan, biaoming, tiaojian);
            List<JieChuZuHeModel> Lis = DanLiFanWenDB.Cerate().GetLisT<JieChuZuHeModel>(sql);
            if (Lis.Count > 0)
            {
                for (int i = 0; i < Lis.Count; i++)
                {
                    PaiXie(Lis[i]);
                }

            }
           
        }

        private void PaiXie(JieChuZuHeModel ruKuFrom)
        {
            int index = this.dataGrid1.Rows.Add();
            this.dataGrid1.Rows[index].Cells[0].Value = ruKuFrom.HWDanHao;
            this.dataGrid1.Rows[index].Cells[1].Value = ruKuFrom.HWName;

            this.dataGrid1.Rows[index].Cells[2].Value = ruKuFrom.JCTime;
            this.dataGrid1.Rows[index].Cells[3].Value = ruKuFrom.JCRen;
            this.dataGrid1.Rows[index].Cells[4].Value = ruKuFrom.JCGongHao;
            this.dataGrid1.Rows[index].Cells[5].Value = ruKuFrom.JCCount;
            this.dataGrid1.Rows[index].Cells[6].Value = ruKuFrom.JCBuMen;
        }
    }
}
