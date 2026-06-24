namespace SunamoHttp.Interfaces;

public interface IProgressBarHttp
{
    bool IsRegistered { get; set; }

    int WriteOnlyDivisibleBy { get; set; }

    void Init(IPercentCalculatorHttp percentCalculator);

    void Init(IPercentCalculatorHttp percentCalculator, bool isNotUnitTest);

    void DoneOne(object asyncResult);

    void DoneOne();

    void DoneOne(int currentIndex);

    void Start(int totalCount);

    void Done();
}
