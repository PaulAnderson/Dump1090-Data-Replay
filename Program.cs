using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ReplayDump1090Data
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Configuration
            const int SpeedFactor = 10; //1 for realtime, 10 for 10x faster etc
            const int TCPPort = 30003; // The port to listen for incoming connections on
            string filePath = "dump1090.txt"; // The path to the file with the data to replay

            // Create a new TCP listener
            TcpListener listener = new TcpListener(IPAddress.Any, TCPPort);

            // Start the listener
            listener.Start();
            Console.WriteLine("Listening for incoming connections on port {0}...", TCPPort);

            // Wait for a client to connect
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Open the file with the data to replay
            StreamReader reader = new StreamReader(filePath);

            // Read the data from the file one line at a time
            string line = reader.ReadLine();
            DateTime lastTimestamp = DateTime.MinValue;
            while (line != null)
            {
                // Extract the timestamp from the line
                Match match = Regex.Match(line, @"^.+,(\d+/\d+/\d+,\d+:\d+:\d+\.\d+),.+$");
                if (match.Success)
                {
                    DateTime timestamp = DateTime.Parse(match.Groups[1].Value);

                    // Calculate the delay between this line and the last one
                    if (lastTimestamp != DateTime.MinValue)
                    {
                        TimeSpan delay = timestamp - lastTimestamp;
                        if (SpeedFactor!=1) delay/=SpeedFactor;
                        Console.WriteLine("Delaying for {0} seconds before sending next line...", delay.TotalSeconds);
                        Thread.Sleep(delay);
                    }

                    lastTimestamp = timestamp;
                }

                // Send the line to the client
                byte[] buffer = Encoding.ASCII.GetBytes(line);
                client.GetStream().Write(buffer, 0, buffer.Length);

                // Read the next line from the file
                line = reader.ReadLine();
            }

            // Close the file and the client connection
            reader.Close();
            client.Close();

            // Stop the listener
            listener.Stop();

            Console.WriteLine("Replay finished. Last line sent at {0}.", lastTimestamp);
        }
    }
}