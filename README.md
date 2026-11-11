# FileExplorer

A Windows Explorer style file browser for Windows, built with C# WinForms on .NET Framework 4.7.2.

## Features

- Explorer-style folder tree and details list (Name, Date modified, Type, Size)
- Real system icons via the Windows shell (`SHGetFileInfo`)
- Back / Forward / Up navigation with an address bar (`~` and `%ENV%` supported)
- Zip files and folders (toolbar button or right-click), extract `.zip` files with right-click
- Copy / paste and new folder in the right-click menus of the folder tree and file list
- Preview panel can be docked right or bottom (View menu)
- Custom right-click actions with file-extension filters (`.txt; .md; *.log`, `*` = all),
  arguments support `{file}` and `{files}` placeholders; stored in `%APPDATA%\FileExplorer\custom-actions.json`
- Hidable preview panel for text, JSON and XML (foldable tree view with syntax coloring, plus a raw text view) and HTML (rendered)

## Build

Open `FileExplorer.sln` in Visual Studio 2019/2022 and build. No NuGet packages are required.

## Run

Start `FileExplorer.exe` from `FileExplorer/bin/Debug` (or Release).
