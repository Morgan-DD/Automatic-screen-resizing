using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace screenResolution_load
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string screenSettingsTitle = "MyConfig";//name of the configuration
            string tempDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + "\\temp\\" + screenSettingsTitle;//path to a temp file
            string outputFileName = "\\test.xml";//name of the configuration file
            string sharePath = "\\\\Path\\To\\My\\Share";//path of the share where is the MonitorSwitcher exe
            string todayDate = DateTime.Now.ToString("dd-MM-yyyy(hh-mm-ss)");//date for the folder name on the share
            string destinationPath = sharePath + "\\" + screenSettingsTitle + "\\" + todayDate;// path of the distination the the share

            //If the temp directory does not exist, we create it.
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);//create the directory
            }

            //start MonitorSwitcher with the paramter for saving the configuration
            Process.Start(sharePath + "\\MonitorSwitcher.exe", "-save:" + tempDir + outputFileName);

            //If the directory on the share does not exist, we create it.
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);//create the directory
            }

            destinationPath += outputFileName;//destination path on the share with the fileName

            Thread.Sleep(1000);//Sleep to prevent bugs

            File.Copy(tempDir + outputFileName, destinationPath);//copy the xml configuration file on the share

            Directory.Delete(Path.GetDirectoryName(tempDir), true);//delet the temporary folder and all its content

        }
    }
}
