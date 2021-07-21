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
using GongJuJiHe.WinFromDJ;

namespace CKGLXT
{
    public partial class Login : BaseFuFrom
    {
        public YuanGongBiao Yuangong=new YuanGongBiao();
        public Login()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
            AnXiaShiJianToKongJian anXiaShiJianToKongJian = new AnXiaShiJianToKongJian();
            anXiaShiJianToKongJian.BangDingAnXiaToKongJian(this.textBox1, this.textBox2);
            anXiaShiJianToKongJian.BangDingAnXiaToKongJian(this.textBox2, this.button1);
        }
        protected override void GuanBi()
        {
            System.Environment.Exit(0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GuanBi();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string zhanghao = this.textBox1.Text;
            string mima = this.textBox2.Text;
            if (string.IsNullOrEmpty(zhanghao))
            {
                this.QiDongTiShiKuang("账号不能为空");
                return;
            }
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<YuanGongBiao>();          
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("YGDLM='{0}'", zhanghao));
            tiaojian.Add(string.Format("YGMM='{0}'", mima));
            tiaojian.Add(string.Format("YGIsUse={0}", 1));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<YuanGongBiao>(), tiaojian);
            YuanGongBiao yuangong= DanLiFanWenDB.Cerate().GetDanGeT<YuanGongBiao>(sql);
            if (yuangong == null)
            {
                this.QiDongTiShiKuang("该用户不存在");
            }
            else
            {
                Yuangong = yuangong;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            this.textBox1.Select();
        }
    }
}
