using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorCompany.Models
{
    public class Passenger
    {
        public readonly int DesiredLevel;

        public Passenger(int desiredLevel)
        {
            DesiredLevel = desiredLevel;
        }
    }
}
