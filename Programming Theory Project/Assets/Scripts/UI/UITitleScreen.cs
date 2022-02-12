using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UITitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject waveSelect;

    // Start is called before the first frame update
    void Start()
    {
        UpdateWaveDropdown();
    }

    private void UpdateWaveDropdown()
    {
        TMP_Dropdown dropdown = waveSelect?.GetComponent<TMP_Dropdown>();
        if(dropdown == null)
        {
            Debug.Log("wave select dropdown not assigned");
            return;
        }

        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int maxWave = WaveManager.Instance?.waves.highest ?? 1;
        for (int i = 0; i < maxWave; i++)
        {
            options.Add(new TMP_Dropdown.OptionData("Wave " + (i+1)));
        }
        dropdown.AddOptions(options);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void WaveSelected(int newValue)
    {
        WaveManager.Instance?.SelectWave(newValue + 1);
    }

    public void DeleteSave()
    {
        WaveManager.Instance?.DeleteWaveData();
    }
}
