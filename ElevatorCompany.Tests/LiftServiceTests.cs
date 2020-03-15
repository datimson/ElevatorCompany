using ElevatorCompany.Models;
using ElevatorCompany.Models.Enums;
using ElevatorCompany.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ElevatorCompany.Tests
{
    public class LiftServiceTests
    {
        public class CalculateNextInstruction
        {
            [Fact]
            public void When_Then()
            {

            }
        }

        public class ExecuteInstruction
        {
            [Fact]
            public void WhenStop_ThenLiftStopped()
            {
                var lift = new Lift()
                {
                    State = LiftState.Moving
                };

                LiftService.ExecuteInstruction(Instruction.Stop, lift);

                Assert.True(lift.State == LiftState.Stopped);
            }

            [Fact]
            public void WhenOpenDoors_ThenLiftDoorsOpen()
            {
                var lift = new Lift()
                {
                    Level = 5,
                    State = LiftState.Stopped,
                    Passengers = new List<Passenger>()
                    {
                        new Passenger(5),
                        new Passenger(0)
                    }
                };

                LiftService.ExecuteInstruction(Instruction.OpenDoors, lift);

                Assert.True(lift.State == LiftState.DoorsOpen);
                Assert.DoesNotContain(lift.Passengers, x => x.DesiredLevel == lift.Level);
            }

            [Fact]
            public void WhenTravelUp_ThenMoveLiftUp()
            {
                var lift = new Lift()
                {
                    Level = 5,
                    State = LiftState.Stopped
                };

                LiftService.ExecuteInstruction(Instruction.TravelUp, lift);

                Assert.True(lift.State == LiftState.Moving);
                Assert.True(lift.Direction == Direction.Up);
                Assert.True(lift.Level == 6);
            }

            [Fact]
            public void WhenTravelDown_ThenMoveLiftDown()
            {
                var lift = new Lift()
                {
                    Level = 5,
                    State = LiftState.Stopped
                };

                LiftService.ExecuteInstruction(Instruction.TravelDown, lift);

                Assert.True(lift.State == LiftState.Moving);
                Assert.True(lift.Direction == Direction.Down);
                Assert.True(lift.Level == 4);
            }
        }
    }
}
