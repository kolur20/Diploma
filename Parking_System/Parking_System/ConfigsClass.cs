using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Parking_System
{
    static class ConfigsClass
    {
        static string Config = "Config.xml";

        static string _parkingCount;
        static string _parkingSize;
        static int _localPort;
        static PairDBConnection _currentDatabase = new PairDBConnection();


        static public int LocalPort
        {
            get { return _localPort; }
            set { _localPort = value; }
        }
        static public string ParkingSize
        {
            get { return _parkingSize; }
            set { _parkingSize = value; }
        }
        static public string ParkingCount
        {
            get { return _parkingCount; }
            set { _parkingCount = value; }
        }
        static public string CurrentDatabaseName
        {
            get { return _currentDatabase.NameDB; }
            set { _currentDatabase.NameDB = value; }
        }
        static public string CurrentDatabaseUrl
        {
            get { return _currentDatabase.UrlDB; }
            set { _currentDatabase.UrlDB = value; }
        }

        /// <summary>
        /// Загрузка настроек ИЗ файла
        /// </summary>
        /// <returns></returns>
        static public bool Download()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Config;
            try
            {
                if (!Directory.Exists(path))
                {

                    
                    var doc = new XmlDocument();
                    {
                        doc.Load(path);
                        foreach (XmlNode conf in doc.SelectNodes("Config"))
                        {
                            ParkingCount = conf.SelectNodes("ParkingCount").Item(0).InnerText.ToString();
                            ParkingSize = conf.SelectNodes("ParkingSize").Item(0).InnerText.ToString();
                            foreach (XmlNode server in conf.SelectNodes("Server"))
                            {
                                CurrentDatabaseName = server.FirstChild.InnerText.ToString();
                                CurrentDatabaseUrl = server.LastChild.InnerText.ToString();
                                
                            }
                            LocalPort = Convert.ToInt32(conf.SelectNodes("LocalPort").Item(0).InnerText);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                CreateXML();
                return false;
            }
            return true;
        }


        /// <summary>
        /// Создание пустой xml
        /// </summary>
        /// <returns></returns>
        static public bool CreateXML()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Config;
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<?xml version=""1.0""?>
                        <Config>
                         <ParkingCount></ParkingCount>
                         <ParkingSize></ParkingSize>   
                         <Server>
	                        <Name></Name>
	                        <Url></Url>
                         </Server>
                        <LocalPort></LocalPort>
                        </Config>");


            doc.PreserveWhitespace = false;

            doc.Save(path);

            return true;
        }


        /// <summary>
        /// изменение текущего подключения в xml документ
        /// </summary>
        /// <param name="NameDb"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        static public bool SetConnectionDb(string NameDb, string Url)
        {
            CurrentDatabaseName = NameDb;
            CurrentDatabaseUrl = Url;
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Config;
            try
            {
                if (!Directory.Exists(path))
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    foreach (XmlNode conf in doc.SelectNodes("Config"))
                    {
                        //ParkingCount = conf.SelectNodes("ParkingCount").Item(0).InnerText.ToString();
                        foreach (XmlNode server in conf.SelectNodes("Server"))
                        {
                            server.FirstChild.InnerText = NameDb;
                            server.LastChild.InnerText = Url;

                        }
                    }
                    doc.PreserveWhitespace = false;
                    doc.Save(path);

                }
            }
            catch (FileNotFoundException)
            {
                CreateXML();
                return false;
            }
            return true;
        }

        /// <summary>
        /// изменение текущего кол-ва свободных мест
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        static public bool SetParkingCount(string count)
        {
            ParkingCount = count;
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Config;
            try
            {
                if (!Directory.Exists(path))
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    foreach (XmlNode conf in doc.SelectNodes("Config"))
                        conf.SelectNodes("ParkingCount").Item(0).InnerText = ParkingCount;
                    doc.PreserveWhitespace = false;
                    doc.Save(path);

                }
            }
            catch (FileNotFoundException)
            {
                CreateXML();
                return false;
            }
            return true;
        }

        /// <summary>
        /// изменение кол-ва мест на парковке всего
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        static public bool SetParkingSize(string count)
        {
            ParkingSize = count;
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Config;
            try
            {
                if (!Directory.Exists(path))
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    foreach (XmlNode conf in doc.SelectNodes("Config"))
                        conf.SelectNodes("ParkingSize").Item(0).InnerText = ParkingSize;
                    doc.PreserveWhitespace = false;
                    doc.Save(path);

                }
            }
            catch (FileNotFoundException)
            {
                CreateXML();
                return false;
            }
            return true;
        }


        static public bool SetLocalPort(int port)
        {
            LocalPort = port;
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Config;
            try
            {
                if (!Directory.Exists(path))
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    foreach (XmlNode conf in doc.SelectNodes("Config"))
                        conf.SelectNodes("LocalPort").Item(0).InnerText = port.ToString();
                    doc.PreserveWhitespace = false;
                    doc.Save(path);

                }
            }
            catch (FileNotFoundException)
            {
                CreateXML();
                return false;
            }
            return true;
        }
    }
}
