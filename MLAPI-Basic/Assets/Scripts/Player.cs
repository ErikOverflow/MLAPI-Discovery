using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        [SerializeField]
        SpriteRenderer spriteRenderer;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            { TargetClientIds = new ulong[1] {0} }
        };
        public void Move()
        {
            SubmitPositionRequestServerRpc();
        }
        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
            clientRpcParams.Send.TargetClientIds[0] = rpcParams.Receive.SenderClientId;
            SubmitColorChangeClientRpc(Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f)),clientRpcParams);
        }

        [ClientRpc]
        void SubmitColorChangeClientRpc(Color color, ClientRpcParams rpcParams = default)
        {
            spriteRenderer.color = color;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            //if (NetworkManager.Singleton.IsServer)
            //{
            //    Position.Value += Vector3.one * 0.01f;
            //}
            transform.position = Position.Value;
        }
    }
}