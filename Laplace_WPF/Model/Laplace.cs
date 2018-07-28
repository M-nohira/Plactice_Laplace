using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace_WPF.Model
{
    static class Laplace
    {
        public static Data.Result Subsititution(Data.Condition condition, int dpi, ref int iterate, double omega = 0, double[,] grid = null, double conv = 0)
        {
            if (grid == null)
                grid = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];

            double[,] temp = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];
            int cnt = 0;

            var errors = new List<double>();

            Parallel.For(0, iterate, (i, LoopState) =>
            {
                double max = 0;
                for (int x = 1; x + 1 < condition.X_LMax * dpi; x++)
                {
                    for (int y = 1; y + 1 < condition.Y_LMax * dpi; y++)
                    {
                        if (x >= condition.X_Pos * dpi && y >= condition.Y_Pos * dpi)
                            if (x <= (condition.X_LMax - condition.X_Pos) * dpi && y <= (condition.Y_LMax - condition.Y_Pos) * dpi)
                            {
                                grid[x, y] = 1;
                                continue;
                            }
                        double data = grid[x, y];
                        if (omega == 0)
                        {
                            temp[x, y] = 0.25 * (grid[x + 1, y] + grid[x - 1, y] + grid[x, y + 1] + grid[x, y - 1]);
                            max = Math.Abs(data - temp[x, y]) > max ? Math.Abs(data - temp[x, y]) : max;
                            continue;
                        }
                        temp[x, y] = grid[x, y] + omega * (0.25 * (grid[x + 1, y] + grid[x - 1, y] + grid[x, y + 1] + grid[x, y - 1]) - grid[x, y]);
                        max = Math.Abs(data - temp[x, y]) > max ? Math.Abs(data - temp[x, y]) : max;
                        //errors.Add(max);
                    }
                }


                grid = temp;

                if (conv != 0)
                    if (max < conv)

                        LoopState.Stop();
                //cnt = i;
                lock (errors)
                    lock ((object)cnt)
                    {
                        cnt++;
                        errors.Add(max);
                    }

            });
            iterate = cnt;
            return new Data.Result(grid, errors);
        }

        public static Data.Result Gauss(Data.Condition condition, int dpi, ref int iterate, double omega = 0, double[,] grid = null, double conv = 0)
        {
            if (grid == null) grid = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];
            double[,] temp = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];

            var errors = new List<double>();

            int cnt = 0;
            Parallel.For(0, iterate, (i, loopState) =>
              {
                  double[,] gridOld = grid;
                  double max = 0;
                  for (int x = 1; x + 1 < condition.X_LMax * dpi; x++)
                  {
                      for (int y = 1; y + 1 < condition.Y_LMax * dpi; y++)
                      {
                          if (x >= condition.X_Pos * dpi && y >= condition.Y_Pos * dpi)
                              if (x <= (condition.X_Pos + condition.X_LRec) * dpi && y <= (condition.Y_Pos + condition.y_LRec) * dpi)
                              {
                                  grid[x, y] = 1;
                                  temp[x, y] = 1;
                                  continue;
                              }
                          double data = grid[x, y];
                          if (omega == 0)
                          {

                              grid[x, y] = 0.25 * (temp[x + 1, y] + grid[x - 1, y] + temp[x, y + 1] + grid[x, y - 1]);
                              max = Math.Abs(data - grid[x, y]) > max ? Math.Abs(data - grid[x, y]) : max;
                              continue;
                          }

                          grid[x, y] = temp[x, y] + omega * (0.25 * (temp[x + 1, y] + grid[x - 1, y] + temp[x, y + 1] + grid[x, y - 1]) - temp[x, y]);
                          max = Math.Abs(data - grid[x, y]) > max ? Math.Abs(data - grid[x, y]) : max;
                          //errors.Add(max);
                      }
                  }


                  temp = gridOld;

                  if (conv != 0)
                      if (conv > max)

                          loopState.Stop();
                  //cnt++;
                  lock (errors)
                      lock ((object)cnt)
                      {
                          cnt++;
                          errors.Add(max);
                      }

              });
            iterate = cnt;
            return new Data.Result(grid, errors);
        }
    }
}
