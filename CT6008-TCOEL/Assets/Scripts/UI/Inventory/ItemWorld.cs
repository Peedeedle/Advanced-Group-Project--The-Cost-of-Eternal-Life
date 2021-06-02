using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
	public static ItemWorld SpawnItemWorld(Vector3 position, Items item) {
		Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
		ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
		itemWorld.SetItem(item);
		return itemWorld;
	}

	private Items item;
	private SpriteRenderer spriteRenderer;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	public void SetItem(Items item) {
		this.item = item;
		spriteRenderer.sprite = item.GetSprite();
	}

	public Items GetItem() {
		return item;
	}

	public void DestroySelf() {
		Destroy(gameObject);
	}

	public static ItemWorld DropItem(Vector3 dropPosition, Items item) {
		ItemWorld itemWorld = SpawnItemWorld(dropPosition, item);
		//itemWorld.GetComponent<Rigidbody2D>().AddForce(dropPosition * -0.05f, ForceMode2D.Impulse);
		return itemWorld;
	}
}
