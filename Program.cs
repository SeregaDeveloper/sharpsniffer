// (Не) сниффер, создан для прослушки сети и автоматической загрузки нужной информации в БД, для её дальнейшей обработки
// любыми средствами и на любых языках.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;



namespace SharpSniffer
{
    static class Program
    {
          static void Main(string[] args)
          {
            var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork).AsEnumerable();

            // выясняем какие адреса находятся в сети на данный момент

          Console.WriteLine("Protocol\tSourceIP:Port\t===>\tDestinationIP:Port\t---Flag");

            // проходимся по всем адресам и прослушиваем их входящие и исходящие пакеты

            foreach (IPAddress ip in IPv4Addresses)
                                  Sniff(ip);

            // пока не нажмём любую клавишу

            Console.Read();
            }

            public static string ToProtocolString(this byte b)  {
       
            // выясняем тип протокола
            switch (b)
            {
                case 1: return "ICMP";
                case 2: return "IGMP";
                case 6: return "TCP";
                case 17: return "UDP";
                case 41: return "IPv6";
                case 121: return "SMP";
                default: return "#" + b.ToString();
            }
             


            }
          static void Sniff(IPAddress ip)  {
               
            // запускаем сокет на прослушку протоколов версии 4

              Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
              sck.Bind(new IPEndPoint(ip, 0));
              sck.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
              sck.IOControl(IOControlCode.ReceiveAll, new byte[4] { 1, 0, 0, 0 }, null);

            //массив для удержания данных из пакета

            //использую стандартный ip заголовок  (20byte)  + 4 + 4 + 4 +2  для парсинга необходимой части ТСР заголовка

            byte[] buffer = new byte[34];

            // используем асинхронную обработку данных

            Action<IAsyncResult> OnReceive = null;
               OnReceive = (ar) =>

                {
                   
                   
                     Console.WriteLine("{0}\t{1}:{2}\t===>\t{3}:{4}  ", buffer.Skip(9).First().ToProtocolString() 

                        , new IPAddress(BitConverter.ToUInt32(buffer, 12)).ToString() // сендер

                        , ((ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 20))).ToString() // порт с

                         , new IPAddress(BitConverter.ToUInt32(buffer, 16)).ToString() //  ресивер

                        , ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 22))).ToString() // порт р


                         );
                   
                   buffer = new byte[34]; // очистка буффера

                    sck.BeginReceive(buffer, 0, 34, SocketFlags.None, new AsyncCallback(OnReceive), null); // повтор прослушки
                };     
               
            // продолжаем прослушку сокетом
                sck.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,new AsyncCallback(OnReceive), null);
           }           
    }
}
// CREATED BY SERGEY BESEDIN
