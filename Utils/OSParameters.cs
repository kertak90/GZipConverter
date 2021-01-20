using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GZipTest.Models;

namespace GZipTest.Utils
{
    public class OSParameters
    {
        public OSMemory GetMetrics()
        {
            if(IsUnix())
            {
                return GetUnixMetrics();
            }
    
            return GetWindowsMetrics();
        }
    
        private bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                        RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }
    
        private OSMemory GetWindowsMetrics()
        {
            var output = "";
    
            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;
            
            using(var process = Process.Start(info))
            {                
                output = process.StandardOutput.ReadToEnd();
            }
    
            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);
        
            var metrics = new OSMemory();
            metrics.TotalMemory = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.FreeMemory = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            
            return metrics;            
        }
    
        private OSMemory GetUnixMetrics()
        {
            var output = "";
    
            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;
            
            using(var process = Process.Start(info))
            {                
                output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }
    
            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
            var metrics = new OSMemory();
            metrics.TotalMemory = double.Parse(memory[1]);
            metrics.FreeMemory = double.Parse(memory[3]);
    
            return metrics;            
        }
    }
}   