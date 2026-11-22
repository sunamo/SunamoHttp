namespace SunamoHttp.Interfaces;

/// <summary>
/// Represents the i progress bar http type.
/// </summary>
public interface IProgressBarHttp
{
    /// <summary>
    /// Gets or sets the is registered.
    /// </summary>
    bool IsRegistered { get; set; }

    /// <summary>
    /// Gets or sets the write only divisible by.
    /// </summary>
    int WriteOnlyDivisibleBy { get; set; }

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="percentCalculator">The percent calculator.</param>
    /// <returns>The init result.</returns>
    void Init(IPercentCalculatorHttp percentCalculator);

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="percentCalculator">The percent calculator.</param>
    /// <returns>The init result.</returns>
    void Init(IPercentCalculatorHttp percentCalculator, bool isNotUnitTest);

    /// <summary>
    /// Done one.
    /// </summary>
    /// <param name="percentCalculator">The percent calculator.</param>
    /// <returns>The done one result.</returns>
    void DoneOne(object asyncResult);

    /// <summary>
    /// Done one.
    /// </summary>
    /// <param name="percentCalculator">The percent calculator.</param>
    void DoneOne();

    /// <summary>
    /// Done one.
    /// </summary>
    /// <param name="percentCalculator">The percent calculator.</param>
    /// <param name="isNotUnitTest">The is not unit test.</param>
    void DoneOne(int currentIndex);

    /// <summary>
    /// Start.
    /// </summary>
    /// <param name="asyncResult">The async result.</param>
    void Start(int totalCount);

    /// <summary>
    /// Done.
    /// </summary>
    void Done();
}
