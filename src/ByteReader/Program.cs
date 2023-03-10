using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace ByteReader
{
    internal class Program
    {
        private static string inPath;
        private static string outPath;
        private static List<byte> inBytes;
        private static bool saveToFile;
        private static int mode;
        private static bool writeBytesToConsole;
        private static bool running = true;

        private static void Main(string[] args)
        {
            MainLoop();
        }

        private static void MainLoop()
        {
            while (running)
            {
            startofloop:
                Console.WriteLine("\nWhat mode do you want to use? 0=readbytes 1=createfilefrombytefile");

                try
                {
                    mode = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a Number!");
                    goto startofloop;
                }

                if (mode == 0)
                {
                    ReadBytes();
                }
                else
                {
                    CreatByteFileFromText();
                }

                running = false;
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void CreatByteFileFromText()
        {
            Console.WriteLine("Please enter the file of the input byte file down below:");

            inPath = StringExt.RemoveStringSigns(Console.ReadLine());

            var fileLines = File.ReadAllLines(inPath);

            inBytes = fileLines.Select(byte.Parse).ToList();

            Console.WriteLine("Please enter the out path and name + ending of the file below:");

            outPath = StringExt.RemoveStringSigns(Console.ReadLine());

            Console.WriteLine("Creating file: " + outPath);

            Thread.Sleep(1000);

            File.WriteAllBytes(outPath, SevenZip.Compression.LZMA.SevenZipHelper.Decompress(inBytes.ToArray()));

            Console.WriteLine("Created file with success!");
        }

        private static void ReadBytes()
        {
            Console.WriteLine("Enter the file path of the file you want to read the bytes from: ");

            inPath = StringExt.RemoveStringSigns(Console.ReadLine());

            Console.WriteLine("Reading bytes...");

            Thread.Sleep(1000);

            inBytes = SevenZip.Compression.LZMA.SevenZipHelper.Compress(File.ReadAllBytes(inPath)).ToList();

            Console.WriteLine("Do you want to output the bytes to the console? (true=yes false=no)");

            writeBytesToConsole = Convert.ToBoolean(Console.ReadLine());

            if (writeBytesToConsole)
            {
                Console.WriteLine("\nBytes:");

                foreach (var b in inBytes)
                {
                    Console.Write(b + " ");
                }
            }
            Console.WriteLine("\nDo you want to save the bytes to a File? (true=yes false=no)");

            saveToFile = Convert.ToBoolean(Console.ReadLine());

            Console.WriteLine("");

            if (saveToFile)
            {
                Console.WriteLine("Please enter the file out path: ");
                outPath = Console.ReadLine();

                var file = File.Create(outPath);
                file.Close();

                using (var sw = new StreamWriter(outPath))
                {
                    foreach (var item in inBytes)
                    {
                        sw.WriteLine(item + " ");
                    }
                }

                Console.WriteLine("Done!");
            }
        }
    }

    internal static class StringExt
    {
        public static string RemoveStringSigns(string target)
        {
            if (target.Contains("\""))
            {
                target = target.Replace('\"', ' ');
            }

            return target;
        }
    }
}