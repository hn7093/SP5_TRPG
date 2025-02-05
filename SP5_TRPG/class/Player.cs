using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP5_TRPG
{
    class Player : ICharacter
    {
        public string Name { get; set; }
        public Job job { get; set; }
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public bool isDead { get; set; }
        public List<Item> equips { get; set; }
        public List<Item> items { get; set; }
        public int reqExp = 1;
        public int nowExp = 0;
        public Player(string name, Job playerJob)
        {
            Name = name;
            job = playerJob;
            Level = 1;
            switch (playerJob)
            {
                case Job.Warrior:
                    Attack = 10;
                    Defense = 5;
                    Health = 100;
                    MaxHealth = Health;
                    Gold = 1500;
                    break;
                case Job.Thief:
                    Attack = 13;
                    Defense = 3;
                    Health = 90;
                    MaxHealth = Health;
                    Gold = 2500;
                    break;
            }
            items = new List<Item>();
            equips = new List<Item>();
        }

        public bool TakeDamage(int damage)
        {
            if (isDead) return false;
            if (damage <= 0) return false;
            else
            {
                Health -= damage;
                if (Health <= 0)
                    isDead = true;
                return true;
            }
        }

        public void TryAttack(ICharacter target)
        {
            target.TakeDamage(Attack);
        }
        public void Equip(Item item)
        {
            // 장착하고 있는 장비라면 장비목록에서 삭제
            var sameNameItem = equips.FirstOrDefault(equipItem => equipItem.Name == item.Name);
            if (sameNameItem != null)
            {
                Disarm(sameNameItem);
                return;
            }

            // 장착하고 있는 부위라면 장비 해제
            var sameTypeItem = equips.FirstOrDefault(equipItem => equipItem.Status == item.Status);
            if (sameTypeItem != null)
            {
                Disarm(sameTypeItem);
            }

            // 장착 장비 목록에 추가 - 장착
            equips.Add(item);
            // 스텟 변경
            switch (item.Status)
            {
                case ItemStatus.Attack:
                    Attack += item.Value;
                    break;
                case ItemStatus.Defense:
                    Defense += item.Value;
                    break;
                case ItemStatus.Health:
                    MaxHealth += item.Value;
                    Health = Math.Min(MaxHealth, Health);
                    break;
            }
        }
        public void Disarm(Item item)
        {
            equips.Remove(item);
            // 해제 - 스텟 변경
            switch (item.Status)
            {
                case ItemStatus.Attack:
                    Attack -= item.Value;
                    break;
                case ItemStatus.Defense:
                    Defense -= item.Value;
                    break;
                case ItemStatus.Health:
                    Health -= item.Value;
                    break;
            }
        }
        // 아이템 구입
        public int Exchange(int gold, Item item)
        {
            // 소지금 보다 많으면 실패
            if (gold > Gold) return -1;
            else if (items.Any(myItem => myItem.Name == item.Name))
            {
                // 이미 가지고 있는 아이템이라면 실패
                return -2;
            }
            else
            {
                // 교환성공
                Gold -= gold;
                items.Add(item);
                return 0;
            }
        }
        // 아이템 판매
        public int Sale(Item item)
        {
            // 교환성공
            if (equips.Contains(item))
            {
                // 장비 해제
                Disarm(item);
            }
            items.Remove(item);
            int sale = item.Gold * 85 / 100;
            Gold += sale;
            return 0;
        }
        public int Rest(int gold, int maxHeal)
        {
            // 소지금 보다 많으면 실패
            if (gold > Gold) return -1;
            else
            {
                // 회복 성공
                Gold -= gold;
                int health = Health + maxHeal;
                Health = Math.Min(MaxHealth, health);
                return 0;
            }
        }
        public int GetEquipStatus(ItemStatus itemStatus)
        {
            int res = 0;
            for (int i = 0; i < equips.Count; i++)
            {
                if (equips[i].Status == itemStatus)
                    res += equips[i].Value;
            }
            return res;
        }
        // 던전 클리어, 경험치 획득
        public void ClearDungeon()
        {
            nowExp++;
            if (nowExp == reqExp)
                LevelUp();
        }
        private void LevelUp()
        {
            Level++;
            reqExp++;
            nowExp = 0;
            // 스텟 상승 , 방어 1상승, 공격력은 2레벨마다
            Defense++;
            if (Level % 2 != 0)
                Attack++;
        }
    }
}
