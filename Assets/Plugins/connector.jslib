mergeInto(LibraryManager.library, {
	SendTextToBrowser: function (text) {
		navigator.clipboard.writeText(UTF8ToString(text));
		//need some alert for player to know it's been copied. maybe text update
	}
});