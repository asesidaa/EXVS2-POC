namespace ServerOver.Processor.Class.Effect;

public class ClassEffectDeterminer
{
    public uint Determine(uint classId, uint rank)
    {
        if (classId != 4)
        {
            return 0;
        }

        if (rank <= 10)
        {
            return 3;
        }

        if (rank <= 20)
        {
            return 2;
        }

        if (rank <= 30)
        {
            return 1;
        }

        return 0;
    }
}