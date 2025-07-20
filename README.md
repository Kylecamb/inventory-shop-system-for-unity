# inventory/shop system for unity
add custom spawnable game objects from your game allow spawnng anything from health pickups like hearts for platformers to custom items like wigs viking hats or health potions/weapons ect
....(setup info) 1 Project Structure & Script Placement
Ensure all provided C# scripts (ShopItemSO.cs, CoinCollect.cs, CoinManager.cs, ShopItemUI.cs, ShopManager.cs, InventoryItemUI.cs, InventoryManager.cs, EquipmentManager.cs, PanelTriggerActivator.cs, InventoryPanelActivator.cs, UIPauseManager.cs) are in your Unity project's Assets folder (e.g., in an Assets/Scripts folder).

2. Install Newtonsoft.Json 📦
The inventory system uses JSON for saving/loading.

In Unity, go to Window > Package Manager.

In the Package Manager window, ensure the dropdown menu is set to "Unity Registry".

Search for "Newtonsoft Json" (com.unity.nuget.newtonsoft-json).

Click "Install".

3. Create ScriptableObject Assets
ShopItemSO (ScriptableObject) defines your game items.

In your Project window, right-click and go to Create > Shop > Item.

Create an asset for each item you want in your shop/game (e.g., "HealthPotion", "IronHelmet", "GoldSword").

For each ShopItemSO asset in the Inspector:

Item Name: Display name (e.g., "Health Potion").

Icon: Assign a UI Sprite for the item's icon in the inventory/shop.

Price: Cost in coins.

Item Prefab: Drag the 3D GameObject prefab that represents this item in the game world (e.g., your health potion model, helmet model).

Is Consumable: Check this if the item should be removed from inventory after use (e.g., potions).

Equipment Slot: If the item is equipable (like a helmet or sword), select its intended slot (e.g., Head, RightHand).

Attached Offset / Attached Rotation: If Equipment Slot is not None, adjust these Vector3 values to fine-tune the item's local position and rotation when attached to the player's body part.

4. Scene Setup 🏡
Drag and drop these GameObjects into your scene:

A. CoinManager (GameObject: "CoinManager")
Create an Empty GameObject in your scene (GameObject > Create Empty), name it "CoinManager".

Drag the CoinManager.cs script onto it.

Assign Coin Text: Create a UI Text element for displaying coins (e.g., a TextMeshPro Text under your main Canvas). Drag this Text component into the Coin Text slot on the "CoinManager" in the Inspector.

B. ShopManager (GameObject: "Shop")
Create an Empty GameObject in your scene, name it "Shop".

Drag the ShopManager.cs script onto it.

Assign Shop Items: Drag all your created ShopItemSO assets from your Project window into the Shop Items list on the "Shop" GameObject.

Assign Shop Item UI Prefab: Create a UI prefab for individual shop items (e.g., a Panel containing an Image, two Text fields, and a Button for buying). Drag this UI prefab into the Shop Item UI Prefab slot.

Assign Shop Item Container: Create an Empty GameObject (e.g., a GridLayoutGroup or VerticalLayoutGroup) under your Canvas. This will hold the generated shop item UI elements. Drag this GameObject into the Shop Item Container slot.

Assign Coin Manager: Drag the "CoinManager" GameObject from your Hierarchy into the Coin Manager slot.

C. InventoryManager (GameObject: "Inventory")
Create an Empty GameObject in your scene, name it "Inventory".

Drag the InventoryManager.cs script onto it.

Assign Inventory Container: Create an Empty GameObject (e.g., a GridLayoutGroup or VerticalLayoutGroup) under your Canvas. This will hold the generated inventory item UI elements. Drag this GameObject into the Inventory Container slot.

Assign Inventory Item UI Prefab: Create a UI prefab for individual inventory items (e.g., a Panel containing an Image and a Button for using). Drag this UI prefab into the Inventory Item UI Prefab slot.

Assign All Available Shop Items: This is CRUCIAL for loading. Drag all your ShopItemSO assets from your Project window into the All Available Shop Items list on the "Inventory" GameObject.

D. EquipmentManager (GameObject: "Equipment")
Create an Empty GameObject in your scene, name it "Equipment". (Alternatively, place this script directly on your Player GameObject if it persists between scenes).

Drag the EquipmentManager.cs script onto it.

