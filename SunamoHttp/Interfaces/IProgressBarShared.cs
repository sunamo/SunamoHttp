namespace SunamoHttp.Interfaces;

public interface IProgressBarHttp
{
    bool isRegistered { get; set; }
    int writeOnlyDividableBy { get; set; }
    void Init(IPercentCalculatorHttp pc);
    void Init(IPercentCalculatorHttp pc, bool isNotUt);


    void DoneOne(object asyncResult);
    void DoneOne();
    void DoneOne(int i);
    void Start(int obj);
    void Done();
}