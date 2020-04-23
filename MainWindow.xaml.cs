using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
using DataLib;
using System.Collections.Specialized;
using System.Drawing;

namespace MyWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Chart myChart = new Chart();
        ObservableModelData obj = new ObservableModelData();
        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(MainWindow));
        public static RoutedCommand DrawCommand = new RoutedCommand("Draw", typeof(MainWindow));
        ModelData modelData = new ModelData(0, 0);
        ModelDataView modelDataView;
        public MainWindow()
        {
            InitializeComponent();
            myWinFormsHost.Child = myChart;
            this.DataContext = obj;
            myGrid.DataContext = modelData;

            modelDataView = new ModelDataView(obj);

            myGridView.DataContext = modelDataView;
            myComboBox.ItemsSource = modelDataView.typesOfLines;
            
        }
        public void Data_Changed_Handler(object source, NotifyCollectionChangedEventArgs args)
        {
            
            modelDataView = new ModelDataView(obj);
           
        }
        private void Save_Func()
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            if (obj.IfChanged)
            {
                var result = MessageBox.Show("Save changes?", "Message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (sfd.ShowDialog() == true)
                        ObservableModelData.Save(sfd.FileName, ref obj);
                }
                else
                    MessageBox.Show("Data may be lost!", "Message");
            }
        }
        private void window_Closed(object sender, EventArgs e)
        {
            Save_Func();
        }
        private void Update_Items()
        {
            this.DataContext = obj;
            modelDataView.modelDatas = obj;
           //modelDataView = new ModelDataView(obj.)

            //obj.CollectionChanged += Data_Changed_Handler;
            //ListBox_All.ItemsSource = obj;         //
            // ListBox_Project.ItemsSource = obj.Get_Projects;
        }

        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Func();
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                ObservableModelData.Load(ofd.FileName, ref obj);
                Update_Items();
            }

        }

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (obj.IfChanged == true)
            //    e.CanExecute = true;
            //else
            //    e.CanExecute = false;
            e.CanExecute = obj.IfChanged == true;
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Func();
        }

        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Func();
            obj = new ObservableModelData();
            // this.DataContext = obj;
            Update_Items();

        }

        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            obj.Add_ModelData(modelData.DeepCopy() as ModelData);
        }

        private void CanAddCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            if (myGrid != null)
            {
                foreach (FrameworkElement child in myGrid.Children)
                {
                    if (Validation.GetHasError(child) == true)
                    {
                        e.CanExecute = false;
                    }
                }
            }
        }

        private void RemoveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            obj.Remove_At(myListBox.SelectedIndex);
        }

        private void CanRemoveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !myListBox.SelectedIndex.Equals(-1);
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        //private void butt_clicked(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show(modelDataView.type + " " + modelDataView.count);
        //}

        private void DrawCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
           
            modelDataView.Draw(myChart, myListBox.SelectedItem as ModelData);
        }

        private void CanDrawCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !myListBox.SelectedIndex.Equals(-1) && !(myComboBox.SelectedItem == null);

            if (myGridView != null)
            {
                foreach (FrameworkElement child in myGridView.Children)
                {
                    if (Validation.GetHasError(child) == true)
                    {
                        e.CanExecute = false && e.CanExecute;
                    }
                }
            }

        }
    }
}
