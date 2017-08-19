using UnityEngine;

namespace HIEngine.Character
{
    public class CharacterManager : MonoBehaviour
    {

        public GameObject player;
        PlayerStatus ps;
        CharacterSkillManager chSkillManager;
        public GameObject[] players;

        //void Awake()
        //{
        //    if (GameMain.Instance.isOnline)
        //        player = Instantiate(Resources.Load("Character/" + players[CharacterTemplate.Instance.jobId - 1].name),
        //                         transform.position, transform.rotation) as GameObject;
        //    else
        //        player = Instantiate(Resources.Load(CharacterTemplate.Instance.jobModel),
        //                             transform.position, transform.rotation) as GameObject;
        //    player.transform.parent = transform;
        //    ps = GetComponent<PlayerStatus>();
        //    InitCharacter();
        //    InitSkill(CharacterTemplate.Instance.jobId);
        //}

        //void InitCharacter()
        //{
        //    ps.level = CharacterTemplate.Instance.lv;
        //    ps.exp = CharacterTemplate.Instance.expCur;
        //    ps.force = CharacterTemplate.Instance.force;
        //    ps.intellect = CharacterTemplate.Instance.intellect;
        //    ps.attackSpeed = CharacterTemplate.Instance.attackSpeed;
        //    ps.HP = ps.MaxHP = CharacterTemplate.Instance.maxHp;
        //    ps.SP = ps.MaxSP = CharacterTemplate.Instance.maxMp;
        //    //		ps.damage = CharacterTemplate.Instance.damageMax + (int)CharacterTemplate.Instance.power;
        //    ps.damage = (int)CharacterTemplate.Instance.power;

        //}

        //void InitSkill(int jobId)
        //{
        //    GetComponent<CharacterSkillManager>().skills.Clear();
        //    OperatingDB.Instance.CreateDataBase();
        //    //		SqliteDataReader skill = OperatingDB.Instance.db.Select("T_Skill" + jobId,"ID",">=","0");
        //    SqliteDataReader skill = OperatingDB.Instance.db.ReadFullTable("T_Skill" + jobId);
        //    while (skill.Read())
        //    {
        //        int i = 1;
        //        SkillData sd = new SkillData();
        //        Type t = typeof(SkillData);
        //        //			FieldInfo[] myfields = t.GetFields();
        //        foreach (var item in t.GetProperties()) //GetProperties获取属性
        //        {
        //            if (item.PropertyType.Equals(typeof(string)))
        //                item.SetValue(sd, skill[i].ToString(), null);
        //            else if (item.PropertyType.Equals(typeof(float)))
        //                item.SetValue(sd, float.Parse(skill[i].ToString()), null);
        //            else if (item.PropertyType.Equals(typeof(string[])))
        //            {
        //                string[] str = skill[i].ToString().Split(',');
        //                item.SetValue(sd, str, null);
        //            }
        //            else
        //            {
        //                item.SetValue(sd, int.Parse(skill[i].ToString()), null);
        //            }
        //            i++;
        //        }

        //        GetComponent<CharacterSkillManager>().skills.Add(sd);
        //    }
        //    OperatingDB.Instance.db.CloseSqlConnection();
        //}

    }
}

