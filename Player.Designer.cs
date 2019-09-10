namespace RR.Player
{
    partial class Player
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.vlcplayer = new Vlc.DotNet.Forms.VlcControl();
            this.mask = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.vlcplayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mask)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcplayer
            // 
            this.vlcplayer.BackColor = System.Drawing.Color.Black;
            this.vlcplayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcplayer.Location = new System.Drawing.Point(0, 0);
            this.vlcplayer.Name = "vlcplayer";
            this.vlcplayer.Size = new System.Drawing.Size(712, 429);
            this.vlcplayer.Spu = -1;
            this.vlcplayer.TabIndex = 0;
            this.vlcplayer.Text = "vlcControl1";
            this.vlcplayer.VlcLibDirectory = null;
            this.vlcplayer.VlcMediaplayerOptions = null;
            this.vlcplayer.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcplayer_VlcLibDirectoryNeeded);
            this.vlcplayer.Buffering += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerBufferingEventArgs>(this.vlcplayer_Buffering);
            this.vlcplayer.MediaChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs>(this.vlcplayer_MediaChanged);
            this.vlcplayer.Opening += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerOpeningEventArgs>(this.vlcplayer_Opening);
            this.vlcplayer.Paused += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs>(this.vlcplayer_Paused);
            this.vlcplayer.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.vlcplayer_Playing);
            this.vlcplayer.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(this.vlcplayer_PositionChanged);
            this.vlcplayer.TimeChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs>(this.vlcplayer_TimeChanged);
            this.vlcplayer.Stopped += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs>(this.vlcplayer_Stopped);
            // 
            // mask
            // 
            this.mask.BackColor = System.Drawing.Color.Transparent;
            this.mask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mask.Location = new System.Drawing.Point(0, 0);
            this.mask.Name = "mask";
            this.mask.Size = new System.Drawing.Size(712, 429);
            this.mask.TabIndex = 1;
            this.mask.TabStop = false;
            this.mask.DoubleClick += new System.EventHandler(this.mask_DoubleClick);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.vlcplayer.Controls.Add(this.mask);
            this.Controls.Add(this.vlcplayer);
            this.Name = "Player";
            this.Size = new System.Drawing.Size(712, 429);
            this.Load += new System.EventHandler(this.Player_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vlcplayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mask)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcplayer;
        private System.Windows.Forms.PictureBox mask;
        private System.Windows.Forms.Timer timer1;
    }
}
