using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace N18DCCN231_CDCNPM {
    public partial class Default : System.Web.UI.Page {

        public static List<String> selectedTableNames = new List<string>(); //tên các table đang được chọn
        public static List<String> listColumnName = new List<string>(); //tên các column đang được chọn

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
                this.GetTableName();
        }

        protected void CheckBoxListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTableNames.Clear();
            CheckBoxListColumn.Items.Clear();
            txtQuery.Text = "";

            foreach (ListItem item in CheckBoxListTable.Items)
                if (item.Selected)
                {
                    selectedTableNames.Add(item.Text);
                    GetColumnName(item.Value, item.Text);
                }                        
        }

        protected void CheckBoxListColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tên cột", Type.GetType("System.String"));
            dt.Columns.Add("Tên bảng", Type.GetType("System.String"));

            foreach (ListItem item in CheckBoxListColumn.Items)
                if (item.Selected)
                    dt.Rows.Add(item.Text, item.Value);

            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void ButtonClearColumn_Click(object sender, EventArgs e)
        {
            CheckBoxListColumn.Items.Clear();
            listColumnName.Clear();
            GridView1.Controls.Clear();
            selectedTableNames.Clear();

            foreach (ListItem item in CheckBoxListTable.Items)
                if (item.Selected)
                {
                    selectedTableNames.Add(item.Text);
                    GetColumnName(item.Value, item.Text);
                }

            txtQuery.Text = "";
        }

        public String GetRelationship(String a_id, String b_id)
        {
            List<String> values = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["QLVTConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "exec sp_TimKhoaNgoai " + a_id + ", " + b_id;

                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            values.Add(sdr["table_name"].ToString() + "." + sdr["column_name"].ToString());
                        }
                    }
                    conn.Close();
                }
            }
            return string.Join(" = ", values);
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            List<String> columns = new List<string>();
            List<String> conditions = new List<string>();
            List<String> groups = new List<string>();
            List<String> sorts = new List<string>();
            List<String> havings = new List<string>();            

            String query = "SELECT ";

            String column = "", table = "", sort = "", function = "", condition = "";

            if(selectedTableNames.Count > 1)
            {
                String a_id, b_id, relationship;
                for (int i = 0; i < selectedTableNames.Count - 1; i++)
                    for (int j = i + 1; j < selectedTableNames.Count; j++)
                    {
                        a_id = CheckBoxListTable.Items.FindByText(selectedTableNames[i]).Value;
                        b_id = CheckBoxListTable.Items.FindByText(selectedTableNames[j]).Value;
                        relationship = GetRelationship(a_id, b_id);
                        if (!relationship.Equals(""))
                            conditions.Add(relationship);
                    }
            }              

            foreach (GridViewRow row in GridView1.Rows)
            {
                
                sort = (row.Cells[0].FindControl("DropDownList_Sort") as DropDownList).SelectedValue;
                function = (row.Cells[1].FindControl("DropDownList_Function") as DropDownList).SelectedValue;
                condition = (row.Cells[2].FindControl("txtDieuKien") as TextBox).Text;
                column = row.Cells[3].Text;
                table = row.Cells[4].Text;

                //Xử lý hàm
                if (function.Equals("Select"))
                    columns.Add(table + "." + column);
                else if (function.Equals("Group by"))
                {
                    columns.Add(table + "." + column);
                    groups.Add(columns[columns.Count - 1]);
                }                        
                else
                    columns.Add(function + "(" + table + "." + column + ")");

                //Xử lý điều kiện
                if (!condition.Equals(""))
                    if(function.Equals("Select") || function.Equals("Group by"))
                        conditions.Add(columns[columns.Count - 1] + " " + condition);
                    else
                        havings.Add(columns[columns.Count - 1] + " " + condition);

                if (!sort.Equals("None"))
                    sorts.Add(columns[columns.Count - 1] + " " + sort);
            }
                
            query += string.Join(", ", columns) + " from " + string.Join(", ", selectedTableNames);

            //Nếu có điều kiện
            if (conditions.Count != 0)
                query += " where " + string.Join(" and ", conditions);

            //Nếu có group by
            if (groups.Count != 0)
                query += " group by " + string.Join(", ", groups);

            //Nếu có điều kiện sau khi group by
            if (havings.Count != 0)
                query += " having " + string.Join(" and ", havings);

            //Nếu có sắp xếp
            if (sorts.Count != 0)
                query += " order by " + string.Join(", ", sorts);

            txtQuery.Text = query;
        }

        protected void ButtonReport_Click(object sender, EventArgs e)
        {
            Session["query"] = txtQuery.Text;
            Session["title"] = TextBoxNhapTieuDe.Text;
            Response.Redirect("Viewer.aspx");
            Server.Execute("Viewer.aspx");
        }
        private void GetTableName()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["QLVTConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "select object_id, name from sys.tables where name <> 'sysdiagrams'";

                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["name"].ToString();
                            item.Value = sdr["object_id"].ToString();
                            CheckBoxListTable.Items.Add(item);
                        }
                    }
                    conn.Close();
                }
            }
        }
        private void GetColumnName(String tableID, String tableName)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["QLVTConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "select object_id, name from sys.columns where object_id = " + tableID;

                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["name"].ToString() + " (" + tableName + ")";
                            item.Value = tableName;
                            CheckBoxListColumn.Items.Add(item);
                        }
                    }
                    conn.Close();
                }
            }
        }
    }
}