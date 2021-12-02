using System;
using UnityEngine;
using UnityEngine.Networking;

namespace SolarSystemKritskiy_Network
{
#pragma warning disable 618
    public abstract class NetworkMovableObject : NetworkBehaviour
#pragma warning restore 618
    {
        protected abstract float _speed { get; }
        protected Action _onUpdateAction { get; set; }
        protected Action _onFixedUpdateAction { get; set; }
        protected Action _onLateUpdateAction { get; set; }
        protected Action _onPreRenderAction{ get; set; }
        protected Action _onPostRenderAction { get; set; }
        
#pragma warning disabe 618
        [SyncVar] protected Vector3 _serverPosition;
        [SyncVar] protected Vector3 _serverEuler;
#pragma warning restore 618

        protected virtual void Movement()
        {
            if (hasAuthority)
                HasAuthorityMovement();
            else
                FromServerUpdate();
        }

        protected abstract void HasAuthorityMovement();
        protected abstract void FromServerUpdate();
        protected abstract void SendToServer();
    }
}
