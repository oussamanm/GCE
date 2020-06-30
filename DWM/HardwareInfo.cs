using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Management;

public static class HardwareInfo
{
    /// <summary>
    /// Retrieving Processor Id.
    /// </summary>
    /// <returns></returns>
    /// 

    public static string GetBIOSserNo()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("SerialNumber").ToString();
            }

            catch { }

        }

        return "BIOS Serial Number: Unknown";

    }


}