using System;
using System.Data;
using System.Data.SqlClient;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.Common
{
	///<summary>
	///Defines a set of common static database functions<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  4-14-2003<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class DatabaseFunctions
	{
		public DatabaseFunctions()
		{
		}

		#region ExecuteSQLScalar method

		public static string ExecuteSQLScalar(string commandText, string connectionString)
		{
			string result = "";
			SqlConnection sqlConn = new SqlConnection(connectionString);
			
			try
			{
				sqlConn.Open();
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLScalar(string, string)", "ConnectionString: " + connectionString + "; CommandText: " + commandText.Substring(0, 100), ErrorType.DbUnavailable, sqlE);
			}

			SqlCommand sqlComm = new SqlCommand(commandText, sqlConn);
			
			try
			{
				result = sqlComm.ExecuteScalar().ToString();
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLScalar(string, string)", "ConnectionString: " + connectionString + "; CommandText: " + commandText.Substring(0, 100), ErrorType.InvalidArgument, sqlE);
			}

			sqlConn.Close();
			sqlConn.Dispose();
			sqlComm.Dispose();

			return result;
		}

		#endregion

		#region ExecuteSQLSelect method

		public static DataTable ExecuteSQLSelect(string commandText, string connectionString)
		{
			DataTable dbTable = new DataTable();
			SqlDataAdapter dbAdapter = new SqlDataAdapter(commandText, connectionString);

			try
			{
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLSelect(string, string)", "ConnectionString: " + connectionString + "; CommandText: " + commandText.Substring(0, 100), ErrorType.DbUnavailable, sqlE);
			}

			dbAdapter.Dispose();

			return dbTable;			
		}
		
		#endregion

		#region ExecuteSQLSelect method

		public static DataTable ExecuteSQLSelect(SqlCommand sqlComm, string connectionString)
		{
			SqlConnection sqlConn = new SqlConnection(connectionString);
			
			try
			{
				sqlConn.Open();
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLSelect(SqlCommand, string)", "ConnectionString: " + connectionString + "; CommandText: " + sqlComm.CommandText.Substring(0, 100), ErrorType.DbUnavailable, sqlE);
			}

			sqlComm.Connection = sqlConn;
			DataTable dbTable = new DataTable();
			SqlDataAdapter dbAdapter = new SqlDataAdapter(sqlComm);

			try
			{
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLSelect(SqlCommand, string)", "ConnectionString: " + connectionString + "; CommandText: " + sqlComm.CommandText.Substring(0, 100), ErrorType.DbUnavailable, sqlE);
			}

			sqlConn.Close();
			sqlConn.Dispose();
			dbAdapter.Dispose();

			return dbTable;			
		}

		#endregion

		#region ExecuteSQLNonQuery method

		public static int ExecuteSQLNonQuery(string commandText, string connectionString)
		{
			int recsAffected = 0;

			SqlConnection sqlConn = new SqlConnection(connectionString);
			
			try
			{
				sqlConn.Open();
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLNonQuery(string, string)", "ConnectionString: " + connectionString + "; CommandText: " + commandText.Substring(0, 100), ErrorType.DbUnavailable, sqlE);
			}

			SqlCommand sqlCommand = new SqlCommand(commandText, sqlConn);
			
			try
			{
				recsAffected = sqlCommand.ExecuteNonQuery();				
			}
			catch(SqlException sqlE)
			{
				//The source argument must be less than 250 characters or an exception will be thrown when writing to the CancerGov event log
				CancerGovError.LogError("DatabaseFunctions.ExecuteSQLNonQuery(string, string)", "ConnectionString: " + connectionString + "; CommandText: " + commandText.Substring(0, 100), ErrorType.DbUnavailable, sqlE);
				recsAffected = -1;
			}

			sqlConn.Close();
			sqlConn.Dispose();
			sqlCommand.Dispose();			

			return recsAffected;			
		}


		#endregion
	}
}
