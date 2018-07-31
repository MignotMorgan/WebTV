var panelVideo;
var playerInstance;

var Channel = "";
var CurrentMedia = {};
var NextMedia = {};

window.onload = OnLoad;
function OnLoad()
{
    SetChannel();
    panelVideo = document.getElementById("panelVideo");
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
        width: panelVideo.style.width,
        controls: "false"
    });

    playerInstance.on("firstFrame", function () { playerInstance.seek(start); });
    playerInstance.on("time", OnTimeMedia);
}

function OnTimeMedia() {
    var position = playerInstance.getPosition() - CurrentMedia.StartSeconds;
    var duration = CurrentMedia.Duration;

    var lbl = document.getElementById("mylabel");
    lbl.innerHTML = position + "/" + duration;

    if (position >= duration)
    {
        CurrentMedia = NextMedia;
        MediaOnPlayer(CurrentMedia.Src, CurrentMedia.StartSeconds);
        CallNextMedia();
    }
}

function CallNextMedia() {
    var stringData = "{channel : " + Channel + " }";
    $.ajax({
        type: "POST",
        url: "/WebTV/NextMedia",
        data: stringData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            NextMedia = JSON.parse(response);
            var pnl = document.getElementById("panelHeading");
            pnl.innerHTML = Channel + " : " + CurrentMedia.Name + " : " + NextMedia.Name;
        },
        failure: function (response) {
            //alert("failure : CallNextMedia");
            //alert(response.responseText);
        },
        error: function (response) {
            //alert("error : CallNextMedia");
            //alert(response.responseText);
        }
    });
}
function CallFirstMedia() {
    $.ajax({
        type: "POST",
        url: "/WebTV/FirstMedia?channel="+Channel,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var obj = JSON.parse(response);
            CurrentMedia = obj.Current;
            NextMedia = obj.Next;
            MediaOnPlayer(CurrentMedia.Src, CurrentMedia.StartSeconds);

            var pnl = document.getElementById("panelHeading");
            pnl.innerHTML = Channel + " : " + CurrentMedia.Name + " : " + NextMedia.Name;
        },
        failure: function (response) {
            alert("failure : CallFirstMedia");
            //alert(response.responseText);
        },
        error: function (response) {
            alert("error : CallFirstMedia");
            //alert(response.responseText);
        }
    });
}

