namespace GZipTest.Utils.Interfaces
{
    public interface ICompressor
    {
        public void Compress(string inputFileName, string outputFileName, int blockSize = 10);
        public void Decompress(string inputFileName, string outputFileName, int blockSize = 10);
    }
}