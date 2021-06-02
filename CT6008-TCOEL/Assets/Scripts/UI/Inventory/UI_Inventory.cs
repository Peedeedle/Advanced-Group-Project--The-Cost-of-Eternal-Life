using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_Inventory : MonoBehaviour
{
	private Inventory inventory;
	private Transform itemSlotContainer;
	private Transform itemSlotTemplate;

	private PlayerController player;

	private void Awake() {
		itemSlotContainer = transform.Find("ItemSlotContainer");
		itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
	}

	public void SetPlayer(PlayerController player) {
		this.player = player;
	}

	public void SetInventory(Inventory inventory) {
		this.inventory = inventory;

		inventory.OnItemsListChanged += Inventory_OnItemListChanged;
		RefreshInventoryItems();
	}

	private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
		RefreshInventoryItems();
	}

	private void RefreshInventoryItems() {
		foreach(Transform child in itemSlotContainer) {
			if(child == itemSlotTemplate) {
				continue;
			} else {
				Destroy(child.gameObject);
			}
		}

		int x = 0;
		int y = 0;
		float itemSlotCellSize = 150f;

		foreach (Items item in inventory.GetItemList()) {
			RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
			itemSlotRectTransform.gameObject.SetActive(true);

			itemSlotRectTransform.GetComponent<ClickableObject>().ClickFunc = () => {
				inventory.UseItem(item);
			};

			itemSlotRectTransform.GetComponent<ClickableObject>().MouseRightClickFunc = () => {
				Items duplicateItem = new Items { itemType = item.itemType, iItemAmmount = item.iItemAmmount };
				inventory.RemoveItem(item);
				ItemWorld.DropItem(new Vector3(player.transform.position.x + 2,
									player.transform.position.y,
									player.transform.position.z),
									duplicateItem);
			};

			
			
			itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
			Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
			image.sprite = item.GetSprite();

			TextMeshProUGUI uiText = itemSlotRectTransform.Find("itemAmmountText").GetComponent<TextMeshProUGUI>();
			if (item.iItemAmmount > 1) {
				uiText.SetText(item.iItemAmmount.ToString());
			} else {
				uiText.SetText("");
			}

			TextMeshProUGUI uiItemName = itemSlotRectTransform.Find("itemName").GetComponent<TextMeshProUGUI>();
			switch (item.itemType) {
				default:
				case Items.ItemType.SMG:
					uiItemName.SetText("SMG");
					break;
				case Items.ItemType.Shotgun:
					uiItemName.SetText("Shotgun");
					break;
				case Items.ItemType.Health:
					uiItemName.SetText("Health Potion");
					break;
			}

			x++;
			if(x > 3) {
				x = 0;
				y--;
			}
		}
	}

	public void DropItem() {
		foreach (Items item in inventory.GetItemList()) {
			Items duplicateItem = new Items { itemType = item.itemType, iItemAmmount = item.iItemAmmount };
			inventory.RemoveItem(item);
			ItemWorld.DropItem(new Vector3 (player.transform.position.x + 2,
								player.transform.position.y,
								player.transform.position.z),
								duplicateItem);
		}
	}
}
