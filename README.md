# Game2048

Cross-platform implementation of the classic **2048** puzzle game.  
Built with **.NET 8** and **Avalonia UI**, tested on macOS (Apple Silicon).

---

## 📂 Project structure
Game2048/
├─ Game2048.Core/ # Core game logic (board, tiles, moves)
├─ Game2048.Desktop/ # Avalonia desktop UI (uses Core)
├─ Game2048.Core.sln # Solution for Core only
└─ Game2048.Desktop.sln # Solution for Desktop + Core

---

## 🚀 How to run (development mode)

Make sure you have **.NET 9 SDK** installed:
```bash
dotnet --version
git clone https://github.com/<your_nick>/Game2048.git
```

Run the desktop app:
```bash
dotnet run --project Game2048.Desktop
```

📦 How to publish (standalone app)

Build a self-contained app for macOS arm64 (Apple Silicon):
```bash
dotnet publish Game2048.Desktop -c Release -r osx-arm64 --self-contained true
```
The compiled binary will be in:
```bash
Game2048.Desktop/bin/Release/net9.0/osx-arm64/publish/
```
Run it directly:
```bash
./Game2048.Desktop/bin/Release/net9.0/osx-arm64/publish/Game2048.Desktop
```