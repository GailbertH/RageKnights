using System;

//Tracks combats result
public class CombatTracker
{
    public string currentStage;

    public int killCount;
    public int deathCount;
    public long goldEarned;
    public int stageCompleted;
    public TimeSpan completionTime;

    //TODO should be server time.
    private DateTime timeStart;
    private DateTime timeEnd;

    public CombatTracker(string curStage)
    {
        currentStage = curStage;
        timeStart = DateTime.Now; 
    }

    public void UpdateKillCount()
    {
        killCount++;
    }

    public void UpdateDeathCount()
    {
        deathCount++;
    }

    public void UpdateGoldEarned(long earnings)
    {
        goldEarned += earnings;
    }

    public void UpdateStageCompleteCount()
    {
        stageCompleted++;
    }

    public void SetCompletionTime()
    {
        timeEnd = DateTime.Now;
        completionTime = timeStart - timeEnd;
    }
}
