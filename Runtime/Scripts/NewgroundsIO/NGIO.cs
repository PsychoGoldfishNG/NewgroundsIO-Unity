// External events
#if !UNITY_EDITOR
#define UNITY_BUILD
#endif

using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Diagnostics;

/// <summary>
/// A static wrapper for the NewgroundsIO library.
/// </summary>
public static class NGIO {

	/** ================================ Constants ================================= **/

	// preloading statuses
	public const string STATUS_INITIALIZED			= "initialized";
	public const string STATUS_CHECKING_LOCAL_VERSION	= "checking-local-version";
	public const string STATUS_LOCAL_VERSION_CHECKED	= "local-version-checked";
	public const string STATUS_PRELOADING_ITEMS		= "preloading-items";
	public const string STATUS_ITEMS_PRELOADED		= "items-preloaded";
	public const string STATUS_READY			= "ready";

	// aliases from SessionState
	public const string STATUS_SESSION_UNINITIALIZED	= NewgroundsIO.SessionState.SESSION_UNINITIALIZED;
	public const string STATUS_WAITING_FOR_SERVER		= NewgroundsIO.SessionState.WAITING_FOR_SERVER;
	public const string STATUS_LOGIN_REQUIRED		= NewgroundsIO.SessionState.LOGIN_REQUIRED;
	public const string STATUS_WAITING_FOR_USER		= NewgroundsIO.SessionState.WAITING_FOR_USER;
	public const string STATUS_LOGIN_CANCELLED		= NewgroundsIO.SessionState.LOGIN_CANCELLED;
	public const string STATUS_LOGIN_SUCCESSFUL		= NewgroundsIO.SessionState.LOGIN_SUCCESSFUL;
	public const string STATUS_LOGIN_FAILED			= NewgroundsIO.SessionState.LOGIN_FAILED;
	public const string STATUS_USER_LOGGED_OUT		= NewgroundsIO.SessionState.USER_LOGGED_OUT;
	public const string STATUS_SERVER_UNAVAILABLE		= NewgroundsIO.SessionState.SERVER_UNAVAILABLE;
	public const string STATUS_EXCEEDED_MAX_ATTEMPTS	= NewgroundsIO.SessionState.EXCEEDED_MAX_ATTEMPTS;

	// scoreboard periods
	public const string PERIOD_TODAY = "D";
	public const string PERIOD_CURRENT_WEEK = "W";
	public const string PERIOD_CURRENT_MONTH = "M";
	public const string PERIOD_CURRENT_YEAR = "Y";
	public const string PERIOD_ALL_TIME = "A";

	/** ================================ Public Vars ================================ **/

	/** ============================= Public Properties ============================= **/

	/// <summary>The user's overall Newgrounds medal score</summary>
	public static int medalScore { get; private set; } = -1;

	/// <summary>A dictionary of any preloaded medals, keyed on medal ids</summary>
	public static Dictionary<int, NewgroundsIO.objects.Medal> medals { get; private set; } = null;

	/// <summary>A dictionary of any preloaded medals, keyed on scoreboard ids</summary>
	public static Dictionary<int, NewgroundsIO.objects.ScoreBoard> scoreBoards { get; private set; } = null;
	
	/// <summary>A dictionary of any preloaded medals, keyed on the save slot number</summary>
	public static Dictionary<int, NewgroundsIO.objects.SaveSlot> saveSlots { get; private set; } = null;

	/// <summary>The last time a component or queue was executed</summary>
	public static DateTime lastExecution { get; private set; } = DateTime.Now;

	/// <summary>Will be true if we've called Init().</summary>
	public static bool isInitialized { get; private set; }

	/// <summary>Returns true if we currently have a valid session ID.</summary>
	public static bool hasSession { get { return session is not null && !session.expired; }}

	/// <summary>Returns true if we currently have a valid session ID.</summary>
	public static bool hasUser { get { return user is not null; }}

	/// <summary>Will be true if we've finished logging in and preloading data.</summary>
	public static bool isReady { get { return lastConnectionStatus == STATUS_READY; }}

	/// <summary>Contains all information about the current user session.</summary>
	public static NewgroundsIO.objects.Session session { get { return ngioCore is null ? null : ngioCore.session; }}

