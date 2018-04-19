namespace MiniRpgGame.Domain
{
    public interface IStockroom
    {
        (int Cost, Weapon Weapons) TakeWeapons();
        (int Cost, Armor Armor) TakeArmor();
    }
}