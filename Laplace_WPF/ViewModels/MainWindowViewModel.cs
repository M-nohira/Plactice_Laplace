using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using System.Drawing;
using System;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;

namespace Laplace_WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _textbox1 = "7068";
        public string TextBox1
        {
            get { return _textbox1; }
            set { SetProperty(ref _textbox1, value); }
        }

        private BitmapSource _image;
        public BitmapSource Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private string dpi = "5";
        public string DPI
        {
            get { return dpi; }
            set { SetProperty(ref dpi, value); }
        }

        private string iterate = "1000";
        public string Iterate
        {
            get { return iterate; }
            set { SetProperty(ref iterate, value); }
        }

        private bool _checkedSub = true;
        public bool CheckedSub
        {
            get { return _checkedSub; }
            set { SetProperty(ref _checkedSub, value); }
        }

        private bool _checkedGauss;
        public bool CheckedGauss
        {
            get { return _checkedGauss; }
            set { SetProperty(ref _checkedGauss, value); }
        }

        private bool _checkedRapid = false;
        public bool CheckedRapid
        {
            get { return _checkedRapid; }
            set { SetProperty(ref _checkedRapid, value); }
        }

        private string _constRapid = "0";
        public string ConstRapid
        {
            get { return _constRapid; }
            set { SetProperty(ref _constRapid, value); }
        }

        private string iterateCnt = "0";
        public string IterateCnt
        {
            get { return iterateCnt; }
            set { SetProperty(ref iterateCnt, value); }
        }

        private string conv = "0";
        public string Conv
        {
            get { return conv; }
            set { SetProperty(ref conv, value); }
        }

        private ICommand _culc;
        private bool culcFlag;
        public ICommand Culc
        {
            get
            {
                if (_culc != null) return _culc;
                if (culcFlag == true) return _culc;
                culcFlag = true;
                _culc = new DelegateCommand(async () =>
                {
                    int dpi;
                    if (!int.TryParse(DPI, out dpi)) return;
                    int iterate;
                    if (!int.TryParse(Iterate, out iterate)) return;
                    int crapid;
                    if (!int.TryParse(ConstRapid, out crapid)) return;
                    double conv;
                    if (!double.TryParse(Conv, out conv)) return;

                    int num;
                    if (!int.TryParse(TextBox1.Substring(2, 2), out num)) return;
                    var con = new Data.Condition();
                    con.X_LMax = 50;
                    con.Y_LMax = 50;
                    con.X_LRec = 14 + (num / 10);
                    con.y_LRec = 4 + (num % 10);

                    await Task.Run(() =>
                    {
                        double[,] data = null;


                        data = (CheckedSub == true) ? Model.Laplace.Subsititution(con, dpi, ref iterate, null, conv) : (CheckedGauss == true) ? Model.Laplace.Gauss(con, dpi, ref iterate, null, conv) : null;
                        //data = (CheckedSub == true) ? Model.Laplace.Subsititution(con, dpi, iterate / 10 , data,conv) : (CheckedGauss == true) ? Model.Laplace.Gauss(con, dpi, iterate / 10, data,conv) : null;
                        Bitmap bitmap = Model.Images.CreateImage(con, dpi, data);


                        using (Stream st = new MemoryStream())
                        {
                            bitmap.Save(st, ImageFormat.Png);
                            st.Seek(0, SeekOrigin.Begin);
                            Image = BitmapFrame.Create(st, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        }

                        IterateCnt = iterate.ToString();
                    });

                });
                culcFlag = false;
                return _culc;
            }
        }

        public MainWindowViewModel()
        {

        }
    }
}
