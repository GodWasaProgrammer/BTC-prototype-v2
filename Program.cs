using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System;
using System.Collections.Generic;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System.IO;
using System.Drawing;
using ImageProcessor.Imaging;

namespace BTC_Prototype
{
	internal class Program
	{
		public static void Main()
		{
			Directory.CreateDirectory("output");
			Directory.CreateDirectory("text added");

			var inputFile = new MediaFile { Filename = "..\\..\\testvideo.mkv" };
			int interval = 15;
			List<string> FilePaths = new List<string>();

			// creates 10 images from a mediafile
			for (int i = 0; i < 10; i++)
			{
				var outputFileName = new MediaFile { Filename = $"output/{i + 1}.jpeg" };

				GrabThumbNail(inputFile, outputFileName, interval);
				interval += 15;

				FilePaths.Add(outputFileName.Filename);
			}

			foreach (string filepath in FilePaths)
			{
				ImageProcessor(filepath);
			}

		}

		// MediaToolKit
		public static void GrabThumbNail(MediaFile inputFile, MediaFile outputFile, int intervalBetweenPictures)
		{
			using (var engine = new Engine())
			{
				engine.GetMetadata(inputFile);

				// selects where to save the image
				var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(intervalBetweenPictures) };

				// fetches image, creates it.
				engine.GetThumbnail(inputFile, outputFile, options);

			}

		}

		public static void ImageProcessor(string file)
		{
			byte[] photoBytes = File.ReadAllBytes(file);

			// Format is automatically detected though can be changed.
			ISupportedImageFormat format = new JpegFormat { Quality = 70 };

			Size size = new Size(1280, 720);
			file = file.TrimStart('o', 'u', 't', 'p', 'u', 't', '/');
			string textAdded = $"text added/{file}";
			using (MemoryStream inStream = new MemoryStream(photoBytes))
			{
				using (MemoryStream outStream = new MemoryStream())
				{
					// Initialize the ImageFactory using the overload to preserve EXIF metadata.
					using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
					{
						// Load, resize, set the format and quality and save an image.
						imageFactory.Load(inStream)
									.Resize(size)
									.Format(format)
									.Watermark(new TextLayer()
									{
										DropShadow = true,
										FontFamily = FontFamily.GenericMonospace,
										Text = "BTC Proto",
										FontSize = 400,
										Style = FontStyle.Bold,
										FontColor = Color.GreenYellow
									})
									.Save(textAdded);
					}

				}

			}

		}

	}

}
