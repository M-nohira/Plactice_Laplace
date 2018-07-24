using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace_WPF.Model
{
    static class file
    {
        static public string GetCSV(List<double> erros)
        {
            string msg = "";
            int cnt = 1;
            foreach (var error in erros)
            {
                msg += $@"{cnt},{error.ToString()}{Environment.NewLine}";
                cnt++;
            }
            return msg;
        }
    }
}
