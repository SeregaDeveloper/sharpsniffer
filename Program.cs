// Shiffer for Windows 10. 
// Made for collect IP and TCP packet data into MySQL Database

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
            // Get hosts from our network
              
              var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork).AsEnumerable();

            

          Console.WriteLine("Protocol\tSourceIP:Port\t===>\tDestinationIP:Port\t---Flag");

            // Listen each packet to each address 

            foreach (IPAddress ip in IPv4Addresses)
                                  Sniff(ip);

            // While any the button is not pressed

            Console.Read();
            }

            public static string ToProtocolString(this byte b)  {
       
            // Detect the type of the protocol
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
               
            // Start sniffing for ipv4 protocol packets

              Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
              sck.Bind(new IPEndPoint(ip, 0));
              sck.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
              sck.IOControl(IOControlCode.ReceiveAll, new byte[4] { 1, 0, 0, 0 }, null);

            // Data Array
            // Use standart IP header  (20byte) and the part of TCP header (14byte) for parsing

            byte[] buffer = new byte[34];

            

            Action<IAsyncResult> OnReceive = null;
               OnReceive = (ar) =>

                {
                   
                   
                     Console.WriteLine("{0}\t{1}:{2}\t===>\t{3}:{4}  ", buffer.Skip(9).First().ToProtocolString() 

                        , new IPAddress(BitConverter.ToUInt32(buffer, 12)).ToString() // Sender IP

                        , ((ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 20))).ToString() // Sender Port

                         , new IPAddress(BitConverter.ToUInt32(buffer, 16)).ToString() //  Reciever IP

                        , ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 22))).ToString() // Reciever Port


                         );
                   
                   buffer = new byte[34]; // Clean the buffer

                    sck.BeginReceive(buffer, 0, 34, SocketFlags.None, new AsyncCallback(OnReceive), null); // Repeat listening
                };     
               
      
                sck.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,new AsyncCallback(OnReceive), null);
           }           
    }
}

