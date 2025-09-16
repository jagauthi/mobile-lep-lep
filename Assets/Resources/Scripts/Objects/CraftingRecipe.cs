using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe {

    protected List<Item> ingredients;
    protected Item product;

    public CraftingRecipe(List<Item> ingredients, Item product) {
        this.ingredients = ingredients;
        this.product = product;
    }
}
