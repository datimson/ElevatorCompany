using ElevatorCompany.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorCompany.Models
{
    public class Summon
    {
        public readonly int Level;
        public readonly Direction Direction;
        public readonly List<Passenger> Passengers;

        public Summon(int level, Direction direction, List<Passenger> passengers)
        {
            Level = level;
            Direction = direction;
            Passengers = passengers;
        }
    }
}
