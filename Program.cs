using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using WinForms_Login;
//using BCrypt.Net;

namespace WinForms;

static class Program
{
    static private readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=TestDB;Trusted_Connection=True;TrustServerCertificate=true";

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        // Application.EnableVisualStyles();
        // Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        LoginForm login = new LoginForm();
        login.InitializeLoginForm();

        if (login.ShowDialog() == DialogResult.OK)
        {
            Application.Run(new Form1());
        }
    }

    static private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12); // Work factor defines cost
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    }


    static public bool TryLogin(string username, string password)
    {
        using SqlConnection conn = GetFreshConnection();

        string query = "SELECT password_hash FROM users WHERE username = @user";
        using SqlCommand command = new SqlCommand(query, conn);
        command.Parameters.AddWithValue("@user", username);

        // Get the stored hash        
        object result = command.ExecuteScalar();
        if (result == null || result == DBNull.Value)
            return false;

        string storedHash = result.ToString();

        // Use bcrypt to verify
        return VerifyPassword(password, storedHash);
    }


    static public void SQLAddUser(string username, string password)
    {
        if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            return;

        //Hash user's password
        string hashedPassword = HashPassword(password);

        using SqlConnection conn = GetFreshConnection();

        string query = "INSERT INTO users (username, password_hash) VALUES (@user, @pw)";

        SqlCommand command = conn.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@user", username);
        command.Parameters.AddWithValue("@pw", hashedPassword);

        //Execute query
        command.ExecuteNonQuery();
    }

    static public void SQLInsert(List<string> data)
    {
        using SqlConnection conn = GetFreshConnection();

        string insertQuery = "INSERT INTO person (first_name, last_name, phone, email, street, city, zip_code, country) VALUES (@f_name, @l_name, @phone, @email, @street, @city, @zip, @country)";

        SqlCommand command = conn.CreateCommand();
        command.CommandText = insertQuery;

        //Note: Prolly add some sort of validation here
        command.Parameters.AddWithValue("@f_name", data[0]);
        command.Parameters.AddWithValue("@l_name", data[1]);
        command.Parameters.AddWithValue("@phone", data[2]);
        command.Parameters.AddWithValue("@email", data[3]);
        command.Parameters.AddWithValue("@street", data[4]);
        command.Parameters.AddWithValue("@city", data[5]);
        command.Parameters.AddWithValue("@zip", data[6]);
        command.Parameters.AddWithValue("@country", data[7]);

        //Execute query
        command.ExecuteNonQuery();
    }

    static public SqlConnection GetFreshConnection()
    {
        //Set connection string and open DB connection
        var conn = new SqlConnection(connectionString);
        conn.Open();
        return conn;
    }

    static public DataTable SQLSelect(string whereText)
    {
        //By using the 'using' statement we make sure the DB connection is closed down after each use
        using SqlConnection conn = GetFreshConnection();

        string query;

        //Note: Make sure we’re not building queries with user input directly(e.g., string concatenation), and instead always use parameterized queries like:
        //command.CommandText = "SELECT * FROM person WHERE first_name = @firstName";
        //command.Parameters.AddWithValue("@firstName", userInput);
        if (whereText.IsNullOrEmpty())
            query = "SELECT * FROM person";
        else
            query = "SELECT * FROM person WHERE first_name = @firstName";

        SqlCommand command = conn.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@firstName", whereText);

        SqlDataAdapter da = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();

        da.Fill(dataTable);

        return dataTable;
    }
}