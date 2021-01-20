using System;
using System.IO;

namespace GZipTest.Helpers
{
    public class ArgumentCheck
    {
        public void CheckArguments(string[] args)
        {
            if(args.Length == 3)
            {
                if(args[0] != "compress" && args[0] != "decompress")
                {
                    throw new ArgumentException($"No such command: {args[0]}");
                }

                if(!File.Exists(args[1]))
                {
                    throw new FileNotFoundException($"File not exist: {args[1]}");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Wrong number of arguments");
            }
        }
    }
}