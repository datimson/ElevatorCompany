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
        public class CalculateOptimalInstructions
        {
            [Theory]
            [MemberData(nameof(Data))]
            public void CanOptimiseInstructionsStartingAtGround(List<Summon> summons, List<Instruction> optimalInstructions)
            {
                var lift = new Lift { Level = 0 }; // 0 = Ground
                var instructions = LiftService.CalculateOptimalInstructions(lift, summons);

                Assert.Equal(instructions, optimalInstructions);
            }

            public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                /*
                 * Passenger summons lift on the ground floor.
                 * Once in, chooses to go to level 5.
                 */
                new object[] {
                    new List<Summon>
                    {
                        new Summon(0, Direction.Up, new List<Passenger> { new Passenger(5) }),
                    },
                    new List<Instruction>
                    {
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.Stop,
                        Instruction.OpenDoors
                    }
                },
                /*
                 * Passenger summons lift on level 6 to go down.
                 * A passenger on level 4 summons the lift to go down.
                 * They both choose L1.
                 */
                new object[] {
                    new List<Summon>
                    {
                        new Summon(6, Direction.Down, new List<Passenger> { new Passenger(1) }),
                        new Summon(4, Direction.Down, new List<Passenger> { new Passenger(1) }),
                    },
                    new List<Instruction>
                    {
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.Stop,
                        Instruction.OpenDoors
                    }
                },
                /*
                 * Passenger 1 summons lift to go up from L2.
                 * Passenger 2 summons lift to go down from L4.
                 * Passenger 1 chooses to go to L6.
                 * Passenger 2 chooses to go to Ground Floor
                 */
                new object[] {
                    new List<Summon>
                    {
                        new Summon(2, Direction.Up, new List<Passenger> { new Passenger(6) }),
                        new Summon(4, Direction.Down, new List<Passenger> { new Passenger(0) }),
                    },
                    new List<Instruction>
                    {
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.Stop,
                        Instruction.OpenDoors
                    }
                },
                /*
                 * Passenger 1 summons lift to go up from Ground.
                 * They choose L5.
                 * Passenger 2 summons lift to go down from L4.
                 * Passenger 3 summons lift to go down from L10.
                 * Passengers 2 and 3 choose to travel to Ground.
                 */
                new object[] {
                    new List<Summon>
                    {
                        new Summon(0, Direction.Up, new List<Passenger> { new Passenger(5) }),
                        new Summon(4, Direction.Down, new List<Passenger> { new Passenger(0) }),
                        new Summon(10, Direction.Down, new List<Passenger> { new Passenger(0) })
                    },
                    new List<Instruction>
                    {
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.TravelUp,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.Stop,
                        Instruction.OpenDoors,
                        Instruction.Stop,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.TravelDown,
                        Instruction.Stop,
                        Instruction.OpenDoors
                    }
                }
            };
        }

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
            public void WhenOpenDoorsAndNoDirection_ThenLiftDoorsOpenAndSetDireciton()
            {
                var lift = new Lift()
                {
                    Level = 5,
                    State = LiftState.Stopped,
                    Direction = null
                };

                var passengerGettingOn = new Passenger(0);
                var summons = new List<Summon>()
                {
                    new Summon(5, Direction.Down, new List<Passenger>(){ passengerGettingOn })
                };

                LiftService.ExecuteInstruction(Instruction.OpenDoors, lift, summons);

                Assert.True(lift.State == LiftState.DoorsOpen);
                // passengers get on
                Assert.Contains(passengerGettingOn, lift.Passengers);
                // answered summons are removed
                Assert.Empty(summons);
                // direction set
                Assert.True(lift.Direction == Direction.Down);
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
