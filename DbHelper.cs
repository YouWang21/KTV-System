﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework.Controls;
using CCWin;
using CCWin.SkinControl;
using System.Windows.Forms;

namespace KTV_management_system
{
    class DbHelper
    {
        private const string Source = "Data Source=LINDONG;Initial Catalog=KTV_entertainment_management_system;UID=sa;PWD=114514";

        public static DataTable getDataTable(string sql)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Source))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, sqlConnection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                return dataTable;
            }
        }

        public static int executeNonQuery(string sql)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Source))
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public static string executeScalar(string sql)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Source))
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();

                return command.ExecuteScalar().ToString();
            }
        }

        public static void Add_button(SkinFlowLayoutPanel skinFlowLayout,string sql)
        {
            DataTable dataTable = getDataTable(sql);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                SkinButton skinButton = new SkinButton();
                skinButton.Text = dataRow[1].ToString();
                skinButton.Margin = new Padding(3,5,0,0);
                skinButton.Radius = 9;
                skinButton.Cursor = Cursors.Hand;
                skinButton.RoundStyle = CCWin.SkinClass.RoundStyle.Top;
                skinButton.Tag = dataRow[0].ToString();
                skinFlowLayout.Controls.Add(skinButton);
            }
        }

        public static void Private_roomsListView(SkinListView skinListView, string sql)
        {
            DataTable dataTable = getDataTable(sql);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                ListViewItem listViewItem = new ListViewItem();

                switch (dataRow[1].ToString())
                {
                    case "可供":
                        listViewItem.ImageIndex = 0;
                        break;
                    case "占用":
                        listViewItem.ImageIndex = 1;
                        break;
                    case "停用":
                        listViewItem.ImageIndex = 2;
                        break;
                    case "预订":
                        listViewItem.ImageIndex = 3;
                        break;
                }

                listViewItem.Text = dataRow[0].ToString();
                listViewItem.SubItems.Add(dataRow[1].ToString());
                listViewItem.SubItems.Add(dataRow[2].ToString());
                listViewItem.SubItems.Add(dataRow[3].ToString());
                listViewItem.SubItems.Add(dataRow[4].ToString());
                listViewItem.SubItems.Add(dataRow[5].ToString());

                skinListView.Items.Add(listViewItem);
            }
        }

        public static void skinDataGridView(SkinDataGridView skinDataGridView,string sql,string Pinyin)
        {
            DataTable dataTable = getDataTable(sql);

            if (!string.IsNullOrEmpty(Pinyin))
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    var str = PingYinHelper.GetFirstSpell(item[1].ToString()).ToLower();

                    if (str.IndexOf(Pinyin.ToUpper()) >= 0)
                    {
                        skinDataGridView.DataSource = item.Table;
                    }
                }
            }
            else
            {
                skinDataGridView.DataSource = dataTable;
            }
        }

        public static void skinCollections(SkinComboBox skinComboBox,string sql,string value,string name,string Default)
        {
            DataTable dataTable = getDataTable(sql);

            DataRow dataRow = dataTable.NewRow();
            dataRow[value] = -1;
            dataRow[name] = Default;
            dataTable.Rows.InsertAt(dataRow,0);

            skinComboBox.ValueMember = value;
            skinComboBox.DisplayMember = name;
            skinComboBox.DataSource = dataTable;
        }
    }
}
