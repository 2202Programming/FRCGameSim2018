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
            Role[] team1 = { Role.OPPONENTSWITCH, Role.EXCHANGE, Role.DEFENSE };
            Role[] team2 = { Role.TEAMSWITCH, Role.EXCHANGE, Role.DEFENSE };
            var match = new SimulateMatch(team1, team2);
            var result = match.AllocatePoints(false);
            Console.WriteLine(result.Team1);
            Console.WriteLine(result.Team2);
            Console.ReadKey();
        }
    }
}
