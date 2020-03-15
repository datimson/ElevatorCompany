using ElevatorCompany.Models;
using ElevatorCompany.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorCompany.Services
{
    public class LiftService
    {
        public static List<Instruction> CalculateOptimalInstructions(Lift lift, List<Summon> summons)
        {
            var instructions = new List<Instruction>();

            int maxIterations = 1000; // just in case
            int counter = 0;
            while (summons.Any() || lift.Passengers.Any())
            {
                var nextInstruction = CalculateNextInstruction(lift, summons);
                instructions.Add(nextInstruction);

                ExecuteInstruction(nextInstruction, lift, summons);

                counter++;
                if (counter > maxIterations)
                {
                    throw new Exception("Exceeded max iterations");
                }
            }

            return instructions;
        }

        public static Instruction CalculateNextInstruction(Lift lift, List<Summon> summons)
        {
            Instruction instruction = Instruction.Stop;

            switch (lift.State)
            {
                case LiftState.Stopped:
                    // let passengers off?
                    if (lift.Passengers.Any(x => x.DesiredLevel == lift.Level))
                    {
                        instruction = Instruction.OpenDoors;
                        break;
                    }

                    // collect more passengers that are going the same direction
                    if (summons.Any(x => x.Level == lift.Level && (!lift.Direction.HasValue || x.Direction == lift.Direction)))
                    {
                        instruction = Instruction.OpenDoors;
                        break;
                    }

                    // there is a summons to go other direction
                    // no reason to keep going this direction
                    if (summons.Any(x => x.Level == lift.Level)
                        && ((lift.Direction == Direction.Down && !summons.Any(x => x.Level < lift.Level)) || (lift.Direction == Direction.Up && !summons.Any(x => x.Level > lift.Level))))
                    {
                        instruction = Instruction.OpenDoors;
                        break;
                    }

                    // continue down
                    if (lift.Direction == Direction.Down && (lift.Passengers.Any(x => x.DesiredLevel < lift.Level) || summons.Any(x => x.Level < lift.Level)))
                    {
                        instruction = Instruction.TravelDown;
                        break;
                    }

                    // continue up
                    if (lift.Direction == Direction.Up && (lift.Passengers.Any(x => x.DesiredLevel > lift.Level) || summons.Any(x => x.Level > lift.Level)))
                    {
                        instruction = Instruction.TravelUp;
                        break;
                    }

                    // should change direction?
                    if ((!lift.Direction.HasValue || lift.Direction == Direction.Up) && !(lift.Passengers.Any(x => x.DesiredLevel > lift.Level) || summons.Any(x => x.Level > lift.Level)))
                    {
                        instruction = Instruction.TravelDown;
                        break;
                    }
                    if ((!lift.Direction.HasValue || lift.Direction == Direction.Down) && !(lift.Passengers.Any(x => x.DesiredLevel < lift.Level) || summons.Any(x => x.Level < lift.Level)))
                    {
                        instruction = Instruction.TravelUp;
                        break;
                    }
                    break;
                case LiftState.DoorsOpen:
                    // should stay open?
                    if (summons.Any(x => x.Level == lift.Level && (!lift.Direction.HasValue || x.Direction == lift.Direction))
                        || lift.Passengers.Any(x => x.DesiredLevel == lift.Level))
                    {
                        instruction = Instruction.OpenDoors;
                        break;
                    }

                    instruction = Instruction.Stop; // close doors
                    break;
                case LiftState.Moving:
                    // stop to drop passengers off?
                    if (lift.Passengers.Any(x => x.DesiredLevel == lift.Level))
                    {
                        instruction = Instruction.Stop;
                        break;
                    }
                    // stop to pickup more passengers going the same direction
                    if (summons.Any(x => x.Level == lift.Level && x.Direction == lift.Direction))
                    {
                        instruction = Instruction.Stop;
                        break;
                    }
                    // stop to change direction
                    if ((lift.Direction == Direction.Up && !(lift.Passengers.Any(x => x.DesiredLevel > lift.Level) || summons.Any(x => x.Level > lift.Level)))
                        || (lift.Direction == Direction.Down && !(lift.Passengers.Any(x => x.DesiredLevel < lift.Level) || summons.Any(x => x.Level < lift.Level))))
                    {
                        instruction = Instruction.Stop;
                        break;
                    }

                    // keep going
                    instruction = lift.Direction == Direction.Up ? Instruction.TravelUp : Instruction.TravelDown;
                    break;
            }

            return instruction;
        }

        public static void ExecuteInstruction(Instruction instruction, Lift lift, List<Summon> summons)
        {
            switch (instruction)
            {
                case Instruction.Stop:
                    lift.State = LiftState.Stopped;
                    if (!summons.Any() && !lift.Passengers.Any())
                    {
                        lift.Direction = null;
                        break;
                    }

                    // there is a summons to go Down
                    // no reason to keep going Up
                    if (summons.Any(x => x.Level == lift.Level) && lift.Direction == Direction.Up && !summons.Any(x => x.Level > lift.Level))
                    {
                        lift.Direction = Direction.Down;
                        break;
                    }

                    // there is a summons to go Up
                    // no reason to keep going Down
                    if (summons.Any(x => x.Level == lift.Level) && lift.Direction == Direction.Down && !summons.Any(x => x.Level < lift.Level))
                    {
                        lift.Direction = Direction.Down;
                        break;
                    }
                    break;
                case Instruction.OpenDoors:
                    lift.State = LiftState.DoorsOpen;
                    // passengers get off
                    lift.Passengers.RemoveAll(x => x.DesiredLevel == lift.Level);
                    // set the direction if not already heading in one
                    lift.Direction = lift.Direction ?? summons.First(x => x.Level == lift.Level).Direction;
                    // passengers get on
                    lift.Passengers.AddRange(summons.Where(x => x.Level == lift.Level && x.Direction == lift.Direction)
                        .SelectMany(x => x.Passengers));
                    // remove answered summons
                    summons.RemoveAll(x => x.Level == lift.Level && x.Direction == lift.Direction);
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
