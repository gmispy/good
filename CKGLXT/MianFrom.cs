using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CKGLXT.Model.DataModel;
using CKGLXT.XianShiData;
using FuChuanTiFrom;
using GongJuJiHe.WinFromDJ;

namespace CKGLXT
{
    public partial class MianFrom : BaseFuFrom
    {
        YuanGongBiao Yuangong;
        private JieJueChuangTiPingJie JieJueChuangTiPingJie = new JieJueChuangTiPingJie();
        public MianFrom(YuanGongBiao yuanGongBiao)
        {
            InitializeComponent();
            Yuangong = yuanGongBiao;
            label1.Text = string.Format("用户名:{0}", Yuangong.YGDXM);
            label2.Text = string.Format("权限:{0}", Yuangong.YGIsZhiWei==1?"普通":"管理员");
        }


        protected override void GuanBi()
        {
            System.Environment.Exit(0);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XianShiShuJu xianShiShuJu = new XianShiShuJu(Yuangong);
            FromDX fromDX = new FromDX();
            fromDX.DX_chuangti = xianShiShuJu;
            fromDX.DX_kongjian = this.panel4;
            fromDX.DX_mingcheng = "xianShiShuJu";
          
            JieJueChuangTiPingJie.OpenFrom(fromDX);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Yuangong.YGIsZhiWei == 2)
            {
                ChaKanYuYueFrom xianShiShuJu = new ChaKanYuYueFrom();
                FromDX fromDX = new FromDX();
                fromDX.DX_chuangti = xianShiShuJu;
                fromDX.DX_kongjian = this.panel4;
                fromDX.DX_mingcheng = "ChaKanYuYueFrom";

                JieJueChuangTiPingJie.OpenFrom(fromDX);
            }
            
        }

        private void MianFrom_Load(object sender, EventArgs e)
        {
            this.button2.Visible = false;
            if (Yuangong.YGIsZhiWei == 2)
            {
                this.button2.Visible = true;
            }
        }
    }
}