	/// <summary>Will be null unless there was an error in our session.</summary>
	public static NewgroundsIO.objects.Error sessionError { get; private set; } = null;

	/// <summary>Contains user information if the user is logged in. Otherwise null.</summary>
	public static NewgroundsIO.objects.User user { get { return session is null ? null : session.user; }}

	/// <summary>Will be set to false if the local copy of the game is being hosted illegally.</summary>
	public static bool legalHost { get; private set; } = true;

	/// <summary>Will be set to true if this is an out-of-date copy of the game.</summary>
	public static bool isDeprecated { get; private set; } = false;

	/// <summary>This is the version number(string) of the newest available copy of the game.</summary>
	public static string newestVersion { get; private set; } = null;

	/// <summary>Contains the last connection status. Value will be one of the STATUS_XXXXX constants.</summary>
	public static string lastConnectionStatus { get; private set; }
	
	/// <summary>Will be true if the user opened the login page via OpenLoginPage().</summary>
	public static bool loginPageOpen { get; private set; } = false;

	/// <summary>The current version of the Newgrounds.io gateway.</summary>
	public static string gatewayVersion { get; private set; } = null;
	
	/// <summary>Stores the last medal that was unlocked.</summary>
	public static NewgroundsIO.objects.Medal lastMedalUnlocked { get; private set; } = null;

	/// <summary>Stores the last scoreboard that was posted to.</summary>
	public static NewgroundsIO.objects.ScoreBoard lastBoardPosted { get; private set; } = null;

	/// <summary>Stores the last score that was posted to.</summary>
	public static NewgroundsIO.objects.Score lastScorePosted { get; private set; } = null;

	/// <summary>Stores the last scores that were loaded.</summary>
	public static NewgroundsIO.results.ScoreBoard.getScores lastGetScoresResult { get; private set; } = null;

	/// <summary>Stores the last event that was logged</summary>
	public static string lastLoggedEvent { get; private set; } = null;

	/// <summary>Stores the last saveSlot that had data loaded.</summary>
	public static NewgroundsIO.objects.SaveSlot lastSaveSlotLoaded { get; private set; } = null;

	/// <summary>Stores the last saveSlot that had data saved.</summary>
	public static NewgroundsIO.objects.SaveSlot lastSaveSlotSaved { get; private set; } = null;

	/// <summary>Stores the last DateTime that was loaded from the API.</summary>
	public static string lastDateTime { get; private set; } = "0000-00-00";

	/// <summary>Stores the last unix timestamp that was loaded API.</summary>
	public static int lastTimeStamp { get; private set; } = 0;

	/// <summary>Stores wether the last server ping succeeded.</summary>
	public static bool lastPingSuccess { get; private set; } = true;

	/// <summary>A reference to the NewgroundsIO.Core instance created in Init().</summary>
	public static NewgroundsIO.Core ngioCore { get; private set; } = null;


	/** =============================== Private Vars ================================ **/

	// Init options
	private static string _version = "0.0.0";
	private static bool _debugMode = false;
	private static bool _checkHostLicense = false;
	private static bool _autoLogNewView = false;
	private static bool _preloadMedals = false;
	private static bool _preloadScoreBoards = false;
	private static bool _preloadSaveSlots = false;

	// Connection states
	private static bool _sessionReady = false;
	private static bool _skipLogin = false;
	private static bool _checkingConnectionStatus = false;

	// tells the Newgrounds page a medal was unlocked so it can highlight in real time
	// only used in WebGL builds
	[DllImport("__Internal"), Conditional("UNITY_BUILD"), Conditional("UNITY_WEBGL")]
	private static extern void OnMedalUnlocked(int medal_id);

	// tells the Newgrounds page a score was posted so the current board can refresh
	// only used in WebGL builds
	[DllImport("__Internal"), Conditional("UNITY_BUILD"), Conditional("UNITY_WEBGL")]
	private static extern void OnScorePosted(int board_id);

	/** ============================= Misc Public Methods ============================ **/

