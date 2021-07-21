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
    public partial class XianShiShuJu : BaseFuFrom
    {
        YuanGongBiao Yuangong;
        public XianShiShuJu(YuanGongBiao yuanGongBiao)
        {
            InitializeComponent();
            this.QuXiaoBiaoTi();
            Yuangong = yuanGongBiao;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HuWuBiao huWuBiao = new HuWuBiao();       
           RuKuFrom ruKuFrom = new RuKuFrom();
            ruKuFrom.SetCanShu(huWuBiao,false);
            if (ruKuFrom.ShowDialog(this) == DialogResult.OK)
            {
                this.dataGrid1.Rows.Clear();
                this.Waiting(ShuaXin, "正在加载数据...", this);
            }
        }


        private void ShuaXin()
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();         
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            List<HuWuBiao> Lis = DanLiFanWenDB.Cerate().GetLisT<HuWuBiao>(sql);
            if (Lis.Count>0)
            {
                this._Controlinkove.FanXingGaiBing(()=> {
                    for (int i = 0; i < Lis.Count; i++)
                    {
                        PaiXie(Lis[i]);
                    }
                });
               
            }
            int yuangong = DanLiFanWenDB.Cerate().GetCount(sql);
            
        }

        private void PaiXie(HuWuBiao ruKuFrom)
        {
            int index = this.dataGrid1.Rows.Add();
            this.dataGrid1.Rows[index].Cells[0].Value = ruKuFrom.HWDanHao;
            this.dataGrid1.Rows[index].Cells[1].Value = ruKuFrom.HWName;
            this.dataGrid1.Rows[index].Cells[2].Value = ruKuFrom.HWCount;
            this.dataGrid1.Rows[index].Cells[3].Value = ruKuFrom.HWJieChuCount;
            this.dataGrid1.Rows[index].Cells[4].Value = ruKuFrom.HWKYCount;

            this.dataGrid1.Rows[index].Cells[5].Value = ruKuFrom.HWWHCount;
            this.dataGrid1.Rows[index].Cells[6].Value = ruKuFrom.HWBFCount;
            this.dataGrid1.Rows[index].Cells[7].Value = ruKuFrom.HWBYYCount;
            this.dataGrid1.Rows[index].Cells[8].Value = ruKuFrom.HWTime;
            this.dataGrid1.Rows[index].Cells[9].Value = ruKuFrom.HWWeiZhi;      
          
                   
        }

        private void dataGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Right)
            {
               
                this.借出ToolStripMenuItem.Visible = false;
                this.删除ToolStripMenuItem.Visible = false;
                this.查看ToolStripMenuItem.Visible = false;
                this.还回ToolStripMenuItem.Visible = false;
               
                this.查看借出ToolStripMenuItem.Visible = false;
                this.待维修ToolStripMenuItem.Visible = false;
                this.报废ToolStripMenuItem.Visible = false;
                this.维护入库ToolStripMenuItem.Visible = false;
                if (this.dataGrid1.SelectedRows.Count>0)
                {
                    if (Yuangong.YGIsZhiWei == 2)
                    {
                      
                        this.借出ToolStripMenuItem.Visible = true;
                        this.删除ToolStripMenuItem.Visible = true;
                        this.查看ToolStripMenuItem.Visible = true;
                        this.还回ToolStripMenuItem.Visible = true;
                        
                        this.查看借出ToolStripMenuItem.Visible = true;
                        this.待维修ToolStripMenuItem.Visible = true;
                        this.报废ToolStripMenuItem.Visible = true;
                        this.维护入库ToolStripMenuItem.Visible = true;
                    }
                    else if (Yuangong.YGIsZhiWei == 1)
                    {                        
                        this.借出ToolStripMenuItem.Visible = true;                     
                        this.查看ToolStripMenuItem.Visible = true;
                        this.还回ToolStripMenuItem.Visible = true;
                      
                        this.查看借出ToolStripMenuItem.Visible = true;
                        this.维护入库ToolStripMenuItem.Visible = true;
                    }
                    this.contextMenuStrip1.Tag = this.dataGrid1.SelectedRows[0];
                    this.contextMenuStrip1.Show(this.dataGrid1,e.X,e.Y);
                }
            }
        }

       

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                //DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
               
                //int id = ShuJuZhuanHuan.TryZhuanHuan(row.Cells[0].Value, -1);
               
                //ChaKanYuYueFrom chaKanYuYueFrom = new ChaKanYuYueFrom();
                //chaKanYuYueFrom.SetCanShu(id);
                //chaKanYuYueFrom.ShowDialog(this);
            }
          
          
        }

      
    
        private void button4_Click(object sender, EventArgs e)
        {
            this.dataGrid1.Rows.Clear();
            this.Waiting(ShuaXin, "正在加载数据...", this);
        }

        private void 还回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                HuWuBiao huWuBiao = new HuWuBiao();
                huWuBiao.HWDanHao = row.Cells[0].Value.ToString();
                GuiHaiFrom chaZhaoFrom = new GuiHaiFrom();
                chaZhaoFrom.SetCanShu(huWuBiao.HWDanHao, row.Cells[1].Value.ToString());
                if (chaZhaoFrom.ShowDialog(this)==DialogResult.OK)
                {
                    this.dataGrid1.Rows.Clear();
                    this.Waiting(ShuaXin, "正在加载数据...", this);
                }
            }
        }

        private void 借出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                HuWuBiao huWuBiao = new HuWuBiao();
                huWuBiao.HWDanHao = row.Cells[0].Value.ToString();
                huWuBiao.HWName = row.Cells[1].Value.ToString();
               
                if (huWuBiao.HWZhuanTai == 1 || huWuBiao.HWZhuanTai == 5)
                {
                    JieChuCaoZuoFrom ruKuFrom = new JieChuCaoZuoFrom();
                    ruKuFrom.SetCanShu(huWuBiao);
                    if (ruKuFrom.ShowDialog(this) == DialogResult.OK)
                    {
                        this.dataGrid1.Rows.Clear();
                        this.Waiting(ShuaXin, "正在加载数据...", this);
                    }
                }
                else
                {
                    this.QiDongTiShiKuang("货物已经借出 或者报废");
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                HuWuBiao huWuBiao = new HuWuBiao();
                huWuBiao.HWDanHao = row.Cells[0].Value.ToString();
                huWuBiao.HWKYCount =ShuJuZhuanHuan.TryZhuanHuan( row.Cells[4].Value.ToString(),0);
                huWuBiao.HWJieChuCount = ShuJuZhuanHuan.TryZhuanHuan(row.Cells[3].Value.ToString(), 0);
                huWuBiao.HWCount = ShuJuZhuanHuan.TryZhuanHuan(row.Cells[2].Value.ToString(), 0);
               
                if (huWuBiao.HWJieChuCount==0)
                {
                    ShanChuYuYue(huWuBiao.HWDanHao);
                    this.dataGrid1.Rows.Clear();
                    this.Waiting(ShuaXin, "正在加载数据...", this);
                }
                else
                {
                    this.QiDongTiShiKuang("货物已经借出");
                }
            }
        }

        private void ShanChuYuYue(string huowuid)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();

            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHao='{0}'", huowuid));


            string sql = sqlYuJuPingJie.DeleteSql(sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            int yuangong = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            JieChuChaKanFrom chaKanYuYueFrom = new JieChuChaKanFrom();
            chaKanYuYueFrom.SetCanShu(-1,"");
            chaKanYuYueFrom.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChaZhaoFrom chaZhaoFrom = new ChaZhaoFrom();
            if (chaZhaoFrom.ShowDialog(this)==DialogResult.OK)
            {
                this.dataGrid1.Rows.Clear();
                this.Waiting(()=> { ShuaXinMohu(chaZhaoFrom.MingCheng); }, "正在加载数据...", this);
            }
        }

        private void ShuaXinMohu(string nam)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<HuWuBiao>();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWName like '%{0}%'", nam));
            string sql = sqlYuJuPingJie.SelectSql(ziduan, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>(), tiaojian);
            List<HuWuBiao> Lis = DanLiFanWenDB.Cerate().GetLisT<HuWuBiao>(sql);
            if (Lis.Count > 0)
            {
                this._Controlinkove.FanXingGaiBing(() => {
                    for (int i = 0; i < Lis.Count; i++)
                    {
                        PaiXie(Lis[i]);
                    }
                });

            }
            int yuangong = DanLiFanWenDB.Cerate().GetCount(sql);

        }

        private void 待维修ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                HuWuBiao huWuBiao = new HuWuBiao();
                huWuBiao.HWDanHao = row.Cells[0].Value.ToString();
                huWuBiao.HWName = row.Cells[1].Value.ToString();
                DaiWeiHuFrom daiWeiHuFrom = new DaiWeiHuFrom();
                daiWeiHuFrom.SetCanShu(huWuBiao.HWDanHao, huWuBiao.HWName);
                if (daiWeiHuFrom.ShowDialog(this)==DialogResult.OK)
                {
                    this.dataGrid1.Rows.Clear();
                    this.Waiting(ShuaXin, "正在加载数据...", this);
                }
            
            }
        }

        private void 报废ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                HuWuBiao huWuBiao = new HuWuBiao();
                huWuBiao.HWDanHao = row.Cells[0].Value.ToString();
                huWuBiao.HWName = row.Cells[1].Value.ToString();
                BaoFeiFrom daiWeiHuFrom = new BaoFeiFrom();
                daiWeiHuFrom.SetCanShu(huWuBiao.HWDanHao, huWuBiao.HWName);
                if (daiWeiHuFrom.ShowDialog(this) == DialogResult.OK)
                {
                    this.dataGrid1.Rows.Clear();
                    this.Waiting(ShuaXin, "正在加载数据...", this);
                }
            }
        }

        private void XianShiShuJu_Load(object sender, EventArgs e)
        {
            this.dataGrid1.Rows.Clear();
            this.Waiting(ShuaXin, "正在加载数据...", this);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 维护入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                HuWuBiao huWuBiao = new HuWuBiao();
                huWuBiao.HWDanHao = row.Cells[0].Value.ToString();
                huWuBiao.HWName = row.Cells[1].Value.ToString();
                RuKuFrom daiWeiHuFrom = new RuKuFrom();
                daiWeiHuFrom.SetCanShu(huWuBiao,true);
                if (daiWeiHuFrom.ShowDialog(this) == DialogResult.OK)
                {
                    this.dataGrid1.Rows.Clear();
                    this.Waiting(ShuaXin, "正在加载数据...", this);
                }

            }
        }

        private void 查看借出ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;

                string bianhao = row.Cells[0].Value.ToString();

                JieChuChaKanFrom chaKanYuYueFrom = new JieChuChaKanFrom();
                chaKanYuYueFrom.SetCanShu(1, bianhao);
                chaKanYuYueFrom.ShowDialog(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GuiHaiJiLuFrom chaKanYuYueFrom = new GuiHaiJiLuFrom();
            chaKanYuYueFrom.SetCanShu();
            chaKanYuYueFrom.ShowDialog(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Yuangong.YGIsZhiWei==2)
            {
                SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
                List<string> tiaojian = new List<string>();
                tiaojian.Add(string.Format("JCState=2"));
                tiaojian.Add(string.Format("JCGHTime<'{0}'", DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss")));
                string sql = sqlYuJuPingJie.DeleteSql(sqlYuJuPingJie.GetBiaoMing<JieChuBiao>(), tiaojian);
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
            }
        }
    }
}
