using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vlc.DotNet.Core;

namespace RR.Player
{
    public partial class Player
    {
        public event EventHandler<VlcMediaPlayerMediaChangedEventArgs> MediaChanged;
        public event EventHandler<VlcMediaPlayerOpeningEventArgs> Opening;
        public event EventHandler<VlcMediaPlayerPausedEventArgs> Paused;
        public event EventHandler<VlcMediaPlayerPlayingEventArgs> Playing;
        public event EventHandler<VlcMediaPlayerPositionChangedEventArgs> PositionChanged;
        public event EventHandler<VlcMediaPlayerStoppedEventArgs> Stopped;
        public event EventHandler<VlcMediaPlayerTimeChangedEventArgs> TimeChanged;
        public event Action<bool> FullScreenChanged;
        public event EventHandler<VlcMediaPlayerMediaChangedEventArgs> MediaFirstTimeToPlay;
        public event EventHandler<VlcMediaPlayerBufferingEventArgs> Buffering;
        public event EventHandler<AudioTrackChangedEventArgs> AudioTrackChanged;
        public event Action FinishPlay;
    }

    public sealed class AudioTrackChangedEventArgs : EventArgs {
        public int TrackIndex { get; private set; }
        public int TrackId { get; private set; }
        public AudioTrackChangedEventArgs(int trackIndex,int trackId){
            TrackIndex = trackIndex;
            TrackId = trackId;
        }
    }
}
