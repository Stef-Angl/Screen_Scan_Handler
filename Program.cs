using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public class ScreenScanner
{


    // Import necessary Windows API functions and constants
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern int GetPixel(IntPtr hdc, int x, int y);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
   
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const int MOUSEEVENTF_RIGHTUP = 0x10;
    public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
    public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
    public const int QKEY = 0x51; // Key Q 
    public const int WKEY = 0x57; // Key W 
    public const int EKEY = 0x45; // Key E 
    public const int SKEY = 0x53; // Key S 
    public const int SPACEBAR = 0x20; // espaço
    public const int PLAYERSTATSX = 1574; // pixel status of player with no addon
    public const int PLAYERSTATSY = 470; // pixel status of player with no addon
    public const int RANGEX = 1662; // pixel status of player with no addon
    public const int RANGEY = 6003; // pixel status of player with no addon
    public const int FACINGX = 1577; // pixel status of player with no addon
    public const int FACINGY = 715; // pixel status of player with no addon
    public const int ONE = 0x31; // Key 1 
    public const int TWO = 0x32; // Key 2 
    public const int THREE = 0x33; // Key 3 
    public const int FOUR = 0x34; // Key 4 
    public const int FIVE = 0x35; // Key 5 
    public const int SIX = 0x36; // Key 6 
    public const int SEVEN = 0x37; // Key 7 
    public const int EIGHT = 0x38; // Key 8 
    public const int NINE = 0x39; // Key 9 
    public const int ZERO = 0x30; // Key 0 
    public const int N1 = 0x61; // Numpad key 1 
    public const int N2 = 0x62; // Numpad key 2 
    public const int N3 = 0x63; // Numpad Key 3 
    public const int N4 = 0x64; // Numpad Key 4 
    public const int N5 = 0x65; // Numpad Key 5 
    public const int N6 = 0x66; // Numpad Key 6 
    public const int N7 = 0x67; // Numpad Key 7 
    public const int N8 = 0x68; // Numpad Key 8 
    public const int N9 = 0x69; // Numpad Key 9 
    public const int N0 = 0x60; // Numpad Key 0

    public static bool IsRedColor(int r, int g, int b)
    {
        // Check if the color falls within the range of red shades
        return r >= 200 && g <= 50 && b <= 50;
    }

    public static void SendKeyForDuration(Keys key, int durationMilliseconds)
    {
        SendKeys.Send(key.ToString());
        Thread.Sleep(durationMilliseconds);
        SendKeys.SendWait(key.ToString());
    }
    public static void wait(int milliseconds) // Wait in number of miliseconds. 
    {
        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds <= milliseconds)
        {
            Application.DoEvents();
        }
    }
    public static void FocusGame() // Focus game window
    {
        var prc = Process.GetProcessesByName("Game");
        if (prc.Length > 0)
        {
            SetForegroundWindow(prc[0].MainWindowHandle);
        }
        else
            MessageBox.Show("Game window not found");
        
    }
    public static void PressKey(byte key, int time = 50) // it will send coded key, and keep it pressed for number of miliseconds in argument 2, 50 miliseconds will be default. 
    {
        FocusGame(); // this is the focusGame method above
        if (time != 2) keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0);
        if (time != 2) wait(time); // this is wait method above 
        if (time > 0) keybd_event(key, 0, KEYEVENTF_KEYUP, 0); //Release Key 
    }
    
    public static void Main()
    {
        // Get the dimensions of the primary display
        int screenWidth = GetSystemMetrics(SM_CXSCREEN);
        int screenHeight = GetSystemMetrics(SM_CYSCREEN);

        // Calculate the center coordinates
        int centerX = screenWidth / 2;
        int centerY = screenHeight / 2;

        // Get the pixel color at the center coordinates
        IntPtr hdc = GetDC(IntPtr.Zero);
        int pixel = GetPixel(hdc, centerX, centerY);
        ReleaseDC(IntPtr.Zero, hdc);

        // Extract RGB values from the pixel
        int pixelColor = (int)(pixel & 0x00FFFFFF);
        int pixelR = (int)(pixelColor & 0xFF);
        int pixelG = (int)((pixelColor >> 8) & 0xFF);
        int pixelB = (int)((pixelColor >> 16) & 0xFF);

        bool isRedColor = ScreenScanner.IsRedColor(pixelR, pixelG, pixelB);

        if (isRedColor)
        {
            Console.WriteLine("Solid red color detected at the center of the screen!");
            PressKey(WKEY, 50);
        }
        else
        {
            Console.WriteLine("No solid red color detected at the center of the screen.");
        }
    }

   
}
