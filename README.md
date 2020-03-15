# Elevator Company
## Control the travel of a lift for a 10 storey building

Your task is to write a program to control the travel of a lift for a 10 storey building.
A passenger can summon the lift to go up or down from any floor. Once in the lift, they can choose the floor they'd like to travel to.
Your program needs to plan the optimal set of instructions for the lift to travel, stop, and open its doors.

### Some test cases:
- <strong>A)</strong> Passenger summons lift on the ground floor. Once in, chooses to go to level 5.
- <strong>B)</strong> Passenger summons lift on level 6 to go down. A passenger on level 4 summons the lift to go down. They both choose L1.
- <strong>C)</strong> Passenger 1 summons lift to go up from L2. Passenger 2 summons lift to go down from L4. Passenger 1 chooses to go to L6. Passenger 2 chooses to go to Ground Floor
- <strong>D)</strong> Passenger 1 summons lift to go up from Ground. They choose L5. Passenger 2 summons lift to go down from L4. Passenger 3 summons lift to go down from L10. Passengers 2 and 3 choose to travel to Ground.

### How to run this
This repository contains a Visual Studio solution. There is a Console project named ElevatorCompany.LiftConsole and when run this will print the lift instructions for each of the above test cases. There is also an xUnit project with a bunch of Unit Tests named ElevatorCompany.Tests. .Net Core 3.1 is required.

### Implementation Decisions

#### Definition of Optimal
The lift should travel in the same direction to the highest or lowest floor for any summons or
passenger before changing direction, and collect any passengers along the way who also want to travel in the same direction.

#### 0 is considered Bottom/Ground Floor

#### LiftState.Stop is also Closed Doors

For example, a set of instructions might look like this:

1. TravelUp
2. TravelUp
3. Stop
4. OpenDoors
5. Stop

Here the final instruction to Stop is saying to go back to the stopped state or 'Close Doors'.
