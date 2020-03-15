using ElevatorCompany.Models;
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
                case Models.Enums.LiftState.Stopped:
                    instruction = Instruction.Travel;
                    break;
                case Models.Enums.LiftState.DoorsOpen:
                    instruction = Instruction.Stop; // close doors
                    break;
                case Models.Enums.LiftState.Moving:
                    instruction = Instruction.Stop;
                    break;
            }

            return instruction;
        }
    }
}
