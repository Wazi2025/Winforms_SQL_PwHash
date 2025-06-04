using Microsoft.IdentityModel.Tokens;

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
    private TextBox tbWhere;
    //private GroupBox gbInsert;

    public DataGridView dataWindow;

    public void Initialize()
    {
        lblFirstName = new Label();
        lblFirstName.Text = "First name";
        lblFirstName.AutoSize = true;

        lblLastName = new Label();
        lblLastName.Text = "Last name";
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

        btnInsert = new Button();
        btnInsert.FlatStyle = FlatStyle.Popup;
        btnInsert.TabIndex = 8;
        btnInsert.Text = "Insert";
        btnInsert.AutoSize = true;

        // gbInsert = new GroupBox();
        // gbInsert.Text = " Insert ";
        // gbInsert.FlatStyle = FlatStyle.Flat;
        // gbInsert.Controls.Add(btnInsert);
        // gbInsert.Controls.Add(tbFirstName);
        // gbInsert.Controls.Add(tbLastName);
        // gbInsert.Controls.Add(tbPhone);
        // gbInsert.Controls.Add(tbEmail);
        // gbInsert.Controls.Add(tbStreet);
        // gbInsert.Controls.Add(tbCity);
        // gbInsert.Controls.Add(tbZip);
        // gbInsert.Controls.Add(tbCountry);

        //Hook up event
        btnInsert.Click += new EventHandler(this.btnInsert_Click);

        btnSelect = new Button();
        btnSelect.FlatStyle = FlatStyle.Popup;
        btnSelect.TabIndex = 9;
        btnSelect.Text = "Select";
        btnSelect.AutoSize = true;

        tbWhere = new TextBox();
        tbWhere.TabIndex = 10;
        //Hook up event
        btnSelect.Click += new EventHandler(this.btnSelect_Click);

        dataWindow = new DataGridView();
        dataWindow.TabStop = false;
        dataWindow.Dock = DockStyle.Top;
        dataWindow.AutoSize = true;

        TableLayoutPanel table = new TableLayoutPanel();

        table.Dock = DockStyle.Fill;

        table.Controls.Add(dataWindow, 0, 0);
        table.SetColumnSpan(dataWindow, 8);

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

        table.Controls.Add(btnSelect, 0, 30);
        table.Controls.Add(tbWhere, 1, 30);

        table.Controls.Add(btnInsert, 0, 20);

        //table.Controls.Add(gbInsert, 1, 20);

        table.AutoSize = true;

        // Add to form
        this.Controls.Add(table);
    }


    void btnSelect_Click(object sender, EventArgs e)
    {
        //Add query result to DataSource component
        dataWindow.DataSource = Program.SQLSelect(tbWhere.Text);
    }

    void btnInsert_Click(object sender, EventArgs e)
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

        if (data.Contains(""))
        {
            MessageBox.Show("Fields cannot be empty!", "Warning");
            return;
        }

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
    }

    public Form1()
    {
        InitializeComponent();

        this.Name = "MainForm";
        this.Text = "Main";
        this.Size = new System.Drawing.Size(900, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.AutoSize = true;

        Initialize();
    }
}
