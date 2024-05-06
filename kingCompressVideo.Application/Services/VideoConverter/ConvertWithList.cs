using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingCompressVideo.Application.Services.VideoConverter
{
    public class ConvertWithList
    {
        private string ffmpegPath;
        public ConvertWithList(string ffmpegPath)
        {
            this.ffmpegPath = ffmpegPath;
        }
        public async Task ConvertToMP4(List<string> inputFilePaths)
        {
            foreach (string inputFilePath in inputFilePaths)
            {
                string projectRoot = Directory.GetCurrentDirectory(); // Get the project's root directory
                string outputFilePath = Path.Combine(projectRoot, "wwwroot", $"converted/{Path.GetFileName(inputFilePath)}.mp4"); // Combine paths to get full FFmpeg executable path   

                string arguments = $"-i \"{inputFilePath}\" -c:v libx264 -crf 23 -preset medium -c:a aac -b:a 128k -movflags +faststart \"{outputFilePath}\"";

                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine($"Converting {Path.GetFileName(inputFilePath)} to {Path.GetFileName(outputFilePath)}");
                Console.WriteLine("-----------------------------------------------------------------------------");

                Process process = new Process();
                process.StartInfo.FileName = ffmpegPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await Task.Run(() => process.WaitForExit());
            }
        }
    }
}
