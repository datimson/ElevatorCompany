using ElevatorCompany.Models.Enums;
using System;

namespace ElevatorCompany.Models
{
    public class Lift
    {
        public int Level { get; set; } = 0;
        public LiftState State { get; set; } = LiftState.Stopped;
        public Direction? Direction { get; set; }
    }
}
