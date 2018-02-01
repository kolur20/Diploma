using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System
{
    class PairDBConnection
    {
        string _name;
        string _connection;
        public PairDBConnection(string nameDB, string connectionDB)
        {
            NameDB = nameDB;
            UrlDB = connectionDB;
        }
        public PairDBConnection()
        {
            NameDB = "";
            UrlDB = "";
        }
        public string NameDB
        {
            get { return _name; }
            set { _name = value; }
        }
        public string UrlDB
        {
            get { return _connection; }
            set { _connection = value; }
        }
    }
}
