public static class GameEvents
{
    // Sự kiện kích hoạt khi trạng thái âm thanh thay đổi
    public static class Sound
    {
        public static System.Action<bool> OnSoundToggled;
    }

    public static class Music
    {
        public static System.Action<bool> OnMusicToggled;
    }
}