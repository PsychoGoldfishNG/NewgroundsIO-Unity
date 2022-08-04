using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO {

	/// <summary>A class used to get object/component/result instances from a string name and deserialized JSON properties.</summary>
	class ObjectIndex {

		/// <summary>Handles creation of NewgroundsIO.BaseObject subclasses.</summary>
		/// <param name="name">The object's name/type.</param>
		/// <param name="json">The values to apply to the object.</param>
		/// <returns>A subclass instance, cast back to NewgroundsIO.BaseObject</returns>
		public static NewgroundsIO.BaseObject CreateObject(string name, object json)
		{
			switch (name.ToLower()) {

				case "request":

					NewgroundsIO.objects.Request new_Request = new NewgroundsIO.objects.Request();
					new_Request.FromJSON(json);
					return new_Request as NewgroundsIO.BaseObject;

				case "debug":

					NewgroundsIO.objects.Debug new_Debug = new NewgroundsIO.objects.Debug();
					new_Debug.FromJSON(json);
					return new_Debug as NewgroundsIO.BaseObject;

				case "response":

					NewgroundsIO.objects.Response new_Response = new NewgroundsIO.objects.Response();
					new_Response.FromJSON(json);
					return new_Response as NewgroundsIO.BaseObject;

				case "error":

					NewgroundsIO.objects.Error new_Error = new NewgroundsIO.objects.Error();
					new_Error.FromJSON(json);
					return new_Error as NewgroundsIO.BaseObject;

				case "session":

					NewgroundsIO.objects.Session new_Session = new NewgroundsIO.objects.Session();
					new_Session.FromJSON(json);
					return new_Session as NewgroundsIO.BaseObject;

				case "user":

					NewgroundsIO.objects.User new_User = new NewgroundsIO.objects.User();
					new_User.FromJSON(json);
					return new_User as NewgroundsIO.BaseObject;

				case "usericons":

					NewgroundsIO.objects.UserIcons new_UserIcons = new NewgroundsIO.objects.UserIcons();
					new_UserIcons.FromJSON(json);
					return new_UserIcons as NewgroundsIO.BaseObject;

				case "medal":

					NewgroundsIO.objects.Medal new_Medal = new NewgroundsIO.objects.Medal();
					new_Medal.FromJSON(json);
					return new_Medal as NewgroundsIO.BaseObject;

				case "scoreboard":

					NewgroundsIO.objects.ScoreBoard new_ScoreBoard = new NewgroundsIO.objects.ScoreBoard();
					new_ScoreBoard.FromJSON(json);
					return new_ScoreBoard as NewgroundsIO.BaseObject;

				case "score":

					NewgroundsIO.objects.Score new_Score = new NewgroundsIO.objects.Score();
					new_Score.FromJSON(json);
					return new_Score as NewgroundsIO.BaseObject;

				case "saveslot":

					NewgroundsIO.objects.SaveSlot new_SaveSlot = new NewgroundsIO.objects.SaveSlot();
					new_SaveSlot.FromJSON(json);
					return new_SaveSlot as NewgroundsIO.BaseObject;

			}

			return null;
		}

		/// <summary>Handles creation of NewgroundsIO.BaseComponent subclasses.</summary>
		/// <param name="name">The object's name/type.</param>
		/// <param name="json">The values to apply to the object.</param>
		/// <returns>A subclass instance, cast back to NewgroundsIO.BaseComponent</returns>
		public static NewgroundsIO.BaseComponent CreateComponent(string name, object json)
		{
			switch (name.ToLower()) {

				case "app.logview":

					NewgroundsIO.components.App.logView new_App_logView = new NewgroundsIO.components.App.logView();
					new_App_logView.FromJSON(json);
					return new_App_logView as NewgroundsIO.BaseComponent;

				case "app.checksession":

					NewgroundsIO.components.App.checkSession new_App_checkSession = new NewgroundsIO.components.App.checkSession();
					new_App_checkSession.FromJSON(json);
					return new_App_checkSession as NewgroundsIO.BaseComponent;

				case "app.gethostlicense":

					NewgroundsIO.components.App.getHostLicense new_App_getHostLicense = new NewgroundsIO.components.App.getHostLicense();
					new_App_getHostLicense.FromJSON(json);
					return new_App_getHostLicense as NewgroundsIO.BaseComponent;

				case "app.getcurrentversion":

					NewgroundsIO.components.App.getCurrentVersion new_App_getCurrentVersion = new NewgroundsIO.components.App.getCurrentVersion();
					new_App_getCurrentVersion.FromJSON(json);
					return new_App_getCurrentVersion as NewgroundsIO.BaseComponent;

				case "app.startsession":

					NewgroundsIO.components.App.startSession new_App_startSession = new NewgroundsIO.components.App.startSession();
					new_App_startSession.FromJSON(json);
					return new_App_startSession as NewgroundsIO.BaseComponent;

				case "app.endsession":

					NewgroundsIO.components.App.endSession new_App_endSession = new NewgroundsIO.components.App.endSession();
					new_App_endSession.FromJSON(json);
					return new_App_endSession as NewgroundsIO.BaseComponent;

				case "cloudsave.clearslot":

					NewgroundsIO.components.CloudSave.clearSlot new_CloudSave_clearSlot = new NewgroundsIO.components.CloudSave.clearSlot();
					new_CloudSave_clearSlot.FromJSON(json);
					return new_CloudSave_clearSlot as NewgroundsIO.BaseComponent;

				case "cloudsave.loadslot":

					NewgroundsIO.components.CloudSave.loadSlot new_CloudSave_loadSlot = new NewgroundsIO.components.CloudSave.loadSlot();
					new_CloudSave_loadSlot.FromJSON(json);
					return new_CloudSave_loadSlot as NewgroundsIO.BaseComponent;

				case "cloudsave.loadslots":

					NewgroundsIO.components.CloudSave.loadSlots new_CloudSave_loadSlots = new NewgroundsIO.components.CloudSave.loadSlots();
					new_CloudSave_loadSlots.FromJSON(json);
					return new_CloudSave_loadSlots as NewgroundsIO.BaseComponent;

				case "cloudsave.setdata":

					NewgroundsIO.components.CloudSave.setData new_CloudSave_setData = new NewgroundsIO.components.CloudSave.setData();
					new_CloudSave_setData.FromJSON(json);
					return new_CloudSave_setData as NewgroundsIO.BaseComponent;

				case "event.logevent":

					NewgroundsIO.components.Event.logEvent new_Event_logEvent = new NewgroundsIO.components.Event.logEvent();
					new_Event_logEvent.FromJSON(json);
					return new_Event_logEvent as NewgroundsIO.BaseComponent;

				case "gateway.getversion":

					NewgroundsIO.components.Gateway.getVersion new_Gateway_getVersion = new NewgroundsIO.components.Gateway.getVersion();
					new_Gateway_getVersion.FromJSON(json);
					return new_Gateway_getVersion as NewgroundsIO.BaseComponent;

				case "gateway.getdatetime":

					NewgroundsIO.components.Gateway.getDatetime new_Gateway_getDatetime = new NewgroundsIO.components.Gateway.getDatetime();
					new_Gateway_getDatetime.FromJSON(json);
					return new_Gateway_getDatetime as NewgroundsIO.BaseComponent;

				case "gateway.ping":

					NewgroundsIO.components.Gateway.ping new_Gateway_ping = new NewgroundsIO.components.Gateway.ping();
					new_Gateway_ping.FromJSON(json);
					return new_Gateway_ping as NewgroundsIO.BaseComponent;

				case "loader.loadofficialurl":

					NewgroundsIO.components.Loader.loadOfficialUrl new_Loader_loadOfficialUrl = new NewgroundsIO.components.Loader.loadOfficialUrl();
					new_Loader_loadOfficialUrl.FromJSON(json);
					return new_Loader_loadOfficialUrl as NewgroundsIO.BaseComponent;

				case "loader.loadauthorurl":

					NewgroundsIO.components.Loader.loadAuthorUrl new_Loader_loadAuthorUrl = new NewgroundsIO.components.Loader.loadAuthorUrl();
					new_Loader_loadAuthorUrl.FromJSON(json);
					return new_Loader_loadAuthorUrl as NewgroundsIO.BaseComponent;

				case "loader.loadreferral":

					NewgroundsIO.components.Loader.loadReferral new_Loader_loadReferral = new NewgroundsIO.components.Loader.loadReferral();
					new_Loader_loadReferral.FromJSON(json);
					return new_Loader_loadReferral as NewgroundsIO.BaseComponent;

				case "loader.loadmoregames":

					NewgroundsIO.components.Loader.loadMoreGames new_Loader_loadMoreGames = new NewgroundsIO.components.Loader.loadMoreGames();
					new_Loader_loadMoreGames.FromJSON(json);
					return new_Loader_loadMoreGames as NewgroundsIO.BaseComponent;

				case "loader.loadnewgrounds":

					NewgroundsIO.components.Loader.loadNewgrounds new_Loader_loadNewgrounds = new NewgroundsIO.components.Loader.loadNewgrounds();
					new_Loader_loadNewgrounds.FromJSON(json);
					return new_Loader_loadNewgrounds as NewgroundsIO.BaseComponent;

				case "medal.getlist":

					NewgroundsIO.components.Medal.getList new_Medal_getList = new NewgroundsIO.components.Medal.getList();
					new_Medal_getList.FromJSON(json);
					return new_Medal_getList as NewgroundsIO.BaseComponent;

				case "medal.getmedalscore":

					NewgroundsIO.components.Medal.getMedalScore new_Medal_getMedalScore = new NewgroundsIO.components.Medal.getMedalScore();
					new_Medal_getMedalScore.FromJSON(json);
					return new_Medal_getMedalScore as NewgroundsIO.BaseComponent;

				case "medal.unlock":

					NewgroundsIO.components.Medal.unlock new_Medal_unlock = new NewgroundsIO.components.Medal.unlock();
					new_Medal_unlock.FromJSON(json);
					return new_Medal_unlock as NewgroundsIO.BaseComponent;

				case "scoreboard.getboards":

					NewgroundsIO.components.ScoreBoard.getBoards new_ScoreBoard_getBoards = new NewgroundsIO.components.ScoreBoard.getBoards();
					new_ScoreBoard_getBoards.FromJSON(json);
					return new_ScoreBoard_getBoards as NewgroundsIO.BaseComponent;

				case "scoreboard.postscore":

					NewgroundsIO.components.ScoreBoard.postScore new_ScoreBoard_postScore = new NewgroundsIO.components.ScoreBoard.postScore();
					new_ScoreBoard_postScore.FromJSON(json);
					return new_ScoreBoard_postScore as NewgroundsIO.BaseComponent;

				case "scoreboard.getscores":

					NewgroundsIO.components.ScoreBoard.getScores new_ScoreBoard_getScores = new NewgroundsIO.components.ScoreBoard.getScores();
					new_ScoreBoard_getScores.FromJSON(json);
					return new_ScoreBoard_getScores as NewgroundsIO.BaseComponent;

			}

			return null;
		}

		/// <summary>Handles creation of NewgroundsIO.BaseResult subclasses.</summary>
		/// <param name="name">The object's name/type.</param>
		/// <param name="json">The values to apply to the object.</param>
		/// <returns>A subclass instance, cast back to NewgroundsIO.BaseResult</returns>
		public static NewgroundsIO.BaseResult CreateResult(string name, object json)
		{
			switch (name.ToLower()) {

				case "app.checksession":

					NewgroundsIO.results.App.checkSession new_App_checkSession = new NewgroundsIO.results.App.checkSession();
					new_App_checkSession.FromJSON(json);
					return new_App_checkSession as NewgroundsIO.BaseResult;

				case "app.gethostlicense":

					NewgroundsIO.results.App.getHostLicense new_App_getHostLicense = new NewgroundsIO.results.App.getHostLicense();
					new_App_getHostLicense.FromJSON(json);
					return new_App_getHostLicense as NewgroundsIO.BaseResult;

				case "app.getcurrentversion":

					NewgroundsIO.results.App.getCurrentVersion new_App_getCurrentVersion = new NewgroundsIO.results.App.getCurrentVersion();
					new_App_getCurrentVersion.FromJSON(json);
					return new_App_getCurrentVersion as NewgroundsIO.BaseResult;

				case "app.startsession":

					NewgroundsIO.results.App.startSession new_App_startSession = new NewgroundsIO.results.App.startSession();
					new_App_startSession.FromJSON(json);
					return new_App_startSession as NewgroundsIO.BaseResult;

				case "cloudsave.clearslot":

					NewgroundsIO.results.CloudSave.clearSlot new_CloudSave_clearSlot = new NewgroundsIO.results.CloudSave.clearSlot();
					new_CloudSave_clearSlot.FromJSON(json);
					return new_CloudSave_clearSlot as NewgroundsIO.BaseResult;

				case "cloudsave.loadslot":

					NewgroundsIO.results.CloudSave.loadSlot new_CloudSave_loadSlot = new NewgroundsIO.results.CloudSave.loadSlot();
					new_CloudSave_loadSlot.FromJSON(json);
					return new_CloudSave_loadSlot as NewgroundsIO.BaseResult;

				case "cloudsave.loadslots":

					NewgroundsIO.results.CloudSave.loadSlots new_CloudSave_loadSlots = new NewgroundsIO.results.CloudSave.loadSlots();
					new_CloudSave_loadSlots.FromJSON(json);
					return new_CloudSave_loadSlots as NewgroundsIO.BaseResult;

				case "cloudsave.setdata":

					NewgroundsIO.results.CloudSave.setData new_CloudSave_setData = new NewgroundsIO.results.CloudSave.setData();
					new_CloudSave_setData.FromJSON(json);
					return new_CloudSave_setData as NewgroundsIO.BaseResult;

				case "event.logevent":

					NewgroundsIO.results.Event.logEvent new_Event_logEvent = new NewgroundsIO.results.Event.logEvent();
					new_Event_logEvent.FromJSON(json);
					return new_Event_logEvent as NewgroundsIO.BaseResult;

				case "gateway.getversion":

					NewgroundsIO.results.Gateway.getVersion new_Gateway_getVersion = new NewgroundsIO.results.Gateway.getVersion();
					new_Gateway_getVersion.FromJSON(json);
					return new_Gateway_getVersion as NewgroundsIO.BaseResult;

				case "gateway.getdatetime":

					NewgroundsIO.results.Gateway.getDatetime new_Gateway_getDatetime = new NewgroundsIO.results.Gateway.getDatetime();
					new_Gateway_getDatetime.FromJSON(json);
					return new_Gateway_getDatetime as NewgroundsIO.BaseResult;

				case "gateway.ping":

					NewgroundsIO.results.Gateway.ping new_Gateway_ping = new NewgroundsIO.results.Gateway.ping();
					new_Gateway_ping.FromJSON(json);
					return new_Gateway_ping as NewgroundsIO.BaseResult;

				case "loader.loadofficialurl":

					NewgroundsIO.results.Loader.loadOfficialUrl new_Loader_loadOfficialUrl = new NewgroundsIO.results.Loader.loadOfficialUrl();
					new_Loader_loadOfficialUrl.FromJSON(json);
					return new_Loader_loadOfficialUrl as NewgroundsIO.BaseResult;

				case "loader.loadauthorurl":

					NewgroundsIO.results.Loader.loadAuthorUrl new_Loader_loadAuthorUrl = new NewgroundsIO.results.Loader.loadAuthorUrl();
					new_Loader_loadAuthorUrl.FromJSON(json);
					return new_Loader_loadAuthorUrl as NewgroundsIO.BaseResult;

				case "loader.loadreferral":

					NewgroundsIO.results.Loader.loadReferral new_Loader_loadReferral = new NewgroundsIO.results.Loader.loadReferral();
					new_Loader_loadReferral.FromJSON(json);
					return new_Loader_loadReferral as NewgroundsIO.BaseResult;

				case "loader.loadmoregames":

					NewgroundsIO.results.Loader.loadMoreGames new_Loader_loadMoreGames = new NewgroundsIO.results.Loader.loadMoreGames();
					new_Loader_loadMoreGames.FromJSON(json);
					return new_Loader_loadMoreGames as NewgroundsIO.BaseResult;

				case "loader.loadnewgrounds":

					NewgroundsIO.results.Loader.loadNewgrounds new_Loader_loadNewgrounds = new NewgroundsIO.results.Loader.loadNewgrounds();
					new_Loader_loadNewgrounds.FromJSON(json);
					return new_Loader_loadNewgrounds as NewgroundsIO.BaseResult;

				case "medal.getlist":

					NewgroundsIO.results.Medal.getList new_Medal_getList = new NewgroundsIO.results.Medal.getList();
					new_Medal_getList.FromJSON(json);
					return new_Medal_getList as NewgroundsIO.BaseResult;

				case "medal.getmedalscore":

					NewgroundsIO.results.Medal.getMedalScore new_Medal_getMedalScore = new NewgroundsIO.results.Medal.getMedalScore();
					new_Medal_getMedalScore.FromJSON(json);
					return new_Medal_getMedalScore as NewgroundsIO.BaseResult;

				case "medal.unlock":

					NewgroundsIO.results.Medal.unlock new_Medal_unlock = new NewgroundsIO.results.Medal.unlock();
					new_Medal_unlock.FromJSON(json);
					return new_Medal_unlock as NewgroundsIO.BaseResult;

				case "scoreboard.getboards":

					NewgroundsIO.results.ScoreBoard.getBoards new_ScoreBoard_getBoards = new NewgroundsIO.results.ScoreBoard.getBoards();
					new_ScoreBoard_getBoards.FromJSON(json);
					return new_ScoreBoard_getBoards as NewgroundsIO.BaseResult;

				case "scoreboard.postscore":

					NewgroundsIO.results.ScoreBoard.postScore new_ScoreBoard_postScore = new NewgroundsIO.results.ScoreBoard.postScore();
					new_ScoreBoard_postScore.FromJSON(json);
					return new_ScoreBoard_postScore as NewgroundsIO.BaseResult;

				case "scoreboard.getscores":

					NewgroundsIO.results.ScoreBoard.getScores new_ScoreBoard_getScores = new NewgroundsIO.results.ScoreBoard.getScores();
					new_ScoreBoard_getScores.FromJSON(json);
					return new_ScoreBoard_getScores as NewgroundsIO.BaseResult;

			}

			return null;
		}

	}
}