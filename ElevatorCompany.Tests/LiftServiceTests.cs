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
            [Trait("State", "Stopped")]
            public void WhenStoppedAndPassengerAtDesiredLevel_ThenOpenDoors()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Passengers = new List<Passenger>() { new Passenger(1) }
                };
                var summons = new List<Summon>();
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.OpenDoors);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndSummonsAtSameLevelWithOppositeDirectionAndSummonsInSameDirection_ThenDontOpen()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = Direction.Up
                };
                var summons = new List<Summon>() { new Summon(1, Direction.Down, new List<Passenger>()), new Summon(2, Direction.Down, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.False(nextInstruction == Instruction.OpenDoors);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndSummonsAtSameLevelWithSameDirection_ThenOpenDoors()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = Direction.Up
                };
                var summons = new List<Summon>() { new Summon(1, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.OpenDoors);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndDirectionUpAndSummonsAboveWithSameDirection_ThenTravelUp()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = Direction.Up
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelUp);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndDirectionDownAndSummonsBelowWithSameDirection_ThenTravelDown()
            {
                var lift = new Lift
                {
                    Level = 3,
                    State = LiftState.Stopped,
                    Direction = Direction.Down
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Down, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelDown);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndNoDirectionAndSummonsBelow_ThenTravelDown()
            {
                var lift = new Lift
                {
                    Level = 3,
                    State = LiftState.Stopped,
                    Direction = null
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Down, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelDown);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndNoDirectionAndSummonsBelowWithDirectionUp_ThenTravelDown()
            {
                var lift = new Lift
                {
                    Level = 3,
                    State = LiftState.Stopped,
                    Direction = null
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelDown);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndNoDirectionAndSummonsAbove_ThenTravelUp()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = null
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelUp);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndNoDirectionAndSummonsAboveWithDirectionDown_ThenTravelUp()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = null
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Down, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelUp);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndDirectionDownAndNoPassengersAndNoSummonsBelowAndSummonOnLevel_ThenOpenDoors()
            {
                var lift = new Lift
                {
                    Level = 5,
                    State = LiftState.Stopped,
                    Direction = Direction.Down
                };
                var summons = new List<Summon>() { new Summon(5, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.OpenDoors);
            }

            // continue in same direction until no more summons or passengers
            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedDirectionUpAndSummonsAbove_ThenTravelUp()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = Direction.Up
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Down, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelUp);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedDirectionUpAndPassengersDesiredLevelAbove_ThenTravelUp()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Stopped,
                    Direction = Direction.Up,
                    Passengers = new List<Passenger>() { new Passenger(4) }
                };
                var summons = new List<Summon>();
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelUp);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedDirectionDownAndSummonsBelow_ThenTravelDown()
            {
                var lift = new Lift
                {
                    Level = 5,
                    State = LiftState.Stopped,
                    Direction = Direction.Down
                };
                var summons = new List<Summon>() { new Summon(2, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelDown);
            }

            [Fact]
            [Trait("State", "Stopped")]
            public void WhenStoppedAndDirectionDownAndPassengersDesiredLevelBelow_ThenTravelDown()
            {
                var lift = new Lift
                {
                    Level = 5,
                    State = LiftState.Stopped,
                    Direction = Direction.Down,
                    Passengers = new List<Passenger>() { new Passenger(2) }
                };
                var summons = new List<Summon>();
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.TravelDown);
            }


            [Fact]
            [Trait("State", "DoorsOpen")]
            public void WhenDoorsOpenAndPassengerAtDesiredLevel_ThenStayOpen()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.DoorsOpen,
                    Passengers = new List<Passenger>() { new Passenger(1) }
                };
                var summons = new List<Summon>();
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.OpenDoors);
            }

            [Fact]
            [Trait("State", "DoorsOpen")]
            public void WhenDoorsOpenAndSummonsAtSameLevelAndDirection_ThenStayOpen()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.DoorsOpen,
                    Direction = Direction.Up
                };
                var summons = new List<Summon>() { new Summon(1, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.OpenDoors);
            }


            [Fact]
            [Trait("State", "Moving")]
            public void WhenMovingAndPassengerAtDesiredLevel_ThenStop()
            {
                var lift = new Lift
                {
                    Level = 1,
                    State = LiftState.Moving,
                    Direction = Direction.Up,
                    Passengers = new List<Passenger>() { new Passenger(1) }
                };
                var summons = new List<Summon>();
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.Stop);
            }

            [Fact]
            [Trait("State", "Moving")]
            public void WhenMovingAndSummonAtLevelWithSameDirection_ThenStop()
            {
                var lift = new Lift
                {
                    Level = 5,
                    State = LiftState.Moving,
                    Direction = Direction.Down,
                    Passengers = new List<Passenger>() { new Passenger(1) }
                };
                var summons = new List<Summon>() { new Summon(5, Direction.Down, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.Stop);
            }

            [Fact]
            [Trait("State", "Moving")]
            public void WhenMovingAndSummonAtLevelWithOppositeDirectionAndNoOtherSummonsOrPassengers_ThenStop()
            {
                var lift = new Lift
                {
                    Level = 5,
                    State = LiftState.Moving,
                    Direction = Direction.Down,
                    Passengers = new List<Passenger>()
                };
                var summons = new List<Summon>() { new Summon(5, Direction.Up, new List<Passenger>()) };
                var nextInstruction = LiftService.CalculateNextInstruction(lift, summons);

                Assert.True(nextInstruction == Instruction.Stop);
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
