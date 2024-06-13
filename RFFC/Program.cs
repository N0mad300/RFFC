using Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace CommandLineApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} <command> [args...]");
                return;
            }

            string command = args[0];
            List<string> arguments = new List<string>(args[1..]);

            HandleCommand(command, arguments);
        }

        static void HandleCommand(string command, List<string> args)
        {
            if (command == "convert")
            {
                if (args.Count >= 5)
                {
                    string game = args[0];
                    string inputFormat = args[1];
                    string outputFormat = args[2];
                    string inputPath = args[3];
                    string outputPath = args[4];

                    bool isRecursive = false;

                    if (args.Count == 6 && args[5].ToLower() == "true")
                    {
                        isRecursive = true;
                    }

                    if (game == "alanwake2" || game == "a" || game == "aw2")
                    {
                        if (inputFormat == "texture" || inputFormat == "tex" || inputFormat == "dds")
                        {
                            void ConvertTextureAW2 (string input_path, string output_path)
                            { 
                                IStream Stream;

                                if (!Directory.Exists(output_path))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(outputPath);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Invalid output path, must be a valid directory");
                                    }
                                }

                                if (File.Exists(input_path))
                                {
                                    Stream = FStream.Open(input_path, FileMode.Open, FileAccess.Read);
                                    Bitmap convertedBitmap = DDSToBitmap.Convert(Stream);

                                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(input_path);

                                    if (outputFormat == "png")
                                    {
                                        convertedBitmap.Save(Path.Combine(output_path, $"{fileNameWithoutExtension}.png"), ImageFormat.Png);
                                    }
                                    else if (outputFormat == "jpg" || outputFormat == "jpeg")
                                    {
                                        convertedBitmap.Save(Path.Combine(output_path, $"{fileNameWithoutExtension}.jpg"), ImageFormat.Jpeg);
                                    }
                                    else if (outputFormat == "bmp" || outputFormat == "bitmap")
                                    {
                                        convertedBitmap.Save(Path.Combine(output_path, $"{fileNameWithoutExtension}.bmp"), ImageFormat.Bmp);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid output format");
                                    }

                                    convertedBitmap.Dispose();
                                }
                                else if (Directory.Exists(input_path))
                                {
                                    string[] files = Directory.GetFiles(input_path, "*.tex");

                                    foreach (string file in files)
                                    {
                                        try
                                        {
                                            Stream = FStream.Open(file, FileMode.Open, FileAccess.Read);
                                            Bitmap convertedBitmap = DDSToBitmap.Convert(Stream);

                                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                                            if (outputFormat == "png")
                                            {
                                                convertedBitmap.Save(Path.Combine(output_path, $"{fileNameWithoutExtension}.png"), ImageFormat.Png);
                                            }
                                            else if (outputFormat == "jpg" || outputFormat == "jpeg")
                                            {
                                                convertedBitmap.Save(Path.Combine(output_path, $"{fileNameWithoutExtension}.jpg"), ImageFormat.Jpeg);
                                            }
                                            else if (outputFormat == "bmp" || outputFormat == "bitmap")
                                            {
                                                convertedBitmap.Save(Path.Combine(output_path, $"{fileNameWithoutExtension}. bmp"), ImageFormat.Bmp);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid output format");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"An error occurred while processing {file}: {ex.Message}");
                                        }
                                    }

                                    if (isRecursive == true)
                                    {
                                        // Recursively process each subdirectory
                                        string[] subdirectories = Directory.GetDirectories(input_path);
                                        foreach (string subdirectory in subdirectories)
                                        {
                                            // Create the corresponding subdirectory in the output path
                                            string subdirectoryName = Path.GetFileName(subdirectory);
                                            string newOutputPath = Path.Combine(output_path, subdirectoryName);
                                            Directory.CreateDirectory(newOutputPath);

                                            ConvertTextureAW2(subdirectory, newOutputPath);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input path");
                                }
                            }

                            ConvertTextureAW2(inputPath, outputPath);
                        }
                    }
                    else if (game == "control" || game == "c" || game == "ctrl")
                    {
                        if (inputFormat == "texture" || inputFormat == "tex" || inputFormat == "dds")
                        {
                            // Conversion logic for Control
                        }
                    }
                    else if (game == "quantumbrake" || game == "q")
                    {
                        // Conversion logic for Quantum Break
                    }
                    else
                    {
                        Console.WriteLine($"Invalid game: {game}");
                    }
                }
                else
                {
                    Console.WriteLine("Usage: convert <game> <input_format> <output_format> <input_path> <output_path> <options>");
                }
            }
            else if (command == "chext")
            {
                if (args.Count >= 2) 
                {
                    string inputPath = args[0];
                    string newExtension = args[1];
                    bool isRecursive = false;

                    void changeExtension(string input_path)
                    {
                        if (string.IsNullOrEmpty(input_path))
                        {
                            throw new ArgumentException("File path cannot be null or empty", nameof(input_path));
                        }

                        if (string.IsNullOrEmpty(newExtension) || !newExtension.StartsWith("."))
                        {
                            throw new ArgumentException("New extension must start with a dot and cannot be null or empty", nameof(newExtension));
                        }

                        if (File.Exists(input_path) && args.Count == 2)
                        {
                            string newFilePath = Path.ChangeExtension(input_path, newExtension);

                            if (newFilePath == null)
                            {
                                throw new InvalidOperationException("Failed to change file extension");
                            }

                            using (FileStream fs = File.Create(newFilePath))
                            {
                                if (!File.Exists(newFilePath))
                                {
                                    throw new InvalidOperationException("Failed to create the new file");
                                }

                                using (FileStream originalFs = File.OpenRead(input_path))
                                {
                                    originalFs.CopyTo(fs);
                                }
                            }

                            File.Delete(input_path);
                        }
                        else if (Directory.Exists(input_path) && args.Count >= 3)
                        {
                            string initialExtension = args[2];

                            if (args.Count == 4 && args[3].ToLower() == "true")
                            {
                                isRecursive = true;
                            }

                            string[] files = Directory.GetFiles(input_path, $"*{initialExtension}");

                            foreach (string file in files)
                            {
                                try
                                {
                                    string newFilePath = Path.ChangeExtension(file, newExtension);

                                    if (newFilePath == null)
                                    {
                                        throw new InvalidOperationException("Failed to change file extension");
                                    }

                                    using (FileStream fs = File.Create(newFilePath))
                                    {
                                        if (!File.Exists(newFilePath))
                                        {
                                            throw new InvalidOperationException("Failed to create the new file");
                                        }

                                        using (FileStream originalFs = File.OpenRead(file))
                                        {
                                            originalFs.CopyTo(fs);
                                        }
                                    }

                                    File.Delete(file);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred while processing {file}: {ex.Message}");
                                }

                                if (isRecursive == true)
                                {
                                    // Recursively process each subdirectory
                                    string[] subdirectories = Directory.GetDirectories(input_path);
                                    foreach (string subdirectory in subdirectories)
                                    {
                                        changeExtension(subdirectory);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid directory, must be a valid directory");
                        }
                    }

                    changeExtension(inputPath);
                }
                else
                {
                    Console.WriteLine("Usage: chext [<input_file> <new_extension>] || [<input_directory> <new_extension> <initial_extension>]");
                }
            }
            else
            {
                Console.WriteLine($"Unknown command: {command}");
            }
        }
    }
}
