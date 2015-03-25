using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Smo;

namespace Simple.Data.AspNet.Identity.Tests
{
    /// <summary>
    /// Class provides functionality to help developer perform unit testing on data access code
    /// </summary>
    public class DatabaseSupport : IDisposable
    {
        private SqlConnection _sqlConnection;
        private Server _targetServer;
        private bool _disposed = false;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">standard connection string for db to interact with</param>
        public DatabaseSupport(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _targetServer = new Server(_sqlConnection.DataSource);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~DatabaseSupport()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a new empty database on the server if one of that name doesn't already exist
        /// </summary>
        /// <param name="name">Name of the database to create</param>
        public void CreateDB(string name)
        {
            if (!_targetServer.Databases.Contains(name))
            {
                var toCreate = new Microsoft.SqlServer.Management.Smo.Database(_targetServer, name);
                toCreate.Create();
            }
        }

        /// <summary>
        /// Drops a database on the server
        /// </summary>
        /// <param name="name">name of the database to drop</param>
        public void DropDB(string name)
        {
            if (_targetServer.Databases.Contains(name))
            {
                _targetServer.KillAllProcesses(_sqlConnection.Database);
                _targetServer.KillDatabase(name);
            }
        }


        /// <summary>
        /// Runs a specified script against a target db
        /// </summary>
        /// <param name="scriptPath">Path to the script file to execute</param>
        public void RunScript(string scriptPath)
        {
            RunScript(scriptPath, false, null);
        }

        /// <summary>
        /// Runs a specified script against a target db
        /// </summary>
        /// <param name="scriptPath">Path to the script file to execute</param>
        /// <param name="parameters">Parameters to use within the script</param>
        public void RunScript(string scriptPath, Dictionary<string, string> parameters)
        {
            RunScript(scriptPath, false, parameters);
        }

        /// <summary>
        /// Runs a specified script against a target db 
        /// </summary>
        /// <param name="scriptPath">Path to the script file to execute</param>
        /// <param name="returnResults">Flag indicating we expect to be return results from executing the script</param>
        /// <returns>Dataset containing data returned from executing the script</returns>
        public DataSet RunScript(string scriptPath, bool returnResults)
        {
            return RunScript(scriptPath, returnResults, null);
        }

        /// <summary>
        /// Runs a specified script against a target db
        /// </summary>
        /// <param name="scriptPath">Path to the script file to execute</param>
        /// <param name="returnResults">Flag indicating we expect to be return results from executing the script</param>
        /// <param name="parameters">Parameters to use within the script</param>
        /// <returns>Dataset containing data returned from executing the script</returns>
        public DataSet RunScript(string scriptPath, bool returnResults, Dictionary<string, string> parameters)
        {
            DataSet results = null;
            string script = LoadScript(scriptPath);

            // If parameters provided then alter the script accordingly
            if (parameters != null && parameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    script = script.Replace(parameter.Key, parameter.Value);
                }
            }

            if (returnResults)
            {
                results = _targetServer.ConnectionContext.ExecuteWithResults(script);
            }
            else
            {
                _targetServer.ConnectionContext.ExecuteNonQuery(script);
            }

            return results;
        }

        /// <summary>
        /// Method to dispose of objects used 
        /// </summary>
        /// <param name="disposing">Flag to indicate we are currenlty disposing the object</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_sqlConnection.State != ConnectionState.Closed)
                        _sqlConnection.Close();
                    _disposed = true;
                }

            }
        }


        /// <summary>
        /// Loads the path with the data provided
        /// </summary>
        private string LoadScript(string path)
        {
            string script;

            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("No path has been provided for the path. Unable to load.");

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    script = sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                throw new IOException("Error occured whilst trying to LoadScript");
            }

            return script;
        }
    }

}
