using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.KC
{
    public interface IKCCore
    {

    }

    [System.Serializable]
    public class KCCore2D : IKCCore
    {
        [Header("Rigibody")]
        [SerializeField]
        private LayerMask targetLayer;

        [Header("Gravity")]
        [SerializeField]
        private bool useGravity;
        [SerializeField]
        private float gravityPower;
        [SerializeField]
        private Vector2 gravityDirection = Vector2.down;

        private Vector2 movementInput = Vector2.zero;
        private Vector2 movementVector = Vector2.zero;

        public void KCAwake()
        {
            
        }

        public void KCStart()
        {

        }

        public void KCUpdate()
        {
            ProbeGround();
        }

        public void KCFixedUpdate()
        {

        }

        public bool ProbeGround()
        {

            return false;
        }
    }
}