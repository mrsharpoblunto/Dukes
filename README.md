Dukes
=====

The Dukebox of Hazzard (Dukes) is a media player built around the idea of fair and equal access – in particular those situations where there is only one set of speakers in a communal space, and multiple people who want to be able to play their music. Dukes is designed to prevent people cutting other peoples tracks off partway through (by design there’s no way to skip tracks), and ensures that all users get an equal chance to hear their music by running through a round-robin of all the users who have submitted tracks for playing. Users can access the web UI from whereever they are, using whatever device they have, be it a phone, tablet, or PC.

Dukes consists of a C# application that indexes media on the host machine as well as running a standalone HTTP server that runs the players web UI. All the indexed music metadata is stored in a small SQLite database, the music player uses the FMOD audio library, and the web UI communicates with the Dukes server using a set of JSON API’s.

##Building Dukes
To build Dukes, you will need Visual Studio 2012, simply open Dukes.sln and build! There is also an NSIS installer included, to build this you will need to compile the installer script at Installer/installer.nsi using NSIS (http://nsis.sourceforge.net/Download).

##Running Dukes
To run Dukes, you will need to run DukesServer.exe which will open a UI for configuring the server, from here you will want to add users and media sources and ensure the webUI is listening on the desired port. After some initial users and media sources have been defined, you can actually start queueing up music to be played by connecting to the webUI using a web browser (DukesServer.exe hosts the webUI, so if you close this then the server will also close)

