using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [SerializeField] List<Map> _maps;

    public void Save(RPGSceneManager manager)
    {
        PlayerPrefs.SetString("player", JsonUtility.ToJson(manager.Player.GetSaveData()));
        var mapName = manager.ActiveMap.name;
        mapName = mapName.Replace("(Clone)", "");
        PlayerPrefs.SetString("activeMap", mapName);
        var instantMapData = manager.ActiveMap.GetInstantSaveData();
        PlayerPrefs.SetString("instantMapData", JsonUtility.ToJson(instantMapData));

        var activeMapKey = $"map_{manager.ActiveMap.name.Replace("(Clone)", "")}";
        foreach (var map in _maps)
        {
            var key = $"map_{map.name.Replace("(Clone)", "")}";
            if(key == activeMapKey)
            {
                Save(manager.ActiveMap);
            }
            else
            {
                SaveWithTemporary(map);
            }
        }
    }

    void Save(Map map)
    {
        var key = $"map_{map.name.Replace("(Clone)", "")}";
        PlayerPrefs.SetString(key, JsonUtility.ToJson(map.GetSaveData()));
    }

    void SaveWithTemporary(Map map)
    {
        var key = $"map_{map.name.Replace("(Clone)", "")}";
        var tempKey = "temp_" + key;
        if(PlayerPrefs.HasKey(tempKey))
        {
            PlayerPrefs.SetString(key, PlayerPrefs.GetString(tempKey));
            PlayerPrefs.DeleteKey(tempKey);
        }
    }

    public void SaveTemporary(Map map)
    {
        var saveData = map.GetSaveData();
        PlayerPrefs.SetString($"temp_map_{map.name.Replace("(Clone)", "")}", JsonUtility.ToJson(saveData));
    }
}