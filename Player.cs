using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Vlc.DotNet.Core;

namespace RR.Player
{
    public partial class Player : UserControl
    {
        private string _vlcpath;
        public bool AllowFullScreen{get;set;}
        public bool IsFullScreen{get;set;}
        private Panel fullScreenPanel;
        private Size parentFormOrgSize;
        private Point parentFormOrgLocation;
        private Control orgParent;
        private Form parentForm;
        public Player(string vlcPath,bool allowFullScreen=true)
        {
            _vlcpath = vlcPath;
            AllowFullScreen=allowFullScreen;
            Init();
        }

        public Player(bool allowFullScreen=true) {
            _vlcpath = AppDomain.CurrentDomain.BaseDirectory + "vlc";
            AllowFullScreen = allowFullScreen;
            Init();
        }

        private void Init() {
            InitializeComponent();
            fullScreenPanel = new Panel { Location=new Point(0,0)};
        }

        public Vlc.DotNet.Forms.VlcControl VLCPlayer
        {
            get
            {
                return this.vlcplayer;
            }
        }
        //如果mask使用透明panel的话，显示Logo的情况下切换全屏时图片跳动严重
        public PictureBox Mask
        {
            get
            {
                return this.mask;
            }
        }

        private void vlcplayer_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new System.IO.DirectoryInfo(_vlcpath);
        }

        private void mask_DoubleClick(object sender, EventArgs e)
        {
            if (AllowFullScreen) {
                ToggleFullScreen();
            }
        }

        public void ToggleFullScreen()
        {
            if (AllowFullScreen)
            {
                if (!IsFullScreen)
                {
                    IsFullScreen = true;
                    //记录原尺寸各位置
                    parentFormOrgSize = parentForm.Size;
                    parentFormOrgLocation = parentForm.Location;
                    //全屏
                    parentForm.Location = new Point(0, 0);
                    parentForm.Width = Screen.PrimaryScreen.Bounds.Width;
                    parentForm.Height = Screen.PrimaryScreen.Bounds.Height;
                    //置顶
                    parentForm.TopMost = true;

                    parentForm.Controls.Add(fullScreenPanel);
                    fullScreenPanel.BringToFront();
                    fullScreenPanel.Width = parentForm.Width;
                    fullScreenPanel.Height = parentForm.Height;

                    orgParent.Controls.Remove(this);
                    fullScreenPanel.Controls.Add(this);
                }
                else
                {
                    IsFullScreen = false;
                    //还原
                    parentForm.Location = parentFormOrgLocation;
                    parentForm.Width = parentFormOrgSize.Width;
                    parentForm.Height = parentFormOrgSize.Height;
                    parentForm.TopMost = false;

                    fullScreenPanel.Controls.Remove(this);
                    orgParent.Controls.Add(this);
                    parentForm.Controls.Remove(fullScreenPanel);
                }
                if (FullScreenChanged != null)
                    FullScreenChanged(IsFullScreen);
            }
        }

        private void Player_Load(object sender, EventArgs e)
        {
            this.orgParent = this.Parent;
            this.parentForm = this.ParentForm;
        }

        public void SetLogo(Image img=null)
        {
            mask.BackgroundImage = img;
        }

        public VlcMedia CurrentMedia
        {
            get
            {
                return vlcplayer.GetCurrentMedia();
            }
        }
        public TimeSpan Duration
        {
            get
            {
                if (CurrentMedia != null)
                    return CurrentMedia.Duration;
                else
                    return TimeSpan.FromSeconds(0);
            }
        }
        public long PlayTime
        {
            get
            {
                return vlcplayer.Time;
            }
            set
            {
                vlcplayer.Time = value;
                Play();
            }
        }
        public float PlayPosition {
            get {
                return vlcplayer.Position;
            }
            set {
                vlcplayer.Position = value < 1 ? value : 1;
                Play();
            }
        }
        public bool IsPlaying
        {
            get
            {
                return vlcplayer.IsPlaying;
            }
        }

        public IAudioManagement Audio{
            get { return vlcplayer.Audio; }
        }
        public List<TrackDescription> AudioTracks {
            get {return vlcplayer.Audio.Tracks.All.Where(t => t.ID >= 0).ToList(); }
        }

        /// <summary>
        /// 按顺序改变音轨
        /// </summary>
        public void ChangeAudioTrack(){

            var tracks = AudioTracks;
            int current = AudioTrackIndex;
            if (tracks.Count > 1) {
                if (current >= tracks.Count - 1)
                {
                    AudioTrackIndex = 0;
                }
                else {
                    AudioTrackIndex = current + 1;
                }
            }
        }

        /// <summary>
        /// 获取或设置音轨
        /// </summary>
        public TrackDescription AudioTrack{ 
            get{return vlcplayer.Audio.Tracks.Current;}
            set{
                vlcplayer.Audio.Tracks.Current=value;
                if (AudioTrackChanged != null)
                    this.Invoke(AudioTrackChanged, this, new AudioTrackChangedEventArgs(AudioTrackIndex,AudioTrackId));
            }
        }
        /// <summary>
        /// 获取或设置音轨序号
        /// </summary>
        public int AudioTrackIndex
        {
            get {
                var current = AudioTrack;
                if (current == null)
                    return 0;
                else {
                    int i=0;
                    foreach (TrackDescription d in AudioTracks) {
                        if (current.ID == d.ID) {
                            break;
                        }
                        i++;
                    }
                    return i;
                }
            }
            set {
                var tracks = AudioTracks;
                if (value < 0)
                    value = 0;
                else if(value>0){ 
                    if (tracks.Count() == 0) {
                        value = 0;
                    }
                    else if (value + 1 > tracks.Count()) {
                        value = tracks.Count() - 1;
                    }
                }
                if (tracks.Count() > 0)
                {
                    AudioTrack = tracks[value];
                }
            }
        }
        /// <summary>
        /// 获取或设置音轨Id
        /// </summary>
        public int AudioTrackId {
            get {
                var current = AudioTrack;
                if (current != null)
                    return current.ID;
                else
                    return -1;
            }
            set {
                var tracks=AudioTracks;
                foreach (TrackDescription d in AudioTracks)
                {
                    if (d.ID==value)
                    {
                        AudioTrack = d;
                    }
                }
            }
        }


