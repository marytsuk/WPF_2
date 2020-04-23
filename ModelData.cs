using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace DataLib
{
    interface IDeepCopy
    {
        object DeepCopy();
    }
    [Serializable]
    public class ModelData : IDataErrorInfo, IDeepCopy, INotifyPropertyChanged
    {
        public int number_of_grid { get; set; }

        public string Error
        {
            get
            {
                return "Error!";
            }
        }

        public string this[string columnName]
        {
            get
            {
                string msg = null;
                switch (columnName)
                {
                    case "number_of_grid" :
                        if (number_of_grid > nMax || number_of_grid < nMin)
                        {
                            msg = "Invalid number of grid nodes!";
                        }
                        break;
                    case "p" :
                        if (p > pMax || p < pMin)
                        {
                            msg = "Invalid parameter value!";
                        }
                        break;
                    default:
                        break;
       
                }
                return msg;
            }
        }

        public double p { get; set; }
        public double[] x;
        public double[] y;
        static double pMin = 0.5;
        static double pMax = 10;
        static int nMin = 2;
        static int nMax = 10;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Func(double val)
        {
            return  Math.Cos(p * val) ;
        }
        public ModelData(int count, double param)
        {
            number_of_grid = count;
            p = param;
            x = new double[number_of_grid];
            
            double delta = 1.0 / (number_of_grid - 1);
            
            for (int i = 1; i < number_of_grid; i++)
            {
                x[i] = x[i - 1] + delta;
            }
            
            y = new double[number_of_grid];
            for (int i = 0; i < number_of_grid; i++)
            {
                y[i] = Func(x[i]);
            }
            //string str = "";
            //for (int i = 0; i < number_of_grid; i++)
            //{
            //    str += x[i] + " " + y[i] + '\n';
            //}
            //MessageBox.Show(str);

        }
        public double F(double value)
        {
            int j = 0;
            for (int i = 0; i < number_of_grid - 1; i++)
            {
                if (value < x[i + 1] && value > x[i])
                {
                    j = i;
                    break;
                }
            }
            return (((value - x[j]) * (y[j + 1] - y[j])) / (x[j + 1] - x[j])) + y[j];
        }

        public override string ToString()
        {
            return p.ToString() + "  " + number_of_grid.ToString();
        }
        virtual public object DeepCopy()
        {
            return new ModelData(number_of_grid, p);
        }
    }

}
