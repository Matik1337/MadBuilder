using System.Collections.Generic;

namespace Extensions
{
    public static class AmplitudeExtensions
    {
        public static void LogLevelStart(this Amplitude amplitude, int level)
        {
            var eventArguments = new Dictionary<string, object>
            {
                {AmplitudeEvents.Params.Level, level}
            };
            amplitude.logEvent(AmplitudeEvents.LevelStart, eventArguments);
        }
        
        public static void LogLevelRestart(this Amplitude amplitude, int level)
        {
            var eventArguments = new Dictionary<string, object>
            {
                {AmplitudeEvents.Params.Level, level}
            };
            amplitude.logEvent(AmplitudeEvents.Restart, eventArguments);
        }
        
        public static void LogLevelComplete(this Amplitude amplitude, int level, int secondsSpent)
        {
            var eventArguments = new Dictionary<string, object>
            {
                {AmplitudeEvents.Params.Level, level}, 
                {AmplitudeEvents.Params.TimeSpent, secondsSpent}
            };
            amplitude.logEvent(AmplitudeEvents.LevelComplete, eventArguments);
        }
        
        public static void LogLevelFail(this Amplitude amplitude, int level, string reason, int secondsSpent)
        {
            var eventArguments = new Dictionary<string, object>
            {
                {AmplitudeEvents.Params.Level, level}, 
                {AmplitudeEvents.Params.Reason, reason},
                {AmplitudeEvents.Params.TimeSpent, secondsSpent}
            };
            amplitude.logEvent(AmplitudeEvents.Fail, eventArguments);
        }

        public static void LogGameStart(this Amplitude amplitude, int sessionCount)
        {
            var eventArguments = new Dictionary<string, object>
            {
                {AmplitudeEvents.SessionCount, sessionCount}
            };
            amplitude.logEvent(AmplitudeEvents.GameStart, eventArguments);
        }
    }
}