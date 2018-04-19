namespace MiniRpgGame.Domain
{
    public interface IStockroom
    {
        (int Cost, Weapons Weapons) TakeWeapons();
        (int Cost, Armor Armor) TakeArmor();
    }
}