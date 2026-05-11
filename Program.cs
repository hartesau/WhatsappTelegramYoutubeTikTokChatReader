#nullable disable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;
using Windows.Media.Core;
using System.Text.Json; 
using System.Numerics;

namespace TikTokChatReader
{
    public class FrameForm : Form
    {
        private Color _frameColor = Color.White;
        public Color FrameColor
        {
            get { return _frameColor; }
            set { _frameColor = value; this.Invalidate(); }
        }

        public FrameForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            if (_frameColor == Color.Transparent) return;

            int thickness = 4;
            using (Pen pen = new Pen(_frameColor, thickness))
            {
                e.Graphics.DrawRectangle(pen, thickness / 2, thickness / 2, this.Width - thickness, this.Height - thickness);
            }
        }
        
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; 
                return cp;
            }
        }
    }

    public class FlyReaderForm : Form
    {
        private RichTextBox rtb;
        private MainForm parentForm;
        private bool colorToggle = false;
        private bool isUsername = true;
        private Button btnClear;
        private Button btnPlay;
        private Button btnStop;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 0x115;
        private const int SB_BOTTOM = 7;

        public FlyReaderForm(MainForm parent)
        {
            this.parentForm = parent;
            this.Text = "FlyReader - Meister Edition";
            this.TopMost = true;
            this.Size = new Size(800, 150);
            
            try
            {
                if (File.Exists("flyreader_settings.txt"))
                {
                    string[] parts = File.ReadAllText("flyreader_settings.txt").Split(',');
                    if (parts.Length == 4)
                    {
                        Rectangle bounds = new Rectangle(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
                        if (Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(bounds)))
                        {
                            this.StartPosition = FormStartPosition.Manual;
                            this.Bounds = bounds;
                        }
                        else this.StartPosition = FormStartPosition.CenterScreen;
                    }
                }
                else this.StartPosition = FormStartPosition.CenterScreen;
            }
            catch { this.StartPosition = FormStartPosition.CenterScreen; }

            this.FormBorderStyle = FormBorderStyle.Sizable; 
            
            rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.Font = new Font("Segoe UI", 24f, FontStyle.Bold); 
            rtb.ReadOnly = true;
            rtb.BackColor = Color.Black;
            rtb.ForeColor = Color.White;
            rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
            
            btnClear = new Button();
            btnClear.Text = "X";
            btnClear.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnClear.Size = new Size(35, 35);
            btnClear.Location = new Point(this.ClientSize.Width - 45, 10);
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.BackColor = Color.DarkRed;
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Cursor = Cursors.Hand;
            btnClear.Click += (s, e) => { parentForm.PerformHardReset(); };

            btnStop = new Button();
            btnStop.Text = "⏹";
            btnStop.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnStop.Size = new Size(35, 35);
            btnStop.Location = new Point(this.ClientSize.Width - 85, 10);
            btnStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStop.BackColor = Color.DarkOrange;
            btnStop.ForeColor = Color.White;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Cursor = Cursors.Hand;
            btnStop.Click += (s, e) => { parentForm.StopEverything(); };

            btnPlay = new Button();
            btnPlay.Text = "▶";
            btnPlay.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnPlay.Size = new Size(35, 35);
            btnPlay.Location = new Point(this.ClientSize.Width - 125, 10);
            btnPlay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPlay.BackColor = Color.DarkGreen;
            btnPlay.ForeColor = Color.White;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.Click += (s, e) => { parentForm.StartEverything(); };

            this.Controls.Add(btnPlay);
            this.Controls.Add(btnStop);
            this.Controls.Add(btnClear);
            this.Controls.Add(rtb);
            
            btnPlay.BringToFront();
            btnStop.BringToFront();
            btnClear.BringToFront();

            this.FormClosing += FlyReaderForm_FormClosing;
        }

        private void FlyReaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try 
            { 
                Rectangle b = this.WindowState == FormWindowState.Minimized ? this.RestoreBounds : this.Bounds;
                File.WriteAllText("flyreader_settings.txt", $"{b.X},{b.Y},{b.Width},{b.Height}"); 
            } 
            catch { }

            if (parentForm != null && !parentForm.IsDisposed)
            {
                parentForm.WindowState = FormWindowState.Normal;
            }
        }

        public void ClearText()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ClearText()));
                return;
            }
            rtb.Clear();
            isUsername = true;
            colorToggle = false;
        }

        public void AppendText(string text, bool inlineLayout)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AppendText(text, inlineLayout)));
                return;
            }
            
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            if (inlineLayout)
            {
                if (isUsername)
                {
                    if (rtb.TextLength > 0) rtb.AppendText(Environment.NewLine);
                    rtb.SelectionColor = Color.Gold; 
                    rtb.AppendText(text + " ");  
                    isUsername = false;
                }
                else
                {
                    rtb.SelectionColor = colorToggle ? Color.LightSkyBlue : Color.White; 
                    colorToggle = !colorToggle;
                    rtb.AppendText(text);
                    isUsername = true;
                }
            }
            else
            {
                if (rtb.TextLength > 0) rtb.AppendText(Environment.NewLine);
                
                if (isUsername)
                {
                    rtb.SelectionColor = Color.Gold;
                    rtb.AppendText(text);
                    isUsername = false;
                }
                else
                {
                    rtb.SelectionColor = colorToggle ? Color.LightSkyBlue : Color.White;
                    colorToggle = !colorToggle;
                    rtb.AppendText(text);
                    isUsername = true;
                }
            }

            rtb.SelectionStart = rtb.Text.Length;
            rtb.ScrollToCaret();
            
            SendMessage(rtb.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
        }
    }