	/// <summary>
	/// Initializes the NGIO wrapper. You must call this BEFORE using any other methods!
	/// </summary>
	/// <param name="appID">The App ID from your Newgrounds Project's "API Tools" page.</param>
	/// <param name="aesKey">The AES-128 encryption key from your Newgrounds Project's "API Tools" page.</param>
	/// <param name="options">
	/// A dictionary of option names and values. Options are:
	///  * "debugMode"          	- Set to true to run in debug mode.
	///  * "version"            	- A string in "X.X.X" format indicating the current version of this game.
	///  * "checkHostLicense"   	- Set to true to check if the site hosting your game has been blocked.
	///  * "preloadMedals"      	- Set to true to preload medals (will show if the player has any unlocked, and get their current medal score).
	///  * "preloadeScoreBoards"	- Set to true to preload Score Board information.
	///  * "preloadeSaveSlots"  	- Set to true to preload Save Slot information.
	///  * "autoLogNewView"     	- Set to true to automatcally log a new view to your stats.
	/// </param>
	public static void Init(string appID, string aesKey, Dictionary<string,object> options=null)
	{
		if (!isInitialized) {
			ngioCore = new NewgroundsIO.Core(appID, aesKey);
			ngioCore.ServerResponse += OnServerResponse;

			if (!(options is null)) {
				foreach(var (prop, val) in options)
				{
					if (prop == "version")			_version = (string)val;
					if (prop == "debugMode")		_debugMode = (bool)val;
					if (prop == "checkHostLicense")		_checkHostLicense = (bool)val;
					if (prop == "autoLogNewView")		_autoLogNewView = (bool)val;
					if (prop == "preloadMedals")		_preloadMedals = (bool)val;
					if (prop == "preloadScoreBoards")	_preloadScoreBoards = (bool)val;
					if (prop == "preloadSaveSlots")		_preloadSaveSlots = (bool)val;
				}
			}

			ngioCore.debug = _debugMode;

			lastConnectionStatus = STATUS_INITIALIZED;

			isInitialized = true;
		}
	}

	/** ======================== Public Login/Session Methods ======================== **/

	/// <summary>
	/// Call this if you want to skip logging the user in.
	/// </summary>
	public static void SkipLogin()
	{
		if (!_sessionReady)	_skipLogin = true;
	}

	/// <summary>
	/// Opens the Newgrounds login page in a new browser tab.
	/// </summary>
	public static void OpenLoginPage()
	{
		if (!loginPageOpen) {
			loginPageOpen = true;
			session.OpenLoginPage();
		} else {
			UnityEngine.Debug.LogWarning("loginPageOpen is true. Use CancelLogin to reset.");
		}
	}

	/// <summary>
	/// If the user opened the NG login page, you can call this to cancel the login attempt.
	/// </summary>
	public static void CancelLogin()
	{
		ResetConnectionStatus();
		session.CancelLogin();
	}

	/** ============================ Public Loader Methods =========================== **/

	/// <summary>
	/// Loads "Your Website URL", as defined on your App Settings page, in a new browser tab.
	/// </summary>
	public static void LoadAuthorUrl()
	{
		ngioCore.LoadComponent(new NewgroundsIO.components.Loader.loadAuthorUrl());
	}

	/// <summary>
	/// Loads our "Official Version URL", as defined on your App Settings page, in a new browser tab.
	/// </summary>
	public static void LoadOfficialUrl()
	{
		ngioCore.LoadComponent(new NewgroundsIO.components.Loader.loadOfficialUrl());
	}

	/// <summary>
	/// Loads the Games page on Newgrounds in a new browser tab.
	/// </summary>
	public static void LoadMoreGames()
	{
		ngioCore.LoadComponent(new NewgroundsIO.components.Loader.loadMoreGames());
	}

	/// <summary>
	/// Loads the Newgrounds frontpage in a new browser tab.
	/// </summary>
	public static void LoadNewgrounds()
	{
		ngioCore.LoadComponent(new NewgroundsIO.components.Loader.loadNewgrounds());
	}

	/// <summary>
	/// Loads a referral URL, as defined in your Events & Loaders page, in a new browser tab.
	/// </summary>
	/// <param name="referralName">The name of your custom referral.</param>
	public static void LoadReferral(string referralName)
	{
		var component = new NewgroundsIO.components.Loader.loadReferral();
		component.referral_name = referralName;
		ngioCore.LoadComponent(component);
	}

	/** ============================ Public Medal Methods ============================ **/

