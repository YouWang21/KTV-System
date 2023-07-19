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
        private static string sql = @"select [project_ID],[Name],[unit],[Preset_unit_price],[cost],[TypeName],[Repository],[exchange],[Redeem_points] from [dbo].[Commodity] as a
            join[dbo].[commodityType] as b on a.category_ID = b.CommodityTypeID
            where 1=1";
        private static string tmp = sql;

        public Item_merchandise()
        {
            InitializeComponent();
        }

        public void commodity_flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView4, "select [CommodityTypeID], [TypeName], [Waiter] = (case [Waiter] when '1' then '需要' when '0' then '不需要' end) from [dbo].[commodityType]","");
            
            DbHelper.skinDataGridView(skinDataGridView5, tmp,"");
        }

        private void Item_merchandise_Load(object sender, EventArgs e)
        {
            commodity_flushed();

            DbHelper.skinCollections(skinComboBox2, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]", "CommodityTypeID", "TypeName", "所有项目");
        }

        private void skinComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(skinComboBox2.SelectedValue) > 0)
            {
                tmp += $" and a.[category_ID] = '{skinComboBox2.SelectedValue}'";
                commodity_flushed();
                return;
            }
            commodity_flushed();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView5, sql, textBox1.Text);
        }
    }
}
