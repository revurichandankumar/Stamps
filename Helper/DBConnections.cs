﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace OneposStamps.Helper
{
    public sealed class SqlDBConnection : IDisposable
    {
        private static SqlDBConnection instance = null;
        private SqlDBConnection()
        {
            _connectionString = $"data source={_serverName};Persist Security Info=True;initial catalog={_dbName};User ID={_user};Password={_password}";
        }
        public static SqlDBConnection GetInstance(string serverName = null, string dbName = null, string userName = null, string password = null)
        {
            if (instance == null)
            {
                _serverName = serverName;
                _dbName = dbName;
                _user = userName;
                _password = password;
                instance = new SqlDBConnection();
            }
            return instance;
        }
        private static string _serverName;
        private static string _dbName;
        private static string _user;
        private static string _password;
        private readonly string _connectionString;
        private SqlConnection connection = new SqlConnection();
        private SqlCommand command = new SqlCommand();
        private SqlDataAdapter adapter = new SqlDataAdapter();
        private DataSet dataSet = new DataSet();
        private DataTable dataTable = new DataTable();
        private SqlCommandBuilder commandBld = new SqlCommandBuilder();

        private MySqlConnection con = new MySqlConnection();
        private MySqlDataAdapter adp = new MySqlDataAdapter();
        private MySqlCommand cmd = new MySqlCommand();

        private void OpenConnection()
        {
            try
            {
                if (!(connection.State.ToString() == "Open"))
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void CloseConnection()
        {
            try
            {
                if (connection.State.ToString() == "Open")
                    connection.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error in close connection", ex);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in close connection", ex);
            }
        }

        public DataSet GetMysqlDataSet(string sqlStmt, DateTime startdate, DateTime enddate, string sid)
        {

            try
            {
                var connectionString = string.Empty;
                
                connectionString = ConfigurationManager.ConnectionStrings["OnePos"].ConnectionString;
          
              
                //MySqlConnection con = new MySqlConnection(connectionString);
                con.ConnectionString = connectionString;
                con.Open();
                //string rtn = "SuperCategory";
                cmd.Connection = con;
                cmd.CommandText = sqlStmt;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdate", startdate);
                cmd.Parameters.AddWithValue("@enddate", enddate);
                cmd.Parameters.AddWithValue("@SId", sid);
                cmd.CommandTimeout = int.MaxValue;
                adp.SelectCommand = cmd;

                if (dataSet != null)
                    dataSet.Reset();
                adp.Fill(dataSet);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                cmd.Parameters.Clear();
            }
            return dataSet;
        }
        public DataSet GetDataSet(string sqlStmt, bool IsStoredProcedure, params object[] parameters)
        {
            OpenConnection();
            try
            {
                command.CommandText = sqlStmt;
                command.Connection = connection;
                if (IsStoredProcedure)
                    command.CommandType = CommandType.StoredProcedure;
                else
                    command.CommandType = CommandType.Text;
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                command.CommandTimeout = int.MaxValue;
                adapter.SelectCommand = command;
                if (dataSet != null)
                    dataSet.Reset();
                adapter.Fill(dataSet);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                CloseConnection();
                command.Parameters.Clear();
            }
            return dataSet;
        }
        public DataTable GetDataTable(string sqlStmt)
        {
            OpenConnection();
            try
            {
                command.CommandText = sqlStmt;
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                if (dataTable != null)
                    dataTable.Reset();
                adapter.Fill(dataTable);
                commandBld.DataAdapter = adapter;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }

        private bool _isDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_isDisposed)
                {
                    instance = null;
                    _isDisposed = true;
                }
            }
        }
    }
}