const NatCorderWebGL={$sharedInstance:{recordingCallback:null,framebuffer:null,framebufferContext:null,pixelBuffer:null,audioContext:null,audioStream:null,recorder:null,MIME_TYPE:"video/webm"},NCStartRecording:function(e,n,a,r,t,o,s,c,d,u){if(1!==a)return console.log("NatCorder Error: NatCorder WebGL only supports VP8/WebM container"),0;sharedInstance.recordingCallback=n,sharedInstance.framebuffer=document.createElement("canvas"),sharedInstance.framebuffer.width=r,sharedInstance.framebuffer.height=t,sharedInstance.framebufferContext=sharedInstance.framebuffer.getContext("2d"),sharedInstance.pixelBuffer=sharedInstance.framebufferContext.getImageData(0,0,r,t);const f=[sharedInstance.framebuffer.captureStream(o).getVideoTracks()[0]];d>0&&(sharedInstance.audioContext=new AudioContext({latencyHint:"interactive",sampleRate:d}),sharedInstance.audioStream=sharedInstance.audioContext.createMediaStreamDestination({channelCount:u,channelCountMode:"explicit"}),f.push(sharedInstance.audioStream.stream.getAudioTracks()[0]));const l={mimeType:sharedInstance.MIME_TYPE,videoBitsPerSecond:s};return sharedInstance.recorder=new MediaRecorder(new MediaStream(f),l),sharedInstance.recorder.start(),console.log("NatCorder: Starting recording with resolution",r+"x"+t),1},NCStopRecording:function(e){console.log("NatCorder: Stopping recording"),sharedInstance.recorder.ondataavailable=function(e){const n=new Blob([e.data],{type:sharedInstance.MIME_TYPE}),a=URL.createObjectURL(n);console.log("NatCorder: Completed recording video",n,"to URL:",a);const r=lengthBytesUTF8(a)+1,t=_malloc(r);stringToUTF8(a,t,r),Runtime.dynCall("vi",sharedInstance.recordingCallback,[t]),_free(t)},sharedInstance.recorder.stop(),sharedInstance.audioContext&&sharedInstance.audioContext.close(),sharedInstance.recorder=null,sharedInstance.framebuffer=null,sharedInstance.framebufferContext=null,sharedInstance.pixelBuffer=null,sharedInstance.audioContext=null},NCEncodeFrame:function(e,n,a,r){for(var t=sharedInstance.pixelBuffer.width,o=sharedInstance.pixelBuffer.height,s=4*t,c=0;c<o;c++)sharedInstance.pixelBuffer.data.set(new Uint8Array(HEAPU8.buffer,n+(o-c-1)*s,s),c*s);sharedInstance.framebufferContext.putImageData(sharedInstance.pixelBuffer,0,0)},NCEncodeSamples:function(e,n,a,r){const t=sharedInstance.audioContext.createBuffer(sharedInstance.channelCount,a/sharedInstance.channelCount,sharedInstance.sampleRate);n=new Float32Array(HEAPU8.buffer,n,a);for(var o=0;o<t.numberOfChannels;o++){const e=t.getChannelData(o);for(var s=0;s<t.length;s++)e[s]=n[s*t.numberOfChannels+o]}var c=sharedInstance.audioContext.createBufferSource();c.buffer=t,c.connect(sharedInstance.audioStream),c.start()},NCCurrentTimestamp:function(){return Math.round(1e6*performance.now())}};autoAddDeps(NatCorderWebGL,"$sharedInstance"),mergeInto(LibraryManager.library,NatCorderWebGL);