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
    public partial class YuYueCaoZuoFrom : BaseFuFrom
    {

        private bool BaoCunCheng = false;
        public YuYueCaoZuoFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }


        private void ShuaXin()
        {
            
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            List<HuWuBiao> Lis = DanLiFanWenDB.Cerate().GetLisT<HuWuBiao>(sql);
            if (Lis.Count > 0)
            {
                this._Controlinkove.FanXingGaiBing(() => {

                    this.comboBox1.DisplayMember = "HWName";
                    this.comboBox1.ValueMember = "HWDanHao";
                    this.comboBox1.DataSource = Lis;
                });

            }
          

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedValue!=null)
            {
                YuYueBiao yuYueBiao = new YuYueBiao();
                yuYueBiao.HWDanHaoY = this.comboBox1.SelectedValue.ToString();
                yuYueBiao.YuBuMen = textBox3.Text;
                yuYueBiao.YuCount = ShuJuZhuanHuan.TryZhuanHuan(textBox1.Text,0);
                yuYueBiao.YuGongHao = textBox4.Text;
                yuYueBiao.YuRen = textBox5.Text;
                yuYueBiao.YuTime = this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
                List<int> vs = PanDuanID(yuYueBiao.HWDanHaoY);
                if (yuYueBiao.YuCount> vs[0])
                {
                    this.QiDongTiShiKuang("预约数量大于可用数量");
                    return;
                }
                if (yuYueBiao.YuCount <=0)
                {
                    this.QiDongTiShiKuang("预约数量小于0");
                    return;
                }
                if (yuYueBiao.YuGongHao=="")
                {
                    this.QiDongTiShiKuang("预约工卡号没有");
                    return;
                }
                BaoCun(yuYueBiao, vs);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void BaoCun(YuYueBiao huWuBiao, List<int> count1)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();                  
            int jiechu = JiXuJie(huWuBiao);
            if (jiechu > 0)
            {
                using (AddOrUpdate orUpdate = new AddOrUpdate())
                {
                    orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<YuYueBiao>());
                   
                    orUpdate.ZengJiaSql("YuBuMen", huWuBiao.YuBuMen, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("YuGongHao", huWuBiao.YuGongHao, ZiFuOrInt.ZiFu);

                    orUpdate.ZengJiaSql("YuCount", huWuBiao.YuCount + jiechu, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("YuRen", huWuBiao.YuRen, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("YuTime", huWuBiao.YuTime, ZiFuOrInt.ZiFu);

                    orUpdate.Where("HWDanHaoY", huWuBiao.HWDanHaoY, ZiFuOrInt.ZiFu);
                    orUpdate.Where("YuGongHao", huWuBiao.YuGongHao, ZiFuOrInt.ZiFu);
                    string sql = orUpdate.GetSQLString();
                    int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                    if (count > 0)
                    {
                        GaiBianZhuanTai(count1[0], huWuBiao.YuCount, count1[1], huWuBiao.HWDanHaoY);
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
                    orUpdate.SetBiaoZhi(UpdateOrAdd.Add, sqlYuJuPingJie.GetBiaoMing<YuYueBiao>());
                    orUpdate.ZengJiaSql("HWDanHaoY", huWuBiao.HWDanHaoY, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("YuBuMen", huWuBiao.YuBuMen, ZiFuOrInt.ZiFu);
                  

                    orUpdate.ZengJiaSql("YuCount", huWuBiao.YuCount, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("YuRen", huWuBiao.YuRen, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("YuTime", huWuBiao.YuTime, ZiFuOrInt.ZiFu);

                   
                    orUpdate.ZengJiaSql("YuGongHao", huWuBiao.YuGongHao, ZiFuOrInt.ZiFu);
                    string sql = orUpdate.GetSQLString();
                    int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                    if (count > 0)
                    {
                        GaiBianZhuanTai(count1[0], huWuBiao.YuCount, count1[1], huWuBiao.HWDanHaoY);
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

        private int JiXuJie(YuYueBiao huWuBiao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<YuYueBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHaoY='{0}'", huWuBiao.HWDanHaoY));
            tiaojian.Add(string.Format("YuGongHao='{0}'", huWuBiao.YuGongHao));
           
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<YuYueBiao>(), tiaojian);
            object yuangong = DanLiFanWenDB.Cerate().GetDanGeShu(sql, "YuCount");
            if (yuangong != null)
            {
                return ShuJuZhuanHuan.TryZhuanHuan(yuangong, 0);
            }
            return 0;
        }


        private List<int> PanDuanID(string danhao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao='{0}'", danhao));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            Dictionary<string, object> yuangong = DanLiFanWenDB.Cerate().GetDuoGeShu(sql, new List<string>() { "HWKYCount", "HWBYYCount" });
            if (yuangong != null)
            {
                try
                {
                    List<int> zhi = new List<int>();
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWKYCount"], 0));
                    zhi.Add(ShuJuZhuanHuan.TryZhuanHuan(yuangong["HWBYYCount"], 0));
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
        private void YuYueCaoZuoFrom_Load(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = null;
            this.Waiting(()=> { ShuaXin(); },"正在加载数据...",this);
        }

        private void GaiBianZhuanTai(int keyongshuliang, int jiechushuliang, int yiqianjiechu,string danhao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                orUpdate.ZengJiaSql("HWKYCount", keyongshuliang - jiechushuliang, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("HWBYYCount", yiqianjiechu + jiechushuliang, ZiFuOrInt.Int);
                orUpdate.Where("HWDanHao", danhao, ZiFuOrInt.ZiFu);

                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }

    }
}
