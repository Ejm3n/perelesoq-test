using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Serialization
{
    [CreateAssetMenu(fileName = "ElectricGraph", menuName = "SmartHome/Electric Graph Asset")]
    public class ElectricGraphAsset : ScriptableObject
    {
        [Serializable]
        public class NodeConnection
        {
            public string fromNodeId;
            public string toNodeId;
        }

        [Serializable]
        public class DeviceNode
        {
            public string id;
            public GameObject targetObject;
        }

        public List<NodeConnection> connections = new();
        public List<DeviceNode> nodes = new();

        public GameObject GetObjectById(string id)
        {
            return nodes.Find(n => n.id == id)?.targetObject;
        }

        public List<string> GetChildren(string fromId)
        {
            List<string> result = new();
            foreach (var conn in connections)
            {
                if (conn.fromNodeId == fromId)
                    result.Add(conn.toNodeId);
            }
            return result;
        }
    }
}
