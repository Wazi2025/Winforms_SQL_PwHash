using System.Data;
using Microsoft.IdentityModel.Tokens;
using WinForms;

namespace WinForms_Login;

public class LoginForm : Form
{
    //Add fields to the Form class so we can access them directly instead of having to iterate
    //through the Form's controls
    private Label lblUserName;
    private Label lblPassword;

    static public TextBox tbUserName;
    private TextBox tbPassword;

    private Button btnLogIn;
    private ComboBox cbDBType;


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

        cbDBType = new ComboBox();
        cbDBType.Items.AddRange(new object[] { "SQL Server", "PostgreSQL" });
        cbDBType.SelectedIndex = 0;

        //Hook up event
        cbDBType.SelectedIndexChanged +=
           new System.EventHandler(cbDBType_SelectedIndexChanged);

        //Hook up event
        btnLogIn.Click += new EventHandler(this.btnLogin_Click);

        TableLayoutPanel loginTable = new TableLayoutPanel();
        loginTable.Dock = DockStyle.Fill;

        loginTable.Controls.Add(lblUserName, 0, 5);
        loginTable.Controls.Add(tbUserName, 0, 10);

        loginTable.Controls.Add(lblPassword, 0, 20);
        loginTable.Controls.Add(tbPassword, 0, 25);

        loginTable.Controls.Add(btnLogIn, 0, 30);
        loginTable.Controls.Add(cbDBType, 0, 40);

        loginTable.AutoSize = true;

        // Add to form
        this.Controls.Add(loginTable);
        this.Text = "Login";
        this.Width = 600;
        this.StartPosition = FormStartPosition.CenterScreen;

        //this.AutoSize = true;
    }

    private void cbDBType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbDBType.SelectedIndex == 0)
            Program.MicrosoftSqlConnection = true;
        else
            Program.MicrosoftSqlConnection = false;
    }
    private void btnLogin_Click(object sender, EventArgs e)
    {
        string user = tbUserName.Text.Trim();
        string pass = tbPassword.Text;
        string messageBoxLoginText;

        const string loginSuccess = "Login successful!";
        const string loginFail = "Invalid login. Please try again.";
        const string information = "Information!";
        const string warning = "Warning!";

        if (Program.SQL_serverDBConnection())
        {
            //Microsoft SQL Server
            if (Program.TryLogin(user, pass))
            {
                messageBoxLoginText = loginSuccess;
                MessageBox.Show($"{messageBoxLoginText}", $"{information}");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                messageBoxLoginText = loginFail;
                MessageBox.Show($"{messageBoxLoginText}", $"{warning}");
            }
        }
        else
        {
            //PostgreSQL
            if (Program.TryLogin_Postgres(user, pass))
            {
                messageBoxLoginText = loginSuccess;
                MessageBox.Show($"{messageBoxLoginText}", $"{information}");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                messageBoxLoginText = loginFail;
                MessageBox.Show($"{messageBoxLoginText}", $"{warning}");
            }
        }

        //Log user attempt login
        Program.LogUserLogin(user, pass, messageBoxLoginText);
    }
}
