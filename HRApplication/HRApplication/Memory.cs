using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

public class Memory
{
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        VMRead = 0x0010,
        VMWrite = 0x0020,
        VMOperation = 0x0008
    }

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("psapi.dll")]
    public static extern bool EnumProcessModules(IntPtr hProcess, [Out] IntPtr[] lphModule, uint cb, out uint lpcbNeeded);

    private IntPtr processHandle = IntPtr.Zero;

    // Add this struct
    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    [DllImport("kernel32.dll")]
    private static extern int VirtualQuery(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

    public bool AttachToProcessById(int processId)
    {
        try
        {
            // Request higher privileges
            processHandle = OpenProcess(ProcessAccessFlags.All, false, processId);

            if (processHandle == IntPtr.Zero)
            {
                // Try with lower rights as fallback
                processHandle = OpenProcess(ProcessAccessFlags.VMRead | ProcessAccessFlags.VMWrite | ProcessAccessFlags.VMOperation, false, processId);
            }

            return processHandle != IntPtr.Zero;
        }
        catch
        {
            return false;
        }
    }

    public byte[] ReadMemory(IntPtr address, int size)
    {
        byte[] buffer = new byte[size];
        ReadProcessMemory(processHandle, address, buffer, size, out _);
        return buffer;
    }

    public bool WriteMemory(IntPtr address, byte[] value)
    {
        return WriteProcessMemory(processHandle, address, value, value.Length, out _);
    }

    public bool WriteMemoryWithProtection(IntPtr address, byte[] value)
    {
        try
        {
            // Change memory protection to writable
            uint oldProtect;
            VirtualProtectEx(processHandle, address, (uint)value.Length, 0x40, out oldProtect); // PAGE_EXECUTE_READWRITE

            bool result = WriteProcessMemory(processHandle, address, value, value.Length, out _);

            // Restore original protection
            VirtualProtectEx(processHandle, address, (uint)value.Length, oldProtect, out _);

            return result;
        }
        catch { return false; }
    }

    [DllImport("kernel32.dll")]
    private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

    // Value Scanner
    public IntPtr FindValue(int valueToFind, long startAddress = 0x00400000, long maxSize = 0x8000000)
    {
        byte[] buffer = new byte[0x100000];

        try
        {
            for (long i = 0; i < maxSize; i += buffer.Length)
            {
                IntPtr current = (IntPtr)(startAddress + i);
                if (!ReadProcessMemory(processHandle, current, buffer, buffer.Length, out _))
                    continue;

                for (int j = 0; j < buffer.Length - 4; j += 4)
                {
                    if (BitConverter.ToInt32(buffer, j) == valueToFind)
                        return (IntPtr)(current.ToInt64() + j);
                }
            }
        }
        catch { }

        return IntPtr.Zero;
    }

    // String Scanner (ASCII + Unicode)
    public List<IntPtr> FindAllStrings(string textToFind)
    {
        List<IntPtr> results = new List<IntPtr>();
        byte[] buffer = new byte[0x200000]; // 2MB chunks

        byte[] asciiBytes = Encoding.ASCII.GetBytes(textToFind);
        byte[] unicodeBytes = Encoding.Unicode.GetBytes(textToFind);

        try
        {
            IntPtr address = IntPtr.Zero;
            MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();

            while (VirtualQuery(address, out mbi, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0)
            {
                if (mbi.State == 0x1000) // Committed memory
                {
                    long regionSize = mbi.RegionSize.ToInt64();
                    long current = mbi.BaseAddress.ToInt64();

                    for (long i = 0; i < regionSize; i += buffer.Length)
                    {
                        IntPtr currAddr = new IntPtr(current + i);

                        if (!ReadProcessMemory(processHandle, currAddr, buffer, buffer.Length, out _))
                            continue;

                        for (int j = 0; j < buffer.Length - Math.Max(asciiBytes.Length, unicodeBytes.Length); j++)
                        {
                            // Check ASCII
                            bool asciiMatch = true;
                            for (int k = 0; k < asciiBytes.Length; k++)
                            {
                                if (buffer[j + k] != asciiBytes[k])
                                {
                                    asciiMatch = false;
                                    break;
                                }
                            }
                            if (asciiMatch)
                            {
                                results.Add(new IntPtr(currAddr.ToInt64() + j));
                                continue;
                            }

                            // Check Unicode
                            bool unicodeMatch = true;
                            for (int k = 0; k < unicodeBytes.Length; k++)
                            {
                                if (buffer[j + k] != unicodeBytes[k])
                                {
                                    unicodeMatch = false;
                                    break;
                                }
                            }
                            if (unicodeMatch)
                                results.Add(new IntPtr(currAddr.ToInt64() + j));
                        }
                    }
                }

                address = new IntPtr(mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64());
            }
        }
        catch { }

        return results;
    }

    public void Detach()
    {
        if (processHandle != IntPtr.Zero)
            CloseHandle(processHandle);
    }

    //this is working
    //public List<IntPtr> FindAllValues(long valueToFind, bool useFloat = false)
    //{
    //    List<IntPtr> results = new List<IntPtr>();
    //    byte[] buffer = new byte[0x200000]; // 2MB chunks

    //    try
    //    {
    //        // Get the actual base address of the main module
    //        IntPtr baseAddr = GetMainModuleBase();

    //        long start = baseAddr.ToInt64();
    //        long maxScan = start + 0x80000000; // Scan up to 2GB from base

    //        for (long i = 0; i < maxScan; i += buffer.Length)
    //        {
    //            IntPtr current = new IntPtr(start + i);

    //            if (!ReadProcessMemory(processHandle, current, buffer, buffer.Length, out _))
    //                continue;

    //            for (int j = 0; j < buffer.Length - 8; j += 4)
    //            {
    //                if (useFloat)
    //                {
    //                    float val = BitConverter.ToSingle(buffer, j);
    //                    if (Math.Abs(val - valueToFind) < 5.0f)
    //                    {
    //                        results.Add(new IntPtr(current.ToInt64() + j));
    //                    }
    //                }
    //                else
    //                {
    //                    if (BitConverter.ToInt32(buffer, j) == valueToFind)
    //                    {
    //                        results.Add(new IntPtr(current.ToInt64() + j));
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }

    //    return results;
    //}

    //private IntPtr GetMainModuleBase()
    //{
    //    try
    //    {
    //        Process[] processes = Process.GetProcessesByName("DyingLightGame_TheBeast_x64_rwdi");
    //        if (processes.Length > 0)
    //        {
    //            return processes[0].MainModule.BaseAddress;
    //        }
    //    }
    //    catch { }
    //    return new IntPtr(0x0000000100000000); // fallback
    //}

    public List<IntPtr> FindAllValues(long valueToFind, bool useFloat = false)
    {
        List<IntPtr> results = new List<IntPtr>();
        byte[] buffer = new byte[0x200000];

        try
        {
            IntPtr address = IntPtr.Zero;
            MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();

            while (VirtualQuery(address, out mbi, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0)
            {
                if (mbi.State == 0x1000) // MEM_COMMIT
                {
                    long regionSize = mbi.RegionSize.ToInt64();
                    long current = mbi.BaseAddress.ToInt64();

                    for (long i = 0; i < regionSize; i += buffer.Length)
                    {
                        IntPtr currAddr = new IntPtr(current + i);

                        if (!ReadProcessMemory(processHandle, currAddr, buffer, buffer.Length, out _))
                            continue;

                        for (int j = 0; j < buffer.Length - 8; j += 4)
                        {
                            if (useFloat)
                            {
                                float val = BitConverter.ToSingle(buffer, j);
                                if (Math.Abs(val - valueToFind) < 5.0f)
                                    results.Add(new IntPtr(currAddr.ToInt64() + j));
                            }
                            else
                            {
                                if (BitConverter.ToInt32(buffer, j) == valueToFind)
                                    results.Add(new IntPtr(currAddr.ToInt64() + j));
                            }
                        }
                    }
                }

                address = new IntPtr(mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64());
            }
        }
        catch { }

        return results;
    }
    public List<IntPtr> FindAllValuesWithProgress(long valueToFind, bool useFloat = false, Action<int> progressCallback = null)
    {
        List<IntPtr> results = new List<IntPtr>();
        byte[] buffer = new byte[0x200000]; // 2MB chunks

        long[] regions = new long[]
        {
        0x0000000100000000,
        0x0000001000000000,
        0x0000010000000000,
        0x0000100000000000
        };

        long totalScanned = 0;
        long totalSize = 0x100000000; // Estimated total for progress

        try
        {
            foreach (long start in regions)
            {
                for (long i = 0; i < 0x40000000; i += buffer.Length)
                {
                    IntPtr current = new IntPtr(start + i);

                    if (ReadProcessMemory(processHandle, current, buffer, buffer.Length, out _))
                    {
                        for (int j = 0; j < buffer.Length - 8; j += 4)
                        {
                            if (useFloat)
                            {
                                float val = BitConverter.ToSingle(buffer, j);
                                if (Math.Abs(val - valueToFind) < 5.0f)
                                    results.Add(new IntPtr(current.ToInt64() + j));
                            }
                            else
                            {
                                if (BitConverter.ToInt32(buffer, j) == valueToFind)
                                    results.Add(new IntPtr(current.ToInt64() + j));
                            }
                        }
                    }

                    totalScanned += buffer.Length;

                    // Update progress every ~200MB
                    if (totalScanned % 0xC800000 == 0)
                    {
                        int progress = (int)((double)totalScanned / totalSize * 100);
                        progressCallback?.Invoke(Math.Min(progress, 95));
                    }
                }
            }
        }
        catch { }

        progressCallback?.Invoke(100);
        return results;
    }
}