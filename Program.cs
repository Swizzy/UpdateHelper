using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UpdateHelper {

    internal static class Program {
        private static void Main(string[] args) {
            try {
                Thread.Sleep(1000);
                if(args.Length < 3)
                    Usage();
                if(!IsNumeric(args[0]))
                    KillApp(args[0]);
                else
                    KillApp(int.Parse(args[0]));
                if(!File.Exists(args[1]))
                    Usage();
                else if(File.Exists(args[2]))
                    File.Delete(args[2]);
                File.Move(args[1], args[2]);
                var newargs = "";
                if(args.Length > 3) {
                    for(var i = 3; i < args.Length; i++)
                        newargs += string.Format(" \"{0}\"", args[i]);
                }
                Process.Start(args[2], newargs);
                Environment.Exit(0);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private static void Usage() {
            MessageBox.Show(string.Format("Usage: {0} [ProcessName/PID] [New Executable Path] [Old Executable Path] <Optional: Arguments passed back to the new executable>", Path.GetFileName(Application.ExecutablePath)), "Bad arguments to Swizzy's Update helper!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Environment.Exit(1);
        }

        private static bool IsNumeric(string input) {
            int result;
            return int.TryParse(input, out result);
        }

        private static void KillApp(string appname) {
            try {
                var processesByName = Process.GetProcessesByName(appname);
                if(processesByName.Length <= 0)
                    return;
                foreach(var process in processesByName) {
                    if(process.HasExited)
                        continue;
                    process.Kill();
                    process.WaitForExit();
                }
                KillApp(appname);
            }
            catch(Exception ex) { }
        }

        private static void KillApp(int id) {
            try {
                var processById = Process.GetProcessById(id);
                if(processById.HasExited)
                    return;
                processById.Kill();
                processById.WaitForExit();
            }
            catch(Exception ex) { }
        }
    }

}