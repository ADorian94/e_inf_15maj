using System;
using System.IO;

namespace radio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"1. feladat");
            Console.WriteLine($@"{Directory.GetCurrentDirectory()}\{Constants.INPUT_TXT}");
            RadioStation radio = new RadioStation();

            Console.WriteLine($"{Environment.NewLine}2. feladat");
            Console.WriteLine($"Az első üzenet küldője: {radio.GetFirstRadioStation()}");
            Console.WriteLine($"Az utolsó üzenet küldője: {radio.GetLastRadioStation()}");

            Console.WriteLine($"{Environment.NewLine}3. feladat");

            foreach (var item in radio.GetSpecialDays("farkas"))
                Console.WriteLine(item);

            Console.WriteLine($"{Environment.NewLine}4. feladat");

            foreach (var item in radio.GetStat())
                Console.WriteLine(item);

            Console.WriteLine($"{Environment.NewLine}5. feladat");
            Console.WriteLine($@"{Directory.GetCurrentDirectory()}\{Constants.OUTPUT_TXT}");
            radio.Reconstuction();

            Console.WriteLine($"{Environment.NewLine}6. feladat");
            while (true)
            {
                Console.Write(@"Day: ");
                string d = Console.ReadLine();
                Console.Write(@"StationID:");
                string sid = Console.ReadLine();
                int.TryParse(sid, out int x);
                int.TryParse(d, out int y);

                Console.WriteLine(radio.SearchData(x, y));
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
