using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe
{

    protected Dictionary<string, int> ingredients;
    protected Item product;

    public CraftingRecipe(Dictionary<string, int> ingredients, Item product)
    {
        this.ingredients = ingredients;
        this.product = product;
    }

    public Item getProduct()
    {
        return product;
    }

    public Dictionary<string, int> getIngredients()
    {
        return ingredients;
    }
}
