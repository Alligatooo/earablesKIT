﻿using EarablesKIT.Models;
using EarablesKIT.Models.Extentionmodel;
using EarablesKIT.Models.Extentionmodel.Activities;
using EarablesKIT.Models.Extentionmodel.Activities.RunningActivity;
using EarablesKIT.Models.Library;
using EarablesKIT.Resources;
using MediaManager;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

namespace EarablesKIT.ViewModels
{
    class MusicModeViewModel : BaseModeViewModel, INotifyPropertyChanged
    {
        private bool _running = false;
        private bool _musicModeActive = false;
        private IActivityManager _activityManager;

        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsRunning
        {
            set
            {
                _running = value;
                if (_running)
                {
                    CrossMediaManager.Current.Play();
                }
                else
                {
                    CrossMediaManager.Current.Pause();
                }
                //OnPropertyChanged("StartStopLabel");
                OnPropertyChanged();
            }
            get => _running;
        }

        public Command ToggleMusicMode
        {
            get => new Command(() =>
            {
                _musicModeActive = !_musicModeActive;
                if (_musicModeActive)
                {
                    if (!StartActivity()) _musicModeActive = !_musicModeActive;
                }
                else
                {
                    StopActivity();
                }
                //CrossMediaManager.Current.PlayPause();
                OnPropertyChanged(nameof(StartStopLabel));
            });
        }


        public string StartStopLabel
        {
            get => _musicModeActive ? "Stop" : "Start";
        }

        public string CurrentStatusLabel
        {
            get => IsRunning ? AppResources.MusicModeCurrentStatusLabelWalking : AppResources.MusicModeCurrentStatusLabelStanding;
        }


        /// <summary>
        /// Loading the music file and pausing the player.
        /// </summary>
        private async void RestartMusic()
        {
            try
            {
                await CrossMediaManager.Current.Play(_path);
                await CrossMediaManager.Current.Pause();
            } catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private AbstractRunningActivity runningActivity { get; set; }
        private string _path { get; set; }

        public MusicModeViewModel()
        {
            _activityManager = (IActivityManager)ServiceManager.ServiceProvider.GetService(typeof(IActivityManager));
            runningActivity = (AbstractRunningActivity)_activityManager.ActitvityProvider.GetService(typeof(AbstractRunningActivity));

            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "music/ukulele.mp3");
            Directory.CreateDirectory(Path.GetDirectoryName(_path));

            // Copying the resource music file to the MyDocuments Path because the MediaPlayer can't play streams.
            using (BinaryWriter writer = new BinaryWriter(File.Open(_path, FileMode.Create)))
            {
                using (var input = new BinaryReader(AppResources.ukulele_low))
                {
                    while (true)
                    {
                        try
                        {
                            var b = input.ReadByte();
                            writer.Write(b);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }
        }

        public override void OnActivityDone(object sender, ActivityArgs args)
        {
            IsRunning = ((RunningEventArgs)args).Running;
        }

        public override bool StartActivity()
        {
            /* Debug events 
                         if (true)
            {
                Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                {
                    // Do something
                    OnActivityDone(this, new RunningEventArgs(!IsRunning));
                    return true; // True = Repeat again, False = Stop the timer
                });
                //((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
                return true;
            }
            else
            {
                return false;
            }
             */
            if (CheckConnection())
            {
                RestartMusic();
                runningActivity.ActivityDone += OnActivityDone;
                ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StartSampling();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void StopActivity()
        {
            IsRunning = false;
            ((IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection))).StopSampling();
            runningActivity.ActivityDone -= OnActivityDone;
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
