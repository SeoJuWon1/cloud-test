using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Microsoft.Extensions.Logging;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

namespace WebApplication1
{
    public class Program
    {
        public static string[] addr1 = new string[10];
        public static string[] addr2 = new string[10];
        public static string[] camtype;
        public static string[] lat;
        public static string[] lon;
        public static string[] lmt;


        
        public static void Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "https://silladbserver.database.windows.net";
                builder.UserID = "admin123";
                builder.Password = "Seojuwon123";
                builder.InitialCatalog = "camerabox";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    int x = 0;
                    connection.Open();                    

                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append("SELECT camtype,lmt,addr1,addr2,lat,lon FROM SECURITY");

                    String sql2 = Convert.ToString(sb1.ToString());

                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                x++;
                            }
                        }
                    }
                    camtype = new string[x];
                    lmt = new string[x];
                    addr1= new string[x];
                    addr2 = new string[x];
                    lat = new string[x];
                    lon = new string[x];
                    int i = 0;
                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                                                {
                            while (reader.Read())
                            {
                                //Console.WriteLine("{0}", reader.GetString(0));
                                camtype[i] = reader.GetString(0);
                                lmt[i] = reader.GetString(1);
                                addr1[i] = reader.GetString(2);
                                addr2[i] = reader.GetString(3);
                                lat[i] = reader.GetString(4);
                                lon[i] = reader.GetString(5);
                                i++;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}