
using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization; 

public class KeysSender
{

    public static void PressTheKey(char ch)
    {
        byte vk = WindowsAPI.VkKeyScan(ch);
        ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);
        WindowsAPI.keybd_event(vk, 0, WindowsAPI.KEYEVENTF_EXTENDEDKEY, 0);
        WindowsAPI.keybd_event(vk, 0, WindowsAPI.KEYEVENTF_KEYUP, 0);
    }

    public static void PressKey(char ch, bool press)
    {
        byte vk = WindowsAPI.VkKeyScan(ch);
        ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);

        if (press)
            KeyDown(scanCode);
        else
            KeyUp(scanCode);
    }


    public static void SendCharUnicode(char ch)
    {
        INPUT[] input = new INPUT[2];
        input[0] = new INPUT();
        input[0].type = WindowsAPI.INPUT_KEYBOARD;
        input[0].ki.wVk = 0;
        input[0].ki.wScan = (ushort)ch;
        input[0].ki.time = 0;
        input[0].ki.dwFlags = WindowsAPI.KEYEVENTF_UNICODE;
        input[0].ki.dwExtraInfo = WindowsAPI.GetMessageExtraInfo();

        input[1] = new INPUT();
        input[1].type = WindowsAPI.INPUT_KEYBOARD;
        input[1].ki.wVk = 0;
        input[1].ki.wScan = (ushort)ch;
        input[1].ki.time = 0;
        input[1].ki.dwFlags = WindowsAPI.KEYEVENTF_UNICODE | WindowsAPI.KEYEVENTF_KEYUP;
        input[1].ki.dwExtraInfo = WindowsAPI.GetMessageExtraInfo();
        WindowsAPI.SendInput(2, input, Marshal.SizeOf(typeof(INPUT)));
    }

  




    public static void KeyDown(ushort scanCode)
    {
        INPUT[] inputs = new INPUT[1];
        inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
        inputs[0].ki.dwFlags = 0;
        inputs[0].ki.wScan = (ushort)(scanCode & 0xff);

        uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
        if (intReturn != 1)
        {
            throw new Exception("Could not send key: " + scanCode);
        }
    }

    public static void KeyUp(ushort scanCode)
    {
        INPUT[] inputs = new INPUT[1];
        inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
        inputs[0].ki.wScan = scanCode;
        inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP;
        uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
        if (intReturn != 1)
        {
            throw new Exception("Could not send key: " + scanCode);
        }
    }


    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
    [DllImport("user32.dll")]
    static extern IntPtr GetKeyboardLayout(uint thread);
    public static CultureInfo GetCurrentKeyboardLayout()
    {
        try
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            uint foregroundProcess = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
            int keyboardLayout = GetKeyboardLayout(foregroundProcess).ToInt32() & 0xFFFF;
            return new CultureInfo(keyboardLayout);
        }
        catch (Exception _)
        {
            return new CultureInfo(1033); // Assume English if something went wrong.
        }
    }


}

      

 
