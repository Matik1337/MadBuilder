public static class AmplitudeEvents
{
    public const string GameStart = "game_start";
    public const string LevelStart = "level_start";
    public const string LevelComplete = "level_complete";
    public const string Fail = "fail";
    public const string Restart = "restart";
    public const string MainMenu = "main_menu";
    public const string CurrentSoft = "current_soft";
    public const string SessionCount = "session_count";
    public const string RegDay = "reg_day";
    public const string LastLevel = "last_level";
    public const string DaysInGame = "days_in_game";

    public static class Params
    {
        public const string Level = "level";
        public const string Reason = "reason";
        public const string TimeSpent = "time_spent";
    }
}
