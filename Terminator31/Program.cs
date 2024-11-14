using System;
using System.IO;
namespace Terminator31;
class Program
{
    static void Main(string path = "E://test")
    {
        (double sizeTotal, double sizeCleaned) = Terminate(path);
        Console.WriteLine($"Size before cleaning: {sizeTotal} bytes; {Environment.NewLine}" +
            $"Size after cleaning: {sizeCleaned} bytes.");
        // ironicaly to the name, i can't test it, cause all my test subject weight 0 bytes
        Console.ReadKey();
    }
    static (double, double) Terminate(string dirName, double sizeTotal = 0, double sizeCleaned = 0)
    {
        if (Directory.Exists(dirName))
        {
            string[] files = Directory.GetFiles(dirName);
            foreach (string file in files)
            {
                try
                {
                    sizeTotal += file.Length; // why iterate twice if it looks all the same from the user perspective!
                    if (DateTime.Now - File.GetLastWriteTime(file) > TimeSpan.FromMinutes(30))
                    {
                        double size = file.Length;
                        File.Delete(file);
                        sizeCleaned += size; // so it doesn't add if there's an ex at delete attempt
                    }
                }
                catch (Exception e) { Console.WriteLine($"Error: {e}"); }
            }
            string[] dirs = Directory.GetDirectories(dirName);
            foreach (string dir in dirs)
            {
                if (DateTime.Now - Directory.GetLastWriteTime(dir) > TimeSpan.FromMinutes(30))
                {
                    try
                    {
                        (double sizeTotalNew, double sizeCleanedNew) = Terminate(dir);
                        sizeTotal += sizeTotalNew;
                        sizeCleaned += sizeCleanedNew;
                    }
                    catch (Exception e) { Console.WriteLine($"Error: {e}"); };
                    try { Directory.Delete(dir); } catch { }
                }
            }
        }
        return (sizeTotal, sizeCleaned);
    }
}