using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] Dropdown resolitionDropdown;
    [SerializeField] Toggle toggle;

    int defaultQuality = 0;

    Resolution[] resolutions;

    private void Start()
    {
        defaultQuality = QualitySettings.GetQualityLevel();
        qualityDropdown.value = defaultQuality;

        resolutions = Screen.resolutions;
        resolitionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRate + "hz";

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currResolutionIndex = i;
            }
        }

        resolitionDropdown.AddOptions(options);
        resolitionDropdown.value = currResolutionIndex;
        resolitionDropdown.RefreshShownValue();

        toggle.isOn = SoundPlayer.Instance.playTutorial;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetTutorial(bool play)
    {
        SoundPlayer.Instance.playTutorial = play;
    }

}
