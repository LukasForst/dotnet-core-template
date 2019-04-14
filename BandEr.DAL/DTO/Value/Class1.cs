using System;
using System.Collections.Generic;
using System.Text;

namespace BandEr.DAL.DTO
{
    class Class1
    {
    }

    public class ValueDetailDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; }
    }

    public class ValueListDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int OwnerId { get; set; }
    }

    public class AddValueDto
    {
        public string Value { get; set; }
    }

    public class UpdateValueDto
    {
        public string Value { get; set; }
    }
}
