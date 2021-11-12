using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoWebForms.DataBase
{
    public class DataBaseExec
    {
        static string connectionString = @"Data Source=DESKTOP-SVARKC4\SQLEXPRESS;Initial Catalog = ProyectoWebForms; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static SqlCommand CreateBasicCommand()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            return command;
        }

        public static SqlCommand CreateBasicCommand(string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query);
            command.Connection = connection;
            return command;
        }

        public static List<SqlCommand> CreateNBasicCommand(int n)
        {
            List<SqlCommand> res = new List<SqlCommand>();
            SqlConnection connection = new SqlConnection(connectionString);
            for (int i = 0; i < n; i++)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                res.Add(command);
            }
            return res;
        }

        public static void ExecuteNBasicCommand(List<SqlCommand> commands)
        {
            SqlTransaction transaction = null;
            SqlCommand command1 = commands[0];
            try
            {
                command1.Connection.Open();
                transaction = command1.Connection.BeginTransaction();
                foreach (var item in commands)
                {
                    item.Transaction = transaction;
                    item.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                command1.Connection.Close();
            }
        }

        public static string GetGenerateIDTable(string tableName)
        {
            string res = "";
            string query = "SELECT IDENT_CURRENT('" + tableName + "')+IDENT_INCR('" + tableName + "')";
            SqlCommand command = CreateBasicCommand(query);

            try
            {
                command.Connection.Open();
                res = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
            return res;
        }

        public static int ExecuteBasicCommand(SqlCommand command)
        {
            try
            {
                command.Connection.Open();
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public static DataTable ExecuteDataTableCommand(SqlCommand command)
        {
            DataTable res = new DataTable();
            try
            {
                command.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(res);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
            return res;
        }
    }
}