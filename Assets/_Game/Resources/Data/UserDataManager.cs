using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private string dataFilePath;

    [Serializable]
    public class UserData
    {
        public int level = 1;
        public int coin = 100;
        public WeaponType playerWeapon = WeaponType.Wp_Candy_1;
        public PantType playerPant = PantType.Pant_dabao;
        public AccessoryType playerAccessory = AccessoryType.ACC_None;
        public HatType playerHat = HatType.HAT_None;
        public SkinType playerSkin = SkinType.SKIN_Normal;

        public int[] weaponStateList = new int[Enum.GetNames(typeof(WeaponType)).Length];
        public int[] pantStateList = new int[Enum.GetNames(typeof(PantType)).Length];
        public int[] accessoryStateList = new int[Enum.GetNames(typeof(AccessoryType)).Length];
        public int[] hatStateList = new int[Enum.GetNames(typeof(HatType)).Length];
        public int[] skinStateList = new int[Enum.GetNames(typeof(SkinType)).Length];
    }

    [SerializeField]
    public UserData userData;

    private void Awake()
    {
        // Khởi tạo đường dẫn file
        dataFilePath = Path.Combine(Application.persistentDataPath, "userdata.json");
        Debug.Log(dataFilePath);

        // Tải dữ liệu khi bắt đầu
        LoadUserData();
    }

    public void SaveUserData()
    {
        string jsonData = JsonUtility.ToJson(userData);
        File.WriteAllText(dataFilePath, jsonData);
    }

    public void LoadUserData()
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            userData = JsonUtility.FromJson<UserData>(jsonData);
        }
        else
        {
            userData = new UserData();
            SaveUserData();
        }
    }

    public void SetEnumData<T>(string key, T value) where T : Enum
    {
        userData.GetType().GetField(key).SetValue(userData, value);
        SaveUserData();
    }

    public T GetEnumData<T>(string key, T defaultValue) where T : Enum
    {
        var field = userData.GetType().GetField(key);
        if (field != null && field.GetValue(userData) is T enumValue)
        {
            return enumValue;
        }
        return defaultValue;
    }

    public void SetDataState(string key, int index, int state)
    {
        var field = userData.GetType().GetField(key);
        if (field != null && field.FieldType == typeof(int[]))
        {
            var array = (int[])field.GetValue(userData);
            if (index >= 0 && index < array.Length)
            {
                array[index] = state;
                SaveUserData();
            }
            else
            {
                Debug.LogError($"Invalid index: {index} for array {key}");
            }
        }
        else
        {
            Debug.LogError($"Invalid key: {key} (not an int[] field)");
        }
    }

    public int GetDataState(string key, int index)
    {
        var field = userData.GetType().GetField(key);
        if (field != null && field.FieldType == typeof(int[]))
        {
            var array = (int[])field.GetValue(userData);
            if (index >= 0 && index < array.Length)
            {
                return array[index];
            }
            else
            {
                Debug.LogError($"Invalid index: {index} for array {key}");
                return -1;
            }
        }
        else
        {
            Debug.LogError($"Invalid key: {key} (not an int[] field)");
            return -1;
        }
    }
}

