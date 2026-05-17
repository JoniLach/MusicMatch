# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

**Build:** Open `MusicMatch.sln` in Visual Studio 2017+ and press F5, or via CLI:
```
msbuild MusicMatch.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

**Run (WPF app):** Set `MusicMatch` as the startup project and run. Output binary: `MusicMatch/bin/Debug/MusicMatch.exe`

**Run (Web API):** Set `WebServices` as the startup project and run. Serves on `http://localhost:50954/`. API docs available at `http://localhost:50954/Help`.

**No test projects exist** in this solution.

## Project Structure

This is a four-project Visual Studio solution targeting **.NET Framework 4.7.2**:

- **`MusicMatch/`** — WPF application (XAML pages + code-behind, converters, image helper)
- **`Model/`** — Domain entities (`User` (abstract) → `Student`/`Teacher`, `Lesson`, `Instrument`, `Notification`)
- **`ViewModel/`** — Misleadingly named; this is the **Data Access Layer (DAL)**, not MVVM ViewModels. Contains `*DB.cs` classes and the Access database file.
- **`WebServices/`** — ASP.NET Web API 5 (MVC 5) project exposing REST endpoints backed by the same Model and ViewModel projects.

## Architecture

**Not MVVM.** Business logic lives directly in XAML page code-behind files. The `ViewModel/` project name is historical — it holds DB access classes, not presentation logic.

**Navigation** is frame-based. `MainWindow.xaml` holds a `Frame` control; pages navigate by calling:
```csharp
MainWindow.MainFrame.Navigate(new SomePage());
```
Pages reach the main window via `Window.GetWindow(this)`.

**Authenticated user** is held in a static property: `MainWindow.LoggedInUser`. Many pages branch on `if (LoggedInUser is Student)` vs `Teacher`.

**Database** is Microsoft Access (`ViewModel/MusicMatchDB.accdb`), accessed via OleDb (`System.Data.OleDb`). The connection string is a hardcoded relative path:
```
..\..\..\..\ViewModel\MusicMatchDB.accdb
```
Each entity has a corresponding `*DB` class inheriting from `BaseDB`, which provides CRUD and change tracking via `inserted`/`updated`/`deleted` lists flushed with `SaveChanges()`.

**Image handling** is managed by `ImageHelper.cs`. Profile pictures are saved to the `MusicMatch/Images/` folder and stored in the DB as relative paths. The `PathToImageConverter` in `Converters.cs` resolves these for WPF binding.

**UI theming** uses MaterialDesignThemes (dark theme, green/lime palette), configured globally in `App.xaml`.

## Web API (WebServices project)

**Stack:** ASP.NET Web API 5.2.9 + MVC 5, Newtonsoft.Json 13, running on IIS Express port 50954.

**Route conventions:**
- REST API: `api/{controller}/{id}` (attribute routing also enabled)
- MVC views: `{controller}/{action}/{id}` (defaults to `HomeController/Index`)
- Auto-generated docs: `/Help`

**Controllers:**
- `UserLoginController` — user authentication endpoint (in progress, not yet implemented)
- `ValuesController` — sample CRUD template (can be removed or repurposed)
- `HomeController` — serves the default MVC index view

**Project references:** `WebServices` references both `Model` and `ViewModel`, so it shares domain entities and database access classes (OleDb → `MusicMatchDB.accdb`) with the WPF app. No duplicate DB logic needed.