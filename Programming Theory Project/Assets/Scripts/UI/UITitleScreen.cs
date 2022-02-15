using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UITitleScreen : MonoBehaviour
{
    /// <summary>
    /// Reference to the wave selection dropdown.
    /// </summary>
    [SerializeField] private GameObject waveSelect;

    // Start is called before the first frame update
    void Start()
    {
        UpdateWaveDropdown();
    }

    /// <summary>
    /// Updates the wave selection dropdown to contain all previously reached waves.
    /// </summary>
    private void UpdateWaveDropdown()
    {
        // get dropdown component
        TMP_Dropdown dropdown = waveSelect?.GetComponent<TMP_Dropdown>();
        if(dropdown == null)
        {
            Debug.Log("wave select dropdown not assigned");
            return;
        }
        // update options in dropdown
        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int maxWave = WaveManager.Instance?.waves.highest ?? 1;
        for (int i = 0; i < maxWave; i++)
        {
            options.Add(new TMP_Dropdown.OptionData("Wave " + (i+1)));
        }
        dropdown.AddOptions(options);
        dropdown.value = (WaveManager.Instance?.waves.selected ?? 1) - 1;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Called when a wave is selected in the according dropdown.
    /// </summary>
    /// <param name="newValue">Index of the selected option (starting at 0).</param>
    public void OnWaveSelected(int newValue)
    {
        WaveManager.Instance?.SelectWave(newValue + 1);
    }

    /// <summary>
    /// Deletes the save file (and therefore progress).
    /// </summary>
    public void DeleteSave()
    {
        WaveManager.Instance?.DeleteWaveData();
    }
}
