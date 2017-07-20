using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public sealed class Unity_BatchBuild : EditorWindow {

	private const string m_menu_str = "Build/ProjectBuild";
	private static string m_title_str = "ProjectBuild";
	private static int m_min_win_width = 300;
	private static int m_min_win_height = 400;

	private static bool m_ios_relese = false;
	private static bool m_and_relese = false;

	[MenuItem(m_menu_str)]
	private static void OpenWindow() {
		// Windowを生成し、タイトルとサイズを設定します。
		Unity_BatchBuild win = (Unity_BatchBuild)EditorWindow.GetWindow(typeof(Unity_BatchBuild));
		win.title = m_title_str;
		win.minSize = new Vector2(m_min_win_width, m_min_win_height);
	}

	void OnGUI () {

		DateLoading();

		GUILayout.Label("iOS Build設定", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.Space();

		if (GUILayout.Button("\niOS Build\n"))
		{
			if (!BuildiOS(m_ios_relese)==false) EditorUtility.DisplayDialog("ビルドが完了しました", "「" + System.IO.Directory.GetCurrentDirectory() + "/」にプロジェクトファイルが保管されていますので、Xcodeで.ipaファイルの作成、アプリ申請を行ってください。", "");
			else EditorUtility.DisplayDialog("ビルドエラーです", "開発元へソースコードの確認をお願いします。", "");
		}

		m_ios_relese = EditorGUILayout.Toggle("Release版Build", m_ios_relese);
		if(EditorPrefs.GetBool("is_product_name") == false) EditorGUILayout.LabelField("Version", EditorPrefs.GetString("bundleversion"));
		else EditorGUILayout.LabelField("Version", EditorPrefs.GetString("ios_bundleversion"));

		EditorGUILayout.EndVertical();
		EditorGUILayout.Space();

		GUILayout.Label("Android Build設定", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.Space();

		if (GUILayout.Button("\nAndroid Build\n"))
		{
			if (!BuildAndroid(m_and_relese)==false) EditorUtility.DisplayDialog("ビルドが完了しました", "「" + System.IO.Directory.GetCurrentDirectory() + "/」に.apkファイルが保管されていますので、アプリ申請を行ってください。", "");
			else EditorUtility.DisplayDialog("ビルドエラーです", "開発元へソースコードの確認をお願いします。", "");
		}

		m_and_relese = EditorGUILayout.Toggle("Release版Build", m_and_relese);
		if(EditorPrefs.GetBool("is_product_name") == false) EditorGUILayout.LabelField("Version", EditorPrefs.GetString("bundleversion"));
		else EditorGUILayout.LabelField("Version", EditorPrefs.GetString("and_bundleversion"));
		EditorGUILayout.LabelField("Build Version", EditorPrefs.GetInt("buildversion").ToString());

		EditorGUILayout.EndVertical();
		EditorGUILayout.Space();

		EditorGUILayout.HelpBox("動作環境はUnityProが必須です！", MessageType.Warning);
		EditorGUILayout.HelpBox("リリース用のビルドは、ボタン下のチェックボックスにチェックを入れてください。", MessageType.Info);
		EditorGUILayout.HelpBox("各プラットフォームのバージョンは、\nProjectSettingで設定した値が使用されています。", MessageType.Info);
		EditorGUILayout.Space();

	}

	private static void DateLoading(){
		PlayerSettings.companyName = EditorPrefs.GetString("company");
		PlayerSettings.productName = EditorPrefs.GetString("product_name");

		PlayerSettings.bundleVersion = EditorPrefs.GetString("bundleversion");
		PlayerSettings.Android.bundleVersionCode = EditorPrefs.GetInt("buildversion");

		PlayerSettings.Android.keystoreName = EditorPrefs.GetString("keystorePath");
		PlayerSettings.Android.keystorePass = EditorPrefs.GetString("keystorePass");
		PlayerSettings.Android.keyaliasPass = EditorPrefs.GetString("keyaliasPass");
	}

	private static bool BuildiOS(bool release){

		DateLoading();

		Debug.Log("Start Build( iOS )");
		BuildOptions opt = BuildOptions.SymlinkLibraries;

		if(EditorPrefs.GetBool("is_product_name") == true)PlayerSettings.bundleIdentifier = EditorPrefs.GetString("ios_identifier");
		else PlayerSettings.bundleIdentifier =  EditorPrefs.GetString("identifier");

		if(EditorPrefs.GetBool("is_product_name") == true) PlayerSettings.bundleVersion = EditorPrefs.GetString("ios_bundleversion");
		else PlayerSettings.bundleVersion = EditorPrefs.GetString("bundleversion");

		if ( release==false ) opt |= BuildOptions.Development|BuildOptions.ConnectWithProfiler|BuildOptions.AllowDebugging;

		string errorMsg = BuildPipeline.BuildPlayer(GetScenes(),"ios",BuildTarget.iOS,opt);

		if ( string.IsNullOrEmpty(errorMsg) ) return true;
		return false;
	}

	private static bool BuildAndroid(bool release){

		DateLoading();

		Debug.Log("Start Build( Android )");
		BuildOptions opt = BuildOptions.None;

		if(EditorPrefs.GetBool("is_product_name") == true) PlayerSettings.bundleIdentifier = EditorPrefs.GetString("and_identifier");
		else EditorPrefs.GetString("identifier");

		if(EditorPrefs.GetBool("is_product_name") == true) PlayerSettings.bundleVersion = EditorPrefs.GetString("and_bundleversion");
		else EditorPrefs.GetString("bundleversion");

		if(release == true) PlayerSettings.Android.keyaliasName = EditorPrefs.GetString("keyaliasName");
		else PlayerSettings.Android.keyaliasName = "Unsigned";

		if ( !release==false )opt |= BuildOptions.Development|BuildOptions.ConnectWithProfiler|BuildOptions.AllowDebugging;

		string errorMsg = BuildPipeline.BuildPlayer(GetScenes(),"APK_Name.apk",BuildTarget.Android,opt);

		if ( string.IsNullOrEmpty(errorMsg) ) return true;
		return false;
	}

	private static string[] GetScenes(){
		List<string> scenes = new List<string>();

		foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes ) {
			if( scene.enabled ) {
				scenes.Add( scene.path );
				Debug.Log("Add Scene " + scene.path);
			}
		}
		return scenes.ToArray();
	}

	private static bool BatchBuildAndroid()
	{
		// 引数取得
		string[] args = System.Environment.GetCommandLineArgs();

		int i, len = args.Length;
		if (args.Length == 0)
			return false;
		switch (args [0]) {
		case "release":
			return BuildAndroid (true);
			break;
		case "debug":
			return BuildAndroid (false);	
			break;
		}
		return false;
	}
}