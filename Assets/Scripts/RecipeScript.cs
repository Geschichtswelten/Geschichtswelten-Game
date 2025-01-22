using UnityEngine;
using QFSW.QC;
public class RecipeScript : MonoBehaviour
{
    public  BlueprintDatabase database;
    public  ItemDataBaseList itemList;
    [Command("ShowRecipe", "Let's you see all the recipes that include the item you parsed.")]
    public string ShowRecipe(string item_Name)
    {
        var item = itemList.getItemByName(item_Name);
        if (item == null) return "Item not found\n";
        string returnString = "";
        var blueprints = database.blueprints;
        for (int i = 0; i < blueprints.Count; i++)
        {
            if (blueprints[i].ingredients.Contains(item.itemID) || blueprints[i].finalItem == item)
            {
                string ingredientString = "";
                for (int j = 0; j < blueprints[i].ingredients.Count; j++)
                {
                    ingredientString += itemList.getItemByID(blueprints[i].ingredients[j]).itemName+"["+blueprints[i].amount[j]+"]\n";
                }
                returnString += "\n-----------------------\nProduced item[amount]: " +
                                blueprints[i].finalItem.itemName + "[" + blueprints[i].amountOfFinalItem + "]\n\n" +
                                "Ingredients[amount]: " + ingredientString;
            }
        }

        if (returnString != "")
        {
            return returnString;
        }

        return "Found no recipes for this item\n";
    }
    
    
    [Command("ShowAllRecipes", "Let's you see all the recipes.")]
    public string ShowAllRecipes()
    {
        string returnString = "";
        var blueprints = database.blueprints;
        for (int i = 0; i < blueprints.Count; i++)
        {
            
                string ingredientString = "";
                for (int j = 0; j < blueprints[i].ingredients.Count; j++)
                {
                    ingredientString += itemList.getItemByID(blueprints[i].ingredients[j]).itemName+"["+blueprints[i].amount[j]+"]\n";
                }
                returnString += "\n-----------------------\nProduced item[amount]: " +
                                blueprints[i].finalItem.itemName + "[" + blueprints[i].amountOfFinalItem + "]\n\n" +
                                "Ingredients[amount]: " + ingredientString;
            
        }

        if (returnString != "")
        {
            return returnString;
        }

        return "Found no recipes for this item\n";
    }
}
