using ElevatorCompany.Models;
using ElevatorCompany.Models.Enums;
using System;

namespace ElevatorCompany.Services
{
    public class LiftService
    {
        public static Instruction CalculateNextInstruction(Lift lift)
        {
            Instruction instruction = Instruction.Stop;

            switch (lift.State)
            {
                case LiftState.Stopped:
                    instruction = Instruction.TravelUp;
                    break;
                case LiftState.DoorsOpen:
                    instruction = Instruction.Stop; // close doors
                    break;
                case LiftState.Moving:
                    instruction = Instruction.Stop;
                    break;
            }

            return instruction;
        }

        public static void ExecuteInstruction(Instruction instruction, Lift lift)
        {
            switch (instruction)
            {
                case Instruction.Stop:
                    lift.State = LiftState.Stopped;
                    break;
                case Instruction.OpenDoors:
                    lift.State = LiftState.DoorsOpen;
                    break;
                case Instruction.TravelUp:
                    lift.State = LiftState.Moving;
                    lift.Direction = Direction.Up;
                    lift.Level++;
                    break;
                case Instruction.TravelDown:
                    lift.State = LiftState.Moving;
                    lift.Direction = Direction.Down;
                    lift.Level--;
                    break;
            }
        }
    }
}
