using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

class Program
{
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}

public class MainForm : Form
{
    // Define the connection string (replace with your actual database connection string)
    private string connectionString = @"Data source=DESKTOP-PLTE4AP\SQLEXPRESS;Initial Catalog=records;Integrated security=True";

    private Label lblId, lblName, lblAddress, lblContact, lblEmail, lblPassword;
    private TextBox txtId, txtName, txtAddress, txtContact, txtEmail, txtPassword;
    private Button btnCreate, btnUpdate, btnDelete, btnClose;
    private DataGridView dataGrid;

    public MainForm()
    {

        // Set the form properties
        this.Text = "User Information Form";
        this.Width = 600;
        this.Height = 600;

        // Create and configure the ID label and textbox
        lblId = new Label { Text = "ID:", Left = 20, Top = 20, Width = 100 };
        txtId = new TextBox { Left = 120, Top = 20, Width = 200 };

        // Create and configure the Name label and textbox
        lblName = new Label { Text = "Name:", Left = 20, Top = 60, Width = 100 };
        txtName = new TextBox { Left = 120, Top = 60, Width = 200 };

        // Create and configure the Address label and textbox
        lblAddress = new Label { Text = "Address:", Left = 20, Top = 100, Width = 100 };
        txtAddress = new TextBox { Left = 120, Top = 100, Width = 200 };

        // Create and configure the Contact label and textbox
        lblContact = new Label { Text = "Contact:", Left = 20, Top = 140, Width = 100 };
        txtContact = new TextBox { Left = 120, Top = 140, Width = 200 };

        // Create and configure the Email label and textbox
        lblEmail = new Label { Text = "Email:", Left = 20, Top = 180, Width = 100 };
        txtEmail = new TextBox { Left = 120, Top = 180, Width = 200 };

        // Create and configure the Password label and textbox
        lblPassword = new Label { Text = "Password:", Left = 20, Top = 220, Width = 100 };
        txtPassword = new TextBox { Left = 120, Top = 220, Width = 200 };

        // Create and configure buttons
        btnCreate = new Button { Text = "Create", Left = 120, Top = 270 };
        btnUpdate = new Button { Text = "Update", Left = 200, Top = 270 };
        btnDelete = new Button { Text = "Delete", Left = 280, Top = 270 };
        btnClose = new Button { Text = "Close", Left = 360, Top = 270 };

        // Set up the Close button's click event to close the form
        btnClose.Click += (sender, e) => this.Close();

        // Set up button click event handlers
        btnCreate.Click += BtnCreate_Click;
        btnUpdate.Click += BtnUpdate_Click;
        btnDelete.Click += BtnDelete_Click;

        // Create and configure the DataGridView

        dataGrid = new DataGridView { Left = 20, Top = 310, Width = 550, Height = 200 };

    // Subscribe to CellClick event for DataGridView
    dataGrid.CellClick += DataGrid_CellClick;


        // Add controls to the form
        this.Controls.Add(lblId);
        this.Controls.Add(txtId);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblAddress);
        this.Controls.Add(txtAddress);
        this.Controls.Add(lblContact);
        this.Controls.Add(txtContact);
        this.Controls.Add(lblEmail);
        this.Controls.Add(txtEmail);
        this.Controls.Add(lblPassword);
        this.Controls.Add(txtPassword);
        this.Controls.Add(btnCreate);
        this.Controls.Add(btnUpdate);
        this.Controls.Add(btnDelete);
        this.Controls.Add(btnClose);
        this.Controls.Add(dataGrid);

        // Load data into DataGridView when form loads
        LoadData();
    }

    private void LoadData()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM employee";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.DataSource = table;
        }
    }

    // Event handler for CellClick to populate form fields
private void DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
{
    if (e.RowIndex >= 0) // Ensure the clicked cell is not a header
    {
        DataGridViewRow row = dataGrid.Rows[e.RowIndex];

        // Populate text boxes with values from the selected row
        txtId.Text = row.Cells["Id"].Value?.ToString();
        txtName.Text = row.Cells["UserName"].Value?.ToString();
        txtAddress.Text = row.Cells["Address"].Value?.ToString();
        txtContact.Text = row.Cells["Contact"].Value?.ToString();
        txtEmail.Text = row.Cells["Email"].Value?.ToString();
        txtPassword.Text = row.Cells["Password"].Value?.ToString();
    }
}

    // Event handler for Create button
    private void BtnCreate_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO employee (UserName, Address, Contact, Email, Password) VALUES (@Name, @Address, @Contact, @Email, @Password)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                // cmd.Parameters.AddWithValue("@ID", txtId.Text);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record inserted successfully.");
                LoadData();  // Refresh the data grid view
                ClearFields();
            }
        }
    }

    // Event handler for Update button
    private void BtnUpdate_Click(object sender, EventArgs e)
    {
        if (dataGrid.SelectedRows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE employee SET UserName=@Name, Address=@Address, Contact=@Contact, Email=@Email, Password=@Password WHERE Id=@ID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", txtId.Text);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record updated successfully.");
                    LoadData();  // Refresh the data grid view
                    ClearFields();
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a row to update.");
        }
    }

    // Event handler for Delete button
    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dataGrid.SelectedRows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM employee WHERE Id=@ID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", txtId.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted successfully.");
                    LoadData();  // Refresh the data grid view
                    ClearFields();
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a row to delete.");
        }
    }

    // Helper method to clear the input fields
    private void ClearFields()
    {
        txtId.Clear();
        txtName.Clear();
        txtAddress.Clear();
        txtContact.Clear();
        txtEmail.Clear();
        txtPassword.Clear();
    }
}