        #region action
        //play
        public void Play() {
            this.vlcplayer.Play();
        }
        public void Play(FileInfo file, int audioTrackIndex = 0, params string[] options)
        {
            this.SetMedia(file,audioTrackIndex, options);
            this.Play();
        }
        public void Play(Uri uri, int audioTrackIndex = 0, params string[] options)
        {
            this.SetMedia(uri,audioTrackIndex, options);
            this.Play();
        }
        public void Play(string mrl, int audioTrackIndex = 0, params string[] options)
        {
            this.SetMedia(mrl,audioTrackIndex, options);
            this.Play();
        }
        public void Play(Stream stream, int audioTrackIndex = 0, params string[] options)
        {
            this.SetMedia(stream,audioTrackIndex, options);
            this.Play();
        }

        private string[] ProcOptions(int audioTrackIndex = 0, params string[] options)
        {
            audioTrackIndex = audioTrackIndex < 0 ? 0 : audioTrackIndex;
            defAudioTrackIndex = audioTrackIndex;
            if (options == null)
            {
                options = new string[] { ":audio-track=" + audioTrackIndex };
            }
            else {
                List<string> l = options.ToList();
                l.Add(":audio-track=" + audioTrackIndex);
                options = l.ToArray();
            }
            return options;
        }
        //setmedia
        private int defAudioTrackIndex = 0;
        public void SetMedia(FileInfo file,int audioTrackIndex=0, params string[] options)
        {
            options = ProcOptions(audioTrackIndex, options);
            this.vlcplayer.SetMedia(file, options);
            //this.vlcplayer.GetCurrentMedia().Parse();
        }

        public void SetMedia(Uri file, int audioTrackIndex = 0, params string[] options)
        {
            defAudioTrackIndex = audioTrackIndex;
            options = ProcOptions(audioTrackIndex, options);
            this.vlcplayer.SetMedia(file, options);
            //.vlcplayer.GetCurrentMedia().Parse();
        }

        public void SetMedia(string mrl, int audioTrackIndex = 0, params string[] options)
        {
            options = ProcOptions(audioTrackIndex, options);
            this.vlcplayer.SetMedia(mrl, options);
            //this.vlcplayer.GetCurrentMedia().Parse();
        }

        public void SetMedia(Stream stream, int audioTrackIndex = 0, params string[] options)
        {
            options = ProcOptions(audioTrackIndex, options);
            this.vlcplayer.SetMedia(stream, options);
            //this.vlcplayer.GetCurrentMedia().Parse();
        } 

        public void Pause() {
            vlcplayer.Pause();
        }
        public void Stop()
        {
            vlcplayer.Stop();
        }
        #endregion

        #region event
        private bool firstTimeToPlay { get; set; }
        private void vlcplayer_MediaChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs e)
        {
            lastplaytime = 10;
            firstTimeToPlay = true;
            e.NewMedia.Parse();
            if (this.MediaChanged != null)
                this.Invoke(MediaChanged, this, e);
        }
        private void vlcplayer_Opening(object sender, Vlc.DotNet.Core.VlcMediaPlayerOpeningEventArgs e)
        {
            if (this.Opening != null)
                this.Invoke(Opening,this, e);
        }
        private void vlcplayer_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            SetTimerEnabled(false);
            if (this.Paused != null)
                this.Invoke(Paused,this, e);
        }
        private void vlcplayer_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            SetTimerEnabled(true);
            if (this.Playing != null)
                this.Invoke(Playing,this, e);
            if (firstTimeToPlay) {
                var tracks = AudioTracks;
                if (defAudioTrackIndex > 0 && defAudioTrackIndex + 1 > tracks.Count) {
                    int ti = tracks.Count == 0 ? 0 : tracks.Count - 1;
                    AudioTrackIndex = ti;
                    defAudioTrackIndex = ti;
                }
                if (this.MediaFirstTimeToPlay != null)
                    this.Invoke(MediaFirstTimeToPlay,this,new VlcMediaPlayerMediaChangedEventArgs(CurrentMedia));
            }
            firstTimeToPlay = false;
        }
        private void vlcplayer_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            if (this.PositionChanged != null)
                this.Invoke(PositionChanged,this, e);
        }
        private void vlcplayer_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            SetTimerEnabled(false);
            if (this.Stopped != null)
            {
                this.Invoke(Stopped, this, e);
            }
        }
        private void vlcplayer_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            if (this.TimeChanged != null)
                this.Invoke(TimeChanged,this, e);
        }
        private void vlcplayer_Buffering(object sender, VlcMediaPlayerBufferingEventArgs e)
        {
            if(this.Buffering!=null)
                this.Invoke(Buffering, this, e);
        }
        #endregion

        private void SetTimerEnabled(bool e) {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetTimerEnabled), e);
            else
                timer1.Enabled = e;
        }

        long lastplaytime = -10,currentplaytime;
        private void timer1_Tick(object sender, EventArgs e)
        {
            currentplaytime = PlayTime;
            if (currentplaytime>0 && currentplaytime/1000 > Duration.Seconds-1 && currentplaytime == lastplaytime)
            {
                Stop();
                if (FinishPlay != null)
                    this.Invoke(FinishPlay);
            }
            lastplaytime = currentplaytime;
        }
    }
}
