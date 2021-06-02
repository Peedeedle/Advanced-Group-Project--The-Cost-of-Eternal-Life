using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	public event EventHandler OnItemsListChanged;

	public List<Items> itemList;
	public List<Items> PlayerItemsList;

	private GameObject player;
	private PlayerController playerController;
	private PlayerData playerData;

	private Action<Items> useItemAction;

	public Inventory(Action<Items> useItemAction) {
		this.useItemAction = useItemAction;
		itemList = new List<Items>();
		//itemList = playerData.InventoryList;
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		//AddItem(new Items { itemType = Items.ItemType.Health, iItemAmmount = playerController.iHealAmmount });
		//Debug.Log(itemList.Count);
	}

	public void AddItem(Items item) {
		if (item.IsStackable()) {
			bool bItemAlreadyInInventory = false;
			foreach(Items inventoryItem in itemList) {
				if(inventoryItem.itemType == item.itemType) {
					inventoryItem.iItemAmmount += item.iItemAmmount;
					bItemAlreadyInInventory = true;
				}
			}
			if (!bItemAlreadyInInventory) {
				itemList.Add(item);
			}
		} else {
			itemList.Add(item);
		}
		
		OnItemsListChanged?.Invoke(this, EventArgs.Empty);
	}

	public List<Items> GetItemList() {
		return itemList;
	}

	public void UseItem(Items item) {
		useItemAction(item);
	}

	public void RemoveItem(Items item) {
		if(item.itemType == Items.ItemType.SMG) {
			playerController.DropWeapon();
		} else if (item.itemType == Items.ItemType.Shotgun) {
				playerController.DropWeapon();
			}

		if (item.IsStackable()) {
			Items itemInInventory = null;
			foreach (Items inventoryItem in itemList) {
				if (inventoryItem.itemType == item.itemType) {
					inventoryItem.iItemAmmount -= item.iItemAmmount;
					itemInInventory = inventoryItem;
				}
			}
			if (itemInInventory != null && itemInInventory.iItemAmmount <= 0) {
				itemList.Remove(itemInInventory);
				
			}
		} else {
			itemList.Remove(item);
		}
		OnItemsListChanged?.Invoke(this, EventArgs.Empty);
	}
}
