using ElevatorCompany.Models.Enums;
using System;
using System.Collections.Generic;

namespace ElevatorCompany.Models
{
    public class Lift
    {
        public int Level { get; set; } = 0;
        public LiftState State { get; set; } = LiftState.Stopped;
        public Direction? Direction { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
    }
}
