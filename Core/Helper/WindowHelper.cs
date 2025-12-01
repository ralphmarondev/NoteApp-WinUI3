using System;
using Windows.Graphics;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace NoteApp.Core.Helper;

public static class WindowHelper
{
    private static AppWindow GetAppWindow(Window window)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        var id = Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(id);
    }

    public static void SetSize(Window window, int width, int height)
    {
        var appWindow = GetAppWindow(window);
        appWindow.Resize(new SizeInt32(width, height));
    }

    public static void SetMinSize(Window window, int minWidth, int minHeight)
    {
        var hwnd = WindowNative.GetWindowHandle(window);

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Changed += (_, e) =>
            {
                if (e.DidSizeChange)
                {
                    var size = appWindow.Size;
                    if (size.Width < minWidth || size.Height < minHeight)
                    {
                        appWindow.Resize(new SizeInt32(
                            Math.Max(size.Width, minWidth),
                            Math.Max(size.Height, minHeight)));
                    }
                }
            };
        });
    }

    public static void CenterOnScreen(Window window)
    {
        var appWindow = GetAppWindow(window);
        var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);

        var screenBounds = displayArea.OuterBounds;
        var windowSize = appWindow.Size;

        var x = screenBounds.X + (screenBounds.Width - windowSize.Width) / 2;
        var y = screenBounds.Y + (screenBounds.Height - windowSize.Height) / 2;

        appWindow.Move(new PointInt32(x, y));
    }
}