﻿using System;
using System.Collections.Generic;

namespace ObsControlMobile.Models
{
    public static class ColorDictionary
    {
        public static Dictionary<string, string> FilterColors = new Dictionary<string, string>
            {
                {"R","crimson"},
                {"G","LightGreen"},
                {"B","CornflowerBlue"},
                {"L","lightsteelblue"},
                {"Ha","mediumorchid"},
                {"Sii","rosybrown"},
                {"Oiii","darkolivegreen"},
            };
    }

    public class IQPItem
    {
        //Xamarin specific
        public string Id { get; set; }
        public string Color
        {
            get {
                string retst = "";
                if (!ColorDictionary.FilterColors.TryGetValue(this.ImageFilter, out retst)) retst = "White";
                return retst;
            }
        }
        public DateTime DateObsMsk
        {
            get {
                DateTime MSK = AsrtoUtils.Conversion.DateTimeUtils.ConvertToLocal(DateObsUTC);
                return MSK;
            }
        }   

        public string Description { get; set; } //not used

        //DSS
        public Int32 StarsNumber { get; set; }
        public Double SkyBackground { get; set; }
        public double MeanRadius { get; set; }
        public double AspectRatio { get; set; }

        //FITS
        //DATE-OBS
        DateTime dateobsutc;
        public DateTime DateObsUTC {
            get { return dateobsutc; }
            set {
                dateobsutc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }   
        public double ImageExposure { get; set; }   //EXPOSURE
        public string ImageFilter { get; set; }   //FILTER
        public string ImageType { get; set; }   //IMAGETYP

        public double ImageBinningX { get; set; }   //XBINNING
        public double ImageBinningY { get; set; }   //YBINNING

        public double ImageSetTemp { get; set; }   //SET-TEMP
        public double ImageTemp { get; set; }   //CCD-TEMP

        public double CameraPixelSizeX { get; set; }   //XPIXSZ
        public double CameraPixelSizeY { get; set; }   //YPIXSZ

        public string ObjName { get; set; }    //OBJECT
        public string ObjRA { get; set; }      //OBJCTRA
        public string ObjDec { get; set; }     //OBJCTDEC
        public double ObjAlt { get; set; }     //OBJCTALT
        public double ObjAz { get; set; }      //OBJCTAZ

        public string CameraName { get; set; }   //INSTRUME    
        public string Observer { get; set; }    //OBSERVER
        public string TelescopeName { get; set; }      //TELESCOP
        public double TelescopeFocusLen { get; set; }      //FOCALLEN
        public double TelescopeDiameter { get; set; }      //APTDIA

        //FileSystem and Calculated
        public string FITSFileName { get; set; }
        public double PixelResolution { get; set; }

        public double FWHM { get; set; }
    }
}