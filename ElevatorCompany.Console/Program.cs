using ElevatorCompany.Models;
using ElevatorCompany.Models.Enums;
using ElevatorCompany.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorCompany.LiftConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Elevator Company Tests");
            Console.WriteLine("-----------------------------------------");

            var lift = new Lift();
            var testA = new List<Summon>
            {
                new Summon(0, Direction.Up, new List<Passenger>() { new Passenger(5) })
            };

            var instructions = LiftService.CalculateOptimalInstructions(lift, testA);
            Console.WriteLine("Test A");
            PrintInstructions(instructions);

            lift = new Lift();
            var testB = new List<Summon>
            {
                new Summon(6, Direction.Down, new List<Passenger> { new Passenger(1) }),
                new Summon(4, Direction.Down, new List<Passenger> { new Passenger(1) }),
            };

            instructions = LiftService.CalculateOptimalInstructions(lift, testB);
            Console.WriteLine("Test B");
            PrintInstructions(instructions);

            lift = new Lift();
            var testC = new List<Summon>
            {
                new Summon(2, Direction.Up, new List<Passenger> { new Passenger(6) }),
                new Summon(4, Direction.Down, new List<Passenger> { new Passenger(0) }),
            };

            instructions = LiftService.CalculateOptimalInstructions(lift, testC);
            Console.WriteLine("Test C");
            PrintInstructions(instructions);

            lift = new Lift();
            var testD = new List<Summon>
            {
                new Summon(0, Direction.Up, new List<Passenger> { new Passenger(5) }),
                new Summon(4, Direction.Down, new List<Passenger> { new Passenger(0) }),
                new Summon(10, Direction.Down, new List<Passenger> { new Passenger(0) })
            };

            instructions = LiftService.CalculateOptimalInstructions(lift, testD);
            Console.WriteLine("Test D");
            PrintInstructions(instructions);

            Console.ReadLine();
        }

        public static void PrintInstructions(List<Instruction> instructions)
        {
            var index = 1;
            foreach (var instruction in instructions)
            {
                Console.WriteLine($"    {index}. {instruction}");
                index++;
            }
        }
    }
}
