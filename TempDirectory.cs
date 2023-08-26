using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Actions.Library {
    public static class TempDirectory {

        public static string New() {
            var assemblyPath = Assembly.GetEntryAssembly().Location;
            var assemblyDir = Path.GetDirectoryName(assemblyPath);
            var tmpDir = Path.Combine(assemblyDir, "Temp");
            Directory.CreateDirectory(tmpDir);

            int id = 0;
            while (true) {
                var dir = Path.Combine(tmpDir, id.ToString());
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                    return dir;
                }
                ++id;
            }
        }
    }
}
