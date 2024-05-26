using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Actions.Library {
    public static class Config {
        private static string PropertyNamePrefix = "Actions_";

        private static string? Get([CallerMemberName]string? name = null) {
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }
            var result = System.Environment.GetEnvironmentVariable(PropertyNamePrefix + name);
            return result;
        }
        private static void Set(string? value, [CallerMemberName] string? name = null) {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }
            System.Environment.SetEnvironmentVariable(PropertyNamePrefix + name, value);
        }


        public static string? Nuget_RepoAuth { get { return Get(); } set { Set(value); } }
        public static string? Nuget_RepoName { get { return Get(); } set { Set(value); } }
    }
}
