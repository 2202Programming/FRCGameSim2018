using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRCSimulation
{
    class SimulateMatch
    {
        private const int NUM_SWITCHES = 3;
        
        private Role[] team1;
        private Role[] team2;
        private double[] team1Focus;
        private double[] team2Focus;
        private int team1Vault;
        private int team2Vault;

        public SimulateMatch(Role[] team1, Role[] team2)
        {
            this.team1 = team1;
            this.team2 = team2;
            //3 is the number of switches to place blocks on
            team1Focus = new double[NUM_SWITCHES];
            team2Focus = new double[NUM_SWITCHES];

            CalcFocusPercentage();
        }

        private void CalcFocusPercentage()
        {
            foreach (var t in team1)
            {
                if (t != Role.DEFENSE && t != Role.EXCHANGE)
                    team1Focus[(int)t]++;

                if(t == Role.EXCHANGE)
                {
                    team1Vault = 9;
                }
            }

            foreach (var t in team2)
            {
                if (t != Role.DEFENSE && t != Role.EXCHANGE)
                    team2Focus[(int)t]++;

                switch (t)
                {
                    case Role.DEFENSE:
                        team1Focus[1] *= Constants.DEFENSEMULT;
                        break;
                    case Role.EXCHANGE:
                        team2Vault = 9;
                        break;
                }
            }

            foreach (var t in team1)
            {
                if(t == Role.DEFENSE)
                {
                    team2Focus[1] *= Constants.DEFENSEMULT;
                }
            }
        }

        //If the parameter is true then team 1 captured the scale first otherwise team 2 captured it first
        //It is assumed that each team will capture their own switch first
        public MatchResult AllocatePoints(bool scaleCapture)
        {
            var team1Points = 0;
            var team2Points = 0;

            team1Points += team1Vault * 5;
            team2Points += team2Vault * 5;
            
            if (team1Focus[0] != 0 || team2Focus[0] != 0)
            {
                if (team1Focus[0] > team2Focus[0])
                {
                    team1Points += 135;
                }
                else
                {
                    double percentCapture = team1Focus[0] / (team1Focus[0] + team2Focus[0]);
                    team1Points += (int)Math.Round(135 * percentCapture);
                }
            }

            if (team1Focus[1] != 0 || team2Focus[1] != 0)
            {
                if ((team1Focus[1] > team2Focus[1] == scaleCapture) && (team1Focus[1] != team2Focus[1]))
                {
                    if (team1Focus[1] > team2Focus[1])
                    {
                        team1Points += 135;
                    }
                    else
                    {
                        team2Points += 135;
                    }
                }
                else
                {
                    double percentCapture = team1Focus[1] / (team1Focus[1] + team2Focus[1]);
                    team1Points += (int)Math.Round(135 * percentCapture);
                    percentCapture = team2Focus[1] / (team1Focus[1] + team2Focus[1]);
                    team2Points += (int)Math.Round(135 * percentCapture);
                }
            }

            if (team1Focus[2] != 0 || team2Focus[2] != 0)
            {
                if (team2Focus[2] > team1Focus[2])
                {
                    team2Points += 135;
                }
                else
                {
                    double percentCapture = team2Focus[2] / (team1Focus[2] + team2Focus[2]);
                    team2Points += (int)Math.Round(135 * percentCapture);
                }
            }

            return new MatchResult
            {
                Team1 = team1Points,
                Team2 = team2Points
            };
        }
    }
}
