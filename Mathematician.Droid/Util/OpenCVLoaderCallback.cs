using System;

using Android.App;
using Android.Content;
using OpenCV.Android;

namespace Mathematician.Droid.Util
{
    public class OpenCVLoaderCallback : BaseLoaderCallback
    {
        public OpenCVLoaderCallback(Context AppContext) : base(AppContext)
        {
        }

        public override void OnManagerConnected(int p0)
        {
            switch (p0)
            {
                /** OpenCV initialization was successful. **/
                case LoaderCallbackInterface.Success:
                    {
                        /** Application must override this method to handle successful library initialization. **/
                    }
                    break;
                /** OpenCV loader can not start Google Play Market. **/
                case LoaderCallbackInterface.MarketError:
                    {
                        AlertDialog marketError = new AlertDialog.Builder(base.MAppContext).Create();
                        marketError.SetTitle("OpenCV Manager");
                        marketError.SetMessage("Package installation failed");
                        marketError.SetCancelable(false);
                        marketError.SetButton("OK", (s, ev) => Dispose());
                        marketError.Show();
                    }
                    break;
                /** Package installation has been canceled. **/
                case LoaderCallbackInterface.InstallCanceled:
                    {
                        //
                    }
                    break;
                /** Application is incompatible with this version of OpenCV Manager. Possibly, a service update is required. **/
                case LoaderCallbackInterface.IncompatibleManagerVersion:
                    {
                        AlertDialog incompatibleManager = new AlertDialog.Builder(MAppContext).Create();
                        incompatibleManager.SetTitle("OpenCV Manager");
                        incompatibleManager.SetMessage("OpenCV Manager service is incompatible with this app. Try to update it via Google Play.");
                        incompatibleManager.SetCancelable(false);
                        incompatibleManager.SetButton("OK", (s, ev) => Dispose());
                        incompatibleManager.Show();
                    }
                    break;
                /** Other status, i.e. INIT_FAILED. **/
                default:
                    {
                        AlertDialog fail = new AlertDialog.Builder(MAppContext).Create();
                        fail.SetTitle("OpenCV Manager");
                        fail.SetMessage("Package installation failed");
                        fail.SetCancelable(false);
                        fail.SetButton("OK", (s, ev) => Dispose());
                        fail.Show();
                    }
                    break;
            }
        }

        public void OnPackageInstall(int p0, IInstallCallbackInterface p1)
        {
            switch (p0)
            {
                case InstallCallbackInterface.NewInstallation:
                    {
                        AlertDialog installManager = new AlertDialog.Builder(MAppContext).Create();
                        installManager.SetTitle("Package not found");
                        installManager.SetMessage(p1.PackageName + " package was not found! Try to install it?");
                        installManager.SetCancelable(false);
                        installManager.SetButton("Yes", (s, ev) => p1.Install());
                        installManager.SetButton("No", (s, ev) => p1.Cancel());
                        installManager.Show();
                    }
                    break;
                case InstallCallbackInterface.InstallationProgress:
                    {
                        AlertDialog WaitMessage = new AlertDialog.Builder(MAppContext).Create();
                        WaitMessage.SetTitle("OpenCV is not ready");
                        WaitMessage.SetMessage("Installation is in progress. Wait or exit?");
                        WaitMessage.SetCancelable(false); // This blocks the 'BACK' button
                        WaitMessage.SetButton("Wait", (s, ev) => p1.Wait_install());
                        WaitMessage.SetButton("Exit", (s, ev) => p1.Cancel());
                        WaitMessage.Show();
                    }
                    break;
            }

        }
    }
}