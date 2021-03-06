//  Project  : ACTORS
//  Contacts : Pixeye - ask@pixeye.games


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixeye
{
	public class ComponentObject : IComponent, IDisposable
	{
		public Transform transform;
		public int poolType = -1;

		public void Dispose() { transform = null; }
	}

	public static class ExtensionComponentObject
	{
		public static ComponentObject ComponentObject(this int entity) { return Storage<ComponentObject>.Instance.components[entity]; }

		public static bool HasComponentObject(this int entity) { return Storage<ComponentObject>.Instance.HasComponent(entity); }
	}
}