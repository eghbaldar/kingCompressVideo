using FFMpegCore;
using kingCompressVideo.Application.Services.VideoConverter;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Xabe.FFmpeg;

namespace Endpoint.Site.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            string projectRoot = Directory.GetCurrentDirectory(); // Get the project's root directory
            string inputFilePath = Path.Combine(projectRoot, "wwwroot", "originals/test.mp4"); // Combine paths to get full FFmpeg executable path
            string ffmpegPath = Path.Combine(projectRoot, "wwwroot", "ffmpeg.exe"); // Replace with the path to your FFmpeg executable

            Console.WriteLine(GetVideoWidth(inputFilePath, ffmpegPath));

            //ConverOneVideo();
            //ConvertWithList();
            //ConvertVideoMultiplieResolutions();
            /////////////////////////////////////////////////////////////////////////////////////
            // NOTE
            // [1]
            // when you stop your VS or even the brower that the VS will launch it, your all conversion (asynchorized process) will be cut off, why?
            //The behavior you're experiencing with your video conversion code is expected when running asynchronous tasks in C#. When you stop
            //the process from Visual Studio (VS), the running asynchronous tasks are typically terminated as well. This behavior is common in
            //development environments like Visual Studio, where stopping debugging or closing the application interrupts any ongoing processes.
            // [2]
            // When the user close its browser the process will be cut off? or on the server is not going to be happened like this, why?
            // When a user closes their browser, it does not directly affect processes running on the server. Here's why: Client-Side vs. Server-Sid or Background Processes on Servers
            return View();
        }
        public IActionResult GetVideos()
        {
            GetVideos getVideos = new GetVideos();
            return Json(getVideos.GetVideoList());
        }
        public async Task ConverOneVideo()
        {
            string projectRoot = Directory.GetCurrentDirectory(); // Get the project's root directory
            string inputFilePath = Path.Combine(projectRoot, "wwwroot", "originals/avi.avi"); // Combine paths to get full FFmpeg executable path
            string outputFilePath = Path.Combine(projectRoot, "wwwroot", "converted/converted.mp4"); // Combine paths to get full FFmpeg executable path   
            string ffmpegPath = Path.Combine(projectRoot, "wwwroot", "ffmpeg.exe"); // Replace with the path to your ffmpeg executable

            ConvertOneVideo videoConverter = new ConvertOneVideo(ffmpegPath);
            await videoConverter.ConvertToMP4(inputFilePath, outputFilePath);
        }
        public async Task ConvertWithList()
        {
            GetVideos getVideos = new GetVideos();
            GetVideosDto getvideodto = new GetVideosDto();
            getvideodto = getVideos.GetVideoList();


            string projectRoot = Directory.GetCurrentDirectory(); // Get the project's root directory
            List<string> inputFiles = new List<string>();
            foreach (var video in getvideodto.Videos)
            {
                inputFiles.Add(Path.Combine(projectRoot, "wwwroot", $"originals/{video}"));
            }

            string ffmpegPath = Path.Combine(projectRoot, "wwwroot", "ffmpeg.exe"); // Replace with the path to your ffmpeg executable

            ConvertWithList videoConverter = new ConvertWithList(ffmpegPath);
            await videoConverter.ConvertToMP4(inputFiles);
        }
        public async Task ConvertVideoMultiplieResolutions()
        {
            string projectRoot = Directory.GetCurrentDirectory(); // Get the project's root directory
            string inputFilePath = Path.Combine(projectRoot, "wwwroot", "originals/test.mp4"); // Combine paths to get full FFmpeg executable path
            string outputFilePath = Path.Combine(projectRoot, "wwwroot", "converted/converted.mp4"); // Combine paths to get full FFmpeg executable path   
            string ffmpegPath = Path.Combine(projectRoot, "wwwroot", "ffmpeg.exe"); // Replace with the path to your ffmpeg executable

            List<Tuple<int, int>> resolutions = new List<Tuple<int, int>>
                {
                new Tuple<int, int>(1920, 1080), // FULL HD 1920x1080
                new Tuple<int, int>(1280, 720),  // HD 720p 1280x720
                new Tuple<int, int>(854, 480)    // SD 854x480
                };
            ConvertOneVideoMultipleResolutions convertOneVideoMultipleResolutions = new ConvertOneVideoMultipleResolutions(ffmpegPath);
            await convertOneVideoMultipleResolutions.ConvertToMP4(inputFilePath, outputFilePath, resolutions);
        }

        private string GetVideoInformation(string inputFilePath, string ffmpegPath)
        {
            string arguments = $"-i \"{inputFilePath}\"";

            Process process = new Process();
            process.StartInfo.FileName = ffmpegPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            StringBuilder outputBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    // Append the output to the StringBuilder
                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    // Append the error output to the StringBuilder
                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            string output = outputBuilder.ToString();

            // Extract the bitrate and resolution from the output
            return output.ToString();

        }
        private int GetVideoWidth(string inputFilePath, string ffmpegPath)
        {
            // references:
            // https://stackoverflow.com/questions/6758042/how-to-get-video-dimensions-using-ffmpeg
            // https://askubuntu.com/questions/110264/how-to-find-frames-per-second-of-any-video-file
            //

            var re = new Regex("(\\d{2,4})x(\\d{2,4})");
            Match m = re.Match(GetVideoInformation(inputFilePath,ffmpegPath));
            int width = 1920; // default value to return
            if (m.Success)
            {
                //int height = 0; //int.TryParse(m.Groups[2].Value, out height);
                int.TryParse(m.Groups[1].Value, out width);                
                return width;
            }
            return width;
        }
    }
}