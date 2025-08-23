using System;
using LibVLCSharp.WinForms; // for VideoView (render surface only)

namespace GT_Medical.Abstractions
{
    /// <summary>
    /// UI-only video surface that exposes a VideoView for rendering,
    /// raises user-intent events (play/pause/seek/volume),
    /// and accepts state updates from the backend (VideoPlayer).
    /// </summary>
    public interface IVideoSurfaceUi
    {
        // The render surface; the service will assign MediaPlayer to it.
        VideoView VideoSurface { get; }

        // === UI -> Service events (user interactions) ===
        event Action TogglePlayPauseRequested;
        event Action ReplayRequested;
        event Action FullscreenToggleRequested;
        event Action<int> VolumeChanged;      // 0..100
        event Action<float> SeekRequested;    // 0..1
        event EventHandler VideoSurfaceInitialized;    // 0..1
        void NotifyFinishInitialization();
        // === Service -> UI state updates ===
        void SetPlaybackState(bool isPlaying, long currentMs, long lengthMs, int volume, float position01);

        // Show overlay briefly (e.g., after user input)
        void ShowOverlayTemporarily();
    }
}

