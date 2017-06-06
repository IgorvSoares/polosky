using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using UserInactivityMonitoring;
using System.Timers;
using System.Xml.Linq;
using System.Xml;
using System.Runtime.InteropServices;


namespace Main
{

    public partial class Main : Form
    {
        #region Activate and Deactivate TaskBar
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        void HideTaskBar()
        {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_HIDE);
        }

        void ShowTaskBar()
        {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_SHOW);
        }

        #endregion

        #region Public Vars
        String strDate = string.Empty;
        Label LblDate = new Label() { AutoSize = true};

        string xmlfile = Application.StartupPath + "\\Config.xml";
        bool DynamicFormOn_Off = false;
        bool ScreenSaverOn_Off = false;

        List<Dictionary<long, string>> _ButtonApplications;
        List<Dictionary<long, string>> _ButtonModule;
        List<Dictionary<long, string>> _DateLabel;
        List<Dictionary<long, string>> _PanelList;
        List<Dictionary<long, string>> _LabelList;
        List<Dictionary<long, string>> _PicturesList;

        string _BackgroundImagePath = Application.StartupPath + "\\Files\\MainImgs\\";
        string _ImageFilesFolder = Application.StartupPath + "\\Files\\";
        IInactivityMonitor inactivityMonitor = null;
        Frm_ScreenSaver _frm_ScreenSaver;
       // Rectangle Resolution = Screen.PrimaryScreen.WorkingArea;



    //    bool AcMod = false;

        #endregion

        #region  Public Constructors
            public Main()
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



                inactivityMonitor = MonitorCreator.CreateInstance(MonitorType.LastInputMonitor);
                inactivityMonitor.SynchronizingObject = this;
                inactivityMonitor.MonitorKeyboardEvents = false;
                inactivityMonitor.MonitorMouseEvents = true;
                inactivityMonitor.Interval = 0;
                inactivityMonitor.Elapsed += TimeElapsed;
                inactivityMonitor.Reactivated += Reactivated;
                inactivityMonitor.Enabled = true;

                pictureBoxLogout.Parent = Pic_Background;

           

            }
        #endregion

            #region MouseDetetion
            private void Reactivated(object sender, EventArgs e)
            {
                if (_frm_ScreenSaver != null)
                {
                    _frm_ScreenSaver.Visible = false;
                    _frm_ScreenSaver.Close();
                }

                _frm_ScreenSaver = null;
                timer_ScreenSaver.Stop();

            }

            private void TimeElapsed(object sender, ElapsedEventArgs e)
            {  
                timer_ScreenSaver.Start();

            }
            #endregion

            #region ButtonApplication

            public void ButtonRead()
            {
                for (int i = 0; i < _ButtonApplications.Count; i++)
                {
                    Dictionary<long, string> ButtonsDictionary = _ButtonApplications[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                    ButtonApplication _buttonApp = new ButtonApplication();
                    RoundButton bttn = new RoundButton();

                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk/Controls/ButtonApplication");
                    foreach (XmlNode xn in xnList)
                    {

                        XmlElement xmle = (XmlElement)xn;//  
                        if (xmle.GetAttribute("id") == ButtonsDictionary.First().Key.ToString())//ID  
                        {
                            _buttonApp.id = xmle.GetAttribute("id");
                            if (xmle.SelectSingleNode("Shape").InnerText.Equals("Circle"))
                                _buttonApp = bttn;
                            _buttonApp.Text = xmle.SelectSingleNode("Text").InnerText;

                            //_buttonApp.Font = xmle.SelectSingleNode("Font").InnerText.ToFont();//Bug
                            _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value),
                               float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value));



                            #region FontStyle

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Regular);
                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {

                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Strikeout);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonApp.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }
                            #endregion



                            _buttonApp.TextAlign = xmle.SelectSingleNode("TextAlign").InnerText.ToAlign();
                            string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                            string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                            _buttonApp.Size = new Size(int.Parse(width), int.Parse(height));
                            string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                            string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                            _buttonApp.Location = new Point(int.Parse(X), int.Parse(Y));
                            //if (!xmle.SelectSingleNode("Image").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("Image").InnerText))
                            //    _buttonApp.Image = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("Image").InnerText);
                            //else
                            //    _buttonApp.Image = null;
                            //_buttonApp.ImageAlign = xmle.SelectSingleNode("ImageAlign").InnerText.ToAlign();
                            if (!xmle.SelectSingleNode("BackgroundImage").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText))
                                _buttonApp.BackgroundImage = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText);
                            else
                                _buttonApp.BackgroundImage = null;
                            _buttonApp.BackgroundImageLayout = xmle.SelectSingleNode("BackgroundImageLayout").InnerText.ToBackGroundImageLayout();
                            _buttonApp.Dock = xmle.SelectSingleNode("Dock").InnerText.ToDockStyle();
                            _buttonApp.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();
                            _buttonApp.ForeColor = xmle.SelectSingleNode("ForeColor").InnerText.ToColor();
                            _buttonApp.FlatStyle = xmle.SelectSingleNode("FlatStyle").InnerText.ToFlatStyle();
                            _buttonApp.FlatAppearance.BorderColor = xmle.SelectSingleNode("FlatAppearance").Attributes["BorderColor"].Value.ToColor();
                            _buttonApp.FlatAppearance.BorderSize = int.Parse(xmle.SelectSingleNode("FlatAppearance").Attributes["BorderSize"].Value);
                            _buttonApp.FlatAppearance.MouseDownBackColor = xmle.SelectSingleNode("FlatAppearance").Attributes["MouseDownBackColor"].Value.ToColor();
                            _buttonApp.FlatAppearance.MouseOverBackColor = xmle.SelectSingleNode("FlatAppearance").Attributes["MouseOverBackColor"].Value.ToColor();
                            _buttonApp.Action = xmle.SelectSingleNode("Action").InnerText;
                        }

                    }
                    this.Controls.Add(_buttonApp);
                    _buttonApp.Parent = Pic_Background;
                    _buttonApp.Click += Click_ButtonAplication;
                }
            }
            #endregion

            #region ButtonModule

            public void ButtonReadModule()
            {
                for (int i = 0; i < _ButtonModule.Count; i++)
                {
                    Dictionary<long, string> ButtonsModuleDictionary = _ButtonModule[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                    ButtonModule _buttonModule = new ButtonModule();
                    RoundButtonModule bttn = new RoundButtonModule();

                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk/Controls/ButtonModule");
                    foreach (XmlNode xn in xnList)
                    {

                        XmlElement xmle = (XmlElement)xn;//  
                        if (xmle.GetAttribute("id") == ButtonsModuleDictionary.First().Key.ToString())//ID  
                        {
                            _buttonModule.id = xmle.GetAttribute("id");
                            if (xmle.SelectSingleNode("Shape").InnerText.Equals("Circle"))
                                _buttonModule = bttn; 
                            _buttonModule.Text = xmle.SelectSingleNode("Text").InnerText;
                           // _buttonModule.Font = xmle.SelectSingleNode("Font").InnerText.ToFont();

                            _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value),
                             float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value));

                            #region FontStyle

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Regular);
                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {

                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Strikeout);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _buttonModule.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }
                            #endregion


                            _buttonModule.TextAlign = xmle.SelectSingleNode("TextAlign").InnerText.ToAlign();
                            string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                            string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                            _buttonModule.Size = new Size(int.Parse(width), int.Parse(height));
                            string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                            string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                            _buttonModule.Location = new Point(int.Parse(X), int.Parse(Y));
                            if (!xmle.SelectSingleNode("Image").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("Image").InnerText))
                                _buttonModule.Image = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("Image").InnerText);
                            _buttonModule.ImageAlign = xmle.SelectSingleNode("ImageAlign").InnerText.ToAlign();
                            if (!xmle.SelectSingleNode("BackgroundImage").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText))
                                _buttonModule.BackgroundImage = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText);
                            _buttonModule.BackgroundImageLayout = xmle.SelectSingleNode("BackgroundImageLayout").InnerText.ToBackGroundImageLayout();
                            _buttonModule.Dock = xmle.SelectSingleNode("Dock").InnerText.ToDockStyle();
                            _buttonModule.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();
                            _buttonModule.ForeColor = xmle.SelectSingleNode("ForeColor").InnerText.ToColor();
                            _buttonModule.FlatStyle = xmle.SelectSingleNode("FlatStyle").InnerText.ToFlatStyle();
                            _buttonModule.FlatAppearance.BorderColor = xmle.SelectSingleNode("FlatAppearance").Attributes["BorderColor"].Value.ToColor();
                            _buttonModule.FlatAppearance.BorderSize = int.Parse(xmle.SelectSingleNode("FlatAppearance").Attributes["BorderSize"].Value);
                            _buttonModule.FlatAppearance.MouseDownBackColor = xmle.SelectSingleNode("FlatAppearance").Attributes["MouseDownBackColor"].Value.ToColor();
                            _buttonModule.FlatAppearance.MouseOverBackColor = xmle.SelectSingleNode("FlatAppearance").Attributes["MouseOverBackColor"].Value.ToColor();
                            _buttonModule.Module = xmle.SelectSingleNode("Module").InnerText;
                        }

                      

                    }
                    this.Controls.Add(_buttonModule);
                    _buttonModule.Parent = Pic_Background;
                    _buttonModule.Click += Click_ButtonModule;
                }
               
            }
            #endregion

        /// <summary>
        /// Label criada pelo administrador
        /// </summary>
            #region Lbls
            public void Labels()
            {
                for (int i = 0; i < _LabelList.Count; i++)
                {
                    Dictionary<long, string> LabelDictionary = _LabelList[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                    Label _Lbl = new Label() { AutoSize = true};

                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk/Controls/Label");
                    foreach (XmlNode xn in xnList)
                    {

                        XmlElement xmle = (XmlElement)xn;//  
                        if (xmle.GetAttribute("id") == LabelDictionary.First().Key.ToString())//ID  
                        {
                            _Lbl.Name = xmle.SelectSingleNode("Name").InnerText;
                            _Lbl.Text = xmle.SelectSingleNode("Text").InnerText;
                            _Lbl.FlatStyle = xmle.SelectSingleNode("FlatStyle").InnerText.ToFlatStyle();
                           // _Lbl.Font = xmle.SelectSingleNode("Font").InnerText.ToFont();

                            _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value),
                             float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value));

                            #region FontStyle

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Regular);
                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {

                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Strikeout);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                _Lbl.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }
                            #endregion

                            _Lbl.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();
                            _Lbl.ForeColor = xmle.SelectSingleNode("ForeColor").InnerText.ToColor();
                            string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                            string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                            _Lbl.Location = new Point(int.Parse(X), int.Parse(Y));
                            string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                            string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                            _Lbl.Size = new Size(int.Parse(width), int.Parse(height));
                        
                        }
                    }
                    this.Controls.Add(_Lbl);
                    _Lbl.Parent = Pic_Background;
                }
            }
            #endregion
            
        /// <summary>
        /// Label com as horas no ecra inicial
        /// </summary>
            #region DateLabel
            public void DateLbl()
            {
                for (int i = 0; i < _DateLabel.Count; i++)
                {
                    Dictionary<long, string> DateLabelDictionary = _DateLabel[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk/Controls/DateLabel");
                    foreach (XmlNode xn in xnList)
                    {

                        XmlElement xmle = (XmlElement)xn;//  
                        if (xmle.GetAttribute("id") == DateLabelDictionary.First().Key.ToString())//ID  
                        {                        
                            LblDate.Name = xmle.SelectSingleNode("Name").InnerText;
                            strDate = xmle.SelectSingleNode("DataFormat").InnerText;
                            LblDate.FlatStyle = xmle.SelectSingleNode("FlatStyle").InnerText.ToFlatStyle();
                            //LblDate.Font = xmle.SelectSingleNode("Font").InnerText.ToFont();
                            LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value),
                             float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value));

                            #region FontStyle

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Regular);
                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {

                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Strikeout);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline);
                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "False")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);

                            }


                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "False" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }

                            if (xmle.SelectSingleNode("Font").Attributes["Bold"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Italic"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Underline"].Value == "True" && xmle.SelectSingleNode("Font").Attributes["Strikeout"].Value == "True")
                            {
                                LblDate.Font = new Font(ExtensionMethods.ToFontFamily(xmle.SelectSingleNode("Font").Attributes["FontFamily"].Value), float.Parse(xmle.SelectSingleNode("Font").Attributes["FontSize"].Value), FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout);

                            }
                            #endregion
                        

                            LblDate.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();
                            LblDate.ForeColor = xmle.SelectSingleNode("ForeColor").InnerText.ToColor();
                            string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                            string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                            LblDate.Location = new Point(int.Parse(X), int.Parse(Y));
                            string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                            string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                            LblDate.Size = new Size(int.Parse(width), int.Parse(height));
                           // ExtensionMethods.AssignVisibleControl(LblDate, xmle.SelectSingleNode("Visible").InnerText);
                        }
                    }
                    this.Controls.Add(LblDate);
                    LblDate.Parent = Pic_Background;
                }
                
              
            }
            #endregion

            #region Picture

            public void PictureRead()
            {
                for (int i = 0; i < _PicturesList.Count; i++)
                {
                    Dictionary<long, string> PictureDictionary = _PicturesList[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                    PictureBox _Picture = new PictureBox() ;

                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk/Controls/PictureBox");
                    foreach (XmlNode xn in xnList)
                    {

                        XmlElement xmle = (XmlElement)xn;//  
                        if (xmle.GetAttribute("id") == PictureDictionary.First().Key.ToString())//ID  
                        {
                            _Picture.Name = xmle.SelectSingleNode("Name").InnerText;
                         
                            _Picture.Dock = xmle.SelectSingleNode("Dock").InnerText.ToDockStyle();
                            _Picture.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();

                            string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                            string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                            _Picture.Location = new Point(int.Parse(X), int.Parse(Y));
                            string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                            string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                            _Picture.Size = new Size(int.Parse(width), int.Parse(height));

                            if (!xmle.SelectSingleNode("BackgroundImage").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText))
                                _Picture.BackgroundImage = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText);
                            else
                                _Picture.BackgroundImage = null;
                            _Picture.BackgroundImageLayout = xmle.SelectSingleNode("BackgroundImageLayout").InnerText.ToBackGroundImageLayout();

                            if (!xmle.SelectSingleNode("Image").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("Image").InnerText))
                                _Picture.Image = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("Image").InnerText);
                            else
                                _Picture.Image = null;

                            _Picture.SizeMode = xmle.SelectSingleNode("SizeMode").InnerText.ToSizeMode();
                        }
                    }
                    this.Controls.Add(_Picture);
                    _Picture.Parent = Pic_Background;
                }
           
            }
         
            #endregion

            #region Panel


            public void PanelRead()
            {
                for (int i = 0; i < _PanelList.Count; i++)
                {
                    Dictionary<long, string> PanelDictionary = _PanelList[i];//obeter o nome e o id do butao apartir do item selecionado na combo

                    Panel _Panel = new Panel();

                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk/Controls/NewPanel");
                    foreach (XmlNode xn in xnList)
                    {

                        XmlElement xmle = (XmlElement)xn;//  
                        if (xmle.GetAttribute("id") == PanelDictionary.First().Key.ToString())//ID  
                        {
                            _Panel.Name = xmle.SelectSingleNode("Name").InnerText;
                            _Panel.BackgroundImageLayout = xmle.SelectSingleNode("BackgroundImageLayout").InnerText.ToBackGroundImageLayout();
                            _Panel.Dock = xmle.SelectSingleNode("Dock").InnerText.ToDockStyle();
                            _Panel.BackColor = xmle.SelectSingleNode("BackColor").InnerText.ToColor();
                       
                            string X = xmle.SelectSingleNode("Location").Attributes["X"].Value;
                            string Y = xmle.SelectSingleNode("Location").Attributes["Y"].Value;
                            _Panel.Location = new Point(int.Parse(X), int.Parse(Y));
                            string width = xmle.SelectSingleNode("Size").Attributes["Width"].Value;
                            string height = xmle.SelectSingleNode("Size").Attributes["Height"].Value;
                            _Panel.Size = new Size(int.Parse(width), int.Parse(height));

                            if (!xmle.SelectSingleNode("BackgroundImageLayout").InnerText.Equals("") && File.Exists(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText))
                                _Panel.BackgroundImage = Image.FromFile(_ImageFilesFolder + xmle.SelectSingleNode("BackgroundImage").InnerText);
                            else
                                _Panel.BackgroundImage = null;
                        }
                    }
                    this.Controls.Add(_Panel);
                    _Panel.Parent = Pic_Background;
                }
                
            }

            #endregion


     
            private void timerDate_Tick(object sender, EventArgs e)
            {
                DateTime dateTime = DateTime.Now;
                LblDate.Text = string.Format(strDate, dateTime);
            }


            void Click_ButtonAplication(object sender, EventArgs e)
            {
                Process.Start(((ButtonApplication)sender).Action);// Diretorio Completo só executaveis de aplicaçoes portateis  ou apps da pasta system32
            }

            string arg;
            void Click_ButtonModule(object sender, EventArgs e)
            {                
                    XmlDocument xmld = new XmlDocument();
                    xmld.Load(xmlfile);

                    XmlNodeList xnList = xmld.SelectNodes("/Kiosk");
                    foreach (XmlNode xn in xnList)
                    {
                        XmlNode anode = xn.SelectSingleNode("Controls");
                        if (anode != null)
                        {
                            XmlNodeList CNodes = xn.SelectNodes("Controls/ButtonModule");


                            foreach (XmlNode node in CNodes)
                            {
                                XmlElement xmle = (XmlElement)node;//  



                                if (xmle.GetAttribute("id") == ((ButtonModule)sender).id) //ID  
                                {
                                    XmlNodeList Nodelist = xmle.SelectNodes("Config");
                                    foreach (XmlNode nd in Nodelist)
                                        arg = string.Format(@"<Config>{0}</Config>", nd.InnerXml);
                                }

                            }
                        }
                    }
                    string TheFolder = Path.GetDirectoryName(string.Format("{0}\\Modules\\{1}", Application.StartupPath, ((ButtonModule)sender).Module));

                 /*   StreamWriter teste = new StreamWriter(TheFolder + "\\Config.xml");
                    teste.Write(XDocument.Parse(arg));
                    teste.Close();
                */
                    ProcessStartInfo startInfo = new ProcessStartInfo() 
                    { 
                        FileName = string.Format("{0}\\Modules\\{1}", Application.StartupPath, ((ButtonModule)sender).Module), 
                        Arguments = String.Format("\"{0}\" {1}", xmlfile, ((ButtonModule)sender).id) 
                    };

                    Process.Start(startInfo);

            }


            private void Main_Load(object sender, EventArgs e)
            {
              
              // Application.Run(new SplashScreen());
                SplashScreen SpSc = new SplashScreen();
                SpSc.Close();
           
              // HideTaskBar();
               XDocument xDocument = XDocument.Load(xmlfile);

               var Form = from d in xDocument.Descendants("Form")//Lê tudo o que e button
                          select new
                          {
                              Name = (string)d.Element("Name").Value,
                              Dynamic = (string)d.Element("Dynamic").Value,
                              Image = (string)d.Element("Image").Value,
                              SizeMode = (string)d.Element("SizeMode").Value,
                              BackColor = (string)d.Element("BackColor").Value,
                              ScreenSaverState = (string)d.Element("ScreenSaverState").Value,
                              ScreenSaverTime = int.Parse(d.Element("ScreenSaverTime").Value),
                              RandomDynamic = d.Element("RandomDynamic").Value,
                          };

               var ButtonApplications = from d in xDocument.Descendants("ButtonApplication")//Lê tudo o que e button
                                        select new
                                        {
                                            id = long.Parse(d.Attribute("id").Value),
                                            Name = (string)d.Element("Name")
                                        };

               var ButtonModule = from d in xDocument.Descendants("ButtonModule")//Lê tudo o que e button
                                  select new
                                  {
                                      id = long.Parse(d.Attribute("id").Value),
                                      Name = (string)d.Element("Name")
                                  };


               var DateLabel = from d in xDocument.Descendants("DateLabel")//Lê tudo o que e button
                               select new
                               {
                                   id = long.Parse(d.Attribute("id").Value),
                                   Name = (string)d.Element("Name")
                               };

               var Label = from d in xDocument.Descendants("Label")//Lê tudo o que e button
                           select new
                           {
                               id = long.Parse(d.Attribute("id").Value),
                               Name = (string)d.Element("Name")
                           };


               var Panel = from d in xDocument.Descendants("NewPanel")//Lê tudo o que e button
                           select new
                           {
                               id = long.Parse(d.Attribute("id").Value),
                               Name = (string)d.Element("Name")
                           };

               var Picture = from d in xDocument.Descendants("PictureBox")//Lê tudo o que e button
                             select new
                             {
                                 id = long.Parse(d.Attribute("id").Value),
                                 Name = (string)d.Element("Name")
                             };

               if (_ButtonApplications == null) _ButtonApplications = new List<Dictionary<long, string>>();
               if (_ButtonModule == null) _ButtonModule = new List<Dictionary<long, string>>();
               if (_DateLabel == null) _DateLabel = new List<Dictionary<long, string>>();
               if (_LabelList == null) _LabelList = new List<Dictionary<long, string>>();
               if (_PanelList == null) _PanelList = new List<Dictionary<long, string>>();
               if (_PicturesList == null) _PicturesList = new List<Dictionary<long, string>>();

               foreach (var x in Form)
               {
                   this.Name = x.Name;

                   if (x.Dynamic.Equals("On"))
                   {
                       DynamicFormOn_Off = true;
                       timerDynamicForm.Start();
                   }
                   else
                       DynamicFormOn_Off = false;

                   if (x.RandomDynamic.Equals("On"))
                       DynamicFormRandom = true;
                   else
                       DynamicFormRandom = false;

                   if (!x.Image.Equals("") && File.Exists(_BackgroundImagePath + x.Image))
                       Pic_Background.Image = Image.FromFile(_BackgroundImagePath + x.Image);
                   else
                       Pic_Background.Image = null;


                   if (x.SizeMode.Equals("Normal"))
                  Pic_Background.SizeMode = PictureBoxSizeMode.Normal;
                   if (x.SizeMode.Equals("AutoSize"))
                       Pic_Background.SizeMode = PictureBoxSizeMode.AutoSize;
                   if (x.SizeMode.Equals("StretchImage"))
                       Pic_Background.SizeMode = PictureBoxSizeMode.StretchImage;
                   if (x.SizeMode.Equals("CenterImage"))
                       Pic_Background.SizeMode = PictureBoxSizeMode.CenterImage;
                   if (x.SizeMode.Equals("Zoom"))
                       Pic_Background.SizeMode = PictureBoxSizeMode.Zoom;
                


                  Pic_Background.BackColor = x.BackColor.ToColor();


                   if (x.ScreenSaverState.Equals("On"))
                   {
                       ScreenSaverOn_Off = true;//
                      // inactivityMonitor.Enabled = true;
                   }
                   else if (x.ScreenSaverState.Equals("Off"))
                   {
                       ScreenSaverOn_Off = false;
                       inactivityMonitor.Enabled = false;
                   }

                   timer_ScreenSaver.Interval = x.ScreenSaverTime * 1000;
               }

               foreach (var x in ButtonApplications)
               {
                   Dictionary<long, string> ButtonsApplicationID = new Dictionary<long, string>();
                   ButtonsApplicationID.Add(x.id, x.Name);
                   _ButtonApplications.Add(ButtonsApplicationID);

               }
               foreach (var x in ButtonModule)
               {
                   Dictionary<long, string> ButtonsModuleID = new Dictionary<long, string>();
                   ButtonsModuleID.Add(x.id, x.Name);
                   _ButtonModule.Add(ButtonsModuleID);

               }
               foreach (var x in DateLabel)
               {
                   Dictionary<long, string> DateLabelID = new Dictionary<long, string>();
                   DateLabelID.Add(x.id, x.Name);
                   _DateLabel.Add(DateLabelID);

               }
               foreach (var x in Label)
               {
                   Dictionary<long, string> LabelID = new Dictionary<long, string>();
                   LabelID.Add(x.id, x.Name);
                   _LabelList.Add(LabelID);

               }
               foreach (var x in Panel)
               {
                   Dictionary<long, string> PanelID = new Dictionary<long, string>();
                   PanelID.Add(x.id, x.Name);
                   _PanelList.Add(PanelID);

               }
               foreach (var x in Picture)
               {
                   Dictionary<long, string> PictureID = new Dictionary<long, string>();
                   PictureID.Add(x.id, x.Name);
                   _PicturesList.Add(PictureID);

               }
               
             

                ButtonReadModule();
                ButtonRead();
                DateLbl();        
                Labels();
                PanelRead();
                PictureRead();
              

                timerDate.Start();
                createdynamicForm();

                if (Directory.GetFiles(_BackgroundImagePath).Length != 0)
                {
                  if(DynamicFormRandom)
                  {
                      int countfiles = Directory.GetFiles(_BackgroundImagePath).Length;
                      Random rnd = new Random();
                      int filesRnd = rnd.Next(0, countfiles);
                      i = filesRnd;
                  }
                  else
                     i = 0;

                  Pic_Background.Image = Image.FromFile(_BackgroundImagePath + imageName[i]);
                }
                else
                    timerDynamicForm.Stop();


                this.Refresh();
              
            }

            public void createdynamicForm()
            {

                if (_BackgroundImagePath != null)
                {
                    DirectoryInfo _di = new DirectoryInfo(_BackgroundImagePath);

                    //image files
                    FileInfo[] _diar1 = _di.GetFiles("*.jpg");
                    FileInfo[] _diar2 = _di.GetFiles("*.bmp");
                    FileInfo[] _diar3 = _di.GetFiles("*.png");
                    FileInfo[] _diar4 = _di.GetFiles("*.gif");
                    FileInfo[] _diar5 = _di.GetFiles("*.jpeg");


                    var _diarList = new List<FileInfo>();
                    _diarList.AddRange(_diar1);
                    _diarList.AddRange(_diar2);
                    _diarList.AddRange(_diar3);
                    _diarList.AddRange(_diar4);
                    _diarList.AddRange(_diar5);

                    FileInfo[] diar = _diarList.ToArray();

                    FileInfo _dra = null;
                    foreach (FileInfo _dra_loopVariable in _diarList)
                    {
                        _dra = _dra_loopVariable;
                        imageName = diar;
                    }
                 //   this.Refresh();
                }
            }


            bool DynamicFormRandom = false;

            int i = 0;
            FileInfo[] imageName;

            private void timerDynamicForm_Tick(object sender, EventArgs e)
            {
                

                if (DynamicFormRandom)
                {
                    int countfiles = Directory.GetFiles(_BackgroundImagePath).Length;
                    Random rnd = new Random();
                    int filesRnd = rnd.Next(0, countfiles);
                    i = filesRnd;
                    Pic_Background.Image = Image.FromFile(_BackgroundImagePath + imageName[i]);
                }
                else
                {
                    i++;

                    if (i < imageName.Count())

                        Pic_Background.Image = Image.FromFile(_BackgroundImagePath + imageName[i]);
                    else
                    {
                        i = 0;
                        Pic_Background.Image = Image.FromFile(_BackgroundImagePath + imageName[i]);
                    }
                }
                this.Refresh();
            }

            private void timer_ScreenSaver_Tick(object sender, EventArgs e)
            {
                _frm_ScreenSaver = new Frm_ScreenSaver();
                _frm_ScreenSaver.Show();
                timer_ScreenSaver.Stop();
            }

            private void pictureBoxLogout_DoubleClick(object sender, EventArgs e)
            {
                Frm_Logout frm = new Frm_Logout();
                frm.ShowDialog();
              
            }

            private void Main_FormClosing(object sender, FormClosingEventArgs e)
            {
            
              // ShowTaskBar();
            }

            private void Main_FormClosed(object sender, FormClosedEventArgs e)
            {
           
            }

    

          


    }
}
