using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.IO;
using XLua;

public class LuaFileUtil
{
    private static LuaFileUtil instance = null;
    public static LuaFileUtil Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LuaFileUtil();
            }
            return instance;
        }
        protected set
        {
            instance = value;
        }
    }

    public LuaFileUtil()
    {
        instance = this;
    }

    private List<string> luapathList = new List<string>();
    private StringBuilder strLuaPath = new StringBuilder(256);

    //lua package path ת��
    private string ToPackagePath(string path)
    {
        StringBuilder sb = new StringBuilder(256);
        sb.Append(path);
        sb.Replace("\\", "/");
        if (sb.Length > 0 && sb[sb.Length - 1] != '/')
        {
            sb.Append("/");
        }
        sb.Append("?.lua;");
        return sb.ToString();
    }

    //�������·��
    public void AddSearchPath(string path)
    {
        if(!Path.IsPathRooted(path))
        {
            throw new LuaException(path + "is not a full path");
        }
        string ppath = ToPackagePath(path);
        luapathList.Add(ppath);
    }

    //��ȡ���µ�package.path·��
    public string GetPackagePath()
    {
        strLuaPath.Clear();
        foreach(string path in luapathList)
        {
            strLuaPath.Append(path);
        }
        return strLuaPath.ToString();
    }
}
