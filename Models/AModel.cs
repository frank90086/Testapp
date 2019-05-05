using System;

namespace Test.Models
{
    public class AModel : BaseModel
    {
        public override string Name { get; set; }

        public AModel(){
            Name = "I'm A Model";
        }
    }
}