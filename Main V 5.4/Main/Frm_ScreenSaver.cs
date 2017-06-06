using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Main
{
    public partial class Frm_ScreenSaver : Form
    {


        #region Public Vars
        String strDate = string.Empty;//To recognize date in timer
        String strTime = string.Empty;//To recognize Time in timer
        Label DateScreenLbl = new Label();
        Label TimeScreenLbl = new Label();
        bool dataformat = false;
        bool timeformat = false;
        int i = 0;
        FileInfo[] imageName;
        string _ScreenSaverImagesPath = Application.StartupPath + "\\Files\\ScreenSaver\\Imgs\\";
        string _ScreenSaverVideosPath = Application.StartupPath + "\\Files\\ScreenSaver\\Videos\\";
        string xmlfile = Application.StartupPath + "\\Config.xml";
        bool VideoState = false;
        List<Dictionary<long, string>> _ScreenLabel;
        #endregion

        #region Public Constructor
        public Frm_ScreenSaver()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.ContainerControl |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor
                          , true);


            XDocument xDocument = XDocument.Load(xmlfile);

            var ScreenSaver = from d in xDocument.Descendants("ScreenSaver")//Lê tudo o que e button
                              select new
                              {
                                  Name = (string)d.Element("Name").Value,
                                  Video = (string)d.Element("Video").Value,
                                  VideoUrl = (string)d.Element("VideoUrl").Value,

                              };

            var ScreenLabel = from d in xDocument.Descendants("ScreenLabel")//Lê tudo o que e button
                              select new
                              {
                                  id = long.Parse(d.Attribute("id").Value),
                                  Name = (string)d.Element("Name")
                              };

            if (_ScreenLabel == null) _ScreenLabel = new List<Dictionary<long, string>>();
          
            foreach (var x in ScreenLabel)
            {
                Dictionary<long, string> ScreenLabelID = new Dictionary<long, string>();
                ScreenLabelID.Add(x.id, x.Name);
                _ScreenLabel.Add(ScreenLabelID);

            }
            foreach (var x in ScreenSaver)
            {

                if (x.Video.Equals("On"))
                {
                    VideoState = true;

                    axWindowsMediaPlayer1.BringToFront();
                    axWindowsMediaPlayer1.URL = _ScreenSaverVideosPath + x.VideoUrl;// diretorio completo
                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                    axWindowsMediaPlayer1.PlayStateChange += axWindowsMediaPlayer1_PlayStateChange;
                    timerTime.Stop();
                    timer_Day.Stop();
                    timerImage.Stop();

                }
                else if (x.Video.Equals("Off"))
                {
                    LoadImageScreenSaver();
                    VideoState = false;
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    axWindowsMediaPlayer1.Visible = false;

                    if (Directory.GetFiles(_ScreenSaverImagesPath).Length != 0)
                    {
                        this.pictureBox1.BackgroundImage = Image.FromFile(_ScreenSaverImagesPath + imageName[0]);

                        timerImage.Start();

                    }
                    else
                        pictureBox1.BackColor = Color.Black;

     
                    timerImage.Start();
                    timerTime.Start();
                    timer_Day.Start();
                    timerImage.Start();
                    DateLblRead();


                    axWindowsMediaPlayer1.SendToBack();
                    pictureBox1.SendToBack();
                    DateScreenLbl.BringToFront();
                    TimeScreenLbl.BringToFront();

                }

            }
            DateScreenLbl.Parent = pictureBox1;
            TimeScreenLbl.Parent = pictureBox1;
        }

     

        void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            AxWMPLib.AxWindowsMediaPlayer WMP = default(AxWMPLib.AxWindowsMediaPlayer);
            WMP = (AxWMPLib.AxWindowsMediaPlayer)sender;

            if (WMP.playState == WMPLib.WMPPlayState.wmppsStopped)
                WMP.Ctlcontrols.play();
        }
        #endregion

        #region Lbls
        public void DateLblRead()
        {
            for (int i = 0; i < _ScreenLabel.Count; i++)
            {
                Dictionary<long, string> ScreenLabel = _ScreenLabel[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                Label _Label = new Label() { AutoSize = true};

                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlfile);

                XmlNodeList xnList = xmld.SelectNodes("/Kiosk/ScreenLabel");
                foreach (XmlNode xn in xnList)
                {

                    XmlElement xmle = (XmlElement)xn;//  
                    if (xmle.GetAttribute("id") == ScreenLabel.First().Key.ToString())//ID  
                    {
                        if (xmle.SelectSingleNode("Name").InnerText.Equals("DateScreenLbl"))
                        {
                            strDate = xmle.SelectSingleNode("DataFormat").InnerText;
                            DateScreenLbl = _Label;
                        }

                        if (xmle.SelectSingleNode("Name").InnerText.Equals("TimeScreenLbl"))
                        {
                            strTime = xmle.SelectSingleNode("DataFormat").InnerText;
                            TimeScreenLbl = _Label;
                        }
                   
                        _Label.FlatStyle = xmle.SelectSingleNode("FlatStyle").InnerText.ToFlatStyle();
                      //  _Label.Font = xmle.SelectSingleNode("Font").InnerText.ToFont();
                        _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value),
                            float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value));


                        #region FontStyle

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Regular);
                        }


                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold);
                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {

                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic);
                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Strikeout);
                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic);
                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Strikeout);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline);
                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline | FontStyle.Strikeout);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Strikeout);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);

                        }


                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                        }


                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                        }

                        if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                        {
                            _Label.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                        }
                        #endregion



                        _Label.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();
                        _Label.ForeColor = xmle.SelectSingleNode("ForeColor").InnerText.ToColor();
                        string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                        string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                        _Label.Location = new Point(int.Parse(X), int.Parse(Y));
                        string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                        string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                        _Label.Size = new Size(int.Parse(width), int.Parse(height));
                        ExtensionMethods.AssignVisibleControl(_Label, xmle.SelectSingleNode("Visible").InnerText);
                    }
                }
                this.Controls.Add(_Label);
            }
        }
        #endregion
        
        #region Functions
        public void LoadImageScreenSaver()
        {

            if (_ScreenSaverImagesPath != null)
            {
                DirectoryInfo _di = new DirectoryInfo(_ScreenSaverImagesPath);

                //image files
                FileInfo[] _diar1 = _di.GetFiles("*.jpg");
                FileInfo[] _diar2 = _di.GetFiles("*.bmp");
                FileInfo[] _diar3 = _di.GetFiles("*.png");
                FileInfo[] _diar4 = _di.GetFiles("*.gif");


                var _diarList = new List<FileInfo>();
                _diarList.AddRange(_diar1);
                _diarList.AddRange(_diar2);
                _diarList.AddRange(_diar3);
                _diarList.AddRange(_diar4);

                FileInfo[] diar = _diarList.ToArray();


                FileInfo _dra = null;


                foreach (FileInfo _dra_loopVariable in _diarList)
                {

                    _dra = _dra_loopVariable;
                    imageName = diar;
                }


            }
        }
        #endregion

        #region Event Handlers

        private void ScreenSaver_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Show();
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }


        private void timerImage_Tick(object sender, EventArgs e)
        {
            i++;
      
            if (i < imageName.Count())
            {
                //Effects.Animate(pictureBox1, Effects.Effect.Slide, 150, 180);
                Application.DoEvents();
                this.pictureBox1.BackgroundImage = Image.FromFile(_ScreenSaverImagesPath + imageName[i]);
                //Effects.Animate(pictureBox1, Effects.Effect.Slide, 150, 0);
            }
            else
            {
                //Effects.Animate(this, Effects.Effect.Slide, 150, 180);
                Application.DoEvents();
                i = 0;
                this.pictureBox1.BackgroundImage = Image.FromFile(_ScreenSaverImagesPath + imageName[i]);
                // Effects.Animate(this, Effects.Effect.Slide, 150, 0);
            }
        }


        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            this.Close();
            Cursor.Show();
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            DateTime Time = DateTime.Now;
            this.TimeScreenLbl.Text = String.Format(strTime, Time);
        }

        private void timer_Day_Tick(object sender, EventArgs e)
        {
            DateTime Date = DateTime.Now;
            this.DateScreenLbl.Text = String.Format(strDate, Date);
        }


        #endregion

    }
}
