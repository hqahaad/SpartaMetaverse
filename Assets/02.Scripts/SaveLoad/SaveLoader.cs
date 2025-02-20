using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev.SaveLoad;
using System.Linq;

//¼öÁ¤ÇØ¾ßµÊ
public class SaveLoader : Singleton<SaveLoader>
{
    private IDataService service;
    public GameData data;

    protected override void Awake()
    {
        base.Awake();

        service = new FileDataService(new JsonSerializer());
    }

    void Start()
    {
        SceneLoadManager.Instance.OnSceneLoaded += () => BindAll();
    }

    void BindAll()
    {
        data = Load(data.name);

        Bind<FlappyBird, FlappyBirdData>(data.flappyBirdData);
        Bind<PlayerEntity, PositionData>(data.positionData);
    }

    public void Save(GameData newData, bool overwrite = true)
    {
        service.Save(newData, overwrite);
    }

    public void Save(bool overwrite = true)
    {
        service.Save(this.data, overwrite);
    }

    public GameData Load(string name)
    {
        data = service.Load(name);

        return data;
    }

    void Update()
    {

    }

    private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISavable, new()
    {
        var list = FindObjectsByType<T>(FindObjectsSortMode.None);

        foreach (var iter in list)
        {
            iter.Bind(data);
        }
    }
}