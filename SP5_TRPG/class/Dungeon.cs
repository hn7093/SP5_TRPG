using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP5_TRPG
{
    class Dungeon
    {
        public string name { get; set; }
        public int reqDefense { get; set; }
        public int reward { get; set; }
        public Dungeon(string name, int reqDefense, int reward)
        {
            this.name = name;
            this.reqDefense = reqDefense;
            this.reward = reward;
        }
        public bool EnterDungeon(Player player)
        {
            Random random = new Random();
            if (reqDefense > player.Defense)
            {
                int fail = random.Next(11); // 1 ~ 10
                // 40퍼센트로 실패
                if (fail <= 4)
                {
                    player.TakeDamage(player.Health / 2);
                    return false;
                }
            }
            // 성공
            // 방어도에 따른 체력 감소
            int lost = random.Next(20, 36); // 20 ~ 35
            lost = lost - (player.Defense - reqDefense);
            lost = Math.Max(lost, 0); // 0 미만 방지

            player.TakeDamage(lost);
            // 공격력에 따른 보상 증가
            int bonus = random.Next(player.Attack, player.Attack * 2 + 1);
            int gold = reward + (int)(reward * bonus / 100.0);
            player.Gold += gold;

            // 클리어
            player.ClearDungeon();
            return true;

        }
    }
}
