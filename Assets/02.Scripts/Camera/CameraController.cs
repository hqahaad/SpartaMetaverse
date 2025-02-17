using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dev.CameraAction;

public class CameraController : MonoBehaviour
{
    private List<ICameraAction> actions = new();
    private ICameraAction currentAction;

    void Awake()
    {
        currentAction = new NormalFollowGameObject(GameObject.Find("Player"));
        currentAction.SetCamera(this.GetComponent<Camera>());
    }

    void Update()
    {
        currentAction?.UpdateCamera();
    }

    void FixedUpdate()
    {
        currentAction?.FixedUpdateCamera();
    }
}

namespace Dev.CameraAction
{
    public class NormalFollowGameObject : ICameraAction
    {
        private Camera camera;
        private GameObject followingGameObject;

        public NormalFollowGameObject(GameObject go)
        {
            followingGameObject = go;
        }

        public void UpdateCamera()
        {

        }

        public void FixedUpdateCamera()
        {
            var trans = followingGameObject.transform;
            camera.transform.position = new Vector3(trans.position.x, trans.position.y, camera.transform.position.z);
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }
    }

}