Assign Attachment Points: In the Inspector, drag the actual Transform instances (e.g., bones or empty GameObjects) from your Player character's hierarchy into the corresponding slots (Head Point, Right Hand Point, Left Hand Point, Torso Point, etc.). Ensure these are scene objects, not prefab assets.

E. UI Panels & Activators (on Canvas or separate Manager)
Main Canvas: Create a Canvas GameObject (GameObject > UI > Canvas). Set its Render Mode to Screen Space - Overlay or Screen Space - Camera as appropriate.

Shop Panel: Create an Empty GameObject under your Canvas, name it ShopPanel. Design your shop UI inside this panel (buttons, text, the Shop Item Container from above). Initially disable this panel in the Inspector.

Inventory Panel: Create an Empty GameObject under your Canvas, name it InventoryPanel. Design your inventory UI inside this panel (the Inventory Container from above, an "Exit" button). Initially disable this panel in the Inspector.

UIManager (GameObject: "UIManager")

Create an Empty GameObject in your scene, name it "UIManager".

Drag the UIPauseManager.cs script onto it. This handles game pausing and cursor visibility.

Drag the InventoryPanelActivator.cs script onto it.

Assign Inventory Panel: Drag the InventoryPanel GameObject from your Canvas into the Inventory Panel slot on the InventoryPanelActivator script.

Shop Trigger (GameObject: e.g., "ShopTriggerZone")

Create a 3D Primitive (e.g., Cube) for your shop trigger zone. Name it "ShopTriggerZone".

Check "Is Trigger" on its Collider component.

Remove or disable its Mesh Renderer if you don't want it visible.

Drag the PanelTriggerActivator.cs script onto it.

Assign Panel To Activate: Drag your ShopPanel GameObject from your Canvas into the Panel To Activate slot.

5. Player Setup 🚶‍♀️
Tag Your Player: Select your Player character GameObject. In the Inspector, set its Tag to "Player". (If "Player" tag doesn't exist, create it via the Tag dropdown > Add Tag...).

Player Controller: Ensure your Player character has a script that controls its movement and has a Collider component. If you need PlayerController.Instance for other parts of your game, make sure your player script has a static Instance property (as discussed in previous steps).

Rigidbody: If your player has a Rigidbody, ensure its settings are appropriate to avoid being "flung" by spawned items (e.g., Is Kinematic if using a CharacterController, or appropriate Mass and Drag if relying on physics).

6. Button Wiring (UI) 🖱️
A. Inventory Panel "Exit" Button
On your InventoryPanel on the Canvas, add a UI Button (e.g., "Exit Inventory").

Select this button. In the Inspector, for its On Click() event:

Add a new event (+).

Drag the "UIManager" GameObject (which has UIPauseManager) into the "None (Object)" slot.

From the function dropdown, select UIPauseManager > ClosePanel(GameObject).

Drag your InventoryPanel GameObject into the new GameObject slot.

B. Shop Panel "Exit" Button
On your ShopPanel on the Canvas, add a UI Button (e.g., "Exit Shop").

Select this button. In the Inspector, for its On Click() event:

Add a new event (+).

Drag the "UIManager" GameObject (which has UIPauseManager) into the "None (Object)" slot.

From the function dropdown, select UIPauseManager > ClosePanel(GameObject).

Drag your ShopPanel GameObject into the new GameObject slot.

How It Works 💡
Shop: Player enters the ShopTriggerZone, activating the ShopPanel and pausing the game. Players can click items to buy them. CoinManager handles currency, InventoryManager receives purchased items. "Exit" button closes the shop.

Inventory: Pressing the 'I' key toggles the InventoryPanel, pausing/unpausing the game. Items in inventory can be "used" (either consumed or equipped). "Exit" button also closes the inventory.

Equipping: Items marked with an Equipment Slot will be instantiated as children of the corresponding bone on the player via the EquipmentManager. Only one item can be equipped per slot; clicking again toggles it off, or replaces it if a different item for that slot is clicked.

Consumables: Items marked Is Consumable are removed from inventory after being used.

Persistence: Coins and Inventory are saved to PlayerPrefs and loaded automatically when the game starts. Because PlayerPrefs is Unity's way of storing data persistently on the user's device (like a small database), any value saved with PlayerPrefs.SetInt() will remain saved even when:

You load a new scene.

You close the game and reopen it later.

This means your players' coin balances will always be carried over, providing a seamless and persistent economic system in your game.