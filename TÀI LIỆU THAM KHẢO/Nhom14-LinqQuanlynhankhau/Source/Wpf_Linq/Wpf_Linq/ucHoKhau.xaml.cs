using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using DataLayer;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
//using System.Runtime.InteropServices;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for ucHoKhau.xaml
    /// </summary>
    public partial class ucHoKhau : UserControl
    {
        ObservableCollection<clsHoKhauTemp> HoKhaus;
        CollectionViewSource hOKHAUViewSource;
        private int countItems = 0;
        private string currentSoHoKhauModify = string.Empty;
        private int currentIndexModify = 0;
        private short flag = 0;
        private bool isSearching = false;
        private bool isLoadData = false;
        private bool isDisableNavigator = false;

        public ucHoKhau()
        {
            InitializeComponent();
            hOKHAUViewSource = ((CollectionViewSource)(this.FindResource("hOKHAUViewSource")));
        }

        public void dataGridHoKhau_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridHoKhau.SelectedIndex == -1)
                return;

            if (flag == 0)
            {
                string index = ((clsHoKhauTemp)dataGridHoKhau.SelectedItem).SoHoKhau;
                clsHoKhauTemp currentHoKhau = HoKhaus.Single(c => c.SoHoKhau == index);
                txtSoHoKhau.Text = currentHoKhau.SoHoKhau;
                txtSoNha.Text = currentHoKhau.SoNha;
                txtDuong.Text = currentHoKhau.TenDuong;
                txtPhuongXa.Text = currentHoKhau.PhuongXa;
                txtQuanHuyen.Text = currentHoKhau.QuanHuyen;
                txtTinhTP.Text = currentHoKhau.TinhTP;
            }
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            Progress pg = new Progress();
            pg.Title = "Đang tải dữ liệu ... (^_^)";
            pg.IsBusy = true;
            myGrid.Children.Add(pg);

            isLoadData = true;
            BackgroundWorker loadWorker = new BackgroundWorker();
            loadWorker.DoWork += new DoWorkEventHandler(loadWorker_DoWork);
            loadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadWorker_RunWorkerCompleted);
            loadWorker.RunWorkerAsync();
        }

        #region LoadData
        void loadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);
            var query = from hk in hKDataContext.HOKHAUs    //  << Query does
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
            try
            {
                HoKhaus = new ObservableCollection<clsHoKhauTemp>(query);//    << Query Executes here 
            }
            catch
            {
                MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            countItems = HoKhaus.Count();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
            {
                btnFirst.IsEnabled = false;
                btnBack.IsEnabled = false;
                hOKHAUViewSource.Source = HoKhaus;
                hOKHAUViewSource.View.MoveCurrentToFirst();
                txtItemCount.Text = "/" + countItems.ToString();
            }));
        }
        #endregion

        void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myGrid.Children.RemoveAt(1);
            if (HoKhaus == null)
                return;
            enableButton();
            isLoadData = false;
        }

        private void btnThemHoKhau_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;
            visibleButton(Visibility.Hidden, Visibility.Visible, true);
            txtSoHoKhau.Text = string.Empty;
            txtSoNha.Text = string.Empty;
            txtDuong.Text = string.Empty;
            //txtPhuongXa.Text = string.Empty;
            //txtQuanHuyen.Text = string.Empty;
            //txtTinhTP.Text = string.Empty;
        }

        private void btnCapNhatHoKhau_Click(object sender, RoutedEventArgs e)
        {
            flag = 2;
            visibleButton(Visibility.Hidden, Visibility.Visible, true);
            //currentIndexModify = dataGridHoKhau.SelectedIndex;
            currentSoHoKhauModify = ((clsHoKhauTemp)dataGridHoKhau.SelectedItem).SoHoKhau;
            currentIndexModify = dataGridHoKhau.SelectedIndex;
        }

        private void enableButton()
        {
            txtLoadData.Text = "Refresh";
            btnCapNhatHoKhau.IsEnabled = true;
            btnThemHoKhau.IsEnabled = true;
            btnXemChiTiet.IsEnabled = true;
            cboTimKiem.IsEnabled = true;
            //btnFirst.IsEnabled = true;
            //btnBack.IsEnabled = true;
            isDisableNavigator = true;
            btnForward.IsEnabled = true;
            btnLast.IsEnabled = true;
            txtCurrentIndex.IsEnabled = true;
            //btnXoaHoKhau.IsEnabled = true;
        }

        private void visibleButton(Visibility v, Visibility v1, bool f)
        {
            btnCapNhatHoKhau.Visibility = v;
            btnThemHoKhau.Visibility = v;
            btnLuu.Visibility = v1;
            btnHuyBo.Visibility = v1;
            txtSoHoKhau.IsEnabled = f;
            txtSoNha.IsEnabled = f;
            txtDuong.IsEnabled = f;
            //txtPhuongXa.IsEnabled = f;
            //txtQuanHuyen.IsEnabled = f;
            //txtTinhTP.IsEnabled = f;
        }

        private void btnHuyBo_Click(object sender, RoutedEventArgs e)
        {
            visibleButton(Visibility.Visible, Visibility.Hidden, false);
            flag = 0;
            //dataGridHoKhau_SelectionChanged(null, null);
        }

        private void btnXoaHoKhau_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        private void luuHoKhau()
        {
            //
        }

        #region CapNhatHoKhau
        private bool capNhatHoKhau(int index)
        {
            HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);
            clsHoKhauTemp currentHoKhau = HoKhaus[index];
            if (txtSoHoKhau.Text != string.Empty && txtSoNha.Text != string.Empty && txtDuong.Text != string.Empty &&
                (txtSoHoKhau.Text != currentHoKhau.SoHoKhau || txtSoNha.Text != currentHoKhau.SoNha || txtDuong.Text != currentHoKhau.TenDuong))
            {
                try
                {
                    HOKHAU hk = hKDataContext.HOKHAUs.Single(c => c.SOHK == currentHoKhau.SoHoKhau);
                    //change SoHoKHau !!!
                    //kiem tra trung SoHoKhau !!!
                    if (txtSoHoKhau.Text != currentHoKhau.SoHoKhau)
                    {
                        int intResult = hKDataContext.ExecuteCommand(string.Format("update HOKHAU set SOHK = '{0}' where SOHK = '{1}';", txtSoHoKhau.Text, hk.SOHK));
                        if (intResult != 1)
                            throw new Exception("Error when update some record");

                    }
                    hk.SONHA = txtSoNha.Text;
                    hk.DUONG = txtDuong.Text;

                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                try
                {
                    hKDataContext.SubmitChanges();
                    MessageBox.Show(string.Format("Số hộ khẩu {0} đã cập nhật thành công", currentHoKhau.SoHoKhau));
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return false;
        }
        #endregion

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 1)
            {
                HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);
                var newHOKHAU = new HOKHAU
                {
                    SOHK = txtSoHoKhau.Text,
                    SONHA = txtSoNha.Text,
                    DUONG = txtDuong.Text,
                    PHUONGXA = txtPhuongXa.Text,
                    QUANHUYEN = txtQuanHuyen.Text,
                    TINHTP = txtTinhTP.Text,
                    TINHTRANG = 0
                };
                hKDataContext.HOKHAUs.InsertOnSubmit(newHOKHAU);
                try
                {
                    hKDataContext.SubmitChanges();
                    MessageBox.Show("Thêm hộ khẩu thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    MessageBox.Show("Thêm hộ khẩu không thành công !", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                updateData();
                countItems = HoKhaus.Count();
                txtItemCount.Text = "/" + countItems.ToString();

                dataGridHoKhau.SelectedIndex = countItems-1;
                visibleButton(Visibility.Visible, Visibility.Hidden, false);
                flag = 0;
            }
            if (flag == 2)
            {
                var query = HoKhaus
                        .Select((c, index) => new { index, c.SoHoKhau })
                        .Where(c => c.SoHoKhau == currentSoHoKhauModify)
                        .Select(c => c.index);
                if (query.Count() != 1)
                    return;
                int i = query.First();
                if (!capNhatHoKhau(i))
                    return;
                if (isSearching)
                {
                    //change SoHoKHau !!!
                    HoKhaus[i].SoHoKhau = txtSoHoKhau.Text;
                    HoKhaus[i].SoNha = txtSoNha.Text;
                    HoKhaus[i].TenDuong = txtDuong.Text;
                    timKiem(null, null);
                    goto exit;
                }
                refreshData(i);//dataGridHoKhau_SelectionChanged
                //update Display
                dataGridHoKhau.SelectedIndex = currentIndexModify;
            //dataGridHoKhau.ScrollIntoView(dataGridHoKhau.SelectedItem);
            exit:
                visibleButton(Visibility.Visible, Visibility.Hidden, false);
                flag = 0;
            }
        }

        private void updateData()
        {
            //change SoHoKHau !!!
            clsHoKhauTemp hk = new clsHoKhauTemp();
            hk.SoHoKhau = txtSoHoKhau.Text;
            hk.SoNha = txtSoNha.Text;
            hk.TenDuong = txtDuong.Text;
            hk.PhuongXa = txtPhuongXa.Text;
            hk.QuanHuyen = txtQuanHuyen.Text;
            hk.TinhTP=txtTinhTP.Text;
            hk.TinhTrang = string.Empty;
            hk.ChiTiet = null;
            HoKhaus.Add(hk);
            hOKHAUViewSource.View.Refresh();
        }

        private void refreshData(int index)
        {
            //change SoHoKHau !!!
            HoKhaus[index].SoHoKhau = txtSoHoKhau.Text;
            HoKhaus[index].SoNha = txtSoNha.Text;
            HoKhaus[index].TenDuong = txtDuong.Text;
            hOKHAUViewSource.View.Refresh();
        }

        private void txtCurrentIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItems == 0 || isLoadData || hOKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndex.Text, out index);
            if (!f)
                return;
            --index;
                dataGridHoKhau.ScrollIntoView(HoKhaus[index]);//hOKHAUViewSource.View.CurrentItem     dataGridHoKhau.SelectedItem       potay.com T__T
            if (isDisableNavigator)
            {
                enable_1(true);
                enable_2(true);
            }
            if (index == 0)//hOKHAUViewSource.View.CurrentPosition       potay.com T__T
            {
                enable_1(false);
                isDisableNavigator = true;
            }
            if (index == countItems - 1)//hOKHAUViewSource.View.CurrentPosition       potay.com T__T
            {
                enable_2(false);
                isDisableNavigator = true;
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

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            hOKHAUViewSource.View.MoveCurrentToLast();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            hOKHAUViewSource.View.MoveCurrentToNext();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            hOKHAUViewSource.View.MoveCurrentToPrevious();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            hOKHAUViewSource.View.MoveCurrentToFirst();
        }

        private void cboTimKiem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTimKiem.SelectedIndex != 0)
                txtTimKiem.IsEnabled = true;
            else
                txtTimKiem.IsEnabled = false;
        }

        #region TimKiem
        private void timKiem(object sender, RoutedEventArgs e)
        {
            string strTim = txtTimKiem.Text;
            if (strTim == string.Empty)
            {
                hOKHAUViewSource.Source = null;
                hOKHAUViewSource.Source = HoKhaus;
                hOKHAUViewSource.View.MoveCurrentToFirst();
                countItems = HoKhaus.Count;
                txtItemCount.Text = "/" + countItems.ToString();
                isSearching = false;
                return;
            }
            int t;
            //start filtering
            var query = HoKhaus.AsEnumerable();
            if (cboTimKiem.SelectedIndex == 1)
            {
                if (rbnGanDung.IsChecked == true)
                    query = query.Where(hk => hk.SoNha.Contains(strTim));
                else
                    query = query.Where(hk => hk.SoNha.Equals(strTim));
            }
            else if (cboTimKiem.SelectedIndex == 2)
            {
                if (rbnGanDung.IsChecked == true)
                    query = query.Where(hk => hk.TenDuong.Contains(strTim));
                else
                    query = query.Where(hk => hk.TenDuong.Equals(strTim));
            }
            else
                return;
            t = query.Count();
            if (t < 1)
            {
                countItems = 0;
                txtItemCount.Text = "/" + countItems.ToString();
                hOKHAUViewSource.Source = null;
                return;
            }
            ObservableCollection<clsHoKhauTemp> HoKhausTemp = new ObservableCollection<clsHoKhauTemp>(query);
            countItems = t;
            txtItemCount.Text = "/" + countItems.ToString();
            hOKHAUViewSource.Source = null;
            hOKHAUViewSource.Source = HoKhausTemp;
            hOKHAUViewSource.View.MoveCurrentToFirst();
            isSearching = true;
            //Radio Button & ComboBox selection change 
        }
        #endregion

        private void btnXemChiTiet_Click(object sender, RoutedEventArgs e)
        {
            clsHoKhauTemp hokhau = hOKHAUViewSource.View.CurrentItem as clsHoKhauTemp;
            windowNhanKhau w = new windowNhanKhau(hokhau);
            w.Show();
        }
    }
}
