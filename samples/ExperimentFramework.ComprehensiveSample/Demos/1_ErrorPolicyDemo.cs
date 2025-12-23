using ExperimentFramework.ComprehensiveSample.Services.ErrorPolicy;

namespace ExperimentFramework.ComprehensiveSample.Demos;

/// <summary>
/// Demonstrates all 5 error policies: Throw, RedirectAndReplayDefault, RedirectAndReplayAny, RedirectAndReplay, RedirectAndReplayOrdered
/// </summary>
public class ErrorPolicyDemo(
    IThrowPolicyService throwService,
    IRedirectDefaultService redirectDefaultService,
    IRedirectAnyService redirectAnyService,
    IRedirectSpecificService redirectSpecificService,
    IRedirectOrderedService redirectOrderedService
)
{
    public async Task RunAsync()
    {
        Console.WriteLine("\n" + new string('=', 80));
        Console.WriteLine("DEMO 1: ERROR POLICIES");
        Console.WriteLine(new string('=', 80));

        // Demo 1: OnErrorThrow - Exception propagates immediately
        Console.WriteLine("\n1.1 OnErrorThrow Policy (fails fast):");
        Console.WriteLine("  - If selected trial throws, exception propagates immediately");
        Console.WriteLine("  - No fallback attempts");
        try
        {
            await throwService.ProcessAsync();
            Console.WriteLine("  ✅ SUCCESS: Trial executed without errors");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  ❌ EXCEPTION CAUGHT: {ex.Message}");
            Console.WriteLine("  → This is expected behavior for OnErrorThrow");
        }

        // Demo 2: OnErrorRedirectAndReplayDefault - Falls back to default trial
        Console.WriteLine("\n1.2 OnErrorRedirectAndReplayDefault Policy:");
        Console.WriteLine("  - If selected trial throws, falls back to default trial");
        Console.WriteLine("  - Tries: [preferred, default]");
        try
        {
            var result = await redirectDefaultService.ProcessAsync();
            Console.WriteLine($"  ✅ SUCCESS: {result}");
            Console.WriteLine("  → Experiment succeeded (either preferred trial worked, or fell back to default)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ EXCEPTION: {ex.Message}");
        }

        // Demo 3: OnErrorRedirectAndReplayAny - Tries all trials until one succeeds
        Console.WriteLine("\n1.3 OnErrorRedirectAndReplayAny Policy:");
        Console.WriteLine("  - If selected trial throws, tries all other trials");
        Console.WriteLine("  - Tries all variants until one succeeds");
        Console.WriteLine("  - Order: [preferred, then all others]");
        try
        {
            var result = await redirectAnyService.ProcessAsync();
            Console.WriteLine($"  ✅ SUCCESS: {result}");
            Console.WriteLine("  → Experiment succeeded after trying trials");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ EXCEPTION: {ex.Message}");
            Console.WriteLine("  → All trials failed");
        }

        // Demo 4: OnErrorRedirectAndReplay - Redirects to a specific fallback trial
        Console.WriteLine("\n1.4 OnErrorRedirectAndReplay Policy:");
        Console.WriteLine("  - If selected trial throws, redirects to a specific fallback trial");
        Console.WriteLine("  - Useful for dedicated diagnostics/Noop handlers");
        Console.WriteLine("  - Order: [preferred, specific_fallback]");
        try
        {
            var result = await redirectSpecificService.ProcessAsync();
            Console.WriteLine($"  ✅ SUCCESS: {result}");
            Console.WriteLine("  → Experiment redirected to specific fallback handler");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ EXCEPTION: {ex.Message}");
            Console.WriteLine("  → Both preferred and fallback trials failed");
        }

        // Demo 5: OnErrorRedirectAndReplayOrdered - Tries ordered list of fallback trials
        Console.WriteLine("\n1.5 OnErrorRedirectAndReplayOrdered Policy:");
        Console.WriteLine("  - If selected trial throws, tries ordered fallback trials");
        Console.WriteLine("  - Fine-grained control over fallback order");
        Console.WriteLine("  - Order: [preferred, fallback1, fallback2, fallback3, ...]");
        try
        {
            var result = await redirectOrderedService.ProcessAsync();
            Console.WriteLine($"  ✅ SUCCESS: {result}");
            Console.WriteLine("  → Experiment succeeded with ordered fallback strategy");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ EXCEPTION: {ex.Message}");
            Console.WriteLine("  → All fallback trials failed");
        }
    }
}
