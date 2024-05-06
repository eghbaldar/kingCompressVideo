using kingCompressVideo.Application.Services.VideoConverter;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Site.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //ConverOneVideo();
            ConvertWithList();
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
    }
}