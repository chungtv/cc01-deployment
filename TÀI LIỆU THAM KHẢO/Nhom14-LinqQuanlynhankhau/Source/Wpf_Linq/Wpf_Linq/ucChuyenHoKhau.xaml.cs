using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DataLayer;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for ucChuyenHoKhau.xaml
    /// </summary>
    public partial class ucChuyenHoKhau : UserControl
    {
        ObservableCollection<clsHoKhauTemp> HoKhaus;
        ObservableCollection<HOKHAULUU> HoKhausChuyen;
        CollectionViewSource hOKHAUViewSource;
        CollectionViewSource chuyenHOKHAUViewSource;
        private int countItems = 0;
        private int countItemsChuyen = 0;
        private bool isDisableNavigator = false;
        private bool isDisableNavigator_2 = false;

        public ucChuyenHoKhau()
        {
            InitializeComponent();
            hOKHAUViewSource = ((CollectionViewSource)(this.FindResource("hOKHAUViewSource")));
            chuyenHOKHAUViewSource = ((CollectionViewSource)(this.FindResource("chuyenHOKHAUViewSource")));
            dpNgayChuyen.SelectedDate = DateTime.Now.Date;
        }

        #region Navigator
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnBack")
                hOKHAUViewSource.View.MoveCurrentToPrevious();
            else if (btn.Name == "btnBack_2")
                chuyenHOKHAUViewSource.View.MoveCurrentToPrevious();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnFirst")
                hOKHAUViewSource.View.MoveCurrentToFirst();
            else if (btn.Name == "btnFirst_2")
                chuyenHOKHAUViewSource.View.MoveCurrentToFirst();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnForward")
                hOKHAUViewSource.View.MoveCurrentToNext();
            else if (btn.Name == "btnForward_2")
                chuyenHOKHAUViewSource.View.MoveCurrentToNext();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnLast")
                hOKHAUViewSource.View.MoveCurrentToLast();
            else if (btn.Name == "btnLast_2")
                chuyenHOKHAUViewSource.View.MoveCurrentToLast();
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

        private void txtCurrentIndexChuyen_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItemsChuyen == 0 || chuyenHOKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndexChuyen.Text, out index);
            if (!f)
                return;
            --index;
            dataGridHoKhauChuyen.ScrollIntoView(HoKhausChuyen[index]);
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
            if (index == countItemsChuyen - 1)
            {
                enable_22(false);
                isDisableNavigator_2 = true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (countItems == 0 || hOKHAUViewSource.Source == null)
                return;
            ComboBox cb = sender as ComboBox;
            dataGridHoKhauChuyen.ScrollIntoView(cb.SelectedItem);
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
            Grid.SetRowSpan(pg, 4);
            myGridHoKhau.Children.Add(pg);

            enable_1(false); enable_11(false);
            enable_2(false); enable_22(false);
            HoKhaus = new ObservableCollection<clsHoKhauTemp>();HoKhausChuyen = new ObservableCollection<HOKHAULUU>();
            hOKHAUViewSource.Source = null;chuyenHOKHAUViewSource.Source = null;
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
                          where hkc.SOHK != hkc.SOHOSO
                          select hkc;
            #endregion
            try
            {
                HoKhaus = new ObservableCollection<clsHoKhauTemp>(query);//    << Query Executes here 
                HoKhausChuyen = new ObservableCollection<HOKHAULUU>(query_2);//    << Query Executes here 
            }
            catch
            {
                MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            countItems = HoKhaus.Count(); countItemsChuyen = HoKhausChuyen.Count();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
            {
                hOKHAUViewSource.Source = HoKhaus;
                chuyenHOKHAUViewSource.Source = HoKhausChuyen;
                cboHoChuyenDen.SelectedIndex = 0;
                txtItemCount.Text = "/" + countItems.ToString(); txtItemCount2.Text = "/" + countItemsChuyen.ToString();
                txtCurrentIndex_TextChanged(null, null); txtCurrentIndexChuyen_TextChanged(null, null);
            }));
        }

        void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myGridHoKhau.Children.RemoveAt(4);
            if (HoKhaus == null)
                return;
            enableButton();
        }

        private void enableButton()
        {
            btnChuyen.IsEnabled = true;

            txtCurrentIndex.IsEnabled = true;
            txtCurrentIndexChuyen.IsEnabled = true;
            txtLyDo.IsEnabled = true;
            txtNguoiKyHoSo.IsEnabled = true;
            dpNgayChuyen.IsEnabled = true;
        }

        private void btnChuyen_Click(object sender, RoutedEventArgs e)
        {
            if (txtLyDo.Text == string.Empty || txtNguoiKyHoSo.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập lý do chuyển hoặc người ký hồ sơ !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cboHoChuyenDen.SelectedItem == hOKHAUViewSource.View.CurrentItem)
            {
                MessageBox.Show("Hộ khẩu chuyển đi và chuyển đến không được trùng nhau !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!dpNgayChuyen.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng nhập ngày chuyển !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DateTime.Compare(dpNgayChuyen.SelectedDate.Value, DateTime.Now.Date) > 0)
            {
                MessageBox.Show("Ngày chuyển không thể lớn hơn ngày hiện tại !", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            clsHoKhauTemp hkdi = hOKHAUViewSource.View.CurrentItem as clsHoKhauTemp;
            clsHoKhauTemp hkden = cboHoChuyenDen.SelectedItem as clsHoKhauTemp;
            string s = "Bạn có muốn chuyển hộ khẩu :\n" + hkdi.SoHoKhau + ": " + hkdi.SoNha + ' ' + hkdi.TenDuong + ' ' + hkdi.PhuongXa + ' ' + hkdi.QuanHuyen + ' ' + hkdi.TinhTP;
            s += "\nĐến :\n" + hkden.SoHoKhau + ": " + hkden.SoNha + ' ' + hkden.TenDuong + ' ' + hkden.PhuongXa + ' ' + hkden.QuanHuyen + ' ' + hkden.TinhTP;
            MessageBoxResult result = MessageBox.Show(s, "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                insertHKLuu();
            }
        }

        private void insertHKLuu()
        {

            HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);

            clsHoKhauTemp hkdi = hOKHAUViewSource.View.CurrentItem as clsHoKhauTemp;
            clsHoKhauTemp hkden = cboHoChuyenDen.SelectedItem as clsHoKhauTemp;
            List<clsChiTietHoKhauTemp> ChiTiet = hkdi.ChiTiet.ToList();
            List<clsChiTietHoKhauTemp> ChiTiet_2 = hkdi.ChiTiet.ToList();

            windowCapNhatQHVoiChuHo capnhat = new windowCapNhatQHVoiChuHo(ChiTiet_2);
            capnhat.ShowDialog();
            if (capnhat.DialogResult.HasValue && capnhat.DialogResult.Value)
            {
                try
                {
                    var newHOKHAULUU = new HOKHAULUU
                    {
                        SOHK = hkdi.SoHoKhau,
                        SONHA = hkdi.SoNha,
                        DUONG = hkdi.TenDuong,
                        PHUONGXA = hkdi.PhuongXa,
                        QUANHUYEN = hkdi.QuanHuyen,
                        TINHTP = hkdi.TinhTP,
                        SOHOSO = hkden.SoHoKhau,
                        NOICHUYENDEN = hkden.SoNha + '-' + hkden.TenDuong + '-' + hkden.PhuongXa + '-' + hkden.QuanHuyen + '-' + hkden.TinhTP,
                        NGAYCC = dpNgayChuyen.SelectedDate.Value,
                        LYDO = txtLyDo.Text,
                        HOTENNGUOIKYHS = txtNguoiKyHoSo.Text
                    };

                    hKDataContext.HOKHAULUUs.InsertOnSubmit(newHOKHAULUU);


                    var query = from qh in hKDataContext.QUANHEs
                                select qh;
                    List<QUANHE> quanhe = query.ToList();
                    var query_2 = from ct in hKDataContext.CTHOKHAUs
                                  where ct.SOHK == hkdi.SoHoKhau
                                  select ct;
                    List<CTHOKHAU> update = query_2.ToList();
                    HOKHAU hk = hKDataContext.HOKHAUs.Single(c => c.SOHK == hkdi.SoHoKhau);
                    hk.TINHTRANG = 2;

                    for (int i = 0; i < ChiTiet.Count(); ++i)
                    {
                        update[i].TODANPHO = ChiTiet[i].ToDanPho;
                        QUANHE q = quanhe.Single(c => c.QUANHE1 == ChiTiet_2[i].QHVoiChuHo);
                        update[i].QHVOICHUHO = q.MAQH;

                        var newCTHOKHAULUU = new CTHOKHAULUU
                        {
                            SOHK = hkdi.SoHoKhau,
                            HO = ChiTiet[i].Ho,
                            CHULOT = ChiTiet[i].ChuLot,
                            TEN = ChiTiet[i].Ten,
                            QHVOICHUHO = ChiTiet[i].QHVoiChuHo,
                            NGAYSINH = ChiTiet[i].NgaySinh,
                            BIDANH = ChiTiet[i].BiDanh,
                            TODANPHO = ChiTiet[i].ToDanPho,
                            NOISINH = ChiTiet[i].NoiSinh,
                            NGUYENQUAN = ChiTiet[i].NguyenQuan,
                            GIOITINH = ChiTiet[i].GioiTinh == "Nam" ? true : false,
                            DANTOC = ChiTiet[i].DanToc,
                            TONGIAO = ChiTiet[i].TonGiao,
                            NGHENGHIEP = ChiTiet[i].NgheNghiep,
                            NOILAMVIEC = ChiTiet[i].NoiLamViec,
                            CMND = ChiTiet[i].CMND,
                            NGAYCAP_CMND = ChiTiet[i].NgayCapCMND,
                            NOICAP_CMND = ChiTiet[i].NoiCapCMND,
                            NTNDKTT = ChiTiet[i].NgayDangKyThuongTru,
                            NTTTKCD = ChiTiet[i].NoiThuongTruTruocKhiChuyenDen,
                            NGAYCHUYENDEN = ChiTiet[i].NgayChuyenDen,
                            LYDO = ChiTiet[i].LyDo,
                            GHICHU = ChiTiet[i].GhiChu
                        };

                        hKDataContext.CTHOKHAULUUs.InsertOnSubmit(newCTHOKHAULUU);
                    }
                    hKDataContext.SubmitChanges();
                    int intResult = hKDataContext.ExecuteCommand(string.Format("update CTHOKHAU set SOHK = '{0}' where SOHK = '{1}';", hkden.SoHoKhau, hkdi.SoHoKhau));
                    if (intResult != ChiTiet.Count)
                        throw new Exception("Error when update some record");
                    MessageBox.Show("Chuyển hộ khẩu thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    MessageBox.Show("Chuyển hộ khẩu không thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
