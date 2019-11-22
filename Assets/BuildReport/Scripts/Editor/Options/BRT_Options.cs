using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


namespace BuildReportTool
{

// class for holding options
// this is the class that is serialized when saving the options
[System.Serializable, XmlRoot("BuildReportToolOptions")]
public class SavedOptions
{
	public string EditorLogOverridePath;

	public bool IncludeSvnInUnused = true;
	public bool IncludeGitInUnused = true;

	public bool AllowDeletingOfUsedAssets;
	public bool CollectBuildInfo = true;

	public string BuildReportFolderName = BuildReportTool.Options.BUILD_REPORTS_DEFAULT_FOLDER_NAME;

	/// <summary>
	/// Where build reports are saved to: in user's My Documents, or outside the project folder.
	/// </summary>
	public int SaveType;

	/// <summary>
	/// Use file filters from global config, or use the ones embedded in the saved build report file.
	/// </summary>
	public int FilterToUseInt;

	public int AssetListPaginationLength = 300;
	public int UnusedAssetsEntriesPerBatch = 1000;

	public bool IncludeUsedAssetsInReportCreation = true;
	public bool IncludeUnusedAssetsInReportCreation = true;
	public bool IncludeUnusedPrefabsInReportCreation = true;
	public bool IncludeBuildSizeInReportCreation = true;

	public bool GetImportedSizesForUsedAssets = true;
	public bool GetImportedSizesForUnusedAssets = true;
	public bool GetProjectSettings = true;

	public bool AutoShowWindowAfterNormalBuild = true;
	public bool AutoShowWindowAfterBatchModeBuild;

	public bool UseThreadedFileLoading = false;

	public void OnBeforeSave()
	{
		// get rid of invalid characters for folder name
		// but still alow slash/backward slash so user could make relative paths
		
		BuildReportFolderName = BuildReportFolderName.Replace(":", string.Empty);
		BuildReportFolderName = BuildReportFolderName.Replace("*", string.Empty);
		BuildReportFolderName = BuildReportFolderName.Replace("?", string.Empty);
		BuildReportFolderName = BuildReportFolderName.Replace("\"", string.Empty);
		BuildReportFolderName = BuildReportFolderName.Replace("<", string.Empty);
		BuildReportFolderName = BuildReportFolderName.Replace(">", string.Empty);
		BuildReportFolderName = BuildReportFolderName.Replace("|", string.Empty);
	}

	public static void Save(string savePath, SavedOptions optionsToSave)
	{
		optionsToSave.OnBeforeSave();

		XmlSerializer x = new XmlSerializer( typeof(SavedOptions) );
		TextWriter writer = new StreamWriter(savePath);
		x.Serialize(writer, optionsToSave);
		writer.Close();
			
		//Debug.LogFormat("Build Report Tool: Saved options to: {0}", savePath);
	}

	public static SavedOptions Load(string path)
	{
		SavedOptions result = null;
			
		XmlSerializer x = new XmlSerializer( typeof(SavedOptions) );
			
		using(FileStream fs = new FileStream(path, FileMode.Open))
		{
			if (fs.Length == 0)
			{
				// nothing inside
				return null;
			}
			XmlReader reader = new XmlTextReader(fs);
			result = (SavedOptions)x.Deserialize(reader);
			fs.Close();
		}
			
		//Debug.LogFormat("Build Report Tool: Loaded options from: {0}", path);
		return result;
	}
}

public static class Options
{
	// =======================================================
	// constants
	public const string BUILD_REPORT_PACKAGE_MOVED_MSG = "BuildReport package seems to have been moved. Finding...";

	public const string BUILD_REPORT_PACKAGE_MISSING_MSG = "Unable to find BuildReport package folder! Cannot find suitable GUI Skin.\nTry editing the source code and change the value\nof `BUILD_REPORT_TOOL_DEFAULT_PATH` to what path the Build Report Tool is in.\nMake sure the folder is named \"BuildReport\".";

	public const string BUILD_REPORT_TOOL_DEFAULT_PATH = "Assets/BuildReport";
	public const string BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME = "BuildReport";

	public const string BUILD_REPORTS_DEFAULT_FOLDER_NAME = "UnityBuildReports";
	

	// =======================================================
	// 
	static BuildReportTool.SavedOptions _savedOptions;
	static string _foundPathForSavedOptions;
	const string SAVED_OPTIONS_FILENAME = "BuildReportToolOptions.xml";

	static string DefaultOptionsPath
	{
		get
		{
			return string.Format("{0}/BuildReport/{1}", Application.dataPath, SAVED_OPTIONS_FILENAME);
		}
	}

