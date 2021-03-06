//  Project  : ACTORS
//  Contacts : Pixeye - ask@pixeye.games

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixeye
{
	public class ProcessingScene : IDisposable, IKernel
	{
		public static ProcessingScene Default = new ProcessingScene();

		protected readonly Dictionary<string, Transform> sceneObjs = new Dictionary<string, Transform>();

		internal static Transform Dynamic = GameObject.Find("Dynamic").transform;
 

		public void Dispose()
		{
			sceneObjs.Clear();
		}

		public Transform Get(string id)
		{
			Transform obj;
			var       haveFound = sceneObjs.TryGetValue(id, out obj);
			if (!haveFound)
			{
				var o = GameObject.Find(id);
				if (o)
					obj = o.transform;
				if (obj) sceneObjs.Add(id, obj);
			}

			return obj;
		}

		public Transform Get(WorldParenters parent)
		{
			switch (parent)
			{
				case WorldParenters.Level:
					return Get("Dynamic");
				case WorldParenters.UI:
					return Get("[UI]");
				case WorldParenters.None:
					return Get("[SCENE]");
			}

			return null;
		}
	}

 
}