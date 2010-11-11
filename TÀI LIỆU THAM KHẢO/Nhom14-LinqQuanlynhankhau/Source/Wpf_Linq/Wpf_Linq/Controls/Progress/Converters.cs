using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace Wpf_Linq
{
    public class ShowIfTrueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Int32), typeof(String))]
    public class PlusOneConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int t = (int)value;
            return (t + 1).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            int result;
            if (int.TryParse(strValue, out result))
            {
                return result - 1;
            }
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class DiaChiConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            clsHoKhauTemp hokhau = value as clsHoKhauTemp;
            if (hokhau != null)
            {
                return string.Format("Số {0}-Đường{1}", hokhau.SoNha, hokhau.TenDuong);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class DiaChiConverter_2 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            clsHoKhauTemp hokhau = value as clsHoKhauTemp;
            if (hokhau != null)
            {
                return string.Format("Số {0}-Đường {1}-{2}-{3}-{4}", hokhau.SoNha, hokhau.TenDuong, hokhau.PhuongXa, hokhau.QuanHuyen, hokhau.TinhTP);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class DiaChiConverter_3 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataLayer.HOKHAULUU hokhau = value as DataLayer.HOKHAULUU;
            if (hokhau != null)
            {
                return string.Format("Số {0}-Đường {1}-{2}-{3}-{4}", hokhau.SONHA, hokhau.DUONG, hokhau.PHUONGXA, hokhau.QUANHUYEN, hokhau.TINHTP);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime dt = (DateTime)value;
                if (dt != null)
                    return dt.ToShortDateString();
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    [ValueConversion(typeof(String), typeof(String))]
    public class HoTenConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            clsChiTietHoKhauTemp hokhau = value as clsChiTietHoKhauTemp;
            if (hokhau != null)
            {
                if (hokhau.ChuLot == null)
                    return string.Format("{0} {1}", hokhau.Ho, hokhau.Ten);
                return string.Format("{0} {1} {2}", hokhau.Ho, hokhau.ChuLot, hokhau.Ten);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class HoTenConverter_2 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataLayer.CTHOKHAU hokhau = value as DataLayer.CTHOKHAU;
            if (hokhau != null)
            {
                if (hokhau.CHULOT == null)
                    return string.Format("{0} {1}", hokhau.HO, hokhau.TEN);
                return string.Format("{0} {1} {2}", hokhau.HO, hokhau.CHULOT, hokhau.TEN);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class HoTenConverter_3 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            clsNhanKhauCatChuyen hokhau = value as clsNhanKhauCatChuyen;
            if (hokhau != null)
            {
                if (hokhau.ChuLot == null)
                    return string.Format("{0} {1}", hokhau.Ho, hokhau.Ten);
                return string.Format("{0} {1} {2}", hokhau.Ho, hokhau.ChuLot, hokhau.Ten);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class HoTenConverter_4 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataLayer.NHANKHAULUU hokhau = value as DataLayer.NHANKHAULUU;
            if (hokhau != null)
            {
                if (hokhau.CHULOT == null)
                    return string.Format("{0} {1}", hokhau.HO, hokhau.TEN);
                return string.Format("{0} {1} {2}", hokhau.HO, hokhau.CHULOT, hokhau.TEN);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class HoTenConverter_5 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataLayer.DOITUONG hokhau = value as DataLayer.DOITUONG;
            if (hokhau != null)
            {
                if (hokhau.CHULOT == null)
                    return string.Format("{0} {1}", hokhau.HO, hokhau.TEN);
                return string.Format("{0} {1} {2}", hokhau.HO, hokhau.CHULOT, hokhau.TEN);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Boolean), typeof(String))]
    public class GioiTinhConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Nam" : "Nữ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class QHVoiChuHoConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (!string.IsNullOrEmpty(s))
            {
                return string.Format("QH với chủ hộ:{0}", s);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
