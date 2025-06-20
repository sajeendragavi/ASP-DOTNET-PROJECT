using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace ASPCRUD
{
    public partial class Index : System.Web.UI.Page
    {
        string connectionString = @"Server=localhost;Database=aspcruddb;Uid=root;Pwd=199598;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                Clear();
                GridFill();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
                {
                    sqlCon.Open();
                    MySqlCommand sqlCmd = new MySqlCommand("ProductAddOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("_productid", Convert.ToInt32(hfProductID.Value == "" ? "0" : hfProductID.Value));
                    sqlCmd.Parameters.AddWithValue("_product", txtProduct.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("_price", Convert.ToDecimal(txtPrice.Text.Trim()));
                    sqlCmd.Parameters.AddWithValue("_count", Convert.ToInt32(txtCount.Text.Trim()));
                    sqlCmd.Parameters.AddWithValue("_description", txtDescription.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    GridFill();
                    Clear();

                    lblSuccessMessage.Text = "Submitted Successfully";

                }
            }
            catch (Exception ex)
            {

                lblErrorMessage.Text = ex.Message;
            }


        }

        void Clear()
        {
            hfProductID.Value = "";
            txtProduct.Text = txtPrice.Text = txtCount.Text = txtDescription.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            lblErrorMessage.Text = lblSuccessMessage.Text = "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void GridFill() {
            using (MySqlConnection sqlcon = new MySqlConnection(connectionString))
            {
                sqlcon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("ProductViewAll", sqlcon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                gvProduct.DataSource = dtbl;
                gvProduct.DataBind();
               
            }
        
        }

        protected void lnkSelected_OnClick(object sender, EventArgs e) {
            int productID = Convert.ToInt32((sender as LinkButton).CommandArgument);    
            using (MySqlConnection sqlcon = new MySqlConnection(connectionString))
            {
                sqlcon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("ProductViewByID", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("_productid", productID);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);

                txtProduct.Text = dtbl.Rows[0][1].ToString();
                txtPrice.Text = dtbl.Rows[0][2].ToString();
                txtCount.Text = dtbl.Rows[0][3].ToString();
                txtDescription.Text = dtbl.Rows[0][4].ToString();

                hfProductID.Value = dtbl.Rows[0][0].ToString();

                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
               

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
            {
                sqlCon.Open();
                MySqlCommand sqlCmd = new MySqlCommand("ProductDeleteByID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("_productid", Convert.ToInt32(hfProductID.Value == "" ? "0" : hfProductID.Value));
                
                sqlCmd.ExecuteNonQuery();
                GridFill();
                Clear();

                lblSuccessMessage.Text = "Deleted Successfully";

            }
        }
    }
}