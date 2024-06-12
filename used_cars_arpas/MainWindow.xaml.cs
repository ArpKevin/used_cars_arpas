using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace used_cars_arpas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Car> cars = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var data = File.ReadAllLines(@"..\..\..\cleaned_used_cars.csv");

            for (int i = 1; i < data.Length; i++)
            {
                var item = data[i].Split(',');

                string name = item[1];
                string model = item[2];
                string fuelType = item[3];
                int mileage = int.Parse(item[4]);
                int registrationDate = int.Parse(item[5]);
                int price = int.Parse(item[6]);

                cars.Add(new Car(name, model, fuelType, mileage, registrationDate, price, $"image_{i}.jpg"));
            }

            cars_container.ItemsSource = cars;

            combobox_make.Items.Add("All");
            combobox_regdate.Items.Add("All");

            combobox_make.SelectionChanged += (sender, e) => { if (combobox_make.SelectedItem != null) combobox_make_SelectionChanged(); };
            combobox_model.IsEnabled = false;

            foreach (var make in cars.Select(car => car.Name).Distinct()) combobox_make.Items.Add(make);

            for (int year = cars.Max(car => car.RegistrationDate); year >= cars.Min(car => car.RegistrationDate); year--)
            {
                combobox_regdate.Items.Add(year);
            }

            combobox_make.SelectedIndex = 0;
            combobox_model.SelectedIndex = 0;
            combobox_regdate.SelectedIndex = 0;

            combobox_make.SelectionChanged += (sender, e) => { UpdateCarList(); };
            combobox_model.SelectionChanged += (sender, e) => { UpdateCarList(); };
            combobox_regdate.SelectionChanged += (sender, e) => { UpdateCarList(); };
        }
        private void combobox_make_SelectionChanged()
        {
            combobox_model.IsEnabled = combobox_make.SelectedItem != null;
            var selectedMake = combobox_make.SelectedItem as string;

            var modelsForMake = cars.Where(car => car.Name == selectedMake).Select(car => car.Model).Distinct();
            combobox_model.Items.Clear();
            combobox_model.Items.Add("All");
            foreach (var model in modelsForMake)
            {
                combobox_model.Items.Add(model);
            }
        }
        private void UpdateCarList()
        {
            var selectedMake = combobox_make.SelectedItem as string;
            var selectedModel = combobox_model.SelectedItem as string;
            var selectedYear = combobox_regdate.SelectedItem;

            var filteredCars = cars.Where(car =>
            {
                bool includeMake = selectedMake == null || selectedMake == "All" || car.Name == selectedMake;
                bool includeModel = selectedModel == null || selectedModel == "All" || car.Model == selectedModel;
                bool includeYear = selectedYear == null || selectedYear == "All" || car.RegistrationDate == (int)selectedYear;

                return includeMake && includeModel && includeYear;
            }).ToList();

            cars_container.ItemsSource = filteredCars;
        }
    }
}