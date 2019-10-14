using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;


namespace LatencyMonitor
{

    public class Database : INotifyPropertyChanged
    {
        //Central observed object
        //Has input method executeNonQuery for pollers to update the database status
        //Has output method executeReader to return status

        private readonly String _connectionString;

        //This global collection can be subscribed to to receive collectionChanged events
        //When the collection or its members change
        public static ObservableMembersCollection<Database> all = new ObservableMembersCollection<Database>();

        private DatabaseReplicationStatus _status;
        public DatabaseReplicationStatus status
        {
            get
            {
                return _status;
            }
            set
            {
                if (!value.Equals(_status))
                {
                    _status = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(status)));
                }

            }
        }

        public String name;
        public string hostName;
        public Guid guid;

        public event PropertyChangedEventHandler PropertyChanged;

        public Database(String dBHostname, String dBUserName, String password, String deploymentCode)
        {
            _connectionString = $"Server = tcp:{dBHostname},1433;Initial Catalog={deploymentCode};Persist Security Info = False;User ID={dBUserName};Password={password};MultipleActiveResultSets=False; Encrypt = True; TrustServerCertificate = False; Authentication = 'Active Directory Password'";
            this.name = deploymentCode;
            this.hostName = dBHostname;
            this.guid = Guid.NewGuid();

           // Server = tcp:rye - db - scus - 1.database.windows.net,1433; Initial Catalog = Volga_Test; Persist Security Info = False; User ID = { your_username }; Password ={ your_password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Authentication = "Active Directory Password";
        }
        
        private SqlConnection getConnection()
        {
            SqlConnection database = new SqlConnection(_connectionString);
            database.Open();
            return database;
        }

        public Object executeReader(SqlCommand command, Func<IDataReader, Object> reader)
        {
            Object result = null;
            using (SqlConnection connection = getConnection())
            {
                if (connection == null) return result;
                command.Connection = connection;
                result = reader(command.ExecuteReader()); //throws null exception when executed in paralllel
                connection?.Close();
            }
            return result;
        }

        public Boolean executeNonQuery(SqlCommand cmd)
        {
            Boolean result = false;
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    if (connection == null) return result;

                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    result = true;
                    connection?.Close();
                }
            }
            catch (Exception e)
            {
                this.status = new DatabaseReplicationStatus("DISCONNECTED", "DISCONNECTED", e.Message, new DateTimeOffset());
            }
            return result;
        }
    }
}
