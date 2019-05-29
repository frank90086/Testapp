using System;
using System.Collections.Generic;

namespace Test.XSS
{
    public class XSSDirective
    {
        private readonly string _directive;
        public XSSDirective(string directive)
        {
            _directive = directive;
        }

        private List<string> _source { get; set; } = new List<string>();
        public virtual XSSDirective AllowAny() => Allow("*");
        public virtual XSSDirective Disable() => Allow("'none'");
        public virtual XSSDirective AllowSelf() => Allow("'self'");

        public virtual XSSDirective Allow(string source)
        {
            _source.Add(source);
            return this;
        }

        public override string ToString() => _source.Count > 0 ? $"{_directive} {String.Join(" ", _source)}; " : "";
    }
}