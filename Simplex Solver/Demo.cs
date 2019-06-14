using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Services;

namespace Simplex_Solver
{
    class Demo
    {


        public void getDemo()
        {
            Console.WriteLine("Enter the name of input file");


            //Singleton Design Pattern
            var solver = SolverContext.GetContext();

            // Create empty model
            var model = solver.CreateModel();

            //get data
            string[] data;
            string input = Console.ReadLine();

            //valid file name control
            try
            {
                new Info(input);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message + "\nProcess failed.");
                Console.ReadKey();
                return;
            }
            Info info = new Info(input);



            //read data from file
            data = info.Read();


            //all the information we put into the necessary collections
            info.allInfo();



            //create variables
            Decision[] xs = new GetDecision().getX(data.Length - 1);
            Decision[] ys = new GetDecision().getY(data.Length - 1);

            //add xs and ys
            model.AddDecisions(xs);
            model.AddDecisions(ys);

            // I addded Wts and Zts
            List<Term> wt = info.addWt(xs);
            List<Term> zt = info.addZt(ys);
            //ı adeed constraints
            for (int i = 0; i < wt.Count; i++) model.AddConstraint(("Constraint1_" + i), info.D[i] <= wt[i] + zt[i] + info.E[i]);
            for (int i = 0; i < wt.Count; i++) model.AddConstraint(("Constraint2_" + i), zt[i] <= 0.2 * (wt[i] + zt[i] + info.E[i]));

            // I took the  expression from term denominated of z equation
            model.AddGoal("Goal", GoalKind.Minimize, info.getGoalTerm(xs, ys));

            //Report and solution part
            // SimplexDirective = Instruction, orders. It's about how you're going to work on what you want to figure out
            SimplexDirective simplex = new SimplexDirective();
            Solution solution = solver.Solve(simplex);


            //print report
            Console.WriteLine("--------------------------------------------");
            Report report = solution.GetReport();
            Console.Write("{0}", report);
            Console.WriteLine("--------------------------------------------\n\n");


            //our comment
            getFormat();

            //I got the variables into an array
            double[] varResults = new double[xs.Length * 2];
            int k = 0;
            foreach (Decision d in solution.Decisions)
            {
                varResults[k] = d.ToDouble();
              
                k++;
            }
            double totalCost = 0.0;

            k = 0;
            int m = varResults.Length / 2;

            //using the array in the specified format
            for (int i = 0; i < varResults.Length/2; i++)
            {
                Console.WriteLine( "\n\n"+info.t[k] + ".year ->> ("+ info.c[k] +  "  *  " + varResults[k]  + ")  =  " + info.c[k] * varResults[k]);
                Console.WriteLine(info.t[k] + ".year ->> (" + info.n[k] + "  *  " + varResults[m] + ")  =  " + info.n[k] * varResults[m]);
                Console.WriteLine(info.t[k] + ".year ->>  " + ((info.c[k] * varResults[k]) + (info.n[k] * varResults[m])) + "\n\n");
              
                totalCost += (info.c[k] * varResults[k]) + (info.n[k] * varResults[m]);
                k++; m++;
            }
            Console.WriteLine("\n\nFinal Total Cost = " + totalCost);
                

            Console.ReadKey();
        }
        /// <summary>
        /// the description of variables and coefficients according to the given assignment
        /// </summary>
        private void getFormat()
        {
            Console.WriteLine("------Format-------\n" +
         "t.year ->> (< cost to produce 1MW using coal capacity > * < amount of coal capacity brought on line in year t.> )  = cost for coal production for year t.\n" +
         "t.year ->> (< cost to produce 1MW using nuclear capacity > * < amount of nuclear capacity brought on line in year t.> )  = cost for nuclear production for year t.\n" +
         "t.year ->> Total cost for year t.  =  <Total Cost> \n...\n...\n...\nFinal Total Cost for all year = <total cost>\n");
            Console.WriteLine("-------------------------------------\n\n");
        }
        

    }
}