	/// <summary>
	/// Gets a preloaded Medal object.
	/// </summary>	
	/// <param name="medalID">The ID of the medal</param>
	public static NewgroundsIO.objects.Medal GetMedal(int medalID)
	{
		if (medals is null) {
			UnityEngine.Debug.LogError("NGIO Error: Can't use GetMedal without setting preloadMedals option to true");
			return null;
		}
		return medals.ContainsKey(medalID) ? medals[medalID] : null;
	}

	/** ========================== Public ScoreBoard Methods ========================= **/

	/// <summary>
	/// Gets a preloaded ScoreBoard object.
	/// </summary>	
	/// <param name="boardID">The ID of the score board</param>
	public static NewgroundsIO.objects.ScoreBoard GetScoreBoard(int boardID)
	{
		if (scoreBoards is null) {
			UnityEngine.Debug.LogError("NGIO Error: Can't use GetScoreBoard without setting preloadScoreBoards option to true");
			return null;
		}
		return scoreBoards.ContainsKey(boardID) ? scoreBoards[boardID] : null;
	}

	/** =========================== Public SaveSlot Methods ========================== **/

	/// <summary>
	/// Gets a preloaded SaveSlot object. (Use GetSaveSlotData to get actual save file)
	/// </summary>
	/// <param name="slotID">The desired slot number</param>
	public static NewgroundsIO.objects.SaveSlot GetSaveSlot(int slotID)
	{
		if (saveSlots is null) {
			UnityEngine.Debug.LogError("NGIO Error: Can't use GetSaveSlot without setting preloadSaveSlots option to true");
			return null;
		}
		return saveSlots.ContainsKey(slotID) ? saveSlots[slotID] : null;
	}

	/// <summary>
	/// Gets the number of non-empty save slots.
	/// </summary>
	public static short GetTotalSaveSlots()
	{
		short total = 0;

		foreach(var (id, slot) in saveSlots) {
			if (slot.hasData) total++; 
		}

		return total;
	}


	/** ======================= Public Login/Preload Coroutines ====================== **/

	/// <summary>
	/// Intended to be called from your game loop, this does an entire process of things based on your Init() options:
	///  * Checks if the hosting site has a legal copy of this game
	///  * Checks for a newer version of the game
	///  * Makes sure you have a user session
	///  * Checks if the current user is logged in
	///  * Preloads Medals, Saveslots, etc
	///  * Logs a new view to your stats
	/// 
	/// Whenever a new operation begins or ends, the current state will be passed to your handler function.
	/// </summary>
	/// <param name="handler">A function to be called when there's a change of status. Will match one of the STATUS_XXXX constants.</param>
	public static IEnumerator GetConnectionStatus(Action<string> handler)
	{

		if (_checkingConnectionStatus || (lastConnectionStatus is null) || (session is null)) yield break;

		_checkingConnectionStatus = true;

		if (lastConnectionStatus == STATUS_INITIALIZED)
		{
			lastConnectionStatus = STATUS_CHECKING_LOCAL_VERSION;
			handler(lastConnectionStatus);

			yield return CheckLocalVersion();
			handler(lastConnectionStatus);

		} else if (!_sessionReady) {

			yield return session.Update();

			if (session.statusChanged || _skipLogin) {
				lastConnectionStatus = session.status;

				if (session.status == NewgroundsIO.SessionState.LOGIN_SUCCESSFUL || _skipLogin) {
					lastConnectionStatus = NewgroundsIO.SessionState.LOGIN_SUCCESSFUL;
					_sessionReady = true;
				}

				handler(lastConnectionStatus);
			}

			_skipLogin = false;

		} else if (lastConnectionStatus == NewgroundsIO.SessionState.LOGIN_SUCCESSFUL) {

			lastConnectionStatus = STATUS_PRELOADING_ITEMS;
			handler(lastConnectionStatus);

			yield return PreloadItems();
			handler(lastConnectionStatus);

			_skipLogin = false;

		} else if (lastConnectionStatus == STATUS_ITEMS_PRELOADED) {
			loginPageOpen = false;
			lastConnectionStatus = STATUS_READY;
			handler(lastConnectionStatus);

			_skipLogin = false;
		}

		_checkingConnectionStatus = false;
	}

	/// <summary>
	/// Logs the current use out of the game (locally and on the server) and resets the connection status.
	/// </summary>
	public static IEnumerator LogOut()
	{
		yield return session.LogOut();
		ResetConnectionStatus();
	}

