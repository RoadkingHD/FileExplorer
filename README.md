# FileExplorer

A Windows Explorer style file browser for Windows, built with C# WinForms on .NET Framework 4.7.2.

## Features

- Explorer-style folder tree and details list (Name, Date modified, Type, Size)
- Real system icons via the Windows shell (`SHGetFileInfo`)
- Back / Forward / Up navigation with an address bar (`~` and `%ENV%` supported)
- Zip files and folders (toolbar button or right-click), extract `.zip` files with right-click
- Copy / cut / paste (paste-move for cut items, shell `Preferred DropEffect` compatible with Explorer) and new folder in the right-click menus of the folder tree and file list
- Rename files and folders (F2 or right-click), with conflict and invalid-character checks
- Drag & drop into the list: drop from Explorer to copy, drag within the list onto a folder to move
- Preview panel can be docked right or bottom (View menu)
- Custom right-click actions with file-extension filters (`.txt; .md; *.log`, `*` = all),
  arguments support `{file}` and `{files}` placeholders; stored in `%APPDATA%\FileExplorer\custom-actions.json`
- Hidable preview panel for text, JSON and XML (foldable tree view with syntax coloring, plus a raw text view), HTML (rendered), PDF (Adobe Reader) and Word / Excel / PowerPoint documents (converted via Office COM automation)
- PE browser preview for `.exe` / `.dll`: machine, subsystem, sections, imports, exports, and managed vs. native detection — for managed images the CLR runtime version and target framework (e.g. `.NETFramework,Version=v4.7.2`, `.NETCoreApp,Version=v8.0`) are shown
- File search (Ctrl+F or the Search toolbar button): recursive search with wildcards (`*.txt`, `data??.csv`, multiple patterns separated by `;`) and optional full-text search inside files (with match-case option, line numbers and matching-line preview); double-click a result to open it in the browser
- Compare two files or two folders (Compare toolbar button or Tools menu): line-based text diff with color coding for text files, side-by-side hex view with highlighted differences for binary files, and a recursive folder comparison (only in left / only in right / different / same) with drill-down into file pairs

## Build

Open `FileExplorer.sln` in Visual Studio 2019/2022 and build. No NuGet packages are required.

## Run

Start `FileExplorer.exe` from `FileExplorer/bin/Debug` (or Release).
