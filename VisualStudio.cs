using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Actions.Library;

namespace Actions {
    public static class VisualStudio {
        public static class Version {
            public static readonly string VisualStudio2017 = "14.16";
            public static readonly string VisualStudio2015 = "14.0";
        }

        public class VSWhereOutput {
            public List<Instance> Instances = new List<Instance>();

            public class Instance {
                public System.Text.Json.JsonElement JsonElement;

                public string DisplayName => JsonElement.GetProperty("displayName").GetString();
                public bool IsLaunchable => JsonElement.GetProperty("isLaunchable").GetBoolean();
                public bool IsPrerelease => JsonElement.GetProperty("isPrerelease").GetBoolean();

                public string Catalog_ProductLineVersion => JsonElement.GetProperty("catalog").GetProperty("productLineVersion").GetString();
                public string Catalog_ProductSemanticVersion => JsonElement.GetProperty("catalog").GetProperty("productSemanticVersion").GetString();
                //

            }

            public VSWhereOutput(System.Text.Json.JsonDocument vswhereJsonOutput) {
                var cnt = vswhereJsonOutput.RootElement.GetArrayLength();
                for (int i = 0; i < cnt; i++) {
                    Instance inst = new Instance();
                    inst.JsonElement = vswhereJsonOutput.RootElement[i];
                    Instances.Add(inst);
                }
            }
        }

        public static VSWhereOutput VSWhereQuery(bool latest, bool prerelease) {
            var programFilesDir = System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            var vsInstalledDir = Path.Combine(programFilesDir, @"Microsoft Visual Studio\Installer\");
            var vsWherePath = Path.Combine(vsInstalledDir, "vswhere.exe");
            var vsWhereResult = SystemProcess.Run(vsWherePath, $"-format json -nologo {(latest ? "-latest" : "")} {(prerelease ? "-prerelease" : "")}");

            var json = System.Text.Json.JsonDocument.Parse(string.Join("\n", vsWhereResult.StdOut));

            VSWhereOutput result = new VSWhereOutput(json);
            return result;
        }

        public static string FindVisualStudioPath() {
            var programFilesDir = System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            var vsInstalledDir = Path.Combine(programFilesDir, @"Microsoft Visual Studio\Installer\");
            var vsWherePath = Path.Combine(vsInstalledDir, "vswhere.exe");
            var vsWhereResult = SystemProcess.Run(vsWherePath, "-latest -property installationPath");
            Console.WriteLine("Detected VisualStudio installation path: " + vsWhereResult.StdOut[0]);
            return vsWhereResult.StdOut[0];
        }
        public static string FindVCvarsallBatFile() {
            var vsInstallPath = FindVisualStudioPath();
            return Path.Combine(vsInstallPath, @"VC\Auxiliary\Build\vcvarsall.bat");
        }

        public static void SetupEnvironment(CpuArchitecture targetArch) {
            var programFilesDir = System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            var vsInstalledDir = Path.Combine(programFilesDir, @"Microsoft Visual Studio\Installer\");
            var vsWherePath = Path.Combine(vsInstalledDir, "vswhere.exe");
            var vsWhereResult = SystemProcess.Run(vsWherePath, "-latest -property installationPath");
            var vsInstallPath = vsWhereResult.StdOut[0];


            Console.WriteLine("Setting up build environment");

            var vsEnvBatchFile = FindVCvarsallBatFile();

            var batchFileArgs = "";
            if (targetArch == CpuArchitecture.x86) {
                batchFileArgs = "x86";
            } else if (targetArch == CpuArchitecture.x64) {
                batchFileArgs = "amd64";
            } else if (targetArch == CpuArchitecture.armeabi_v7a) {
                batchFileArgs = "x86_arm";
            } else if (targetArch == CpuArchitecture.arm64_v8a) {
                batchFileArgs = "x86_arm64";
            }

          
            //batchFileArgs += $" -vcvars_ver={Config.VisualStudio.ToolsetVersion}";
            


            var vsEnvResult = SystemProcess.Run("cmd", $"/C \"{vsEnvBatchFile}\" {batchFileArgs} > nul 2>&1 && set");
            Regex r = new Regex("^([^=]+)=(.*)");
            foreach (var s in vsEnvResult.StdOut) {
                var m = r.Match(s);
                if (m.Success) {
                    System.Environment.SetEnvironmentVariable(m.Groups[1].Value, m.Groups[2].Value);
                }
            }
        }

        public static void CleanEnvironment() {
            var batchFileArgs = " /clean_env";
            var vsEnvBatchFile = FindVCvarsallBatFile();
            var vsEnvResult = SystemProcess.Run("cmd", $"/C \"{vsEnvBatchFile}\" {batchFileArgs} > nul 2>&1 && set");
            Regex r = new Regex("^([^=]+)=(.*)");
            foreach (var s in vsEnvResult.StdOut) {
                var m = r.Match(s);
                if (m.Success) {
                    System.Environment.SetEnvironmentVariable(m.Groups[1].Value, m.Groups[2].Value);
                }
            }
        }
    }
}
