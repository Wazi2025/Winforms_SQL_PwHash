using Microsoft.IdentityModel.Tokens;
using WinForms_Login;

namespace WinForms;

public partial class Form1 : Form
{
    //Add fields to the Form class so we can access them directly instead of having to iterate
    //through the Form's controls
    private Label lblFirstName;
    private Label lblLastName;
    private Label lblPhone;
    private Label lblEmail;
    private Label lblStreet;
    private Label lblCity;
    private Label lblZip;
    private Label lblCountry;
    private TextBox tbFirstName;
    private TextBox tbLastName;
    private TextBox tbPhone;
    private TextBox tbEmail;
    private TextBox tbStreet;
    private TextBox tbCity;
    private TextBox tbZip;
    private TextBox tbCountry;
    private Button btnSelect;
    private Button btnInsert;
    private Button btnAddUser;
    private TextBox tbNewUser;
    private TextBox tbNewUserPw;
    private TextBox tbWhere;

    private DataGridView dataWindow;

    private void InitializeInsertControls()
    {
        //Add Insert controls
        btnInsert = new Button();
        btnInsert.FlatStyle = FlatStyle.Popup;
        btnInsert.TabIndex = 8;
        btnInsert.Text = "Insert";
        btnInsert.AutoSize = true;
        //Hook up event
        btnInsert.Click += new EventHandler(this.btnInsert_Click);

        lblFirstName = new Label();
        lblFirstName.Text = "First Name";
        lblFirstName.AutoSize = true;

        lblLastName = new Label();
        lblLastName.Text = "Last Name";
        lblLastName.AutoSize = true;

        lblPhone = new Label();
        lblPhone.Text = "Phone";
        lblPhone.AutoSize = true;

        lblEmail = new Label();
        lblEmail.Text = "E-mail";
        lblEmail.AutoSize = true;

        lblStreet = new Label();
        lblStreet.Text = "Address";
        lblStreet.AutoSize = true;

        lblCity = new Label();
        lblCity.Text = "City";
        lblCity.AutoSize = true;

        lblZip = new Label();
        lblZip.Text = "Zip code";
        lblZip.AutoSize = true;

        lblCountry = new Label();
        lblCountry.Text = "Country";
        lblCountry.AutoSize = true;

        tbFirstName = new TextBox();
        tbFirstName.TabIndex = 0;

        //Hook up Validating event
        tbFirstName.Validating += new System.ComponentModel.CancelEventHandler(this.tbFirstName_Validating);


        tbLastName = new TextBox();
        tbLastName.TabIndex = 1;
        tbPhone = new TextBox();
        tbPhone.TabIndex = 2;
        tbEmail = new TextBox();
        tbEmail.TabIndex = 3;
        tbStreet = new TextBox();
        tbStreet.TabIndex = 4;
        tbCity = new TextBox();
        tbCity.TabIndex = 5;
        tbZip = new TextBox();
        tbZip.TabIndex = 6;
        tbCountry = new TextBox();
        tbCountry.TabIndex = 7;
    }

    private void InitializeSelectControls()
    {
        //Add Select controls
        btnSelect = new Button();
        btnSelect.FlatStyle = FlatStyle.Popup;
        btnSelect.TabIndex = 10;
        btnSelect.Text = "Select";
        btnSelect.AutoSize = true;
        //Hook up event
        btnSelect.Click += new EventHandler(this.btnSelect_Click);
        tbWhere = new TextBox();
        tbWhere.TabIndex = 9;
    }

    private void InitializeNewUserControls()
    {
        //Add New User controls
        btnAddUser = new Button();
        btnAddUser.FlatStyle = FlatStyle.Popup;
        btnAddUser.TabIndex = 13;
        btnAddUser.Text = "Add user";
        btnAddUser.AutoSize = true;
        //Hook up event
        btnAddUser.Click += new EventHandler(this.btnAddUser_Click);

        tbNewUser = new TextBox();
        tbNewUser.TabIndex = 11;
        tbNewUserPw = new TextBox();
        tbNewUserPw.TabIndex = 12;
    }

    private void InitializeDataGrid()
    {
        //Add DataGridView
        dataWindow = new DataGridView();
        dataWindow.TabStop = false;
        dataWindow.Dock = DockStyle.Top;
        dataWindow.AutoSize = true;
    }

