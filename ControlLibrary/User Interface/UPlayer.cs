﻿using ControlLibrary.Network;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utility.Musics;
using Utility.Network.Dialog;

namespace ControlLibrary
{
    public partial class UPlayer : UserControl
    {
        public Player player = new Player();

        public List<Music> Playlist = new List<Music>();
        public int PlaylistIndex;

        public Music InPlaying;

        public UPlayer()
        {
            InitializeComponent();
            player.player.PlayStateChange += Player_PlayStateChange;
            UIMusicImage.BackgroundImage = Properties.Resources.No_Cover_Image;
        }

        private void Player_PlayStateChange(int NewState)
        {
            if (NewState == 8)
            {
                if (PlaylistIndex + 1 < Playlist.Count)
                {
                    PlaylistIndex++;
                    NetworkClient.SendObject(new Request(Playlist[PlaylistIndex]));
                }
            }
        }

        private void UIPlay_Click(object sender, EventArgs e)
            => player.player.controls.play();

        private void UIPause_Click(object sender, EventArgs e)
            => player.player.controls.pause();

        private void UITrackbarMusic_Scroll(object sender, EventArgs e)
            => player.player.settings.volume = UITrackbarMusic.Value;

        private void UIBackward_Click(object sender, EventArgs e)
        {
            if (PlaylistIndex - 1 >= 0)
            {
                PlaylistIndex--;
                UIForward.Enabled = false;
                UIBackward.Enabled = false;
                NetworkClient.SendObject(new Request(Playlist[PlaylistIndex]));
            }
        }

        private void UIForward_Click(object sender, EventArgs e)
        {
            if (PlaylistIndex + 1 < Playlist.Count)
            {
                PlaylistIndex++;
                UIForward.Enabled = false;
                UIBackward.Enabled = false;
                NetworkClient.SendObject(new Request(Playlist[PlaylistIndex]));
            }
        }

        public void PlayMusic(Music music)
        {
            InPlaying = music;
            UIPlayingMusic.Text = InPlaying.Title;
            UIArtist.Text = InPlaying.Author.Name;
            UIFormat.Text = InPlaying.Format;
            UIForward.Enabled = true;
            UIBackward.Enabled = true;
            player.PlayMusic(InPlaying);
            try
            {
                UIMusicImage.BackgroundImage = Tags.GetMetaImage(player.player.URL);
            }
            catch
            {
                UIMusicImage.BackgroundImage = Properties.Resources.No_Cover_Image;
            }
        }

        public void Reset()
        {
            UIPlayingMusic.Text = "No music";
            UIArtist.Text = "Artist";
            UIFormat.Text = "Format";
            UIForward.Enabled = false;
            UIBackward.Enabled = false;
            UIMusicImage.BackgroundImage = null;
            player.player.controls.stop();
        }
    }
}
