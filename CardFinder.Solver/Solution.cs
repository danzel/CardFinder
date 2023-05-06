namespace CardFinder.Solver;

public record class Solution(Dictionary<string, (Store Store, CardDetails Card, int Amount)[]> Cards);
