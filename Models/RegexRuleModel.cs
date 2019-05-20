using System;

namespace Test.Models
{
    public class RegexRuleModel : IRegexRule
    {
        public string RegexRule { get; set; }

        public RegexRuleModel(){
            RegexRule = @"(?:\s)\b(?:insert|update|delete)\b";
        }
    }
}