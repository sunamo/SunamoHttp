namespace SunamoHttp.Interfaces;

/// <summary>
/// Interface for progress bar functionality in HTTP operations
/// </summary>
public interface IProgressBarHttp
{
    /// <summary>
    /// Gets or sets a value indicating whether the progress bar is registered
    /// </summary>
    bool IsRegistered { get; set; }

    /// <summary>
    /// Gets or sets the value indicating progress should only be written when divisible by this number
    /// </summary>
    int WriteOnlyDivisibleBy { get; set; }

    /// <summary>
    /// Initializes the progress bar with a percent calculator
    /// </summary>
    /// <param name="percentCalculator">The percent calculator</param>
    void Init(IPercentCalculatorHttp percentCalculator);

    /// <summary>
    /// Initializes the progress bar with a percent calculator
    /// </summary>
    /// <param name="percentCalculator">The percent calculator</param>
    /// <param name="isNotUnitTest">Indicates whether this is not a unit test context</param>
    void Init(IPercentCalculatorHttp percentCalculator, bool isNotUnitTest);

    /// <summary>
    /// Marks one item as done
    /// </summary>
    /// <param name="asyncResult">The async result object</param>
    void DoneOne(object asyncResult);

    /// <summary>
    /// Marks one item as done
    /// </summary>
    void DoneOne();

    /// <summary>
    /// Marks one item as done
    /// </summary>
    /// <param name="currentIndex">The current index of the completed item</param>
    void DoneOne(int currentIndex);

    /// <summary>
    /// Starts the progress tracking
    /// </summary>
    /// <param name="totalCount">The total count of items to process</param>
    void Start(int totalCount);

    /// <summary>
    /// Marks all processing as done
    /// </summary>
    void Done();
}