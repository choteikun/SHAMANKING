using Cysharp.Threading.Tasks;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Datamanager;
using Gamemanager;
namespace Datamanager
{
    public class DataManager
    {
        public DataGroup DataGroup = new DataGroup();
        public ReaTimePlayerData reaTimePlayerData = new ReaTimePlayerData();
        public async Task InitDataMananger()
        {
            var CSVString = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>("CSV/ShamanKingCSV");
            var stringData = await CSVClassGenerator.GenClassArrayByCSV<DatasPath>(CSVString);
            PropertyInfo[] propertyInfo = typeof(DataGroup).GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                var dataObj = propertyInfo[i].GetValue(DataGroup);
                Type type;
                if (dataObj is IDataBase database)
                {
                    type = database.ThisDataType;
                    var CSVstring = await AddressableSearcher.GetAddressableAssetAsync<TextAsset>(stringData[i].Path);
                    var List = await CSVClassGenerator.GenClassArrayByCSV(type, CSVstring);
                    database.DataArray = List;
                }
            }
            await UniTask.Delay(100);
            //GameManager.Instance.GameFinishInit = true;
            Debug.Log("FinishInit");
        }
        public T GetDataByID<T>(int id) where T : class
        {
            var database = DataGroup.TryGetDataBase<T>();
            foreach (var item in database.DataArray)
            {
                if (item is not IWithIdData data)
                {
                    throw new UnityException("Not a IWithIdData");
                }
                if (data.Id == id)
                {
                    return data as T;
                }
            }
            return null;
        }
        public T GetDataByName<T>(string name) where T : class
        {
            var database = DataGroup.TryGetDataBase<T>();
            foreach (var item in database.DataArray)
            {
                if (item is not IWithNameData data)
                {
                    throw new UnityException("Not a IWithNameData");
                }
                if (data.Name == name)
                {
                    return data as T;
                }
            }
            return null;
        }
        //public float GetSpeedPercentageByHeldingCoin(int coinCount)
        //{
        //    for (int i = 0; i < DataGroup.GuardianSpeedPercentageDataBase.DataArray.Length; i++)
        //    {
        //        var templete = GetDataByID<GuardianSpeedPercentageByCoinThresholdTemplete>(i);
        //        if (coinCount >= templete.CoinThreshold)
        //        {
        //            return templete.SpeedRatio;
        //        }
        //    }
        //    return 1f;
        //}
        //public float GetDropedCoinPercentageByHeldingCoin(int coinCount)
        //{
        //    for (int i = 0; i < DataGroup.PlayerDropedCoinPercentageDataBase.DataArray.Length; i++)
        //    {
        //        var templete = GetDataByID<PlayerDropedCoinPercentageByCoinThresholdTemplete>(i);
        //        if (coinCount >= templete.CoinThreshold)
        //        {
        //            return templete.DropedRatio;
        //        }
        //    }
        //    return 1f;
        //}
    }
}

