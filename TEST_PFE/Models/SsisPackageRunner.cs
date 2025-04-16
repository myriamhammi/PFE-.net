using System.Diagnostics;

namespace TEST_PFE.Models
{
    public class SsisPackageRunner
    {
        public static async Task<bool> RunPackageAsync(string packagePath)
        {
            var process = new Process();
            process.StartInfo.FileName = "dtexec";
            process.StartInfo.Arguments = $"/F \"{packagePath}\""; // /F = run a .dtsx file
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Console.WriteLine(args.Data);
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Console.Error.WriteLine("ERROR: " + args.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            return process.ExitCode == 0;
        }
    }
}
