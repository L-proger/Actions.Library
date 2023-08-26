using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions.Library {
    public class CMake {

        public static void Configure(string sourceDirectoryPath, string buildDirectoryPath) {
            SystemProcess.Run("cmake", $"-DCMAKE_BUILD_TYPE:STRING=\"Release\" -DCMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE:PATH=\"{buildDirectoryPath}/bin\"  -H{sourceDirectoryPath} -B{buildDirectoryPath}", null);
        }

        public static void Build(string buildDirectoryPath) {
            SystemProcess.Run("cmake", $"--build {buildDirectoryPath} --config Release", null);
        }
    }
}
