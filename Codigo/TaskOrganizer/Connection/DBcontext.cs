using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using TaskOrganizer.Utilities;

namespace TaskOrganizer.Connection
{


    public class DBcontext
    {
        public class QueryResult
        {
            public DataTable dt { get; set; }
            public statesQuery result { get; set; }
            public string messageResult { get; set; }

        }

        public class statementResult
        {
            public statesQuery result { get; set; }
            public string messageResult { get; set; }
            public int rowsAffected { get; set; }

        }

        // Method that runs a SQL query using Npgsql and returns the result in a QueryResult object
        public QueryResult runQuery(string strsql, List<NpgsqlParameter> parameterList, CommandType commandType)
        {
            // Create an instance of QueryResult to store the result of the query
            QueryResult queryResult = new QueryResult();

            // Variable to control retry logic for connection attempts
            bool persistenceResponse = false;

            // Counter for the number of connection attempts
            int tryConecction = 0;

            // To store any error messages that might occur
            string errorMessage = "";

            // Loop to attempt connection until it succeeds or reaches 5 attempts
            while (persistenceResponse == false)
            {
                // Create a new PostgreSQL connection using the connection string
                NpgsqlConnection npgsqlConnection = new NpgsqlConnection(InterfaceConfig.connectionString);

                // Create an empty command object
                NpgsqlCommand command = new NpgsqlCommand();

                // Increment the number of attempts
                tryConecction += 1;

                try
                {
                    // If the connection is already open, close it and reopen it
                    if (npgsqlConnection.State == ConnectionState.Open)
                    {
                        npgsqlConnection.Close();
                        npgsqlConnection.Open();
                    }
                    else
                    {
                        // Open the connection
                        npgsqlConnection.Open();
                    }

                    // Create a command object from the open connection
                    command = npgsqlConnection.CreateCommand();
                    command.CommandType = commandType;     // Set the type of command (e.g., Text or StoredProcedure)
                    command.CommandText = strsql;          // Set the SQL query text

                    // If there are parameters provided, add them to the command
                    if (parameterList != null)
                    {
                        foreach (var parameter in parameterList)
                        {
                            // Add each parameter to the command with its name and value
                            command.Parameters.Add(new NpgsqlParameter(parameter.ParameterName, parameter.Value));
                        }
                    }

                    // Create a DataTable to hold the results of the query
                    DataTable dT = new DataTable();

                    // Create a data adapter to fill the DataTable with query results
                    NpgsqlDataAdapter dA = new NpgsqlDataAdapter(command);
                    dA.Fill(dT);  // Fill the DataTable with the result of the command

                    // If the execution reaches here, the query was successful
                    persistenceResponse = true;

                    // Store the result in the queryResult object
                    queryResult.dt = dT;

                    // Log the successful execution of the query
                    Log.recordLog($"Query executed ---->[{strsql}]");

                    // Indicate the query result was successful
                    queryResult.result = statesQuery.OK;
                }
                catch (Exception ex)
                {
                    // If there is an error, store the message
                    errorMessage = $"Error executing the following script ---> [{strsql}], Description ---> [{ex.Message}]";

                    // Log the error
                    Log.recordLog($"Error executing the following script ---> [{strsql}], Description ---> [{ex.Message}]");

                    // Mark the attempt as failed
                    persistenceResponse = false;

                    // Store the error details in the result object
                    queryResult.result = statesQuery.ERROR;
                    queryResult.messageResult = errorMessage;
                }
                finally
                {
                    // Clean up: dispose and close the connection
                    npgsqlConnection.Dispose();
                    npgsqlConnection.Close();
                }

                // If failed after 5 attempts, stop trying
                if (persistenceResponse == false && tryConecction == 5)
                {
                    persistenceResponse = true;
                }
            }

            // Return the result of the query, either with data or with error details
            return queryResult;
        }





    }
}
