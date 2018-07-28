﻿using System.IO;
using UnityEngine;

namespace BlueNoah.Download
{
	public static class DownloadConstant
	{

		public const string CONFIG_FILE = "assetbundle_config.json";

        public const int MAX_DOWNLOAD_COUNT = 5;

        public const int MAX_DOWNLOAD_SIZE = 10 * 1024 * 1024;

		public static string MANIFEST_FILE{
			get{
				return CommonConstant.PLATFORM;
			}
		}

		public static string APPLICATION_DATA_PATH {
			get {
#if UNITY_EDITOR
				return Application.dataPath;
#else
                return Application.persistentDataPath;
#endif
			}
		}

		public static string REMOTE_ASSET_PATH_MANIFEST {
			get {
				return REMOTE_ASSET_PATH_BASE + MANIFEST_FILE;
			}
		}

		public static string REMOTE_ASSET_PATH_CONFIG {
			get {
				return REMOTE_ASSET_PATH_BASE + CONFIG_FILE;
			}
		}

		public static string REMOTE_ASSET_PATH (string path)
		{
			return REMOTE_ASSET_PATH_BASE + path;
		}

		public static string REMOTE_ASSET_PATH_BASE {
			get {
				return
#if DEVELOP
                    "http://127.0.0.1/DownloadSample/" + PLATFORM + "/";
#elif STG
                    "http://127.0.0.1/DownloadSample/" + PLATFORM + "/";
#elif PRODUCT
                    "http://127.0.0.1/DownloadSample/" + PLATFORM + "/";
#else

                    "http://ultrasoul.ifkzgph84v.ap-northeast-1.elasticbeanstalk.com/AssetBundleBuilds/" + CommonConstant.PLATFORM + "/";
                    //"http://127.0.0.1/DownloadSample/" + PLATFORM + "/";
#endif
			}
		}

		public static string DOWNLOAD_ASSET_PATH_BASE_ROOT {
			get {
                return "/Framework_Download/DownloadAssets/" + CommonConstant.PLATFORM + "/";
			}
		}

		public static string DOWNLOAD_ASSET_PATH_BASE {
			get {
				#if UNITY_EDITOR
				return Application.dataPath + DOWNLOAD_ASSET_PATH_BASE_ROOT;
				#else
                return Application.persistentDataPath + DOWNLOAD_ASSET_PATH_BASE_ROOT;
				#endif
			}
		}

		public static string DOWNLOAD_ASSET_PATH_MANIFEST {
			get {
				return DOWNLOAD_ASSET_PATH_BASE + MANIFEST_FILE;
			}
		}

		public static string GetDownloadAssetBundlePath (string path)
		{
			return DOWNLOAD_ASSET_PATH_BASE + path;
		}

	}
}
