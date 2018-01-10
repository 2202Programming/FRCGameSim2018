using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRCSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            RobotTask[] team1 = { new RobotTask(Role.SCALE, Endgame.CLIMB),
                new RobotTask(Role.SCALE, Endgame.NOTHING),
                new RobotTask(Role.SCALE, Endgame.NOTHING)
            };

            RobotTask[] team2 = { new RobotTask(Role.SCALE, Endgame.NOTHING),
                new RobotTask(Role.SCALE, Endgame.NOTHING),
                new RobotTask(Role.SCALE, Endgame.NOTHING)
            };

            var match = new SimulateMatch(team1, team2);
            var result = match.AllocatePoints(true);
            Console.WriteLine(result.team1);
            Console.WriteLine(result.team2);
            Console.ReadKey();
        }
    }
}
