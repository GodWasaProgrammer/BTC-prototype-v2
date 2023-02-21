using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;

namespace BTC_Prototype
{
	internal class Program
	{
		public static void Main()
		{
			Directory.CreateDirectory("output");
			Directory.CreateDirectory("text added");

			var inputFile = new MediaFile { Filename = "..\\..\\testvideo.mp4" };
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

			List<MagickReadSettings> listOfSettingsForText = new List<MagickReadSettings>();

			var settingsTextOne = new MagickReadSettings
			{
				Font = "italic",
				FillColor = MagickColors.ForestGreen,
				StrokeColor = MagickColors.LimeGreen,
				FontStyle = FontStyleType.Bold,
				TextGravity = Gravity.Center,
				BackgroundColor = MagickColors.Transparent,
				Height = 250, // height of text box
				Width = 200, // width of text box

			};

			listOfSettingsForText.Add(settingsTextOne);

			var settingsTextTwo = new MagickReadSettings
			{
				Font = "italic",
				FillColor = MagickColors.PaleGoldenrod,
				StrokeColor = MagickColors.LimeGreen,
				FontStyle = FontStyleType.Bold,
				TextGravity = Gravity.Center,
				BackgroundColor = MagickColors.Transparent,
				Height = 250, // height of text box
				Width = 200, // width of text box

			};

			listOfSettingsForText.Add(settingsTextTwo);

			var settingsTextThree = new MagickReadSettings
			{
				Font = "italic",
				FillColor = MagickColors.Orange,
				StrokeColor = MagickColors.LimeGreen,
				FontStyle = FontStyleType.Oblique,
				TextGravity = Gravity.Center,
				BackgroundColor = MagickColors.Transparent,
				Height = 250, // height of text box
				Width = 200, // width of text box

			};

			listOfSettingsForText.Add(settingsTextThree);

			var settingsTextFour = new MagickReadSettings
			{
				Font = "italic",
				FillColor = MagickColors.ForestGreen,
				StrokeColor = MagickColors.LimeGreen,
				FontStyle = FontStyleType.Bold,
				TextGravity = Gravity.Center,
				BackgroundColor = MagickColors.Transparent,
				Height = 250, // height of text box
				Width = 200, // width of text box

			};

			listOfSettingsForText.Add(settingsTextFour);

			Random RandomSettings = new Random();
			foreach (string filepath in FilePaths)
			{
				string filepathCorrected = filepath.TrimStart('o', 'u', 't', 'p', 'u', 't', '/');
				string textAdded = $"text added/{filepathCorrected}";
				var pathToBackgroundImage = filepath;
				var pathToNewImage = textAdded;
				var textToWrite = "Done with ImageMagick";

				// These settings will create a new caption
				// which automatically resizes the text to best
				// fit within the box.
				
					var settings = listOfSettingsForText[RandomSettings.Next(listOfSettingsForText.Count)];

					using (var image = new MagickImage(pathToBackgroundImage))
					{
						using (var caption = new MagickImage($"caption:{textToWrite}", settings))
						{
							// Add the caption layer on top of the background image

							var size = new MagickGeometry(1280, 720);
							image.Composite(caption, 1, 200, CompositeOperator.Over);
							image.Composite(caption, 600, 50, CompositeOperator.Over);
							image.Composite(caption, 1200, 100, CompositeOperator.Over);
							image.Composite(caption, 100, 700, CompositeOperator.Over);
							image.Resize(size);
							image.Write(pathToNewImage);

						}

					}

			}


		}

		// MediaToolKit
		public static void GrabThumbNail(MediaFile inputFile, MediaFile outputFile, int intervalBetweenPictures)
		{
			using (var engine = new Engine())
			{
				engine.GetMetadata(inputFile);

				var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(intervalBetweenPictures) };

				// fetches image, creates it.
				engine.GetThumbnail(inputFile, outputFile, options);

			}

		}

	}

}
