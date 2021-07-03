using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Management;

namespace FirstSeed
{
    class Program
    {
        public static int wx = Console.LargestWindowWidth, 
            wy = Console.LargestWindowHeight;

        
        
        public static void Main(string[] args)
        {

            

            Console.SetWindowSize(wx * 3/4, wy*3/4);
            Console.SetWindowPosition(0,0);

            var cd = Environment.CurrentDirectory;
            var ec = Environment.ExitCode;
            var hss = Environment.HasShutdownStarted;
            var mname = Environment.MachineName;
            var OSv = Environment.OSVersion;
            var ss = Environment.StackTrace;
            var sysdir = Environment.SystemDirectory;
            
            Console.WriteLine("CurrentDirectory: {0}", cd);

            Console.WriteLine("ExitCode: {0}", Environment.ExitCode);

            Console.WriteLine("HasShutdownStarted: {0}", Environment.HasShutdownStarted);

            Console.WriteLine("MachineName: {0}", Environment.MachineName);

            Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

            Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);

            Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);

            Console.WriteLine("TickCount: {0}", Environment.TickCount);

            Console.WriteLine("UserDomainName: {0}", Environment.UserDomainName);

            Console.WriteLine("UserInteractive: {0}", Environment.UserInteractive);

            Console.WriteLine("UserName: {0}", Environment.UserName);

            Console.WriteLine("Version: {0}", Environment.Version.ToString());

            Console.WriteLine("WorkingSet: {0}", Environment.WorkingSet);


            String query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
            Console.WriteLine("ExpandEnvironmentVariables:\n {0}", Environment.ExpandEnvironmentVariables(query));

            Console.WriteLine("GetEnvironmentVariable:\n  My temporary directory is {0}.", Environment.GetEnvironmentVariable("TEMP"));

            Console.WriteLine("GetEnvironmentVariables: ");
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                Console.WriteLine("  {0} = {1}", de.Key, de.Value);
            }

            Console.WriteLine("GetFolderPath: {0}", Environment.GetFolderPath(Environment.SpecialFolder.System));

            String[] drives = Environment.GetLogicalDrives();
            Console.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));

            //WMI Temp get
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\WMI",
                    "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("MSAcpi_ThermalZoneTemperature instance");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Active: {0}", queryObj["Active"]);
                    if (queryObj["ActiveTripPoint"] == null)
                        Console.WriteLine("ActiveTripPoint: {0}", queryObj["ActiveTripPoint"]);
                    else
                    {
                        UInt32[] arrActiveTripPoint = (UInt32[])(queryObj["ActiveTripPoint"]);
                        foreach (UInt32 arrValue in arrActiveTripPoint)
                        {
                            Console.WriteLine("ActiveTripPoint: {0}", arrValue);
                        }
                    }
                    Console.WriteLine("ActiveTripPointCount: {0}", queryObj["ActiveTripPointCount"]);
                    Console.WriteLine("CriticalTripPoint: {0}", queryObj["CriticalTripPoint"]);
                    Console.WriteLine("CurrentTemperature: {0}", queryObj["CurrentTemperature"]);
                    Console.WriteLine("InstanceName: {0}", queryObj["InstanceName"]);
                    Console.WriteLine("PassiveTripPoint: {0}", queryObj["PassiveTripPoint"]);
                    Console.WriteLine("Reserved: {0}", queryObj["Reserved"]);
                    Console.WriteLine("SamplingPeriod: {0}", queryObj["SamplingPeriod"]);
                    Console.WriteLine("ThermalConstant1: {0}", queryObj["ThermalConstant1"]);
                    Console.WriteLine("ThermalConstant2: {0}", queryObj["ThermalConstant2"]);
                    Console.WriteLine("ThermalStamp: {0}", queryObj["ThermalStamp"]);
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }

            

            Console.Read();
            dbconnect.Run(Environment.CurrentDirectory);
            return;
        }
    }
}
