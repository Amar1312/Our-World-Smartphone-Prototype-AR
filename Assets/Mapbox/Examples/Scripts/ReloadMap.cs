﻿namespace Mapbox.Examples
{
	using Mapbox.Geocoding;
	using UnityEngine.UI;
	using Mapbox.Unity.Map;
	using UnityEngine;
	using System;
	using System.Collections;

	public class ReloadMap : MonoBehaviour
	{
		public Camera _camera;
		Vector3 _cameraStartPos;
		AbstractMap _map;

		[SerializeField]
		ForwardGeocodeUserInput _forwardGeocoder;

		[SerializeField]
		Slider _zoomSlider;

		private HeroBuildingSelectionUserInput[] _heroBuildingSelectionUserInput;

		Coroutine _reloadRoutine;

		WaitForSeconds _wait;

		void Awake()
		{
			_cameraStartPos = _camera.transform.position;
			_map = FindObjectOfType<AbstractMap>();
			if (_map == null)
			{
				Debug.LogError("Error: No Abstract Map component found in scene.");
				return;
			}
			if (_zoomSlider != null)
			{
				_map.OnUpdated += () => { _zoomSlider.value = _map.Zoom; };
				_zoomSlider.onValueChanged.AddListener(Reload);
			}
			if (_forwardGeocoder != null)
			{
				_forwardGeocoder.OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
			}
			_heroBuildingSelectionUserInput = GetComponentsInChildren<HeroBuildingSelectionUserInput>();
			if (_heroBuildingSelectionUserInput != null)
			{
				for (int i = 0; i < _heroBuildingSelectionUserInput.Length; i++)
				{
					_heroBuildingSelectionUserInput[i].OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
				}
			}
			_wait = new WaitForSeconds(.3f);
		}

		void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response)
		{
			Debug.Log("ForwardGeocoder response ::: 1");
			if (null != response.Features && response.Features.Count > 0)
			{
				int zoom = _map.AbsoluteZoom;
				_map.UpdateMap(response.Features[0].Center, zoom);
			}
		}

		void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response, bool resetCamera)
		{
			Debug.Log("ForwardGeocoder response :::");
			if (response == null)
			{
				return;
			}
			if (resetCamera)
			{
				_camera.transform.position = _cameraStartPos;
			}
			ForwardGeocoder_OnGeocoderResponse(response);
		}

		void Reload(float value)
		{
			Debug.Log("Map reloaded :::");
			if (_reloadRoutine != null)
			{
				StopCoroutine(_reloadRoutine);
				_reloadRoutine = null;
			}
			if (this.gameObject.activeInHierarchy)
				_reloadRoutine = StartCoroutine(ReloadAfterDelay((int)value));
		}

		IEnumerator ReloadAfterDelay(int zoom)
		{
			Debug.Log("Map reload after delay :::");
			yield return _wait;
			_camera.transform.position = _cameraStartPos;
			_map.UpdateMap(_map.CenterLatitudeLongitude, zoom);
			_reloadRoutine = null;
		}
	}
}