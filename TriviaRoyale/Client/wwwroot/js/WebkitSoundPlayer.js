let sounds = []
console.log("WebkitSoundPlayer.js loaded")
if ('webkitAudioContext' in window) {
    console.log("webkitAudioContext found")
    var myAudioContext = new webkitAudioContext();
}

function loadSound(Path) {
    request = new XMLHttpRequest();
    request.open('GET', path, true);
    request.responseType = 'arraybuffer';
    request.addEventListener('load', bufferSound, false);
    request.send();
}

function bufferSound(event) {
    console.log(event)
    var request = event.target;
    var source = myAudioContext.createBufferSource();
    source.buffer = myAudioContext.createBuffer(request.response, false);
    mySource = source;
}

function playSound(sound) {
}