	static bool IsBuildReportInRegularPaths
	{
		get
		{
			return Directory.Exists(string.Format("{0}/BuildReport", Application.dataPath)) ||
			       Directory.Exists(string.Format("{0}/Plugins/BuildReport", Application.dataPath));
		}
	}

	public static string FoundPathForSavedOptions
	{
		get { return _foundPathForSavedOptions; }
	}

	static void InitializeOptionsIfNeeded()
	{
		if (_savedOptions == null)
		{
			_foundPathForSavedOptions = string.Empty;
		}

		if (string.IsNullOrEmpty(_foundPathForSavedOptions))
		{
			// look for the file in this order:
			// 1. inside the BuildReport folder
			// 2. at the very topmost Assets folder
			// 3. outside the Assets folder
			// 4. in the ProjectSettings folder
			// 5. in the User's My Documents folder

			
			// ---------------------------------------------------
			// look in /Assets/BuildReport/
			var optionsInBuildReportFolder = DefaultOptionsPath;
			if (File.Exists(optionsInBuildReportFolder))
			{
				_savedOptions = BuildReportTool.SavedOptions.Load(optionsInBuildReportFolder);
				_foundPathForSavedOptions = optionsInBuildReportFolder;
				return;
			}
			
			// ---------------------------------------------------
			// look in /Assets/Plugins/BuildReport/
			var optionsInPluginsBuildReport = string.Format("{0}/Plugins/BuildReport/{1}", Application.dataPath, SAVED_OPTIONS_FILENAME);
			if (File.Exists(optionsInPluginsBuildReport))
			{
				_savedOptions = BuildReportTool.SavedOptions.Load(optionsInPluginsBuildReport);
				_foundPathForSavedOptions = optionsInPluginsBuildReport;
				return;
			}
			
			// ---------------------------------------------------
			// search for "BuildReport" folder and look in there
			if (!IsBuildReportInRegularPaths)
			{
				string customBuildReportFolder = BuildReportTool.Util.FindAssetFolder(Application.dataPath, BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME);
				if (!string.IsNullOrEmpty(customBuildReportFolder))
				{
					var optionsInCustomBuildReportFolder = string.Format("{0}/{1}", customBuildReportFolder, SAVED_OPTIONS_FILENAME);
					if (File.Exists(optionsInCustomBuildReportFolder))
					{
						_savedOptions = BuildReportTool.SavedOptions.Load(optionsInCustomBuildReportFolder);
						_foundPathForSavedOptions = optionsInCustomBuildReportFolder;
						return;
					}
				}
			}

			// ---------------------------------------------------
			// look in /Assets/
			var optionsInTopmostAssets = string.Format("{0}/{1}", Application.dataPath, SAVED_OPTIONS_FILENAME);
			if (File.Exists(optionsInTopmostAssets))
			{
				_savedOptions = BuildReportTool.SavedOptions.Load(optionsInTopmostAssets);
				_foundPathForSavedOptions = optionsInTopmostAssets;
				return;
			}
			
			// ---------------------------------------------------
			// look in Unity project folder (where Assets, Library, and ProjectSettings folder are)
			var outsideAssets = BuildReportTool.Util.GetProjectPath(Application.dataPath);
			var optionsOutsideAssets = string.Format("{0}{1}", outsideAssets, SAVED_OPTIONS_FILENAME);
			if (File.Exists(optionsOutsideAssets))
			{
				_savedOptions = BuildReportTool.SavedOptions.Load(optionsOutsideAssets);
				_foundPathForSavedOptions = optionsOutsideAssets;
				return;
			}
			
			// ---------------------------------------------------
			// look inside ProjectSettings folder
			var optionsInProjectSettings = string.Format("{0}ProjectSettings/{1}", outsideAssets, SAVED_OPTIONS_FILENAME);
			//Debug.LogFormat("Looking in {0}", optionsInProjectSettings);
			if (File.Exists(optionsInProjectSettings))
			{
				_savedOptions = BuildReportTool.SavedOptions.Load(optionsInProjectSettings);
				_foundPathForSavedOptions = optionsInProjectSettings;
				return;
			}

			// ---------------------------------------------------
			// look in /My Documents/UnityBuildReports/
			var optionsInMyDocs = string.Format("{0}/{1}/{2}", BuildReportTool.Util.GetUserHomeFolder(), BUILD_REPORTS_DEFAULT_FOLDER_NAME, SAVED_OPTIONS_FILENAME);
			//Debug.LogFormat("Looking in {0}", optionsInMyDocs);
			if (File.Exists(optionsInMyDocs))
			{
				_savedOptions = BuildReportTool.SavedOptions.Load(optionsInMyDocs);
				_foundPathForSavedOptions = optionsInMyDocs;
				return;
			}

			// ---------------------------------------------------
		}

		// if the options file failed to load
		// one last try
		//
		if (_savedOptions == null)
		{
			if (!string.IsNullOrEmpty(_foundPathForSavedOptions) && File.Exists(_foundPathForSavedOptions))
			{
				// there's a valid options file already
				// just load that one
				_savedOptions = BuildReportTool.SavedOptions.Load(_foundPathForSavedOptions);
			}
		}
		
		// could not load the file, or there isn't one yet (at least, not in any recognized valid paths).
		// so create a new one at the default path
		if (_savedOptions == null)
		{
			_savedOptions = new BuildReportTool.SavedOptions();
			_foundPathForSavedOptions = DefaultOptionsPath;

			var defaultFolder = Path.GetDirectoryName(_foundPathForSavedOptions);
			if (!string.IsNullOrEmpty(defaultFolder) && !Directory.Exists(defaultFolder))
			{
				Directory.CreateDirectory(defaultFolder);
			}

			SavedOptions.Save(_foundPathForSavedOptions, _savedOptions);
			Debug.LogFormat("Build Report Tool: Created a new options file at: {0}", _foundPathForSavedOptions);
		}
	}

