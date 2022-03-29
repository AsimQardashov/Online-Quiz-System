using System;
using System.Data.SqlClient;

namespace ExamingSystem.Controllers
{
    internal class SqlConnetction
    {
        public string ConnectionString { get; internal set; }

        internal void Open()
        {
            throw new NotImplementedException();
        }

        public static implicit operator SqlConnection(SqlConnetction v)
        {
            throw new NotImplementedException();
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }
    }
}