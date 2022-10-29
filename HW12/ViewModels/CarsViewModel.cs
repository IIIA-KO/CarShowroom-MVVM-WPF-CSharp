using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HW12.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HW12.ViewModels
{
    internal class CarsViewModel : DependencyObject
    {
        private readonly string pattern = @"[A-Y]{2}\d{4}[A-Y]{2}";
        private readonly string path = "CarsList.bin";

        public ICommand AddCar { get; }
        public ICommand RemoveCars { get; }
        public ICommand SaveCarsList { get; }


        public string SearchByAnyField
        {
            get { return (string)GetValue(SearchByAnyFieldProperty); }
            set { SetValue(SearchByAnyFieldProperty, value); }
        }
        public static readonly DependencyProperty SearchByAnyFieldProperty =
            DependencyProperty.Register("SearchByAllFields", typeof(string), typeof(CarsViewModel), new PropertyMetadata("", AllFieldsFilterChanged));
        private static void AllFieldsFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByAnyField;
            }
        }
        private bool PredicateByAnyField(object obj)
        {
            if (obj is Car car)
                return car.ToString().Contains(SearchByAnyField);
            return false;
        }



        public string SearchByModel
        {
            get { return (string)GetValue(SearchByModelProperty); }
            set { SetValue(SearchByModelProperty, value); }
        }
        public static readonly DependencyProperty SearchByModelProperty =
            DependencyProperty.Register("SearchByModel", typeof(string), typeof(CarsViewModel), new PropertyMetadata("", ModelFilterChanged));
        private static void ModelFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByModel;
            }
        }
        private bool PredicateByModel(object obj)
        {
            if (obj is Car car)
                return car.Model.Contains(SearchByModel);
            return false;
        }



        public string SearchByBrand
        {
            get { return (string)GetValue(SearchByBrandProperty); }
            set { SetValue(SearchByBrandProperty, value); }
        }
        public static readonly DependencyProperty SearchByBrandProperty =
            DependencyProperty.Register("SearchByBrand", typeof(string), typeof(CarsViewModel), new PropertyMetadata("", BrandFilterChanged));
        private static void BrandFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByBrand;
            }
        }
        private bool PredicateByBrand(object obj)
        {
            if (obj is Car car)
                return car.Brand.Contains(SearchByBrand);
            return false;
        }



        public string SearchByStateNumber
        {
            get { return (string)GetValue(SearchByStateNumberProperty); }
            set { SetValue(SearchByStateNumberProperty, value); }
        }
        public static readonly DependencyProperty SearchByStateNumberProperty =
            DependencyProperty.Register("SearchByStateNumber", typeof(string), typeof(CarsViewModel), new PropertyMetadata("", StateNumberFilterChanged));
        private static void StateNumberFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByStateNumber;
            }
        }
        private bool PredicateByStateNumber(object obj)
        {
            if (obj is Car car)
                return car.StateNumber.Contains(SearchByStateNumber);
            return false;
        }



        public string SearchByVIN
        {
            get { return (string)GetValue(SearchByVINProperty); }
            set { SetValue(SearchByVINProperty, value); }
        }
        public static readonly DependencyProperty SearchByVINProperty =
            DependencyProperty.Register("SearchByVIN", typeof(string), typeof(CarsViewModel), new PropertyMetadata("", VINFilterChanged));
        private static void VINFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByVIN;
            }
        }
        private bool PredicateByVIN(object obj)
        {
            if (obj is Car car)
                return car.VIN.Contains(SearchByVIN);
            return false;
        }



        public DateTime SearchByLeftDate
        {
            get { return (DateTime)GetValue(SearchByLeftDateProperty); }
            set { SetValue(SearchByLeftDateProperty, value); }
        }
        public static readonly DependencyProperty SearchByLeftDateProperty =
            DependencyProperty.Register("SearchByLeftDate", typeof(DateTime), typeof(CarsViewModel), new PropertyMetadata(DateTime.Today, LeftDateFilterChanged));
        private static void LeftDateFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByDate;
            }
        }
        public DateTime SearchByRightDate
        {
            get { return (DateTime)GetValue(SearchByRightDateProperty); }
            set { SetValue(SearchByRightDateProperty, value); }
        }
        public static readonly DependencyProperty SearchByRightDateProperty =
            DependencyProperty.Register("SearchByRightDate", typeof(DateTime), typeof(CarsViewModel), new PropertyMetadata(DateTime.Today, RightDateFilterChanged));
        private static void RightDateFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CarsViewModel THIS)
            {
                THIS.CarsCollection.Filter = null;
                THIS.CarsCollection.Filter = THIS.PredicateByDate;
            }
        }
        private bool PredicateByDate(object obj)
        {
            if (obj is Car car)
                return car.ReleaseDate >= SearchByLeftDate && car.ReleaseDate <= SearchByRightDate;
            return false;
        }



        public ICollectionView CarsCollection
        {
            get { return (ICollectionView)GetValue(CarsCollectionProperty); }
            set { SetValue(CarsCollectionProperty, value); }
        }
        public static readonly DependencyProperty CarsCollectionProperty =
            DependencyProperty.Register("CarsCollection", typeof(ICollectionView), typeof(CarsViewModel), new PropertyMetadata(null));



        private readonly ObservableCollection<Car> CarsList;
        public CarsViewModel()
        {
            CarsList = new ObservableCollection<Car>(DeserializeCars());

            SearchByAnyField = "";
            SearchByModel = "";
            SearchByBrand = "";
            SearchByStateNumber = "";
            SearchByVIN = "";
            SearchByLeftDate = DateTime.Today;
            SearchByRightDate = DateTime.Today;

            CarsCollection = CollectionViewSource.GetDefaultView(CarsList);
            CarsCollection.Filter = PredicateByAnyField;

            AddCar = new RelayCommand(AddCarToList, CanAddCar);
            RemoveCars = new RelayCommand(RemoveCarsFromList, CanRemoveCars);
            SaveCarsList = new RelayCommand(SaveCars, CanSaveCars);
        }

        private void SaveCars(object? obj)
        {
            SerializeCars();
        }
        private bool CanSaveCars(object? obj)
        {
            try
            {
                if (obj is ItemCollection sp)
                    return sp.Count > 0;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return false;
            }
        }
        private void SerializeCars()
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (Stream stream = File.Create(path))
                {
                    binaryFormatter.Serialize(stream, CarsList.ToList());
                }
                MessageBox.Show("Зміни було в списку було збережено", "Збереження файлу", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }
        private List<Car> DeserializeCars()
        {
            try
            {
                List<Car> cars = new List<Car>();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                if (File.Exists(path))
                {
                    using (Stream stream = File.OpenRead(path))
                    {
                        cars = binaryFormatter.Deserialize(stream) as List<Car>;
                    }
                }
                return cars;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зчитування: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }


        private void AddCarToList(object? obj)
        {
            try
            {
                if (obj is Car car)
                    CarsList.Add(car);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }
        private bool CanAddCar(object? obj)
        {
            try
            {
                if (obj is Car car)
                {
                    if (String.IsNullOrEmpty(car.Brand)
                        || car.Motor <= 0
                        || String.IsNullOrEmpty(car.Model)
                        || String.IsNullOrEmpty(car.StateNumber)
                        || (!Regex.IsMatch(car.StateNumber, pattern))
                        || String.IsNullOrEmpty(car.VIN)
                        || !IsVinValid(car.VIN)
                        )
                        return false;

                    if (CarsList.Any(i => i.StateNumber == car.StateNumber) ||
                        CarsList.Any(i => i.VIN == car.VIN))
                        return false;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return false;
            }
        }
        private bool IsVinValid(string vin)
        {
            try
            {
                if (vin.Length != 20)
                    return false;

                if (vin.Contains('Z') || vin.Contains('z'))
                    return false;

                if (vin != vin.ToUpper())
                    return false;

                foreach (char c in vin)
                {
                    if (!char.IsLetterOrDigit(c))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return false;
            }
        }


        private void RemoveCarsFromList(object? obj)
        {
            try
            {
                if (obj is IList items)
                {
                    foreach (var item in new List<object>((IEnumerable<object>)items))
                    {
                        if (item is Car car)
                            CarsList.Remove(car);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }
        private bool CanRemoveCars(object? obj)
        {
            try
            {
                if (obj is IEnumerable<object> cars)
                    return cars.Count() > 0;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return false;
            }
        }
    }
}