using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using Entity.Properties;

namespace ServerConnection;

class Server
{
    static void Main(string[] args)
    {
        StartServerFunc();
    }

    static void StartServerFunc()
    {
        IPAddress ipAddress = IPAddress.Any;
        int portNumber = 31369;

        TcpListener listenerTcp = new TcpListener(ipAddress, portNumber);

        try
        {
            listenerTcp.Start();
            Console.WriteLine("Server has been started. Waiting for connections...");

            while (true)
            {
                TcpClient tcpClient = listenerTcp.AcceptTcpClient();
                Console.WriteLine("Client connected successfully.");

                HandleClient(tcpClient);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            listenerTcp.Stop();
        }
    }

    static void HandleClient(TcpClient tcpClient)
    {
        try
        {
            NetworkStream stream = tcpClient.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string requestData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received request: {requestData}");


            string[] requestParts = requestData.Split(':');
            HeaderOfSQRP receivedHeader = new HeaderOfSQRP();
            receivedHeader.HeaderData = ulong.Parse(requestParts[0]);

            BodyFormatOfSQRP receivedBody = new BodyFormatOfSQRP();
            receivedBody.SetBody(requestParts[1]);




            byte[] responseHeaderBytes = BitConverter.GetBytes(receivedHeader.HeaderData);
            stream.Write(responseHeaderBytes, 0, responseHeaderBytes.Length);

            byte[] responseData = Encoding.ASCII.GetBytes(receivedBody.ContentBodyOfSQRP);
            stream.Write(responseData, 0, responseData.Length);

            Console.WriteLine("Response sent successfully.");

            tcpClient.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while handling client request: {ex.Message}");
        }
    }
}
