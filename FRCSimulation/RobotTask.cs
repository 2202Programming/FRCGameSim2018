using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRCSimulation
{
    class RobotTask
    {
        public Role role
        {
            get;
            set;
        }

        public Endgame endgame
        {
            get;
            set;
        }

        public RobotTask(Role role, Endgame endgame)
        {
            this.role = role;
            this.endgame = endgame;
        }
    }
}
