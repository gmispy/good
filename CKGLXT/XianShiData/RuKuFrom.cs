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
    public partial class RuKuFrom : BaseFuFrom
    {
        private bool BaoCunCheng = false;
        private HuWuBiao HuWuBiao = new HuWuBiao();
        private bool weihuruku = false;
        public RuKuFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
            this.comboBox1.Items.Clear();
            foreach (var item in StaicLei.HuWuLeiXing.Keys)
            {
                this.comboBox1.Items.Add(StaicLei.HuWuLeiXing[item]);
            }
            if (this.comboBox1.Items.Count > 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
        }

        public void SetCanShu(HuWuBiao huWuBiao,bool isweihuruku)
        {
            HuWuBiao = huWuBiao;
            this.textBox1.Text = HuWuBiao.HWName;
            for (int i = 0; i < this.comboBox1.Items.Count; i++)
            {
                if (this.comboBox1.Items[i].ToString().Equals(huWuBiao.HWType))
                {
                    this.comboBox1.SelectedIndex = i;
                    break;
                }
            }
            this.textBox3.Text = HuWuBiao.HWTime;
            textBox4.Text = HuWuBiao.HWWeiZhi;
            weihuruku = false;
            if (isweihuruku)
            {
                textBox5.Text = HuWuBiao.HWDanHao;
                textBox5.Enabled = false;
                weihuruku = true;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            BaoCunCheng = false;
            HuWuBiao huWuBiao = new HuWuBiao();
            huWuBiao.HWDanHao = textBox5.Text;
            huWuBiao.HWName = this.textBox1.Text;
            huWuBiao.HWTime = textBox3.Text;
            huWuBiao.HWType = this.comboBox1.Text;
            huWuBiao.HWZhuanTai = HuWuBiao.HWZhuanTai;
            huWuBiao.HWWeiZhi = textBox4.Text;
            huWuBiao.HWCount = ShuJuZhuanHuan.TryZhuanHuan(textBox2.Text, 0);
            if (string.IsNullOrEmpty(huWuBiao.HWDanHao)|| huWuBiao.HWCount<=0)
            {
                this.QiDongTiShiKuang("物料编号不能为空,尽量是字母与数字,或者入库数量不能小于等于0");
                return;
            }
            this.Waiting(()=> { BaoCun(huWuBiao); },"正在保存数据...",this);
            if (BaoCunCheng)
            {
                this.DialogResult = DialogResult.OK;
            }
        }


        private bool PanDuanID(HuWuBiao huWuBiao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao={0}", huWuBiao.HWDanHao));
          
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            int yuangong = DanLiFanWenDB.Cerate().GetCount(sql);
            if (yuangong>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void BaoCun(HuWuBiao huWuBiao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            if (weihuruku == false)
            {
                if (PanDuanID(huWuBiao) == false)
                {
                    using (AddOrUpdate orUpdate = new AddOrUpdate())
                    {
                        orUpdate.SetBiaoZhi(UpdateOrAdd.Add, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                        orUpdate.ZengJiaSql("HWName", huWuBiao.HWName, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWTime", huWuBiao.HWTime, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWType", huWuBiao.HWType, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWZhuanTai", huWuBiao.HWZhuanTai, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWWeiZhi", huWuBiao.HWWeiZhi, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWDanHao", huWuBiao.HWDanHao, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWKYCount", huWuBiao.HWCount, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWJieChuCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWWHCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWBYYCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWBFCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWCount", huWuBiao.HWCount, ZiFuOrInt.Int);

                        string sql = orUpdate.GetSQLString();
                        int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                        if (count > 0)
                        {
                            this.QiDongTiShiKuang("保存成功");
                            BaoCunCheng = true;
                        }
                        else
                        {
                            this.QiDongTiShiKuang("保存失败");
                        }
                    }
                }
                else
                {
                    using (AddOrUpdate orUpdate = new AddOrUpdate())
                    {
                        orUpdate.SetBiaoZhi(UpdateOrAdd.Add, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                        orUpdate.ZengJiaSql("HWName", huWuBiao.HWName, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWTime", huWuBiao.HWTime, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWType", huWuBiao.HWType, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWZhuanTai", huWuBiao.HWZhuanTai, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWWeiZhi", huWuBiao.HWWeiZhi, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWDanHao", huWuBiao.HWDanHao, ZiFuOrInt.ZiFu);
                        orUpdate.ZengJiaSql("HWKYCount", huWuBiao.HWCount, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWJieChuCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWWHCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWBYYCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWBFCount", 0, ZiFuOrInt.Int);
                        orUpdate.ZengJiaSql("HWCount", huWuBiao.HWCount, ZiFuOrInt.Int);

                        string sql = orUpdate.GetSQLString();
                        int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                        if (count > 0)
                        {
                            this.QiDongTiShiKuang("保存成功");
                            BaoCunCheng = true;
                        }
                        else
                        {
                            this.QiDongTiShiKuang("保存失败");
                        }
                    }
                }
            }
            else
            {
                List<int> weihu = PanDuanID(huWuBiao.HWDanHao);
                using (AddOrUpdate orUpdate = new AddOrUpdate())
                {
                    orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                  
                    orUpdate.ZengJiaSql("HWKYCount", weihu[0] + huWuBiao.HWCount, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("HWWHCount", weihu[1] -huWuBiao.HWCount, ZiFuOrInt.Int);
                    orUpdate.Where("HWDanHao", huWuBiao.HWDanHao, ZiFuOrInt.ZiFu);

                    string sql = orUpdate.GetSQLString();
                    int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                    if (count > 0)
                    {
                        this.QiDongTiShiKuang("保存成功");
                        BaoCunCheng = true;
                    }
                    else
                    {
                        this.QiDongTiShiKuang("保存失败");
                    }
                }
            }
        }


        private List<int> PanDuanID(string danhao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao='{0}'", danhao));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            Dictionary<string, object> yuangong = DanLiFanWenDB.Cerate().GetDuoGeShu(sql, new List<string>() { "HWKYCount", "HWWHCount" });
            if (yuangong != null)
            {
                try
                {
                    List<int> zhi = new List<int>();
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWKYCount"], 0));
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWWHCount"], 0));
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
