var panelVideo;
var playerInstance;
var CurrentMedia = {};
var NextMedia = {};

window.onload = OnLoad;
function OnLoad()
{
    panelVideo = document.getElementById("panelVideo");
    //panelVideo.style.position = 'absolute';
    //panelVideo.style.left = "0px";
    //panelVideo.style.top = "0px";
    //panelVideo.width = window.innerWidth;
    //panelVideo.height = window.innerHeight;

    playerInstance = jwplayer("myElement");
    playerInstance.key = "DSIshXd/x70UrpMj1rWk4/6M/NVPXnp4C1/29Q==";

    CallFirstMedia();
}

function MediaOnPlayer(source, start)
{
    var src = "//www.youtube.com/watch?v=" + source;

    playerInstance.setup({
        file: src,
        autostart: "true",
        //width: panelVideo.width,
        //height: panelVideo.height,
        controls: "false",
    });

    playerInstance.on("firstFrame", function () { playerInstance.seek(start); });
    playerInstance.on("time", OnTimeMedia);

    playerInstance.on('pause', function () { playerInstance.play(); });

}

function OnTimeMedia() {
    var position = playerInstance.getPosition() - CurrentMedia.StartSeconds;
    var duration = CurrentMedia.Duration;

    if (position >= duration)
    {
        CurrentMedia = NextMedia;
        MediaOnPlayer(CurrentMedia.Src, CurrentMedia.StartSeconds);
        CallNextMedia();
    }
}

function CallNextMedia() {
    $.ajax({
        type: "POST",
        url: "/WebTV/NextMedia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            NextMedia = JSON.parse(response);
        },
        failure: function (response) { },
        error: function (response) { }
    });
}
function CallFirstMedia() {
    $.ajax({
        type: "POST",
        url: "/WebTV/FirstMedia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var obj = JSON.parse(response);
            CurrentMedia = obj.Current;
            NextMedia = obj.Next;
            MediaOnPlayer(CurrentMedia.Src, CurrentMedia.StartSeconds);
        },
        failure: function (response) { },
        error: function (response) { }
    });
}

