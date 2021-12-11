#if FIRESTORE
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

class TableSheet
{
    public string id;
    public string className;
    public Dictionary<string, object> schema;
    public bool isKeyValueOnly;
    public Dictionary<string, object> rows;
}

public class SyncDataConfig : EditorWindow
{
    string path = "Assets/_Games/_Common/Scripts/AutoGenerate/Models";

    [MenuItem("Custom/Firestore Sync Data Config")]
    static void Init()
    {
        SyncDataConfig window = (SyncDataConfig)EditorWindow.GetWindow(typeof(SyncDataConfig));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Sync"))
        {
            DoSync();
        }
    }

    private async void DoSync()
    {
        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            string dbName = "data_config_dev";

#if PRODUCTION
            dbName = "data_config_pro";
#elif STAGING
            dbName = "data_config_sta";
#endif

            // Set a flag here to indicate whether Firebase is ready to use by your app.
            var db = FirebaseFirestore.DefaultInstance;
            var dataConfigRef = db.Collection(dbName);
            var snapshot = await dataConfigRef.GetSnapshotAsync();
            var sheets = new List<TableSheet>();

            foreach (var doc in snapshot.Documents)
            {
                var sh = new TableSheet();
                sh.id = doc.Id;
                sh.className = doc.Id.First().ToString().ToUpper() + doc.Id.Substring(1) + "Type";

                var dict = doc.ToDictionary();
                if (dict.ContainsKey("schema"))
                {
                    sh.schema = (Dictionary<string, object>)dict["schema"];
                }

                sh.isKeyValueOnly = sh.schema == null;
                sh.rows = doc.ToDictionary();

                sheets.Add(sh);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (StreamWriter writer = File.CreateText(path + "/" + "DataConfigModel.cs"))
            {
                writer.WriteLine("/// <summary>");
                writer.WriteLine("/// AutoGenerate Class, do not modify");
                writer.WriteLine("/// </summary>");

                writer.WriteLine("#if FIRESTORE");
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using Firebase.Firestore;");
                writer.WriteLine();

                writer.WriteLine("namespace DataConfig");
                writer.WriteLine("{");
                writer.WriteLine("  [Serializable]");
                writer.WriteLine("  public class DataConfigModel");
                writer.WriteLine("  {");

                foreach (TableSheet sh in sheets)
                {
                    if (sh.isKeyValueOnly)
                    {
                        writer.WriteLine(string.Format("    public {0} {1} = new {0}();", sh.className, sh.id));
                    }
                    else
                    {
                        writer.WriteLine(string.Format("    public Dictionary<string, {0}> {1} = new Dictionary<string, {0}>();", sh.className, sh.id));
                    }
                }
                writer.WriteLine();

                writer.WriteLine("    public void AddDocument(DocumentSnapshot doc)");
                writer.WriteLine("    {");

                foreach (TableSheet sh in sheets)
                {
                    writer.WriteLine(string.Format("      if (doc.Id.CompareTo(\"{0}\") == 0)", sh.id));
                    writer.WriteLine("      {");

                    if (sh.isKeyValueOnly)
                    {
                        writer.WriteLine(string.Format("        this.{0} = doc.ConvertTo<{1}>();", sh.id, sh.className));
                    }
                    else
                    {
                        writer.WriteLine(string.Format("        this.{0} = doc.ConvertTo<Dictionary<string, DataConfig.{1}>>();", sh.id, sh.className));
                    }

                    writer.WriteLine("      }");
                }

                writer.WriteLine("    }");
                writer.WriteLine("  }");

                foreach (TableSheet sh in sheets)
                {
                    writer.WriteLine();
                    writer.WriteLine("  [FirestoreData]");
                    writer.WriteLine(string.Format("  public class {0}", sh.className));
                    writer.WriteLine("  {");

                    if (sh.isKeyValueOnly)
                    {
                        foreach (KeyValuePair<string, object> row in sh.rows)
                        {
                            var memberName = row.Key;
                            if (memberName == "schema") continue;

                            var memberValue = row.Value;
                            var memberType = "string";

                            if (memberValue is int) memberType = "int";
                            else if (memberValue is long) memberType = "long";
                            else if (memberValue is double) memberType = "double";
                            else if (memberValue is float) memberType = "float";
                            else if (memberValue is bool) memberType = "bool";


                            if (memberType.CompareTo("int") == 0)
                            {
                                writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]");
                            }
                            else if (memberType.CompareTo("long") == 0)
                            {
                                writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]");
                            }
                            else if (memberType.CompareTo("double") == 0)
                            {
                                writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]");
                            }
                            else if (memberType.CompareTo("float") == 0)
                            {
                                writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]");
                            }
                            else if (memberType.CompareTo("bool") == 0)
                            {
                                writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreBoolConverter))]");
                            }
                            else if (memberType.CompareTo("string") == 0)
                            {
                                writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]");
                            }
                            else
                            {
                                writer.WriteLine("    [FirestoreProperty]");
                            }
                            writer.WriteLine(string.Format("    public {0} {1} {{ get; set; }}", memberType, memberName));

                            writer.WriteLine();
                        }
                    }
                    else
                    {
                        if (sh.schema != null)
                        {
                            foreach (KeyValuePair<string, object> kvp in sh.schema)
                            {
                                var memberName = kvp.Key;
                                var memberType = (string)kvp.Value;

                                if (memberType.CompareTo("ignore") == 0) memberType = "string";

                                if (memberType.CompareTo("schema") == 0)
                                {
                                    var firstRow = sh.rows[sh.rows.Keys.First()] as Dictionary<string, object>;
                                    object firstValue = firstRow[memberName];

                                    if (firstValue is long) memberType = "int";
                                    else memberType = "string";
                                }

                                if (memberType.CompareTo("int") == 0)
                                {
                                    writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]");
                                }
                                else if (memberType.CompareTo("long") == 0)
                                {
                                    writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]");
                                }
                                else if (memberType.CompareTo("double") == 0)
                                {
                                    writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]");
                                }
                                else if (memberType.CompareTo("float") == 0)
                                {
                                    writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]");
                                }
                                else if (memberType.CompareTo("bool") == 0)
                                {
                                    writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreBoolConverter))]");
                                }
                                else if (memberType.CompareTo("string") == 0)
                                {
                                    writer.WriteLine("    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]");
                                }
                                else
                                {
                                    writer.WriteLine("    [FirestoreProperty]");
                                }

                                writer.WriteLine(string.Format("    public {0} {1} {{ get; set; }}", memberType, memberName));
                                writer.WriteLine();
                            }
                        }
                    }

                    writer.WriteLine("  }");
                }

                writer.WriteLine("}");
                writer.WriteLine("#endif");
            }

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            Debug.Log("Finished sync data config!");
        }
        else
        {
            Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        }
    }
}
#endif