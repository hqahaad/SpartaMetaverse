using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev.SaveLoad;

public class SaveLoader : Singleton<SaveLoader>
{
    private IDataService service;
    public GameData data;

    protected override void Awake()
    {
        base.Awake();

        service = new FileDataService(new JsonSerializer());
    }

    public void Save(GameData data, bool overwrite = true)
    {
        service.Save(data, overwrite);
    }

    public GameData Load(string name)
    {
        return service.Load(name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bind<TestMono, PositionData>();
        }
    }

    private void Bind<T, TData>() where T : MonoBehaviour, IBind<TData> where TData : ISavable, new()
    {
        var list = FindObjectsByType<T>(FindObjectsSortMode.None);

        foreach (var iter in list)
        {
            iter.Bind(new());
        }
    }
}
