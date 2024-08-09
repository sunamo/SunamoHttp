namespace SunamoHttp.Interfaces;

public interface IProgressBarHttp
{
    bool isRegistered { get; set; }
    int writeOnlyDividableBy { get; set; }
    void Init(IPercentCalculatorHttp pc);
    void Init(IPercentCalculatorHttp pc, bool isNotUt);


    void LyricsHelper_AnotherSong(object asyncResult);
    void LyricsHelper_AnotherSong();
    void LyricsHelper_AnotherSong(int i);
    void LyricsHelper_OverallSongs(int obj);
    void LyricsHelper_WriteProgressBarEnd();
}