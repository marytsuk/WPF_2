using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace DataLib
{
    public class ModelDataView: IDataErrorInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<string> typesOfLines;
        public ObservableModelData modelDatas { get; set; }
        
        public ModelDataView(ObservableModelData obj)
        {
            typesOfLines = new List<string>();
            typesOfLines.Add("Line");
            typesOfLines.Add("Spline");
            modelDatas = obj;
        }
        public string type { get; set; }
        public int count { get; set; }

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
                    case "count":
                        if (count < 1 || count > 5)
                        {
                            msg = "Invalid number of decimal places!";
                        }
                        break;
                }
                return msg;
            }
        }

        public void Draw(Chart chart, ModelData selectedmodelData)
        {
            
            chart.ChartAreas.Clear();
            


            chart.ChartAreas.Add(new ChartArea("ChartArea1"));
            chart.Series.Clear();
            chart.Legends.Clear();
            //chart.Invalidate();

            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisX.Maximum = 1;
            chart.ChartAreas[0].AxisX.Title = "X";
            chart.ChartAreas[0].AxisY.Title = "Y";

            
            chart.Series.Add(new Series(chart.Series.NextUniqueName()));
            chart.Legends.Add(new Legend());
            chart.Series[0].LegendText = "Parameter = " + selectedmodelData.p.ToString();


            chart.ChartAreas[0].AxisX.LabelStyle.Format = "F" + count.ToString();
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "F" + count.ToString();
            
            if (type == "Line")
            {
                chart.Series[0].ChartType = SeriesChartType.Line;
            }
            else
                chart.Series[0].ChartType = SeriesChartType.Spline;
            
            //chart.Series[0].MarkerStyle = MarkerStyle.Circle;
            //chart.Series[0].MarkerSize = 5;
            chart.Series[0].Points.DataBindXY(selectedmodelData.x, selectedmodelData.y);
            chart.Series[0].BorderWidth = 2;

            int j = 1;
            foreach (var it in modelDatas)
            {
                if (it.p <= selectedmodelData.p && it != selectedmodelData)
                {
                    chart.Series.Add(new Series(chart.Series.NextUniqueName() ));
                    chart.Series[j].LegendText = "Parameter = " + it.p.ToString();
                    if (type == "Line")
                    {
                        chart.Series[j].ChartType = SeriesChartType.Line;
                    }
                    else
                        chart.Series[j].ChartType = SeriesChartType.Spline;

                    chart.Series[j].BorderWidth = 2;
                    chart.Series[j].Points.DataBindXY(it.x, it.y);
                    j++;

                }

            }
            

            //MessageBox.Show(j.ToString());




            chart.ChartAreas.Add(new ChartArea("ChartArea2"));
            
            chart.ChartAreas[1].AxisX.LabelStyle.Format = "F" + count.ToString();

            chart.ChartAreas[1].AxisY.LabelStyle.Format = "F" + count.ToString();
            chart.ChartAreas[1].AxisX.Minimum = 0;
            chart.ChartAreas[1].AxisX.Maximum = 1;
            chart.ChartAreas[1].AxisX.Title = "X";
            chart.ChartAreas[1].AxisY.Title = "Y";

            chart.Series.Add("");
            double[] max_y = new double[selectedmodelData.number_of_grid];
            double[] min_y = new double[selectedmodelData.number_of_grid];
            double[] middle_y = new double[selectedmodelData.number_of_grid];

            for (int i = 0; i < selectedmodelData.number_of_grid; i++)
            {
                double max = selectedmodelData.x[i];
                double min = selectedmodelData.x[i];
                foreach (var it in modelDatas)
                {
                    double tmp = it.F(selectedmodelData.x[i]);
                    if (tmp > max)
                        max = tmp;
                    else if (tmp < min)
                        min = tmp;
                }
                max_y[i] = max;
                min_y[i] = min;
                middle_y[i] = (max + min) / 2;
            }

            chart.Series.Add("");
            if (type == "Line")
            {
                chart.Series[j].ChartType = SeriesChartType.Line;
            }
            else
                chart.Series[j].ChartType = SeriesChartType.Spline;
            
            chart.Series[j].Points.DataBindXY(selectedmodelData.x, max_y);



            
            if (type == "Line")
            {
                chart.Series[j + 1].ChartType = SeriesChartType.Line;
            }
            else
                chart.Series[j + 1].ChartType = SeriesChartType.Spline;



            chart.Series.Add("");
            chart.Series[j + 1].Points.DataBindXY(selectedmodelData.x, min_y);
            if (type == "Line")
            {
                chart.Series[j + 2].ChartType = SeriesChartType.Line;
            }
            else
                chart.Series[j + 2].ChartType = SeriesChartType.Spline;
            chart.Series[j + 2].Points.DataBindXY(selectedmodelData.x, middle_y);
            //chart.Series[j].ChartArea = "ChartArea2";
            //chart.Series[j + 1].ChartArea = "ChartArea2";
            //chart.Series[j + 2].ChartArea = "ChartArea2";
            for (int k = j; k < j + 3; k++)
            {
                //chart.Series.Add("");
                //if (type == "Line")
                //{
                //    chart.Series[k].ChartType = SeriesChartType.Line;
                //}
                //else
                //    chart.Series[k].ChartType = SeriesChartType.Spline;
                chart.Series[k].BorderWidth = 2;
                chart.Series[k].ChartArea = "ChartArea2";
                chart.Series[k].IsVisibleInLegend = false;
            }


            for (int k = j; k < j + 3; k++)
            {
                chart.Series[k].MarkerStyle = MarkerStyle.Circle;
                chart.Series[k].MarkerSize = 7;
                for (int i = 0; i < chart.Series[k].Points.Count; i++)

                    chart.Series[k].Points[i].ToolTip =

                    "x = " + chart.Series[k].Points[i].XValue.ToString() +

                    "\ny = " + chart.Series[k].Points[i].YValues[0].ToString("F3");

            }
        }
        
    }
}
