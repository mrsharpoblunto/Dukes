var DukesService = {
    search: function(artist, album, title, callback) {
        if (title != "") {
            $.get("DukesService/GetSongsByTitle?title=" + title,{}, function(result) {
                callback(result);
            },"json");
        }
        else {
            $.get("DukesService/GetSongsByArtistAndAlbum?artist=" + artist + "&album=" + album,{}, function(result) {
            callback(result);
            }, "json");
        }
    },
    
    getQueue: function(callback) {
        $.get("DukesService/GetQueue",{}, function(result) {
            callback(result);
        }, "json");
    },
    
    getMyQueue: function(callback) {
        $.get("DukesService/GetMyQueue", {}, function(result) {
            callback(result);
        }, "json");
    },
    
    getPlayerState: function(callback) {
        $.get("DukesService/GetPlayerState", {}, function(result) {
            callback(result);
        }, "json");
    },
        
    getCurrentSong: function(callback) {
        $.get("DukesService/GetCurrentSong", {}, function(result) {
            callback(result);
        }, "json");
    },

    addToMyQueue: function(idList,callback) {
        var idString = idList[0];
        for (var i = 1; i < idList.length; i++) {
            idString += "," + idList[i];
        }

        $.post("DukesService/AddToMyQueue", {ids: idString},function(result) {
            callback(result);
        }, "json");
    },
    
    addSongToMyQueue: function(id,callback) {
        var ids = new Array();
        ids[ids.length] = id;
        this.addToMyQueue(ids,callback);      
	},

    clearMyQueue: function(callback) {
        $.get("DukesService/ClearMyQueue",{}, function(result) {
            callback(result);
        }, "json");
    },

    removeFromMyQueue: function(id, callback) {
        $.get("DukesService/RemoveFromMyQueue?id=" + id,{}, function(result) {
            callback(result);
        }, "json");
    },

    setPlayerState: function(state,callback) {
        $.get("DukesService/SetPlayerState?state=" + state,{}, function(result) {
            callback(result);
        }, "json");
    },

    getUser: function(callback) {
        $.get("DukesService/GetUser", {}, function(result) {
            callback(result);
        }, "json");
    }
}

