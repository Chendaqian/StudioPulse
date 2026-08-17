using Microsoft.VisualStudio.Shell;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudioPulse
{
    /// <summary>
    /// 查找 VS 状态栏并挂载扩展控件
    /// </summary>
    internal class StatusBarHost
    {
        private readonly Window mainWindow;

        private Panel panel;

        public StatusBarHost(Window pMainWindow)
        {
            mainWindow = pMainWindow;
            FindStatusBar();
        }

        private static DependencyObject FindChild(DependencyObject parent, string childName)
        {
            if (parent == null)
            {
                return null;
            }

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                {
                    return frameworkElement;
                }

                child = FindChild(child, childName);

                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        private void FindStatusBar()
        {
            // VS 内部状态栏没有公开的自定义控件 API，只能从可视树中定位容器。
            FrameworkElement frameworkElement = FindChild(mainWindow, "StatusBarContainer") as FrameworkElement;

            if (frameworkElement != null)
            {
                panel = frameworkElement.Parent as DockPanel;
            }
        }

        private void RefindStatusBar()
        {
            if (panel == null)
            {
                FindStatusBar();
            }
        }

        public void InjectControl(FrameworkElement pControl)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // Dock.Right 保持原状态栏布局，索引 1 将控件放在右侧信息区域。
            RefindStatusBar();

            if (panel != null)
            {
                pControl.SetValue(DockPanel.DockProperty, Dock.Right);
                panel.Children.Insert(1, pControl);
            }
        }

        public bool IsInjected(FrameworkElement pControl)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            RefindStatusBar();
            return panel?.Children.Contains(pControl) == true;
        }

        public void UninjectControl(FrameworkElement pControl)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            RefindStatusBar();
            panel?.Children.Remove(pControl);
        }
    }
}