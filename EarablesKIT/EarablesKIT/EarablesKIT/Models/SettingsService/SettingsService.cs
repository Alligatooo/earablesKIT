﻿using EarablesKIT.Models.Library;
using EarablesKIT.ViewModels;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Models.SettingsService
{
    /// <summary>
    /// Class SettingsService implements a Service which handles the settings user, language and
    /// samplingrate. Saves the settings between the sessions.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private const string LANGUAGE_PROPERTY = "Language";
        private const string USER_PROPERTY = "User";
        private const string SAMPLINGRATE_PROPERTY = "Samplingrate";

        private const string STANDARD_USERNAME = "Nutzer";
        private const int STANDARD_STEPLENGTH = 70;
        private const SamplingRate STANDARD_SAMPLINGRATE = SamplingRate.Hz_50;

        private CultureInfo _activeLanguage;

        /// <inheritdoc />
        public CultureInfo ActiveLanguage
        {
            get => _activeLanguage;
            set
            {
                UpdateValue(LANGUAGE_PROPERTY, value.ToString());
                _activeLanguage = value;
            }
        }

        private SamplingRate _samplingRate;

        /// <inheritdoc />
        public SamplingRate SamplingRate
        {
            get => _samplingRate;
            set
            {
                var service = ServiceManager.ServiceProvider.GetService<IEarablesConnection>();

                try
                {
                    service.SampleRate = (int)value;
                }
                catch (Exception e)
                {
                    _exceptionHandler.HandleException(e);
                    return;
                }

                UpdateValue(SAMPLINGRATE_PROPERTY, (int)value);
                IEarablesConnection earablesConnection = (IEarablesConnection) ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
                earablesConnection.SampleRate = (int)value;
                _samplingRate = value;
            }
        }

        private User _activeUser;
        private IExceptionHandler _exceptionHandler;

        /// <inheritdoc />
        public User ActiveUser
        {
            get => _activeUser;
            set
            {
                UpdateValue(USER_PROPERTY, value.ToString());
                _activeUser = value;
            }
        }

        /// <summary>
        /// Constructor of class SettingsServices. Loads all properties or initializes them on the first start of the app.
        /// </summary>
        public SettingsService()
        {
            _exceptionHandler =
                (IExceptionHandler) ServiceManager.ServiceProvider.GetService(typeof(IExceptionHandler));
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!Application.Current.Properties.ContainsKey(LANGUAGE_PROPERTY))
            {
                //Language property doesn't exist
                if (CultureInfo.CurrentUICulture.DisplayName == CultureInfo.GetCultureInfo("de-DE").DisplayName)
                {
                    ActiveLanguage = CultureInfo.CurrentUICulture;
                }
                else
                {
                    ActiveLanguage = CultureInfo.GetCultureInfo("en-US");
                }
            }
            else
            {
                //Language property exists
                try
                {
                    _activeLanguage = CultureInfo.GetCultureInfo((string)Application.Current.Properties[LANGUAGE_PROPERTY]);
                }
                catch (CultureNotFoundException)
                {
                    _exceptionHandler.HandleException(new CultureNotFoundException("Language couldn't be loaded!"));
                    ActiveLanguage = CultureInfo.CurrentUICulture;
                }
            }

            //Load user if he exists. If not, the standarduser get loaded
            if (!Application.Current.Properties.ContainsKey(USER_PROPERTY))
            {
                User standardUser = new User(STANDARD_USERNAME, STANDARD_STEPLENGTH);
                ActiveUser = standardUser;
            }
            else
            {
                try
                {
                    User parsedUser = User.ParseUser((string)Application.Current.Properties[USER_PROPERTY]);
                    if (parsedUser == null)
                    {
                        User standardUser = new User(STANDARD_USERNAME, STANDARD_STEPLENGTH);
                        ActiveUser = standardUser;
                    }
                    else
                    {
                        _activeUser = parsedUser;
                    }
                }
                catch
                {
                    _exceptionHandler.HandleException(new ArgumentException("User failed to load!"));
                    User standardUser = new User(STANDARD_USERNAME, STANDARD_STEPLENGTH);
                    ActiveUser = standardUser;
                }
            }

            //Load SamplingRate
            if (!Application.Current.Properties.ContainsKey(SAMPLINGRATE_PROPERTY))
            {
                //Sampling Rate doesn't exist
                SamplingRate = STANDARD_SAMPLINGRATE;
            }
            else
            {
                //SamplingRate exists
                try
                {
                    _samplingRate = (SamplingRate)Application.Current.Properties[SAMPLINGRATE_PROPERTY];
                }
                catch
                {
                    _exceptionHandler.HandleException(new ArgumentException("Samplingrate failed to load!"));
                    SamplingRate = STANDARD_SAMPLINGRATE;
                }
                UpdateValue(SAMPLINGRATE_PROPERTY, SamplingRate);
                IEarablesConnection earablesConnection = (IEarablesConnection)ServiceManager.ServiceProvider.GetService(typeof(IEarablesConnection));
                earablesConnection.SampleRate = (int)_samplingRate;

            }
        }

        private void UpdateValue(string key, object value)
        {
            Application.Current.Properties[key] = value;
            Application.Current.SavePropertiesAsync();
        }
    }
}