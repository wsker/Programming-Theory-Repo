using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// Class containing all persistent wave data.
    /// </summary>
    [System.Serializable]
    public class Waves
    {
        /// <summary>
        /// The highest reached wave, ever.
        /// </summary>
        public int highest;
        /// <summary>
        /// The currently selected start wave.
        /// </summary>
        public int selected;
    }

    /// <summary>
    /// Singleton instance of the wave manager.
    /// </summary>
    public static WaveManager Instance;
    /// <summary>
    /// Persistent wave data.
    /// </summary>
    public Waves waves;

    private void Awake()
    {
        // ensure Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Save the persistent wave data to a file.
    /// </summary>
    public void SaveWaveData()  // ABSTRACTION
    {
        if (waves != null)
        {
            // if there is wave data: save it to file
            string path = Application.persistentDataPath + "/waves.json";
            string json = JsonUtility.ToJson(waves);
            File.WriteAllText(path, json);
        }
    }

    /// <summary>
    /// Load the persistent wave data from a file.
    /// </summary>
    public void LoadWaveData()
    {
        string path = Application.persistentDataPath + "/waves.json";
        if (File.Exists(path))
        {
            // if a savefile exists: load it
            string json = File.ReadAllText(path);
            waves = JsonUtility.FromJson<Waves>(json);
        }
        else
        {
            // if there is no file: instantiate empty
            waves = new Waves() { highest = 1, selected = 1 };
        }
    }

    /// <summary>
    /// Delete the save file from disk.
    /// </summary>
    public void DeleteWaveData()
    {
        string path = Application.persistentDataPath + "/waves.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Sets the currently selected wave.
    /// </summary>
    /// <param name="wave">The selected wave.</param>
    public void SelectWave(int wave)
    {
        waves.selected = wave;
        SaveWaveData();
    }

    /// <summary>
    /// Report the currently reached wave. Updates persistent data if a new
    /// highest wave was reached.
    /// </summary>
    /// <param name="wave">The reached wave.</param>
    public void ReportReachedWave(int wave)
    {
        if (wave > waves.highest)
        {
            waves.highest = wave;
            SaveWaveData();
        }
    }
}
