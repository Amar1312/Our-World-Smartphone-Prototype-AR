using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Location;
using UnityEngine.UI;
using System;

public class ViewController : MonoBehaviour
{
	public static ViewController Instance = null;

	public List<GameObject> _MapViews;
	public PinchRotationSample _PinchObj;
	[SerializeField]
	List<AbstractMap> _maps;
	public Vector2d _currentLatlong;
	public float _zoomvalue;
	public string _currentSearch;
	public LocationProviderFactory _locationfactory;
	public UpdateMapWithLocationProvider _updateMap;

	[Header("GameObject for pinch")]
	public List<GameObject> _PinchGameObject;

	[Header("Control UpdateMap")]
	public Toggle _ToggleUpdateMap;

	[Header("Player")]
	public List<MeshRenderer> _Players;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}
	// Start is called before the first frame update
	void Start()
	{
		_ToggleUpdateMap.onValueChanged.AddListener(UpdateMapToggle);
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnviewClick(int index)
	{
		foreach (GameObject obj in _MapViews)
			obj.SetActive(false);
		foreach (MeshRenderer obj in _Players)
			obj.enabled = false;

		_MapViews[index].SetActive(true);

		_maps[index].SetCenterLatitudeLongitude(_currentLatlong);
		_maps[index].SetZoom(_zoomvalue);

		if (_maps[index].isActiveAndEnabled)
			StartCoroutine(IemnumWaitFormap(index));

		_locationfactory.mapManager = _maps[index];
		_updateMap._map = _maps[index];

		_PinchObj.target = _PinchGameObject[index].transform;

		if (index == 0)
			_PinchObj.AR = true;
		else
			_PinchObj.AR = false;
	}

	IEnumerator IemnumWaitFormap(int index)
	{
		yield return new WaitForSeconds(2.0f);
		_maps[index].UpdateMap();
		yield return new WaitForSeconds(0.5f);
		CheckMapForPlayer();
	}

	//check map for player
	public void CheckMapForPlayer()
	{
		if (_updateMap._map.gameObject.activeSelf)
		{
			foreach (MeshRenderer obj in _Players)
				obj.enabled = true;
		}
		else
		{
			foreach (MeshRenderer obj in _Players)
				obj.enabled = false;
		}


	}

	public void UpdateMapToggle(bool value)
	{
		_updateMap.enabled = value;
	}




}
