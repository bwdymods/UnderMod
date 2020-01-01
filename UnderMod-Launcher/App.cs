using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace UnderMod_Launcher
{
    public partial class App : Application
    {
        public const string DllFolder = @"UnderMod_Data";
        private static string[] UMDlls = { "Mono.Cecil.dll", "Mono.Cecil.Mdb.dll", "Mono.Cecil.Pdb.dll", "Mono.Cecil.Rocks.dll" };
        private static string AppPath = new FileInfo((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath).Directory.FullName;

        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            //add our custom assembly resolving, for patch functionality
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string assemblyFile = (args.Name.Contains(','))
                    ? args.Name.Substring(0, args.Name.IndexOf(','))
                    : args.Name;

                assemblyFile += ".dll";
                Console.WriteLine("Attempting to resolve dll: " + assemblyFile);

                // if it's not one of ours, it must be part of UnderMine or Unity
                if (!UMDlls.Contains(assemblyFile))
                {
                    string targetPath2 = System.IO.Path.Combine(AppPath, @"UnderMine_Data", @"Managed", assemblyFile);
                    try { return Assembly.LoadFile(targetPath2); } catch (Exception) { return null; }
                }

                //one of ours, lets check the UnderMod_Data folder
                string targetPath = System.IO.Path.Combine(AppPath, DllFolder, assemblyFile);
                try { return Assembly.LoadFile(targetPath); } catch (Exception) { return null; }
            };


            App app = new App();
            MainWindow window = new MainWindow();
            app.Run(window);
        }
    }
}