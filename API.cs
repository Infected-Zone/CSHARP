using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Principal;
using System.Windows;

namespace ZoneAuth
{
    class API
    {
        static string key;

        public static void login()
        {
            if (File.Exists("key.dat"))
            {
                key = File.ReadAllText("key.dat");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("> [+] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Insert Your Auth Key:");
                key = Console.ReadLine();
            }
            try
            {
                using (HttpRequest httpRequest = new HttpRequest())
                {
                    httpRequest.IgnoreProtocolErrors = true;
                    httpRequest.UserAgent = Http.ChromeUserAgent();
                    httpRequest.ConnectTimeout = 30000;
                    string checkauth = httpRequest.Post("https://infected-zone.com/auth.php", "type=auth&key=" + key + "&hwid=" + gethwid(), "application/x-www-form-urlencoded").ToString();
                    dynamic json = JsonConvert.DeserializeObject(checkauth);
                    string auth = json.auth;
                    if (auth != "success")
                    {
                        MessageBox.Show("Error " + json.status, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        if (File.Exists("key.dat")) File.Delete("key.dat");
                        Environment.Exit(0);
                    }
                    else
                    {
                        string secgroup = json.secgroup;
                        if (secgroup != "invalid")
                        {
                            // MessageBox.Show("Welcome " + json.username + ", enjoy your exclusive infected-zone.com tool!", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("> [+] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Auth Granted, Welcome ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(json.username);
                            Console.WriteLine();
                            File.WriteAllText("key.dat", key);
                        }
                        else
                        {
                            MessageBox.Show("You need to be Premium+ to use this tool sir.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Environment.Exit(0);
                return;
            }
        }

        private static string gethwid()
        {
            return WindowsIdentity.GetCurrent().User.Value;
        }
    }
}
