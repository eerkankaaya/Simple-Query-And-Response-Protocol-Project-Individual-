using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;
using Entity.Properties;

namespace ClientConnection;


class Client
{
    static void Main(string[] args)
    {
        ConnectionToServer();
    }

    static void ConnectionToServer()
    {
        try
        {
            string serverIpAddress = "127.0.0.1";
            int serverPort = 31369;

            TcpClient client = new TcpClient();

            client.Connect(serverIpAddress, serverPort);
            Console.WriteLine("Connected to the server successfully.");


            HeaderOfSQRP requestHeader = new HeaderOfSQRP();
            requestHeader.typeOfMessages = TypeOfMessages.Query;
            requestHeader.QueryType = TypeOfQueries.VerifyDirectoryExistence;

            BodyFormatOfSQRP requestBody = new BodyFormatOfSQRP();
            requestBody.SetBody("C:\\eerkaa\\directory");


            string requestData = $"{requestHeader.HeaderData}:{requestBody.ContentBodyOfSQRP}";
            byte[] requestDataBytes = Encoding.ASCII.GetBytes(requestData);

            NetworkStream nStream = client.GetStream();
            nStream.Write(requestDataBytes, 0, requestDataBytes.Length);
            Console.WriteLine("Request sent.");


            byte[] headerBuffer = new byte[8];
            nStream.Read(headerBuffer, 0, headerBuffer.Length);
            ulong responseHeaderData = BitConverter.ToUInt64(headerBuffer, 0);

            HeaderOfSQRP responseHeader = new HeaderOfSQRP();
            responseHeader.HeaderData = responseHeaderData;


            byte[] bodyBuffer = new byte[responseHeader.BodyLength];
            nStream.Read(bodyBuffer, 0, bodyBuffer.Length);
            string responseBody = Encoding.ASCII.GetString(bodyBuffer);
            Console.WriteLine($"Server Response: {responseBody}");


            StatusCode statusCode = responseHeader.StatusCode;
            switch (statusCode)
            {
                case StatusCode.SUCCESS:
                    Console.WriteLine("SUCCESS");
                    break;
                case StatusCode.NOT_EXIST:
                    Console.WriteLine("NOT_EXIST");
                    break;
                case StatusCode.DIRECTORY_NEEDED:
                    Console.WriteLine("DIRECTORY_NEEDED");
                    break;
                case StatusCode.CHANGED:
                    Console.WriteLine("CHANGED");
                    break;
                case StatusCode.NOT_CHANGED:
                    Console.WriteLine("NOT_CHANGED.");
                    break;
                case StatusCode.EXIST:
                    Console.WriteLine("EXIST");
                    break;

                default:
                    Console.WriteLine("Unknown status code.");
                    break;
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
