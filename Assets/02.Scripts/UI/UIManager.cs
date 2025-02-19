using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Canvas globalCanvas;

    protected override void Awake()
    {
        base.Awake();

        globalCanvas = transform.GetComponentsInChildren<Canvas>().FirstOrDefault();
    }

    void Start()
    {
        SceneLoadManager.Instance.OnSceneUnloaded += ClearCanvas;
    }

    public Canvas GetCanvas()
    {
        return globalCanvas;
    }

    private void ClearCanvas()
    {
        var canvasList = globalCanvas.transform.GetComponentsInChildren<RectTransform>();

        foreach (var canvas in canvasList)
        {
            if (canvas.gameObject == globalCanvas.gameObject)
                continue;

            canvas.transform.SetParent(null);
        }
    }
}