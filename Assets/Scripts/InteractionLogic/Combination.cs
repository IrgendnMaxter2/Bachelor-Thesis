using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CombinationRecipe",
    menuName = "Inventory/Combination Recipe")]
public sealed class CombinationRecipe : ScriptableObject
{
    [SerializeField] private string[] ingredientIds;
    [SerializeField] private string resultId;
    [SerializeField] private string resultName;
    [SerializeField] private string resultDescription;
    [SerializeField] private Sprite resultIcon;

    public string[] IngredientIds => ingredientIds;
    public string ResultId => resultId;
    public string ResultName => resultName;
    public string ResultDescription => resultDescription;
    public Sprite ResultIcon => resultIcon;

    public bool Matches(IReadOnlyList<InventoryEntry> entries)
    {
        if (entries == null || entries.Count != ingredientIds.Length)
            return false;

        var selectedIds = entries.Select(entry => entry.Id).ToArray();

        return selectedIds.OrderBy(id => id)
            .SequenceEqual(ingredientIds.OrderBy(id => id));
    }
}

[System.Serializable]
public sealed class CombinationHint
{
    [SerializeField] private string firstIngredientId;
    [SerializeField] private string secondIngredientId;

    [TextArea]
    [SerializeField] private string message;

    public string Message => message;

    public bool Matches(IReadOnlyList<InventoryEntry> entries)
    {
        if (entries == null || entries.Count != 2)
            return false;

        return
            (entries[0].Id == firstIngredientId &&
             entries[1].Id == secondIngredientId) ||
            (entries[0].Id == secondIngredientId &&
             entries[1].Id == firstIngredientId);
    }
}