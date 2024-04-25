using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Locale : MonoBehaviour
{	
	static public Locale Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new Locale();
					if(string.IsNullOrEmpty(instance.locale)) instance.locale = Application.systemLanguage == SystemLanguage.Spanish ? "ES" : Application.systemLanguage == SystemLanguage.Portuguese ? "PT" : "";
				}
				return instance;
			}
		}

		static private Locale instance = null;

		public string locale; // = "PT";
		private Row[] data;

		public string this[string path]
		{
			get
			{
				var scene = SceneManager.GetActiveScene().name;
				var row = data.SingleOrDefault(o => o.Scene == scene && o.Path == path);
				return row == null ? null : locale == "ES" ? row.ES : row.PT;
			}
		}
		public string this[Text component]
		{
			get
			{
				var path = component.transform.Path();
				var scene = SceneManager.GetActiveScene().name;
				var row = data.SingleOrDefault(o => o.Scene == scene && o.Path == path);
				return row == null ? component.text : locale == "ES" ? row.ES : row.PT;
			}
		}
		public Sprite this[Image component]
		{
			get
			{
				var path = component.transform.Path();
				var scene = SceneManager.GetActiveScene().name;
				var row = data.SingleOrDefault(o => o.Scene == scene && o.Path == path);
				if(row != null)
				{
					var texture = Resources.Load<Texture2D>(locale == "ES" ? row.ES : row.PT);
					if(texture != null) return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(.5f, .5f), 100f);
				}
				return component.sprite;
			}
		}
		public Material this[Renderer component]
		{
			get
			{
				var path = component.transform.Path();
				var scene = SceneManager.GetActiveScene().name;
				var row = data.SingleOrDefault(o => o.Scene == scene && o.Path == path);
				return row != null ? Resources.Load<Material>(locale == "ES" ? row.ES : row.PT) : null;
			}
		}

		private Locale()
		{
			var resource = Resources.Load<TextAsset>("locale");
			var lines = resource.text.Replace("\r", "").Split('\n').Skip(1);
			data = lines.Select(o => o.Replace("|", "\r\n").Split(';')).Where(o => o.Length == 4).Select(o => new Row { Scene = o[0], Path = o[1], ES = o[2], PT = o[3] }).ToArray();
		}

		private class Row
		{
			public string Scene { get; set; }
			public string Path  { get; set; }
			public string ES    { get; set; }
			public string PT    { get; set; }
		}
}
	static public partial class Extensions
	{
		static public string Path(this Transform transform)
		{
			var path = ""; for(; transform != null; transform = transform.parent) path = "/"+transform.name+path;
			return path;
		}
	}

