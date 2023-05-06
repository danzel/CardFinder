using AngleSharp.Html.Dom;
using CardFinder.Solver;
using System.Collections.ObjectModel;

namespace CardFinder;

/// <summary>
/// Holds state for a current solve
/// </summary>
public class SolverContext
{
	public Store[] Stores { get; }
	public CardAmount[] Cards { get; }

	public ReadOnlyDictionary<Store, List<CardDetails>[]>? CardsAtEachStore { get; private set; }

	public Solution? Solution { get; private set; }

	//TODO: Use an API for currency conversion maybe?
	private readonly Dictionary<Currency, decimal> _convertToNzdMultiplier = new() { { Currency.NZD, 1 } };

	public SolverContext(Store[] stores, CardAmount[] cards)
	{
		Stores = stores;
		Cards = cards;
	}

	public async IAsyncEnumerable<SolverStatus> Solve()
	{
		if (CardsAtEachStore == null)
		{
			var cardsAtEachStore = new Dictionary<Store, List<CardDetails>[]>();
			foreach (var store in Stores)
				cardsAtEachStore.Add(store, new List<CardDetails>[Cards.Length]);

			for (var i = 0; i < Cards.Length; i++)
			{
				var card = Cards[i];
				var tasks = new List<Task>();

				yield return new SolverStatus($"Fetching '{card.CardName}'", i / (double)Cards.Length);

				string? failureMessage = null;
				foreach (var store in Stores)
				{
					var c = card.CardName;
					var s = store;
					tasks.Add(Task.Run(async () =>
					{
						try
						{
							var results = await s.Scraper.Scrape(c, CancellationToken.None);
							lock (cardsAtEachStore)
							{
								cardsAtEachStore[s][i] = results
									.Where(r => r.Stock > 0)
									.OrderBy(r => r.Price)
									.ToList();
							}
						}
						catch (Exception ex)
						{
							failureMessage = $"Failed scraping '{c}' from {s.Name}: " + ex.Message;
						}
					}));
				}
				await Task.WhenAll(tasks);

				if (failureMessage != null)
				{
					yield return new SolverStatus(failureMessage, 1);
					yield break;
				}
			}

			CardsAtEachStore = cardsAtEachStore.AsReadOnly();
		}

		//Check if any card doesn't have enough
		List<string>? notEnough = null;
		for (var i = 0; i < Cards.Length; i++)
		{
			var card = Cards[i];
			var sum = CardsAtEachStore.Sum(x => x.Value[i].Sum(y => y.Stock));

			if (sum < card.Amount)
			{
				notEnough ??= new List<string>();
				notEnough.Add($"{card.Amount}, need {card.CardName}, found {sum}");
			}
		}
		if (notEnough != null)
		{
			yield return new SolverStatus("Failed\n" + string.Join("\n", notEnough), 1);
			yield break;
		}

		//Solve based on lowest cost
		yield return new SolverStatus("Solving", 0);
		decimal bestPrice = decimal.MaxValue;
		Solution? bestSolution = null;
		Store[]? bestCombo = null;

		//For each combination of stores
		foreach (var combo in Helpers.Combinations(Stores))
		{
			//TODO: Shipping breakpoints (For now assume none and use shipping from all available stores)
			decimal total = combo.Sum(c => _convertToNzdMultiplier[c.ShippingCostCurrency] * c.ShippingCost);

			//CardName -> where to buy it and how much
			var thisPurchase = new Dictionary<string, (Store store, CardDetails Card, int Amount)[]>();

			bool couldntFindEnough = false;

			for (int i = 0; i < Cards.Length; i++)
			{
				var card = Cards[i];

				//Find the n lowest prices
				var bestToBuy = combo
					.SelectMany(store => CardsAtEachStore[store][i].SelectMany(card => Enumerable.Range(0, card.Stock).Select(_ => (store, card))))
					.OrderBy(x => x.card.Price * x.store.GstMultiplier * _convertToNzdMultiplier[x.card.Currency])
					.Take(card.Amount).ToArray();

				if (bestToBuy.Length < card.Amount)
				{
					couldntFindEnough = true;
					break;
				}

				total += bestToBuy.Sum(x => x.card.Price * x.store.GstMultiplier * _convertToNzdMultiplier[x.card.Currency]);
				thisPurchase[card.CardName] = bestToBuy.GroupBy(x => (x.store, x.card)).Select(x => (x.Key.store, x.Key.card, x.Count())).ToArray();
			}

			if (couldntFindEnough)
				continue;

			//If we are better
			//if (bestCombo == null || combo.Length <= bestCombo.Length || (combo.Length == bestCombo.Length && total < bestPrice))
			if (bestCombo == null || total < bestPrice)
			{
				//minStores = combo.Length;
				bestPrice = total;
				bestSolution = new Solution(thisPurchase);
				bestCombo = combo;
			}
		}
		Solution = bestSolution;

		yield return new SolverStatus("Done", 100);
	}
}

public readonly record struct SolverStatus(string Status, double Percent);