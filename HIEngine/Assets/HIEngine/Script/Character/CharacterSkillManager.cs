using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace HIEngine.Character
{
    /// <summary>
    /// 技能管理类
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour
    {
        ///// <summary>管理所有技能的容器</summary>
        //public List<SkillData> skills = new List<SkillData>();
        ///// <summary>技能的拥有者</summary>
        //private CharacterStatus chStatus = null;

        ////初始化技能数据(有什么技能)
        //public void Start()
        //{
        //    chStatus = GetComponent<CharacterStatus>();

        //    foreach (var item in skills)
        //    {
        //        //动态加载技能特效预制体  //Resources/Skill -- 技能特效预制体 
        //        if (item.skillPrefab == null && !string.IsNullOrEmpty(item.prefabName))
        //            item.skillPrefab = LoadFxPrefab("Skill/" + item.prefabName);
        //        //Resources/Skill/HitFx     技能伤害特效预制体
        //        if (item.hitFxPrefab == null && !string.IsNullOrEmpty(item.hitFxName))
        //            item.hitFxPrefab = LoadFxPrefab("Skill/HitFx/" + item.hitFxName);
        //    }
        //}

        ////将特效预制件载入到对象池，以备将来使用
        //private GameObject LoadFxPrefab(string path)
        //{
        //    var key = path.Substring(path.LastIndexOf("/") + 1);
        //    var go = Resources.Load<GameObject>(path);
        //    GameObjectPool.instance.MyDestory(
        //        GameObjectPool.instance.CreateObject(
        //        key, go, transform.position, transform.rotation)
        //        );
        //    return go;
        //}

        ////准备技能
        //public SkillData PrepareSkill(int id)
        //{
        //    //从技能容器中找出相应ID的技能
        //    var skillData = skills.Find(p => p.skillID == id);
        //    if (skillData != null &&//查找到技能
        //        chStatus.SP >= skillData.costSP && //检查角色SP是否够使用该技能
        //        skillData.coolRemain == 0) //且该技能已经冷却结束
        //    {
        //        skillData.Owner = gameObject;
        //        return skillData;
        //    }
        //    return null;
        //}

        ////释放技能
        //public void DeploySkill(SkillData skillData)
        //{
        //    //创建技能预制体
        //    var tempGo = GameObjectPool.instance.CreateObject(skillData.prefabName, skillData.skillPrefab, transform.position, transform.rotation);
        //    //从预制体对象上找到技能释放对象 
        //    var deployer = tempGo.GetComponent<SkillDeployer>();
        //    //设置要释放的技能
        //    deployer.skillData = skillData;
        //    //调用释放方法
        //    deployer.DeploySkill();

        //    //技能持续时间过后，技能要销毁
        //    if (skillData.durationTime > 1)
        //        GameObjectPool.instance.MyDestory(tempGo, skillData.durationTime);
        //    else
        //        GameObjectPool.instance.MyDestory(tempGo, 0.5f);

        //    //开始冷却计时
        //    StartCoroutine(CoolTimeDown(skillData));
        //}

        ////冷却时间倒计时
        //public IEnumerator CoolTimeDown(SkillData skillData)
        //{
        //    skillData.coolRemain = skillData.coolTime;
        //    while (skillData.coolRemain > 0)
        //    {
        //        yield return new WaitForSeconds(1);
        //        skillData.coolRemain -= 1;
        //    }
        //    skillData.coolRemain = 0;
        //}

        ////取得冷却倒计时的剩余时间(秒)
        //public int GetSkillCoolRemain(int id)
        //{
        //    return skills.Find(p => p.skillID == id).coolRemain;
        //}

    }
}
