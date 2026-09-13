namespace Content.Shared.Ashfall.Recorder;

public readonly record struct RecorderEntry(
    TimeSpan Timestamp,
    string SpeakerName,
    string SpeechVerb,
    string Text,
    string FontId,
    int FontSize,
    bool Bold
);
