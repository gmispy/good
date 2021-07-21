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
    public partial class JieChuCaoZuoFrom : BaseFuFrom
    {
        HuWuBiao HuWuBiao;
        private bool BaoCunCheng = false;
        public JieChuCaoZuoFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        public void SetCanShu(HuWuBiao huWuBiao)
        {
            HuWuBiao = huWuBiao;
            this.textBox1.Text = huWuBiao.HWName;
            this.textBox2.Text = huWuBiao.HWType;

            this.textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JieChuBiao jieChuBiao = new JieChuBiao();
            jieChuBiao.JCHWID = 0;
            jieChuBiao.HWDanHaoJ = HuWuBiao.HWDanHao;
            jieChuBiao.JCRen = this.textBox4.Text;
            jieChuBiao.JCState = 1;
            jieChuBiao.JCCount = ShuJuZhuanHuan.TryZhuanHuan(this.textBox3.Text, 0);
            jieChuBiao.JCGongHao = this.textBox6.Text;
            jieChuBiao.JCBuMen = this.textBox5.Text;
            jieChuBiao.JCTime = this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(jieChuBiao.JCRen))
            {
                this.QiDongTiShiKuang("借出人不能为空");
                return;
            }
            if (string.IsNullOrEmpty(jieChuBiao.JCGongHao)|| jieChuBiao.JCGongHao.Length!=6)
            {
                this.QiDongTiShiKuang("借出人工号不能为空，只能为6位数");
                return;
            }
            this.Waiting(()=> { BaoCun(jieChuBiao); },"正在保存...",this);
            if (BaoCunCheng)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private List<int> PanDuanID()
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao='{0}'", HuWuBiao.HWDanHao));
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
            return new List<int>() { 0,0};
        }

        private int JiXuJie(JieChuBiao huWuBiao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<JieChuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHaoJ='{0}'", huWuBiao.HWDanHaoJ));
            tiaojian.Add(string.Format("JCGongHao='{0}'", huWuBiao.JCGongHao));
            tiaojian.Add(string.Format("JCState=1"));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<JieChuBiao>(), tiaojian);
             object yuangong = DanLiFanWenDB.Cerate().GetDanGeShu(sql,  "JCCount");
            if (yuangong!=null)
            {
                return ShuJuZhuanHuan.TryZhuanHuan(yuangong,0);
            }
            return 0;
        }


        private void BaoCun(JieChuBiao huWuBiao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<int> count1 = PanDuanID();
            if (huWuBiao.JCCount > count1[0])
            {
                this.QiDongTiShiKuang("库存不足");
                return;
            }
            int jiechu = JiXuJie(huWuBiao);
            if (jiechu > 0)
            {
                using (AddOrUpdate orUpdate = new AddOrUpdate())
                {
                    orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<JieChuBiao>());
                    orUpdate.ZengJiaSql("JCHWID", huWuBiao.JCHWID, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("JCRen", huWuBiao.JCRen, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("JCTime", huWuBiao.JCTime, ZiFuOrInt.ZiFu);
                   
                    orUpdate.ZengJiaSql("JCCount", huWuBiao.JCCount+ jiechu, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("JCBuMen", huWuBiao.JCBuMen, ZiFuOrInt.ZiFu);
                    orUpdate.Where("JCGongHao", huWuBiao.JCGongHao, ZiFuOrInt.ZiFu);
                    orUpdate.Where("JCState",1, ZiFuOrInt.Int);
                    orUpdate.Where("HWDanHaoJ", huWuBiao.HWDanHaoJ, ZiFuOrInt.ZiFu);
                    string sql = orUpdate.GetSQLString();
                    int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                    if (count > 0)
                    {
                        GaiBianZhuanTai(count1[0], huWuBiao.JCCount, count1[1]);
                        YuYueXiFang(huWuBiao.HWDanHaoJ, huWuBiao.JCGongHao);
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
                    orUpdate.SetBiaoZhi(UpdateOrAdd.Add, sqlYuJuPingJie.GetBiaoMing<JieChuBiao>());
                    orUpdate.ZengJiaSql("JCHWID", huWuBiao.JCHWID, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("JCRen", huWuBiao.JCRen, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("JCTime", huWuBiao.JCTime, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("JCState", huWuBiao.JCState, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("JCCount", huWuBiao.JCCount, ZiFuOrInt.Int);
                    orUpdate.ZengJiaSql("JCBuMen", huWuBiao.JCBuMen, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("JCGongHao", huWuBiao.JCGongHao, ZiFuOrInt.ZiFu);
                    orUpdate.ZengJiaSql("HWDanHaoJ", huWuBiao.HWDanHaoJ, ZiFuOrInt.ZiFu);
                    string sql = orUpdate.GetSQLString();
                    int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                    if (count > 0)
                    {
                        GaiBianZhuanTai(count1[0], huWuBiao.JCCount, count1[1]);
                        YuYueXiFang(huWuBiao.HWDanHaoJ, huWuBiao.JCGongHao);
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

       

        private void  GaiBianZhuanTai(int keyongshuliang,int jiechushuliang,int yiqianjiechu)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                orUpdate.ZengJiaSql("HWKYCount", keyongshuliang- jiechushuliang, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("HWJieChuCount", yiqianjiechu+ jiechushuliang, ZiFuOrInt.Int);
                orUpdate.Where("HWDanHao", HuWuBiao.HWDanHao, ZiFuOrInt.ZiFu);
               
                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
                
            }
        }

        private void YuYueXiFang(string danhao, string gonghao)
        {
            int shuliang = YouWuYuYue(danhao, gonghao);
            List<int> vs = PanDuanID();
            GaiBianYuYueZhuanTai(vs[0], shuliang);
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHaoY='{0}'", danhao));
            tiaojian.Add(string.Format("YuGongHao='{0}'", gonghao));
            string sql = sqlYuJuPingJie.DeleteSql(sqlYuJuPingJie.GetBiaoMing<YuYueBiao>(), tiaojian);
            int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
        }
        private void GaiBianYuYueZhuanTai(int keyongshuliang, int yuyueshuju)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                orUpdate.ZengJiaSql("HWKYCount", keyongshuliang + yuyueshuju, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("HWBYYCount", 0, ZiFuOrInt.Int);
                orUpdate.Where("HWDanHao", HuWuBiao.HWDanHao, ZiFuOrInt.ZiFu);

                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }
        private int YouWuYuYue(string danhao ,string gonghao)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<YuYueBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHaoY='{0}'", danhao));
            tiaojian.Add(string.Format("YuGongHao='{0}'", gonghao));
         
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<YuYueBiao>(), tiaojian);
            object yuangong = DanLiFanWenDB.Cerate().GetDanGeShu(sql, "YuCount");
            if (yuangong != null)
            {
                return ShuJuZhuanHuan.TryZhuanHuan(yuangong, 0);
            }
            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int yueyueshu = YouWuYuYue(HuWuBiao.HWDanHao, textBox6.Text);
            textBox7.Text = yueyueshu.ToString();
        }
    }
}
