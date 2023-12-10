using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

class Program
{
    static async Task Main()
    {
        Console.WriteLine(" - - - Welcome to YVD - - - ");
        Console.WriteLine("Enter YouTube video URL to download it");
        Console.Write("URL : ");
        string videoUrl = Console.ReadLine();
        await DownloadVideoAsync(videoUrl);
    }

    static async Task DownloadVideoAsync(string videoUrl)
    {
        var youtube = new YoutubeClient();

        try
        {
            var video = await youtube.Videos.GetAsync(videoUrl);
            var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
            var videoStreamInfo = streamInfoSet.GetMuxedStreams().GetWithHighestVideoQuality();

            if (videoStreamInfo != null)
            {
                Console.WriteLine($"Downloading video '{video.Title}'...");

                var outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DownloadedVideos");
                Directory.CreateDirectory(outputDirectory);

                var outputFilePath = Path.Combine(outputDirectory, $"{video.Title}.{videoStreamInfo.Container}");

                
                {
                    await youtube.Videos.Streams.DownloadAsync(videoStreamInfo, outputFilePath);
                }

                Console.WriteLine($"Video downloaded: {outputFilePath}");
                Console.WriteLine("Want to download one more video? tape : yes or if not tape : no");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "yes":
                        Console.Clear();
                        Main();
                        break;
                    case "no":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("No video stream found for the given URL.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
