using System;
using System.Collections.Generic;

namespace Test.XSS
{
    public class FrameDirective : XSSDirective
    {
        public string XFrame { get; private set; }
        public FrameDirective(string directive) : base(directive) { }
        public override XSSDirective AllowAny()
        {
            XFrame = "";
            return base.AllowAny();
        }
        public override XSSDirective Disable()
        {
            XFrame = "deny";
            return base.Disable();
        }
        public override XSSDirective AllowSelf()
        {
            XFrame = "sameorigin";
            return base.AllowSelf();
        }

        public override XSSDirective Allow(string source)
        {
            XFrame = $"allow-from {source}";
            return base.Allow(source);
        }
    }
}