using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingCompressVideo.Application.Services.VideoConverter
{    
    public class ConvertOneVideoMultipleResolutions
    {
        private string ffmpegPath;
        public ConvertOneVideoMultipleResolutions(string ffmpegPath)
        {
            this.ffmpegPath = ffmpegPath;
        }
        public async Task ConvertToMP4(string inputFilePath, string outputFilePath, List<Tuple<int, int>> resolutions)
        {
            foreach (var resolution in resolutions)
            {
                string arguments = $"-i \"{inputFilePath}\" -vf scale={resolution.Item1}:{resolution.Item2} -c:v libx264 -crf 23 -preset medium -c:a aac -b:a 128k -movflags +faststart \"{outputFilePath}_{resolution.Item1}x{resolution.Item2}.mp4\"";

                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine($"Converting {Path.GetFileName(inputFilePath)} to {Path.GetFileName(outputFilePath)}_{resolution.Item1}x{resolution.Item2}.mp4");
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
