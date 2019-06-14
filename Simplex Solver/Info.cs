using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.SolverFoundation.Services;

namespace Simplex_Solver
{
    class Info
    {
        public List<double> t { get; }
        public List<double> D { get; }
        public List<double> E { get; }
        public List<double> c { get; }
        public List<double> n { get; }
        public List<Term> wt { get; }
        public List<Term> zt { get; }
        private string[] data;
        private string fileName;

        /// <summary>
        /// prepares required lists and takes the name of the input file
        /// </summary>
        /// <param name="fileName">name of input file</param>
        public Info(string fileName)
        {
            t = new List<double>();
            D = new List<double>();
            E = new List<double>();
            c = new List<double>();
            n = new List<double>();
            wt = new List<Term>();
            zt = new List<Term>();
            if (!fileName.Equals(null) && (fileName.Equals("ornek.txt") || fileName.Equals("input1.txt")
                || fileName.Equals("input2.txt")   ))
            {
                this.fileName = fileName;
            }
            else throw new ArgumentException("Invalid Argument Name");
        }

        /// <summary>
        /// reads the data in the file line by line
        /// </summary>
        /// <returns>file data</returns>
        public string[] Read()
        {
            if (fileName.Equals("ornek.txt")) Console.WriteLine("\n\n---This is demo file---\n");
            else  Console.WriteLine("\n\n---This is test file---\n");
            data= File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName);
            
            return data;
            
            
        }
        /// <summary>
        /// all the information we put into the necessary collections
        /// </summary>
        public void allInfo()
        {
            char[] delimiter = new char[] { '\t' , ' ' };
           
            int k;
            for (int i = 1; i < data.Length; i++)
            {
                

                string[] splits = data[i].Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                k = -1;
                do
                {
                    k++;
                    splits[k] = splits[k].Replace('.', ',');
                    if (splits[k].Last() == '.') splits[k] += "0";
                    switch (k)
                    {

                        case 0:
                            {

                                t.Add(Convert.ToDouble(splits[k]));
                                break;
                            }
                        case 1:
                            {
                                D.Add(Convert.ToDouble(splits[k]));
                                break;
                            }
                        case 2:
                            {
                                E.Add(Convert.ToDouble(splits[k]));
                                break;
                            }
                        case 3:
                            {
                                c.Add(Convert.ToDouble(splits[k]));
                                break;
                            }
                        case 4:
                            {
                                n.Add(Convert.ToDouble(splits[k]));
                                break;
                            }
                    }

                }while (k != 4);           
            }       
        }
        /// <summary>
        /// Bring purpose function
        /// </summary>
        /// <param name="xs">x variables</param>
        /// <param name="ys">y variables</param>
        /// <returns></returns>
        public Term getGoalTerm(Decision[] xs , Decision[] ys)
        {
            Term term =c[0] * xs[0];
            
            for (int i = 1; i < xs.Length; i++) term +=c[i] * xs[i];
            for (int i = 0; i < ys.Length; i++) term +=n[i] * ys[i];
            return term;
        }

        /// <summary>
        /// Create constraint terms for wt and zt
        /// </summary>
        /// <param name="des">x or y variables</param>
        /// <param name="count">how many pieces to add</param>
        /// <param name="startIndex"> origin</param>
        /// <returns> created term</returns>
        private Term addTerm(Decision[] des, int count,int startIndex)
        {
            Term term = des[startIndex];
            for (int i = startIndex +1 ; i < count; i++)
            {
                term = term + des[i];
            }
            
            return term;

        }
        /// <summary>
        /// creates wts and returns them in a list
        /// </summary>
        /// <param name="xs">wt-forming variables(xs)</param>
        /// <returns>list of wt terms</returns>
        public List<Term> addWt(Decision[] xs)
        {
            for (int i = 0; i < xs.Length; i++) wt.Add(addTerm(xs, i + 1, Math.Max(0,i-19)));
            return wt;
        }
        /// <summary>
        /// creates zt and returns them in a list
        /// </summary>
        /// <param name="ys">zt-forming variables(ys)</param>
        /// <returns>list of zt terms</returns>
        public List<Term> addZt(Decision[] ys)
        {
            for (int i = 0; i < ys.Length; i++) zt.Add(addTerm(ys, i + 1, Math.Max(0, i - 14)));
            return zt;
        }
    }
}