	/** ========================= Public KeepAlive Coroutine ========================= **/

	/// <summary>
	/// Call this in your game loop to prevent sessions from expiring.
	/// This will only hit the server once every 30 seconds, no matter how often you call it.
	/// </summary>
	public static IEnumerator KeepSessionAlive()
	{
		if (!hasUser) yield break;

		TimeSpan elapsed = DateTime.Now.Subtract(lastExecution);
		if (elapsed.Seconds >= 30) {
			lastExecution = DateTime.Now;
			yield return ngioCore.ExecuteComponent(new NewgroundsIO.components.Gateway.ping());
		}
	}

	/** ========================= Public CloudSave Coroutines ======================== **/

	/// <summary>
	/// Loads the actual save file from a save slot, and passes the string result to a callback function.
	/// </summary>
	/// <param name="slotID">The slot number to load from</param>
	/// <param name="callback">A function to run when the file has been loaded</param>
	public static IEnumerator GetSaveSlotData(int slotID, Action<string> callback)
	{
		if (saveSlots is null) {
			UnityEngine.Debug.LogError("GetSaveSlotData data called without any preloaded save slots.");
			callback(null);
		}

		var slot = GetSaveSlot(slotID);
		lastSaveSlotLoaded = slot;
		yield return slot.GetData(callback);
	}


	/// <summary>
	/// Loads the actual save file from a save slot and returns the save slot to an optional callback function when complete.
	/// </summary>
	/// <param name="slotID">The slot number to save to.</param>
	/// <param name="data">The (serialized) data you want to save.</param>
	/// <param name="callback">A function to run when the file finished saving.</param>
	public static IEnumerator SetSaveSlotData(int slotID, string data, Action<NewgroundsIO.objects.SaveSlot> callback=null)
	{
		if (saveSlots is null) {
			UnityEngine.Debug.LogError("SetSaveSlotData data called without any preloaded save slots.");
			if (callback is not null) callback(null);
			yield break;
		}
		
		var slot = GetSaveSlot(slotID);
		if (slot is null) {
			UnityEngine.Debug.LogError("Slot #"+slotID+" does not exist.");
			if (callback is not null) callback(null);
			yield break;
		}
		yield return slot.SetData(data);
		
		if (callback is not null) callback(lastSaveSlotSaved);
	}

	/** =========================== Public Event Coroutines ========================== **/

	/// <summary>
	/// Logs a custom event and returns the eventName to an optional callback function when complete.
	/// </summary>
	/// <param name="eventName">The name of the event to log.</param>
	/// <param name="callback">A function to run when the event has logged.</param>
	public static IEnumerator LogEvent(string eventName, Action<string> callback=null)
	{
		var component = new NewgroundsIO.components.Event.logEvent();
		component.event_name = eventName;
		yield return ngioCore.ExecuteComponent(component);
		
		if (callback is not null) callback(lastLoggedEvent);
	}

	/** ========================== Public Gateway Coroutines ========================= **/

	/// <summary>
	/// Loads the current DateTime from the server and returns it to an optional callback function.
	/// </summary>
	/// <param name="callback">A function to run when the datetime has loaded.</param>
	public static IEnumerator GetDateTime(Action<string,int> callback=null)
	{
		var component = new NewgroundsIO.components.Gateway.getDatetime();

		yield return ngioCore.ExecuteComponent(component);

		if (callback is not null) callback(lastDateTime,lastTimeStamp);
	}

	/** =========================== Public Medal Coroutines ========================== **/

	/// <summary>
	/// Attempts to unlock a medal and returns the medal to an optional callback function when complete.
	/// </summary>
	/// <param name="medalID">The id of the medal you are unlocking.</param>
	/// <param name="callback">A function to run when the medal has unlocked.</param>
	public static IEnumerator UnlockMedal(int medalID, Action<NewgroundsIO.objects.Medal> callback=null)
	{
		if (medals is null) {
			UnityEngine.Debug.LogError("UnlockMedal called without any preloaded medals.");
			if (callback is not null) callback(null);
			yield break;
		}
		var medal = GetMedal(medalID);
		if (medal is null) {
			UnityEngine.Debug.LogError("Medal #"+medalID+" does not exist.");
			if (callback is not null) callback(null);
			yield break;
		}

		yield return medal.Unlock();
		
		if (callback is not null) callback(lastMedalUnlocked);
	}

