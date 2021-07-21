using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKGLXT.KongJian
{
    public class DataGrid:DataGridView
    {
        public DataGrid() : base()
        {
            this.DoubleBuffered = true;
            this.MouseClick += DataGrid_MouseClick;
        }

        private void DataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (GetRowIndexAt(e.Y) == -1)
            {
               
                this.ClearSelection();
            }
        }
        public int GetRowIndexAt(int mouseLocation_Y)
        {
            if (this.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;
            }
            if (this.ColumnHeadersVisible == true && mouseLocation_Y <= this.ColumnHeadersHeight)
            {
                return -1;
            }
            int index = this.FirstDisplayedScrollingRowIndex;
            int displayedCount = this.DisplayedRowCount(true);
            for (int k = 1; k <= displayedCount;)
            {
                if (this.Rows[index].Visible == true)
                {
                    Rectangle rect = this.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域   
                    if (rect.Top <= mouseLocation_Y && mouseLocation_Y < rect.Bottom)
                    {
                        return index;
                    }
                    k++;
                }
                index++;
            }
            return -1;
        }
    }
}
