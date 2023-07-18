using CCWin;
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
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class System_settings : Skin_Mac
    {
        private static string sql = @"select [Private_rooms_ID],[type_Name],[Private_room_status] = (case [Private_room_status] when '0' then '可供' when '1' then '占用' when '2' then '停用' when '3' then '预订' end),[location] from [dbo].[Private_rooms] as a
        join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
        where 1=1";
        private static string tmp = sql;

        public System_settings()
        {
            InitializeComponent();
        }

        private void System_settings_Load(object sender, EventArgs e)
        {
            Private_room_type_refresh();

            Member_refresh();

            commodityType_flushed();

            DbHelper.skinCollections(skinComboBox1, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "所有包间");

            Inquire();
        }

        private void skinComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(skinComboBox1.SelectedValue) > 0)
            {
                tmp += $" and a.[Private_rooms_type] = '{skinComboBox1.SelectedValue}'";
                Inquire();
                return;
            }
            Inquire();
        }

        private void Inquire()
        {
            DbHelper.skinDataGridView(skinDataGridView2, tmp);
            tmp = sql;
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            Batch batch = new Batch();
            batch.ShowDialog();

            Inquire();
        }

        private void skinButton8_Click(object sender, EventArgs e)
        {
            method_Individually();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            revise_method();
        }

        private void skinDataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                revise_method();
            }
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            delete_method();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method_Individually();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            revise_method();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete_method();
        }

        public void method_Individually()
        {
            Individually individually = new Individually();
            individually.ShowDialog();

            Inquire();
        }

        public void revise_method()
        {
            Modify_the_private_room modify_The_Private_Room = new Modify_the_private_room();

            modify_The_Private_Room.Type = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column6"].Value.ToString();
            modify_The_Private_Room.id = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column5"].Value.ToString();
            modify_The_Private_Room.region = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column8"].Value.ToString();

            modify_The_Private_Room.ShowDialog();

            Inquire();
        }

        public void delete_method()
        {
            if (MessageBox.Show("该操作不可恢复，是否要执行删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column5"].Value.ToString()}'");
                Inquire();
            }
        }

        public void revise_type_method()
        {
            revise_type();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            revise_type_method();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            Add_type();
        }

        public void Private_room_type_refresh()
        {
            DbHelper.skinDataGridView(skinDataGridView1, @"select [type_Name],[Minimum_consumption],b.[manner_Name],[Capacity],c.[manner_Name] as 'Ordinary_festivals',d.[manner_Name] as 'Special_holidays',[Discount_conditions],[Fold_rate] from [dbo].[Type_of_private_room] as a
            left join[dbo].[Billing_type] as b on a.Billing_method = b.manner_ID
            left join[dbo].[Billing_type] as c on a.Ordinary_festivals = c.manner_ID
            left join[dbo].[Billing_type] as d on a.Special_holidays = d.manner_ID");
        }

        public void Add_type()
        {
            Add_a_private_room_type add_A_Private_Room_Type = new Add_a_private_room_type();
            add_A_Private_Room_Type.ShowDialog();

            Private_room_type_refresh();
        }

        public void delete_type()
        {
            if (MessageBox.Show("该操作会将相关数据全部删除并且不可恢复，是否要执行删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Private_rooms] where [Private_rooms_type] = '{DbHelper.executeScalar($"select [Private_rooms_type_ID] from [dbo].[Type_of_private_room] where [type_Name] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'")}'");
                DbHelper.executeNonQuery($"delete from [dbo].[Type_of_private_room] where [type_Name] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");

                Private_room_type_refresh();
                Inquire();
            }
        }

        public void revise_type()
        {
            Modify_the_private_room_type modify = new Modify_the_private_room_type();

            modify.Type = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            modify.Minimum_consumption = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();
            modify.Capacity = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column4"].Value.ToString();
            modify.Ordinary_festivals = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Ordinary_festivals"].Value.ToString();
            modify.Special_holidays = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Special_holidays"].Value.ToString();
            modify.Discount_conditions = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Discount_conditions"].Value.ToString();
            modify.Fold_rate = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Fold_rate"].Value.ToString();
            modify.Standard_holidays = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column3"].Value.ToString();

            modify.ShowDialog();
            Private_room_type_refresh();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            delete_type();
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Add_type();
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            delete_type();
        }

        private void 修改ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            revise_type();
        }

        public void Member_refresh()
        {
            DbHelper.skinDataGridView(skinDataGridView3, "select [memberID], [TypeName], [Initial_Points], [Fold_rate] from [dbo].[Member]");
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            Add_member add_Member = new Add_member();
            add_Member.ShowDialog();

            Member_refresh();
        }

        private void skinButton10_Click(object sender, EventArgs e)
        {
            Modify_Membership modify_Membership = new Modify_Membership();

            modify_Membership.Membership_Level = skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column10"].Value.ToString();
            modify_Membership.Membership_Points = skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column11"].Value.ToString();
            modify_Membership.Membership_discount = skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column12"].Value.ToString();

            modify_Membership.ShowDialog();
            Member_refresh();
        }

        private void skinButton9_Click(object sender, EventArgs e)
        {
            Member_Deletion();
        }

        public void Member_Deletion()
        {
            if (MessageBox.Show("该操作不可恢复，是否要执行删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Member] where [TypeName] = '{skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column10"].Value}'");
                Member_refresh();
            }
        }

        private void skinButton11_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void commodityType_flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView4, "select [CommodityTypeID], [TypeName], [Waiter] = (case [Waiter] when '1' then '需要' when '0' then '不需要' end) from [dbo].[commodityType]");
        }
    }
}
