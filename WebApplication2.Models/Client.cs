using MessagePack;
using System;

namespace WebApplication2
{
    [MessagePackObject]
    public class Client
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public DateTime Date { get; set; }
    }
}
