using System;

namespace Test.Models
{
    public class BModel : BaseModel
    {
        public override string Name { get; set; }

        public BModel(){
            Name = "I'm B Model";
        }
    }
}