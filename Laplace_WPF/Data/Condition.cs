using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace_WPF.Data
{
    public class Condition
    {
        public int X_LMax { get; set; }
        public int Y_LMax { get; set; }


        private int x_lrec;
        public int X_LRec
        {
            get { return x_lrec; }
            set
            {
                x_lrec = value;
                x_pos = (X_LMax - x_lrec) / 2;
            }
        }

        private int y_lrec { get; set; }
        public int y_LRec
        {
            get { return y_lrec; }
            set
            {
                y_lrec = value;
                y_pos = (Y_LMax - y_lrec) / 2;
            }
        }

        private int x_pos;
        public int X_Pos
        {
            get
            {
                return x_pos;
            }
        }

        private int y_pos;
        public int Y_Pos
        {
            get
            {
                return y_pos;
            }
        }
    }
}