	public static void RefreshOptions()
	{
		_foundPathForSavedOptions = string.Empty;
		_savedOptions = null;
		InitializeOptionsIfNeeded();
	}

	static void SaveOptions()
	{
		if (string.IsNullOrEmpty(_foundPathForSavedOptions))
		{
			return;
		}
		if (_savedOptions == null || !File.Exists(_foundPathForSavedOptions))
		{
			_foundPathForSavedOptions = string.Empty;
			return;
		}
		
		SavedOptions.Save(_foundPathForSavedOptions, _savedOptions);
	}

	// =======================================================
	// user options

	public static string EditorLogOverridePath
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.EditorLogOverridePath;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.EditorLogOverridePath != value)
			{
				_savedOptions.EditorLogOverridePath = value;
				SaveOptions();
			}
		}
	}

	public static bool IncludeSvnInUnused
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.IncludeSvnInUnused;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.IncludeSvnInUnused != value)
			{
				_savedOptions.IncludeSvnInUnused = value;
				SaveOptions();
			}
		}
	}

	public static bool IncludeGitInUnused
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.IncludeGitInUnused;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.IncludeGitInUnused != value)
			{
				_savedOptions.IncludeGitInUnused = value;
				SaveOptions();
			}
		}
	}

	public static FileFilterDisplay GetOptionFileFilterDisplay()
	{
		return FileFilterDisplay.DropDown;
	}

	public static bool AllowDeletingOfUsedAssets
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.AllowDeletingOfUsedAssets;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.AllowDeletingOfUsedAssets != value)
			{
				_savedOptions.AllowDeletingOfUsedAssets = value;
				SaveOptions();
			}
		}
	}


	public static bool CollectBuildInfo
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.CollectBuildInfo;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.CollectBuildInfo != value)
			{
				_savedOptions.CollectBuildInfo = value;
				SaveOptions();
			}
		}
	}
	

	public static string BuildReportFolderName
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.BuildReportFolderName;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.BuildReportFolderName != value)
			{
				_savedOptions.BuildReportFolderName = value;
				SaveOptions();
			}
		}
	}


	public static string BuildReportSavePath
	{
		get
		{
			if (BuildReportTool.Options.SaveType == BuildReportTool.Options.SAVE_TYPE_PERSONAL)
			{
				return BuildReportTool.Util.GetUserHomeFolder() + "/" + BuildReportFolderName;
			}
			else
			{
				// assume BuildReportTool.Options.SaveType == BuildReportTool.Options.SAVE_TYPE_PROJECT

				return BuildReportTool.ReportGenerator.GetSavePathToProjectFolder() + "/" + BuildReportFolderName;
			}
		}
	}




	public static int SaveType
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.SaveType;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.SaveType != value)
			{
				_savedOptions.SaveType = value;
				SaveOptions();
			}
		}
	}

	public enum FileFilterDisplay
	{
		DropDown = 0,
		Buttons = 1
	}

	public const int SAVE_TYPE_PERSONAL = 0;
	public const int SAVE_TYPE_PROJECT = 1;



	public enum FilterToUseType
	{
		UseConfiguredFile,
		UseEmbedded
	}

	public static FilterToUseType GetOptionFilterToUse()
	{
		switch (FilterToUseInt)
		{
			case 0:
				return FilterToUseType.UseConfiguredFile;
			case 1:
				return FilterToUseType.UseEmbedded;
		}
		return FilterToUseType.UseConfiguredFile;
	}

	public static bool ShouldUseConfiguredFileFilters()
	{
		//Debug.Log("GetOptionFilterToUse() " + GetOptionFilterToUse());
		return GetOptionFilterToUse() == FilterToUseType.UseConfiguredFile;
	}

	public static bool ShouldUseEmbeddedFileFilters()
	{
		return GetOptionFilterToUse() == FilterToUseType.UseEmbedded;
	}

	public static int FilterToUseInt
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.FilterToUseInt;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.FilterToUseInt != value)
			{
				_savedOptions.FilterToUseInt = value;
				SaveOptions();
			}
		}
	}



	public static int AssetListPaginationLength
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.AssetListPaginationLength;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.AssetListPaginationLength != value)
			{
				_savedOptions.AssetListPaginationLength = value;
				SaveOptions();
			}
		}
	}


	public static int UnusedAssetsEntriesPerBatch
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.UnusedAssetsEntriesPerBatch;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.UnusedAssetsEntriesPerBatch != value)
			{
				_savedOptions.UnusedAssetsEntriesPerBatch = value;
				SaveOptions();
			}
		}
	}


	// Build Report Calculation
	//  Full report
	//  No prefabs in unused assets calculation
	//  No unused assets calculation, but still has used assets list (won't collect prefabs in scene)
	//  No used assets and unused assets calculation (overview only)

	public static bool IncludeUsedAssetsInReportCreation
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.IncludeUsedAssetsInReportCreation;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.IncludeUsedAssetsInReportCreation != value)
			{
				_savedOptions.IncludeUsedAssetsInReportCreation = value;
				SaveOptions();
			}
		}
	}

	public static bool IncludeUnusedAssetsInReportCreation
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.IncludeUnusedAssetsInReportCreation;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.IncludeUnusedAssetsInReportCreation != value)
			{
				_savedOptions.IncludeUnusedAssetsInReportCreation = value;
				SaveOptions();
			}
		}
	}

	public static bool IncludeUnusedPrefabsInReportCreation
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.IncludeUnusedPrefabsInReportCreation;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.IncludeUnusedPrefabsInReportCreation != value)
			{
				_savedOptions.IncludeUnusedPrefabsInReportCreation = value;
				SaveOptions();
			}
		}
	}


	public static bool IncludeBuildSizeInReportCreation
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.IncludeBuildSizeInReportCreation;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.IncludeBuildSizeInReportCreation != value)
			{
				_savedOptions.IncludeBuildSizeInReportCreation = value;
				SaveOptions();
			}
		}
	}


	public static bool GetImportedSizesForUsedAssets
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.GetImportedSizesForUsedAssets;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.GetImportedSizesForUsedAssets != value)
			{
				_savedOptions.GetImportedSizesForUsedAssets = value;
				SaveOptions();
			}
		}
	}

	public static bool GetImportedSizesForUnusedAssets
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.GetImportedSizesForUnusedAssets;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.GetImportedSizesForUnusedAssets != value)
			{
				_savedOptions.GetImportedSizesForUnusedAssets = value;
				SaveOptions();
			}
		}
	}



	public static bool GetProjectSettings
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.GetProjectSettings;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.GetProjectSettings != value)
			{
				_savedOptions.GetProjectSettings = value;
				SaveOptions();
			}
		}
	}


	public static bool IsCalculationLevelAtFull(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		return includeUsedAssets && includeUnusedAssets && includeUnusedPrefabs;
	}

	public static bool IsCalculationLevelAtNoUnusedPrefabs(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		return includeUsedAssets && includeUnusedAssets && !includeUnusedPrefabs;
	}

	public static bool IsCalculationLevelAtNoUnusedAssets(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		// unused prefabs are not checked. if unused assets are not calculated, it is understood that unused prefabs are not included
		return includeUsedAssets && !includeUnusedAssets;
	}

	public static bool IsCalculationLevelAtOverviewOnly(bool includeUsedAssets, bool includeUnusedAssets, bool includeUnusedPrefabs)
	{
		// if used assets not included, it is understood that unused assets are not included too.
		// if used assets are not included, there is no way to determing if an asset is unused.
		return !includeUsedAssets;
	}




	public static bool IsCurrentCalculationLevelAtFull
	{
		get
		{
			return IsCalculationLevelAtFull(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}
	public static bool IsCurrentCalculationLevelAtNoUnusedPrefabs
	{
		get
		{
			return IsCalculationLevelAtNoUnusedPrefabs(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}
	public static bool IsCurrentCalculationLevelAtNoUnusedAssets
	{
		get
		{
			return IsCalculationLevelAtNoUnusedAssets(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}
	public static bool IsCurrentCalculationLevelAtOverviewOnly
	{
		get
		{
			return IsCalculationLevelAtOverviewOnly(IncludeUsedAssetsInReportCreation, IncludeUnusedAssetsInReportCreation, IncludeUnusedPrefabsInReportCreation);
		}
	}


	public static void SetCalculationLevelToFull()
	{
		IncludeUsedAssetsInReportCreation = true;
		IncludeUnusedAssetsInReportCreation = true;
		IncludeUnusedPrefabsInReportCreation = true;
	}
	public static void SetCalculationLevelToNoUnusedPrefabs()
	{
		IncludeUsedAssetsInReportCreation = true;
		IncludeUnusedAssetsInReportCreation = true;
		IncludeUnusedPrefabsInReportCreation = false;
	}
	public static void SetCalculationLevelToNoUnusedAssets()
	{
		IncludeUsedAssetsInReportCreation = true;
		IncludeUnusedAssetsInReportCreation = false;
		IncludeUnusedPrefabsInReportCreation = false;
	}
	public static void SetCalculationLevelToOverviewOnly()
	{
		IncludeUsedAssetsInReportCreation = false;
		IncludeUnusedAssetsInReportCreation = false;
		IncludeUnusedPrefabsInReportCreation = false;
	}








	public static bool AutoShowWindowAfterNormalBuild
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.AutoShowWindowAfterNormalBuild;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.AutoShowWindowAfterNormalBuild != value)
			{
				_savedOptions.AutoShowWindowAfterNormalBuild = value;
				SaveOptions();
			}
		}
	}
	public static bool AutoShowWindowAfterBatchModeBuild
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.AutoShowWindowAfterBatchModeBuild;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.AutoShowWindowAfterBatchModeBuild != value)
			{
				_savedOptions.AutoShowWindowAfterBatchModeBuild = value;
				SaveOptions();
			}
		}
	}

	public static bool UseThreadedFileLoading
	{
		get
		{
			InitializeOptionsIfNeeded();
			return _savedOptions.UseThreadedFileLoading;
		}
		set
		{
			InitializeOptionsIfNeeded();
			if (_savedOptions.UseThreadedFileLoading != value)
			{
				_savedOptions.UseThreadedFileLoading = value;
				SaveOptions();
			}
		}
	}


	public static void SetAutoShowWindowTypeToNever()
	{
		AutoShowWindowAfterNormalBuild = false;
		AutoShowWindowAfterBatchModeBuild = false;
	}

	public static void SetAutoShowWindowTypeToAlways()
	{
		AutoShowWindowAfterNormalBuild = true;
		AutoShowWindowAfterBatchModeBuild = true;
	}

	public static void SetAutoShowWindowTypeToNotInBatchMode()
	{
		AutoShowWindowAfterNormalBuild = true;
		AutoShowWindowAfterBatchModeBuild = false;
	}



	public static bool IsAutoShowWindowTypeSetToNever
	{
		get
		{
			return
				!AutoShowWindowAfterNormalBuild &&
				!AutoShowWindowAfterBatchModeBuild;
		}
	}

	public static bool IsAutoShowWindowTypeSetToAlways
	{
		get
		{
			return
				AutoShowWindowAfterNormalBuild &&
				AutoShowWindowAfterBatchModeBuild;
		}
	}

	public static bool IsAutoShowWindowTypeSetToNotInBatchMode
	{
		get
		{
			return
				AutoShowWindowAfterNormalBuild &&
				!AutoShowWindowAfterBatchModeBuild;
		}
	}



	public static bool ShouldShowWindowAfterBuild
	{
		get
		{
			return
				(IsInBatchMode && AutoShowWindowAfterBatchModeBuild) ||
				(!IsInBatchMode && AutoShowWindowAfterNormalBuild);
		}
	}

	public static bool IsInBatchMode
	{
		get
		{
			return UnityEditorInternal.InternalEditorUtility.inBatchMode;


#if OTHER_BATCH_MODE_DETECTION_CODE
			// different ways to find out actually.
			// included here in case a new version of Unity
			// removes our current way of figuring out batchmode.

			// check the isHumanControllingUs bool
			return UnityEditorInternal.InternalEditorUtility.isHumanControllingUs;

			// check the command line args for "-batchmode"
			string[] arguments = Environment.GetCommandLineArgs();
			for (int n = 0, len = arguments.Length; n < len; ++n)
			{
				if (arguments[n] == "-batchmode")
				{
					return true;
				}
			}
			return false;
#endif
		}
	}
}

}
