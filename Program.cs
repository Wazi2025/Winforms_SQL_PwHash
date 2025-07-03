using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using WinForms_Login;
using WinForms;
using Npgsql;


namespace WinForms;

static class Program
{
    static public bool MicrosoftSqlConnection = true;
    static private string connectionString = "";

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        LoginForm login = new LoginForm();
        login.InitializeLoginForm();

        if (login.ShowDialog() == DialogResult.OK)
        {
            Application.Run(new Form1());
        }
    }

    static public bool SQL_serverDBConnection()
    {
        if (MicrosoftSqlConnection)
        {
            //Microsoft SQL
            //Local Dockerized SQL Server connection string
            connectionString = "Server=localhost,1433;Database=TestDBMS;User Id=sa;Password=Admin1234#;TrustServerCertificate=True;";
            return true;
        }
        else
        {
            //PostgreSQL
            //Local Dockerized PostgreSQL connection string            
            connectionString = "host=localhost;Port=5432;Database=TestDB;Username=admin;Password=1234";
            return false;
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

    public static bool UserPrivileges(string username)
    {
        //Set Mainform controls visibility based on user
        string checkUserQuery = "SELECT username FROM my_schema.users WHERE username = @user";

        //Microsoft SQL version
        using SqlConnection conn = GetFreshConnection();
        using SqlCommand commandCheck = conn.CreateCommand();
        commandCheck.CommandText = checkUserQuery;
        commandCheck.Parameters.AddWithValue("@user", username);
        object uniqueUser = commandCheck.ExecuteScalar();

        //Check for hardcoded username for now
        if ((uniqueUser is not null) && username.Equals("Wazi"))
            return true;
        else
            return false;
    }

    static public void LogUserLogin(string username, string password, string messageBoxLoginText, string dbType)
    {
        //log user login attempts in file
        string fileDataDir = "Data";
        string fileName = "log.txt";

        //Set the application path
        string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

        string filePath = Path.Combine(projectRoot, fileDataDir, fileName);
        string dirPath = Path.Combine(projectRoot, fileDataDir);
        DateTime dateTime = DateTime.Now;
        string logString = $"{dateTime.ToString()}:  Username:'{username}'  Message:'{messageBoxLoginText}' Type:'{dbType}'";

        //Create file and dir if it does not exist
        if (!File.Exists(filePath) || !Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
            using StreamWriter streamWriter = File.CreateText(filePath);
            streamWriter.WriteLine(logString);
        }
        else //Append to already existing file
        {
            using StreamWriter streamWriter = File.AppendText(filePath);
            streamWriter.WriteLine(logString);
        }

    }
    static public bool TryLogin(string username, string password)
    {
        //Microsoft SQL
        using SqlConnection conn = GetFreshConnection();

        string query = "SELECT password_hash FROM my_schema.users WHERE username = @user";

        //Microsoft SQL
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

    static public bool SQLAddUser(string username, string password)
    {
        //Duplicate username check
        string checkDuplicateUserQuery = "SELECT username FROM my_schema.users WHERE username = @user";
        using SqlConnection conn = GetFreshConnection();
        using SqlCommand commandCheck = conn.CreateCommand();

        commandCheck.CommandText = checkDuplicateUserQuery;
        commandCheck.Parameters.AddWithValue("@user", username);
        object uniqueUser = commandCheck.ExecuteScalar();

        //If uniqueUser is null we know there are no duplicate usernames
        if (uniqueUser is null)
        {
            //Apparently using the the same SqlCommand for different parameterized query's can cause things to go pear-shaped
            //To be safe we'll create a new one for the add new user query
            //
            //Again, we add the 'using' statement to SqlCommand object since it is not guaranteed to be automatically disposed when the SqlConnection it was created from is disposed
            using SqlCommand commandInsert = conn.CreateCommand();

            //Hash user's password
            string hashedPassword = HashPassword(password);

            string query = "INSERT INTO my_schema.users (username, password_hash) VALUES (@newUser, @newUserPw)";

            commandInsert.CommandText = query;
            commandInsert.Parameters.AddWithValue("@newUser", username);
            commandInsert.Parameters.AddWithValue("@newUserPw", hashedPassword);

            //Execute query
            commandInsert.ExecuteNonQuery();
        }

        else if (uniqueUser.ToString().Equals(username))
        {
            return true;
        }
        return false;
    }

    static public void SQLInsert(List<string> data)
    {
        using SqlConnection conn = GetFreshConnection();
        using SqlCommand command = conn.CreateCommand();

        string insertQuery = "INSERT INTO my_schema.person (first_name, last_name, phone, email, street, city, zip_code, country) VALUES (@f_name, @l_name, @phone, @email, @street, @city, @zip, @country)";
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
        //Set connection string and open DB connection (this is the Microsoft SQL version)
        var conn = new SqlConnection(connectionString);
        conn.Open();
        return conn;
    }

    static public DataTable SQLSelect(string whereFName, string whereLName)
    {
        //By using the 'using' statement we make sure the DB connection is closed down after each use
        //Microsoft
        using SqlConnection conn = GetFreshConnection();

        string query;

        //Note: Make sure we’re not building queries with user input directly(e.g., string concatenation), and instead always use parameterized queries like:
        //command.CommandText = "SELECT * FROM person WHERE first_name = @firstName";
        //command.Parameters.AddWithValue("@firstName", userInput);
        //If not, we risk SQL injection
        if (whereFName.IsNullOrEmpty())
            query = "SELECT * FROM my_schema.person";
        else
            query = "SELECT * FROM my_schema.person WHERE first_name = @firstName";

        using SqlCommand command = conn.CreateCommand();

        command.CommandText = query;
        command.Parameters.AddWithValue("@firstName", whereFName);

        SqlDataAdapter da = new SqlDataAdapter(command);
        // DataTable dataTable = new DataTable();
        // da.Fill(dataTable);

        using var reader = command.ExecuteReader();
        DataTable dataTable = new DataTable();
        dataTable.Load(reader);

        return dataTable;
    }

    static public NpgsqlConnection GetFreshConnection_Postgres()
    {
        //Set connection string and open DB connection    
        //PostGreSQL version
        var conn = new NpgsqlConnection(connectionString);

        conn.Open();
        return conn;
    }

    public static bool UserPrivileges_Postgres(string username)
    {
        //Set Mainform controls visibility based on user
        string checkUserQuery = "SELECT username FROM my_schema.users WHERE username = @user";

        //PostGreSQL version
        using NpgsqlConnection conn = GetFreshConnection_Postgres();
        using NpgsqlCommand commandCheck = conn.CreateCommand();
        commandCheck.CommandText = checkUserQuery;
        commandCheck.Parameters.AddWithValue("@user", username);
        object uniqueUser = commandCheck.ExecuteScalar();

        //Check for hardcoded username for now
        if ((uniqueUser is not null) && username.Equals("Wazi"))
            return true;
        else
            return false;
    }

    static public bool TryLogin_Postgres(string username, string password)
    {
        //PostgreSQL
        using NpgsqlConnection conn = GetFreshConnection_Postgres();

        string query = "SELECT password_hash FROM my_schema.users WHERE username = @user";

        //PostgreSQL
        using NpgsqlCommand command = new NpgsqlCommand(query, conn);

        command.Parameters.AddWithValue("@user", username);

        // Get the stored hash        
        object result = command.ExecuteScalar();
        if (result == null || result == DBNull.Value)
            return false;

        string storedHash = result.ToString();

        // Use bcrypt to verify
        return VerifyPassword(password, storedHash);
    }

    static public bool SQLAddUser_Postgres(string username, string password)
    {
        //Duplicate username check
        string checkDuplicateUserQuery = "SELECT username FROM my_schema.users WHERE username = @user";

        //PosgreSQL
        using NpgsqlConnection conn = GetFreshConnection_Postgres();
        //Add the 'using' statement to SqlCommand object since it is not guaranteed to be automatically disposed when the SqlConnection it was created from is disposed
        using NpgsqlCommand commandCheck = conn.CreateCommand();


        commandCheck.CommandText = checkDuplicateUserQuery;
        commandCheck.Parameters.AddWithValue("@user", username);
        object uniqueUser = commandCheck.ExecuteScalar();

        //If uniqueUser is null we know there are no duplicate usernames
        if (uniqueUser is null)
        {
            //Apparently using the the same SqlCommand for different parameterized query's can cause things to go pear-shaped
            //To be safe we'll create a new one for the add new user query
            //
            //Again, we add the 'using' statement to SqlCommand object since it is not guaranteed to be automatically disposed when the SqlConnection it was created from is disposed

            //PostgreSQL
            using NpgsqlCommand commandInsert = conn.CreateCommand();

            //Hash user's password
            string hashedPassword = HashPassword(password);

            string query = "INSERT INTO my_schema.users (username, password_hash) VALUES (@newUser, @newUserPw)";

            commandInsert.CommandText = query;
            commandInsert.Parameters.AddWithValue("@newUser", username);
            commandInsert.Parameters.AddWithValue("@newUserPw", hashedPassword);

            //Execute query
            commandInsert.ExecuteNonQuery();
        }

        else if (uniqueUser.ToString().Equals(username))
        {
            return true;
        }
        return false;
    }

    static public DataTable SQLSelect_Postgres(string whereFName, string whereLName)
    {
        //By using the 'using' statement we make sure the DB connection is closed down after each use        
        using NpgsqlConnection conn = GetFreshConnection_Postgres();

        string query;

        //Note: Make sure we’re not building queries with user input directly(e.g., string concatenation), and instead always use parameterized queries like:
        //command.CommandText = "SELECT * FROM person WHERE first_name = @firstName";
        //command.Parameters.AddWithValue("@firstName", userInput);
        //If not, we risk SQL injection
        if (whereFName.IsNullOrEmpty())
            query = "SELECT * FROM my_schema.person";
        else
            query = "SELECT * FROM my_schema.person WHERE first_name = @firstName";

        using NpgsqlCommand command = conn.CreateCommand();

        command.CommandText = query;
        command.Parameters.AddWithValue("@firstName", whereFName);

        //SqlDataAdapter da = new SqlDataAdapter(command);
        // DataTable dataTable = new DataTable();
        // da.Fill(dataTable);

        using var reader = command.ExecuteReader();
        DataTable dataTable = new DataTable();
        dataTable.Load(reader);

        return dataTable;
    }
    static public void SQLInsert_Postgres(List<string> data)
    {
        //PostgreSQL
        using NpgsqlConnection conn = GetFreshConnection_Postgres();
        using NpgsqlCommand command = conn.CreateCommand();

        //Note: need to insert person_id
        string insertQuery = "INSERT INTO my_schema.person (first_name, last_name, phone, email, street, city, zip_code, country) VALUES (@f_name, @l_name, @phone, @email, @street, @city, @zip, @country)";

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
}//End of Program Class