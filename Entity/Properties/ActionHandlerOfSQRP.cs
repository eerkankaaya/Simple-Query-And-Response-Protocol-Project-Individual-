using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Properties;

public class ActionHandlerOfSQRP
{

    public static void VerifyDirectoryExistenceHandling(string Path, HeaderOfSQRP headerOfSQRP)
    {

        bool existsOfDirectory = Directory.Exists(Path);


        if (existsOfDirectory)
        {
            headerOfSQRP.StatusCode = StatusCode.EXIST;
            Console.WriteLine("Directory exists!");
        }
        else
        {
            headerOfSQRP.StatusCode = StatusCode.NOT_EXIST;
            Console.WriteLine("Directory does not exist!");
        }
    }


    public static void CheckFileExistenceHandling(string Path1, string nameOfFile, HeaderOfSQRP header)
    {

        string pathFile = Path.Combine(Path1, nameOfFile);
        bool existsFile = File.Exists(pathFile);


        if (existsFile)
        {
            header.StatusCode = StatusCode.EXIST;
            Console.WriteLine("File exists!");
        }
        else
        {
            header.StatusCode = StatusCode.NOT_EXIST;
            Console.WriteLine("File does not exist!");
        }
    }


    public static void DetermineFileModificationHandling(string Path1, string nameOfFile, DateTime timestamp, HeaderOfSQRP header)
    {

        string pathFile = Path.Combine(Path1, nameOfFile);
        bool existsFile = File.Exists(pathFile);

        if (!existsFile)
        {
            header.StatusCode = StatusCode.NOT_EXIST;
            Console.WriteLine("File does not exist!");
            return;
        }


        DateTime modifiedLast = File.GetLastWriteTime(pathFile);


        if (modifiedLast > timestamp)
        {
            header.StatusCode = StatusCode.CHANGED;
            Console.WriteLine("File has been modified after the specified timestamp!");
        }
        else
        {
            header.StatusCode = StatusCode.NOT_CHANGED;
            Console.WriteLine("File has not been modified after the specified timestamp!");
        }
    }


    public static void IdentifyModifiedFilesHandling(string Path2, string extensionFile, DateTime timestamp, HeaderOfSQRP header)
    {

        string[] files = Directory.GetFiles(Path2, "*." + extensionFile);


        foreach (string file1 in files)
        {
            DateTime ModifiedLastly = File.GetLastWriteTime(file1);
            if (ModifiedLastly > timestamp)
            {
                header.StatusCode = StatusCode.SUCCESS;
                Console.WriteLine("File {0} modified after the specified timestamp!", file1);
            }
        }
    }
}
