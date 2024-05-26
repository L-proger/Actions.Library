using Actions.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions.Nuget {
    public class Nuget {
        public static void DeletePackage(string repository, string packageName, string packageVersion) {
            var result = SystemProcess.Run("dotnet", $"nuget delete {packageName} {packageVersion} -s {repository} --non-interactive");
            if (!result.Succeeded) {
                throw new Exception("Failed to delete nuget package");
            }
        }

        public static void PushPackage(string repository, string packageFilePath, string? workingDirectory = null) {
            var result = SystemProcess.Run("dotnet", $"nuget push --source {repository} {packageFilePath}", workingDirectory);
            if (!result.Succeeded) {
                throw new Exception("Failed to push nuget package");
            }
        }

        public static void PackPackage(string workingDirectory) {
            var result = SystemProcess.Run("nuget", "pack", workingDirectory);
            if (!result.Succeeded) {
                throw new Exception($"Failed to pack nuget package: {string.Join("\n", result.StdError)}");
            }
        }
    }
}
