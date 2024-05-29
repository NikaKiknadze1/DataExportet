using System.Data;
using Microsoft.Data.SqlClient;
namespace DataExporter
{
    namespace G19_ProductImport
    {
        public class Database
        {
            private readonly string _connectionString;
            private SqlConnection? _connection;
            private SqlTransaction? _transaction;

            public Database(string connectionString)
            {
                _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            }

            public SqlConnection GetConnection() => _connection ??= new SqlConnection(_connectionString);

            public SqlConnection OpenConnection()
            {
                GetConnection().Open();
                return _connection!;
            }

            public void CloseConnection() => _connection?.Close();

            public SqlCommand GetCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
            {
                SqlCommand command = GetConnection().CreateCommand();
                command.CommandText = commandText;
                command.CommandType = commandType;
                command.Parameters.AddRange(parameters);
                if (_transaction != null)
                {
                    command.Transaction = _transaction;
                }

                return command;
            }

            public SqlCommand GetCommand(string commandText, params SqlParameter[] parameters) =>
                GetCommand(commandText, CommandType.Text, parameters);

            public int ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
            {
                using SqlCommand command = GetCommand(commandText, commandType, parameters);
                return command.ExecuteNonQuery();
            }

            public int ExecuteNonQuery(string commandText, params SqlParameter[] parameters) =>
                ExecuteNonQuery(commandText, CommandType.Text, parameters);

            public object? ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters)
            {
                using SqlCommand command = GetCommand(commandText, commandType, parameters);
                return command.ExecuteScalar();
            }

            public object? ExecuteScalar(string commandText, params SqlParameter[] parameters) =>
                ExecuteScalar(commandText, CommandType.Text, parameters);

            public SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
            {
                using SqlCommand command = GetCommand(commandText, commandType, parameters);
                return command.ExecuteReader();
            }

            public SqlDataReader ExecuteReader(string commandText, params SqlParameter[] parameters) =>
                ExecuteReader(commandText, CommandType.Text, parameters);

            public DataTable GetDataTable(string commandText, CommandType commandType, params SqlParameter[] parameters)
            {
                using SqlCommand command = GetCommand(commandText, commandType, parameters);
                using SqlDataAdapter adapter = new(command);
                DataTable table = new();
                adapter.Fill(table);
                return table;
            }

            public DataTable GetDataTable(string commandText, params SqlParameter[] parameters) =>
                GetDataTable(commandText, CommandType.Text, parameters);

            public SqlTransaction BeginTransaction()
            {
                if (_transaction != null)
                {
                    throw new InvalidOperationException("Transaction already started.");
                }
                _transaction = GetConnection().BeginTransaction();
                return _transaction;
            }

            public void CommitTransaction()
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("Transaction not started.");
                }
                _transaction!.Commit();
                _transaction = null;
            }

            public void RollbackTransaction()
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("Transaction not started.");
                }
                _transaction!.Rollback();
                _transaction = null;
            }
            public void Dispose()
            {
                
                CloseConnection();

            }

        }

    }
}