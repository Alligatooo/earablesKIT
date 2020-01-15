﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EarablesKIT.Models.Library
{
    class IMUDataExtractor
    {
        public static IMUDataEntry ExtractIMUDataString(byte[] data, int accScaleFactor, double gyroScaleFactor)
        {

            throw new NotImplementedException();
        }

        public static int ExtractIMURangeAccelerometer(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data2
            int byteValue = bytes[5] & 0x18;
            int Range = 0;
            // Select the right Range
            switch (byteValue)
            {
                case (0x00):
                    Range = 2;
                    break;
                case (0x08):
                    Range = 4;
                    break;
                case (0x10):
                    Range = 8;
                    break;
                case (0x18):
                    Range = 16;
                    break;
            }
            return Range;
        }
        public static int ExtractIMUScaleFactorAccelerometer(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data2
            int byteValue = bytes[5] & 0x18;
            int ScaleFactor = 0;
            // Select the right ScaleFactor
            switch (byteValue)
            {
                case (0x00):
                    ScaleFactor = 16384;
                    break;
                case (0x08):
                    ScaleFactor = 8192;
                    break;
                case (0x10):
                    ScaleFactor = 4096;
                    break;
                case (0x18):
                    ScaleFactor = 2048;
                    break;
            }
            return ScaleFactor;
        }

        public static double ExtractIMURangeGyroscope(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data2
            int byteValue = bytes[5] & 0x18;
            double Range = 0;
            // Select the right Range
            switch (byteValue)
            {
                case (0x00):
                    Range = 250;
                    break;
                case (0x08):
                    Range = 500;
                    break;
                case (0x10):
                    Range = 1000;
                    break;
                case (0x18):
                    Range = 2000;
                    break;
            }
            return Range;
        }
        public static double ExctractIMUScaleFactorGyroscope(byte[] bytes)
        {
            // Get the 3th and the 4th Bit from Data1
            int byteValue = bytes[4] & 0x18;
            double ScaleFactor = 0;
            // Select the right ScaleFactor
            switch (byteValue)
            {
                case (0x00):
                    ScaleFactor = 131;
                    break;
                case (0x08):
                    ScaleFactor = 65.5;
                    break;
                case (0x10):
                    ScaleFactor = 32.8;
                    break;
                case (0x18):
                    ScaleFactor = 16.4;
                    break;
            }
            return ScaleFactor;
        }
    }
}