// NEU: Datenklasse für das Custom Dictionary
    public class CustomDictEntry
    {
        public int Count { get; set; }
        public bool Approved { get; set; }
    }

    public class MainForm : Form
    {
        private Button btnSelectArea;
        private ComboBox cmbFrameColor;
        private ComboBox cmbMode;
        private Button btnStartOcr;
        private Button btnStopOcr;
        private Button btnStartReading;
        private Button btnStopReading;
        private Button btnRestart;
        private Button btnCopyLog;
        private Button btnFlyReader;
        
        private Button btnInfo;
        private Button btnDataInspector; // NEU V024: Daten-Inspektor
        
        private NumericUpDown numDice;
        private NumericUpDown numHistory;
        private NumericUpDown numInterval;
        private NumericUpDown numZoom;
        private CheckBox chkInvert;
        
        private CheckBox chkDictionary;
        private CheckBox chkDebugDict;
        private List<string> dictDebugLog = new List<string>();
        private readonly object dictLogLock = new object();
        
        // NEU V024: Auto-Learn Checkbox und Strikes-Counter
        private CheckBox chkAutoLearnSession; 
        private NumericUpDown numStrikes; 
        
        private Dictionary<string, int> dictDrops = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private readonly object dropLock = new object();
        private bool dropsChanged = false;
        
        private Dictionary<string, CustomDictEntry> customDict = new Dictionary<string, CustomDictEntry>(StringComparer.OrdinalIgnoreCase);
        private readonly object customDictLock = new object();
        private bool customDictChanged = false;
        private string customDictFile = "custom_dictionary.json";
        
        private Dictionary<string, string> autoCorrectDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly object autoCorrectLock = new object();
        private string autoCorrectFile = "auto_correct.txt";
        private HashSet<string> pendingFuzzySearches = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly object fuzzySearchLock = new object();
        
        private Dictionary<string, List<DateTime>> recentDrops = new Dictionary<string, List<DateTime>>(StringComparer.OrdinalIgnoreCase);
        private readonly object recentDropsLock = new object();
        
        private HashSet<string> lastScannedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        private CheckBox chkFlyLayout;
        private CheckBox chkNoPauses;
        
        private NumericUpDown numWordDice;
        private CheckBox chkDebugOverlap;
        private List<string> overlapDebugLog = new List<string>();
        private readonly object overlapLogLock = new object();
        
        private NumericUpDown numSpeed;
        private TrackBar tbVolume;
        
        private ComboBox cmbVoice1;
        private ComboBox cmbVoice2;
        private bool voiceToggle = false;
        
        private ComboBox cmbProfiles;
        private TextBox txtProfileName;
        private Button btnSaveProfile;
        private Button btnLoadProfile;
        private Button btnDelProfile;
        private Dictionary<string, Rectangle> areaProfiles = new Dictionary<string, Rectangle>();
        private string profilesFile = "area_profiles.txt";

        private ListBox lbGlobal;
        private TextBox txtAddGlobal;
        private Button btnAddGlobal;
        private Button btnDelGlobal;
        
        private ListBox lbSpeech;
        private TextBox txtAddSpeech;
        private Button btnAddSpeech;
        private Button btnDelSpeech;

        private ListView listViewChat;
        private RichTextBox rtbCurrentText;
        private SplitContainer splitContainer;
        private System.Windows.Forms.Timer ocrTimer;

        private Rectangle chatArea = Rectangle.Empty;
        private FrameForm persistentFrame;
        private FlyReaderForm flyReader;
        
        private HashSet<string> seenTexts = new HashSet<string>();
        private HashSet<string> validDictionaryWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> sessionLearnedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase); 
        
        private Queue<string> spokenHistory = new Queue<string>();
        private Queue<string> ttsQueue = new Queue<string>();
        private List<string> ttsPendingBuffer = new List<string>();
        private List<string> syncLog = new List<string>();
        
        private string lastRawOcr = "";
        private string lastRawForDice = "";
        private string lastBufForDice = "";
        private string lastOutForDice = "";
        private string lastLogSpkStr = "";
        private string lastSpeakForOverlap = "";
        
        private DateTime lastAddedToQueue = DateTime.Now;
        
        private Windows.Media.SpeechSynthesis.SpeechSynthesizer synthesizer;
        private MediaPlayer mediaPlayer;
        
        private Windows.Media.SpeechSynthesis.SpeechSynthesizer infoSynthesizer;
        private MediaPlayer infoMediaPlayer;
        
        private bool isReadingActive = false;
        private bool isOcrRunning = false;
        private bool isCurrentlySpeaking = false; 
        
        private string settingsFile = "window_settings.txt";
        private string globalBlacklistFile = "global_blacklist.txt";
        private string speechBlacklistFile = "speech_blacklist.txt";
        private string dictionaryFile = "dictionary.txt"; 
        private string infoFile = "info_hilfe.txt"; 
        
        private string vorzimmer = "";

        public MainForm()
        {
            this.Font = new Font("Segoe UI", 12f);
            InitializeComponent();
            LoadWindowBounds();
            LoadLists();
            LoadCustomDictionary(); 
            LoadProfiles();
            LoadAutoCorrect(); 
            
            synthesizer = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            
            infoSynthesizer = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            infoMediaPlayer = new MediaPlayer();
            
            synthesizer.Options.SpeakingRate = (double)numSpeed.Value;
            mediaPlayer.Volume = tbVolume.Value / 100.0;
            
            foreach (var voice in Windows.Media.SpeechSynthesis.SpeechSynthesizer.AllVoices)
            {
                cmbVoice1.Items.Add(voice.DisplayName);
                cmbVoice2.Items.Add(voice.DisplayName);
            }
            
            int heddaIndex = -1;
            for (int i = 0; i < cmbVoice1.Items.Count; i++)
            {
                if (cmbVoice1.Items[i].ToString().IndexOf("Hedda", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    heddaIndex = i;
                    break;
                }
            }
            
            if (heddaIndex >= 0)
            {
                cmbVoice1.SelectedIndex = heddaIndex;
                cmbVoice2.SelectedIndex = heddaIndex;
            }
            else
            {
                if (cmbVoice1.Items.Count > 0) cmbVoice1.SelectedIndex = 0;
                if (cmbVoice2.Items.Count > 1) cmbVoice2.SelectedIndex = 1;
                else if (cmbVoice2.Items.Count > 0) cmbVoice2.SelectedIndex = 0;
            }

            txtProfileName.Text = "yt #1";
            if (cmbProfiles.Items.Contains("yt #1"))
            {
                cmbProfiles.SelectedItem = "yt #1";
            }

            ocrTimer = new System.Windows.Forms.Timer();
            ocrTimer.Interval = (int)numInterval.Value;
            ocrTimer.Tick += OcrTimer_Tick; 
            
            UpdateUI();
        }

        private void InitializeComponent()
        {
            this.Text = "TikTok Chat Reader V024 - Build 06.05.2026 18:50 Uhr";
            this.Size = new Size(1350, 950);
            this.FormClosing += MainForm_FormClosing;

            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 310; 
            this.Controls.Add(topPanel);

            btnSelectArea = new Button() { Text = "Bereich wählen", Location = new Point(10, 10), Width = 140, Height = 35 };
            btnSelectArea.Click += BtnSelectArea_Click;
            topPanel.Controls.Add(btnSelectArea);

            cmbFrameColor = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(160, 14), Width = 90 };
            cmbFrameColor.Items.AddRange(new string[] { "Weiß", "Rot", "Grün", "Blau", "Gelb", "Schwarz", "Keine" }); 
            cmbFrameColor.SelectedIndex = 0;
            cmbFrameColor.SelectedIndexChanged += CmbFrameColor_SelectedIndexChanged;
            topPanel.Controls.Add(cmbFrameColor);

            cmbMode = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(260, 14), Width = 180 };
            cmbMode.Items.AddRange(new string[] { "Modus: TikTok Chat", "Modus: YT Untertitel" });
            cmbMode.SelectedIndex = 1; 
            topPanel.Controls.Add(cmbMode);

            btnStartOcr = new Button() { Text = "OCR Start", Location = new Point(450, 10), Width = 100, Height = 35 };
            btnStartOcr.Click += (s, e) => { 
                if (chatArea != Rectangle.Empty) { 
                    ocrTimer.Start(); 
                    UpdateUI(); 
                } 
            };
            topPanel.Controls.Add(btnStartOcr);

            btnStopOcr = new Button() { Text = "OCR Stop", Location = new Point(560, 10), Width = 100, Height = 35 };
            btnStopOcr.Click += (s, e) => { 
                ocrTimer.Stop(); 
                if (cmbMode.SelectedIndex == 1 && !string.IsNullOrWhiteSpace(vorzimmer)) vorzimmer = ""; 
                UpdateUI();
            };
            topPanel.Controls.Add(btnStopOcr);

            btnStartReading = new Button() { Text = "Vorlesen Start", Location = new Point(670, 10), Width = 130, Height = 35 };
            btnStartReading.Click += BtnStartReading_Click;
            topPanel.Controls.Add(btnStartReading);

            btnStopReading = new Button() { Text = "Vorlesen Stop", Location = new Point(810, 10), Width = 130, Height = 35 };
            btnStopReading.Click += (s, e) => { 
                isReadingActive = false; 
                isCurrentlySpeaking = false;
                mediaPlayer.Pause(); 
                mediaPlayer.Source = null;
                UpdateUI();
            };
            topPanel.Controls.Add(btnStopReading);

            btnRestart = new Button() { Text = "Löschen", Location = new Point(950, 10), Width = 90, Height = 35 };
            btnRestart.Click += BtnRestart_Click;
            topPanel.Controls.Add(btnRestart);

            btnCopyLog = new Button() { Text = "Log kopieren", Location = new Point(1050, 10), Width = 130, Height = 35 };
            btnCopyLog.Click += BtnCopyLog_Click;
            topPanel.Controls.Add(btnCopyLog);

            btnFlyReader = new Button() { Text = "FlyReader", Location = new Point(1190, 10), Width = 110, Height = 35 };
            btnFlyReader.Click += BtnFlyReader_Click;
            topPanel.Controls.Add(btnFlyReader);

            Label lblDice = new Label() { Text = "Dice (%):", Location = new Point(10, 55), Width = 80 };
            topPanel.Controls.Add(lblDice);
            numDice = new NumericUpDown() { Location = new Point(90, 53), Width = 60, Minimum = 0, Maximum = 100, Value = 80 };
            topPanel.Controls.Add(numDice);

            Label lblHistory = new Label() { Text = "Gedächtnis (Sätze zurück):", Location = new Point(160, 55), Width = 190 };
            topPanel.Controls.Add(lblHistory);
            numHistory = new NumericUpDown() { Location = new Point(350, 53), Width = 60, Minimum = 1, Maximum = 100, Value = 15 };
            topPanel.Controls.Add(numHistory);

            Label lblInterval = new Label() { Text = "OCR Intervall (ms):", Location = new Point(420, 55), Width = 150 };
            topPanel.Controls.Add(lblInterval);
            numInterval = new NumericUpDown() { Location = new Point(570, 53), Width = 80, Minimum = 100, Maximum = 60000, Value = 888 }; 
            numInterval.ValueChanged += (s, e) => { if (ocrTimer != null) ocrTimer.Interval = (int)numInterval.Value; };
            topPanel.Controls.Add(numInterval);

            Label lblZoom = new Label() { Text = "Bild-Zoom:", Location = new Point(660, 55), Width = 90 };
            topPanel.Controls.Add(lblZoom);
            numZoom = new NumericUpDown() { Location = new Point(750, 53), Width = 50, Minimum = 1, Maximum = 5, Value = 2 };
            topPanel.Controls.Add(numZoom);
