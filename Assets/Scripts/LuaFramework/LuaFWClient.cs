/***
*
*	Title:"Lua���"��Ŀ
*			����Lua����
*
*	Description:
*		���ܣ�
*			����Lua
*
*	Author: Zhaiyurong
*
*	Date: 2022.2
*
*	Modify:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;
using System.Text;
using System;
using System.IO;

namespace LuaFramework
{
    public class LuaFWClient : MonoBehaviour
    {
        //Lua����
        private LuaEnv luaenv;

        //Update�¼�
        private Action LuaUpdate;
        private Action LuaFixedUpdate;
        private Action LuaLateUpdate;


        void Start()
        {
            //����Lua����
            luaenv = new LuaEnv();

            //���loader
            luaenv.AddLoader(LuaLoader);

            //��ʼִ��Lua����
            luaenv.DoString(LuaFWDefine.LUA_START);

            
            //����Update
            LuaUpdate = luaenv.Global.Get<Action>("Update");
            LuaFixedUpdate = luaenv.Global.Get<Action>("FixedUpdate");
            LuaLateUpdate = luaenv.Global.Get<Action>("LateUpdate");
        }

        /// <summary>
        /// �Զ���Loader
        /// </summary>
        /// <param name="filepath">require��������lua�ļ�·��</param>
        /// <returns></returns>
        public byte[] LuaLoader(ref string filepath)
        {
            //ƴ��Lua�ļ�����·��
            string loadPath = LuaFWPathTool.GetLuaScriptPath() + "/" + filepath + ".lua";
            //Debug.Log("lua filepath = " + loadPath);

            //��ȡLua�ļ�����
            string content = File.ReadAllText(loadPath);
            //Debug.Log("lua text = "+ content);

            return System.Text.Encoding.UTF8.GetBytes(content);
        }

        
        // Update is called once per frame
        void Update()
        {
            LuaUpdate();
        }

        void LateUpdate()
        {
            LuaLateUpdate();
        }

        void FixedUpdate()
        {
            LuaFixedUpdate();
        }
        

        void OnDestroy()
        {
            Debug.Log("GameManager Destroyed");

            //������������
            LuaUpdate = null;
            LuaLateUpdate = null;
            LuaFixedUpdate = null;

            //�ͷ���Դ
            luaenv.Dispose();
        }
    }

}
