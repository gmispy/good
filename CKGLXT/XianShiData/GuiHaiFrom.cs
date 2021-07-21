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
    public partial class GuiHaiFrom : BaseFuFrom
    {
        private string DanHao = "";
        public GuiHaiFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }

        public void SetCanShu(string danhao,string wuping)
        {
            DanHao = danhao;
            this.textBox2.Text = wuping;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<int> vs = PanDuanID(DanHao);
            int count = ShuJuZhuanHuan.TryZhuanHuan(textBox1.Text,0);
            if (count<=0)
            {
                this.QiDongTiShiKuang("归还的数量不能为0");
                return;
            }
            int jiechu= ShuJuZhuanHuan.TryZhuanHuan(textBox3.Text, 0);
            if (count > jiechu)
            {
                this.QiDongTiShiKuang("归还的数量不能为大于借出的数量");
                return;
            }
            if (string.IsNullOrEmpty(textBox6.Text) || textBox6.Text.Length != 6)
            {
                this.QiDongTiShiKuang("借出人工号不能为空，只能为6位数");
                return;
            }
            int chazhi = count - jiechu;
            if (chazhi == 0)
            {
                GaiBianZhuanTai(vs[0], count, vs[1]);
                QingHai(this.textBox6.Text);
            }
            else
            {
                GaiBianZhuanTai(vs[0], count, vs[1]);
                HaiYou(this.textBox6.Text, count, jiechu);
            }
            this.DialogResult = DialogResult.OK;
        }

        private List<int> PanDuanID(string danhao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao='{0}'", danhao));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            Dictionary<string, object> yuangong = DanLiFanWenDB.Cerate().GetDuoGeShu(sql, new List<string>() { "HWKYCount", "HWJieChuCount" });
            if (yuangong != null)
            {
                try
                {
                    List<int> zhi = new List<int>();
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWKYCount"], 0));
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWJieChuCount"], 0));
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

        private void GaiBianZhuanTai(int keyongshuliang, int guihai, int yiqianjiechu)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                orUpdate.ZengJiaSql("HWKYCount", keyongshuliang + guihai, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("HWJieChuCount", yiqianjiechu - guihai, ZiFuOrInt.Int);
                orUpdate.Where("HWDanHao", DanHao, ZiFuOrInt.ZiFu);

                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }

        private void QingHai(string gonghao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<JieChuBiao>());
               // orUpdate.ZengJiaSql("JCCount", 0, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("JCState", 2, ZiFuOrInt.Int);
              
                orUpdate.ZengJiaSql("JCGHTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ZiFuOrInt.ZiFu);                          
                orUpdate.Where("HWDanHaoJ", DanHao, ZiFuOrInt.ZiFu);
                orUpdate.Where("JCGongHao", gonghao, ZiFuOrInt.ZiFu);
                orUpdate.Where("JCState", 1, ZiFuOrInt.Int);
                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }

        private void HaiYou(string gonghao,int guihaishu,int yiqianshu)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<JieChuBiao>());
                orUpdate.ZengJiaSql("JCCount", yiqianshu- guihaishu, ZiFuOrInt.Int);
               
                orUpdate.Where("HWDanHaoJ", DanHao, ZiFuOrInt.ZiFu);
                orUpdate.Where("JCGongHao", gonghao, ZiFuOrInt.ZiFu);
                orUpdate.Where("JCState", 1, ZiFuOrInt.Int);
                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<JieChuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHaoJ='{0}'", DanHao));
            tiaojian.Add(string.Format("JCGongHao='{0}'", textBox6.Text));
            tiaojian.Add(string.Format("JCState=1"));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<JieChuBiao>(), tiaojian);
            Dictionary<string, object> yuangong = DanLiFanWenDB.Cerate().GetDuoGeShu(sql, new List<string>() { "JCCount", "JCBuMen", "JCRen" });
            if (yuangong!=null)
            {
                textBox3.Text = yuangong["JCCount"].ToString();
                textBox5.Text = yuangong["JCBuMen"].ToString();
                textBox4.Text = yuangong["JCRen"].ToString();
            }
        }
    }
}
