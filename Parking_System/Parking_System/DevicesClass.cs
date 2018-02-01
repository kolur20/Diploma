using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Parking_System

{
    static class DevicesClass
    {
        //имя xml
        static string Devices = "Devices.xml";

        static string _ipCamera;

        static public List<PairDBConnection> ListDB = new List<PairDBConnection>();
        static public List<string> ListIpStancesIn = new List<string>();
        static public List<string> ListIpStancesOut = new List<string>();



        ///свойства
        ///
        static public string Camera
        {
            get { return _ipCamera; }
            set { _ipCamera = value; }
        }
        
        

        /// <summary>
        /// загрузка настроек ИЗ файла;
        /// </summary>
        /// <returns></returns>
        static public bool Download()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Devices;
            try
            {
                if (!Directory.Exists(path))
                {

                    ListDB.Clear();
                    var doc = new XmlDocument();
                    {
                        doc.Load(path);
                        foreach (XmlNode devices in doc.SelectNodes("Devices"))
                        {
                            Camera = devices.SelectNodes("IpCamera").Item(0).InnerText.ToString();
                            foreach (XmlNode stances in devices.SelectNodes("IpStancesIn"))
                            {
                                foreach (XmlNode stance in stances.ChildNodes)
                                {
                                    ListIpStancesIn.Add(stance.InnerText.ToString());
                                }
                            }
                            foreach (XmlNode stances in devices.SelectNodes("IpStancesOut"))
                            {
                                foreach (XmlNode stance in stances.ChildNodes)
                                {
                                    ListIpStancesOut.Add(stance.InnerText.ToString());
                                }
                            }
                            foreach (XmlNode servers in devices.SelectNodes("Servers"))
                            {
                                foreach (XmlNode server in servers.ChildNodes)
                                {
                                    ListDB.Add(new PairDBConnection(server.FirstChild.InnerText.ToString(), server.LastChild.InnerText.ToString()));
                                    if (ListDB.Last().NameDB == "" && ListDB.Last().UrlDB == "") ListDB.Remove(ListDB.Last());   
                                }
                            }
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
            path += "\\" + Devices;
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<?xml version=""1.0""?>
                        <Devices>
                        <IpCamera></IpCamera>
                        <IpStancesIn>
                        <In></In>
                        </IpStancesIn>
                        <IpStancesOut>
                        <Out></Out>
                        </IpStancesOut>
                        <Servers>
                        </Servers>
                        </Devices>");

         
            doc.PreserveWhitespace = false;
            
            doc.Save(path);
          
            return true;
        }



        /// <summary>
        /// добавление нового подключения в xml документ
        /// </summary>
        /// <param name="NameDb"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        static public bool AddConnectionUrl(string NameDb, string Url)
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Devices;
            try
            {
                if (!Directory.Exists(path))
                {
                    //добавление нового подключения в список
                    ListDB.Add(new PairDBConnection(NameDb, Url));
                    //добавление нового в xml
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNode devices = doc.SelectNodes("Devices").Item(0);
                    XmlNode servers = devices.SelectNodes("Servers").Item(0);

                    //создание нового подключения
                    var _server = doc.CreateElement("Server");
                    var _name = doc.CreateElement("Name");
                    var _url = doc.CreateElement("Url");

                    _name.AppendChild(doc.CreateTextNode(NameDb));
                    _url.AppendChild(doc.CreateTextNode(Url));
                    //добавление в server
                    _server.AppendChild(_name);
                    _server.AppendChild(_url);
                    //добавление в servers
                    servers.AppendChild(_server);
                    


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
        /// изменение данных в xml файле
        /// </summary>
        static public bool ReplaceDataXML()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\" + Devices;
            try
            {
                if (!Directory.Exists(path))
                {
                    var doc = new XmlDocument();
                    {
                        doc.Load(path);
                        foreach (XmlNode devices in doc.SelectNodes("Devices"))
                        {
                            devices.SelectNodes("IpCamera").Item(0).InnerText = Camera;

                            foreach (XmlNode stances in devices.SelectNodes("IpStancesIn"))
                            {
                                stances.RemoveAll();
                                foreach (var i in ListIpStancesIn)
                                {
                                    var stance = doc.CreateElement("Stance");
                                    stance.InnerText = i;
                                    stances.AppendChild(stance);
                                }
                        
                                
                            }
                            foreach (XmlNode stances in devices.SelectNodes("IpStancesOut"))
                            {
                                stances.RemoveAll();
                                foreach (var i in ListIpStancesOut)
                                {
                                    var stance = doc.CreateElement("Stance");
                                    stance.InnerText = i;
                                    stances.AppendChild(stance);
                                }
                               
                            }

                            foreach (XmlNode servers in devices.SelectNodes("Servers"))
                            {
                                var i = 0;
                                foreach (XmlNode server in servers.ChildNodes)
                                {
                                    server.FirstChild.InnerText = ListDB[i].NameDB;
                                    server.LastChild.InnerText = ListDB[i].UrlDB;
                                    i++;
                                }
                            }
                        }
                        doc.Save(path);
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


    }

}

