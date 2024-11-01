
public enum IAP_Holder
{
    removeads,
    dia300
}

public enum Status_Holder
{
    ATK, // 공격력
    HP, // HP
    MONEY, // 골드 드랍률
    ITEM, // 아이템 드랍률
    SKILL, // 스킬 쿨타임
    ATK_SPEED, // 공격 속도
    CRITICAL_P, // 크리티컬 퍼센티이지
    CRITICAL_D // 크리티컬 데미지
}
public enum Coin_Type
{
    Gold,
    Dia
}
public enum Quest_Type
{
    Monster,
    Stage,
    Gold,
    Dia,
    Hero,
    Upgrade
}

public enum StatusType
{
    Status,
    Mastery,
    Costume
}

public enum SliderType
{
    Default,
    Boss,
    Dungeon
}
public enum ItemType
{
    ALL,
    Equipment,
    Consumable,
    Other
}
public enum Rarity
{
    Common, // 0
    UnCommon, // 1
    Rare, // 2
    Hero, // 3
    Legendary // 4
}

public enum Stage_State
{
    Ready,
    Play,
    Boss,
    Boss_Play,
    Clear,
    Dead,
    Dungeon,
    DungeonClear,
    DungeonFail
}

public enum Level_Data
{
    ATK,
    HP,
    EXP,
    MAX_EXP,
    MONEY
}

public enum Stage_Data
{
    ATK,
    HP,
    MONEY
}