    private void InitializeTable()
    {
        //Add TableLayOutPanel
        TableLayoutPanel table = new TableLayoutPanel();
        table.Dock = DockStyle.Fill;
        table.Controls.Add(dataWindow, 0, 0);
        table.SetColumnSpan(dataWindow, 8);

        //Add controls to table
        table.Controls.Add(tbFirstName, 0, 10);
        table.Controls.Add(lblFirstName, 0, 5);

        table.Controls.Add(tbLastName, 1, 10);
        table.Controls.Add(lblLastName, 1, 5);

        table.Controls.Add(tbPhone, 2, 10);
        table.Controls.Add(lblPhone, 2, 5);

        table.Controls.Add(tbEmail, 3, 10);
        table.Controls.Add(lblEmail, 3, 5);

        table.Controls.Add(tbStreet, 4, 10);
        table.Controls.Add(lblStreet, 4, 5);

        table.Controls.Add(tbCity, 5, 10);
        table.Controls.Add(lblCity, 5, 5);

        table.Controls.Add(tbZip, 6, 10);
        table.Controls.Add(lblZip, 6, 5);

        table.Controls.Add(tbCountry, 7, 10);
        table.Controls.Add(lblCountry, 7, 5);

        table.Controls.Add(btnInsert, 0, 20);

        table.Controls.Add(btnSelect, 1, 30);
        table.Controls.Add(tbWhere, 0, 30);

        table.Controls.Add(btnAddUser, 2, 40);
        table.Controls.Add(tbNewUser, 0, 40);
        table.Controls.Add(tbNewUserPw, 1, 40);

        table.AutoSize = true;

        // Add to form
        this.Controls.Add(table);
    }
    private void InitializeUserPrivileges()
    {
        //set controls visibility based on user
        //Note: Prolly a better idea to simply not create the controls
        if (!Program.UserPrivileges(LoginForm.tbUserName.Text))
        {
            btnAddUser.Visible = false;
            tbNewUser.Visible = false;
            tbNewUserPw.Visible = false;
        }
    }

    void btnSelect_Click(object sender, EventArgs e)
    {
        //Add query result to DataSource component
        dataWindow.DataSource = Program.SQLSelect(tbWhere.Text);
    }

    void tbFirstName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (tbFirstName.Text.IsNullOrEmpty())
        {
            //MessageBox.Show("Field cannot be empty!", "Warning!");
            //tbFirstName.Focus();
        }
    }
    void btnAddUser_Click(object sender, EventArgs e)
    {
        //Note: Use a menu choice instead of a button

        //Check for empty username/pw
        if (tbNewUser.Text.IsNullOrEmpty() || tbNewUserPw.Text.IsNullOrEmpty())
        {
            MessageBox.Show("Username/Password can not be empty.", "Error!");
            return;
        }

        //Add new User and PW into DB        
        if (Program.SQLAddUser(tbNewUser.Text, tbNewUserPw.Text))
        {
            MessageBox.Show("A user with that name already exists. Please choose another.", "Error!");
            return;
        }
        else
            MessageBox.Show("New user added successfully.", "Information!");

        tbNewUser.Clear();
        tbNewUserPw.Clear();
    }

    void btnInsert_Click(object sender, EventArgs e)
    {
        bool fieldValidated = true;
        string warning = "Warning!";
        string warningMessage = "cannot be empty.";

        //Add field validation for FirstName, LastName and Email (the only not null DB columns)
        if (tbFirstName.Text.IsNullOrEmpty())
        {
            tbFirstName.Focus();
            fieldValidated = false;
            MessageBox.Show($"Field '{lblFirstName.Text}' {warningMessage}", warning);
        }
        else
        if (tbLastName.Text.IsNullOrEmpty())
        {
            tbLastName.Focus();
            fieldValidated = false;
            MessageBox.Show($"Field '{lblLastName.Text}' {warningMessage}", warning);
        }
        else
        if (tbEmail.Text.IsNullOrEmpty())
        {
            tbEmail.Focus();
            fieldValidated = false;
            MessageBox.Show($"Field '{lblEmail.Text}' {warningMessage}", warning);
        }
        //Add rudimentary email syntax check
        // if (!tbEmail.Text.Contains("@"))
        // {
        //     tbEmail.Focus();
        //     fieldValidated = false;
        //     MessageBox.Show($"Not a valid {lblEmail.Text} format.", warning);
        // }

        // if (data.Contains(""))
        // {
        //     MessageBox.Show("Fields cannot be empty!", "Warning");
        //     return;
        // }

        if (fieldValidated)
        {
            List<string> data = new List<string>();

            //Add values from TextBoxes to List
            data.Add(tbFirstName.Text);
            data.Add(tbLastName.Text);
            data.Add(tbPhone.Text);
            data.Add(tbEmail.Text);
            data.Add(tbStreet.Text);
            data.Add(tbCity.Text);
            data.Add(tbZip.Text);
            data.Add(tbCountry.Text);

            //Send TextBox values as parameters to SQLInsert method
            Program.SQLInsert(data);

            tbFirstName.Clear();
            tbLastName.Clear();
            tbPhone.Clear();
            tbEmail.Clear();
            tbStreet.Clear();
            tbCity.Clear();
            tbZip.Clear();
            tbCountry.Clear();

            MessageBox.Show("Record inserted.", "Information!");
        }
    }

    public Form1()
    {
        InitializeComponent();

        this.Name = "MainForm";
        this.Text = "Main";
        this.Size = new System.Drawing.Size(900, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

        InitializeInsertControls();
        InitializeSelectControls();
        InitializeInsertControls();
        InitializeNewUserControls();
        InitializeDataGrid();
        InitializeTable();
        InitializeUserPrivileges();
    }
}
