using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// IMPROVE: OMG)
public class InitiativeOrder
{
    public List<Unit> CurrentTurnInitiative { get; private set; }
    public List<Unit> NextTurnInitiative { get; private set; }

    private int currentInitiative;
    private Dictionary<int, List<Unit>> unitsByInitiative;
    private List<int> initiatives;

    public InitiativeOrder(IEnumerable<Unit> units)
    {
        unitsByInitiative = new Dictionary<int, List<Unit>>();
        foreach (var unit in units)
        {
            var initiative = unit.Initiative;
            if (unitsByInitiative.ContainsKey(initiative))
                unitsByInitiative[initiative].Add(unit);
            else
                unitsByInitiative.Add(initiative, new List<Unit>() {unit});
        }

        initiatives = unitsByInitiative.Keys.ToList();
        initiatives.Sort();
        currentInitiative = initiatives.First();
        CurrentTurnInitiative = CalculateTurnInitiative();
        NextTurnInitiative = CalculateTurnInitiative();
    }

    private List<Unit> CalculateTurnInitiative(int startingInitiative = -1)
    {
        var result = new List<Unit>();
        var index = startingInitiative == -1 ? 0 : initiatives.IndexOf(startingInitiative);
        while (index < initiatives.Count)
        {
            result.AddRange(GetUnitsByInitiative(initiatives[index]));
            ++index;
        }

        return result;
    }

    public void MoveToNextInitiative() => currentInitiative = GetNextInitiative();

    private int GetNextInitiative()
    {
        var result = initiatives.IndexOf(currentInitiative);
        result++;
        if (result >= initiatives.Count)
            result = 0;
        return initiatives[result];
    }

    private List<Unit> GetUnitsByInitiative(int initiative)
    {
        if (unitsByInitiative.ContainsKey(initiative) == false)
        {
            Debug.LogError($"Trying to get initiative {initiative} but there are no units with this initiative.");
            return null;
        }

        var result = unitsByInitiative[initiative];
        result.Shuffle();
        return result;
    }
}