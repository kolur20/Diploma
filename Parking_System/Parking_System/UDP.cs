using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;



namespace Parking_System
{
    static class UDP
    {
        static int _localPort = 8085;
        static System.Threading.Thread tRec; 
        static UdpClient receivingUdpClient;

        //ждем ответ от нужного нам ip
        
        static bool _answerFlag;
        static string _answer = "";
        static IPAddress _ip;

        /*
        static CancellationTokenSource cts = new CancellationTokenSource();
        

        static void GetResponse(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            //цикл будет выполняться до тех пор, пока это свойство возвращает false;
            //оно вернет true (цикл завершится), когда кто-нибудь (в нашем случае обработчик нажатия кнопки) не потребует отмены операции
            {//слушаем порт
                IPEndPoint RemoteIpEndPoint = null;
                while (_answerFlag)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                    //проверка на то, что ответ пришел от нужного ip
                    if (RemoteIpEndPoint.Address != _ip) continue;
                    // Преобразуем и отображаем данные
                    _answer = Encoding.UTF8.GetString(receiveBytes);

                }
            }
        }
        */


        /// <summary>
        /// Запускает фоновое прослушивание порта
        /// </summary>
        public static void Start(int localPort)
        {
            _localPort = localPort;
            tRec = new System.Threading.Thread(new System.Threading.ThreadStart(UDP.Receiver));
            tRec.Start();
          
        }

        /// <summary>
        /// Останавливает фоновое прослушивание порта
        /// </summary>
        public static void Stop()
        {
            tRec.Abort();
            receivingUdpClient.Close();
        }

        



        public static void Receiver()
        {
            try
            {
                // Создаем UdpClient для чтения входящих данных
                receivingUdpClient = new UdpClient(_localPort);
                IPEndPoint RemoteIpEndPoint = null;
                while (true)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    if (returnData.Contains("-"))
                    {
                        Start(_localPort);
                        System.Windows.Forms.MessageBox.Show(returnData.ToString(), RemoteIpEndPoint.Address.ToString() + ":" + RemoteIpEndPoint.Port.ToString());

                    }
                    else if (_ip.ToString() == RemoteIpEndPoint.Address.ToString())
                    {
                        _answerFlag = true;
                        _answer = returnData;
                    }

                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), ex.Message);
            }
        }


        /*
         * создать поток
         * отправить запрос
         * получили ответ - закрыть поток
         * не получили, подождать 5 сек - закрыть поток
         * */


        public static string Write(string send, string url)
        {
            var index = url.IndexOf(':');
            return Write(send, IPAddress.Parse(url.Remove(index)), Convert.ToInt32(url.Remove(0, index + 1)));
        }

        public static string Write(string send, string ip, int port)
        {
            return Write(send, IPAddress.Parse(ip), port);
        }

         /// <summary>
         /// Отправляет на указанный ip адрес и port пакет с сообщением send
         /// </summary>
         /// <param name="send">Данные для отправки</param>
         /// <param name="ip">Аддрес получателя</param>
         /// <param name="port">Порт получателя</param>
         /// <returns></returns>
        public static string Write(string send, IPAddress ip, int port)
        {
            
            // Создаем endPoint по информации об удаленном хосте
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            try
            {
                // Преобразуем данные в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(send);
                

                //обнуляем ожидаемые переменные
                _answer = "";
                _answerFlag = false;
                _ip = ip;

                // Отправляем данные
                receivingUdpClient.Send(bytes, bytes.Length, endPoint);
                //останавливаем основной поток
                Thread.Sleep(6000);
        



                return _answerFlag ? _answer : "Устройство не отвечает";
            }
            catch (Exception ex)
            {
                Start(_localPort);
                return ex.Message;
                
            }
            
        }
    }
}
