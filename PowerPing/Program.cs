﻿/**************************************************************************
 * PowerPing - Advanced command line ping tool
 * Written by Matthew Carney [matthewcarney64@gmail.com]
 * ************************************************************************/
/*
MIT License

Copyright (c) 2017 Matthew Carney

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Linq;

namespace PowerPing
{
    class Program
    {
        private static Ping p = new Ping();
        private static Graph g = null;

        static void Main(string[] args)
        {
            // Local variables 
            int curArg = 0;
            bool addrFound = false;
            string opMode = ""; 
            PingAttributes attributes = new PingAttributes();
            attributes.Address = "";

            // Setup console
            Display.DefaultForegroundColor = Console.ForegroundColor;
            Display.DefaultBackgroundColor = Console.BackgroundColor;

            // Show current version info
            Display.Version();

            // Check if no arguments
            if (args.Length == 0)
            {
                PowerPing.Display.Help();
                return;
            }

            // Loop through arguments
            try
            {
                for (int count = 0; count < args.Length; count++)
                {
                    curArg = count;

                    switch (args[count])
                    {
                        //case "/version":
                        //case "-version":
                        //case "--version":
                        //case "/v":
                        //case "-v":
                        //case "--v":
                        //    Display.Version();
                        //    Environment.Exit(0);
                        //    break;
                        case "/count":
                        case "-count":
                        case "--count":
                        case "/c":
                        case "-c":
                        case "--c": // Ping count
                            attributes.Count = Convert.ToInt32(args[count + 1]);
                            break;
                        case "/infinite":
                        case "-infinite":
                        case "--infinite":
                        case "/t":
                        case "-t":
                        case "--t": // Infinitely send
                            attributes.Continous = true;
                            break;
                        case "/timeout":
                        case "-timeout":
                        case "--timeout":
                        case "/w":
                        case "-w":
                        case "--w": // Timeout
                            attributes.Timeout = Convert.ToInt32(args[count + 1]);
                            break;
                        case "/message":
                        case "-message":
                        case "--message":
                        case "/m":
                        case "-m":
                        case "--m": // Message
                            if (args[count + 1].Contains("--") || args[count + 1].Contains("//") || args[count + 1].Contains("-"))
                                throw new ArgumentFormatException();
                            attributes.Message = args[count + 1];
                            break;
                        case "/ttl":
                        case "-ttl":
                        case "--ttl":
                        case "/i":
                        case "-i":
                        case "--i": // Time To Live
                            attributes.Ttl = Convert.ToInt16(args[count + 1]);
                            break;
                        case "/interval":
                        case "-interval":
                        case "--interval":
                        case "/in":
                        case "-in":
                        case "--in": // Interval
                            attributes.Interval = Convert.ToInt32(args[count + 1]);
                            break;
                        case "/type":
                        case "-type":
                        case "--type":
                        case "/pt":
                        case "-pt":
                        case "--pt": // Ping type
                            attributes.Type = Convert.ToByte(args[count + 1]);
                            break;
                        case "/code":
                        case "-code":
                        case "--code":
                        case "/pc":
                        case "-pc":
                        case "--pc": // Ping code
                            attributes.Code = Convert.ToByte(args[count + 1]);
                            break;
                        case "/displaymsg":
                        case "-displaymsg":
                        case "--displaymsg":
                        case "/dm":
                        case "-dm":
                        case "--dm": // Display packet message
                            Display.DisplayMessage = true;
                            break;
                        case "/ipv4":
                        case "-ipv4":
                        case "--ipv4":
                        case "/4":
                        case "-4":
                        case "--4": // Force ping with IPv4
                            if (attributes.ForceV6)
                                // Reset IPv6 force if already set
                                attributes.ForceV6 = false;
                            attributes.ForceV4 = true;
                            break;
                        case "/ipv6":
                        case "-ipv6":
                        case "--ipv6":
                        case "/6":
                        case "-6":
                        case "--6": // Force ping with IPv6
                            if (attributes.ForceV4)
                                // Reset IPv4 force if already set
                                attributes.ForceV4 = false;
                            attributes.ForceV6 = true;
                            break;
                        case "/help":
                        case "-help":
                        case "--help":
                        case "/?":
                        case "-?":
                        case "--?": // Display help message
                            PowerPing.Display.Help();
                            Environment.Exit(0);
                            break;
                        case "/examples":
                        case "-examples":
                        case "--examples":
                        case "/ex":
                        case "-ex":
                        case "--ex": // Displays examples
                            PowerPing.Display.Examples();
                            break;
                        case "/shorthand":
                        case "-shorthand":
                        case "--shorthand":
                        case "/sh":
                        case "-sh":
                        case "--sh": // Use short hand messages
                            Display.Short = true;
                            break;
                        case "/nocolor":
                        case "-nocolor":
                        case "--nocolor":
                        case "/nc":
                        case "-nc":
                        case "--nc": // No color mode
                            Display.NoColor = true;
                            break;
                        case "/noinput":
                        case "-noinput":
                        case "--noinput":
                        case "/ni":
                        case "-ni":
                        case "--ni": // No input mode
                            Display.NoInput = true;
                            break;
                        case "/decimals":
                        case "-decimals":
                        case "--decimals":
                        case "/dp":
                        case "-dp":
                        case "--dp": // Decimal places
                            if (Convert.ToInt32(args[count + 1]) > 3 || Convert.ToInt32(args[count + 1]) < 0)
                                throw new ArgumentFormatException();
                            Display.DecimalPlaces = Convert.ToInt32(args[count + 1]);
                            break;
                        case "/timestamp":
                        case "-timestamp":
                        case "--timestamp":
                        case "/ts":
                        case "-ts":
                        case "--ts": // Display timestamp
                            Display.TimeStamp = true;
                            break;
                        case "/timing":
                        case "-timing":
                        case "--timing":
                        case "/ti":
                        case "-ti":
                        case "--ti": // Timing option
                            switch (args[count + 1].ToLower()) 
                            {
                                case "0":
                                case "paranoid":
                                    attributes.Timeout = 10000;
                                    attributes.Interval = 300000;
                                    break;
                                case "1": 
                                case "sneaky":
                                    attributes.Timeout = 5000;
                                    attributes.Interval = 120000;
                                    break;
                                case "2":
                                case "quiet":
                                    attributes.Timeout = 5000;
                                    attributes.Interval = 30000;
                                    break;
                                case "3":
                                case "polite":
                                    attributes.Timeout = 3000;
                                    attributes.Interval = 3000;
                                    break;
                                case "4":
                                case "nimble":
                                    attributes.Timeout = 2000;
                                    attributes.Interval = 750;
                                    break;
                                case "5":
                                case "speedy":
                                    attributes.Timeout = 1500;
                                    attributes.Interval = 500;
                                    break;
                                case "6":
                                case "insane":
                                    attributes.Timeout = 750;
                                    attributes.Interval = 100;
                                    break;
                                default: // Unknown timing type
                                    throw new ArgumentFormatException();
                            }
                            break;
                        case "/whoami":
                        case "-whoami":
                        case "--whoami": // Current computer location
                            opMode = "whoami";
                            break;
                        case "/location":
                        case "-location":
                        case "--location":
                        case "/loc":
                        case "-loc":
                        case "--loc": // Location lookup
                            opMode = "location";
                            break;
                        case "/listen":
                        case "-listen":
                        case "--listen":
                        case "/li":
                        case "-li":
                        case "--li": // Listen for ICMP packets
                            opMode = "listening";
                            break;
                        case "/graph":
                        case "-graph":
                        case "--graph":
                        case "/g":
                        case "-g":
                        case "--g": // Graph view
                            opMode = "graphing";
                            break;
                        case "/compact":
                        case "-compact":
                        case "--compact":
                        case "/cg":
                        case "-cg":
                        case "--cg": // Compact graph view
                            opMode = "compactgraph";
                            break;
                        case "/flood":
                        case "-flood":
                        case "--flood":
                        case "/fl":
                        case "-fl":
                        case "--fl": // Flood
                            opMode = "flooding";
                            break;
                        case "/scan":
                        case "-scan":
                        case "--scan":
                        case "/sc":
                        case "-sc":
                        case "--sc": // Scan
                            opMode = "scanning";
                            break;
                        default:
                            // Check for invalid argument 
                            if ((args[count].Contains("--") || args[count].Contains("/") || args[count].Contains("-")) 
								& !opMode.Equals("scanning") // (ignore if scanning)
								& args[count].Length < 7) // Ignore args under 7 chars (assume they are an address)
                                throw new ArgumentFormatException();
                            break;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                PowerPing.Display.Error("Missing Argument Parameter", false, false, false);
                PowerPing.Display.Message(" @ \"PowerPing >>>" + args[curArg] + "<<<\"", ConsoleColor.Red);
                PowerPing.Display.Message("Use \"PowerPing /help\" or \"PowerPing /?\" for help.");
                Helper.Pause();
                return;
            }
            catch (OverflowException)
            {
                PowerPing.Display.Error("Overflow while converting", false, false, false);
                PowerPing.Display.Message(" @ \"PowerPing " + args[curArg] + ">>>" + args[curArg + 1] + "<<<\"", ConsoleColor.Red);
                PowerPing.Display.Message("Use \"PowerPing /help\" or \"PowerPing /?\" for help.");
                Helper.Pause();
                return;
            }
            catch (ArgumentFormatException)
            {
                PowerPing.Display.Error("Invalid argument or missing parameter", false, false, false);
                PowerPing.Display.Message(" @ \"PowerPing >>>" + args[curArg] + "<<<\"", ConsoleColor.Red);
                PowerPing.Display.Message("Use \"PowerPing /help\" or \"PowerPing /?\" for help.");
                Helper.Pause();
                return;
            }
            catch (Exception e)
            {
                PowerPing.Display.Error("A " + e.GetType().ToString().Split('.').Last() + " Exception Occured", false, false, false);
                PowerPing.Display.Message(" @ \"PowerPing >>>" + args[curArg] + "<<<\"", ConsoleColor.Red);
                PowerPing.Display.Message("Use \"PowerPing /help\" or \"PowerPing /?\" for more info.");
                Helper.Pause();
                return;
            }

            // Find address
            if (opMode.Equals("") || opMode.Equals("flooding") || opMode.Equals("graphing") || opMode.Equals("compactGraph") || opMode.Equals("location"))
            {
                if (Uri.CheckHostName(args.First()) == UriHostNameType.Unknown && Uri.CheckHostName(args.Last()) == UriHostNameType.Unknown)
                    PowerPing.Display.Error("PowerPing could not find a host address.", true, true);

                if (Uri.CheckHostName(args.First()) == UriHostNameType.Unknown)
                    attributes.Address = args.Last();
                else
                    attributes.Address = args.First();
            }

            // Add Control C event handler 
            if (opMode.Equals("") || opMode.Equals("flooding") || opMode.Equals("graphing") || opMode.Equals("compactgraph")) // add graphing and compact graphing
                Console.CancelKeyPress += new ConsoleCancelEventHandler(ExitHandler);

            // Select correct function using opMode 
            switch (opMode)
            {
                case "listening":
                    p.Listen();
                    break;
                case "location":
                    Helper.GetAddressLocation(attributes.Address, true);
                    break;
                case "whoami":
                    Helper.GetAddressLocation("", true);
                    break;
                case "graphing":
                    g = new Graph(attributes.Address);
                    g.Start();
                    break;
                case "compactgraph":
                    g = new Graph(attributes.Address);
                    g.CompactGraph = true;
                    g.Start();
                    break;
                case "flooding":
                    p.Flood(attributes.Address);
                    break;
                case "scanning":
                    p.Scan(args.Last());
                    break;
                case "":
                    // Send ping normally
                    p.Send(attributes);
                    break;
            }
           
        }

        protected static void ExitHandler(object sender, ConsoleCancelEventArgs args) // Event handler for control - c
        {
            // Cancel termination
            args.Cancel = true;

            // Stop ping
            p.Dispose();

            // Stop graph if it is running
            if (g != null)
                g.Dispose();

            // Reset console colour
            Display.ResetColor();
            Console.CursorVisible = true;
        }
    }
}
