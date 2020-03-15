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
                var summons = new List<Summon>();

                LiftService.ExecuteInstruction(Instruction.Stop, lift, summons);

                Assert.True(lift.State == LiftState.Stopped);
            }

            [Fact]
            public void WhenOpenDoors_ThenLiftDoorsOpen()
            {
                var passengerGettingOff = new Passenger(5);
                var lift = new Lift()
                {
                    Level = 5,
                    State = LiftState.Stopped,
                    Direction = Direction.Down,
                    Passengers = new List<Passenger>()
                    {
                        passengerGettingOff,
                        new Passenger(0)
                    }
                };

                var passengerGettingOn = new Passenger(0);
                var summons = new List<Summon>()
                {
                    new Summon(5, Direction.Down, new List<Passenger>(){ passengerGettingOn })
                };

                LiftService.ExecuteInstruction(Instruction.OpenDoors, lift, summons);

                Assert.True(lift.State == LiftState.DoorsOpen);
                // passengers get off
                Assert.DoesNotContain(passengerGettingOff, lift.Passengers);
                // passengers get on
                Assert.Contains(passengerGettingOn, lift.Passengers);
                // answered summons are removed
                Assert.Empty(summons);
            }

            [Fact]
            public void WhenTravelUp_ThenMoveLiftUp()
            {
                var lift = new Lift()
                {
                    Level = 5,
                    State = LiftState.Stopped
                };
                var summons = new List<Summon>();

                LiftService.ExecuteInstruction(Instruction.TravelUp, lift, summons);

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
                var summons = new List<Summon>();

                LiftService.ExecuteInstruction(Instruction.TravelDown, lift, summons);

                Assert.True(lift.State == LiftState.Moving);
                Assert.True(lift.Direction == Direction.Down);
                Assert.True(lift.Level == 4);
            }
        }
    }
}
