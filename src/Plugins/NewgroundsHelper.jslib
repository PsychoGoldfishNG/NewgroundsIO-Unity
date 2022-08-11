mergeInto(LibraryManager.library, {

	OnMedalUnlocked: function(medal_id) {
		window.top.postMessage(JSON.stringify({ngioComponent:"Medal.unlock",id:medal_id}), "*");
	},

	OnScorePosted: function(board_id) {
		window.top.postMessage(JSON.stringify({ngioComponent:"ScoreBoard.postScore",id:board_id}), "*");
	}
});
