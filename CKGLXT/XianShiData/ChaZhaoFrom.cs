using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FuChuanTiFrom;

namespace CKGLXT.XianShiData
{
    public partial class ChaZhaoFrom : BaseFuFrom
    {
        public  string MingCheng = "";
        public ChaZhaoFrom()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }

        public void SetCanShu()
        {
            this.label4.Text = "数量";
            this.button1.Text = "确定";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MingCheng = this.textBox4.Text;

            this.DialogResult = DialogResult.OK;
        }
    }
}
