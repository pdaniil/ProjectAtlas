using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using ProjectAtlas.Models;
using Point = ProjectAtlas.Models.Point;

namespace ProjectAtlas.Extensions
{
    public class Game
    {
        [DllImport("Kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesRead);

        public static Point GetCoords(Offset offset, string windowName)
        {
            var point = new Point()
            {
                From = new Coordinates(),
                Direction = new Coordinates()
            };

            try
            {
                var processes = Process.GetProcessesByName(windowName);

                if (processes.Length > 0)
                {
                    var process = processes[0].Handle;
                    var buffer = new byte[4];

                    var baseOffset = 0;

                    foreach (int chain in offset.BaseChain)
                    {
                        ReadProcessMemory(process, ((IntPtr)(baseOffset + chain)), buffer, 4, 0);
                        baseOffset = BitConverter.ToInt32(buffer, 0);
                    }

                    ReadProcessMemory(process, ((IntPtr)(baseOffset + offset.DirX)), buffer, 4, 0);
                    point.Direction.X = BitConverter.ToSingle(buffer, 0);

                    ReadProcessMemory(process, ((IntPtr)(baseOffset + offset.DirY)), buffer, 4, 0);
                    point.Direction.Y = BitConverter.ToSingle(buffer, 0);

                    ReadProcessMemory(process, ((IntPtr)(baseOffset + offset.DirZ)), buffer, 4, 0);
                    point.Direction.Z = BitConverter.ToSingle(buffer, 0);

                    ReadProcessMemory(process, ((IntPtr)(baseOffset + offset.PosX)), buffer, 4, 0);
                    point.From.X = BitConverter.ToSingle(buffer, 0);

                    ReadProcessMemory(process, ((IntPtr)(baseOffset + offset.PosY)), buffer, 4, 0);
                    point.From.Y = BitConverter.ToSingle(buffer, 0);

                    ReadProcessMemory(process, ((IntPtr)(baseOffset + offset.PosZ)), buffer, 4, 0);
                    point.From.Z = BitConverter.ToSingle(buffer, 0);
                }
            }
            catch (Exception)
            {
            }

            return point;
        }
    }
}