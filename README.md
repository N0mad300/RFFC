# Remedy File Formats Converter

C# CLI program to convert Remedy Enetertainment games files to common file formats

## Commands

#### convert

```
RFFC.exe convert <game> <input_format> <output_format> <input_path> <output_path> <recursive (only for directory)>
```

Can be use to convert :

- File
- Files in a directory
- Recursive (Files in a directory and subdirectories)

Example :

AW2-Tools is the directory in which i have installed [**Alan-Wake-2-RMDTOC-Tool**](https://github.com/amrshaheen61/Alan-Wake-2-RMDTOC-Tool) to export the **textures** folder in a `output` directory

This command will convert recursively the files of the`world_map` directory that contain .tex files to .png files into another directory

```
RFFC.exe convert aw2 tex png E:\Programmes\AW2-Tools\Output\textures\world_map E:\Programmes\AW2-Tools\\Converted\textures\world_map true
```

#### chext

To change the extension of a file :

```
RFFC.exe chext <input_file> <new_extension>
```

To change the extension of files of a directory :

```
RFFC.exe chext <input_directory> <new_extension> <initial_extension> <recursive>
```

#### Conversion formats

- Alan Wake 2
  - Textures (.tex) -> PNG (.png), JPG (.jpg), Bitmap (.bmp)
- Control
  - WIP
- Quantum Break
  - WIP

## Credits

Thanks to [**amrshaheen61**](https://github.com/amrshaheen61) for making the **Alan-Wake-2-RMDTOC-Tool** that i have use to unpack the `.rmdtoc` and `.rmdblob` and for have made the [original](https://github.com/amrshaheen61/Alan-Wake-2-RMDTOC-Tool/tree/master/Helper) DDS [Helper](./RFFC/Helper/DDS/) files
