var LastFM = {
	apiKey : "9058869f118658b6a35c71f71c907f56",
	
	getAlbumArt: function(song,callback) 
	{
		var hasLocalStorage = 'localStorage' in window && window['localStorage'] !== null;
		
		if (hasLocalStorage) {
			var cached = localStorage.getItem(song.Id);
			if (cached) {
				callback(song.Id,cached);
				return;
			}
		}
		
		$.get("http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key="+LastFM.apiKey+"&artist="+encodeURIComponent(song.Artist)+"&album="+encodeURIComponent(song.Album),{}, function(result) {
			var url = "";
			var images = result.getElementsByTagName("image");
			if (images) 
			{
				for (var i=0;i<images.length;++i)
				{
					if (images[i].attributes["size"].value == "medium") 
					{
						url = images[i].textContent;
						break;
					}
				}
			}
			
			if (hasLocalStorage)
			{
				try 
				{
					localStorage.setItem(song.Id,url);
				}
				catch (ex) 
				{
					//quota has probably been exceeded.
					//remove a few entries to make room
					var i = 0;
					while (i < localStorage.length && i < 10){
						localStorage.removeItem(localStorage.key(i++));
					}
					localStorage.setItem(song.Id,url);
				}
			}
			callback(song.Id,url);
        }, "xml");
	}
};