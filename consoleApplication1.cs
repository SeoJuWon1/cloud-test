using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        public static string[] addr1 = new string[10];
        public static string[] addr2 = new string[10];
        public static string[] camtype;
        public static string[] lat;
        public static string[] lon;
        public static string[] lmt;

        static DataSet ds = new DataSet();
        static string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\클라우드 컴퓨팅 이해\과제\프로젝트\cloudtest\camdata.xlsx;MODE=ReadWrite;";
        
        private static void count_info1()
        {
            string sql = "Select * FROM Sheet1";
            OleDbConnection conn = new OleDbConnection(connStr);
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            cmd.CommandText = sql;
            int count = 0;
            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                count++;
            }
            stype = new string[count];
            sname = new string[count];
            addr1 = new string[count];
            addr2 = new string[count];
            lat = new string[count];
            longt = new string[count];
            iscctv = new string[count];
            cctvcount = new int[count];

            Console.WriteLine("완료 {0}", count);
            reader.Close();
            conn.Close();
        }
        private static void search_info()
        {
            string sql = "Select * FROM Sheet1";
            OleDbConnection conn = new OleDbConnection(connStr);
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            cmd.CommandText = sql;
            int i = 0;

            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                camtype[i] = reader["카메라종류"].ToString();
                addr1[i] = reader["소재지도로"].ToString();
                addr2[i] = reader["소재지지번"].ToString();
                lat[i] = reader["위도"].ToString();
                lon[i] = reader["경도"].ToString();
                lmt[i] = reader["제한속도"].ToString();
                i++;
            }
            reader.Close();
            conn.Close();
            Console.WriteLine("완료 {0}", i);
        }
        static void Main(string[] args)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            string sql1 = "SELECT * FROM Sheet1";
            OleDbDataAdapter adp = new OleDbDataAdapter(sql1, conn);
            adp.Fill(ds);
            count_info1();
            search_info();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "https://silladbserver.database.windows.net";
                builder.UserID = "admin123";
                builder.Password = "Seojuwon123";
                builder.InitialCatalog = "camerabox";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data ADD:");
                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    SqlCommand command = null;
                    for (int i = 0; i<stype.Length; i++)
                    {
                        string sql = "INSERT INTO SECURITY VALUES ('" + camtype[i] + "','" + lmt[i] + "','" + addr1[i] + "','" + addr2[i] + "','"+lat+"','" + lon +");";
                        command = new SqlCommand(sql, connection);
                        command.ExecuteNonQuery();
                    }
                    
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append("SELECT ADDR FROM BELL");
                    
                    String sql2 = Convert.ToString(sb1.ToString());                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}", reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}
