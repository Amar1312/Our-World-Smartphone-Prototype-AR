using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeScreenshot : MonoBehaviour {

	[SerializeField]
	GameObject blink;

	public RawImage _ScreenShotImage;


	#region TakePicture
	public void TakeCameraPicture()
	{
		StartCoroutine(TakeScreenshotAndSave());
	}

	private IEnumerator TakeScreenshotAndSave()
	{
		yield return new WaitForSeconds(0.5f);
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();


		_ScreenShotImage.texture = ss;
		StartCoroutine(IenumStartScreenshot());
		string _time = DateTime.Now.ToString();
		string[] _timearray = _time.Split(' ');
		Debug.Log("Current time ::" + _timearray[0] + _timearray[1]);
		string saveimage = _timearray[0] + "_" + _timearray[1] + ".png";
		NativeGallery.SaveImageToGallery(ss, "OCAR", "PCAR.png");
		yield return new WaitForSeconds(5.0f);
		Destroy(ss);
	}

	IEnumerator IenumStartScreenshot()
	{
		blink.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		blink.SetActive(false);

	}
	#endregion

}
