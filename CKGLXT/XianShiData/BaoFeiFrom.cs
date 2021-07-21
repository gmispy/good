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
using GongJuJiHe.ShuJuZhuanHuanGJ;

namespace CKGLXT.XianShiData
{
    public partial class BaoFeiFrom :BaseFuFrom
    {
        private string DanHao = "";
        public BaoFeiFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        public void SetCanShu(string danhao, string wuping)
        {
            DanHao = danhao;
            this.textBox2.Text = wuping;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> shuju = PanDuanID(DanHao);
            int daixiu = ShuJuZhuanHuan.TryZhuanHuan(this.textBox6.Text, 0);
            if (daixiu <= 0)
            {
                this.QiDongTiShiKuang("请输入报废数量");
                return;
            }
            if (daixiu > shuju[0])
            {
                this.QiDongTiShiKuang("输入报废数量大于可用数量");
                return;
            }
            GaiBianZhuanTai(shuju[0], daixiu, shuju[1]);
            this.DialogResult = DialogResult.OK;
        }

        private void GaiBianZhuanTai(int keyongshuliang, int daixiu, int yiqianjiechu)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                orUpdate.ZengJiaSql("HWKYCount", keyongshuliang - daixiu, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("HWBFCount", yiqianjiechu + daixiu, ZiFuOrInt.Int);
                orUpdate.Where("HWDanHao", DanHao, ZiFuOrInt.ZiFu);

                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }

        private List<int> PanDuanID(string danhao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao='{0}'", danhao));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            Dictionary<string, object> yuangong = DanLiFanWenDB.Cerate().GetDuoGeShu(sql, new List<string>() { "HWKYCount", "HWBFCount" });
            if (yuangong != null)
            {
                try
                {
                    List<int> zhi = new List<int>();
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWKYCount"], 0));
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWBFCount"], 0));
                    return zhi;
                }
                catch
                {


                }

            }
            else
            {

            }
            return new List<int>() { 0, 0 };
        }
    }
}
