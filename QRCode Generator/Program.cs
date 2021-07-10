using System;
using System.Linq;
using System.Drawing;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace QRCode_Generator
{
    class Program
    {
        public struct ChoiceP
        {
            public int Choice;
            public bool Contains;
            public ChoiceP(int choice, bool contains)
            {
                Choice = choice;
                Contains = contains;
            }
        }

        static void Main(string[] args)
        {
            ChoiceP c = Choice();
            while (c.Contains)
            {
                Console.WriteLine("Invalid input");
                c = Choice(); 
            }

            Generator(c.Choice);

            
        }
        
        static ChoiceP Choice()
        {
            int choice = Hello();
            int[] choicePerm = { 1, 2, 3 };
            
            if (choicePerm.Contains(choice)) 
            {
                ChoiceP c = new ChoiceP(choice, false);
                return c;

            } 
            else 
            {
                ChoiceP c = new ChoiceP(choice, true);
                return c;
            }
        }
        static int Hello()
        {
            Console.WriteLine("QRCode Generator");
            Console.WriteLine("Select type QRCode your get");
            Console.WriteLine("1- Text | 2- URL | 3- WiFi");
            int choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }

        static void Generator(int choice)
        {
            string payload = "";
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter Text");
                    string text = Console.ReadLine();
                    payload = text;
                    break;
                case 2:
                    Console.WriteLine("Enter URL");
                    string url = Console.ReadLine();
                    Url generator_url = new PayloadGenerator.Url(url);
                    payload = generator_url.ToString();
                    break;
                case 3:
                    WiFi.Authentication authenticationMode;
                    string ssid;
                    string password;

                    Console.WriteLine("Enter SSID of the WiFi network");
                    ssid = Console.ReadLine();

                    Console.WriteLine("Enter authentication mode");
                    Console.WriteLine("0 to WEP | 1 to WPA/WPA2 | 2 to no password ");
                    authenticationMode = (WiFi.Authentication)Convert.ToInt32(Console.ReadLine());

                    if(authenticationMode != (WiFi.Authentication)2)
                    {
                        Console.WriteLine("Enter password of the WiFi network");
                        password = Console.ReadLine();
                    } else { password = ""; }
                    
                    
                    
                    WiFi generator_wifi = new WiFi(ssid, password,authenticationMode);
                    payload = generator_wifi.ToString();
                    Console.WriteLine(payload);
                    break;

            }
            
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save("myqrcode.png", System.Drawing.Imaging.ImageFormat.Jpeg);

            Console.WriteLine("Successful");
        }
    }
}
