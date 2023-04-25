﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class MainClass
    {
        private static string responseData;
        private static Metrics metrics = new Metrics();
        private static readonly string PATH_TO_SCRIPT = "../Debug/simplesim-3.0/script.sh";
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to 'Defenetly not a virus team'.");
            Net();
            LaunchSim();
            Console.ReadLine();
        }
        public static void GenerateScript()
        {
            //string commnandBuffer = "./sim-outorder -redir:sim results/simulation.res -redir:prog results/applu_progout.res -max:inst 10000 -fastfwd 0 -fetch:ifqsize 4 -bpred:bimod 2048 -decode:width 4 -issue:width 4 -commit:width 4 -ruu:size 16 -lsq:size 8 -cache:dl1 dl1:128:32:4:l -mem:lat 18 2 -tlb:itlb itlb:16:4096:4:l -res:ialu 4 benchmarks/applu.ss < benchmarks/applu.in";
            string commnandBuffer = responseData;
            responseData = string.Empty;
            string cosmin = "/home/timarc/Desktop/MareProiect/SimOutorder/Client/bin/Debug/simplesim-3.0/";
            string alex_desktop = "/home/bellum/Projects/SimOutorder/Client/bin/Debug/simplesim-3.0/";

            File.WriteAllText(PATH_TO_SCRIPT, $"#!/bin/bash\nchmod 777 script.sh\ncd {alex_desktop}\n" + commnandBuffer);
            File.Exists(PATH_TO_SCRIPT);
        }

        public static void LaunchSim()
        {
            GenerateScript();
            string readScript = File.ReadAllText(PATH_TO_SCRIPT);

            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "./script.sh", WorkingDirectory = "../Debug/simplesim-3.0" };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();

            metrics = metrics.parseString("../Debug/simplesim-3.0/results/simulation.res");
            Console.WriteLine("Everiting is done, go in peace my son.");
        }

        public static void Net()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            Console.WriteLine("Connecting to server...");
            clientSocket.Connect(endPoint);
            Console.WriteLine("Connected to server.");

            // Receive response from server
            byte[] responseBytes = new byte[1024];
            int bytesRead = clientSocket.Receive(responseBytes);
            responseData = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);
            Console.WriteLine("Received response from server: " + responseData);

            // Send data to server
            //string data = $"{benchmarkRulatNume} {IR} {RataHitCacheDate} {RataHitCacheInstructiuni}"
            string data = $"{metrics.benchmarkName} {metrics.sim_IPC} {metrics.rataHitDL1} {metrics.rataHitDL2} {metrics.rataHitDTLB} {metrics.rataHitIL1} {metrics.rataHitIL2 } {metrics.rataHitITLB }";
            Console.WriteLine(data);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(dataBytes);

            // Close the client socket
            clientSocket.Close();
        }
        /*
        public static void Net()
        {
            // Create a client Socket
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            Console.WriteLine("Connecting to server...");
            clientSocket.Connect(endPoint);
            Console.WriteLine("Connected to server.");

            // Send data to server
            string data = "Hello from client!";
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(dataBytes);

            // Receive response from server
            byte[] responseBytes = new byte[1024];
            int bytesRead = clientSocket.Receive(responseBytes);
            responseData = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);
            Console.WriteLine("Received response from server: " + responseData);

            // Close the client socket
            clientSocket.Close();
        }
        */
    }
}
