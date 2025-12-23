namespace ExperimentFramework.Models;
/// <summary>
/// Describes the fallback behavior when the selected trial throws an exception.
/// </summary>
public enum OnErrorPolicy
{
    /// <summary>
    /// Rethrow the exception from the selected trial without attempting any fallback.
    /// </summary>
    Throw,

    /// <summary>
    /// If the selected trial fails and is not the default trial, re-invoke once using the default trial.
    /// </summary>
    RedirectAndReplayDefault,

    /// <summary>
    /// If the selected trial fails, attempt other configured trials (including default) until one succeeds.
    /// </summary>
    RedirectAndReplayAny,

    /// <summary>
    /// If the selected trial fails, redirect to a specific single trial.
    /// </summary>
    RedirectAndReplay,

    /// <summary>
    /// If the selected trial fails, attempt an ordered list of fallback trials until one succeeds.
    /// </summary>
    RedirectAndReplayOrdered
}