	/** ======================== Public Scoreboard Coroutines ======================== **/

	/// <summary>
	/// Posts a score and returns the score and scoreboard to an optional callback function when complete.
	/// </summary>
	/// <param name="boardID">The id of the scoreboard you are posting to.</param>
	/// <param name="value">The integer value of your score.</param>
	/// <param name="tag">An optional tag to attach to the score (use null for no tag).</param>
	/// <param name="callback">A function to run when the score has posted.</param>
	public static IEnumerator PostScore(int boardID, int value, string tag=null, Action<NewgroundsIO.objects.ScoreBoard,NewgroundsIO.objects.Score> callback=null)
	{
		if (scoreBoards is null) {
			UnityEngine.Debug.LogError("PostScore called without any preloaded scoreboards.");
			if (callback is not null) callback(null, null);
			yield break;
		}
		var board = GetScoreBoard(boardID);
		if (board is null) {
			UnityEngine.Debug.LogError("ScoreBoard #"+boardID+" does not exist.");
			if (callback is not null) callback(null, null);
			yield break;
		}

		yield return board.PostScore(value, tag);
		
		if (callback is not null) callback(lastBoardPosted, lastScorePosted);
	}

	/// <summary>
	/// Gets the best scores for a board and returns the board, score list, period, tag and social bool to an optional callback.
	/// </summary>
	/// <param name="boardID">The id of the scoreboard you loading from.</param>
	/// <param name="period">The time period to get scores from. Will match one of the PERIOD_XXXX constants.</param>
	/// <param name="tag">An optional tag to filter results by (use null for no tag).</param>
	/// <param name="social">Set to true to only get scores from the user's friends.</param>
	/// <param name="callback">A function to run when the scores have been loaded.</param>
	public static IEnumerator GetScores(int boardID, string period="D", string tag=null, bool social=false, Action<NewgroundsIO.objects.ScoreBoard, List<NewgroundsIO.objects.Score>, string, string, bool> callback=null)
	{
		if (scoreBoards is null) {
			UnityEngine.Debug.LogError("GetScores called without any preloaded scoreboards.");
			if (callback is not null) callback(null, null, period, tag, social);
			yield break;
		}
		var board = GetScoreBoard(boardID);
		if (board is null) {
			UnityEngine.Debug.LogError("ScoreBoard #"+boardID+" does not exist.");
			if (callback is not null) callback(null, null, period, tag, social);
			yield break;
		}
		
		yield return board.GetScores(period, tag, social);

		if (callback is not null) callback(board, lastGetScoresResult.scores, period, tag, social);
	}

	/** ===================== Private Login/Preloader Enumerators ==================== **/

	// Loads the latest version info, and will get the host license and log a view if those Init() options are enabled.
	private static IEnumerator CheckLocalVersion() {
		
		var component = new NewgroundsIO.components.App.getCurrentVersion();
		component.version = _version;
		ngioCore.QueueComponent(component);

		ngioCore.QueueComponent(new NewgroundsIO.components.Gateway.getVersion());
		ngioCore.QueueComponent(new NewgroundsIO.components.Gateway.getDatetime());

		if (_autoLogNewView) {
			ngioCore.QueueComponent(new NewgroundsIO.components.App.logView());
		}
		if (_checkHostLicense) {
			ngioCore.QueueComponent(new NewgroundsIO.components.App.getHostLicense());
		}

		yield return ngioCore.ExecuteQueue();

		lastConnectionStatus = STATUS_LOCAL_VERSION_CHECKED;

		if (!legalHost) {
			UnityEngine.Debug.LogWarning("This host has been blocked fom hosting this game!");
			_sessionReady = true;
			lastConnectionStatus = STATUS_ITEMS_PRELOADED;
		}
	}

	// Preloads any items that were set in the Init() options.
	private static IEnumerator PreloadItems() {
		
		if (_preloadMedals) {
			ngioCore.QueueComponent(new NewgroundsIO.components.Medal.getMedalScore());
			ngioCore.QueueComponent(new NewgroundsIO.components.Medal.getList());
		}
		if (_preloadScoreBoards) {
			ngioCore.QueueComponent(new NewgroundsIO.components.ScoreBoard.getBoards());
		}
		if (!(user is null) && _preloadSaveSlots) ngioCore.QueueComponent(new NewgroundsIO.components.CloudSave.loadSlots());

		if (ngioCore.hasQueue) yield return ngioCore.ExecuteQueue();

		lastConnectionStatus = STATUS_ITEMS_PRELOADED;
	}


