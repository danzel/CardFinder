using CardFinder.BlazorApp.Helpers;
using CardFinder.Solver;
using Microsoft.AspNetCore.Components;

namespace CardFinder.BlazorApp.Pages.FindCards;

public partial class Index
{
	[Inject]
	private StoreFactory _storeFactory { get; set; } = null!;

    private string _cardsText = "";
    
	private bool _isWorking = false;
    private SolverContext? _solverContext = null;
	private string? _solverStatus;
	private double? _solvedPercent;

    async Task PerformSearch()
    {
        try
        {
            _isWorking = true;

            var cards = InputHelper.ParseCardsText(_cardsText);
            _solverContext = new SolverContext(_storeFactory.Stores, cards);

			await foreach (var step in _solverContext.Solve())
			{
				Console.WriteLine($"{step.Status} {step.Percent}");
				_solverStatus = step.Status;
				_solvedPercent = step.Percent;
				StateHasChanged();
			}
        }
        finally
        {
            _isWorking = false;
        }
    }
}