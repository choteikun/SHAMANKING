using Datamanager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Datamanager
{
    public class DataGroup
    {
        public DataBase<GameEffectTemplete> GameEffectDataBase => TryGetDataBase<GameEffectTemplete>(); 
        //public DataBase<PlayerDataBaseTemplete> PlayerDataBase => TryGetDataBase<PlayerDataBaseTemplete>(); //最重要
        //public DataBase<MapDataStringTemplete> MapDataBase => TryGetDataBase<MapDataStringTemplete>(); //遊戲
        //public DataBase<CubeDataTemplete> CubeDataBase => TryGetDataBase<CubeDataTemplete>(); //遊戲
        //public DataBase<ItemDataBaseTemplete> ItemDataBase => TryGetDataBase<ItemDataBaseTemplete>(); //遊戲
        public DataBase<SoundEffectDatabaseTemplete> SoundEffectDatabase => TryGetDataBase<SoundEffectDatabaseTemplete>(); //最重要
        //public DataBase<AnimationDetailDatabaseTemplete> AnimationDetailDatabase => TryGetDataBase<AnimationDetailDatabaseTemplete>(); //遊戲
        //public DataBase<GameEndConditionTemplete> GameEndConditionDatabase => TryGetDataBase<GameEndConditionTemplete>(); //遊戲
        //public DataBase<HitEventConditionTemplete> HitEventConditionTemplete => TryGetDataBase<HitEventConditionTemplete>(); //遊戲
        //public DataBase<SkillTemplete> SkillDatabase => TryGetDataBase<SkillTemplete>(); //遊戲
        //public DataBase<EffectTemplete> EffectDatabase => TryGetDataBase<EffectTemplete>(); //遊戲
        //public DataBase<GuardianSpeedPercentageByCoinThresholdTemplete> GuardianSpeedPercentageDataBase => TryGetDataBase<GuardianSpeedPercentageByCoinThresholdTemplete>(); 
        //public DataBase<PlayerDropedCoinPercentageByCoinThresholdTemplete> PlayerDropedCoinPercentageDataBase => TryGetDataBase<PlayerDropedCoinPercentageByCoinThresholdTemplete>();

        List<IDataBase> databases = new List<IDataBase>();

        public DataBase<T> TryGetDataBase<T>() where T : class
        {
            DataBase<T> result = null;
            foreach (var item in databases)
            {
                if (item is DataBase<T> data)
                {
                    return data;
                }
            }
            if (result == null)
            {
                result = new DataBase<T>();
                databases.Add(result);
            }
            return result;
        }

    }

    public class DataBase<T> : IDataBase where T : class
    {
        public object[] DataArray { get; set; }
        public Type ThisDataType
        {
            get => typeof(T);
            set => throw new Exception();
        }
    }
    public interface IWithIdData
    {
        public int Id { get; set; }
    }
    public interface IWithNameData
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class DatasPath
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Path { get; set; }
    }
}

public class GameEffectTemplete : IWithIdData, IWithNameData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public GameObject PrefabPath { get; set; }

    public GameEffectTemplete Clone()
    {
        var result = new GameEffectTemplete()
        {
            Name = Name,
            Id = Id,
            PrefabPath = PrefabPath
        };
        return result;
    }
}

#region Boomer
public class MapDataStringTemplete : IWithIdData, IWithNameData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string[][] MapaDataString { get; set; }
    public MapDataStringTemplete Clone()
    {
        var result = new MapDataStringTemplete()
        {
            Name = Name,
            Id = Id,
            MapaDataString = MapaDataString,
        };
        return result;
    }
}
public class ItemDataBaseTemplete : IWithNameData, IWithIdData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public GameObject PrefabPath { get; set; }
    public string ItemControllerType { get; set; }
    public int ItemUseTimes { get; set; }
    public float LifeTime { get; set; }
    public float Value { get; set; }
    public GameObject BuffPrefabPath { get; set; }

    public ItemDataBaseTemplete Clone()
    {
        return new ItemDataBaseTemplete()
        {
            Name = Name,
            Id = Id,
            PrefabPath = PrefabPath,
            ItemControllerType = ItemControllerType,
            ItemUseTimes = ItemUseTimes,
            LifeTime = LifeTime,
            Value = Value,
            BuffPrefabPath = BuffPrefabPath,
        };
    }
}

