using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace SP5_TRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string userName;
            Job userJob;
            Player player;
            // start Setting
            // name Setting
            #region NameSetting
            while (true)
            {
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.\n");
                Console.Write("당신의 이름을 입력해주세요 : ");

                userName = Console.ReadLine();

                Console.WriteLine(userName + " 이십니까?\n");
                if (YesOrNo())
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.Clear();
                }
            }
            #endregion

            // Job Setting
            #region JobSetting
            Console.WriteLine("안녕하세요. " +userName + "님\n");
            Console.WriteLine("직업을 선택해주세요\n");
            Console.WriteLine("1) 전사");
            Console.WriteLine("2) 도적\n");

            userJob = (Job)GetNumber(1, 2);
            Console.WriteLine(GetEnumDescription(userJob) + " (으)로 시작합니다.");
            Thread.Sleep(500);
            Console.Clear();
            player = new Player(userName, userJob);
            #endregion


            // 게임 씬
            while (true)
            {
                // 마을
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 휴식하기");
                int action = GetNumber(1, 5);
                switch (action)
                {
                    case 1:
                        ShowPlayerInfo(player);
                        break;
                    case 2:
                        ShowPlayerInventory(player);
                        break;
                    case 3:
                        ShowStore(player);
                        break;
                    case 4:
                        ShowDungeon(player);
                        break;
                    case 5:
                        ShowRest(player);
                        break;
                }
            }
        }
        static void ClearLine()
        {
            // 한줄 지우기
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, top);
        }
        static bool YesOrNo()
        {            
            Console.WriteLine("1) 네");
            Console.WriteLine("2) 아니오\n");
            while (true)
            {
                Console.Write("원하시는 행동을 입력해주세요 : ");
                char answer = Console.ReadKey(true).KeyChar;
                if (char.IsDigit(answer))
                {
                    if (answer == '1')
                    {
                        return true;
                    }
                    else if (answer == '2')
                    {
                        return false;
                    }
                }                
                else
                {
                    int left = Console.CursorLeft;
                    int top = Console.CursorTop;
                    // 한줄 지우기
                    ClearLine();
                    Console.Write("잘못 된 입력입니다.");
                    Thread.Sleep(500);
                }
            }
        }
        static int GetNumber(int start, int end)
        {
            while (true)
            {
                Console.Write("\n원하시는 행동을 입력해주세요 : ");
                char answer = Console.ReadKey(true).KeyChar;
                if (char.IsDigit(answer) && answer >= (char)(start + '0') && answer <= (char)(end + '0'))
                {
                    Console.Write("\n");
                    return answer - '0';
                }
                else
                {
                    int left = Console.CursorLeft;
                    int top = Console.CursorTop;
                    // 한줄 지우기
                    ClearLine();
                    Console.Write("잘못 된 입력입니다.");
                    Thread.Sleep(500);
                    ClearLine();
                }
            }
        }
        static void ShowPlayerInfo(Player player)
        {
            int equipAttack = player.GetEquipStatus(ItemStatus.Attack);
            int equipDefense = player.GetEquipStatus(ItemStatus.Defense);
            int equipHealth = player.GetEquipStatus(ItemStatus.Health);
            // 플레이어 상태 보기
            Console.Clear();
            Console.WriteLine("==== 상태 보기 ====");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine("Lv. " + player.Level.ToString("00"));
            Console.WriteLine($"{player.Name} ({GetEnumDescription(player.job)})");

            if(equipAttack != 0)
                Console.WriteLine($"공격력 : {player.Attack}  ({equipAttack:+#;-#})");
            else
                Console.WriteLine($"공격력 : {player.Attack} ");


            if (equipDefense != 0)
                Console.WriteLine($"방어력 : {player.Defense}  ({equipDefense:+#;-#})");
            else
                Console.WriteLine($"방어력 : {player.Defense} ");


            if (equipHealth != 0)
                Console.WriteLine($"방어력 : {player.Health}  ({equipHealth:+#;-#})");
            else
                Console.WriteLine($"체력 : {player.Health}");
            Console.WriteLine($"Gold : {player.Gold} G");

            Console.WriteLine("\n0. 나가기\n");
            if(GetNumber(0,0) == 0)
                Console.Clear();

        }
        public static string GetEnumDescription(Enum status)
        {
            var fieldInfo = status.GetType().GetField(status.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
            return attribute?.Description ?? status.ToString();
        }
        static void ShowPlayerInventory(Player player)
        {
            while (true)
            {
                // 인벤토리 보기
                Console.Clear();
                Console.WriteLine("==== 상태 보기 ====");
                Console.WriteLine("보유 중인 아이템을 관리 할 수 있습니다\n");
                Console.WriteLine("[아이템 목록]\n");
                ShowInventory(player);
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기\n");
                int action = GetNumber(0, 1);
                switch (action)
                {
                    case 0:
                        Console.Clear();
                        return;
                    case 1:
                        // 인벤토리 - 장착 관리
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("==== 인벤토리 - 장착 관리 ====");
                            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                            Console.WriteLine("[아이템 목록]\n");
                            // 리스트 출력
                            ShowInventory(player, true);
                            Console.Write("\n");
                            if (player.items.Count > 1)
                                Console.WriteLine($"1~{player.items.Count}. 아이템 장착/해제");
                            Console.WriteLine("0. 나가기\n");
                            // 입력 받기
                            int idx = GetNumber(0, player.items.Count);
                            if (idx == 0)
                            {
                                break;
                            }
                            else
                            {
                                // 장착 시도
                                idx--;//아이템은 0부터 시작
                                player.Equip(player.items[idx]);
                                Thread.Sleep(300);
                            }
                        }
                        break;
                }
            }
        }
        static void ShowStore(Player player)
        {
            Item[] items = {
                new Item("저주받은 종이 갑옷", ItemStatus.Defense, -3, 0, "오히려 약해지는 저주 받은 갑옷 ."),
                new Item("수련자의 갑옷", ItemStatus.Defense, 5, 1000, "수련에 도움을 주는 갑옷입니다."),
                new Item("무쇠 갑옷", ItemStatus.Defense, 9, 2000, "무쇠로 만들어져 튼튼한 갑옷입니다."),
                new Item("스파르타의 갑옷", ItemStatus.Defense, 15, 3500, "스파르타의 전사들이 사용했다는 갑옷입니다."),
                new Item("낡은 검", ItemStatus.Attack, 2, 600, "쉽게 볼 수 있는 낡은 검입니다."),
                new Item("청동 도끼", ItemStatus.Attack, 5, 1500, "어디선가 사용됐던거 같은 도끼입니다."),
                new Item("스파르타의 창", ItemStatus.Attack ,7, 4000, "스파르타의 전사들이 사용했다는 전설의 창입니다.")
            };
            // 상점 보기
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== 상점 ====");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]\n");
                Console.WriteLine($"{player.Gold} G\n");

                Console.WriteLine("[아이템 목록]\n");
                ShowItmeList(items, player);
                
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기\n");
                int action = GetNumber(0, 2);
                switch (action)
                {
                    case 0:
                        // 나가기
                        Console.Clear();
                        return;
                    case 1:
                        // 상점 - 아이템 구매
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("==== 상점 -  아이템 구매 ====");
                            Console.WriteLine("필요한 아이템을 구매 할 수 있습니다.\n");
                            Console.WriteLine("[보유 골드]\n");
                            Console.WriteLine($"{player.Gold} G\n");
                            Console.WriteLine("[아이템 목록]\n");
                            // 리스트 출력
                            ShowItmeList(items, player, true);
                            Console.WriteLine($"\n1~{items.Length}. 아이템 구매");
                            Console.WriteLine("0. 나가기\n");
                            // 입력 받기
                            int idx = GetNumber(0, items.Length);
                            if (idx == 0)
                            {
                                break;
                            }
                            else
                            {
                                // 구매 시도
                                idx --;//아이템은 0부터 시작
                                int res = player.Exchange(items[idx].Gold, items[idx]);
                                ClearLine();
                                if (res == 0)
                                {
                                    // 정상 구입
                                    Console.WriteLine("================================================");
                                    Console.WriteLine("\t구매를 완료 했습니다!!");
                                    Console.WriteLine("================================================");
                                }
                                else if (res == -1)
                                {
                                    // 돈 부족
                                    Console.WriteLine("================================================");
                                    Console.WriteLine("\tGOLD가 부족합니다!!");
                                    Console.WriteLine("================================================");
                                }
                                else if (res == -2)
                                {
                                    // 중복
                                    Console.WriteLine("================================================");
                                    Console.WriteLine("\t이미 구매한 아이템입니다!!");
                                    Console.WriteLine("================================================");
                                }
                                Thread.Sleep(1000);
                            }
                        }
                        break;
                    case 2:
                        // 상점 - 아이템 판매
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("==== 상점 -  아이템 판매 ====");
                            Console.WriteLine("소지하고 있는 아이템을 판매 할 수 있습니다.\n");
                            Console.WriteLine("[보유 골드]\n");
                            Console.WriteLine($"{player.Gold} G\n");
                            Console.WriteLine("[아이템 목록]\n");
                            // 리스트 출력
                            ShowInventory(player, true, true);
                            if(player.items.Count > 0)
                                Console.WriteLine($"\n1~{player.items.Count}. 아이템 판매");
                            Console.WriteLine("0. 나가기\n");
                            // 입력 받기
                            int idx = GetNumber(0, player.items.Count);
                            if (idx == 0)
                            {
                                break;
                            }
                            else
                            {
                                // 구매 시도
                                idx--;//아이템은 0부터 시작
                                int res = player.Sale(player.items[idx]);
                                ClearLine();
                                if (res == 0)
                                {
                                    Console.WriteLine("================================================");
                                    Console.WriteLine("\t판매완료했습니다.");
                                    Console.WriteLine("================================================");
                                }
                                Thread.Sleep(1000);
                            }
                        }
                        break;
                }
            }
        }
        static void ShowRest(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== 휴식하기 ====");
                Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 :{player.Gold} G)\n");
                Console.WriteLine($"[현재 체력 : {player.Health} /  {player.MaxHealth}]\n");

                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n");
                int action = GetNumber(0, 1);
                switch (action)
                {
                    case 0:
                        // 나가기
                        Console.Clear();
                        return;
                    case 1:
                        // 휴식하기
                        int res = player.Rest(500, 100);
                        ClearLine();
                        if (res == 0)
                        {
                            // 정상 종료
                            Console.WriteLine("================================================");
                            Console.WriteLine("\t휴식을 완료했습니다");
                            Console.WriteLine("================================================");
                            Thread.Sleep(1000);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("================================================");
                            Console.WriteLine("\tGold 가 부족합니다.");
                            Console.WriteLine("================================================");
                            Thread.Sleep(1000);
                            break;
                        }
                }
            }
        }
        static void ShowDungeon(Player player)
        {
            Dungeon[] dungeons = {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500),
            };
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== 던전 입장 ====");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine($"[현재 방어력 : {player.Defense}]\n");

                // 던전 리스트
                for (int i = 0; i < dungeons.Length; i++)
                { 
                    Console.WriteLine($"{i+1}. {dungeons[i].name}\t| 방어력 {dungeons[i].reqDefense} 이상 권장");
                }
                Console.WriteLine("\n0. 나가기\n");
                int action = GetNumber(0, dungeons.Length);
                if (action == 0)
                {
                    Console.Clear();
                    return;
                }
                // 진행
                Console.Clear();
                Console.WriteLine("던전 진행 중...\n");
                int startHealth = player.Health;
                int startGold = player.Gold;
                action--;
                bool clear = dungeons[action].EnterDungeon(player);

                // 빈 진행바 채우기
                for (int i = 0; i < 10; i++)
                {
                    Console.Write('□');
                }
                int fill = clear ? 10 : 6;
                int top = Console.CursorTop;
                Console.SetCursorPosition(0, top);
                for (int i = 0; i < fill; i++)
                {
                    Console.Write('■');
                    Thread.Sleep(300);
                }
                if (clear)
                    Console.WriteLine("\n성공!!");
                else
                    Console.WriteLine("\n실패!");
                Thread.Sleep(500);
                Console.Clear();

                if (clear)
                {
                    // 탐색 성공
                    Console.WriteLine("축하합니다!!");
                    Console.WriteLine($"{dungeons[action].name}을 클리어 하였습니다.\n");
                    
                }
                else
                {
                    // 탐색 실패
                    Console.WriteLine("탐색 실패!!");
                    Console.WriteLine($"{dungeons[action].name} 탐색에 실패하였습니다.");
                }
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {startHealth} -> {player.Health}");
                Console.WriteLine($"Gold {startGold} -> {player.Gold}\n");
                Console.WriteLine("0. 나가기");
                action = GetNumber(0, 0);
                if (action == 0)
                {
                    Console.Clear();
                    return;
                }
            }
        }
        // 리스트 보기 - 상점
        static void ShowItmeList(Item[] items, Player player, bool showIdx = false)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Console.Write("- ");

                // 인덱스 표시
                if(showIdx)
                    Console.Write(i+1);

                Console.Write($" {items[i].Name}  |  {GetEnumDescription(items[i].Status)} {items[i].Value:+#;-#}  |  {items[i].Descript}  |  ");
                // 이미 가지고 있다면 구매 완료 표시 or 가격 표기
                if (player.items != null && player.items.Any(item => item.Name == items[i].Name))
                {
                    Console.WriteLine("구매 완료");
                }
                else
                    Console.WriteLine($"{items[i].Gold} G");
            }
        }
        // 리스트 보기 - 인벤토리
        static void ShowInventory(Player player, bool showIdx = false, bool forSale = false)
        {
            Item[] items = player.items.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                Console.Write("- ");
                // 인덱스 표시
                if (showIdx)
                    Console.Write(i + 1);
                // 장비 여부 표기
                if (player.equips != null && player.equips.Any(item => item.Name == items[i].Name))
                {
                    Console.Write("[E]");
                }
                Console.Write($" {items[i].Name}  |  {items[i].Status} {items[i].Value:+#;-#}  |  {items[i].Descript}");
                if (forSale)
                {
                    int sale = items[i].Gold * 85 / 100;
                    Console.Write($"{sale}G");
                }
                Console.Write("\n");
            }
        }
    }
    enum Job
    {
        [Description("전사")]
        Warrior = 1,
        [Description("도적")]
        Thief
    }
    enum ItemStatus
    {
        [Description("공격력")]
        Attack,

        [Description("체력")]
        Health,

        [Description("방어력")]
        Defense
    }
    interface ICharacter
    {
        string Name { get; set; }
        int Level { get; set; }
        int Health { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }
        bool isDead { get; set; }
        int Gold { get; set; }
        bool TakeDamage(int damage);
    }
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
                    res+= equips[i].Value;
            }
            return res;
        }
    }
    class Item
    {
        public string Name { get; set; }
        public ItemStatus Status { get; set; }
        public int Value { get; set; }
        public int Gold { get; set; }
        public string Descript { get; set; }
        public Item(string name, ItemStatus status, int value, int gold, string descript)
        {
            Name = name;
            Status = status;
            Value = value;
            Gold = gold;
            Descript = descript;
        }
    }
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
            if (reqDefense > player.Defense ) 
            {
                int fail = random.Next(11); // 1 ~ 10
                // 40퍼센트로 실패
                if (fail <= 4)
                {
                    player.Health /= 2;
                    return false;
                }
            }
            // 성공
            // 방어도에 따른 체력 감소
            int lost = random.Next(20, 36); // 20 ~ 35
            lost = lost - (player.Defense - reqDefense);
            lost = Math.Max(lost, 0); // 0 미만 방지
                
            player.Health -= lost;
            // 공격력에 따른 보상 증가
            int bonus = random.Next(player.Attack, player.Attack * 2 + 1);
            int gold = reward + (int)(reward * bonus / 100.0);
            player.Gold += gold;
            return true;
            
        }
    }
}
