using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions.Library {
    public class SystemProcess {

        public class Result {
            public List<string> StdError = new List<string>();
            public List<string> StdOut = new List<string>();
            public int ExitCode;

            public bool Succeeded {
                get { return ExitCode == 0; }
            }
        }

        public static Result Run(string executablePath, string arguments, string? workingDirectory = null) {
            Result result = new Result();

            Process process = new Process();

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
                if(e.Data != null) {
                    result.StdOut.Add(e.Data);
                    Console.WriteLine(e.Data);
                }
            };

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
                if (e.Data != null) {
                    result.StdError.Add(e.Data);
                    Console.WriteLine(e.Data);
                }
            };
            process.StartInfo.FileName = executablePath;

            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;


            if (!string.IsNullOrWhiteSpace(workingDirectory)) {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

          
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();


            process.WaitForExit();
            result.ExitCode = process.ExitCode;

            return result;
        }

    }
}
