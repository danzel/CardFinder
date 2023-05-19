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
				Console.WriteLine($"{step}");

				switch (step.State)
				{
					case SolverState.Scraping:
						_solverStatus = "Scraping " + _solverContext.Cards[step.CardIndex!.Value].CardName;
						_solvedPercent =
							((double)step.StoresSearchedForCurrentCard!.Value / _solverContext.Stores.Length / _solverContext.Cards.Length)
							+ (double)step.CardIndex!.Value / _solverContext.Cards.Length;
						break;
					case SolverState.Solving:
						_solverStatus = "Solving";
						break;
					case SolverState.Complete:
						_solverStatus = null;
						_solvedPercent = 1;
						break;
					case SolverState.Error:
						_solverStatus = step.ErrorDetails;
						_solvedPercent = null;
						break;
					default:
						throw new NotImplementedException(step.State.ToString());
				}
				StateHasChanged();
			}
		}
		finally
        {
            _isWorking = false;
        }
	}
}