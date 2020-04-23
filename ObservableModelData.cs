using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace DataLib
{
    [Serializable]
    public class ObservableModelData: ObservableCollection<ModelData>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void Handler(object source, NotifyCollectionChangedEventArgs args)
        {
            ((ObservableModelData)source).IfChanged = true;

        }
        private bool chnd;
        public bool IfChanged
        {
            get
            {
                return chnd;
            }
            set
            {
                chnd = value;
                NotifyPropertyChanged("IfChanged");
            }
        }
        public void Add_ModelData(ModelData modelData)
        {
            base.Add(modelData);
        }
        public ObservableModelData()
        {
           // AddDefaults();
            IfChanged = false;
            base.CollectionChanged += Handler;
        }
        public void Remove_At(int index)
        {
            try
            {
                base.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
        public void AddDefaults()
        {
            base.Add(new ModelData(5, 0.5));
            base.Add(new ModelData(3, 2.0));
            base.Add(new ModelData(4, 1.5));
        }
        public double[] getFuncValues(double x)
        {
            double[] result = new double[base.Count];
            int i = 0;
            foreach(var tmp in base.Items)
            {
                result[i] = tmp.Func(x);
            }
            return result;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public static bool Load(string filename, ref ObservableModelData obj)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                obj = binaryFormatter.Deserialize(fileStream) as ObservableModelData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                //Console.WriteLine("Исключение: " + ex.Message);
                return false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            obj.IfChanged = false;
            obj.CollectionChanged += Handler;
            return true;
        }
        public static bool Save(string filename, ref ObservableModelData obj)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                //Console.WriteLine("Исключение: " + ex.Message);
                return false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            obj.IfChanged = false;
            return true;
        }
    }
}
