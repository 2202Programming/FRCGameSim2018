using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRCSimulation
{
    class SimulateMatch
    {
        private RobotTask[][] teams;
        private double[][] teamFocus;
        private int team1Vault;
        private int team2Vault;

        public SimulateMatch(RobotTask[] team1, RobotTask[] team2)
        {
            teams = new RobotTask[2][];
            teams[0] = team1;
            teams[1] = team2;
            //3 is the number of switches to place blocks on
            teamFocus = new double[2][];
            teamFocus[0] = new double[3];
            teamFocus[1] = new double[3];

            CalcFocusPercentage();
        }

        private void CalcFocusPercentage()
        {
            //Assign focus for the scale and the two switches also count blocks to the exchange
            for (int i = 0; i < teams[0].Length; i++)
            {
                if (teams[0][i].role != Role.DEFENSE && teams[0][i].role != Role.EXCHANGE)
                {
                    UpdateFocus(0, i);
                }

                if(teams[0][i].role == Role.EXCHANGE)
                {
                    team1Vault = 9;
                }
            }

            for (int i = 0; i < teams[1].Length; i++)
            {
                if (teams[1][i].role != Role.DEFENSE && teams[1][i].role != Role.EXCHANGE)
                {
                    UpdateFocus(1, i);
                }

                if(teams[1][i].role == Role.EXCHANGE)
                {
                    team2Vault = 9;
                }
            }

            //Decrease the focus on the scale due to defense
            for (int i = 0; i < teams[0].Length; i++)
            {
                if(teams[0][i].role == Role.DEFENSE)
                {
                    teamFocus[1][1] *= Constants.DEFENSEMULT;
                }

                if (teams[1][i].role == Role.DEFENSE)
                {
                    teamFocus[0][1] *= Constants.DEFENSEMULT;
                }
            }
        }

        private void UpdateFocus(int allianceNumber, int teamPosition)
        {
            teamFocus[allianceNumber][(int)teams[allianceNumber][teamPosition].role]++;
            if (teams[allianceNumber][teamPosition].endgame == Endgame.PARK)
            {
                teamFocus[allianceNumber][(int)teams[allianceNumber][teamPosition].role] -= Constants.PARKFOCUS;
            }
            if (teams[allianceNumber][teamPosition].endgame == Endgame.CLIMB)
            {
                teamFocus[allianceNumber][(int)teams[allianceNumber][teamPosition].role] -= Constants.CLIMBFOCUS;
            }
        }

        //If the parameter is true then team 1 captured the scale first otherwise team 2 captured it first
        //It is assumed that each team will capture their own switch first
        public MatchResult AllocatePoints(bool scaleCapture)
        {
            int team1Points = 0;
            int team2Points = 0;

            team1Points += team1Vault * 5;
            team2Points += team2Vault * 5;

            //give points for alliance 1's switch
            if (teamFocus[0][0] != 0 || teamFocus[1][0] != 0)
            {
                if (teamFocus[0][0] > teamFocus[1][0])
                {
                    team1Points += 135;
                }
                else
                {
                    double percentCapture = teamFocus[0][0] / (teamFocus[0][0] + teamFocus[1][0]);
                    team1Points += (int)Math.Round(135 * percentCapture);
                }
            }

            //give points for the scale
            if (teamFocus[0][1] != 0 || teamFocus[1][1] != 0)
            {
                if ((teamFocus[0][1] > teamFocus[1][1] == scaleCapture) && (teamFocus[0][1] != teamFocus[1][1]))
                {
                    if (teamFocus[0][1] > teamFocus[1][1])
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
                    double percentCapture = teamFocus[0][1] / (teamFocus[0][1] + teamFocus[1][1]);
                    team1Points += (int)Math.Round(135 * percentCapture);
                    percentCapture = teamFocus[1][1] / (teamFocus[0][1] + teamFocus[1][1]);
                    team2Points += (int)Math.Round(135 * percentCapture);
                }
            }

            //give points for alliance 2's switch
            if (teamFocus[0][2] != 0 || teamFocus[1][2] != 0)
            {
                if (teamFocus[1][2] > teamFocus[0][2])
                {
                    team2Points += 135;
                }
                else
                {
                    double percentCapture = teamFocus[1][2] / (teamFocus[0][2] + teamFocus[1][2]);
                    team2Points += (int)Math.Round(135 * percentCapture);
                }
            }

            //give points for endgame activities
            for(int i = 0; i < teams[0].Length; i++)
            {
                switch (teams[0][i].endgame)
                {
                    case Endgame.PARK:
                        team1Points += 5;
                        break;
                    case Endgame.CLIMB:
                        team1Points += 30;
                        break;
                }

                switch (teams[1][i].endgame)
                {
                    case Endgame.PARK:
                        team2Points += 5;
                        break;
                    case Endgame.CLIMB:
                        team2Points += 30;
                        break;
                }
            }

            MatchResult result = new MatchResult();
            result.team1 = team1Points;
            result.team2 = team2Points;
            return result;
        }
    }
}
