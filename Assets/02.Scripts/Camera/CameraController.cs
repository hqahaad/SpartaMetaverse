using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev.CameraAction;

public class CameraController : MonoBehaviour
{
    private List<ICameraAction> _actions = new();
    private ICameraAction _currentAction;

    void Awake()
    {
        _currentAction = new NormalFollowGameObject(GameObject.Find("Player"));
        _currentAction.SetCamera(this.GetComponent<Camera>());
    }

    void Update()
    {
        _currentAction?.UpdateCamera();
    }

    void FixedUpdate()
    {
        _currentAction?.FixedUpdateCamera();
    }
}

namespace Dev.CameraAction
{
    public class NormalFollowGameObject : ICameraAction
    {
        private Camera _camera;
        private GameObject _followingGameObject;

        public NormalFollowGameObject(GameObject go)
        {
            _followingGameObject = go;
        }

        public void UpdateCamera()
        {

        }

        public void FixedUpdateCamera()
        {
            var trans = _followingGameObject.transform;
            //camera.transform.position = new Vector3(trans.position.x, trans.position.y, camera.transform.position.z);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(trans.position.x, trans.position.y, _camera.transform.position.z), 0.5f);
        }

        public void SetCamera(Camera camera)
        {
            this._camera = camera;
        }
    }

}