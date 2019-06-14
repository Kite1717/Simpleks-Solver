using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Services;


namespace Simplex_Solver
{
    class GetDecision
    {
        /// <summary>
        /// get x variables
        /// </summary>
        /// <param name="count">x amount </param>
        /// <returns></returns>
       public Decision[] getX(int count)
        {
            Decision[] ret = new Decision[count];
            for (int i = 1; i <=count; i++)
            {
                ret[i-1]= new Decision(Domain.RealNonnegative, "x" + i);
            }
            return ret;

        }
        /// <summary>
        /// get y variables
        /// </summary>
        /// <param name="count">y amount</param>
        /// <returns></returns>
        public Decision[] getY(int count)
        {
            Decision[] ret = new Decision[count];
            for (int i = 1; i <= count; i++)
            {
                ret[i - 1] = new Decision(Domain.RealNonnegative, "y" + i);
            }
            return ret;

        }
    }
}
