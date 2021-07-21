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
    public partial class ChaKanYuYueFrom : BaseFuFrom
    {
        public ChaKanYuYueFrom()
        {
            InitializeComponent();
            this.QuXiaoBiaoTi();
        }

        /// <summary>
        /// id  为-1  差全部  大于1  差单个
        /// </summary>
        /// <param name="id"></param>
        private  void SetCanShu()
        {
            this.dataGrid1.Rows.Clear();
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> ziduan = sqlYuJuPingJie.GetZiDuan<YuYueZuHeModel>();

            List<string> biaoming = new List<string>();
            biaoming.Add(sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
            biaoming.Add(sqlYuJuPingJie.GetBiaoMing<YuYueBiao>());
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("YuYueBiao.HWDanHaoY=HuWuBiao.HWDanHao"));
          
            string sql = sqlYuJuPingJie.SelectSqlDuoBiaoChaXun(ziduan, biaoming, tiaojian);
            List<YuYueZuHeModel> Lis = DanLiFanWenDB.Cerate().GetLisT<YuYueZuHeModel>(sql);
            if (Lis.Count > 0)
            {
                for (int i = 0; i < Lis.Count; i++)
                {
                    PaiXie(Lis[i]);
                }

            }
           
        }

        private void PaiXie(YuYueZuHeModel ruKuFrom)
        {
            int index = this.dataGrid1.Rows.Add();
            this.dataGrid1.Rows[index].Cells[0].Value = ruKuFrom.HWDanHao;
            this.dataGrid1.Rows[index].Cells[1].Value = ruKuFrom.HWName;
        
            this.dataGrid1.Rows[index].Cells[2].Value = ruKuFrom.YuRen;
            this.dataGrid1.Rows[index].Cells[3].Value = ruKuFrom.YuBuMen;
            this.dataGrid1.Rows[index].Cells[4].Value = ruKuFrom.YuGongHao;
            this.dataGrid1.Rows[index].Cells[5].Value = ruKuFrom.YuTime;
            this.dataGrid1.Rows[index].Cells[6].Value = ruKuFrom.YuCount;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            YuYueCaoZuoFrom yuYueCaoZuoFrom = new YuYueCaoZuoFrom();
            if (yuYueCaoZuoFrom.ShowDialog(this)==DialogResult.OK)
            {
                SetCanShu();
            }
        }

        private void ChaKanYuYueFrom_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void dataGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Right)
            {
                if (this.dataGrid1.SelectedRows.Count>0)
                {
                    this.contextMenuStrip1.Tag = this.dataGrid1.SelectedRows[0];
                    this.contextMenuStrip1.Show(this.dataGrid1,e.X,e.Y);
                }
            }
        }

        private void 还回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is DataGridViewRow)
            {
                DataGridViewRow row = this.contextMenuStrip1.Tag as DataGridViewRow;
                string wuliaohao = row.Cells[0].Value.ToString();
                string gonghao = row.Cells[4].Value.ToString();
                YuYueXiFang(wuliaohao, gonghao);
                SetCanShu();
            }
        }

   
        private void YuYueXiFang(string danhao, string gonghao)
        {
            int shuliang = YouWuYuYue(danhao, gonghao);
            List<int> vs = PanDuanID(danhao);
            GaiBianYuYueZhuanTai(danhao,vs[0], shuliang);
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            List<string> tiaojian = new List<string>();
            tiaojian.Add(string.Format("HWDanHaoY='{0}'", danhao));
            tiaojian.Add(string.Format("YuGongHao='{0}'", gonghao));
            string sql = sqlYuJuPingJie.DeleteSql(sqlYuJuPingJie.GetBiaoMing<YuYueBiao>(), tiaojian);
            int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);
        }
        private void GaiBianYuYueZhuanTai(string danhao,int keyongshuliang, int yuyueshuju)
        {
            SqlYuJuPingJie sqlYuJuPingJie = new SqlYuJuPingJie();
            using (AddOrUpdate orUpdate = new AddOrUpdate())
            {
                orUpdate.SetBiaoZhi(UpdateOrAdd.Update, sqlYuJuPingJie.GetBiaoMing<HuWuBiao>());
                orUpdate.ZengJiaSql("HWKYCount", keyongshuliang + yuyueshuju, ZiFuOrInt.Int);
                orUpdate.ZengJiaSql("HWBYYCount", 0, ZiFuOrInt.Int);
                orUpdate.Where("HWDanHao", danhao, ZiFuOrInt.ZiFu);

                string sql = orUpdate.GetSQLString();
                int count = DanLiFanWenDB.Cerate().UpdateOrAdd(sql);

            }
        }

        private int YouWuYuYue(string danhao, string gonghao)
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
    }
}
