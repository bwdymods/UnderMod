using Mono.Cecil;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace UnderMod_Launcher
{

    public partial class MainWindow : Window
    {
        private static string AppPath = new FileInfo((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath).Directory.FullName;
        public const string DllFolder = @"UnderMod_Data";
        const string HackTarget = "UnderMine";//"Assembly-CSharp";
        string AssemblyPath = @"./Undermine_Data/Managed/" + HackTarget + ".dll";
        string TempPath = @"./Undermine_Data/Managed/_" + HackTarget + ".dll";
        string UMPath = @"./Undermine_Data/Managed/UnderMod.dll";
        string BackupPath = @"./Undermine_Data/Managed/" + HackTarget + ".um-bak";

        static bool _patched = false;
        bool patched {
            get { return _patched; }
            set {
                _patched = value;
                if (_patched)
                {
                    LabelStatus.Content = "Status: PATCHED";
                    LabelStatus.Foreground = System.Windows.Media.Brushes.Green;
                    ButtonPatch.IsEnabled = false;
                    ButtonUninstall.IsEnabled = true;
                    ButtonPlay.IsEnabled = true;
                } else
                {
                    LabelStatus.Content = "Status: UNPATCHED";
                    LabelStatus.Foreground = System.Windows.Media.Brushes.Red;
                    ButtonPatch.IsEnabled = true;
                    ButtonUninstall.IsEnabled = false;
                    ButtonPlay.IsEnabled = false;
                }
            }
        }
        static bool runOnce = false;

        public MainWindow()
        {
            InitializeComponent();
            if (!runOnce)
            {
                runOnce = true;
                //validate that we are installed alongside UnderMine.exe
                if (!File.Exists(System.IO.Path.Combine(AppPath, "UnderMine.exe")))
                {
                    MessageBox.Show("UnderMod needs to be installed in the same base folder as UnderMine.exe.", "Wrong directory!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Environment.Exit(0);
                }

                //install UnderMod DLL if it's in our data folder (likely indicates a fresh install or update)
                string targetPath3 = System.IO.Path.Combine(AppPath, DllFolder, "UnderMod.dll");
                if (File.Exists(targetPath3))
                {
                    Console.WriteLine("Appears to be a new installation. Deploying UnderMod.dll ...");
                    try
                    {
                        string destFileName = System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "UnderMod.dll");
                        if (File.Exists(destFileName))
                        {
                            File.Delete(destFileName);
                        }
                        File.Move(targetPath3, destFileName);

                        //install harmony
                        if (!File.Exists(System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "0Harmony.dll"))) File.Copy(System.IO.Path.Combine(AppPath, DllFolder, "0Harmony.dll"), System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "0Harmony.dll"));

                        //install mscorlib (unstripped)
                        if(!File.Exists(System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "mscorlib.umbak")))
                        {
                            File.Move(System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "mscorlib.dll"), System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "mscorlib.umbak"));
                            File.Copy(System.IO.Path.Combine(AppPath, DllFolder, "mscorlib.dll"), System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "mscorlib.dll"));
                        }
                        Console.WriteLine("Deployed UnderMod.dll successfully.");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("UnderMod.dll could not be deployed: " + e.Message, "Error deploying UnderMod.dll", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        Environment.Exit(-1);
                    }
                }

                //validate that UnderMod.dll is installed
                if (File.Exists(System.IO.Path.Combine(AppPath, "UnderMine_Data", "Managed", "UnderMod.dll")))
                {
                    Console.WriteLine("UnderMod.dll found.");
                }
                else
                {
                    MessageBox.Show("UnderMod.dll not found! Please re-install UnderMod.", "UnderMod.dll not found!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Environment.Exit(-1);
                }

                //let's try to load the assembly and see if it's already patched
                try
                {
                    using (var ASM = AssemblyDefinition.ReadAssembly(AssemblyPath))
                    {
                        if (ASM.MainModule.HasAssemblyReferences)
                        {
                            foreach (var mr in ASM.MainModule.ModuleReferences)
                            {
                                if (mr.Name.Contains("UnderMod"))
                                {
                                    Console.WriteLine("Patch is installed.");
                                    patched = true;
                                    break;
                                }
                            }
                            if (!patched)
                            {
                                Console.WriteLine("Patch is not installed.");
                                patched = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Patch is not installed.");
                            patched = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Attempted to read assembly. Error:\n" + e.Message);
                }

                //create the mods folder
                System.IO.Directory.CreateDirectory("Mods");
            }
        }

        //play
        private void Button_Click_Play(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("clicky");
            string sp = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null);
            if (sp != null)
            {
                sp += "\\Steam.exe";
                Console.WriteLine("Found Steam: " + sp);
                Process steamUndermine = new Process();
                steamUndermine.StartInfo.FileName = sp;
                steamUndermine.StartInfo.Arguments = "-applaunch 656350 -UnderModLauncher";
                steamUndermine.StartInfo.UseShellExecute = false;
                steamUndermine.StartInfo.RedirectStandardOutput = true;

                steamUndermine.Start();

                System.IO.StreamReader reader = steamUndermine.StandardOutput;
                string output = reader.ReadToEnd();
                Console.WriteLine(output);
            }
            else
            {
                Console.WriteLine("Can't locate Steam!");
            }
            Environment.Exit(0);
        }

        //patch
        private void Button_Click_Patch(object sender, RoutedEventArgs e)
        {
            if (patched)
            {
                Console.WriteLine("The patch is already installed! We can't install it any farther!");
                return;
            }

            try
            {
                //create the backup
                if (File.Exists(BackupPath)) File.Delete(BackupPath);
                File.Copy(AssemblyPath, BackupPath);

                //read the assembly
                var resolver = new DefaultAssemblyResolver();
                resolver.AddSearchDirectory(System.IO.Path.Combine(AppPath, @"UnderMine_Data", "Managed"));
                var ASM = AssemblyDefinition.ReadAssembly(BackupPath, new ReaderParameters { AssemblyResolver = resolver });

                //add a reference to UnderMod
                ASM.MainModule.ModuleReferences.Add(new ModuleReference("UnderMod.dll"));
                ASM.Write(TempPath);
                ASM.Dispose();
                Console.WriteLine("Reference patch completed.");
                patched = true;

                /*
                System.Threading.Thread.Sleep(1000);
                using (var i = new EinarEgilsson.Utilities.InjectModuleInitializer.Injector())
                {
                    try
                    {
                        i.Inject(AssemblyPath, "UnderMod::Initialize", null, UMPath);
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine("Error:\n" + e2.Message);
                    }
                }
                Console.WriteLine("Patch complete!");
                */
                
                //read the assembly
                ASM = AssemblyDefinition.ReadAssembly(TempPath, new ReaderParameters { AssemblyResolver = resolver });

                //add a reference to UnderMod
                ASM.MainModule.ModuleReferences.Add(new ModuleReference(UMPath));

                TypeDefinition moduleType = ASM.MainModule.Types.First(x => x.Name == "<Module>");
                MethodDefinition[] toDel = moduleType.Methods.Where(x => x.Name == ".cctor").ToArray();
                foreach (var method in toDel) { moduleType.Methods.Remove(method); }

                TypeReference voidRef = ASM.MainModule.ImportReference(typeof(void));
                Mono.Cecil.MethodAttributes cctorAttrs = Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName;
                MethodDefinition moduleCtor = new MethodDefinition(".cctor", cctorAttrs, voidRef);

                MethodReference initMethod = ASM.MainModule.ImportReference(typeof(UnderMod).GetMethod("Initialize"));
                moduleCtor.Body.Instructions.Add(Mono.Cecil.Cil.Instruction.Create(Mono.Cecil.Cil.OpCodes.Call, initMethod));
                moduleCtor.Body.Instructions.Add(Mono.Cecil.Cil.Instruction.Create(Mono.Cecil.Cil.OpCodes.Ret));

                moduleType.Methods.Add(moduleCtor);

                ASM.Write(AssemblyPath);
                ASM.Dispose();
                Console.WriteLine("Patch complete!");
                File.Delete(TempPath);
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message);
                return;
            }
        }

        private void Button_Click_Uninstall(object sender, RoutedEventArgs e)
        {
            if (!patched)
            {
                MessageBox.Show("Your installation does not appear to be patched by UnderMod. Nothing to uninstall.", "Not patched!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //restore the backup
            if (File.Exists(BackupPath))
            {
                File.Delete(AssemblyPath);
                File.Move(BackupPath, AssemblyPath);
                patched = false;
                MessageBox.Show("Restored backup of original game DLL.", "Backup restored!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            } else
            {
                MessageBox.Show("Cannot uninstall: The backup file is not present!", "Cannot restore backup!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void ButtonMods_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Mods");
        }
    }
}
