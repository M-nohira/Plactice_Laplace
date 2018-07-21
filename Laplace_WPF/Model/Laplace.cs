using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace_WPF.Model
{
    static class Laplace
    {
        public static double[,] Subsititution(Data.Condition condition, int dpi, int iterate, double[,] grid = null)
        {
            if (grid == null)
                grid = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];

            double[,] temp = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];

            Parallel.For(0, iterate, i =>
            {
                for (int x = 1; x + 1 < condition.X_LMax * dpi; x++)
                {
                    for (int y = 1; y + 1 < condition.Y_LMax * dpi; y++)
                    {
                        if (x >= condition.X_Pos * dpi && y >= condition.Y_Pos * dpi)
                            if (x <= (condition.X_Pos + condition.X_LRec) * dpi && y <= (condition.Y_Pos + condition.y_LRec) * dpi)
                            {
                                grid[x, y] = 1;
                                continue;
                            }

                        temp[x,y] = 0.25 * (grid[x + 1, y] + grid[x - 1, y] + grid[x, y + 1] + grid[x, y - 1]);

                    }
                }
                grid = temp;
                
            });

            return grid;
        }

        public static double[,] Gauss(Data.Condition condition,int dpi,int iterate, double[,] grid = null)
        {
            if (grid == null) grid = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];
            double[,] temp = new double[condition.X_LMax * dpi, condition.Y_LMax * dpi];

            Parallel.For(0, iterate, i =>
              {
                  double[,] gridOld = grid;
                  for(int x = 1; x + 1 < condition.X_LMax * dpi; x++)
                  {
                      for(int y = 1;y + 1 < condition.Y_LMax * dpi; y++)
                      {
                          if (x >= condition.X_Pos * dpi && y >= condition.Y_Pos * dpi)
                              if (x <= (condition.X_Pos + condition.X_LRec) * dpi && y <= (condition.Y_Pos + condition.y_LRec) * dpi)
                              {
                                  grid[x, y] = 1;
                                  temp[x, y] = 1;
                                  continue;
                              }
                          
                          grid[x,y] = 0.25 * (temp[x + 1, y] + grid[x - 1, y] + temp[x, y + 1] + grid[x, y - 1]);

                      }
                  }
                  temp = gridOld;
                  
              });

            return grid;
        }
    }
}
