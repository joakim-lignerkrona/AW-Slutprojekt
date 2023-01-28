let sounds = []
var myAudioContext
let isUsingWebkit = false
console.log("WebkitSoundPlayer.js loaded")
window.addEventListener("load", () => {
    console.log("window.onload")
    if ('webkitAudioContext' in window) {
        console.log("webkitAudioContext found")
        myAudioContext = new webkitAudioContext();
        isUsingWebkit = true
    }
})
function loadSound(Path) {
    if (isUsingWebkit) {

        request = new XMLHttpRequest();
        request.open('GET', Path, true);
        request.responseType = 'arraybuffer';
        request.addEventListener('load', event => bufferSound(event, Path), false);
        request.send();
    }
}

function bufferSound(event, Path) {
    console.log(event)
    var request = event.target;
    var source = myAudioContext.createBufferSource();
    source.buffer = myAudioContext.createBuffer(request.response, false);
    sounds.push({ source, Path });
}

function playSound(sound) {
    if (isUsingWebkit) {

        var play = sounds.find(s => s.Path == sound);
        if (play && myAudioContext) {
            play.connect(myAudioContext)
            play.source.noteOn(0);
        }
    }
}