chkInvert = new CheckBox() { Text = "Farben invertieren", Location = new Point(820, 55), Width = 170, Checked = true };
            topPanel.Controls.Add(chkInvert);

            Label lblWordDice = new Label() { Text = "Wort-Dice (%):", Location = new Point(1000, 55), Width = 120 };
            topPanel.Controls.Add(lblWordDice);
            numWordDice = new NumericUpDown() { Location = new Point(1120, 53), Width = 60, Minimum = 0, Maximum = 100, Value = 70 };
            topPanel.Controls.Add(numWordDice);

            chkDebugOverlap = new CheckBox() { Text = "Overlap Debug", Location = new Point(1190, 55), Width = 150, Checked = false };
            chkDebugOverlap.CheckedChanged += ChkDebugOverlap_CheckedChanged;
            topPanel.Controls.Add(chkDebugOverlap);

            chkDictionary = new CheckBox() { Text = "Wörterbuch-Filter (braucht dictionary.txt)", Location = new Point(10, 90), Width = 320, Checked = true }; 
            topPanel.Controls.Add(chkDictionary);

            chkDebugDict = new CheckBox() { Text = "Wörterbuch Debug", Location = new Point(340, 90), Width = 170, Checked = false };
            chkDebugDict.CheckedChanged += ChkDebugDict_CheckedChanged;
            topPanel.Controls.Add(chkDebugDict);

            Label lblVoice1 = new Label() { Text = "Stimme 1:", Location = new Point(520, 92), Width = 70 };
            topPanel.Controls.Add(lblVoice1);
            cmbVoice1 = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(590, 89), Width = 150, DropDownWidth = 350 };
            topPanel.Controls.Add(cmbVoice1);

            Label lblVoice2 = new Label() { Text = "Stimme 2:", Location = new Point(750, 92), Width = 70 };
            topPanel.Controls.Add(lblVoice2);
            cmbVoice2 = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(820, 89), Width = 150, DropDownWidth = 350 };
            topPanel.Controls.Add(cmbVoice2);

            chkFlyLayout = new CheckBox() { Text = "FlyReader: 1-Zeilen Layout", Location = new Point(990, 91), Width = 250, Checked = true }; 
            topPanel.Controls.Add(chkFlyLayout);

            Label lblProfile = new Label() { Text = "Rahmen-Profile:", Location = new Point(10, 130), Width = 120 };
            topPanel.Controls.Add(lblProfile);
            cmbProfiles = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(130, 127), Width = 200 };
            topPanel.Controls.Add(cmbProfiles);
            btnLoadProfile = new Button() { Text = "Laden", Location = new Point(340, 126), Width = 80, Height = 30 };
            btnLoadProfile.Click += BtnLoadProfile_Click;
            topPanel.Controls.Add(btnLoadProfile);
            txtProfileName = new TextBox() { Location = new Point(440, 127), Width = 150 };
            topPanel.Controls.Add(txtProfileName);
            btnSaveProfile = new Button() { Text = "Speichern", Location = new Point(600, 126), Width = 100, Height = 30 };
            btnSaveProfile.Click += BtnSaveProfile_Click;
            topPanel.Controls.Add(btnSaveProfile);
            btnDelProfile = new Button() { Text = "Profil löschen", Location = new Point(710, 126), Width = 130, Height = 30 };
            btnDelProfile.Click += BtnDelProfile_Click;
            topPanel.Controls.Add(btnDelProfile);

            chkNoPauses = new CheckBox() { Text = "Lesestream (Pausen/Satzzeichen entfernen)", Location = new Point(860, 128), Width = 350, Checked = true }; 
            topPanel.Controls.Add(chkNoPauses);

            // NEU V024: Auto-Learn Checkbox und Strikes-Counter
            chkAutoLearnSession = new CheckBox() { Text = "Auto-Learn (Sitzung)", Location = new Point(10, 172), Width = 180, Checked = true };
            topPanel.Controls.Add(chkAutoLearnSession);
            Label lblStrikes = new Label() { Text = "Strikes:", Location = new Point(190, 173), Width = 60 };
            topPanel.Controls.Add(lblStrikes);
            numStrikes = new NumericUpDown() { Location = new Point(250, 171), Width = 50, Minimum = 1, Maximum = 20, Value = 3 };
            topPanel.Controls.Add(numStrikes);

            // ANGEPASST V024: Listen ein wenig nach unten gerückt
            Label lblGlobal = new Label() { Text = "Globale Blacklist:", Location = new Point(10, 200), Width = 180 };
            topPanel.Controls.Add(lblGlobal);
            lbGlobal = new ListBox() { Location = new Point(10, 225), Width = 200, Height = 80 };
            topPanel.Controls.Add(lbGlobal);
            txtAddGlobal = new TextBox() { Location = new Point(220, 225), Width = 100 };
            topPanel.Controls.Add(txtAddGlobal);
            btnAddGlobal = new Button() { Text = "+", Location = new Point(330, 224), Width = 30, Height = 30 };
            btnAddGlobal.Click += BtnAddGlobal_Click;
            topPanel.Controls.Add(btnAddGlobal);
            btnDelGlobal = new Button() { Text = "Löschen", Location = new Point(220, 260), Width = 140, Height = 30 };
            btnDelGlobal.Click += BtnDelGlobal_Click;
            topPanel.Controls.Add(btnDelGlobal);

            Label lblSpeech = new Label() { Text = "Sprach-Blacklist:", Location = new Point(380, 200), Width = 180 };
            topPanel.Controls.Add(lblSpeech);
            lbSpeech = new ListBox() { Location = new Point(380, 225), Width = 200, Height = 80 };
            topPanel.Controls.Add(lbSpeech);
            txtAddSpeech = new TextBox() { Location = new Point(590, 225), Width = 100 };
            topPanel.Controls.Add(txtAddSpeech);
            btnAddSpeech = new Button() { Text = "+", Location = new Point(700, 224), Width = 30, Height = 30 };
            btnAddSpeech.Click += BtnAddSpeech_Click;
            topPanel.Controls.Add(btnAddSpeech);
            btnDelSpeech = new Button() { Text = "Löschen", Location = new Point(590, 260), Width = 140, Height = 30 };
            btnDelSpeech.Click += BtnDelSpeech_Click;
            topPanel.Controls.Add(btnDelSpeech);

            Label lblSpeed = new Label() { Text = "Sprech-Tempo:", Location = new Point(940, 172), Width = 110 };
            topPanel.Controls.Add(lblSpeed);
            numSpeed = new NumericUpDown() { Location = new Point(1050, 170), Width = 60, Minimum = 0.5m, Maximum = 6.0m, Increment = 0.1m, DecimalPlaces = 1, Value = 1.0m };
            numSpeed.ValueChanged += (s, e) => { 
                if (synthesizer != null) synthesizer.Options.SpeakingRate = (double)numSpeed.Value; 
                if (infoSynthesizer != null) infoSynthesizer.Options.SpeakingRate = (double)numSpeed.Value;
            };
            topPanel.Controls.Add(numSpeed);

            Label lblVolume = new Label() { Text = "Lautstärke:", Location = new Point(940, 202), Width = 90 };
            topPanel.Controls.Add(lblVolume);
            tbVolume = new TrackBar() { Location = new Point(1030, 200), Width = 150, Height = 25, AutoSize = false, Minimum = 0, Maximum = 100, Value = 100, TickStyle = TickStyle.None };
            tbVolume.ValueChanged += (s, e) => { 
                if (mediaPlayer != null) mediaPlayer.Volume = tbVolume.Value / 100.0; 
                if (infoMediaPlayer != null) infoMediaPlayer.Volume = tbVolume.Value / 100.0;
            };
            topPanel.Controls.Add(tbVolume);

            // NEU V024: Daten-Inspektor Button
            btnDataInspector = new Button() { Text = "📂 Daten-Inspektor", Location = new Point(940, 230), Width = 150, Height = 30, BackColor = Color.LightSkyBlue };
            btnDataInspector.Click += BtnDataInspector_Click;
            topPanel.Controls.Add(btnDataInspector);

            btnInfo = new Button() { Text = "ℹ Info / Hilfe", Location = new Point(1100, 230), Width = 150, Height = 30, BackColor = Color.LightYellow };
            btnInfo.Click += BtnInfo_Click;
            topPanel.Controls.Add(btnInfo);

            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitContainer.SplitterDistance = 400;
            this.Controls.Add(splitContainer);
            splitContainer.BringToFront();

            listViewChat = new ListView();
            listViewChat.Dock = DockStyle.Fill;
            listViewChat.View = View.Details;
            listViewChat.FullRowSelect = true;
            listViewChat.Font = new Font("Segoe UI", 12f);
            
            ImageList dummyList = new ImageList();
            dummyList.ImageSize = new Size(1, 30); 
            listViewChat.SmallImageList = dummyList;

            listViewChat.Columns.Add("Datum/Zeit", 200);
            listViewChat.Columns.Add("Chat-Text", 1000);
            listViewChat.DoubleClick += ListViewChat_DoubleClick;
            splitContainer.Panel1.Controls.Add(listViewChat);

            rtbCurrentText = new RichTextBox();
            rtbCurrentText.Dock = DockStyle.Fill;
            rtbCurrentText.Font = new Font("Segoe UI", 12f);
            splitContainer.Panel2.Controls.Add(rtbCurrentText);
        }

        public void StartEverything()
        {
            if (chatArea != Rectangle.Empty)
            {
                ocrTimer.Start();
                isReadingActive = true;
                UpdateUI();
                
                if (ttsQueue.Count == 0 && ttsPendingBuffer.Count == 0)
                {
                    foreach (ListViewItem item in listViewChat.Items) ttsQueue.Enqueue(item.SubItems[1].Text);
                }
                if (!isCurrentlySpeaking) ProcessTTSQueue(forceFlush: true);
            }
            else
            {
                MessageBox.Show("Bitte wähle zuerst einen Rahmen aus.", "Hinweis");
            }
        }

        public void StopEverything()
        {
            ocrTimer.Stop();
            if (cmbMode.SelectedIndex == 1 && !string.IsNullOrWhiteSpace(vorzimmer)) vorzimmer = ""; 
            
            isReadingActive = false;
            isCurrentlySpeaking = false;
            mediaPlayer.Pause();
            mediaPlayer.Source = null;
            ttsQueue.Clear();
            ttsPendingBuffer.Clear();
            
            UpdateUI();
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            if (!File.Exists(infoFile))
            {
                string defaultInfo = @"===================================================
TTS STUDIO - MEISTER EDITION
Bedienungsanleitung & Feature-Lexikon
===================================================";
                try { File.WriteAllText(infoFile, defaultInfo, System.Text.Encoding.UTF8); } catch { }
            }

            string currentInfo = "";
            try { currentInfo = File.ReadAllText(infoFile); } catch { }

            Form infoForm = new Form() { Text = "Info & Hilfe (Editor)", Size = new Size(900, 750), StartPosition = FormStartPosition.CenterScreen };
            
            Panel topPanelInfo = new Panel() { Dock = DockStyle.Top, Height = 40 };
            Button btnSaveInfo = new Button() { Text = "💾 Speichern", Location = new Point(10, 5), Width = 120, Height = 30, BackColor = Color.LightGreen };
            Button btnPlayInfo = new Button() { Text = "▶ Vorlesen", Location = new Point(140, 5), Width = 120, Height = 30, BackColor = Color.LightSkyBlue };
            Button btnStopInfo = new Button() { Text = "⏹ Stop", Location = new Point(270, 5), Width = 100, Height = 30, BackColor = Color.LightCoral };
            
            topPanelInfo.Controls.Add(btnSaveInfo);
            topPanelInfo.Controls.Add(btnPlayInfo);
            topPanelInfo.Controls.Add(btnStopInfo);
            infoForm.Controls.Add(topPanelInfo);

            RichTextBox rtbInfo = new RichTextBox() { Dock = DockStyle.Fill, Text = currentInfo, Font = new Font("Consolas", 12f), ReadOnly = false };
            infoForm.Controls.Add(rtbInfo);
            rtbInfo.BringToFront();

            btnSaveInfo.Click += (s, ev) => 
            {
                try 
                { 
                    File.WriteAllText(infoFile, rtbInfo.Text, System.Text.Encoding.UTF8); 
                    MessageBox.Show("Anleitung erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } 
                catch (Exception ex) { MessageBox.Show("Fehler beim Speichern: " + ex.Message); }
            };

            btnPlayInfo.Click += async (s, ev) => 
            {
                infoMediaPlayer.Pause();
                infoMediaPlayer.Source = null;
                
                try
                {
                    if (cmbVoice1.SelectedItem != null)
                    {
                        var voice = Windows.Media.SpeechSynthesis.SpeechSynthesizer.AllVoices.FirstOrDefault(v => v.DisplayName == cmbVoice1.SelectedItem.ToString());
                        if (voice != null) infoSynthesizer.Voice = voice;
                    }
                    
                    infoSynthesizer.Options.SpeakingRate = (double)numSpeed.Value;
                    infoMediaPlayer.Volume = tbVolume.Value / 100.0;
                    
                    string textToRead = rtbInfo.SelectedText.Length > 0 ? rtbInfo.SelectedText : rtbInfo.Text;
                    if (!string.IsNullOrWhiteSpace(textToRead))
                    {
                        var stream = await infoSynthesizer.SynthesizeTextToStreamAsync(textToRead);
                        infoMediaPlayer.Source = MediaSource.CreateFromStream(stream, stream.ContentType);
                        infoMediaPlayer.Play();
                    }
                }
                catch { }
            };

            btnStopInfo.Click += (s, ev) => 
            {
                infoMediaPlayer.Pause();
                infoMediaPlayer.Source = null;
            };

            infoForm.FormClosing += (s, ev) => 
            {
                infoMediaPlayer.Pause();
                infoMediaPlayer.Source = null;
            };

            infoForm.Show();
        }

        // NEU V024: Der Daten-Inspektor!
        private void BtnDataInspector_Click(object sender, EventArgs e)
        {
            Form inspectorForm = new Form() { Text = "Daten-Inspektor", Size = new Size(800, 600), StartPosition = FormStartPosition.CenterScreen };
            
            Panel topPanel = new Panel() { Dock = DockStyle.Top, Height = 40 };
            Button btnSitzung = new Button() { Text = "Sitzung (RAM)", Location = new Point(10, 5), Width = 120, Height = 30 };
            Button btnCustom = new Button() { Text = "Custom Dict", Location = new Point(140, 5), Width = 120, Height = 30 };
            Button btnDrop = new Button() { Text = "Drop.json", Location = new Point(270, 5), Width = 120, Height = 30 };
            Button btnAuto = new Button() { Text = "Auto Correct", Location = new Point(400, 5), Width = 120, Height = 30 };
            
            topPanel.Controls.Add(btnSitzung);
            topPanel.Controls.Add(btnCustom);
            topPanel.Controls.Add(btnDrop);
            topPanel.Controls.Add(btnAuto);
            inspectorForm.Controls.Add(topPanel);
            
            RichTextBox rtbView = new RichTextBox() { Dock = DockStyle.Fill, Font = new Font("Consolas", 11f), ReadOnly = true, BackColor = Color.Black, ForeColor = Color.LightGray };
            inspectorForm.Controls.Add(rtbView);
            rtbView.BringToFront();

            btnSitzung.Click += (s, ev) => {
                rtbView.Text = sessionLearnedWords.Count > 0 ? "In dieser Sitzung gelernt:\r\n\r\n" + string.Join(Environment.NewLine, sessionLearnedWords) : "Noch keine Wörter in dieser Sitzung gelernt.";
            };
            
            btnCustom.Click += (s, ev) => {
                try { rtbView.Text = File.Exists(customDictFile) ? File.ReadAllText(customDictFile) : "Datei nicht gefunden."; } catch { }
            };
            
            btnDrop.Click += (s, ev) => {
                try { rtbView.Text = File.Exists("drop.json") ? File.ReadAllText("drop.json") : "Datei nicht gefunden."; } catch { }
            };
            
            btnAuto.Click += (s, ev) => {
                try { rtbView.Text = File.Exists(autoCorrectFile) ? File.ReadAllText(autoCorrectFile) : "Datei nicht gefunden."; } catch { }
            };

            // Startansicht
            btnSitzung.PerformClick();
            inspectorForm.Show();
        }
private void ChkDebugOverlap_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDebugOverlap.Checked)
            {
                string logText = "";
                lock(overlapLogLock)
                {
                    if (overlapDebugLog.Count > 0)
                    {
                        logText = string.Join(Environment.NewLine, overlapDebugLog);
                        overlapDebugLog.Clear();
                    }
                }
                
                if (!string.IsNullOrEmpty(logText))
                {
                    Form debugForm = new Form() { Text = "Overlap Debug Log (Schieberegister)", Size = new Size(800, 600), StartPosition = FormStartPosition.CenterScreen };
                    RichTextBox rtbLog = new RichTextBox() { Dock = DockStyle.Fill, Text = logText, Font = new Font("Consolas", 11f), ReadOnly = true };
                    debugForm.Controls.Add(rtbLog);
                    debugForm.Show();
                }
            }
        }

        private void ChkDebugDict_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDebugDict.Checked)
            {
                string logText = "";
                lock(dictLogLock)
                {
                    if (dictDebugLog.Count > 0)
                    {
                        logText = string.Join(Environment.NewLine, dictDebugLog);
                        dictDebugLog.Clear();
                    }
                }

                if (!string.IsNullOrEmpty(logText))
                {
                    Form debugForm = new Form() { Text = "Wörterbuch & Auto-Learn Debug Log", Size = new Size(800, 600), StartPosition = FormStartPosition.CenterScreen };
                    RichTextBox rtbLog = new RichTextBox() { Dock = DockStyle.Fill, Text = logText, Font = new Font("Consolas", 11f), ReadOnly = true };
                    debugForm.Controls.Add(rtbLog);
                    debugForm.Show();
                }
            }
        }

        private void LoadCustomDictionary()
        {
            try
            {
                if (File.Exists(customDictFile))
                {
                    string json = File.ReadAllText(customDictFile);
                    var loaded = JsonSerializer.Deserialize<Dictionary<string, CustomDictEntry>>(json);
                    if (loaded != null) 
                    {
                        customDict = new Dictionary<string, CustomDictEntry>(loaded, StringComparer.OrdinalIgnoreCase);
                        
                        foreach (var kvp in customDict)
                        {
                            if (kvp.Value.Approved)
                            {
                                validDictionaryWords.Add(kvp.Key);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void SaveCustomDictionary()
        {
            try
            {
                Dictionary<string, CustomDictEntry> copy;
                lock (customDictLock)
                {
                    copy = customDict.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
                }
                
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
                };
                
                string json = JsonSerializer.Serialize(copy, options);
                File.WriteAllText(customDictFile, json, System.Text.Encoding.UTF8);
            }
            catch { }
        }

        private void LoadAutoCorrect()
        {
            try
            {
                if (File.Exists(autoCorrectFile))
                {
                    string[] lines = File.ReadAllLines(autoCorrectFile);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            autoCorrectDict[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
                }
            }
            catch { }
        }

        private void RunFuzzySearchAsync(string brokenWord, bool debugEnabled)
        {
            Task.Run(() =>
            {
                lock (fuzzySearchLock)
                {
                    if (pendingFuzzySearches.Contains(brokenWord)) return;
                    pendingFuzzySearches.Add(brokenWord);
                }

                string bestMatch = null;

                foreach (string validWord in validDictionaryWords)
                {
                    if (validWord.Length >= 10 && brokenWord.Length >= 5) 
                    {
                        int lenDiff = validWord.Length - brokenWord.Length;
                        
                        if (lenDiff >= 1 && lenDiff <= 3) 
                        {
                            if (validWord.StartsWith(brokenWord, StringComparison.OrdinalIgnoreCase) || 
                                validWord.EndsWith(brokenWord, StringComparison.OrdinalIgnoreCase))
                            {
                                bestMatch = validWord;
                                break; 
                            }
                        }
                    }
                }

                if (bestMatch != null) 
                {
                    lock (autoCorrectLock)
                    {
                        if (!autoCorrectDict.ContainsKey(brokenWord))
                        {
                            autoCorrectDict[brokenWord] = bestMatch;
                            try { File.AppendAllText(autoCorrectFile, $"{brokenWord}={bestMatch}{Environment.NewLine}"); } catch { }
                            
                            if (debugEnabled)
                            {
                                lock(dictLogLock)
                                {
                                    dictDebugLog.Add($"[FUZZY-AGENT] Randfehler bei '{brokenWord}' ergänzt zu '{bestMatch}'");
                                }
                            }
                        }
                    }
                }

                lock (fuzzySearchLock)
                {
                    pendingFuzzySearches.Remove(brokenWord);
                }
            });
        }

        private void LoadDrops()
        {
            try
            {
                if (File.Exists("drop.json"))
                {
                    string json = File.ReadAllText("drop.json");
                    var loaded = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                    if (loaded != null) dictDrops = new Dictionary<string, int>(loaded, StringComparer.OrdinalIgnoreCase);
                }
            }
            catch { }
        }

        private void SaveDrops()
        {
            try
            {
                Dictionary<string, int> copy;
                lock (dropLock)
                {
                    copy = dictDrops.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }
                
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
                };
                
                string json = JsonSerializer.Serialize(copy, options);
                File.WriteAllText("drop.json", json, System.Text.Encoding.UTF8);
            }
            catch { }
        }

        private void LoadTreasure()
        {
            // Ab V022 nutzen wir stattdessen LoadCustomDictionary(), Funktion zur Abwärtskompatibilität leer gelassen
        }

        private void SaveTreasure()
        {
            // Ab V022 nutzen wir stattdessen SaveCustomDictionary(), Funktion zur Abwärtskompatibilität leer gelassen
        }

        private void LoadProfiles()
        {
            try
            {
                cmbProfiles.Items.Clear();
                areaProfiles.Clear();
                if (File.Exists(profilesFile))
                {
                    string[] lines = File.ReadAllLines(profilesFile);
                    foreach (string line in lines)
                    {
                        string[] p = line.Split('|');
                        if (p.Length == 5)
                        {
                            string name = p[0];
                            Rectangle rect = new Rectangle(int.Parse(p[1]), int.Parse(p[2]), int.Parse(p[3]), int.Parse(p[4]));
                            areaProfiles[name] = rect;
                            cmbProfiles.Items.Add(name);
                        }
                    }
                }
                if (cmbProfiles.Items.Contains(txtProfileName.Text))
                {
                    cmbProfiles.SelectedItem = txtProfileName.Text;
                }
                else if (cmbProfiles.Items.Count > 0) 
                {
                    cmbProfiles.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void SaveProfiles()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var kvp in areaProfiles)
                {
                    lines.Add($"{kvp.Key}|{kvp.Value.X}|{kvp.Value.Y}|{kvp.Value.Width}|{kvp.Value.Height}");
                }
                File.WriteAllLines(profilesFile, lines);
            }
            catch { }
        }

        private void BtnSaveProfile_Click(object sender, EventArgs e)
        {
            string name = txtProfileName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Bitte gib einen Namen für das Profil ein.", "Hinweis");
                return;
            }
            if (chatArea == Rectangle.Empty || chatArea.Width == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Rahmen aus.", "Hinweis");
                return;
            }

            areaProfiles[name] = chatArea;
            SaveProfiles();
            LoadProfiles();
            cmbProfiles.SelectedItem = name;
            txtProfileName.Clear();
        }

        private void BtnLoadProfile_Click(object sender, EventArgs e)
        {
            if (cmbProfiles.SelectedItem != null)
            {
                string name = cmbProfiles.SelectedItem.ToString();
                if (areaProfiles.ContainsKey(name))
                {
                    chatArea = areaProfiles[name];
                    
                    if (persistentFrame != null && !persistentFrame.IsDisposed) persistentFrame.Hide();
                    if (persistentFrame == null || persistentFrame.IsDisposed) persistentFrame = new FrameForm();
                    
                    persistentFrame.Bounds = chatArea; 
                    persistentFrame.FrameColor = GetSelectedColor(); 
                    persistentFrame.Show();
                    
                    StartEverything();
                }
            }
        }

        private void BtnDelProfile_Click(object sender, EventArgs e)
        {
            if (cmbProfiles.SelectedItem != null)
            {
                string name = cmbProfiles.SelectedItem.ToString();
                if (areaProfiles.ContainsKey(name))
                {
                    areaProfiles.Remove(name);
                    SaveProfiles();
                    LoadProfiles();
                }
            }
        }

        private void BtnFlyReader_Click(object sender, EventArgs e)
        {
            if (flyReader == null || flyReader.IsDisposed)
            {
                flyReader = new FlyReaderForm(this);
                foreach (ListViewItem item in listViewChat.Items)
                {
                    flyReader.AppendText(item.SubItems[1].Text, chkFlyLayout.Checked);
                }
            }
            flyReader.Show();
            this.WindowState = FormWindowState.Minimized;
        }

        private void UpdateUI()
        {
            if (ocrTimer.Enabled)
            {
                btnStartOcr.BackColor = Color.LightGreen;
                btnStopOcr.BackColor = SystemColors.Control;
            }
            else
            {
                btnStartOcr.BackColor = SystemColors.Control;
                btnStopOcr.BackColor = Color.LightCoral;
            }

            if (isReadingActive)
            {
                btnStartReading.BackColor = Color.LightGreen;
                btnStopReading.BackColor = SystemColors.Control;
            }
            else
            {
                btnStartReading.BackColor = SystemColors.Control;
                btnStopReading.BackColor = Color.LightCoral;
            }
        }

        private void LogSyncWithDice(string type, string text, ref string lastTextTracker)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            string cleanText = text.Replace("\r", " ").Replace("\n", " ").Trim();
            if (string.IsNullOrWhiteSpace(cleanText)) return;
            
            double dice = 0;
            if (!string.IsNullOrWhiteSpace(lastTextTracker))
            {
                string c1 = Regex.Replace(cleanText.ToLower(), @"[^a-z0-9äöüß\s]", "").Trim();
                string c2 = Regex.Replace(lastTextTracker.ToLower(), @"[^a-z0-9äöüß\s]", "").Trim();
                if (c1.Length >= 3 && c2.Length >= 3)
                {
                    dice = CalculateDiceCoefficient(c1, c2) * 100.0;
                }
            }
            lastTextTracker = cleanText;
            
            string timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff");
            string paddedType = type.PadRight(4);
            syncLog.Add($"[{timestamp}]\t[{paddedType} | {dice:000}%]\t{cleanText}");
            
            if (syncLog.Count > 10000) syncLog.RemoveAt(0);
        }

        private void BtnCopyLog_Click(object sender, EventArgs e)
        {
            if (syncLog.Count > 0)
            {
                Clipboard.SetText(string.Join(Environment.NewLine, syncLog));
                syncLog.Clear(); 
                MessageBox.Show("Das detaillierte synchrone Log wurde erfolgreich in die Zwischenablage kopiert und danach geleert.", "Log kopiert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Das Log ist aktuell leer.", "Hinweis");
            }
        }

        private void LoadLists()
        {
            try
            {
                if (File.Exists(globalBlacklistFile)) lbGlobal.Items.AddRange(File.ReadAllLines(globalBlacklistFile));
                if (File.Exists(speechBlacklistFile)) lbSpeech.Items.AddRange(File.ReadAllLines(speechBlacklistFile));
                
                if (File.Exists(dictionaryFile))
                {
                    string[] words = File.ReadAllLines(dictionaryFile);
                    foreach (string word in words)
                    {
                        if (!string.IsNullOrWhiteSpace(word)) validDictionaryWords.Add(word.Trim());
                    }
                }
            }
            catch { }
        }

        private void SaveBlacklists()
        {
            try
            {
                File.WriteAllLines(globalBlacklistFile, lbGlobal.Items.Cast<string>());
                File.WriteAllLines(speechBlacklistFile, lbSpeech.Items.Cast<string>());
            }
            catch { }
        }

        private void BtnAddGlobal_Click(object sender, EventArgs e)
        {
            string word = txtAddGlobal.Text.Trim();
            if (!string.IsNullOrEmpty(word) && !lbGlobal.Items.Contains(word))
            {
                lbGlobal.Items.Add(word);
                txtAddGlobal.Clear();
                SaveBlacklists();
            }
        }

        private void BtnDelGlobal_Click(object sender, EventArgs e)
        {
            if (lbGlobal.SelectedIndex >= 0)
            {
                lbGlobal.Items.RemoveAt(lbGlobal.SelectedIndex);
                SaveBlacklists();
            }
        }

        private void BtnAddSpeech_Click(object sender, EventArgs e)
        {
            string word = txtAddSpeech.Text.Trim();
            if (!string.IsNullOrEmpty(word) && !lbSpeech.Items.Contains(word))
            {
                lbSpeech.Items.Add(word);
                txtAddSpeech.Clear();
                SaveBlacklists();
            }
        }

        private void BtnDelSpeech_Click(object sender, EventArgs e)
        {
            if (lbSpeech.SelectedIndex >= 0)
            {
                lbSpeech.Items.RemoveAt(lbSpeech.SelectedIndex);
                SaveBlacklists();
            }
        }

        private Color GetSelectedColor()
        {
            if (cmbFrameColor.SelectedItem == null) return Color.White;
            switch (cmbFrameColor.SelectedItem.ToString())
            {
                case "Rot": return Color.Red;
                case "Grün": return Color.Lime;
                case "Blau": return Color.Cyan;
                case "Gelb": return Color.Yellow;
                case "Schwarz": return Color.Black;
                case "Keine": return Color.Transparent;
                default: return Color.White;
            }
        }

        private void CmbFrameColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (persistentFrame != null && !persistentFrame.IsDisposed)
            {
                persistentFrame.FrameColor = GetSelectedColor();
                persistentFrame.Invalidate();
            }
        }

        private void LoadWindowBounds()
        {
            try
            {
                if (File.Exists(settingsFile))
                {
                    string[] parts = File.ReadAllText(settingsFile).Split(',');
                    if (parts.Length == 4)
                    {
                        Rectangle bounds = new Rectangle(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
                        if (Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(bounds)))
                        {
                            this.StartPosition = FormStartPosition.Manual;
                            this.Bounds = bounds;
                            return;
                        }
                    }
                }
            }
            catch { }
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (persistentFrame != null && !persistentFrame.IsDisposed) persistentFrame.Close();
            if (flyReader != null && !flyReader.IsDisposed) flyReader.Close();
            
            if (infoMediaPlayer != null)
            {
                infoMediaPlayer.Pause();
                infoMediaPlayer.Source = null;
            }
            
            Rectangle boundsToSave = this.WindowState == FormWindowState.Minimized ? this.RestoreBounds : this.Bounds;
            if (boundsToSave.X >= -10000 && boundsToSave.Y >= -10000)
            {
                try { File.WriteAllText(settingsFile, $"{boundsToSave.X},{boundsToSave.Y},{boundsToSave.Width},{boundsToSave.Height}"); } catch { }
            }
        }

        private void BtnSelectArea_Click(object sender, EventArgs e)
        {
            if (persistentFrame != null && !persistentFrame.IsDisposed) persistentFrame.Hide();
            this.WindowState = FormWindowState.Minimized;
            System.Threading.Thread.Sleep(250);

            Form overlay = new Form() {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                AutoScaleMode = AutoScaleMode.None,
                Bounds = SystemInformation.VirtualScreen,
                BackColor = Color.Black,
                Opacity = 0.4,
                Cursor = Cursors.Cross,
                TopMost = true
            };

            Point startPos = Point.Empty;
            Rectangle selection = Rectangle.Empty;
            bool isDragging = false;
            Color drawColor = GetSelectedColor();
            if (drawColor == Color.Transparent) drawColor = Color.White;

            overlay.MouseDown += (s, ev) => { startPos = Cursor.Position; isDragging = true; };
            overlay.MouseMove += (s, ev) => { 
                if (isDragging) { 
                    Point currentPos = Cursor.Position;
                    selection = new Rectangle(Math.Min(startPos.X, currentPos.X), Math.Min(startPos.Y, currentPos.Y), Math.Abs(startPos.X - currentPos.X), Math.Abs(startPos.Y - currentPos.Y)); 
                    overlay.Invalidate(); 
                } 
            };
            overlay.Paint += (s, ev) => { 
                if (selection != Rectangle.Empty) { 
                    Rectangle drawRect = new Rectangle(selection.X - SystemInformation.VirtualScreen.Left, selection.Y - SystemInformation.VirtualScreen.Top, selection.Width, selection.Height);
                    using (Pen pen = new Pen(drawColor, 2)) { ev.Graphics.DrawRectangle(pen, drawRect); }
                } 
            };
            overlay.MouseUp += (s, ev) => { 
                isDragging = false; chatArea = selection; overlay.Close();
                if (chatArea.Width > 0 && chatArea.Height > 0) {
                    if (persistentFrame == null || persistentFrame.IsDisposed) persistentFrame = new FrameForm();
                    persistentFrame.Bounds = chatArea; persistentFrame.FrameColor = GetSelectedColor(); persistentFrame.Show();
                    
                    StartEverything();
                }
                this.WindowState = FormWindowState.Normal;
            };
            overlay.ShowDialog();
        }

        public void PerformHardReset()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(PerformHardReset));
                return;
            }

            mediaPlayer.Pause();
            mediaPlayer.Source = null;
            isCurrentlySpeaking = false;
            
            if (infoMediaPlayer != null)
            {
                infoMediaPlayer.Pause();
                infoMediaPlayer.Source = null;
            }

            listViewChat.Items.Clear(); 
            syncLog.Clear();
            rtbCurrentText.Clear();
            
            vorzimmer = "";
            ttsQueue.Clear();
            ttsPendingBuffer.Clear();
            
            lastRawOcr = "";
            lastRawForDice = "";
            lastBufForDice = "";
            lastOutForDice = "";
            lastLogSpkStr = "";
            lastSpeakForOverlap = "";
            
            voiceToggle = false;
            spokenHistory.Clear();
            seenTexts.Clear();
            
            validDictionaryWords.ExceptWith(sessionLearnedWords);
            sessionLearnedWords.Clear();
            lastScannedWords.Clear(); 
            
            lock(overlapLogLock) overlapDebugLog.Clear();
            chkDebugOverlap.Checked = false;
            
            lock(dictLogLock) dictDebugLog.Clear();
            chkDebugDict.Checked = false;
            
            lock(fuzzySearchLock) pendingFuzzySearches.Clear();
            
            if (flyReader != null && !flyReader.IsDisposed)
            {
                flyReader.ClearText();
            }
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            PerformHardReset();
        }

        private Bitmap PreprocessImage(Bitmap original)
        {
            int scale = (int)numZoom.Value;
            Bitmap processed = new Bitmap(original.Width * scale, original.Height * scale);
            
            using (Graphics g = Graphics.FromImage(processed))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                
                if (chkInvert.Checked)
                {
                    ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                    {
                        new float[] {-1, 0, 0, 0, 0},
                        new float[] {0, -1, 0, 0, 0},
                        new float[] {0, 0, -1, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {1, 1, 1, 0, 1}
                    });

                    using (ImageAttributes attributes = new ImageAttributes())
                    {
                        attributes.SetColorMatrix(colorMatrix);
                        g.DrawImage(original, new Rectangle(0, 0, processed.Width, processed.Height),
                                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                    }
                }
                else
                {
                    g.DrawImage(original, 0, 0, processed.Width, processed.Height);
                }
            }
            return processed;
        }