	/** =============================== Private Methods ============================== **/

	// Resets the connection state, typically after a user logs out or cances a login.
	private static void ResetConnectionStatus()
	{
		lastConnectionStatus = STATUS_INITIALIZED;
		loginPageOpen = false;
		_skipLogin = false;
		_sessionReady = false;
	}

	// Runs anytime the core gets a server response. Grabs individual result objects and runs them through HandleNewComponentResult().
	private static void OnServerResponse(NewgroundsIO.objects.Response response)
	{
		if (!(response is null) && response.success) {

			// make a note of our last update time
			lastExecution = DateTime.Now;

			if (response.isList) {
				response.resultList.ForEach(result => {
					if (!(result is null)) HandleNewComponentResult(result);
				});
			} else {
				if (!(response.result is null)) HandleNewComponentResult(response.result);
			}
		}
	}

	// Checks component results from every server response to see if they need any further handling.
	private static void HandleNewComponentResult(NewgroundsIO.BaseResult result)
	{
		if (!result.success)
		{
			UnityEngine.Debug.LogError(result.error.message+" \ncode("+result.error.code+")");
		}

		switch(result.__object) {

			/** ============================== App Info ============================== **/

			case "App.getCurrentVersion":

				var AppGetCurrentVersionResult = result as NewgroundsIO.results.App.getCurrentVersion;
				if (!result.success) return;

				// Save the latest version and note if this copy of the game is deprecated
				newestVersion = AppGetCurrentVersionResult.current_version;
				isDeprecated = AppGetCurrentVersionResult.client_deprecated;

				break;

			case "App.getHostLicense":

				var AppGetHostLicenseResult = result as NewgroundsIO.results.App.getHostLicense;
				if (!result.success) return;

				// Make a note of whether this game is being hosted legally or not
				legalHost = AppGetHostLicenseResult.host_approved;
				
				break;

			case "App.endSession":

				// reset the connection state if the user logged out
				ResetConnectionStatus();
				
				break;

			case "App.checkSession":

				// if something failes with a session check, reset the connection status
				if (!result.success) ResetConnectionStatus();
				
				break;

			/** ============================ Cloud Saves ============================= **/

			case "CloudSave.loadSlots":

				var CloudSaveLoadSlotsResult = result as NewgroundsIO.results.CloudSave.loadSlots;
				if (!result.success) return;

				// Store the loaded cloud saves in our dictionary so we can get them by slot number
				saveSlots = new Dictionary<int, NewgroundsIO.objects.SaveSlot>();
				CloudSaveLoadSlotsResult.slots.ForEach(slot => {
					saveSlots[slot.id] = slot.clone();
				});

				break;

			case "CloudSave.loadSlot":

				var CloudSaveLoadSlotResult = result as NewgroundsIO.results.CloudSave.loadSlot;
				if (!result.success) return;

				// Save, or replace, the slot in our dictionary so it can be retrieved by it's slot number
				if (!(saveSlots is null)) {
					var slot = CloudSaveLoadSlotResult.slot;
					saveSlots[slot.id] = slot.clone(saveSlots.ContainsKey(slot.id) ? saveSlots[slot.id] : null);
				}

				break;

			case "CloudSave.setData":

				var CloudSaveSaveSlotResult = result as NewgroundsIO.results.CloudSave.setData;
				if (!result.success) {
					lastSaveSlotSaved = null;
					return;
				}

				// Save, or replace, the slot in our dictionary so it can be retrieved by it's slot number
				if (!(saveSlots is null)) {
					var slot = CloudSaveSaveSlotResult.slot;
					saveSlots[slot.id] = slot.clone(saveSlots.ContainsKey(slot.id) ? saveSlots[slot.id] : null);
					lastSaveSlotSaved = saveSlots[slot.id];
				}

				break;

			case "CloudSave.clearSlot":

				var CloudSaveClearSlotResult = result as NewgroundsIO.results.CloudSave.clearSlot;
				if (!result.success) return;

				// Save, or replace, the slot in our dictionary so it can be retrieved by it's slot number
				if (!(saveSlots is null)) {
					var slot = CloudSaveClearSlotResult.slot;
					saveSlots[slot.id] = slot.clone(saveSlots.ContainsKey(slot.id) ? saveSlots[slot.id] : null);
				}

				break;


			/** ============================== Events ================================ **/
			
			case "Event.logEvent":

				var LogEventResult = result as NewgroundsIO.results.Event.logEvent;
				if (!result.success) {
					lastLoggedEvent = null;
					return;
				}

				lastLoggedEvent = LogEventResult.event_name;

				break;

			/** ============================== Gateway ================================ **/
			
			case "Gateway.getDatetime":

				var GatewayGetDateTimeResult = result as NewgroundsIO.results.Gateway.getDatetime;
				if (!result.success) {
					lastTimeStamp = 0;
					lastDateTime = "0000-00-00";
					return;
				}

				lastDateTime = GatewayGetDateTimeResult.datetime;
				lastTimeStamp = GatewayGetDateTimeResult.timestamp;

				break;

			case "Gateway.getVersion":

				var GatewayGetVersionResult = result as NewgroundsIO.results.Gateway.getVersion;
				if (!result.success) {
					gatewayVersion = null;
					return;
				}

				gatewayVersion = GatewayGetVersionResult.version;

				break;

			case "Gateway.ping":

				var PingResult = result as NewgroundsIO.results.Gateway.ping;
				lastPingSuccess = result.success;

				break;

			/** ============================== Medals ================================ **/
			
			case "Medal.getList":

				var MedalGetListResult = result as NewgroundsIO.results.Medal.getList;
				if (!result.success) return;

				// Store the loaded medals in our dictionary so we can get them by their ID
				medals = new Dictionary<int, NewgroundsIO.objects.Medal>();
				MedalGetListResult.medals.ForEach(medal => {
					medals[medal.id] = medal.clone();
				});

				break;

			case "Medal.unlock":

				var MedalUnlockResult = result as NewgroundsIO.results.Medal.unlock;
				if (!result.success) {
					lastMedalUnlocked = null;
					return;
				}

				// Save, or replace, the medal in our dictionary so it can be retrieved by it's ID
				if (!(medals is null)) {
					medals[MedalUnlockResult.medal.id] = MedalUnlockResult.medal.clone();
					lastMedalUnlocked = medals[MedalUnlockResult.medal.id];
				}

				// Record the current user's medal score
				medalScore = MedalUnlockResult.medal_score;

				#if !UNITY_EDITOR && UNITY_WEBGL
				OnMedalUnlocked(MedalUnlockResult.medal.id);
				#endif

				break;

			case "Medal.getMedalScore":

				var MedalGetScoreResult = result as NewgroundsIO.results.Medal.getMedalScore;
				if (!result.success) return;

				// Record the current user's medal score
				medalScore = MedalGetScoreResult.medal_score;

				break;

			/** ============================= ScoreBoards ============================ **/

			case "ScoreBoard.getBoards":
				var ScoreBoardGetBoardsResult = result as NewgroundsIO.results.ScoreBoard.getBoards;
				if (!result.success) return;

				// Store the loaded boards in our dictionary so we can get them by their ID
				scoreBoards = new Dictionary<int, NewgroundsIO.objects.ScoreBoard>();
				ScoreBoardGetBoardsResult.scoreboards .ForEach(board => {
					scoreBoards[board.id] = board.clone();
				});

				break;

			case "ScoreBoard.postScore":
				var ScoreBoardPostScoreResult = result as NewgroundsIO.results.ScoreBoard.postScore;
				if (!result.success) {
					lastScorePosted = null;
					lastBoardPosted = null;
					return;
				}

				lastScorePosted = ScoreBoardPostScoreResult.score;
				lastBoardPosted = GetScoreBoard(ScoreBoardPostScoreResult.scoreboard.id);

				#if !UNITY_EDITOR && UNITY_WEBGL
				OnScorePosted(ScoreBoardPostScoreResult.scoreboard.id);
				#endif

				break;

			case "ScoreBoard.getScores":
				lastGetScoresResult = result as NewgroundsIO.results.ScoreBoard.getScores;
				if (!result.success) {
					lastGetScoresResult = null;
					return;
				}

				break;
		}
	}
}
