using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MemoLib
{
    public class Memo
    {
        #region Windows Api

        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd,out Rect lpRect);

        // 显示/隐藏窗口  
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern int CreateRemoteThread(int hWnd, int attrib, int size, int address, int par, int flags,
            int threadid);

        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        private static extern int OpenProcess(
            int dwDesiredAccess,
            bool bInheritHandle,
            int dwProcessId);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        private static extern void CloseHandle
        (
            int hObject
        );

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(
            int hProcess,
            int lpBaseAddress,
            byte[] lpBuffer,
            int dwSize,
            ref int lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(
            int hProcess,
            int lpBaseAddress,
            int[] lpBuffer,
            int dwSize,
            ref int lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(
            int hProcess,
            int lpBaseAddress,
            double[] lpBuffer,
            int dwSize,
            ref int lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(
            int hProcess,
            int lpBaseAddress,
            float[] lpBuffer,
            int dwSize,
            ref int lpNumberOfBytesRead
        );



        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory
        (
            int hProcess,
            int lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            int lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory
        (
            int hProcess,
            int lpBaseAddress,
            int[] lpBuffer,
            int nSize,
            int lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory
        (
            int hProcess,
            int lpBaseAddress,
            double[] lpBuffer,
            int nSize,
            int lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory
        (
            int hProcess,
            int lpBaseAddress,
            float[] lpBuffer,
            int nSize,
            int lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(int hProcess, int lpAddress, int dwSize, int flNewProtect,
            out int lpflOldProtect);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion

        public Memo(Process process)
        {
            Process = process;
            Handle = OpenProcess(0x1F0FFF, false, process.Id);
        }

        public Process Process { get; }

        public int Handle { get; }


        public void Click(int x, int y)
        {
            const int wmLbuttondown = 0x0201;
            const int mouseMove = 0x0200;
            const int wmLbuttonup = 0x0202;
            SendMessage(Process.MainWindowHandle, mouseMove, 0, (y << 16) | x);
            SendMessage(Process.MainWindowHandle, wmLbuttondown, 0, (y << 16) | x);
            SendMessage(Process.MainWindowHandle, wmLbuttonup, 0, (y << 16) | x);

        }

        
        public static Process[] GetProcess(string processName)
        {
            return Process.GetProcessesByName(processName);
        }

        public int ReadInt(int baseAddress, int bytes = 4)
        {
            var bytesRead = 0;
            var ret = new int[1] {0};
            ReadProcessMemory(Handle, baseAddress, ret, bytes, ref bytesRead);
            return ret[0];
        }

        public float ReadFloat(int baseAddress)
        {
            var bytesRead = 0;
            var ret = new float[1] {0};
            ReadProcessMemory(Handle, baseAddress, ret, 4, ref bytesRead);
            return ret[0];
        }

        public double ReadDouble(int baseAddress)
        {
            var bytesRead = 0;
            var ret = new double[1] {0.0};
            ReadProcessMemory(Handle, baseAddress, ret, 8, ref bytesRead);
            return ret[0];
        }

        public string ReadBytes(int baseAddress, int length)
        {
            var bytesRead = 0;
            var ret = new byte[length];
            ReadProcessMemory(Handle, baseAddress, ret, length, ref bytesRead);
            return BitConverter.ToString(ret);
        }


        public string ReadString(int baseAddress, int length)
        {
            var bytesRead = 0;
            var ret = new byte[length];
            ReadProcessMemory(Handle, baseAddress, ret, length, ref bytesRead);
            var zeroIndex = Array.IndexOf(ret, (byte) 0);
            zeroIndex = (zeroIndex == -1) ? ret.Length : zeroIndex;
            return Encoding.Default.GetString(ret, 0, zeroIndex);
        }


        public bool WriteString(int baseAddress, string content)
        {
            var bytes = Encoding.Default.GetBytes(content);
            return WriteProcessMemory(Handle, baseAddress, bytes, bytes.Length, 0);
        }

        public bool WriteBytes(int baseAddress, string bytesStr)
        {
            var bytesStrs = bytesStr.Split('-', ' ');
            var writeBytes = new byte[bytesStrs.Length];
            for (var i = 0; i < bytesStrs.Length; i++)
            {
                writeBytes[i] = Convert.ToByte(bytesStrs[i], 16);
            }

            return WriteProcessMemory(Handle, baseAddress, writeBytes, writeBytes.Length, 0);
        }

        public bool WriteInt(int baseAddress, int value, int bytes = 4)
        {
            var v = new int[1] {value};
            return WriteProcessMemory(Handle, baseAddress, v, bytes, 0);
        }

        public bool WriteFloat(int baseAddress, float value)
        {
            var v = new float[1] {value};
            return WriteProcessMemory(Handle, baseAddress, v, 4, 0);
        }

        public bool WriteDouble(int baseAddress, double value)
        {
            var v = new double[1] {value};
            return WriteProcessMemory(Handle, baseAddress, v, 8, 0);
        }

        public int GetPointer(int baseAddress)
        {
            return ReadInt(baseAddress);
        }

        public int RemoteCall(int addr)
        {
            VirtualProtectEx(Handle, addr, 1024, 0x40, out var ret);
            CreateRemoteThread(Handle, 0, 0, addr, 0, 0, 0);
            return 0;
        }

        public void ChangeProtect(int addr, int lenth = 0x1024)
        {
            VirtualProtectEx(Handle, addr, lenth, 0x40, out var ret);
        }

        public void MoveWindow(int x, int y)
        {
            MoveWindow(Process.MainWindowHandle, x, y, 0, 0, true);
        }

     

        public void ShowHideWindow(bool show)
        {
            var nCmdShow = show ? 1 : 2;
            ShowWindow(Process.MainWindowHandle, nCmdShow);
        }

        public Rect GetWindowRect()
        {
            GetWindowRect(Process.MainWindowHandle,out var rect);
            return rect;
        }


        public void CloseHandle()
        {

        }
        public void Close()
        {
            Process.Close();
            CloseHandle(Handle);
            Console.WriteLine("Memo.Close()");
        }

        ~Memo()
        {
            Console.WriteLine("~Memo()");
        }
    }
}
