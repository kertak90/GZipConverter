using System;
using System.IO;
using System.IO.Compression;
using GZipTest.Utils.Interfaces;

namespace GZipTest.Utils
{
    public class GZipCompressor : ICompressor
    {
        private OSParameters _parameters;
        public GZipCompressor()
        {
            _parameters = new OSParameters();
        }
        public void Compress(string inputFileName, string outputFileName, int blockSize = 10)
        {
            var memoryMetriks = _parameters.GetMetrics();
            System.Console.WriteLine($"FreeMemory: {memoryMetriks.FreeMemory}");
            System.Console.WriteLine($"TotalMemory: {memoryMetriks.TotalMemory}");

            var fileSize = new FileInfo(inputFileName).Length / 1024 / 1024;        //file size in Mb
            System.Console.WriteLine($"FileSize: {fileSize}");

            var usedMemory = memoryMetriks.TotalMemory - memoryMetriks.FreeMemory;
            if(usedMemory < 0 || usedMemory == 0)
                throw new ArgumentException($"OS used memory less or equal 0: {usedMemory}");

            var avaliableMemory = 0;
            var usedPercentage = usedMemory / memoryMetriks.TotalMemory;
            if( usedPercentage > 0.75)
            {
                avaliableMemory = Convert.ToInt16(memoryMetriks.FreeMemory) / 2;
                if(avaliableMemory < blockSize)
                    throw new OutOfMemoryException($"Lack of RAM. Memory less than: {blockSize}");
            }
            else if(usedPercentage <= 0.75)
            {
                avaliableMemory = Convert.ToInt16(memoryMetriks.TotalMemory * 0.75 - usedMemory);
                if(avaliableMemory < blockSize)
                {
                    avaliableMemory = Convert.ToInt16(memoryMetriks.FreeMemory) / 2;
                    if(avaliableMemory < blockSize)
                        throw new OutOfMemoryException($"Lack of RAM. Memory less than: {blockSize}");
                }
            }
            var numberOfThreads = avaliableMemory / blockSize;

            var buffer = new byte[blockSize * 1024 * 1024];
            var read = 0;
            var blockIndex = 0;
            using(var FS = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                var chunk = 0;
                while((chunk = FS.Read(buffer, read, buffer.Length)) > 0)
                {
                    read += chunk;
                    var copy = new byte[blockSize * 1024 * 1024];
                    Array.Copy(buffer, copy, read);
                    CompressTask(ref copy, $"{outputFileName}_{blockIndex++}");
                }
            }
        }
        public void CompressTask(ref byte[] array, string fileName)
        {
            using (FileStream comp = new FileStream(fileName + ".gz", FileMode.Create))
            {
                using (GZipStream inStream = new GZipStream(comp, CompressionMode.Compress))
                {
                    inStream.Write(array, 0, array.Length);
                    inStream.Close();
                }
                comp.Close();
            }
        }
        public void Decompress(string inputFileName, string outputFileName, int blockSize = 10)
        {
            var avaliableMemory = _parameters.GetMetrics();
            System.Console.WriteLine($"FreeMemory: {avaliableMemory.FreeMemory}");
            System.Console.WriteLine($"TotalMemory: {avaliableMemory.TotalMemory}");
        }
    }
}