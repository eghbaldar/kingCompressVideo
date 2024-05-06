using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingCompressVideo.Application.Services.VideoConverter
{
    public class GetVideosDto
    {
        public List<string> Videos { get; set; }
    }
    public class GetVideos
    {
        public GetVideosDto GetVideoList()
        {
            string projectRoot = Directory.GetCurrentDirectory(); // Get the project's root directory
            string directoryPath = Path.Combine(projectRoot, "wwwroot", "originals"); // Combine paths to get full FFmpeg executable path

            string[] files = Directory.GetFiles(directoryPath);
            var result = files.Select(x => Path.GetFileName(x)).ToList();

            return new GetVideosDto
            {
                Videos = result,
            };
        }
    }
}
