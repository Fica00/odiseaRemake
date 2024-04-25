using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class Database : MonoBehaviour
{
	private const string DATABASE = "https://vr.odisea.es/json/latest.json";
	
	public static Database Instance;

	public DatabasePanel[] Data;

	private void Awake()
	{
		if (Instance==null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Init(Action _callBack)
	{
		StartCoroutine(WebRequestHandler.Instance.GetRequest(DATABASE, _result =>
		{
			DatabaseData _data = JsonConvert.DeserializeObject<DatabaseData>(_result);
			Load(_data);
			_callBack?.Invoke();
		}, _error => { Debug.LogErrorFormat("Database2 load failed: {0}", _error); }));
	}

	private void Load(DatabaseData _db2)
	{
		Data = _db2.Programs.Where(_o => _o.Active.HasValue && _o.Active != 0).Select(_p => new DatabasePanel
		{
			ID = _p.Id,
			Index = _p.ItemOrder ?? 0,
			Theme = _p.Category ?? (int)Theme.Ciudad,
			InfoES = _p.TitleEs,
			InfoPT = _p.TitlePt,
			SynopsisES = _p.DescriptionEs,
			SynopsisPT = _p.DescriptionPt,
			ThumbnailES = _p.PosterEs,
			ThumbnailPT = _p.PosterPt,
			Chapters = _db2.Episodes
				.Where(_c => _c.ProgramId == _p.Id && _c.Active.HasValue && _c.Active.Value != 0 && _c.DateStart.HasValue &&
				            DateTime.Now >= _c.DateStart && _c.DateEnd.HasValue && DateTime.Now <= _c.DateEnd).Select(_c => new DatabaseChapter
				{
					ID = _c.Id,
					Index = _c.ItemOrder ?? 0,
					TitleES = _c.TitleEs,
					TitlePT = _c.TitleEs,
					VideoES264 = _c.UrlH264ES,
					VideoPT264 = _c.UrlH264Pt,
					VideoES265 = _c.UrlH265ES,
					VideoPT265 = _c.UrlH265Pt,
					SynopsisES = _c.DescriptionEs,
					SynopsisPT = _c.DescriptionEs,
					ThumbnailES = _c.PosterES,
					ThumbnailPT = _c.PosterPt,
					Stereoscopic = _c.Stereoscopic.HasValue && _c.Stereoscopic != 0,
				}).ToArray(),
		}).ToArray();
	}
}