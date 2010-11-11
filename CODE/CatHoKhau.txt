using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using DataLayer;
using System.Windows;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using System;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for ucCatHoKhau.xaml
    /// </summary>
    public partial class ucCatHoKhau : UserControl
    {
        ObservableCollection<clsHoKhauTemp> HoKhaus;
        ObservableCollection<HOKHAULUU> HoKhausCat;
        CollectionViewSource hOKHAUViewSource;
        CollectionViewSource catHOKHAUViewSource;
        private int countItems = 0;
        private int countItemsCat = 0;
        private bool isDisableNavigator = false;
        private bool isDisableNavigator_2 = false;

        public ucCatHoKhau()
        {
            InitializeComponent();
            hOKHAUViewSource = ((CollectionViewSource)(this.FindResource("hOKHAUViewSource")));
            catHOKHAUViewSource = ((CollectionViewSource)(this.FindResource("catHOKHAUViewSource")));
        }
        #region Navigator
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnBack")
                hOKHAUViewSource.View.MoveCurrentToPrevious();
            else if (btn.Name == "btnBack_2")
                catHOKHAUViewSource.View.MoveCurrentToPrevious();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnFirst")
                hOKHAUViewSource.View.MoveCurrentToFirst();
            else if (btn.Name == "btnFirst_2")
                catHOKHAUViewSource.View.MoveCurrentToFirst();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnForward")
                hOKHAUViewSource.View.MoveCurrentToNext();
            else if (btn.Name == "btnForward_2")
                catHOKHAUViewSource.View.MoveCurrentToNext();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnLast")
                hOKHAUViewSource.View.MoveCurrentToLast();
            else if (btn.Name == "btnLast_2")
                catHOKHAUViewSource.View.MoveCurrentToLast();
        }
        #endregion

        #region TextChanged
        private void txtCurrentIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItems == 0 || hOKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndex.Text, out index);
            if (!f)
                return;
            --index;
            dataGridHoKhau.ScrollIntoView(HoKhaus[index]);
            if (isDisableNavigator)
            {
                enable_1(true);
                enable_2(true);
            }
            if (index == 0)
            {
                enable_1(false);
                isDisableNavigator = true;
            }
            if (index == countItems - 1)
            {
                enable_2(false);
                isDisableNavigator = true;
            }
        }

        private void txtCurrentIndexCat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItemsCat == 0 || catHOKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndexCat.Text, out index);
            if (!f)
                return;
            --index;
            dataGridHoKhauCat.ScrollIntoView(HoKhausCat[index]);
            if (isDisableNavigator_2)
            {
                enable_11(true);
                enable_22(true);
            }
            if (index == 0)
            {
                enable_11(false);
                isDisableNavigator_2 = true;
            }
            if (index == countItemsCat - 1)
            {
                enable_22(false);
                isDisableNavigator_2 = true;
            }
        }

        private void enable_1(bool f)
        {
            btnBack.IsEnabled = f;
            btnFirst.IsEnabled = f;
        }

        private void enable_2(bool f)
        {
            btnLast.IsEnabled = f;
            btnForward.IsEnabled = f;
        }

        private void enable_11(bool f)
        {
            btnBack_2.IsEnabled = f;
            btnFirst_2.IsEnabled = f;
        }

        private void enable_22(bool f)
        {
            btnLast_2.IsEnabled = f;
            btnForward_2.IsEnabled = f;
        }
        #endregion

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            Progress pg = new Progress();
            pg.Title = "Đang tải dữ liệu ... (^_^)";
            pg.IsBusy = true;
            myGridHoKhau.Children.Add(pg);
            Progress pg_2 = new Progress();
            pg_2.Title = "Đang tải dữ liệu ... (^_^)";
            pg_2.IsBusy = true;
            myGridHoKhauCat.Children.Add(pg_2);

            enable_1(false); enable_11(false);
            enable_2(false); enable_22(false);
            isDisableNavigator = true; isDisableNavigator_2 = true;

            BackgroundWorker loadWorker = new BackgroundWorker();
            loadWorker.DoWork += new DoWorkEventHandler(loadWorker_DoWork);
            loadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadWorker_RunWorkerCompleted);
            loadWorker.RunWorkerAsync();
        }

        void loadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);
            #region query
            var query = from hk in hKDataContext.HOKHAUs    //  << Query does
                        where hk.TINHTRANG == 0
                        select new clsHoKhauTemp            //  << not execute
                        {                                   //  << here
                            SoHoKhau = hk.SOHK,
                            SoNha = hk.SONHA,
                            TenDuong = hk.DUONG,
                            PhuongXa = hk.PHUONGXA,
                            QuanHuyen = hk.QUANHUYEN,
                            TinhTP = hk.TINHTP,
                            TinhTrang = hk.TINHTRANG1.LOAITT,
                            ChiTiet = from ct in hKDataContext.CTHOKHAUs
                                      where ct.SOHK == hk.SOHK
                                      select new clsChiTietHoKhauTemp
                                      {
                                          Ho = ct.HO,
                                          ChuLot = ct.CHULOT,
                                          Ten = ct.TEN,
                                          QHVoiChuHo = ct.QUANHE.QUANHE1,
                                          NgaySinh = ct.NGAYSINH,
                                          BiDanh = ct.BIDANH,
                                          ToDanPho = ct.TODANPHO,
                                          NoiSinh = ct.NOISINH,
                                          NguyenQuan = ct.NGUYENQUAN,
                                          GioiTinh = ct.GIOITINH == true ? "Nam" : "Nữ",
                                          DanToc = ct.DANTOC,
                                          TonGiao = ct.TONGIAO,
                                          NgheNghiep = ct.NGHENGHIEP,
                                          NoiLamViec = ct.NOILAMVIEC,
                                          CMND = ct.CMND,
                                          NgayCapCMND = ct.NGAYCAP_CMND,
                                          NoiCapCMND = ct.NOICAP_CMND,
                                          NgayDangKyThuongTru = ct.NTNDKTT,
                                          NoiThuongTruTruocKhiChuyenDen = ct.NTTTKCD,
                                          NgayChuyenDen = ct.NGAYCHUYENDEN,
                                          LyDo = ct.LYDO,
                                          GhiChu = ct.GHICHU
                                      }
                        };
            #endregion
            #region query_2
            var query_2 = from hkc in hKDataContext.HOKHAULUUs
                          where hkc.SOHK == hkc.SOHOSO
                          select hkc;
            #endregion
            try
            {
                HoKhaus = new ObservableCollection<clsHoKhauTemp>(query);//    << Query Executes here 
                HoKhausCat = new ObservableCollection<HOKHAULUU>(query_2);//    << Query Executes here 
            }
            catch
            {
                MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            countItems = HoKhaus.Count(); countItemsCat = HoKhausCat.Count();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
            {
                hOKHAUViewSource.Source = HoKhaus;
                catHOKHAUViewSource.Source = HoKhausCat;
                txtItemCount.Text = "/" + countItems.ToString(); txtItemCount2.Text = "/" + countItemsCat.ToString();
                txtCurrentIndex_TextChanged(null, null); txtCurrentIndexCat_TextChanged(null, null);
            }));
        }

        void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myGridHoKhau.Children.RemoveAt(1);
            myGridHoKhauCat.Children.RemoveAt(1);
            if (HoKhaus == null)
                return;
            enableButton();
        }

        private void enableButton()
        {
            btnCat.IsEnabled = true;

            txtCurrentIndex.IsEnabled = true;
            txtCurrentIndexCat.IsEnabled = true;
            txtLyDoCat.IsEnabled = true;
            txtNguoiKyHoSo.IsEnabled = true;
            dpNgayCat.IsEnabled = true;
        }

    }
}
