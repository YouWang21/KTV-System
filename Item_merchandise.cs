using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    public partial class Item_merchandise : Form
    {
        public Item_merchandise()
        {
            InitializeComponent();
        }

        public void commodityType_flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView4, "select [CommodityTypeID], [TypeName], [Waiter] = (case [Waiter] when '1' then '需要' when '0' then '不需要' end) from [dbo].[commodityType]");
        }

        private void Item_merchandise_Load(object sender, EventArgs e)
        {
            commodityType_flushed();
        }
    }
}
