using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Waves
    {
        public int highest;
        public int selected;
        public int current;
    }

    public static WaveManager Instance;
    public Waves waves;

    private void Awake()
    {
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

    public void SaveWaveData()
    {
        if (waves != null)
        {
            // if there is a current highscore: save it to file
            string path = Application.persistentDataPath + "/waves.json";
            string json = JsonUtility.ToJson(waves);
            File.WriteAllText(path, json);
            Debug.Log("saved to " + path);
        }
    }

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
            waves = new Waves() { highest = 1, selected = 1, current = 1 };
        }
    }

    public void DeleteWaveData()
    {
        string path = Application.persistentDataPath + "/waves.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void SelectWave(int wave)
    {
        waves.selected = wave;
        SaveWaveData();
    }

    public void ReportReachedWave(int wave)
    {
        waves.current = wave;
        if (wave > waves.highest)
        {
            waves.highest = wave;
            SaveWaveData();
        }
    }
}
