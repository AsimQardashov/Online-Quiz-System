using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Threading;


namespace ExamingSystem.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class Sql
    {
        private const string V = @"Data Source=DESKTOP-HHC7VLQ;Initial Catalog=Examing;
                                                                   Integrated Security=True";
        /// <summary>
        /// Qoşulma Əmri
        /// </summary>
        public static readonly string ConnectionString = V;

        /// <summary>
        /// Paralel Sorğu İcraları Üçün Toqquşmaların Qarşısını Almaq Üçün
        /// </summary>
        private static readonly object LockExecute = new object();
        /// <summary>
        /// Qoşulma Kanalı
        /// </summary>
        static SqlConnection _kanal;

        /// <summary>
        /// Server-ə bağlanır
        /// </summary>
        public static void InitServer()
        {
            _kanal = new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Server-dən ayrılır
        /// </summary>
        public static void Unload()
        {
            _kanal.Close();
        }

        /// <summary>
        /// Bir Tək DataTable Qaytaran Sorğunu İcra Edir
        /// </summary>
        /// <param name="sqlSorgu">Sql Sorğusu</param>
        /// <returns></returns>
        public static DataTable ExecuteOne(string sqlSorgu)
        {
            if (_kanal == null) InitServer();
            lock (LockExecute)
            {
                while (_kanal != null && _kanal.State == ConnectionState.Executing)
                {
                    Thread.Sleep(10);
                }
                if (_kanal != null && _kanal.State == ConnectionState.Closed)
                {
                    _kanal.Open();
                }
                var adapter = new SqlDataAdapter(sqlSorgu, ConnectionString);
                var dt = new DataTable();
                adapter.SelectCommand = new SqlCommand(sqlSorgu, _kanal);
                adapter.Fill(dt);
                _kanal?.Close();
                return dt;
            }
        }

        public static bool ServerUnavailable()
        {
            try
            {
                InitServer();
                ExecuteOne("select 1 as test;");
                Unload();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Bir Neçə DataTable Qaytaran Sorğuları İcra Edir
        /// </summary>
        /// <param name="sqlSorgu">Sql Sorğusu</param>
        /// <returns></returns>
        public static DataTable[] ExecuteMore(string sqlSorgu)
        {
            if (_kanal == null) InitServer();
            var adapter = new SqlDataAdapter(sqlSorgu, ConnectionString);
            var ds = new DataSet();
            adapter.SelectCommand = new SqlCommand(sqlSorgu, _kanal);
            adapter.Fill(ds);
            _kanal?.Close();
            return ds.Tables.Cast<DataTable>().ToArray();
        }
    }

}