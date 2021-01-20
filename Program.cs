using System;
using System.IO;
using GZipTest.Helpers;
using GZipTest.Utils;

namespace GZipTest
{
    class Program
    {
        // GZipTest.exe compress/decompress [имя исходного файла] [имя результирующего файла]
        static int Main(string[] args)
        {
            try
            {
                var argumentsChecker = new ArgumentCheck();
                argumentsChecker.CheckArguments(args);

                var compressor = new GZipCompressor();
                switch(args[0])
                {
                    case "compress":
                        compressor.Compress(args[1], args[2]);
                        break;
                    case "decompress":
                        compressor.Decompress(args[1], args[2]);
                        break;
                }

                return 0;
            }
            catch(Exception exception)
            {
                System.Console.WriteLine(exception.Message);
                return 1;
            }
        }
    }
}
