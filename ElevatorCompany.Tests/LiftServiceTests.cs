using ElevatorCompany.Models;
using ElevatorCompany.Models.Enums;
using ElevatorCompany.Services;
using System;
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
                var lift = new Lift() {
                    State = LiftState.Moving
                };

                LiftService.ExecuteInstruction(Instruction.Stop, lift);

                Assert.True(lift.State == LiftState.Stopped);
            }
        }
    }
}
