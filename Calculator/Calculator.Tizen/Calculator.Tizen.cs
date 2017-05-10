/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd
 *
 * Licensed under the Flora License, Version 1.1 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://floralicense.org/license/
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Tizen.Applications;
using Xamarin.Forms.Platform.Tizen.Native;

using System.Collections.Generic;

namespace Calculator.Tizen
{
    /// <summary>
    /// Calculator application main entry point.
    /// </summary>
    /// <remarks>
    /// This main entry checks device orientation and let Calculator implementation knows which layout should be displayed.
    /// </remarks>
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        /// <summary>
        /// A Calculator App instance.</summary>
        private Calculator app;

        /// <summary>
        /// A AppResourcePath which is used by making full path of the app resources.</summary>
        public static string AppResourcePath
        {
            get;
            private set;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            AppResourcePath = DirectoryInfo.Resource;
            DebuggingPort.MainWindow = MainWindow;
            app = new Calculator(IsLandscape());
            LoadApplication(app);

            try
            {
                MobileCenter.LogLevel = LogLevel.Verbose;
                MobileCenter.Configure("6825bcfe-3582-4d4a-b3a7-fe06154414a4");
                MobileCenter.Start(typeof(Analytics));
            }
            catch (System.Exception exc)
            {
                MobileCenterLog.Debug(MobileCenterLog.LogTag, $"{exc}");
            }

            // Registration for device orientation changing detection.
            MainWindow.RotationChanged += (s, e) =>
            {
                if (IsLandscape())
                {
                    try
                    {
                        Analytics.TrackEvent("Mode Changed", new Dictionary<string, string> { { "mode", "Scientific" } });

                    }
                    catch (System.Exception exc)
                    {
                         MobileCenterLog.Debug(MobileCenterLog.LogTag, $"{exc}");
                    }
                    app.OnOrientationChanged(AppOrientation.Landscape);
                }
                else
                {
                    try
                    {
                        Analytics.TrackEvent("Mode Changed", new Dictionary<string, string> { { "mode", "Regular" } });

                    }
                    catch (System.Exception exc)
                    {
                         MobileCenterLog.Debug(MobileCenterLog.LogTag, $"{exc}");
                    }
                    app.OnOrientationChanged(AppOrientation.Portrait);
                }
            };
        }

        /// <summary>
        /// Checking whether current device orientation is landscape. </summary>
        /// <returns>
        /// true : Landscape orientation, false : Portrait orientation. </returns>
        private bool IsLandscape()
        {
            return (MainWindow.CurrentOrientation == DisplayOrientations.Landscape ||
                MainWindow.CurrentOrientation == DisplayOrientations.LandscapeFlipped);
        }

        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args"> A list of command line arguments.</param>
        static void Main(string[] args)
        {
            var program = new Program();
            // DebuggingPort is registering for the Logging and Pop up.
            global::Xamarin.Forms.DependencyService.Register<DebuggingPort>();
            global::Xamarin.Forms.Platform.Tizen.Forms.Init(program);
            program.Run(args);
        }
    }
}