public class SoundEffectDatabaseTemplete : IWithNameData, IWithIdData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public AudioClip SoundEffect { get; set; }

    public SoundEffectDatabaseTemplete Clone()
    {
        return new SoundEffectDatabaseTemplete()
        {
            Name = Name,
            Id = Id,
            SoundEffect = SoundEffect,
        };
    }
}

public class AnimationDetailDatabaseTemplete : IWithNameData
{
    public string Name { get; set; }
    public int AnimationFrameDuration { get; set; }
    public AnimationDetailDatabaseTemplete Clone()
    {
        return new AnimationDetailDatabaseTemplete()
        {
            Name = Name,
            AnimationFrameDuration = AnimationFrameDuration,
        };
    }
}
public class GameEndConditionTemplete : IWithNameData
{
    public string Name { get; set; }
    public int GameEndNeedCoin { get; set; }
    public float GameTotalTime { get; set; }
    public int GameUsedMapID { get; set; }
    public GameEndConditionTemplete Clone()
    {
        return new GameEndConditionTemplete()
        {
            Name = Name,
            GameEndNeedCoin = GameEndNeedCoin,
            GameTotalTime = GameTotalTime,
            GameUsedMapID = GameUsedMapID
        };
    }
}

public class HitEventConditionTemplete : IWithNameData
{
    public string Name { get; set; } //AttackerName
    public List<object> ConditionBool { get; set; }
    public bool GetBeAttackerBool(HurtCondition beAttacker)
    {
        bool result = default;
        result = (bool)ConditionBool[(int)beAttacker];
        return result;
    }

    public HitEventConditionTemplete Clone()
    {
        return new HitEventConditionTemplete()
        {
            Name = Name,
            ConditionBool = ConditionBool
        };
    }
}

[Serializable]
public enum HurtCondition
{
    Ninja, Guard, Coin, Item, Cube
}
[Serializable]
public enum HitCondition
{
    Ninja, Guard, NinjaCollecter, GuardianCollecter
}
public class GuardianSpeedPercentageByCoinThresholdTemplete : IWithIdData
{
    public int Id { get; set; }
    public int CoinThreshold { get; set; }
    public float SpeedRatio { get; set; }
    public GuardianSpeedPercentageByCoinThresholdTemplete Clone()
    {
        return new GuardianSpeedPercentageByCoinThresholdTemplete()
        {
            Id = Id,
            CoinThreshold = CoinThreshold,
            SpeedRatio = SpeedRatio,
        };
    }
}

public class PlayerDropedCoinPercentageByCoinThresholdTemplete : IWithIdData
{
    public int Id { get; set; }
    public int CoinThreshold { get; set; }
    public float DropedRatio { get; set; }
    public PlayerDropedCoinPercentageByCoinThresholdTemplete Clone()
    {
        return new PlayerDropedCoinPercentageByCoinThresholdTemplete()
        {
            Id = Id,
            CoinThreshold = CoinThreshold,
            DropedRatio = DropedRatio,
        };
    }
}
public class SkillTemplete : IWithNameData
{
    public string Name { get; set; }
    public GameObject SkillPrefabPath { get; set; }
    public GameObject SkillEffectPrefabPath { get; set; }
    public SkillTemplete Clone()
    {
        return new SkillTemplete()
        {
            Name = Name,
            SkillPrefabPath = SkillPrefabPath,
            SkillEffectPrefabPath = SkillEffectPrefabPath
        };
    }
}

public class EffectTemplete : IWithNameData
{
    public string Name { get; set; }
    public GameObject EffectPrefabPath { get; set; }
    public EffectTemplete Clone()
    {
        return new EffectTemplete()
        {
            Name = Name,
            EffectPrefabPath = EffectPrefabPath,
        };
    }
}

#endregion

