#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public static class PathDirectory
{
    public const string CONSTANTS = "Assets/_Games/_Common/Scripts/AutoGenerate/";
    public const string AUTO_CREATING_CONSTANTS = CONSTANTS + "Constants/";
}

public static class NameExtension
{
    public const string META = ".meta";
    public const string ARCHIVE = ".a";
    public const string FRAMEWORK = ".framework";
    public const string BUNDLE = ".bundle";
    public const string ASSET = ".asset";
    public const string TEMPLATE_SCRIPT = ".txt";
    public const string SCRIPT = ".cs";
    public const string EXCEL = ".xls";
    public const string PREFAB = ".prefab";
    public const string SCENE = ".unity";
}

public static class GenerateConstantsClass
{
    private static readonly string[] INVALID_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

    private const char DELIMITER = '_';
    private const string STRING_NAME = "string";
    private const string INT_NAME = "int";
    private const string FLOAT_NAME = "float";

    public static void Create<T>(string className, Dictionary<string, T> valueDict)
    {
        string typeName = null;

        if (typeof(T) == typeof(string))
        {
            typeName = STRING_NAME;
        }
        else if (typeof(T) == typeof(int))
        {
            typeName = INT_NAME;
        }
        else if (typeof(T) == typeof(float))
        {
            typeName = FLOAT_NAME;
        }
        else
        {
            Debug.Log(className + NameExtension.SCRIPT + " .Failed to create the unexpected type " + typeof(T).Name);
            return;
        }

        SortedDictionary<string, T> sortDict = new SortedDictionary<string, T>(valueDict);
        Dictionary<string, T> newValueDict = new Dictionary<string, T>();

        foreach (KeyValuePair<string, T> valuePair in sortDict)
        {
            string newKey = RemoveInvalidChars(valuePair.Key);
            newKey = SetDelimiterBeforeUppercase(newKey);
            newValueDict[newKey] = valuePair.Value;
        }

        int keyLengthMax = 0;
        if (newValueDict.Count > 0)
        {
            keyLengthMax = 1 + newValueDict.Keys.Select(key => key.Length).Max();
        }

        StringBuilder builder = new StringBuilder();

        builder.AppendLine("/// <summary>");
        builder.AppendFormat("/// AutoGenerate Class, do not modify").AppendLine();
        builder.AppendLine("/// </summary>");
        builder.AppendLine("\n");
        builder.AppendFormat("public static class {0}", className).AppendLine("\n{");

        string[] keyArray = newValueDict.Keys.ToArray();
        foreach (string key in keyArray)
        {

            if (string.IsNullOrEmpty(key))
            {
                continue;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(key, @"^[0-9]+$"))
            {
                continue;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(key, @"^[_a-zA-Z0-9]+$"))
            {
                continue;
            }

            string EqualStr = String.Format("{0, " + (keyLengthMax - key.Length).ToString() + "}", "=");

            builder.Append("\t").AppendFormat(@" public const {0} {1} {2} ", typeName, key, EqualStr);

            if (typeName == STRING_NAME)
            {
                builder.AppendFormat(@"""{0}"";", newValueDict[key]).AppendLine();
            }

            else if (typeName == FLOAT_NAME)
            {
                builder.AppendFormat(@"{0}f;", newValueDict[key]).AppendLine();
            }

            else
            {
                builder.AppendFormat(@"{0};", newValueDict[key]).AppendLine();
            }

        }

        builder.AppendLine().AppendLine("}");

        string exportPath = PathDirectory.AUTO_CREATING_CONSTANTS + className + NameExtension.SCRIPT;

        string directoryName = Path.GetDirectoryName(exportPath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(exportPath, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

        Debug.Log(string.Format("{0} SUCCESSFUL created", exportPath));
    }

    private static string RemoveInvalidChars(string str)
    {
        Array.ForEach(INVALID_CHARS, c => str = str.Replace(c, string.Empty));
        return str;
    }

    private static string SetDelimiterBeforeUppercase(string str)
    {
        string conversionStr = "";

        for (int strNo = 0; strNo < str.Length; strNo++)
        {
            bool isSetDelimiter = true;

            if (strNo == 0)
            {
                isSetDelimiter = false;
            }
            else if (char.IsLower(str[strNo]) || char.IsNumber(str[strNo]))
            {
                isSetDelimiter = false;
            }
            else if (char.IsUpper(str[strNo - 1]) && !char.IsNumber(str[strNo]))
            {
                isSetDelimiter = false;
            }
            else if (str[strNo] == DELIMITER || str[strNo - 1] == DELIMITER)
            {
                isSetDelimiter = false;
            }

            if (isSetDelimiter)
            {
                conversionStr += DELIMITER.ToString();
            }
            conversionStr += str.ToUpper()[strNo];
        }

        return conversionStr;
    }
}
#endif //UNITY_EDITOR
