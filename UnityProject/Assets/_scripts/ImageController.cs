using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using static UnityChallenge.ImageController;

namespace UnityChallenge {
	public class ImageController : MonoBehaviour
	{
		public SystemUIElements systemUIElements;
		WebRequestsController currentRequest;

		private void Start()
		{
			systemUIElements.downloadButton.onClick.AddListener(OnDownloadButtonClick);
		}
		private void OnDisable()
		{
			ClearElements();
		}
		public void ClearElements(string urlText = "") {
			systemUIElements.urlText.text = urlText;
			systemUIElements.ErrorMessage.text = "";
			systemUIElements.progressBar.fillAmount = 0;
			systemUIElements.progressText.text = "";
			//clear image
			systemUIElements.downloadedImage.sprite = null;
			systemUIElements.downloadedImage.gameObject.SetActive(false);
		}
		void OnDownloadButtonClick() { 
			WebRequestsController webRequestsController = new WebRequestsController();
			currentRequest = webRequestsController;
			webRequestsController.OnImageDownload += OnImageDownload;
			webRequestsController.OnDownloadError += OnDownloadError;
			webRequestsController.OnDownloadProgressUpdate += OnDownloadProgressUpdate;
			StartCoroutine(webRequestsController.DownloadImageAndCreateSprite(systemUIElements.urlText.text));
			ClearElements(systemUIElements.urlText.text);
		}

		private void OnDownloadProgressUpdate(float progress)
		{
			systemUIElements.progressBar.fillAmount = progress;
			systemUIElements.progressText.text = progress.ToString("P");
		}
		private void OnDownloadError(UnityWebRequest webRequest)
		{
			systemUIElements.ErrorMessage.text = webRequest.error;
		}
		private void OnImageDownload(Sprite sprite)
		{
			systemUIElements.downloadedImage.sprite = sprite;
			systemUIElements.downloadedImage.gameObject.SetActive(true);
		}

		[System.Serializable]
		public class SystemUIElements
		{
			public TMP_InputField urlText;
			public Image downloadedImage;
			public Button downloadButton;
			public Image progressBar;
			public TextMeshProUGUI progressText;
			public TextMeshProUGUI ErrorMessage;
		}
		public class WebRequestsController
		{
			public Action<Sprite> OnImageDownload;
			public Action<UnityWebRequest> OnDownloadError;
			public Action<float> OnDownloadProgressUpdate;

			public IEnumerator DownloadImageAndCreateSprite(string url)
			{
				using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
				{
					webRequest.SendWebRequest();
					while (!webRequest.isDone)
					{
						OnDownloadProgressUpdate?.Invoke(webRequest.downloadProgress);
						yield return null;
					}

					if (webRequest.result != UnityWebRequest.Result.Success)
					{
						OnDownloadError?.Invoke(webRequest);
					}
					else
					{
						Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
						Rect rect = new Rect(0, 0, texture.width, texture.height);
						Vector2 pivot = new Vector2(0.5f, 0.5f);
						Sprite sprite = Sprite.Create(texture, rect, pivot);
						OnImageDownload?.Invoke(sprite);
					}
				}
			}
		}
	}
}
