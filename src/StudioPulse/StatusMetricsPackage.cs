using Microsoft.VisualBasic.Devices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Process = System.Diagnostics.Process;
using Timer = System.Timers.Timer;

namespace StudioPulse
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0")]
    [Guid(StatusInfoIdentifiers.PackageId)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids80.EmptySolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(StatusInfoSettingsPage), "StatusBar Info", "General", 0, 0, true)]
    public sealed class StatusMetricsPackage : AsyncPackage
    {
        private Timer refreshTimer;
        private Process ideProcess;
        private StatusMetricsControl infoControl;

        private PerformanceCounter totalCpuCounter;
        private PerformanceCounter totalRamCounter;

        private volatile bool disposed;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Debug.WriteLine($"Entering InitializeAsync() of: {this}");

            await base.InitializeAsync(cancellationToken, progress);
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            InitExt();
        }

        private void InitExt()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // 初始化计时器、性能计数器和状态栏控件。
            Debug.WriteLine("Init function loaded");

            refreshTimer = new Timer(1000);
            refreshTimer.Elapsed += RefreshTimerElapsed;

            ideProcess = Process.GetCurrentProcess();
            ideProcess.InitCpuUsage();

            totalCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            totalRamCounter = new PerformanceCounter("Memory", "Available Bytes");

            infoControl = new StatusMetricsControl((long)(new ComputerInfo()).TotalPhysicalMemory);

            new StatusBarHost(Application.Current.MainWindow).InjectControl(infoControl);

            if (GetDialogPage(typeof(StatusInfoSettingsPage)) is StatusInfoSettingsPage optionsPage)
                infoControl.Format = optionsPage.Format;

            refreshTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            // Visual Studio 关闭时释放所有系统资源，避免计时器继续回调。
            if (disposing && !disposed)
            {
                disposed = true;

                if (refreshTimer != null)
                {
                    refreshTimer.Stop();
                    refreshTimer.Dispose();
                }

                totalCpuCounter?.Dispose();
                totalRamCounter?.Dispose();
                ideProcess?.Dispose();
            }

            base.Dispose(disposing);
        }

        public void OptionUpdated(string pName, object pValue)
        {
            Debug.WriteLine($"Get option: {pName}");

            switch (pName)
            {
                case "Format":
                    infoControl.Format = (string)pValue;
                    break;

                case "Interval":
                    refreshTimer.Interval = (int)pValue;
                    break;

                case "UseFixedWidth":
                    infoControl.UseFixedWidth = (bool)pValue;
                    break;

                case "FixedWidth":
                    infoControl.FixedWidth = (int)pValue;
                    break;

                default:
                    Debug.WriteLine($"Error nonexsist option: {pName}");
                    break;
            }
        }

        private void RefreshTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!disposed)
            {
                _ = JoinableTaskFactory.RunAsync(UpdateInfoBarAsync);
            }
        }

        private async Task UpdateInfoBarAsync()
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            if (disposed)
            {
                return;
            }

            // 性能采样在 UI 线程统一提交，保证 WPF 控件不会跨线程更新。
            infoControl.CpuUsage = (int)(ideProcess.GetCpuUsage() * 100);
            infoControl.RamUsage = ideProcess.WorkingSet64;
            infoControl.TotalCpuUsage = (int)totalCpuCounter.NextValue();
            infoControl.FreeRam = totalRamCounter.NextSample().RawValue;
        }
    }
}