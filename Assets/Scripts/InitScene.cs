using System;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    private void Awake()
    {
        Amplitude amplitude = Amplitude.Instance;
        amplitude.logging = true;
        amplitude.init("655ef20319d62f205685ace904f6117b");

        if (!PlayerPrefs.HasKey(AmplitudeEvents.SessionCount))
        {
            PlayerPrefs.SetInt(AmplitudeEvents.SessionCount, 0);
            //PlayerPrefs.SetInt(AmplitudeEvents.Params.Money, 0);
            PlayerPrefs.SetInt(AmplitudeEvents.DaysInGame, 1);
            PlayerPrefs.SetInt(AmplitudeEvents.Params.DayOfRegister, DateTime.Today.DayOfYear);
            
            //Amplitude.Instance.setUserProperty(AmplitudeEvents.Params.CurrentSoft, 0);
            Amplitude.Instance.setUserProperty(AmplitudeEvents.RegDay,
                DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year);
        }
        
        DaysFuck();
        
        PlayerPrefs.SetInt(AmplitudeEvents.SessionCount, PlayerPrefs.GetInt(AmplitudeEvents.SessionCount) + 1);
        
        Amplitude.Instance.setUserProperty(AmplitudeEvents.SessionCount, PlayerPrefs.GetInt(AmplitudeEvents.SessionCount));
        Amplitude.Instance.LogGameStart(PlayerPrefs.GetInt(AmplitudeEvents.SessionCount));
        
        if (!PlayerPrefs.HasKey(AmplitudeEvents.LevelComplete))
        {
            PlayerPrefs.SetInt(AmplitudeEvents.LevelComplete, 0);
        }
        
        if(!PlayerPrefs.HasKey(AmplitudeEvents.LastLevel))
        {
            PlayerPrefs.SetInt(AmplitudeEvents.LastLevel, 1);
        }
        
        SceneManager.LoadScene(PlayerPrefs.GetInt(AmplitudeEvents.LastLevel));
    }

    private void DaysFuck()
    {
        int firstDay = PlayerPrefs.GetInt(AmplitudeEvents.Params.DayOfRegister);
        int currentDay = DateTime.Today.DayOfYear;
        
        Amplitude.Instance.setUserProperty(AmplitudeEvents.DaysInGame, currentDay - firstDay);
    }
}

