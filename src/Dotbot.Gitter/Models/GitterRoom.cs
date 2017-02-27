using System.Diagnostics;
using Dotbot.Models;

namespace Dotbot.Gitter.Models
{
    [DebuggerDisplay("{Name,nq} {Id,nq}")]
    internal sealed class GitterRoom
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool OneToOne { get; set; }

        public Room CreateRoom()
        {
            return new Room
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
