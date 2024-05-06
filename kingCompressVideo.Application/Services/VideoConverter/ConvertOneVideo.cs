using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingCompressVideo.Application.Services.VideoConverter
{
    public class ConvertOneVideo
    {
        private string ffmpegPath;
        public ConvertOneVideo(string ffmpegPath)
        {
            this.ffmpegPath = ffmpegPath;

        }
        public async Task ConvertToMP4(string inputFilePath,string outputFilePath)
        {
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
