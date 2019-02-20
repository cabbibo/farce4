## NatMic 1.2.0
+ NatMic now supports multi-channel microphone audio. On platforms that don't support this, NatMic will automatically interleave the mono data across the number of requested channels.
+ NatMic now features a device enumeration API. You can now select which audio input device to record from. See the `Device` class.
+ Added ability to specify adaptive echo cancellation (AEC) on recording device.
+ Added `RealtimeClip` class for converting microphone audio to a Unity `AudioClip` in realtime.
+ Added a formal constructor for `Format` struct.
+ Changed `SampleBufferCallback` to take in only the sample buffer and timestamp. The callback will now only be called with new sample buffers, and not on initialize or finalize events.
+ Fixed crash when app is suspended after recording is stopped on iOS.
+ Deprecated `AudioEvent` enumeration.
+ Deprecated experimental WebGL backend as it is too unstable. We might reintroduce it at a later time.
+ Refactored `Format.DefaultWithMixing` to `Format.Unity`.

## NatMic 1.1f1
+ Added experimental support for WebGL! Check out the README for more information.
+ Added `ClipRecorder` for recording microphone audio to an `AudioClip`.
+ Moved `callback` parameter in `WAVRecorder` from `StartRecording` function to constructor.
+ Deprecated `RecordingCallback`.

## NatMic 1.0f1
+ First release