using System.Data;
using WinForms;

namespace WinForms_Login;

public class LoginForm : Form
{
    //Add fields to the Form class so we can access them directly instead of having to iterate
    //through the Form's controls
    private Label lblUserName;
    private Label lblPassword;

    private TextBox tbUserName;
    private TextBox tbPassword;

    private Button btnLogIn;


    public void InitializeLoginForm()
    {
        lblUserName = new Label();
        lblUserName.Text = "Username";
        lblUserName.AutoSize = true;

        tbUserName = new TextBox();

        lblPassword = new Label();
        lblPassword.Text = "Password";
        lblPassword.AutoSize = true;

        tbPassword = new TextBox();
        tbPassword.UseSystemPasswordChar = true;

        btnLogIn = new Button();
        btnLogIn.Text = "Login";
        btnLogIn.FlatStyle = FlatStyle.Popup;
        btnLogIn.AutoSize = true;

        //Hook up event
        btnLogIn.Click += new EventHandler(this.btnLogin_Click);

        TableLayoutPanel loginTable = new TableLayoutPanel();
        loginTable.Dock = DockStyle.Fill;

        loginTable.Controls.Add(lblUserName, 0, 5);
        loginTable.Controls.Add(tbUserName, 0, 10);

        loginTable.Controls.Add(lblPassword, 0, 20);
        loginTable.Controls.Add(tbPassword, 0, 25);

        loginTable.Controls.Add(btnLogIn, 0, 30);

        loginTable.AutoSize = true;

        // Add to form
        this.Controls.Add(loginTable);
        this.Text = "Login";
        this.StartPosition = FormStartPosition.CenterScreen;

        //this.AutoSize = true;

    }


    private void btnLogin_Click(object sender, EventArgs e)
    {
        string user = tbUserName.Text.Trim();
        string pass = tbPassword.Text;

        if (Program.TryLogin(user, pass))
        {
            MessageBox.Show("Login successful!", "Login");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            MessageBox.Show("Invalid login. Please try again.", "Warning!");
        }
    }



    // public void Form()
    // {
    //     //InitializeComponent();
    //     // this.Name = "LoginForm";
    //     // this.Text = "Login";
    //     // this.Size = new System.Drawing.Size(900, 500);
    //     // this.StartPosition = FormStartPosition.CenterScreen;
    //     // this.AutoSize = true;

    //     //InitializeLoginForm();
    // }
}
