using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Serialization
{
    public class GraphEditorWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        private List<GraphNode> _nodes = new();
        private List<GraphEdge> _edges = new();
        private GraphNode _selectedOutput;
        private ElectricNetworkAsset _currentAsset;
        private bool _dirty;

        [MenuItem("SmartHome/Graph Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<GraphEditorWindow>();
            window.titleContent = new GUIContent("Electric Graph");
            window.minSize = new Vector2(900, 600);
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
            EditorGUILayout.BeginVertical(GUILayout.Width(250));
            EditorGUILayout.LabelField("Devices", EditorStyles.boldLabel);

            foreach (ElectricDeviceType type in Enum.GetValues(typeof(ElectricDeviceType)))
            {
                if (GUILayout.Button($"Add {type}"))
                {
                    _nodes.Add(new GraphNode(type, new Rect(300, 200, 200, 120)));
                    _dirty = true;
                }
            }

            GUILayout.Space(10);
            GUI.enabled = _dirty;
            if (GUILayout.Button("\ud83d\udcc2 Save Asset"))
            {
                SaveAsset();
            }
            GUI.enabled = true;

            if (_currentAsset != null)
            {
                if (GUILayout.Button("\ud83d\udcc2 Overwrite Loaded"))
                {
                    OverwriteLoadedAsset();
                }
                if (GUILayout.Button("\ud83d\udcc2 Open in Project"))
                {
                    Selection.activeObject = _currentAsset;
                    EditorGUIUtility.PingObject(_currentAsset);
                }
            }

            if (GUILayout.Button("\ud83d\udcc1 Load Asset"))
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
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, true, true, GUILayout.Width(position.width - 250), GUILayout.Height(position.height));

            BeginWindows();
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].rect = GUI.Window(i, _nodes[i].rect, DrawNodeWindow, _nodes[i].id);
            }
            EndWindows();
            DrawEdges();
            HandleEdgeClick();
            EditorGUILayout.EndScrollView();
        }

        private void DrawNodeWindow(int id)
        {
            var node = _nodes[id];
            node.id = EditorGUILayout.TextField("ID", node.id);
            GUILayout.Label(node.type.ToString());

            if (GUILayout.Button("Connect"))
            {
                if (_selectedOutput == null)
                {
                    _selectedOutput = node;
                }
                else
                {
                    if (_selectedOutput == node)
                    {
                        // Сброс, если кликнули дважды по одному
                        _selectedOutput = null;
                    }
                    else if (!_edges.Any(e => e.output == _selectedOutput && e.input == node))
                    {
                        _edges.Add(new GraphEdge(_selectedOutput, node));
                        _dirty = true;
                        _selectedOutput = null;
                    }
                }
            }

            if (GUILayout.Button("Delete Node"))
            {
                _edges.RemoveAll(e => e.output == node || e.input == node);
                _nodes.RemoveAt(id);
                GUIUtility.ExitGUI();
            }

            GUI.DragWindow();
        }

        private void DrawEdges()
        {
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
        }

        private void HandleEdgeClick()
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                Vector2 mousePos = e.mousePosition;
                var toRemove = _edges.FirstOrDefault(edge =>
                {
                    float dist = HandleUtility.DistancePointBezier(mousePos, edge.output.rect.center, edge.input.rect.center,
                        edge.output.rect.center + Vector2.right * 50f,
                        edge.input.rect.center + Vector2.left * 50f);
                    return dist < 10f;
                });

                if (toRemove != null)
                {
                    _edges.Remove(toRemove);
                    _dirty = true;
                    e.Use();
                }
            }
        }

        private void SaveAsset()
        {
            if (_nodes == null || _edges == null)
            {
                Debug.LogError("Cannot save: nodes or edges are null.");
                return;
            }

            var asset = ScriptableObject.CreateInstance<ElectricNetworkAsset>();

            // 1. Переносим все ноды
            foreach (var node in _nodes)
            {
                if (string.IsNullOrEmpty(node.id))
                {
                    Debug.LogError("Node has empty ID. Skipping.");
                    continue;
                }

                asset.devices.Add(new ElectricDeviceDefinition
                {
                    id = node.id,
                    type = node.type,
                    inputs = new List<string>()
                });
            }

            // 2. Переносим связи
            foreach (var edge in _edges)
            {
                if (edge.output == null || edge.input == null)
                {
                    Debug.LogWarning("Edge with null endpoint found. Skipping.");
                    continue;
                }

                var from = asset.devices.Find(d => d.id == edge.output.id);
                if (from == null)
                {
                    Debug.LogWarning($"Could not find output node with ID: {edge.output.id}");
                    continue;
                }

                if (!from.inputs.Contains(edge.input.id))
                {
                    from.inputs.Add(edge.input.id);
                }
            }

            // 3. Показываем диалог
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Electric Network",
                "NewElectricGraph",
                "asset",
                "Choose location to save the electric graph asset"
            );

            if (string.IsNullOrEmpty(path))
                return;

            // 4. Проверяем и удаляем, если есть
            var existing = AssetDatabase.LoadAssetAtPath<ElectricNetworkAsset>(path);
            if (existing != null)
                AssetDatabase.DeleteAsset(path);

            // 5. Сохраняем
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _currentAsset = asset;
            _dirty = false;
        }


        private void OverwriteLoadedAsset()
        {
            if (_currentAsset == null) return;
            _currentAsset.devices.Clear();

            foreach (var node in _nodes)
            {
                _currentAsset.devices.Add(new ElectricDeviceDefinition
                {
                    id = node.id,
                    type = node.type
                });
            }

            foreach (var edge in _edges)
            {
                var from = _currentAsset.devices.Find(d => d.id == edge.output.id);
                if (!from.inputs.Contains(edge.input.id))
                    from.inputs.Add(edge.input.id);
            }

            EditorUtility.SetDirty(_currentAsset);
            AssetDatabase.SaveAssets();
            _dirty = false;
        }

        private void LoadAsset()
        {
            if (_currentAsset == null)
            {
                Debug.LogError("LoadAsset called but _currentAsset is null.");
                return;
            }

            if (_currentAsset.devices == null)
            {
                Debug.LogError("Current asset has null devices list.");
                return;
            }

            _nodes.Clear();
            _edges.Clear();

            var map = new Dictionary<string, GraphNode>();

            foreach (var def in _currentAsset.devices)
            {
                if (string.IsNullOrEmpty(def.id))
                {
                    Debug.LogWarning("Skipping node with empty ID.");
                    continue;
                }

                var node = new GraphNode(def.type, new Rect(UnityEngine.Random.Range(100, 600), UnityEngine.Random.Range(100, 500), 200, 120), def.id);
                _nodes.Add(node);
                map[def.id] = node;
            }

            foreach (var def in _currentAsset.devices)
            {
                if (def.inputs == null) continue;

                foreach (var inputId in def.inputs)
                {
                    if (map.TryGetValue(def.id, out var outputNode) && map.TryGetValue(inputId, out var inputNode))
                    {
                        _edges.Add(new GraphEdge(outputNode, inputNode));
                    }
                    else
                    {
                        Debug.LogWarning($"Could not resolve edge from {inputId} -> {def.id}");
                    }
                }
            }

            _dirty = false;
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
