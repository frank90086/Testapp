using System;
using System.Collections.Generic;
using Test.Interface;

namespace Test.XSS
{
    public class XSSOptions
    {
        public bool ReadOnly { get; set; }
        public XSSDirective Defaults { get; set; } = new XSSDirective("default-src");
        public XSSDirective Connects { get; set; } = new XSSDirective("connect-src");
        public XSSDirective Fonts { get; set; } = new XSSDirective("font-src");
        public XSSDirective Frames { get; set; } = new XSSDirective("frame-src");
        public XSSDirective Images { get; set; } = new XSSDirective("img-src");
        public XSSDirective Media { get; set; } = new XSSDirective("media-src");

        public XSSDirective Objects { get; set; } = new XSSDirective("object-src");
        public XSSDirective Scripts { get; set; } = new XSSDirective("script-src");
        public XSSDirective Styles { get; set; } = new XSSDirective("style-src");
        public string ReportURL { get; set; }
        public FrameDirective FrameAncestors { get; set; } = new FrameDirective("frame-ancestors");
    }
}