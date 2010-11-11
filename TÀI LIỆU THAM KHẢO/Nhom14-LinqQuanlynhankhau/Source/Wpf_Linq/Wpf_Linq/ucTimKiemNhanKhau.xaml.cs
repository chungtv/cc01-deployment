﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using DataLayer;
using System.ComponentModel;
using System.Data;
using Wpf_Linq.Reports;
using System.Windows.Threading;
using System.Linq;
using System;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for ucTimKiemNhanKhau.xaml
    /// </summary>
    public partial class ucTimKiemNhanKhau : UserControl
    {
        HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);
        string strTim;
        CollectionViewSource thongTinNHANKHAUViewSource;
        CollectionViewSource chuyenNHANKHAUViewSource;
        CollectionViewSource catNHANKHAUViewSource;
        CollectionViewSource doiTUONGVIPHAMViewSource;
        ObservableCollection<CTHOKHAU> NhanKhaus;
        ObservableCollection<clsNhanKhauCatChuyen> NhanKhausChuyen;
        ObservableCollection<NHANKHAULUU> NhanKhausCat;
        ObservableCollection<DOITUONG> DoiTuongViPham;
        short search;
        private int countItems = 0;
        private bool isDisableNavigator = false;

        public ucTimKiemNhanKhau()
        {
            InitializeComponent();
            thongTinNHANKHAUViewSource = ((CollectionViewSource)(this.FindResource("thongTinNHANKHAUViewSource")));
            chuyenNHANKHAUViewSource = ((CollectionViewSource)(this.FindResource("chuyenNHANKHAUViewSource")));
            catNHANKHAUViewSource = ((CollectionViewSource)(this.FindResource("catNHANKHAUViewSource")));
            doiTUONGVIPHAMViewSource = ((CollectionViewSource)(this.FindResource("doiTUONGVIPHAMViewSource")));
            rbnThongTinNhanKhau.Checked += new RoutedEventHandler(rbnThongTinNhanKhau_Checked);
            rbnChuyenNhanKhau.Checked += new RoutedEventHandler(rbnChuyenNhanKhau_Checked);
            rbnCatNhanKhau.Checked += new RoutedEventHandler(rbnCatNhanKhau_Checked);
            rbnDoiTuongViPham.Checked += new RoutedEventHandler(rbnDoiTuongViPham_Checked);
            rbnTimTatCa_2.Checked += new RoutedEventHandler(rbnTimTatCa_2_Checked);
            rbnTimHoTen.Checked += new RoutedEventHandler(rbnTimHoTen_Checked);
            rbnTimCMND.Checked += new RoutedEventHandler(rbnTimCMND_Checked);
            rbnTimChuHo.Checked += new RoutedEventHandler(rbnTimChuHo_Checked);
        }

        #region Navigator
        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (thongTinNHANKHAUViewSource.Source != null)
            {
                thongTinNHANKHAUViewSource.View.MoveCurrentToNext();
                return;
            }
            else if (chuyenNHANKHAUViewSource.Source != null)
            {
                chuyenNHANKHAUViewSource.View.MoveCurrentToNext();
                return;
            }
            else if (catNHANKHAUViewSource.Source != null)
            {
                catNHANKHAUViewSource.View.MoveCurrentToNext();
                return;
            }

            else if (doiTUONGVIPHAMViewSource.Source != null)
            {
                doiTUONGVIPHAMViewSource.View.MoveCurrentToNext();
                return;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (thongTinNHANKHAUViewSource.Source != null)
            {
                thongTinNHANKHAUViewSource.View.MoveCurrentToPrevious();
                return;
            }
            else if (chuyenNHANKHAUViewSource.Source != null)
            {
                chuyenNHANKHAUViewSource.View.MoveCurrentToPrevious();
                return;
            }
            else if (catNHANKHAUViewSource.Source != null)
            {
                catNHANKHAUViewSource.View.MoveCurrentToPrevious();
                return;
            }

            else if (doiTUONGVIPHAMViewSource.Source != null)
            {
                doiTUONGVIPHAMViewSource.View.MoveCurrentToPrevious();
                return;
            }
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (thongTinNHANKHAUViewSource.Source != null)
            {
                thongTinNHANKHAUViewSource.View.MoveCurrentToFirst();
                return;
            }
            else if (chuyenNHANKHAUViewSource.Source != null)
            {
                chuyenNHANKHAUViewSource.View.MoveCurrentToFirst();
                return;
            }
            else if (catNHANKHAUViewSource.Source != null)
            {
                catNHANKHAUViewSource.View.MoveCurrentToFirst();
                return;
            }

            else if (doiTUONGVIPHAMViewSource.Source != null)
            {
                doiTUONGVIPHAMViewSource.View.MoveCurrentToFirst();
                return;
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (thongTinNHANKHAUViewSource.Source != null)
            {
                thongTinNHANKHAUViewSource.View.MoveCurrentToLast();
                return;
            }
            else if (chuyenNHANKHAUViewSource.Source != null)
            {
                chuyenNHANKHAUViewSource.View.MoveCurrentToLast();
                return;
            }
            else if (catNHANKHAUViewSource.Source != null)
            {
                catNHANKHAUViewSource.View.MoveCurrentToLast();
                return;
            }

            else if (doiTUONGVIPHAMViewSource.Source != null)
            {
                doiTUONGVIPHAMViewSource.View.MoveCurrentToLast();
                return;
            }
        }
        #endregion

        private void btnInChiTiet_Click(object sender, RoutedEventArgs e)
        {
            Progress pg = new Progress();
            pg.Title = "Đang xử lý ... (^_^)";
            pg.IsBusy = true;
            myGridTimKiemNhanKhau.Children.Add(pg);

            if (rbnThongTinNhanKhau.IsChecked == true)
                search = 1234;
            else if (rbnChuyenNhanKhau.IsChecked == true)
                search = 5678;
            else if (rbnCatNhanKhau.IsChecked == true)
                search = 9101;
            else if (rbnDoiTuongViPham.IsChecked == true)
                search = 1121;

            BackgroundWorker loadWorker = new BackgroundWorker();
            loadWorker.DoWork += new DoWorkEventHandler(loadWorker_DoWork_Main);
            loadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadWorker_RunWorkerCompleted_Main);
            loadWorker.RunWorkerAsync();
        }

        #region In
        void loadWorker_DoWork_Main(object sender, DoWorkEventArgs e)
        {
            if (search == 1234)
            {
                DataTable t = new DataTable();
                t.Columns.Add("HO", Type.GetType("System.String"));
                t.Columns.Add("CHULOT", Type.GetType("System.String"));
                t.Columns.Add("TEN", Type.GetType("System.String"));
                t.Columns.Add("QH", Type.GetType("System.String"));
                t.Columns.Add("NGS", Type.GetType("System.String"));
                t.Columns.Add("BIDANH", Type.GetType("System.String"));
                t.Columns.Add("TDP", Type.GetType("System.Int32"));
                t.Columns.Add("NS", Type.GetType("System.String"));
                t.Columns.Add("NQ", Type.GetType("System.String"));
                t.Columns.Add("SEX", Type.GetType("System.String"));
                t.Columns.Add("DT", Type.GetType("System.String"));
                t.Columns.Add("TG", Type.GetType("System.String"));
                t.Columns.Add("NN", Type.GetType("System.String"));
                t.Columns.Add("NLV", Type.GetType("System.String"));
                t.Columns.Add("CMND", Type.GetType("System.String"));
                t.Columns.Add("NGAYCAP_CMND", Type.GetType("System.String"));
                t.Columns.Add("NOICAP_CMND", Type.GetType("System.String"));
                t.Columns.Add("NTNDKTT", Type.GetType("System.String"));
                t.Columns.Add("NTTTKCD", Type.GetType("System.String"));
                t.Columns.Add("NGAYCDEN", Type.GetType("System.String"));
                t.Columns.Add("LYDO", Type.GetType("System.String"));
                t.Columns.Add("GHICHU", Type.GetType("System.String"));

                DataRow r;
                for (int i = 0; i < NhanKhaus.Count; i++)
                {
                    r = t.NewRow();
                    r["HO"] = NhanKhaus[i].HO;
                    r["CHULOT"] = NhanKhaus[i].CHULOT;
                    r["TEN"] = NhanKhaus[i].TEN;
                    r["QH"] = NhanKhaus[i].QUANHE.QUANHE1;
                    r["NGS"] = NhanKhaus[i].NGAYSINH;
                    r["BIDANH"] = NhanKhaus[i].BIDANH;
                    r["TDP"] = NhanKhaus[i].TODANPHO;
                    r["NS"] = NhanKhaus[i].NOISINH;
                    r["NQ"] = NhanKhaus[i].NGUYENQUAN;
                    r["SEX"] = NhanKhaus[i].GIOITINH == true ? "Nam" : "Nữ";
                    r["DT"] = NhanKhaus[i].DANTOC;
                    r["TG"] = NhanKhaus[i].TONGIAO;
                    r["NN"] = NhanKhaus[i].NGHENGHIEP;
                    r["NLV"] = NhanKhaus[i].NOILAMVIEC;
                    r["CMND"] = NhanKhaus[i].CMND;
                    if (NhanKhaus[i].NGAYCAP_CMND.HasValue)
                    {
                        r["NGAYCAP_CMND"] = NhanKhaus[i].NGAYCAP_CMND.Value.ToShortDateString();
                    }
                    else
                        r["NGAYCAP_CMND"] = "";
                    r["NOICAP_CMND"] = NhanKhaus[i].NOICAP_CMND;
                    if (NhanKhaus[i].NTNDKTT.HasValue)
                    {
                        r["NTNDKTT"] = NhanKhaus[i].NTNDKTT.Value.ToShortDateString();
                    }
                    else
                        r["NTNDKTT"] = "";
                    r["NTTTKCD"] = NhanKhaus[i].NTTTKCD;
                    if (NhanKhaus[i].NGAYCHUYENDEN.HasValue)
                    {
                        r["NGAYCDEN"] = NhanKhaus[i].NGAYCHUYENDEN.Value.ToShortDateString();
                    }
                    else
                        r["NGAYCDEN"] = "";
                    r["LYDO"] = NhanKhaus[i].LYDO;
                    r["GHICHU"] = NhanKhaus[i].GHICHU;

                    t.Rows.Add(r);
                }

                CryRe_TK_NK_TTNK objRpt = new CryRe_TK_NK_TTNK();
                objRpt.SetDataSource(t);
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    windowReportView reportviewer = new windowReportView();
                    reportviewer.rptViewer.ReportSource = objRpt;
                    reportviewer.rptViewer.Refresh();
                    reportviewer.Show();
                }));
            }
            else if (search == 5678)
            {
                DataTable t = new DataTable();
                t.Columns.Add("SOHKCDEN", Type.GetType("System.String"));
                t.Columns.Add("DCCDEN", Type.GetType("System.String"));
                t.Columns.Add("HO", Type.GetType("System.String"));
                t.Columns.Add("CHULOT", Type.GetType("System.String"));
                t.Columns.Add("TEN", Type.GetType("System.String"));
                t.Columns.Add("QH", Type.GetType("System.String"));
                t.Columns.Add("NGS", Type.GetType("System.DateTime"));
                t.Columns.Add("BIDANH", Type.GetType("System.String"));
                t.Columns.Add("TDP", Type.GetType("System.Int32"));
                t.Columns.Add("NS", Type.GetType("System.String"));
                t.Columns.Add("NQ", Type.GetType("System.String"));
                t.Columns.Add("SEX", Type.GetType("System.String"));
                t.Columns.Add("DT", Type.GetType("System.String"));
                t.Columns.Add("TG", Type.GetType("System.String"));
                t.Columns.Add("NN", Type.GetType("System.String"));
                t.Columns.Add("NLV", Type.GetType("System.String"));
                t.Columns.Add("CMND", Type.GetType("System.String"));
                t.Columns.Add("NGAYCAP_CMND", Type.GetType("System.String"));
                t.Columns.Add("NOICAP_CMND", Type.GetType("System.String"));
                t.Columns.Add("NTNDKTT", Type.GetType("System.String"));
                t.Columns.Add("NGAYCDI", Type.GetType("System.String"));
                t.Columns.Add("LYDO", Type.GetType("System.String"));
                t.Columns.Add("GHICHU", Type.GetType("System.String"));
                t.Columns.Add("SOHKDI", Type.GetType("System.Int32"));
                t.Columns.Add("DCDI", Type.GetType("System.String"));

                DataRow r;
                for (int i = 0; i < NhanKhausChuyen.Count; i++)
                {
                    r = t.NewRow();
                    r["SOHKCDEN"] = NhanKhausChuyen[i].SoHoSo;
                    r["DCCDEN"] = NhanKhausChuyen[i].NoiChuyenDen;
                    r["HO"] = NhanKhausChuyen[i].Ho;
                    r["CHULOT"] = NhanKhausChuyen[i].ChuLot;
                    r["TEN"] = NhanKhausChuyen[i].Ten;
                    r["QH"] = NhanKhausChuyen[i].QHVoiChuHo;
                    r["NGS"] = NhanKhausChuyen[i].NgaySinh;
                    r["BIDANH"] = NhanKhausChuyen[i].BiDanh;
                    r["TDP"] = NhanKhausChuyen[i].ToDanPho;
                    r["NS"] = NhanKhausChuyen[i].NoiSinh;
                    r["NQ"] = NhanKhausChuyen[i].NguyenQuan;
                    r["SEX"] = NhanKhausChuyen[i].GioiTinh;
                    r["DT"] = NhanKhausChuyen[i].DanToc;
                    r["TG"] = NhanKhausChuyen[i].TonGiao;
                    r["NN"] = NhanKhausChuyen[i].NgheNghiep;
                    r["NLV"] = NhanKhausChuyen[i].NoiLamViec;
                    r["CMND"] = NhanKhausChuyen[i].CMND;
                    if (NhanKhausChuyen[i].NgayCapCMND.HasValue)
                    {
                        r["NGAYCAP_CMND"] = NhanKhausChuyen[i].NgayCapCMND.Value.ToShortDateString();
                    }
                    else
                        r["NGAYCAP_CMND"] = "";
                    r["NOICAP_CMND"] = NhanKhausChuyen[i].NoiCapCMND;
                    if (NhanKhausChuyen[i].NgayDangKyThuongTru.HasValue)
                    {
                        r["NTNDKTT"] = NhanKhausChuyen[i].NgayDangKyThuongTru.Value.ToShortDateString();
                    }
                    else
                        r["NTNDKTT"] = "";
                    if (NhanKhausChuyen[i].NgayChuyenDi.HasValue)
                    {
                        r["NGAYCDI"] = NhanKhausChuyen[i].NgayChuyenDi.Value.ToShortDateString();
                    }
                    else
                        r["NGAYCDI"] = "";
                    r["LYDO"] = NhanKhausChuyen[i].LyDoChuyen;
                    r["GHICHU"] = NhanKhausChuyen[i].GhiChu;
                    r["SOHKDI"] = NhanKhausChuyen[i].SoHoKhau;
                    r["DCDI"] = NhanKhausChuyen[i].DC;

                    t.Rows.Add(r);
                }

                CryRe_TK_NK_CHNK objRpt = new CryRe_TK_NK_CHNK();
                objRpt.SetDataSource(t);
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    windowReportView reportviewer = new windowReportView();
                    reportviewer.rptViewer.ReportSource = objRpt;
                    reportviewer.rptViewer.Refresh();
                    reportviewer.Show();
                }));
            }
            else if (search == 9101)
            {
                DataTable t = new DataTable();
                t.Columns.Add("HO", Type.GetType("System.String"));
                t.Columns.Add("CHULOT", Type.GetType("System.String"));
                t.Columns.Add("TEN", Type.GetType("System.String"));
                t.Columns.Add("QH", Type.GetType("System.String"));
                t.Columns.Add("NGS", Type.GetType("System.String"));
                t.Columns.Add("BIDANH", Type.GetType("System.String"));
                t.Columns.Add("TDP", Type.GetType("System.Int32"));
                t.Columns.Add("NS", Type.GetType("System.String"));
                t.Columns.Add("NQ", Type.GetType("System.String"));
                t.Columns.Add("SEX", Type.GetType("System.String"));
                t.Columns.Add("DT", Type.GetType("System.String"));
                t.Columns.Add("TG", Type.GetType("System.String"));
                t.Columns.Add("NN", Type.GetType("System.String"));
                t.Columns.Add("NLV", Type.GetType("System.String"));
                t.Columns.Add("CMND", Type.GetType("System.String"));
                t.Columns.Add("NGAYCAP_CMND", Type.GetType("System.String"));
                t.Columns.Add("NOICAP_CMND", Type.GetType("System.String"));
                t.Columns.Add("NTNDKTT", Type.GetType("System.String"));
                t.Columns.Add("NGAYCDI", Type.GetType("System.String"));
                t.Columns.Add("LYDO", Type.GetType("System.String"));
                t.Columns.Add("GHICHU", Type.GetType("System.String"));

                DataRow r;
                for (int i = 0; i < NhanKhausCat.Count; i++)
                {
                    r = t.NewRow();
                    r["HO"] = NhanKhausCat[i].HO;
                    r["CHULOT"] = NhanKhausCat[i].CHULOT;
                    r["TEN"] = NhanKhausCat[i].TEN;
                    r["QH"] = NhanKhausCat[i].QHVOICHUHO;
                    r["NGS"] = NhanKhausCat[i].NGAYSINH;
                    r["BIDANH"] = NhanKhausCat[i].BIDANH;
                    r["TDP"] = NhanKhausCat[i].TODANPHO;
                    r["NS"] = NhanKhausCat[i].NOISINH;
                    r["NQ"] = NhanKhausCat[i].NGUYENQUAN;
                    r["SEX"] = NhanKhausCat[i].GIOITINH == true ? "Nam" : "Nữ";
                    r["DT"] = NhanKhausCat[i].DANTOC;
                    r["TG"] = NhanKhausCat[i].TONGIAO;
                    r["NN"] = NhanKhausCat[i].NGHENGHIEP;
                    r["NLV"] = NhanKhausCat[i].NOILAMVIEC;
                    r["CMND"] = NhanKhausCat[i].CMND;
                    if (NhanKhausCat[i].NGAYCAP_CMND.HasValue)
                    {
                        r["NGAYCAP_CMND"] = NhanKhausCat[i].NGAYCAP_CMND.Value.ToShortDateString();
                    }
                    else
                        r["NGAYCAP_CMND"] = "";
                    r["NOICAP_CMND"] = NhanKhausCat[i].NOICAP_CMND;
                    if (NhanKhausCat[i].NTNDKTT.HasValue)
                    {
                        r["NTNDKTT"] = NhanKhausCat[i].NTNDKTT.Value.ToShortDateString();
                    }
                    else
                        r["NTNDKTT"] = "";
                    if (NhanKhausCat[i].NGAYCHUYENDI.HasValue)
                    {
                        r["NGAYCDI"] = NhanKhausCat[i].NGAYCHUYENDI.Value.ToShortDateString();
                    }
                    else
                        r["NGAYCDI"] = "";
                    r["LYDO"] = NhanKhausCat[i].LYDOCHUYEN;
                    r["GHICHU"] = NhanKhausCat[i].GHICHU;

                    t.Rows.Add(r);
                }

                CryRe_TK_NK_CNK objRpt = new CryRe_TK_NK_CNK();
                objRpt.SetDataSource(t);
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    windowReportView reportviewer = new windowReportView();
                    reportviewer.rptViewer.ReportSource = objRpt;
                    reportviewer.rptViewer.Refresh();
                    reportviewer.Show();
                }));
            }
            else if (search == 1121)
            {
                DataTable t = new DataTable();
                t.Columns.Add("TEN", Type.GetType("System.String"));
                t.Columns.Add("CMND", Type.GetType("System.String"));
                t.Columns.Add("SEX", Type.GetType("System.String"));
                t.Columns.Add("NGAYSINH", Type.GetType("System.String"));
                t.Columns.Add("BIDANH", Type.GetType("System.String"));
                t.Columns.Add("LOAIDT", Type.GetType("System.String"));
                t.Columns.Add("QHCH", Type.GetType("System.String"));
                t.Columns.Add("NSINH", Type.GetType("System.String"));
                t.Columns.Add("NQUAN", Type.GetType("System.String"));

                DataRow r;
                for (int i = 0; i < DoiTuongViPham.Count; i++)
                {
                    r = t.NewRow();
                    r["TEN"] = DoiTuongViPham[i].HO + ' ' + DoiTuongViPham[i].CHULOT + ' ' + DoiTuongViPham[i].TEN;
                    r["LOAIDT"] = DoiTuongViPham[i].LOAIDOITUONG.LOAIDT;
                    r["CMND"] = DoiTuongViPham[i].CMND;
                    r["QHCH"] = DoiTuongViPham[i].QHVOICHUHO;
                    r["NGAYSINH"] = DoiTuongViPham[i].NGAYSINH;
                    r["BIDANH"] = DoiTuongViPham[i].BIDANH;
                    r["NSINH"] = DoiTuongViPham[i].NOISINH;
                    r["NQUAN"] = DoiTuongViPham[i].NGUYENQUAN;
                    r["SEX"] = DoiTuongViPham[i].GIOITINH == true ? "Nam" : "Nữ";

                    t.Rows.Add(r);
                }

                CryRe_TK_NK_DT objRpt = new CryRe_TK_NK_DT();
                objRpt.SetDataSource(t);
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    windowReportView reportviewer = new windowReportView();
                    reportviewer.rptViewer.ReportSource = objRpt;
                    reportviewer.rptViewer.Refresh();
                    reportviewer.Show();
                }));
            }
        }
        #endregion

        void loadWorker_RunWorkerCompleted_Main(object sender, RunWorkerCompletedEventArgs e)
        {
            myGridTimKiemNhanKhau.Children.RemoveAt(1);
        }

        private void timKiem(object sender, RoutedEventArgs e)
        {
            Progress pg = new Progress();
            pg.Title = "Đang tải dữ liệu ... (^_^)";
            pg.IsBusy = true;
            myGrid_11.Children.Add(pg);

            strTim = txtTimKiem.Text;
            enable_1(false);
            enable_2(false);
            isDisableNavigator = true;
            btnInChiTiet.IsEnabled = false;

            if (rbnThongTinNhanKhau.IsChecked == true)
            {
                if (rbnTimTatCa_2.IsChecked == true)
                    search = 1;
                else if (rbnTimHoTen.IsChecked == true)
                    search = 2;
                else if (rbnTimCMND.IsChecked == true)
                    search = 3;
                else if (rbnTimChuHo.IsChecked == true)
                    search = 4;
            }
            else if (rbnChuyenNhanKhau.IsChecked == true)
            {
                if (rbnTimTatCa_2.IsChecked == true)
                    search = 5;
                else if (rbnTimHoTen.IsChecked == true)
                    search = 6;
                else if (rbnTimCMND.IsChecked == true)
                    search = 7;
                else if (rbnTimChuHo.IsChecked == true)
                    search = 8;
            }
            else if (rbnCatNhanKhau.IsChecked == true)
            {
                if (rbnTimTatCa_2.IsChecked == true)
                    search = 9;
                else if (rbnTimHoTen.IsChecked == true)
                    search = 10;
                else if (rbnTimCMND.IsChecked == true)
                    search = 11;
                else if (rbnTimChuHo.IsChecked == true)
                    search = 12;
            }
            else if (rbnDoiTuongViPham.IsChecked == true)
            {
                if (rbnTimTatCa_2.IsChecked == true)
                    search = 13;
                else if (rbnTimHoTen.IsChecked == true)
                    search = 14;
                else if (rbnTimCMND.IsChecked == true)
                    search = 15;
                else if (rbnTimChuHo.IsChecked == true)
                    search = 16;
            }

            BackgroundWorker loadWorker = new BackgroundWorker();
            loadWorker.DoWork += new DoWorkEventHandler(loadWorker_DoWork);
            loadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadWorker_RunWorkerCompleted);
            loadWorker.RunWorkerAsync();
        }

        #region search
        void loadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            nulll();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
            {
                nulll_2();
            }));
            if (strTim == string.Empty)
            {
                return;
            }

            #region 1234
            if (search == 1)
            {
                var query = from hk in hKDataContext.CTHOKHAUs    //  << Query does
                            select hk;
                try
                {
                    NhanKhaus = new ObservableCollection<CTHOKHAU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhaus.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    thongTinNHANKHAUViewSource.Source = NhanKhaus;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexThongTinNhanKhau_TextChanged(null, null);
                    txtCurrentIndexThongTinNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 2)
            {
                var query = from hk in hKDataContext.CTHOKHAUs    //  << Query does
                            where (hk.HO + ' ' + hk.CHULOT + ' ' + hk.TEN).Contains(strTim)
                            select hk;
                try
                {
                    NhanKhaus = new ObservableCollection<CTHOKHAU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhaus.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    thongTinNHANKHAUViewSource.Source = NhanKhaus;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexThongTinNhanKhau_TextChanged(null, null);
                    txtCurrentIndexThongTinNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 3)
            {
                var query = from hk in hKDataContext.CTHOKHAUs    //  << Query does
                            where hk.CMND.Contains(strTim)
                            select hk;
                try
                {
                    NhanKhaus = new ObservableCollection<CTHOKHAU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhaus.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    thongTinNHANKHAUViewSource.Source = NhanKhaus;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexThongTinNhanKhau_TextChanged(null, null);
                    txtCurrentIndexThongTinNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 4)
            {
                var query = from hk in hKDataContext.CTHOKHAUs    //  << Query does
                            where hk.QHVOICHUHO == 0
                            where (hk.HO + ' ' + hk.CHULOT + ' ' + hk.TEN).Contains(strTim)
                            select hk;
                try
                {
                    NhanKhaus = new ObservableCollection<CTHOKHAU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhaus.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    thongTinNHANKHAUViewSource.Source = NhanKhaus;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexThongTinNhanKhau_TextChanged(null, null);
                    txtCurrentIndexThongTinNhanKhau.IsEnabled = true;
                }));
            }
            #endregion

            #region 5678
            else if (search == 5)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            join hk in hKDataContext.HOKHAUs
                                on nk.SOHK equals hk.SOHK
                            where nk.SOHK != nk.SOHSCHUYENDI
                            select new clsNhanKhauCatChuyen
                            {
                                Ho = nk.HO,
                                ChuLot = nk.CHULOT,
                                Ten = nk.TEN,
                                QHVoiChuHo = nk.QHVOICHUHO,
                                NgaySinh = nk.NGAYSINH,
                                BiDanh = nk.BIDANH,
                                ToDanPho = nk.TODANPHO,
                                NoiSinh = nk.NOISINH,
                                NguyenQuan = nk.NGUYENQUAN,
                                GioiTinh = nk.GIOITINH == true ? "Nam" : "Nữ",
                                DanToc = nk.DANTOC,
                                TonGiao = nk.TONGIAO,
                                NgheNghiep = nk.NGHENGHIEP,
                                NoiLamViec = nk.NOILAMVIEC,
                                CMND = nk.CMND,
                                NgayCapCMND = nk.NGAYCAP_CMND,
                                NoiCapCMND = nk.NOICAP_CMND,
                                NgayDangKyThuongTru = nk.NTNDKTT,
                                NoiChuyenDen = nk.NOICHUYENDEN,
                                SoHoSo = nk.SOHSCHUYENDI,
                                SoHoKhau = nk.SOHK,
                                NgayChuyenDi = nk.NGAYCHUYENDI,
                                LyDoChuyen = nk.LYDOCHUYEN,
                                GhiChu = nk.GHICHU,
                                DC = hk.SONHA + ' ' + hk.DUONG + ' ' + hk.PHUONGXA + ' ' + hk.QUANHUYEN + ' ' + hk.TINHTP
                            };
                try
                {
                    NhanKhausChuyen = new ObservableCollection<clsNhanKhauCatChuyen>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausChuyen.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    chuyenNHANKHAUViewSource.Source = NhanKhausChuyen;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexChuyenNhanKhau_TextChanged(null, null);
                    txtCurrentIndexChuyenNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 6)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            join hk in hKDataContext.HOKHAUs
                                on nk.SOHK equals hk.SOHK
                            where nk.SOHK != nk.SOHSCHUYENDI
                            where (nk.HO + ' ' + nk.CHULOT + ' ' + nk.TEN).Contains(strTim)
                            select new clsNhanKhauCatChuyen
                            {
                                Ho = nk.HO,
                                ChuLot = nk.CHULOT,
                                Ten = nk.TEN,
                                QHVoiChuHo = nk.QHVOICHUHO,
                                NgaySinh = nk.NGAYSINH,
                                BiDanh = nk.BIDANH,
                                ToDanPho = nk.TODANPHO,
                                NoiSinh = nk.NOISINH,
                                NguyenQuan = nk.NGUYENQUAN,
                                GioiTinh = nk.GIOITINH == true ? "Nam" : "Nữ",
                                DanToc = nk.DANTOC,
                                TonGiao = nk.TONGIAO,
                                NgheNghiep = nk.NGHENGHIEP,
                                NoiLamViec = nk.NOILAMVIEC,
                                CMND = nk.CMND,
                                NgayCapCMND = nk.NGAYCAP_CMND,
                                NoiCapCMND = nk.NOICAP_CMND,
                                NgayDangKyThuongTru = nk.NTNDKTT,
                                NoiChuyenDen = nk.NOICHUYENDEN,
                                SoHoSo = nk.SOHSCHUYENDI,
                                SoHoKhau = nk.SOHK,
                                NgayChuyenDi = nk.NGAYCHUYENDI,
                                LyDoChuyen = nk.LYDOCHUYEN,
                                GhiChu = nk.GHICHU,
                                DC = hk.SONHA + ' ' + hk.DUONG + ' ' + hk.PHUONGXA + ' ' + hk.QUANHUYEN + ' ' + hk.TINHTP
                            };
                try
                {
                    NhanKhausChuyen = new ObservableCollection<clsNhanKhauCatChuyen>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausChuyen.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    chuyenNHANKHAUViewSource.Source = NhanKhausChuyen;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexChuyenNhanKhau_TextChanged(null, null);
                    txtCurrentIndexChuyenNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 7)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            join hk in hKDataContext.HOKHAUs
                                on nk.SOHK equals hk.SOHK
                            where nk.SOHK != nk.SOHSCHUYENDI
                            where nk.CMND.Contains(strTim)
                            select new clsNhanKhauCatChuyen
                            {
                                Ho = nk.HO,
                                ChuLot = nk.CHULOT,
                                Ten = nk.TEN,
                                QHVoiChuHo = nk.QHVOICHUHO,
                                NgaySinh = nk.NGAYSINH,
                                BiDanh = nk.BIDANH,
                                ToDanPho = nk.TODANPHO,
                                NoiSinh = nk.NOISINH,
                                NguyenQuan = nk.NGUYENQUAN,
                                GioiTinh = nk.GIOITINH == true ? "Nam" : "Nữ",
                                DanToc = nk.DANTOC,
                                TonGiao = nk.TONGIAO,
                                NgheNghiep = nk.NGHENGHIEP,
                                NoiLamViec = nk.NOILAMVIEC,
                                CMND = nk.CMND,
                                NgayCapCMND = nk.NGAYCAP_CMND,
                                NoiCapCMND = nk.NOICAP_CMND,
                                NgayDangKyThuongTru = nk.NTNDKTT,
                                NoiChuyenDen = nk.NOICHUYENDEN,
                                SoHoSo = nk.SOHSCHUYENDI,
                                SoHoKhau = nk.SOHK,
                                NgayChuyenDi = nk.NGAYCHUYENDI,
                                LyDoChuyen = nk.LYDOCHUYEN,
                                GhiChu = nk.GHICHU,
                                DC = hk.SONHA + ' ' + hk.DUONG + ' ' + hk.PHUONGXA + ' ' + hk.QUANHUYEN + ' ' + hk.TINHTP
                            };
                try
                {
                    NhanKhausChuyen = new ObservableCollection<clsNhanKhauCatChuyen>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausChuyen.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    chuyenNHANKHAUViewSource.Source = NhanKhausChuyen;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexChuyenNhanKhau_TextChanged(null, null);
                    txtCurrentIndexChuyenNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 8)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            join hk in hKDataContext.HOKHAUs
                                on nk.SOHK equals hk.SOHK
                            where nk.SOHK != nk.SOHSCHUYENDI
                            where nk.QHVOICHUHO == "Chủ hộ"
                            where (nk.HO + ' ' + nk.CHULOT + ' ' + nk.TEN).Contains(strTim)
                            select new clsNhanKhauCatChuyen
                            {
                                Ho = nk.HO,
                                ChuLot = nk.CHULOT,
                                Ten = nk.TEN,
                                QHVoiChuHo = nk.QHVOICHUHO,
                                NgaySinh = nk.NGAYSINH,
                                BiDanh = nk.BIDANH,
                                ToDanPho = nk.TODANPHO,
                                NoiSinh = nk.NOISINH,
                                NguyenQuan = nk.NGUYENQUAN,
                                GioiTinh = nk.GIOITINH == true ? "Nam" : "Nữ",
                                DanToc = nk.DANTOC,
                                TonGiao = nk.TONGIAO,
                                NgheNghiep = nk.NGHENGHIEP,
                                NoiLamViec = nk.NOILAMVIEC,
                                CMND = nk.CMND,
                                NgayCapCMND = nk.NGAYCAP_CMND,
                                NoiCapCMND = nk.NOICAP_CMND,
                                NgayDangKyThuongTru = nk.NTNDKTT,
                                NoiChuyenDen = nk.NOICHUYENDEN,
                                SoHoSo = nk.SOHSCHUYENDI,
                                SoHoKhau = nk.SOHK,
                                NgayChuyenDi = nk.NGAYCHUYENDI,
                                LyDoChuyen = nk.LYDOCHUYEN,
                                GhiChu = nk.GHICHU,
                                DC = hk.SONHA + ' ' + hk.DUONG + ' ' + hk.PHUONGXA + ' ' + hk.QUANHUYEN + ' ' + hk.TINHTP
                            };
                try
                {
                    NhanKhausChuyen = new ObservableCollection<clsNhanKhauCatChuyen>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausChuyen.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    chuyenNHANKHAUViewSource.Source = NhanKhausChuyen;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexChuyenNhanKhau_TextChanged(null, null);
                    txtCurrentIndexChuyenNhanKhau.IsEnabled = true;
                }));
            }
            #endregion

            #region 9101
            else if (search == 9)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            where nk.SOHK == nk.SOHSCHUYENDI
                            select nk;
                try
                {
                    NhanKhausCat = new ObservableCollection<NHANKHAULUU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausCat.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    catNHANKHAUViewSource.Source = NhanKhausCat;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexCatNhanKhau_TextChanged(null, null);
                    txtCurrentIndexCatNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 10)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            where nk.SOHK == nk.SOHSCHUYENDI
                            where (nk.HO + ' ' + nk.CHULOT + ' ' + nk.TEN).Contains(strTim)
                            select nk;
                try
                {
                    NhanKhausCat = new ObservableCollection<NHANKHAULUU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausCat.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    catNHANKHAUViewSource.Source = NhanKhausCat;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexCatNhanKhau_TextChanged(null, null);
                    txtCurrentIndexCatNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 11)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            where nk.SOHK == nk.SOHSCHUYENDI
                            where nk.CMND.Contains(strTim)
                            select nk;
                try
                {
                    NhanKhausCat = new ObservableCollection<NHANKHAULUU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausCat.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    catNHANKHAUViewSource.Source = NhanKhausCat;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexCatNhanKhau_TextChanged(null, null);
                    txtCurrentIndexCatNhanKhau.IsEnabled = true;
                }));
            }
            else if (search == 12)
            {
                var query = from nk in hKDataContext.NHANKHAULUUs    //  << Query does
                            where nk.SOHK == nk.SOHSCHUYENDI
                            where nk.QHVOICHUHO == "Chủ hộ"
                            where (nk.HO + ' ' + nk.CHULOT + ' ' + nk.TEN).Contains(strTim)
                            select nk;
                try
                {
                    NhanKhausCat = new ObservableCollection<NHANKHAULUU>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = NhanKhausCat.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    catNHANKHAUViewSource.Source = NhanKhausCat;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexCatNhanKhau_TextChanged(null, null);
                    txtCurrentIndexCatNhanKhau.IsEnabled = true;
                }));
            }
            #endregion

            #region 1121
            else if (search == 13)
            {
                var query = from dt in hKDataContext.DOITUONGs    //  << Query does
                            select dt;
                try
                {
                    DoiTuongViPham = new ObservableCollection<DOITUONG>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = DoiTuongViPham.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    doiTUONGVIPHAMViewSource.Source = DoiTuongViPham;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexDoiTuongViPham_TextChanged(null, null);
                    txtCurrentIndexDoiTuongViPham.IsEnabled = true;
                }));
            }
            else if (search == 14)
            {
                var query = from dt in hKDataContext.DOITUONGs    //  << Query does
                            where (dt.HO + ' ' + dt.CHULOT + ' ' + dt.TEN).Contains(strTim)
                            select dt;
                try
                {
                    DoiTuongViPham = new ObservableCollection<DOITUONG>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = DoiTuongViPham.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    doiTUONGVIPHAMViewSource.Source = DoiTuongViPham;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexDoiTuongViPham_TextChanged(null, null);
                    txtCurrentIndexDoiTuongViPham.IsEnabled = true;
                }));
            }
            else if (search == 15)
            {
                var query = from dt in hKDataContext.DOITUONGs    //  << Query does
                            where dt.CMND.Contains(strTim)
                            select dt;
                try
                {
                    DoiTuongViPham = new ObservableCollection<DOITUONG>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = DoiTuongViPham.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    doiTUONGVIPHAMViewSource.Source = DoiTuongViPham;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexDoiTuongViPham_TextChanged(null, null);
                    txtCurrentIndexDoiTuongViPham.IsEnabled = true;
                }));
            }
            else if (search == 16)
            {
                var query = from dt in hKDataContext.DOITUONGs    //  << Query does
                            where dt.QHVOICHUHO == "Chủ hộ"
                            where (dt.HO + ' ' + dt.CHULOT + ' ' + dt.TEN).Contains(strTim)
                                                   select dt;
                try
                {
                    DoiTuongViPham = new ObservableCollection<DOITUONG>(query);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy máy chủ ! Vui lòng thay đổi chuỗi kết nối ... T__T", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                countItems = DoiTuongViPham.Count();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
                {
                    doiTUONGVIPHAMViewSource.Source = DoiTuongViPham;
                    txtItemCount.Text = "/" + countItems.ToString();
                    txtCurrentIndexDoiTuongViPham_TextChanged(null, null);
                    txtCurrentIndexDoiTuongViPham.IsEnabled = true;
                }));
            }
            #endregion
        }
        #endregion

        void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myGrid_11.Children.RemoveAt(4);
            if (NhanKhaus.Count == 0 && NhanKhausChuyen.Count == 0 && NhanKhausCat.Count == 0 && DoiTuongViPham.Count == 0)
                return;
            btnInChiTiet.IsEnabled = true;
        }

        #region nulll_2 nulll enable_1 enable_2
        private void nulll_2()
        {
            thongTinNHANKHAUViewSource.Source = null;
            chuyenNHANKHAUViewSource.Source = null;
            catNHANKHAUViewSource.Source = null;
            doiTUONGVIPHAMViewSource.Source = null;
        }

        private void nulll()
        {
            NhanKhaus = new ObservableCollection<CTHOKHAU>();
            NhanKhausChuyen = new ObservableCollection<clsNhanKhauCatChuyen>();
            NhanKhausCat = new ObservableCollection<NHANKHAULUU>();
            DoiTuongViPham = new ObservableCollection<DOITUONG>();
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
        #endregion

        #region Tab
        private void rbnThongTinNhanKhau_Checked(object sender, RoutedEventArgs e)
        {
            myGrid_4.Visibility = System.Windows.Visibility.Visible;
            myGrid_5.Visibility = System.Windows.Visibility.Hidden;
            myGrid_6.Visibility = System.Windows.Visibility.Hidden;
            myGrid_7.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexThongTinNhanKhau.Visibility = System.Windows.Visibility.Visible;
            txtCurrentIndexChuyenNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexCatNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexDoiTuongViPham.Visibility = System.Windows.Visibility.Hidden;
            if (thongTinNHANKHAUViewSource.Source == null)
            {
                txtCurrentIndexThongTinNhanKhau.IsEnabled = false;
                btnInChiTiet.IsEnabled = false;
                txtItemCount.Text = "/0";
                enable_1(false);
                enable_2(false);
            }
            else
            {
                txtCurrentIndexThongTinNhanKhau.IsEnabled = true;
                btnInChiTiet.IsEnabled = true;
                txtItemCount.Text = "/" + countItems.ToString();
                txtCurrentIndexThongTinNhanKhau_TextChanged(null, null);
            }
        }

        private void rbnChuyenNhanKhau_Checked(object sender, RoutedEventArgs e)
        {
            myGrid_4.Visibility = System.Windows.Visibility.Hidden;
            myGrid_5.Visibility = System.Windows.Visibility.Visible;
            myGrid_6.Visibility = System.Windows.Visibility.Hidden;
            myGrid_7.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexThongTinNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexChuyenNhanKhau.Visibility = System.Windows.Visibility.Visible;
            txtCurrentIndexCatNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexDoiTuongViPham.Visibility = System.Windows.Visibility.Hidden;
            if (chuyenNHANKHAUViewSource.Source == null)
            {
                txtCurrentIndexChuyenNhanKhau.IsEnabled = false;
                btnInChiTiet.IsEnabled = false;
                txtItemCount.Text = "/0";
                enable_1(false);
                enable_2(false);
            }
            else
            {
                txtCurrentIndexChuyenNhanKhau.IsEnabled = true;
                btnInChiTiet.IsEnabled = true;
                txtItemCount.Text = "/" + countItems.ToString();
                txtCurrentIndexChuyenNhanKhau_TextChanged(null, null);
            }
        }

        private void rbnCatNhanKhau_Checked(object sender, RoutedEventArgs e)
        {
            myGrid_4.Visibility = System.Windows.Visibility.Hidden;
            myGrid_5.Visibility = System.Windows.Visibility.Hidden;
            myGrid_6.Visibility = System.Windows.Visibility.Visible;
            myGrid_7.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexThongTinNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexChuyenNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexCatNhanKhau.Visibility = System.Windows.Visibility.Visible;
            txtCurrentIndexDoiTuongViPham.Visibility = System.Windows.Visibility.Hidden;
            if (catNHANKHAUViewSource.Source == null)
            {
                txtCurrentIndexCatNhanKhau.IsEnabled = false;
                btnInChiTiet.IsEnabled = false;
                txtItemCount.Text = "/0";
                enable_1(false);
                enable_2(false);
            }
            else
            {
                txtCurrentIndexCatNhanKhau.IsEnabled = true;
                btnInChiTiet.IsEnabled = true;
                txtItemCount.Text = "/" + countItems.ToString();
                txtCurrentIndexCatNhanKhau_TextChanged(null, null);
            }
        }

        private void rbnDoiTuongViPham_Checked(object sender, RoutedEventArgs e)
        {
            myGrid_4.Visibility = System.Windows.Visibility.Hidden;
            myGrid_5.Visibility = System.Windows.Visibility.Hidden;
            myGrid_6.Visibility = System.Windows.Visibility.Hidden;
            myGrid_7.Visibility = System.Windows.Visibility.Visible;
            txtCurrentIndexThongTinNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexChuyenNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexCatNhanKhau.Visibility = System.Windows.Visibility.Hidden;
            txtCurrentIndexDoiTuongViPham.Visibility = System.Windows.Visibility.Visible;
            if (doiTUONGVIPHAMViewSource.Source == null)
            {
                txtCurrentIndexDoiTuongViPham.IsEnabled = false;
                btnInChiTiet.IsEnabled = false;
                txtItemCount.Text = "/0";
                enable_1(false);
                enable_2(false);
            }
            else
            {
                txtCurrentIndexDoiTuongViPham.IsEnabled = true;
                btnInChiTiet.IsEnabled = true;
                txtItemCount.Text = "/" + countItems.ToString();
                txtCurrentIndexDoiTuongViPham_TextChanged(null, null);
            }
        }
        #endregion

        #region TextChanged
        private void txtCurrentIndexThongTinNhanKhau_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItems == 0 || thongTinNHANKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndexThongTinNhanKhau.Text, out index);
            if (!f)
                return;
            --index;
            dataGridThongTinNhanKhau.ScrollIntoView(NhanKhaus[index]);
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

        private void txtCurrentIndexChuyenNhanKhau_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItems == 0 || chuyenNHANKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndexChuyenNhanKhau.Text, out index);
            if (!f)
                return;
            --index;
            dataGridChuyenNhanKhau.ScrollIntoView(NhanKhausChuyen[index]);
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

        private void txtCurrentIndexDoiTuongViPham_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItems == 0 || doiTUONGVIPHAMViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndexDoiTuongViPham.Text, out index);
            if (!f)
                return;
            --index;
            dataGridDoiTuongViPham.ScrollIntoView(DoiTuongViPham[index]);
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

        private void txtCurrentIndexCatNhanKhau_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countItems == 0 || catNHANKHAUViewSource.Source == null)
                return;
            int index;
            bool f = int.TryParse(txtCurrentIndexCatNhanKhau.Text, out index);
            if (!f)
                return;
            --index;
            dataGridCatNhanKhau.ScrollIntoView(NhanKhausCat[index]);
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
        #endregion

        #region "Tìm tất cả"
        private void txtTimKiem_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (rbnTimTatCa_2.IsChecked == true)
                e.Handled = true;
        }

        private void rbnTimTatCa_2_Checked(object sender, RoutedEventArgs e)
        {
            txtTimKiem.Text = "Tìm tất cả";
            txtTimKiem.Focus();
        }

        private void rbnTimHoTen_Checked(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == "Tìm tất cả")
                txtTimKiem.Text = string.Empty;
        }

        private void rbnTimCMND_Checked(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == "Tìm tất cả")
                txtTimKiem.Text = string.Empty;
        }

        private void rbnTimChuHo_Checked(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == "Tìm tất cả")
                txtTimKiem.Text = string.Empty;
        }
        #endregion
    }
}
