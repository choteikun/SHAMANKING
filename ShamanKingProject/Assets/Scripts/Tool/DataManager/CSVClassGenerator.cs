using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Datamanager
{
    public class CSVClassGenerator
    {

        public static async Task<T[]> GenClassArrayByCSV<T>(TextAsset textAsset) where T : new()
        {
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            string[][] tempdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                tempdata[i] = datastring;
            }
            var resultArray = new T[tempdata.Length - 1];
            for (int i = 1; i < tempdata.Length; i++)
            {
                var result = new T();
                await SetClassData<T>(result, tempdata[i]);
                resultArray[i - 1] = result;
            }
            return resultArray;
        }
        public static async Task<object[]> GenClassArrayByCSV(Type type, TextAsset textAsset)
        {
            Debug.Log(type.Name);
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            string[][] tempdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                tempdata[i] = datastring;
            }
            var resultArray = new object[tempdata.Length - 1];
            //var resultArray = Array.CreateInstance(type, tempdata.Length-1);
            for (int i = 1; i < tempdata.Length; i++)
            {
                var result = Activator.CreateInstance(type);
                await SetClassData(type, result, tempdata[i]);
                resultArray[i - 1] = result;
            }
            return resultArray;
        }
        public static string[][] GenStringArrayFromCsvData(TextAsset textAsset)
        {
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            string[][] tempdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                tempdata[i] = datastring;
            }
            return tempdata;
        }
        public static List<string>[] GenStringListFromCsvData(TextAsset textAsset)
        {
            string[] data = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            List<string>[] tempdata = new List<string>[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                List<string> datastring = data[i].Split(new string[] { "," }, System.StringSplitOptions.None).ToList<string>();
                tempdata[i] = datastring;
            }
            return tempdata;
        }
        public static async Task SetClassData<T>(T DataBeSet, string[] dataText)
        {
            PropertyInfo[] propertyInfo = typeof(T).GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (propertyInfo[i].PropertyType == typeof(string))
                {
                    propertyInfo[i].SetValue(DataBeSet, dataText[i].ToString());
                }
                else if (propertyInfo[i].PropertyType == typeof(int))
                {
                    propertyInfo[i].SetValue(DataBeSet, int.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(float))
                {
                    propertyInfo[i].SetValue(DataBeSet, float.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(GameObject))
                {
                    var gameobjectPrefab = await AddressableSearcher.GetAddressableAssetAsync<GameObject>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, gameobjectPrefab);
                }
                else if (propertyInfo[i].PropertyType == typeof(bool))
                {
                    if (dataText[i].ToString() == "TRUE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, true);
                    }
                    else if (dataText[i].ToString() == "FALSE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, false);
                    }
                }
                else if (propertyInfo[i].PropertyType == typeof(Sprite))
                {
                    var sprite = await AddressableSearcher.GetAddressableAssetAsync<Sprite>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, sprite);
                }
                else if (propertyInfo[i].PropertyType == typeof(AudioClip))
                {
                    var audio = await AddressableSearcher.GetAddressableAssetAsync<AudioClip>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, audio);
                }
                else if (propertyInfo[i].PropertyType == typeof(string[][]))
                {
                    var data = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>(dataText[i]);
                    var parseData = GenStringArrayFromCsvData(data);
                    propertyInfo[i].SetValue(DataBeSet, parseData);
                }
                else if (propertyInfo[i].PropertyType == typeof(List<object>))
                {
                    var typeData = dataText[i];
                    var data = genAListByStringAndType(dataText[i]);
                    listParser(propertyInfo[i], data, typeData, DataBeSet);
                }
            }
        }
        public static async Task SetClassData(Type type, object DataBeSet, string[] dataText)
        {
            Debug.Log(type.Name);
            PropertyInfo[] propertyInfo = type.GetProperties();
            Debug.Log(dataText.ToString());
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                Debug.Log(propertyInfo[i].Name);
                if (propertyInfo[i].PropertyType == typeof(string))
                {
                    propertyInfo[i].SetValue(DataBeSet, dataText[i].ToString());
                }
                else if (propertyInfo[i].PropertyType == typeof(int))
                {
                    propertyInfo[i].SetValue(DataBeSet, int.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(float))
                {
                    propertyInfo[i].SetValue(DataBeSet, float.Parse(dataText[i]));
                }
                else if (propertyInfo[i].PropertyType == typeof(GameObject))
                {
                    var gameobjectPrefab = await AddressableSearcher.GetAddressableAssetAsync<GameObject>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, gameobjectPrefab);
                }
                else if (propertyInfo[i].PropertyType == typeof(bool))
                {
                    if (dataText[i].ToString() == "TRUE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, true);
                    }
                    else if (dataText[i].ToString() == "FALSE")
                    {
                        propertyInfo[i].SetValue(DataBeSet, false);
                    }
                }
                else if (propertyInfo[i].PropertyType == typeof(Sprite))
                {
                    var sprite = await AddressableSearcher.GetAddressableAssetAsync<Sprite>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, sprite);
                }
                else if (propertyInfo[i].PropertyType == typeof(AudioClip))
                {
                    var audio = await AddressableSearcher.GetAddressableAssetAsync<AudioClip>(dataText[i]);
                    propertyInfo[i].SetValue(DataBeSet, audio);
                }
                else if (propertyInfo[i].PropertyType == typeof(string[][]))
                {
                    var data = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>(dataText[i]);
                    var parseData = GenStringArrayFromCsvData(data);
                    propertyInfo[i].SetValue(DataBeSet, parseData);
                }
                else if (propertyInfo[i].PropertyType == typeof(List<object>))
                {
                    var content = dataText[i].Trim('(', ')');
                    var dataArray = content.Split(';');
                    var typeData = dataArray[0];
                    var data = genAListByStringAndType(dataText[i]);
                    listParser(propertyInfo[i], data, typeData, DataBeSet);
                }
            }
        }

        static void listParser(PropertyInfo property, List<object> data, string type, object DataBeSet)
        {
            switch (type)
            {
                case "bool":
                    List<bool> boolList = data.Select(ToBool).ToList();
                    property.SetValue(DataBeSet, data);
                    return;
            }
        }
        static bool ToBool(object item)
        {
            if (item is bool)
                return (bool)item;
            throw new ArgumentException($"無法將物件轉換為bool：{item}");
        }
        private static List<object> genAListByStringAndType(string data)
        {
            var content = data.Trim('(', ')');
            var dataArray = content.Split(';');
            var typeData = dataArray[0];
            var result = new List<object>();
            switch (typeData)
            {
                case "bool":
                    for (int i = 1; i < dataArray.Length; i++)
                    {
                        if (dataArray[i] == "TRUE")
                        {
                            result.Add(true);
                        }
                        else
                        {
                            result.Add(false);
                        }
                    }
                    break;
            }
            return result;
        }
    }
}

