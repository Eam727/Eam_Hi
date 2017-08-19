using UnityEngine;
using System.Collections;

public enum InfoType {
	lv,			//角色等级
	expcur,		//角色经验
	force,		//力量
	intellect,	//智力
	attackSpeed,//攻击速度
	maxHP,		//血量
	maxMP,		//魔法
	damageMax,	//物理攻击
	gold,		//金币
	diamond,	//钻石
	Equip,		//装备
	power,		//攻击力
	All
}


public class CharacterTemplate : MonoSingleton<CharacterTemplate> {

	public string name;		//名称
	public int jobId;		//职业id
	public int accountId;	//账号id
	public int characterId;	//角色id
	public int lv = 1;		//等级
	public int expCur = 1;	//经验
	public int force;		//力量
	public int intellect;	//智力
	public int attackSpeed = 5;		//攻击速度
	public int maxHp;		//血量
	public int maxMp;		//魔法
	public int damageMax;	//攻击
	public string jobModel;	//模型
	public int gold;		//金币
	public int diamond;		//钻石

	//public GoodsItem weaponGoodsItem;
	//public GoodsItem clothesGoodsItem;
	//public GoodsItem pantsGoodsItem;
	//public GoodsItem bracersGoodsItem;
	//public GoodsItem necklaceGoodsItem;
	//public GoodsItem shoesGoodsItem;

	public float power;	//力量加成

	public delegate void OnCharacterInfoChangedEvent( InfoType type );
	public event OnCharacterInfoChangedEvent OnCharacterInfoChanged;


	/// <summary>
	/// 穿上装备
	/// </summary>
	//public void Puton(GoodsItem gi)
	//{
	//	gi.IsDressed = true;	
	//	//首先检测有没有穿上相同类型的装备
	//	bool isDressed = false;
	//	GoodsItem goodsItemDressed = null;
	//	switch(gi.Goods.EquipTYPE)
	//	{
	//	case EquipType.Weapon:
	//		if(weaponGoodsItem != null)
	//		{
	//			isDressed = true;
	//			goodsItemDressed = weaponGoodsItem;
	//		}
	//		weaponGoodsItem = gi;	//穿上装备
	//		break;
	//	case EquipType.Bracers:
	//		if(bracersGoodsItem != null)
	//		{
	//			isDressed = true;
	//			goodsItemDressed = bracersGoodsItem;
	//		}
	//		bracersGoodsItem = gi;
	//		break;
	//	case EquipType.Clothes:
	//		if(clothesGoodsItem != null)
	//		{
	//			isDressed = true;
	//			goodsItemDressed = clothesGoodsItem;
	//		}
	//		clothesGoodsItem = gi;
	//		break;
	//	case EquipType.Necklace:
	//		if(necklaceGoodsItem != null)
	//		{
	//			isDressed = true;
	//			goodsItemDressed = necklaceGoodsItem;
	//		}
	//		necklaceGoodsItem = gi;
	//		break;
	//	case EquipType.Pants:
	//		if(pantsGoodsItem != null)
	//		{
	//			isDressed = true;
	//			goodsItemDressed = pantsGoodsItem;
	//		}
	//		pantsGoodsItem = gi;
	//		break;
	//	case EquipType.Shoes:
	//		if(shoesGoodsItem != null)
	//		{
	//			isDressed = true; 
	//			goodsItemDressed = shoesGoodsItem;
	//		}
	//		shoesGoodsItem = gi;
	//		break;
	//	}
	//	//有
	//	if(isDressed)
	//	{
	//		if(gi.Goods.ID != goodsItemDressed.Goods.ID)//如果原有装备不是现在选择的装备
	//		{
	//			goodsItemDressed.IsDressed = false;		//设置为未装备
	//			OperatingDB.Instance.UpdateEquip(goodsItemDressed);	//将原来的装备在数据库中更新为未穿着状态
	//		}
	//	}

	//	UIBackpack.Instance.UpdateEquipInfo();
	//}
	
	///// <summary>
	///// 脱下装备
	///// </summary>
	//public void Unload(GoodsItem gi)
	//{
	//	gi.IsDressed = false;
	//	switch(gi.Goods.EquipTYPE)
	//	{
	//	case EquipType.Weapon:
	//		if(weaponGoodsItem != null)
	//		{
	//			weaponGoodsItem = null;
	//		}
	//		break;
	//	case EquipType.Bracers:
	//		if(bracersGoodsItem != null)
	//		{
	//			bracersGoodsItem = null;
	//		}
	//		break;
	//	case EquipType.Clothes:
	//		if(clothesGoodsItem != null)
	//		{
	//			clothesGoodsItem = null;
	//		}
	//		break;
	//	case EquipType.Necklace:
	//		if(necklaceGoodsItem != null)
	//		{
	//			necklaceGoodsItem = null;
	//		}
	//		break;
	//	case EquipType.Pants:
	//		if(pantsGoodsItem != null)
	//		{
	//			pantsGoodsItem = null;
	//		}
	//		break;
	//	case EquipType.Shoes:
	//		if(shoesGoodsItem != null)
	//		{
	//			shoesGoodsItem = null;
	//		}
	//		break;
	//	}		
	//	UIBackpack.Instance.UpdateEquipInfo();
	//}
	
	////添加金币
	//public void AddGold(int gold)
	//{
	//	OperatingDB.Instance.AddGold(gold);
	//	OnCharacterInfoChanged(InfoType.gold);
	//}
	
	////物品使用
	//public void GoodsUse(GoodsItem gi, int count)
	//{
	//	//处理物品使用后是否还存在
	//	gi.Count -= count;
	//	OperatingDB.Instance.UpdateDrug(gi.Goods.ID);	//更新数据库药品数量
	//	if(gi.Count <= 0)
	//	{
	//		GoodsManager.instance.goodsItemList.Remove(gi);	//移除物品
	//		OperatingDB.Instance.DeleteGoods(gi.Goods.ID);	//删除数据库背包物品
	//	}
	//}
	
	////获取装备力量加成
	//public int GetPower()
	//{
	//	power = 0;
	//	if(weaponGoodsItem != null)
	//		power +=  weaponGoodsItem.Goods.Damage * (1 + (weaponGoodsItem.Level - 1)/ 10f);
	//	if(clothesGoodsItem != null)
	//		power +=  clothesGoodsItem.Goods.Damage * (1 + (clothesGoodsItem.Level - 1)/ 10f);
	//	if(pantsGoodsItem != null)
	//		power +=  pantsGoodsItem.Goods.Damage * (1 + (pantsGoodsItem.Level - 1)/ 10f);
	//	if(bracersGoodsItem != null)
	//		power +=  bracersGoodsItem.Goods.Damage * (1 + (bracersGoodsItem.Level - 1)/ 10f);
	//	if(necklaceGoodsItem != null)
	//		power +=  necklaceGoodsItem.Goods.Damage * (1 + (necklaceGoodsItem.Level - 1)/ 10f);
	//	if(shoesGoodsItem != null)
	//		power +=  shoesGoodsItem.Goods.Damage * (1 + (shoesGoodsItem.Level - 1)/ 10f);
	//	power += damageMax;
	//	return (int)power;
	//}

	////获取金币
	//public bool GetGolds(int number)
	//{
	//	if(gold >= number)
	//	{
	//		gold -= number;
	//		return true;
	//	}
	//	return false;
	//}

}
