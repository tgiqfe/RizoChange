﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RizoChange
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE1
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public short dmOrientation;
        public short dmPaperSize;
        public short dmPaperLength;
        public short dmPaperWidth;
        public short dmScale;
        public short dmCopies;
        public short dmDefaultSource;
        public short dmPrintQuality;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    class User_32
    {
        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE1 devMode);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE1 devMode, int flags);
        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 0x01;
        public const int CDS_TEST = 0x02;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DISP_CHANGE_FAILED = -1;
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2 &&
                int.TryParse(args[0], out int tempWidth) && int.TryParse(args[1], out int tempHeight))
            {
                DEVMODE1 dm = new DEVMODE1();
                dm.dmDeviceName = new String(new char[32]);
                dm.dmFormName = new String(new char[32]);
                dm.dmSize = (short)Marshal.SizeOf(dm);
                if (User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm) != 0)
                {
                    dm.dmPelsWidth = tempWidth;
                    dm.dmPelsHeight = tempHeight;
                    int ret = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);
                    if (ret != User_32.DISP_CHANGE_FAILED)
                    {
                        User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY);
                    }
                }
            }
        }
    }
}
