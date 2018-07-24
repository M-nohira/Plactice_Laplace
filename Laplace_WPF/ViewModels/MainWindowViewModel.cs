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
        private string _title = "Laplace Culcurater";
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

        private bool _checkedRapid = true;
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

        private string culctime;
        public string CulcTime
        {
            get { return culctime; }
            set
            {

                int time = 0;
                if (!int.TryParse(value, out time)) SetProperty(ref culctime, value + "ms");
                var span = TimeSpan.FromMilliseconds((double)time);
                string msg = $"{span.Hours} H {span.Minutes}{span.Seconds} S {span.Milliseconds} mS";
                SetProperty(ref culctime, msg);
            }
        }

        public Bitmap _Bitmap { get; set; }
        public Data.Result result { get; set; }


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
                    double crapid;
                    if (!double.TryParse(ConstRapid, out crapid)) return;
                    if (CheckedRapid == true) crapid = 0;
                    double conv;
                    if (!double.TryParse(Conv, out conv)) return;

                    int num;
                    if (!int.TryParse(TextBox1.Substring(2, 2), out num)) return;
                    var con = new Data.Condition();
                    con.X_LMax = 50;
                    con.Y_LMax = 50;
                    con.X_LRec = 14 + 2 * (num / 10);
                    con.y_LRec = 4 + 4 * (num % 10);

                    await Task.Run(() =>
                    {
                        Data.Result res = new Data.Result();

                        var sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        res = (CheckedSub == true) ? Model.Laplace.Subsititution(con, dpi, ref iterate, crapid, null, conv) : (CheckedGauss == true) ? Model.Laplace.Gauss(con, dpi, ref iterate, crapid, null, conv) : null;
                        //data = (CheckedSub == true) ? Model.Laplace.Subsititution(con, dpi, iterate / 10 , data,conv) : (CheckedGauss == true) ? Model.Laplace.Gauss(con, dpi, iterate / 10, data,conv) : null;
                        sw.Stop();
                        CulcTime = sw.ElapsedMilliseconds.ToString();
                        result = res;


                        if (_Bitmap != null)
                            _Bitmap.Dispose();


                        using (Bitmap bitmap = Model.Images.CreateImage(con, dpi, res.Grid))
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

        private ICommand saveError;
        public ICommand SaveError
        {
            get
            {
                if (saveError != null) return SaveError;
                saveError = new DelegateCommand(() =>
                {
                    using (StreamWriter sw = new StreamWriter($@"{Environment.CurrentDirectory}\Errors.csv",false))
                    {
                        string msg = Model.file.GetCSV(result.Error);
                        sw.Write(msg);
                        sw.Flush();
                    }
                });
                return saveError;
            }
        }


        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save != null) return save;
                save = new DelegateCommand(() =>
                 {
                     _Bitmap.Save("Laplace.png");
                 });
                return save;
            }
        }

        public MainWindowViewModel()
        {

        }
    }
}
