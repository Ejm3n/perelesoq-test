using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEditor;
using UnityEngine;

namespace SmartHome.Serialization
{
    public class GraphEditorWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        private List<GraphNode> _nodes = new();
        private List<GraphEdge> _edges = new();
        private GraphNode _selectedOutput;
        private ElectricNetworkAsset _currentAsset;

        [MenuItem("SmartHome/Graph Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<GraphEditorWindow>();
            window.titleContent = new GUIContent("Electric Graph");
            window.minSize = new Vector2(800, 600);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            DrawSidebar();
            DrawCanvas();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSidebar()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            EditorGUILayout.LabelField("Devices", EditorStyles.boldLabel);

            foreach (ElectricDeviceType type in Enum.GetValues(typeof(ElectricDeviceType)))
            {
                if (GUILayout.Button($"Add {type}"))
                {
                    _nodes.Add(new GraphNode(type, new Rect(300, 200, 150, 60)));
                }
            }

            if (GUILayout.Button("Save Asset"))
            {
                SaveAsset();
            }

            if (GUILayout.Button("Load Asset"))
            {
                string path = EditorUtility.OpenFilePanel("Select Asset", "Assets", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Substring(path.IndexOf("Assets"));
                    _currentAsset = AssetDatabase.LoadAssetAtPath<ElectricNetworkAsset>(path);
                    LoadAsset();
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawCanvas()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            BeginWindows();

            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].rect = GUI.Window(i, _nodes[i].rect, DrawNodeWindow, _nodes[i].id);
            }

            foreach (var edge in _edges)
            {
                Handles.DrawBezier(
                    edge.output.rect.center,
                    edge.input.rect.center,
                    edge.output.rect.center + Vector2.right * 50f,
                    edge.input.rect.center + Vector2.left * 50f,
                    Color.yellow,
                    null,
                    3f
                );
            }

            EndWindows();
            EditorGUILayout.EndScrollView();
        }

        private void DrawNodeWindow(int id)
        {
            var node = _nodes[id];

            GUILayout.Label(node.type.ToString());
            if (GUILayout.Button("Connect"))
            {
                if (_selectedOutput == null)
                {
                    _selectedOutput = node;
                }
                else if (_selectedOutput != node)
                {
                    _edges.Add(new GraphEdge(_selectedOutput, node));
                    _selectedOutput = null;
                }
            }

            GUI.DragWindow();
        }

        private void SaveAsset()
        {
            var asset = ScriptableObject.CreateInstance<ElectricNetworkAsset>();

            foreach (var node in _nodes)
            {
                var def = new ElectricDeviceDefinition
                {
                    id = node.id,
                    type = node.type
                };
                asset.devices.Add(def);
            }

            foreach (var edge in _edges)
            {
                var from = asset.devices.Find(d => d.id == edge.output.id);
                from.inputs.Add(edge.input.id);
            }

            string path = EditorUtility.SaveFilePanelInProject("Save Electric Network", "NewElectricGraph", "asset", "");
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                _currentAsset = asset;
            }
        }

        private void LoadAsset()
        {
            _nodes.Clear();
            _edges.Clear();

            var map = new Dictionary<string, GraphNode>();
            foreach (var def in _currentAsset.devices)
            {
                var node = new GraphNode(def.type, new Rect(UnityEngine.Random.Range(100, 500), UnityEngine.Random.Range(100, 500), 150, 60), def.id);
                _nodes.Add(node);
                map[def.id] = node;
            }

            foreach (var def in _currentAsset.devices)
            {
                foreach (var inputId in def.inputs)
                {
                    if (map.TryGetValue(def.id, out var outputNode) && map.TryGetValue(inputId, out var inputNode))
                    {
                        _edges.Add(new GraphEdge(outputNode, inputNode));
                    }
                }
            }
        }

        private class GraphNode
        {
            public string id;
            public ElectricDeviceType type;
            public Rect rect;

            public GraphNode(ElectricDeviceType type, Rect rect, string id = null)
            {
                this.type = type;
                this.rect = rect;
                this.id = id ?? Guid.NewGuid().ToString();
            }
        }

        private class GraphEdge
        {
            public GraphNode output;
            public GraphNode input;
            public GraphEdge(GraphNode output, GraphNode input)
            {
                this.output = output;
                this.input = input;
            }
        }
    }

}
