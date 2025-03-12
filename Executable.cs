using Actions.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions {
    public class Executable {
        public string FullPath {
            get; private set;
        }
        private Executable() { FullPath = "UNKNOWN"; }
        public Executable(string fullPath) {
            if (!File.Exists(fullPath)) {
                throw new FileNotFoundException($"Executable file not found at path {fullPath}");
            }
            FullPath = fullPath;
        }

        public SystemProcess.Result Run(string args, string? workingDirectory = null) {
            return SystemProcess.Run(FullPath, args, workingDirectory);
        }
    }
}
