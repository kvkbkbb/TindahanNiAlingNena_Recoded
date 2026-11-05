using System.Media;

namespace TindahanNiAlingNena_Recoded.Services;

public static class AudioPlayer
{
    private static readonly Dictionary<string, SoundPlayer> Players = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object PlayersLock = new();

    public static void PlaySound(string soundFileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(soundFileName);
        if (Path.GetFileName(soundFileName) != soundFileName)
        {
            throw new ArgumentException("Provide a sound file name, not a path.", nameof(soundFileName));
        }

        string soundPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Sounds", soundFileName);
        if (!File.Exists(soundPath))
        {
            throw new FileNotFoundException($"Sound file was not found: {soundPath}", soundPath);
        }

        SoundPlayer player;
        lock (PlayersLock)
        {
            if (!Players.TryGetValue(soundFileName, out player!))
            {
                player = new SoundPlayer(soundPath);
                player.Load();
                Players.Add(soundFileName, player);
            }
        }

        player.Play();
    }
}
