namespace DiceServer;
[Serializable]
public class DiceModel
{
    public int Value { get; set; }
    public DiceModel(){}


    public DiceModel(int value)
    {
        this.Value = value;
    }
}