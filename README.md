# Sorter

A WPF desktop app (C# / .NET 8, MVVM) for building ranked leaderboards from lists of names and points. You can type entries in by hand or import them from a photo of a printed table. The app reads the photo with OCR, sorts the entries, assigns placements (including shared ranks such as `3.-5.`), and can export the result as an image.

## Features

- **Manual entry:** add, edit and remove name/points rows in an editable data grid.
- **Import from image:** pick a `.jpg`/`.png` photo of a table. Python + OpenCV preprocess it (grayscale + binary threshold), then Tesseract OCR extracts each name and its points.
- **Sort & rank:** sorts entries by points, highest first, using a counting sort. Tied entries get a shared rank range (e.g. `4.-6.`).
- **Shuffle:** randomly reorders the list.
- **Undo / Redo:** keeps a history of list states on two stacks.
- **Save / Open:** stores and loads lists as JSON (`Save`, `Save As`, `Open`).
- **Export to PNG:** renders the ranked list to an image.
- **Clear:** empties the list.
- Custom borderless window with its own styles, icons and open/close animations.

## Project structure

```
MVVM.sln
MVVM/
├── Model/                 Item (Name, Points, Order), undo/redo collector
├── View/                  Edit, Error, Info, Loading and Success windows
├── ViewModel/             MainWindowViewModel, RelayCommand, sorting extension, helpers
├── CustomUserControls/    Data-grid textbox, window title-bar controls
├── Python/
│   ├── PythonService.cs   Calls the Python scripts via pythonnet
│   ├── tresholding.py     Image preprocessing (OpenCV)
│   └── jsonexport.py      OCR (pytesseract) → JSON [{ "Name", "Points" }]
├── Resources/             XAML vector icons, sample images for the import help dialog
└── Styles/                XAML resource dictionaries
```

## Requirements

- Windows
- Visual Studio 2022 or newer with the **.NET desktop development** workload (.NET 8). The project has a COM reference, so it must be built with Visual Studio's MSBuild; `dotnet build` / `dotnet run` fail with error MSB4803.
- For image import only:
  - **Python 3.12** installed at `C:\Program Files\Python312\` (this path is hard-coded in `MVVM/Python/PythonService.cs`; change it if your Python is somewhere else)
  - Python packages: `opencv-python`, `pytesseract`, `Pillow`
  - [Tesseract OCR](https://github.com/UB-Mannheim/tesseract/wiki) on your `PATH`, **with the Czech language data (`ces`)** installed

```powershell
pip install opencv-python pytesseract Pillow
```

## Build & run

```powershell
git clone https://github.com/Gaspy10/ScoreSorter.git
```

Open `MVVM.sln` in Visual Studio and press **F5**.

To build from the command line, use a Developer PowerShell for Visual Studio:

```powershell
msbuild MVVM.sln -restore
.\MVVM\bin\Debug\net8.0-windows\MVVM.exe
```

## Usage tips

- Crop the photo before you import it, so that it contains only the name and points columns. The import dialog shows an example of a good crop and a bad one.
- Points must be non-negative whole numbers.
- Saved JSON files have this shape:

  ```json
  [
    { "Name": "Alice", "Points": 42 },
    { "Name": "Bob",   "Points": 37 }
  ]
  ```

## Tech stack

- WPF, .NET 8, MVVM pattern
- [pythonnet](https://github.com/pythonnet/pythonnet) for calling Python from C#
- OpenCV, pytesseract / Tesseract OCR
- Microsoft.Xaml.Behaviors.Wpf, System.Drawing.Common
