using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace_WPF.Data
{
    public class Result
    {
        public Result(double[,] grid = null,List<double> error = null)
        {
            Grid = grid;
            Error = error;
        }

        public double[,] Grid { get; set; }
        public List<double> Error { get; set; }

    }
}