private string CleanOcrText(string input, HashSet<string> currentScannedWords)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            input = input.Replace("\r", " ").Replace("\n", " ");
            input = Regex.Replace(input, @"([a-zA-ZäöüÄÖÜß])\1{2,}", "");

            input = Regex.Replace(input, @"\b\S+\s+ist beigetreten\b", "", RegexOptions.IgnoreCase);

            string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var validWords = new List<string>();
            
            bool debugDict = chkDebugDict.Checked;

            foreach (string w in words)
            {
                string pureAlpha = Regex.Replace(w, @"[^a-zA-ZäöüÄÖÜß]", "");

                if (Regex.IsMatch(w, @"\d")) { validWords.Add(w); continue; }
                if (pureAlpha.Length == 0) { validWords.Add(w); continue; }
                
                if (pureAlpha.Length == 1 && !Regex.IsMatch(pureAlpha, @"[aeiouäöüAEIOUÄÖÜ]")) continue;
                if (pureAlpha.Length == 2 && Regex.IsMatch(pureAlpha, @"^[A-ZÄÖÜ]+$")) continue;

                string correctedWord = pureAlpha;
                lock (autoCorrectLock)
                {
                    if (autoCorrectDict.ContainsKey(pureAlpha))
                    {
                        correctedWord = autoCorrectDict[pureAlpha];
                    }
                }
                pureAlpha = correctedWord;
                
                currentScannedWords.Add(pureAlpha);

                if (chkDictionary.Checked && validDictionaryWords.Count > 0)
                {
                    if (pureAlpha.Length > 3)
                    {
                        if (!validDictionaryWords.Contains(pureAlpha))
                        {
                            bool sessionLearned = false;
                            
                            // NEU V024: Auto-Learn Checkbox-Abfrage
                            if (chkAutoLearnSession.Checked)
                            {
                                lock (recentDropsLock)
                                {
                                    if (!recentDrops.ContainsKey(pureAlpha)) recentDrops[pureAlpha] = new List<DateTime>();
                                    
                                    // Zähle den Strike nur, wenn das Wort vorher nicht auf dem Bildschirm war
                                    if (!lastScannedWords.Contains(pureAlpha))
                                    {
                                        recentDrops[pureAlpha].Add(DateTime.Now);
                                        
                                        recentDrops[pureAlpha].RemoveAll(t => (DateTime.Now - t).TotalMinutes > 10);
                                        
                                        // Strike-Anzahl flexibel aus der GUI
                                        if (recentDrops[pureAlpha].Count >= (int)numStrikes.Value)
                                        {
                                            validDictionaryWords.Add(pureAlpha);
                                            sessionLearnedWords.Add(pureAlpha);
                                            recentDrops.Remove(pureAlpha); 
                                            sessionLearned = true;
                                            
                                            lock (customDictLock)
                                            {
                                                if (customDict.ContainsKey(pureAlpha)) customDict[pureAlpha].Count++;
                                                else customDict[pureAlpha] = new CustomDictEntry { Count = 1, Approved = false };
                                                customDictChanged = true;
                                            }
                                            
                                            if (debugDict) 
                                            {
                                                lock(dictLogLock) dictDebugLog.Add($"[SESSION-LEARN] '{pureAlpha}' freigeschaltet -> custom_dict +1");
                                            }
                                        }
                                    }
                                }
                            }

                            if (sessionLearned)
                            {
                                validWords.Add(w); 
                                continue;
                            }

                            if (debugDict) 
                            {
                                lock(dictLogLock) dictDebugLog.Add($"[WÖRTERBUCH] DROP: '{w}' (nicht gefunden)");
                            }
                            
                            // FIX V024: Das Custom Dict zählt nur noch +1, wenn das Wort NEU ins Bild kommt!
                            if (!lastScannedWords.Contains(pureAlpha))
                            {
                                lock (customDictLock)
                                {
                                    if (customDict.ContainsKey(pureAlpha)) customDict[pureAlpha].Count++;
                                    else customDict[pureAlpha] = new CustomDictEntry { Count = 1, Approved = false };
                                    customDictChanged = true;
                                }
                                
                                lock (dropLock)
                                {
                                    if (dictDrops.ContainsKey(pureAlpha)) dictDrops[pureAlpha]++;
                                    else dictDrops[pureAlpha] = 1;
                                    dropsChanged = true;
                                }
                            }
                            
                            RunFuzzySearchAsync(pureAlpha, debugDict);
                            
                            continue; 
                        }
                        else
                        {
                            if (debugDict) 
                            {
                                lock(dictLogLock) dictDebugLog.Add($"[WÖRTERBUCH] OK: '{w}'");
                            }
                        }
                    }
                }

                validWords.Add(w);
            }

            string result = string.Join(" ", validWords);
            return Regex.Replace(result, @"\s+", " ").Trim();
        }

        private string ExtractDeltaAndUpdateVorzimmer(string newText)
        {
            if (string.IsNullOrWhiteSpace(vorzimmer)) {
                vorzimmer = newText;
                return newText;
            }

            string[] rawOldWords = vorzimmer.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] rawNewWords = newText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (rawOldWords.Length == 0 || rawNewWords.Length == 0) return newText;

            string[] oldWords = rawOldWords.Select(w => Regex.Replace(w.ToLower(), @"[^a-z0-9äöüß]", "")).ToArray();
            string[] newWords = rawNewWords.Select(w => Regex.Replace(w.ToLower(), @"[^a-z0-9äöüß]", "")).ToArray();

            string cleanOldFull = string.Join(" ", oldWords);
            string cleanNewFull = string.Join(" ", newWords);
            if (cleanOldFull.Contains(cleanNewFull))
            {
                return ""; 
            }

            int bestOverlapCount = 0;
            int bestNewStartIdx = 0;
            int bestOldStartIdx = 0;

            for (int i = 0; i < oldWords.Length; i++)
            {
                for (int j = 0; j <= 5 && j < newWords.Length; j++)
                {
                    int overlapCount = 0;
                    int oldIdx = i;
                    int newIdx = j;

                    while (oldIdx < oldWords.Length && newIdx < newWords.Length)
                    {
                        if (string.IsNullOrEmpty(oldWords[oldIdx])) { oldIdx++; continue; }
                        if (string.IsNullOrEmpty(newWords[newIdx])) { newIdx++; continue; }

                        if (oldWords[oldIdx] == newWords[newIdx] || CalculateDiceCoefficient(oldWords[oldIdx], newWords[newIdx]) > 0.7)
                        {
                            overlapCount++;
                            oldIdx++;
                            newIdx++;
                        }
                        else break;
                    }

                    if (oldWords.Length - oldIdx <= 3) 
                    {
                        if (overlapCount > bestOverlapCount)
                        {
                            bestOverlapCount = overlapCount;
                            bestNewStartIdx = j;
                            bestOldStartIdx = i;
                        }
                    }
                }
            }

            if (bestOverlapCount >= 1)
            {
                int deltaStartIndex = bestNewStartIdx + bestOverlapCount;
                string[] deltaWords = rawNewWords.Skip(deltaStartIndex).ToArray();

                if (deltaWords.Length > 0)
                {
                    var cleanOldWords = rawOldWords.Take(bestOldStartIdx + bestOverlapCount);
                    var updatedVorzimmerWords = cleanOldWords.Concat(deltaWords);

                    if (updatedVorzimmerWords.Count() > 40) {
                        updatedVorzimmerWords = updatedVorzimmerWords.Skip(updatedVorzimmerWords.Count() - 40);
                    }
                    vorzimmer = string.Join(" ", updatedVorzimmerWords);

                    return string.Join(" ", deltaWords);
                }
                return ""; 
            }

            double fullDice = CalculateDiceCoefficient(cleanOldFull, cleanNewFull);
            if (fullDice > 0.5)
            {
                if (rawNewWords.Length > rawOldWords.Length) vorzimmer = newText;
                return "";
            }

            vorzimmer = newText;
            return newText;
        }

        private void CommitToListViewAndSpeak(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            
            bool skipGlobal = false;
            foreach (string word in lbGlobal.Items)
            {
                if (string.IsNullOrWhiteSpace(word)) continue;
                if (text.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    skipGlobal = true; break;
                }
            }

            if (skipGlobal) return;

            string cleanTextForDice = Regex.Replace(text.ToLower(), @"[^a-z0-9äöüß\s]", "").Trim();
            bool isDuplicate = false;
            double highestDice = 0.0;
            
            if (cleanTextForDice.Length > 3) 
            {
                foreach (string historyItem in spokenHistory)
                {
                    string cleanHistoryItem = Regex.Replace(historyItem.ToLower(), @"[^a-z0-9äöüß\s]", "").Trim();
                    if (string.IsNullOrWhiteSpace(cleanHistoryItem)) continue;

                    if (cleanTextForDice == cleanHistoryItem)
                    {
                        highestDice = 100.0;
                        isDuplicate = true;
                        break;
                    }

                    if (cleanTextForDice.Length >= 3 && cleanHistoryItem.Length >= 3)
                    {
                        double currentDice = CalculateDiceCoefficient(cleanTextForDice, cleanHistoryItem) * 100.0;
                        if (currentDice > highestDice)
                        {
                            highestDice = currentDice;
                        }
                    }
                }
                
                if (highestDice >= (double)numDice.Value)
                {
                    isDuplicate = true;
                }
            }

            if (isDuplicate)
            {
                LogSyncWithDice("DROP", text, ref lastOutForDice);
                return;
            }

            if (cleanTextForDice.Length > 2)
            {
                spokenHistory.Enqueue(text);
                while (spokenHistory.Count > (int)numHistory.Value)
                {
                    spokenHistory.Dequeue();
                }
            }

            LogSyncWithDice("OUT", text, ref lastOutForDice);
            
            ListViewItem item = new ListViewItem(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            item.SubItems.Add(text);
            listViewChat.Items.Add(item);
            listViewChat.EnsureVisible(listViewChat.Items.Count - 1);

            if (flyReader != null && !flyReader.IsDisposed)
            {
                flyReader.AppendText(text, chkFlyLayout.Checked);
            }
            
            ttsQueue.Enqueue(text);
            lastAddedToQueue = DateTime.Now;
            
            if (isReadingActive && !isCurrentlySpeaking) 
                ProcessTTSQueue(forceFlush: false);
        }

        private async void OcrTimer_Tick(object sender, EventArgs e)
        {
            if (isOcrRunning) return;

            if (isReadingActive && !isCurrentlySpeaking)
            {
                if ((DateTime.Now - lastAddedToQueue).TotalSeconds > 4 && ttsPendingBuffer.Count > 0)
                {
                    ProcessTTSQueue(forceFlush: true);
                }
            }

            if (chatArea == Rectangle.Empty || chatArea.Width <= 0 || chatArea.Height <= 0) return;
            
            isOcrRunning = true;
            try
            {
                using (Bitmap rawBmp = new Bitmap(chatArea.Width, chatArea.Height))
                {
                    using (Graphics g = Graphics.FromImage(rawBmp)) g.CopyFromScreen(chatArea.Location, Point.Empty, chatArea.Size);
                    
                    using (Bitmap processedBmp = PreprocessImage(rawBmp))
                    {
                        byte[] imageBytes;
                        using (var ms = new MemoryStream())
                        {
                            processedBmp.Save(ms, ImageFormat.Bmp);
                            imageBytes = ms.ToArray();
                        }

                        using (var ras = new InMemoryRandomAccessStream())
                        {
                            using (var writer = new DataWriter(ras.GetOutputStreamAt(0)))
                            {
                                writer.WriteBytes(imageBytes);
                                await writer.StoreAsync();
                            }
                            
                            var decoder = await BitmapDecoder.CreateAsync(ras);
                            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                            
                            var ocrEngine = OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language("de-DE")) 
                                            ?? OcrEngine.TryCreateFromUserProfileLanguages();
                                            
                            if (ocrEngine != null)
                            {
                                var ocrResult = await ocrEngine.RecognizeAsync(softwareBitmap);
                                string rawOcr = ocrResult.Text;
                                
                                if (!string.IsNullOrWhiteSpace(rawOcr) && rawOcr != lastRawOcr)
                                {
                                    LogSyncWithDice("RAW", rawOcr, ref lastRawForDice);
                                    lastRawOcr = rawOcr;
                                }

                                HashSet<string> currentScannedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                                if (cmbMode.SelectedIndex == 1) 
                                {
                                    string fullText = CleanOcrText(rawOcr, currentScannedWords);
                                    if (!string.IsNullOrEmpty(fullText))
                                    {
                                        rtbCurrentText.Text = fullText;
                                        string deltaText = ExtractDeltaAndUpdateVorzimmer(fullText);
                                        
                                        if (!string.IsNullOrWhiteSpace(deltaText))
                                        {
                                            LogSyncWithDice("BUF", vorzimmer, ref lastBufForDice);
                                            CommitToListViewAndSpeak(deltaText);
                                        }
                                    }
                                }
                                else 
                                {
                                    if (string.IsNullOrWhiteSpace(rawOcr)) return;
                                    
                                    string[] lines = rawOcr.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                    bool addedNew = false;
                                    
                                    foreach (string rawLine in lines)
                                    {
                                        string cleanLine = CleanOcrText(rawLine, currentScannedWords);
                                        if (string.IsNullOrWhiteSpace(cleanLine)) continue;
                                        
                                        if (!seenTexts.Contains(cleanLine))
                                        {
                                            seenTexts.Add(cleanLine);
                                            CommitToListViewAndSpeak(cleanLine);
                                            addedNew = true;
                                        }
                                    }
                                    
                                    if (addedNew) rtbCurrentText.Text = rawOcr;
                                }
                                
                                lastScannedWords = currentScannedWords;
                            }
                        }
                    }
                }
            } 
            catch { }
            finally 
            { 
                isOcrRunning = false; 
                
                if (dropsChanged)
                {
                    dropsChanged = false;
                    _ = Task.Run(() => SaveDrops());
                }
                
                if (customDictChanged)
                {
                    customDictChanged = false;
                    _ = Task.Run(() => SaveCustomDictionary());
                }
            }
        }

        private void ListViewChat_DoubleClick(object sender, EventArgs e)
        {
            if (listViewChat.SelectedItems.Count > 0) 
            { 
                isReadingActive = true; 
                UpdateUI();
                mediaPlayer.Pause(); 
                mediaPlayer.Source = null;
                isCurrentlySpeaking = false;
                ttsQueue.Clear();
                ttsPendingBuffer.Clear();

                int startIndex = listViewChat.SelectedItems[0].Index;
                for (int i = startIndex; i < listViewChat.Items.Count; i++)
                {
                    ttsQueue.Enqueue(listViewChat.Items[i].SubItems[1].Text);
                }
                
                lastAddedToQueue = DateTime.Now;
                ProcessTTSQueue(forceFlush: false); 
            }
        }

        private void BtnStartReading_Click(object sender, EventArgs e)
        {
            if (listViewChat.Items.Count == 0) return;
            isReadingActive = true; 
            UpdateUI();
            
            if (ttsQueue.Count == 0 && ttsPendingBuffer.Count == 0)
            {
                foreach (ListViewItem item in listViewChat.Items) ttsQueue.Enqueue(item.SubItems[1].Text);
            }
            
            if (!isCurrentlySpeaking) ProcessTTSQueue(forceFlush: true);
        }

        private double CalculateDiceCoefficient(string str1, string str2)
        {
            if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2)) return 0.0;
            if (str1 == str2) return 1.0;
            var set1 = new HashSet<string>(); for (int i = 0; i < str1.Length - 1; i++) set1.Add(str1.Substring(i, 2).ToLowerInvariant());
            var set2 = new HashSet<string>(); for (int i = 0; i < str2.Length - 1; i++) set2.Add(str2.Substring(i, 2).ToLowerInvariant());
            int intersection = set1.Intersect(set2).Count();
            if (set1.Count + set2.Count == 0) return 0.0;
            return (2.0 * intersection) / (set1.Count + set2.Count);
        }

        private string ApplySpeechFilters(string rawText)
        {
            string cleanText = Regex.Replace(rawText, @"\b(?=[\w]*[a-zA-Z])(?=[\w]*[0-9])[\w]+\b", "");

            foreach (string word in lbSpeech.Items)
            {
                if (string.IsNullOrWhiteSpace(word)) continue;
                cleanText = Regex.Replace(cleanText, Regex.Escape(word.Trim()), "", RegexOptions.IgnoreCase);
            }

            cleanText = Regex.Replace(cleanText, @"[^a-zA-Z0-9äöüÄÖÜß\s\.,!\?-]", " ");
            return Regex.Replace(cleanText, @"\s+", " ").Trim();
        }

        private string FilterConsecutiveWords(string newText, double threshold, bool debug)
        {
            if (string.IsNullOrWhiteSpace(newText)) return newText;

            string[] words = newText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();

            string wMinus1 = "";
            string wMinus2 = "";
            
            string[] history = lastSpeakForOverlap.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (history.Length >= 1) wMinus1 = Regex.Replace(history[history.Length - 1].ToLower(), @"[^a-z0-9äöüß]", "");
            if (history.Length >= 2) wMinus2 = Regex.Replace(history[history.Length - 2].ToLower(), @"[^a-z0-9äöüß]", "");

            foreach (string rawWord in words)
            {
                string currentClean = Regex.Replace(rawWord.ToLower(), @"[^a-z0-9äöüß]", "");
                if (string.IsNullOrWhiteSpace(currentClean)) 
                {
                    result.Add(rawWord);
                    continue;
                }

                double dice1 = CalculateDiceCoefficient(currentClean, wMinus1);
                double dice2 = CalculateDiceCoefficient(currentClean, wMinus2);

                if (debug)
                {
                    lock(overlapLogLock) overlapDebugLog.Add($"[WORT-TEST] '{currentClean}' | vs N-1 ('{wMinus1}'): {dice1 * 100:0}% | vs N-2 ('{wMinus2}'): {dice2 * 100:0}%");
                }

                if (dice1 >= threshold || dice2 >= threshold)
                {
                    if (debug) lock(overlapLogLock) overlapDebugLog.Add($" ---> [DROP] '{rawWord}' blockiert!");
                }
                else
                {
                    result.Add(rawWord);
                    wMinus2 = wMinus1;
                    wMinus1 = currentClean;
                }
            }

            return string.Join(" ", result);
        }

        private string ProcessDatesAndNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            string datePattern1 = @"(?i)(?:(am|vom|zum|beim|den|dem|ab|seit|der|die|das)\s+)?\b(\d{1,2})\.([a-zäöüß]+|\d{1,2})\.(\d{4})\b";
            string datePattern2 = @"(?i)(?:(am|vom|zum|beim|den|dem|ab|seit|der|die|das)\s+)?\b(\d{4})\.([a-zäöüß]+|\d{1,2})\.(\d{1,2})\b";

            input = Regex.Replace(input, datePattern1, match => EvaluateDateMatch(match, false));
            input = Regex.Replace(input, datePattern2, match => EvaluateDateMatch(match, true));

            string thousandsPattern = @"\b(\d{1,3}(?:\.\d{3})+)\b";
            input = Regex.Replace(input, thousandsPattern, match =>
            {
                string noDots = match.Value.Replace(".", "");
                if (System.Numerics.BigInteger.TryParse(noDots, out System.Numerics.BigInteger num))
                {
                    return ConvertToSpokenNumber(num);
                }
                return match.Value;
            });

            input = Regex.Replace(input, @"\d+", match =>
            {
                if (System.Numerics.BigInteger.TryParse(match.Value, out System.Numerics.BigInteger number))
                {
                    return ConvertToSpokenNumber(number);
                }
                return match.Value;
            });

            return input;
        }

        private string EvaluateDateMatch(Match match, bool yearFirst)
        {
            string prefix = match.Groups[1].Success ? match.Groups[1].Value : "";
            string dayStr = yearFirst ? match.Groups[4].Value : match.Groups[2].Value;
            string monthStr = match.Groups[3].Value;
            string yearStr = yearFirst ? match.Groups[2].Value : match.Groups[4].Value;

            string baseSuffix = "ter";
            if (!string.IsNullOrEmpty(prefix))
            {
                string p = prefix.ToLower();
                if (new[] { "am", "vom", "zum", "beim", "den", "dem", "ab", "seit" }.Contains(p)) baseSuffix = "ten";
                else if (new[] { "der", "die", "das" }.Contains(p)) baseSuffix = "te";
            }

            if (!int.TryParse(dayStr, out int day)) return match.Value;
            string spokenDay = GetOrdinal(day, baseSuffix);

            string spokenMonth = "";
            if (int.TryParse(monthStr, out int month))
            {
                spokenMonth = GetOrdinal(month, baseSuffix);
            }
            else
            {
                string m = monthStr.ToLower();
                if (m.StartsWith("jan")) spokenMonth = "januar";
                else if (m.StartsWith("feb")) spokenMonth = "februar";
                else if (m.StartsWith("mär") || m.StartsWith("mar")) spokenMonth = "märz";
                else if (m.StartsWith("apr")) spokenMonth = "april";
                else if (m.StartsWith("mai")) spokenMonth = "mai";
                else if (m.StartsWith("jun")) spokenMonth = "juni";
                else if (m.StartsWith("jul")) spokenMonth = "juli";
                else if (m.StartsWith("aug")) spokenMonth = "august";
                else if (m.StartsWith("sep")) spokenMonth = "september";
                else if (m.StartsWith("okt")) spokenMonth = "oktober";
                else if (m.StartsWith("nov")) spokenMonth = "november";
                else if (m.StartsWith("dez")) spokenMonth = "dezember";
                else spokenMonth = monthStr; 
            }

            string spokenYear = "";
            if (yearStr.Length == 4 && int.TryParse(yearStr, out int year))
            {
                int century = year / 100;
                int rest = year % 100;
                
                string cStr = ConvertToSpokenNumber(century).ToLower();
                if (cStr == "eins") cStr = "ein"; 
                
                if (rest == 0) 
                {
                    spokenYear = cStr + "hundert";
                } 
                else if (rest < 10 && century >= 20) 
                {
                    spokenYear = cStr + "null" + ConvertToSpokenNumber(rest).ToLower();
                } 
                else 
                {
                    spokenYear = cStr + ConvertToSpokenNumber(rest).ToLower();
                }
            }
            else
            {
                if (System.Numerics.BigInteger.TryParse(yearStr, out System.Numerics.BigInteger y))
                    spokenYear = ConvertToSpokenNumber(y).ToLower();
                else spokenYear = yearStr;
            }

            string prefixStr = string.IsNullOrEmpty(prefix) ? "" : match.Groups[1].Value + " ";
            return $"{prefixStr}{spokenDay}.{spokenMonth}.{spokenYear}";
        }

        private string GetOrdinal(int num, string baseSuffix)
        {
            if (num == 1) return "ers" + baseSuffix;
            if (num == 3) return "drit" + baseSuffix;
            if (num == 7) return "sieb" + baseSuffix;
            if (num == 8) return "ach" + baseSuffix;
            
            string word = ConvertToSpokenNumber(num).ToLower();
            if (word.EndsWith("s") && num == 1) word = "ein"; 
            
            if (num < 20) return word + baseSuffix;
            return word + "s" + baseSuffix;
        }

        private string ConvertToSpokenNumber(System.Numerics.BigInteger number)
        {
            if (number == 0) return "Null";

            string[] twelves = { "", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun", "zehn", "elf", "zwölf" };
            string[] tens = { "", "zehn", "zwanzig", "dreißig", "vierzig", "fünfzig", "sechzig", "siebzig", "achtzig", "neunzig" };
            string[] groups = { "", "tausend", "million", "milliarde", "billion", "billiarde", "trillion", "trilliarde", "quadrillion", "quadrilliarde" };

            string result = "";
            int groupIndex = 0;

            while (number > 0)
            {
                int part = (int)(number % 1000);
                number /= 1000;

                if (part > 0)
                {
                    string partWord = "";
                    int h = part / 100;
                    int tu = part % 100; 
                    int t = tu / 10;
                    int u = tu % 10;

                    if (h > 0) partWord += (h == 1 ? "ein" : twelves[h]) + "hundert";

                    if (tu > 0)
                    {
                        if (tu <= 12)
                        {
                            if (tu == 1)
                            {
                                if (groupIndex == 0 && h == 0) partWord += "eins";
                                else if (groupIndex == 0 && h > 0) partWord += "eins";
                                else if (groupIndex >= 2) partWord += "eine";
                                else partWord += "ein";
                            }
                            else if (tu == 6) partWord += "sechs";
                            else if (tu == 7) partWord += "sieben";
                            else partWord += twelves[tu];
                        }
                        else if (tu < 20)
                        {
                            if (tu == 16) partWord += "sechzehn";
                            else if (tu == 17) partWord += "siebzehn";
                            else partWord += twelves[u] + "zehn";
                        }
                        else
                        {
                            if (u > 0) partWord += (u == 1 ? "ein" : (u == 6 ? "sechs" : (u == 7 ? "sieben" : twelves[u]))) + "und";
                            partWord += tens[t];
                        }
                    }

                    string groupName = groups[groupIndex];
                    if (groupIndex >= 2 && part > 1)
                    {
                        if (groupIndex % 2 == 0) groupName += "en";
                        else groupName += "n";
                    }

                    string separator = (groupIndex >= 1 && result.Length > 0) ? "." : "";
                    result = partWord + groupName + separator + result;
                }
                groupIndex++;
            }

            if (result.Length > 0) result = char.ToUpper(result[0]) + result.Substring(1);

            return result;
        }

        private async void ProcessTTSQueue(bool forceFlush = false)
        {
            if (!isReadingActive || isCurrentlySpeaking) return;

            while (ttsQueue.Count > 0)
            {
                string rawText = ttsQueue.Dequeue();
                string cleanText = ApplySpeechFilters(rawText);

                if (!string.IsNullOrWhiteSpace(cleanText))
                {
                    ttsPendingBuffer.Add(cleanText);
                }
            }

            string combinedSpeech = string.Join(" ", ttsPendingBuffer);
            int wordCount = combinedSpeech.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            bool hasPunctuation = Regex.IsMatch(combinedSpeech, @"[.!?]");
            
            bool isTikTokMode = (cmbMode.SelectedIndex == 0);

            if (isTikTokMode || hasPunctuation || wordCount >= 8 || (forceFlush && ttsPendingBuffer.Count > 0))
            {
                ttsPendingBuffer.Clear();
                
                double threshold = (double)numWordDice.Value / 100.0;
                bool debugEnabled = chkDebugOverlap.Checked;
                
                string finalSpeech = FilterConsecutiveWords(combinedSpeech, threshold, debugEnabled);

                if (!string.IsNullOrWhiteSpace(finalSpeech))
                {
                    LogSyncWithDice("SPK", finalSpeech, ref lastLogSpkStr);
                    
                    string[] combinedWords = (lastSpeakForOverlap + " " + finalSpeech).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (combinedWords.Length > 30) lastSpeakForOverlap = string.Join(" ", combinedWords.Skip(combinedWords.Length - 30));
                    else lastSpeakForOverlap = string.Join(" ", combinedWords);
                    
                    if (cmbVoice1.SelectedItem != null && cmbVoice2.SelectedItem != null)
                    {
                        try
                        {
                            string selectedVoice = voiceToggle ? cmbVoice2.SelectedItem.ToString() : cmbVoice1.SelectedItem.ToString();
                            var voice = Windows.Media.SpeechSynthesis.SpeechSynthesizer.AllVoices.FirstOrDefault(v => v.DisplayName == selectedVoice);
                            if (voice != null) synthesizer.Voice = voice;
                        }
                        catch { } 
                    }
                    voiceToggle = !voiceToggle;

                    finalSpeech = ProcessDatesAndNumbers(finalSpeech);

                    if (chkNoPauses.Checked)
                    {
                        finalSpeech = Regex.Replace(finalSpeech, @"[\.,!\?\-\r\n]", " ");
                        finalSpeech = Regex.Replace(finalSpeech, @"\s+", " ").Trim();
                    }
                    else
                    {
                        finalSpeech = finalSpeech.Replace("\r", " ").Replace("\n", " ");
                    }

                    finalSpeech = finalSpeech.Replace(".", " ");

                    try
                    {
                        isCurrentlySpeaking = true;
                        var stream = await synthesizer.SynthesizeTextToStreamAsync(finalSpeech);
                        mediaPlayer.Source = MediaSource.CreateFromStream(stream, stream.ContentType);
                        mediaPlayer.Play();
                    }
                    catch 
                    { 
                        isCurrentlySpeaking = false; 
                    } 
                }
            }
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            isCurrentlySpeaking = false;
            if (this.InvokeRequired) this.Invoke(new Action(() => ProcessTTSQueue(false)));
            else ProcessTTSQueue(false);
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}