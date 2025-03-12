using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions {
    public static class Find {
        public static string? SystemFilePath(string name) {
            name = Environment.ExpandEnvironmentVariables(name);
            if (!File.Exists(name)) {
                if (string.IsNullOrEmpty(Path.GetDirectoryName(name))) {
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';')) {
                        string path = test.Trim();
                        if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, name)))
                            return Path.GetFullPath(path);
                    }
                }
                return null;
            }
            return Path.GetFullPath(name);
        }


        public static string? ExecutablePath(string name) {
            string? result = SystemFilePath(name);
            if (result == null) {
                if (!name.ToLower().EndsWith(".exe")) {
                    name = name + ".exe";
                    result = SystemFilePath(name);
                }
            }
            return result;
        }

        public static Executable? Executable(string name) {
            var path = ExecutablePath(name);
            if (path == null) {
                return null;
            }
            return new Executable(path);
        }
    }
}
