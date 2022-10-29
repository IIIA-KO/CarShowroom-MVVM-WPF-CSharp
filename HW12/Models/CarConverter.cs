using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HW12.Models
{
    internal class CarConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double _motor = 1;
            if (double.TryParse(values[2].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out _motor))
                return new Car
                {
                    Brand = values[0].ToString(),
                    Model = values[1].ToString(),
                    Motor = _motor,
                    ReleaseDate = values[3] is DateTime dt ? dt : DateTime.Now,
                    StateNumber = values[4].ToString(),
                    VIN = values[5].ToString()
                };
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Car c)
            {
                return new object[]
                {
                    c.Brand,
                    c.Model,
                    c.Motor,
                    c.ReleaseDate,
                    c.StateNumber
                };
            }
            return null;
        }
    }
}
