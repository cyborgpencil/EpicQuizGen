using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TechTool.Models;

namespace TechTool.Converters
{
    public class StringToUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return $"{((User)value).DisplayName},   {((User)value).Username}";
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value != null)
            {
                string[] v = ((string)value).Split(new char[(',')]);
                User u = new User();
                u.DisplayName = v[0];
                u.Username = v[1];
                return u;
            }
            else
                return null;
        }
    }
}
